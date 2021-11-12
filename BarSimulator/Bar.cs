using System;
using System.Collections.Generic;
using System.Threading;

namespace BarSimulator
{
    class Bar
    {
        // After each action, we sleep for a tick to simulate the passing of
        // one minute.
        public const int TICK_MILLISECONDS = 1;

        public const int MINIMAL_AGE = 18;
        Dictionary<Drink, int> drinkStorage = new Dictionary<Drink, int>();
        List<Student> students = new List<Student>();
        Semaphore semaphore = new Semaphore(10, 10);

        public Bar(Drink[] drinks)
        {
            this.Drinks = drinks;

            this.LoadDrinks();
        }

        public Drink[] Drinks { get; }

        public bool Enter(Student student)
        {
            semaphore.WaitOne();
            if(student.Age < MINIMAL_AGE)
            {
                // Student gets dragged out by the bouncer just before going in.
                Console.WriteLine($"{student} is not old enough to enter the bar.");

                // Release the handle acquired after waiting in line.
                semaphore.Release();
                return false;
            }

            lock (students)
            {
                students.Add(student);
            }

            return true;
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
            student.Money -= drink.Price;

            Console.WriteLine($"{student} drinks a {drink}.");
        }

        private void LoadDrinks()
        {
            // Load 10000 units of each available drink type.
            foreach (var drink in this.Drinks)
            {
                this.drinkStorage[drink] = 10000;
            }
        }
    }
}
