using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;

namespace BarSimulator
{
    class Bar
    {
        // After each action, we sleep for a tick to simulate the passing of
        // one minute.
        public const int TICK_MILLISECONDS = 1;

        public const int MINIMAL_AGE = 18;
        // The bar opens at 22:00 and closes at 5:00
        private const int OPENS_HOURS = 22;
        private const int CLOSES_HOURS = 5;

        BarState barState;
        DateTime time;
        Dictionary<Drink, int> drinkStorage = new Dictionary<Drink, int>();
        Dictionary<Drink, int> drinkSales = new Dictionary<Drink, int>();
        List<Student> students = new List<Student>();
        Semaphore semaphore = new Semaphore(10, 10);
        Random random;

        public Bar(Drink[] drinks, Random random)
        {
            this.barState = BarState.NotOpenYet;
            this.Drinks = drinks;
            this.random = random;

            this.LoadDrinks();
        }

        public Drink[] Drinks { get; }

        public bool Enter(Student student)
        {
            int waitCounter = 0;
            while (!semaphore.WaitOne(TICK_MILLISECONDS))
            {
                waitCounter++;
                if (WillPersonLeaveQueue(waitCounter))
                {
                    return false;
                }
            }

            if(student.Age < MINIMAL_AGE)
            {
                // Student gets dragged out by the bouncer just before going in.
                Console.WriteLine($"{student} is not old enough to enter the bar.");

                // Release the handle acquired after waiting in line.
                semaphore.Release();
                return false;
            }

            while (true)
            {
                switch (this.barState)
                {
                    case BarState.NotOpenYet:
                        Console.WriteLine("The bar has not opened yet. Waiting to open...");
                        while (this.barState == BarState.NotOpenYet)
                        {
                            WaitOneTick();
                        }
                        break;
                    case BarState.Open:
                        lock (students)
                        {
                            students.Add(student);
                        }
                        return true;
                    case BarState.Closed:
                        // Release the handle acquired after waiting in line.
                        semaphore.Release();
                        throw new BarClosedException();
                    default:
                        throw new Exception($"Unexpected BarState: {barState}");
                }
            }
        }

        public void Leave(Student student)
        {
            lock (students)
            {
                students.Remove(student);
            }
            semaphore.Release();
        }

        public void WaitOneTick()
        {
            Thread.Sleep(TICK_MILLISECONDS);
        }

        public void Drink(Student student, Drink drink)
        {
            CheckIfBarIsOpen();
            if (this.drinkStorage[drink] <= 0)
            {
                Console.WriteLine($"The bar is out of {drink}s.");
                return;
            }
            if (student.Money < drink.Price)
            {
                Console.WriteLine($"{student} doesn't have enough money to get a {drink}.");
                return;
            }

            this.drinkStorage[drink]--;
            this.drinkSales[drink]++;

            student.Money -= drink.Price;

            Console.WriteLine($"{student} drinks a {drink}.");
        }

        public void Dance(Student student)
        {
            CheckIfBarIsOpen();

            Console.WriteLine($"{student} dances.");
        }

        public void OpenBar()
        {
            this.barState = BarState.Open;

            var now = DateTime.Now;
            this.time = new DateTime(now.Year, now.Month, now.Day, OPENS_HOURS, 0, 0);

            int openForHowManyHours;
            if (OPENS_HOURS > CLOSES_HOURS)
            {
                // Closes on the next day
                openForHowManyHours = 24 - OPENS_HOURS + CLOSES_HOURS;
            }
            else if (OPENS_HOURS < CLOSES_HOURS)
            {
                openForHowManyHours = CLOSES_HOURS - OPENS_HOURS;
            }
            else
            {
                throw new InvalidOperationException("The bar never closes. This is not allowed");
            }

            var closeTime = this.time.AddHours(openForHowManyHours);
            while (this.time < closeTime)
            {
                this.time = this.time.AddMinutes(1);
                WaitOneTick();
            }

            this.barState = BarState.Closed;
        }

        public string GenerateDrinkReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Drink sales:");
            foreach (var kvp in drinkSales)
            {
                sb.AppendLine($"{kvp.Key}: {kvp.Value} units sold");
            }

            sb.AppendLine();
            sb.AppendLine("Units out of stock:");
            var outOfStock = drinkStorage.Where(kvp => kvp.Value == 0);

            if (!outOfStock.Any())
            {
                sb.AppendLine("None");
            }
            else
            {
                foreach (var kvp in outOfStock)
                {
                    sb.AppendLine(kvp.Key.ToString());
                }
            }

            return sb.ToString();
        }

        private void LoadDrinks()
        {
            // Load 10000 units of each available drink type.
            foreach (var drink in this.Drinks)
            {
                this.drinkStorage[drink] = 10000;
                this.drinkSales[drink] = 0;
            }
        }

        private bool WillPersonLeaveQueue(int waitCounter)
        {
            var leaveChance = random.NextDouble();
            // We use a logarithmic function here to simulate real-world
            // patience. For a graph of the function see
            // https://www.desmos.com/calculator/vaci9h8qpl
            return leaveChance <= Math.Log(waitCounter, 2) / 10;
        }

        private void CheckIfBarIsOpen()
        {
            if (this.barState == BarState.Closed)
            {
                throw new BarClosedException();
            }
        }
    }

    public enum BarState
    {
        NotOpenYet, Open, Closed
    }

    public class BarClosedException : Exception
    { }
}
