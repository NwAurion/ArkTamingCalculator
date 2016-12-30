using System.Collections.Generic;

namespace ArkTamingCalculator
{
    class Dinosaur
    {
       // Name, type of food, base affinity, affinity per level, food rate
        public string Name;
        //public string FoodType;
        public double AffinityBase;
        public double AffinityPerLevel;
        public double FoodRate;
        public List<Food> Food;
        
        public Dinosaur(string Name, double AffinityBase, double AffinityPerLevel, double FoodRate, List<Food> Food)
        {
            this.Name = Name;
            //this.FoodType = FoodType;
            this.AffinityBase = AffinityBase;
            this.AffinityPerLevel = AffinityPerLevel;
            this.FoodRate = FoodRate;
            this.Food = Food;
        }
        
    }
}
