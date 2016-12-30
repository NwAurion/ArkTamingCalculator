namespace ArkTamingCalculator
{
    class Food
    {
        // Name, affinity, food value
        public string Name { get; set; }
        public double Affinity;
        public double FoodValue;

        public Food(string Name, double Affinity, double FoodValue)
        {
            this.Name = Name;
            this.Affinity = Affinity;
            this.FoodValue = FoodValue;
        }
    }
}
