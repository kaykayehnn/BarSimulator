using System;
using System.Collections.Generic;
using System.Threading;

namespace BarSimulator
{
    class Program
    {
        /*
         * Take the Bar simulator example from the lectures and extend it with:
            1. Drinks: there are some drinks on sale, each with price and quantity in stock.
               Visitors can choose a drink to get and purchase it.
            2. Visitors have a budget to spent.
            3. Visitors have age and the Bar doesn't let in people below 18.
            4. Visitors that wait in the queue may decide (randomly) to leave the queue and walk
               the streets again. Bonus points if the longer they wait, the more they are likely
               to leave the queue.
            5. Bar can close at some point and kick everyone out, then it won't let anyone in.
            6. Bar reports all sales and which drinks are out of stock
        */
        static void Main(string[] args)
        {
            // We simulate night life of students.
            //
            // Students do:
            // 1. go out for a walk;
            // 2. choose what to do:
            //    - keep walking
            //    - go to a bar
            //    - go back home
            // While at a bar:
            //   - Drink;
            //   - Dance;
            //
            //   - Decide to leave;
            // ---- When you leave, you go back home.
            //
            // The bar has N available seats

            const double MAX_MONEY = 10_000;
            const int MAX_AGE = 100;

            var drinks = new[] {
                new Drink("Beer", 5),
                new Drink("Vodka", 25),
            };

            Random random = new Random();
            Bar bar = new Bar(drinks, random);
            NameGenerator nameGen = new NameGenerator(random);

            var studentThreads = new List<Thread>();
            for (int i = 0; i < 5000; i++)
            {
                var name = nameGen.Next();
                var money = (decimal)(random.NextDouble() * MAX_MONEY);
                var age = random.Next(MAX_AGE);
                var student = new Student(name, money, age, bar, random);
                var thread = new Thread(student.PaintTheTownRed);

                studentThreads.Add(thread);
            }

            // Open the bar
            Thread barThread = new Thread(bar.OpenBar);
            barThread.Start();

            // And let all students go wild
            foreach (var t in studentThreads)
            {
                t.Start();
            }

            foreach (var t in studentThreads)
            {
                t.Join();
            }
            barThread.Join();

            Console.WriteLine();
            
            var report = bar.GenerateDrinkReport();
            Console.WriteLine(report);

            Console.WriteLine("The party is over.");
        }
    }
}
