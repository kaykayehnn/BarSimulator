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

            Bar bar = new Bar();
            Random random = new Random();
            NameGenerator nameGen = new NameGenerator(random);

            var studentThreads = new List<Thread>();
            for (int i = 0; i < 100; i++)
            {
                var name = nameGen.Next();
                var student = new Student(name, bar);
                var thread = new Thread(student.PaintTheTownRed);

                thread.Start();

                studentThreads.Add(thread);
            }

            foreach (var t in studentThreads)
            {
                t.Join();
            }

            Console.WriteLine();
            Console.WriteLine("The party is over.");
        }
    }
}
