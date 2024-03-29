﻿using System;

namespace BarSimulator
{
    class Student
    {
        Random random;

        public Student(string name, decimal money, int age, Bar bar, Random random)
        {
            this.Name = name;
            this.Money = money;
            this.Age = age;
            this.Bar = bar;
            this.random = random;
        }

        public string Name { get; set; }
        public decimal Money { get; set; }
        public int Age { get; set; }
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
            Console.WriteLine($"{this} is walking in the streets.");
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
                        Console.WriteLine($"{this} is going back home.");
                        staysOut = false;
                        break;
                    case NightLifeActivity.GoHome:
                        Console.WriteLine($"{this} is going back home.");
                        staysOut = false;
                        break;
                    default:
                        throw new Exception($"Unexpected night life activity {nextActivity}");
                }
            }
        }

        private void VisitBar()
        {
            // First set this flag to true, in case we never enter the bar.
            bool hasLeftBar = true;
            try
            {
                Console.WriteLine($"{this} is getting in the line to enter the bar.");
                bool didEnter = Bar.Enter(this);
                if (!didEnter)
                {
                    // We got tired of waiting or were not allowed to enter the bar.
                    return;
                }

                // After we have entered the bar, set it to false.
                hasLeftBar = false;
                Console.WriteLine($"{this} entered the bar.");

                while (!hasLeftBar)
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
                            Console.WriteLine($"{this} is leaving the bar.");
                            hasLeftBar = true;
                            break;
                        default:
                            throw new Exception($"Unexpected bar activity {nextActivity}");
                    }
                }
            }
            catch (BarClosedException)
            {
                Console.WriteLine($"The bar closed before {this} could finish partying. Boo!");
            }
            finally
            {
                if (!hasLeftBar)
                {
                    Bar.Leave(this);
                }
            }
        }

        private void Drink()
        {
            var nextDrink = Bar.Drinks[random.Next(Bar.Drinks.Length)];

            Bar.Drink(this, nextDrink);
            Bar.WaitOneTick();
        }

        private void Dance()
        {
            Bar.Dance(this);
            Bar.WaitOneTick();
        }

        public override string ToString()
        {
            return this.Name;
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
