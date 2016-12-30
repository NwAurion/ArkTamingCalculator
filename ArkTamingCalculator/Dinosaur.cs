namespace ArkTamingCalculator
{
    class Dinosaur
    {
       // Name, type of food, base affinity, affinity per level, food rate
        public string Name;
        public string FoodType;
        public double AffinityBase;
        public double AffinityPerLevel;
        public double FoodRate; 
        
        public Dinosaur(string Name, string FoodType, double AffinityBase, double AffinityPerLevel, double FoodRate)
        {
            this.Name = Name;
            this.FoodType = FoodType;
            this.AffinityBase = AffinityBase;
            this.AffinityPerLevel = AffinityPerLevel;
            this.FoodRate = FoodRate;
        }
        
    }
}
