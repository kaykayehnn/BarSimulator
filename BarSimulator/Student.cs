using System;

namespace BarSimulator
{
    class Student
    {
        Random random;

        public Student(string name, Bar bar, Random random)
        {
            this.Name = name;
            this.Bar = bar;
            this.random = random;
        }

        public string Name { get; set; }
        public Bar Bar { get; set; }

        private NightLifeActivity GetRandomNightLifeActivity()
        {
            int n = random.Next(10);
            if (n < 3) return NightLifeActivity.Walk;
            if (n < 8) return NightLifeActivity.VisitBar;

            return NightLifeActivity.GoHome;
        }


        private BarActivity GetRandomBarActivity()
        {
            int n = random.Next(10);
            if (n < 4) return BarActivity.Drink;
            if (n < 9) return BarActivity.Dance;

            return BarActivity.Leave;
        }

        private void WalkOut()
        {
            Console.WriteLine($"{Name} is walking in the streets.");
            Bar.WaitOneTick();
        }

        public void PaintTheTownRed()
        {
            WalkOut();

            bool staysOut = true;
            while (staysOut)
            {
                var nextActivity = GetRandomNightLifeActivity();
                switch (nextActivity)
                {
                    case NightLifeActivity.Walk:
                        WalkOut();
                        break;
                    case NightLifeActivity.VisitBar:
                        VisitBar();
                        Console.WriteLine($"{Name} is going back home.");
                        staysOut = false;
                        break;
                    case NightLifeActivity.GoHome:
                        Console.WriteLine($"{Name} is going back home.");
                        staysOut = false;
                        break;
                    default:
                        throw new Exception($"Unexpected night life activity {nextActivity}");
                }
            }
        }

        private void VisitBar()
        {
            Console.WriteLine($"{Name} is getting in the line to enter the bar.");
            Bar.Enter(this);
            Console.WriteLine($"{Name} entered the bar.");

            bool staysAtBar = true;
            while (staysAtBar)
            {
                var nextActivity = GetRandomBarActivity();
                switch (nextActivity)
                {
                    case BarActivity.Drink:
                        Drink();
                        break;
                    case BarActivity.Dance:
                        Dance();
                        break;
                    case BarActivity.Leave:
                        Bar.Leave(this);
                        Console.WriteLine($"{Name} is leaving the bar.");
                        staysAtBar = false;
                        break;
                    default:
                        throw new Exception($"Unexpected bar activity {nextActivity}");
                }
            }
        }

        private void Drink()
        {
            Console.WriteLine($"{Name} drinks.");
            Bar.WaitOneTick();
        }

        private void Dance()
        {
            Console.WriteLine($"{Name} dances.");
            Bar.WaitOneTick();
        }
    }
    enum NightLifeActivity
    {
        Walk,
        VisitBar,
        GoHome,
    }

    enum BarActivity
    {
        Drink,
        Dance,
        Leave,
    }
}
