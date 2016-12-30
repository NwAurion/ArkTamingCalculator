namespace ArkTamingCalculator
{
    class FoodAffinityMapping
    {
        // Name, affinity, food value
        public string FoodName;
        public double Affinity;
        public double FoodValue;

        public FoodAffinityMapping(string FoodName, double Affinity, double FoodValue)
        {
            this.FoodName = FoodName;
            this.Affinity = Affinity;
            this.FoodValue = FoodValue;
        }
    }
}
