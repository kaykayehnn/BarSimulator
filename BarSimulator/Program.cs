using System;
using System.Collections.Generic;
using System.Threading;

namespace BarSimulator
{
    class Program
    {
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
            var studentThreads = new List<Thread>();
            for (int i = 0; i < 100; i++)
            {
                var student = new Student(i.ToString(), bar);
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
