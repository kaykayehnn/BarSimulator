using System.Collections.Generic;
using System.Threading;

namespace BarSimulator
{
    class Bar
    {
        // After each action, we sleep for a tick to simulate the passing of
        // one minute.
        public const int TICK_MILLISECONDS = 1;

        List<Student> students = new List<Student>();
        Semaphore semaphore = new Semaphore(10, 10);

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
    }
}
