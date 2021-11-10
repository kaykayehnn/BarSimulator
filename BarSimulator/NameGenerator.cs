using System;

namespace BarSimulator
{
    class NameGenerator
    {
        // Source https://eslyes.com/namesdict/popular_names.htm
        private string[] names = {
            "James", "David", "Christopher", "George", "Ronald",
            "John", "Richard", "Daniel", "Kenneth", "Anthony",
            "Robert", "Charles", "Paul", "Steven", "Kevin",
            "Michael", "Joseph", "Mark", "Edward", "Jason",
            "William", "Thomas", "Donald", "Brian", "Jeff",
            "Mary", "Jennifer", "Lisa", "Sandra", "Michelle",
            "Patricia", "Maria", "Nancy", "Donna", "Laura",
            "Linda", "Susan", "Karen", "Carol", "Sarah",
            "Barbara", "Margaret", "Betty", "Ruth", "Kimberly",
            "Elizabeth", "Dorothy", "Helen", "Sharon", "Deborah",
        };
        private Random random;

        public NameGenerator() : this(new Random()) { }

        public NameGenerator(Random random)
        {
            this.random = random;
        }

        public string Next()
        {
            return names[random.Next(names.Length)];
        }
    }
}
