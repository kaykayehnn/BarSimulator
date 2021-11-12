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

        Dictionary<Drink, int> drinkStorage = new Dictionary<Drink, int>();
        List<Student> students = new List<Student>();
        Semaphore semaphore = new Semaphore(10, 10);

        public Bar(Drink[] drinks)
        {
            this.Drinks = drinks;

            this.LoadDrinks();
        }

        public Drink[] Drinks { get; }

        public void Enter(Student student)
        {
            semaphore.WaitOne();
            lock (students)
            {
                students.Add(student);
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
            if (this.drinkStorage[drink] <= 0)
            {
                Console.WriteLine($"The bar is out of {drink}s.");
                return;
            }

            this.drinkStorage[drink]--;

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
