namespace BarSimulator
{
    class Drink
    {
        public Drink(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        // Source https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;

                hash = hash * 23 + this.Name.GetHashCode();

                return hash;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
