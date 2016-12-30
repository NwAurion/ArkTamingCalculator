using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls;
using System.IO;
using System.Globalization;

namespace ArkTamingCalculator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Dictionary<string, Dinosaur> Dinosaurs = new Dictionary<string, Dinosaur>();
        static Dictionary<string, Food> Food = new Dictionary<string, Food>();

        // Adjust affinity gain per food
        static double TamingMultiplier = 1;

        // Adjust how fast food drains
        static double FoodRateMultiplier = 1;

        static int Level = 1;

        double FoodValueOfSuppliedFood;
        double TargetFood { get; set; }
        double CurrentFood { get; set; }
        double MaxFood { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            iudTamingMultiplier.ValueChanged += iudTamingMultiplier_ValueChanged;
            iudFoodRateMultiplier.ValueChanged += iudFoodRateMultiplier_ValueChanged;
            iudLevel.ValueChanged += iudLevel_ValueChanged;

            LoadData();

            cbDinoChooser.ItemsSource = Dinosaurs.Keys;
        }

        private void DoCalculate()
        {
            string dinoName = cbDinoChooser.SelectedValue.ToString();
            if (Dinosaurs.ContainsKey(dinoName))
            {
                Dinosaur dino = Dinosaurs[dinoName];
                double affinityTotal = dino.AffinityBase + dino.AffinityPerLevel * Level;



                FoodLabel.ItemsSource = dino.Food;
                FoodUpDown.ItemsSource = dino.Food;

                List<double> listMaxFoodItemsNeeded = new List<double>();
                List<string> durations = new List<string>();

                double foodAmountNeeded = 0;

                foreach (Food food in dino.Food)
                {
                    double affinityPerFood = food.Affinity * TamingMultiplier;
                    double maxFoodItemsNeeded = Math.Ceiling(affinityTotal / affinityPerFood);
                    listMaxFoodItemsNeeded.Add(maxFoodItemsNeeded);

                    foodAmountNeeded = maxFoodItemsNeeded * food.FoodValue;
                    double timeInSeconds = dino.FoodRate / FoodRateMultiplier * foodAmountNeeded * 10;
                    TimeSpan duration = TimeSpan.FromSeconds(timeInSeconds);
                    string time = duration.ToString(@"hh\:mm\:ss");
                    durations.Add(time);
                }

                MaxLabel.ItemsSource = listMaxFoodItemsNeeded;
                TimeLabel.ItemsSource = durations;
                FoodValueOfSuppliedFood = foodAmountNeeded;
            }
        }

        private void LoadData()
        {
            LoadFood();
            LoadDinosaurs();
        }

        private void LoadDinosaurs()
        {
            string[] allDinosaurInfo = File.ReadAllLines("DinosaurValues.csv");
            foreach (string dinosaurInfo in allDinosaurInfo)
            {
                string[] dinosaur = dinosaurInfo.Split(',');

                string Name = dinosaur[0];
                //string FoodType = dinosaur[1];
                double AffinityBase = Double.Parse(dinosaur[1]);
                double AffinityPerLevel = Double.Parse(dinosaur[2]);
                double FoodRate = Double.Parse(dinosaur[3], CultureInfo.InvariantCulture);
                List<Food> FoodList = new List<Food>();
                foreach (string foodInfo in dinosaur)
                {
                    if (Food.ContainsKey(foodInfo))
                    {
                        FoodList.Add(Food[foodInfo]);
                    }          
                }

                Dinosaur dino = new Dinosaur(Name, AffinityBase, AffinityPerLevel, FoodRate, FoodList);
                Dinosaurs.Add(Name, dino);
            }
        }

        private void LoadFood()
        {
            string[] allFoodAffinityInfo = File.ReadAllLines("FoodAffinityMapping.csv");
            foreach (string foodAffinityInfo in allFoodAffinityInfo)
            {
                string[] foodAffinityMapping = foodAffinityInfo.Split(',');

                string FoodName = foodAffinityMapping[0];
                double Affinity = double.Parse(foodAffinityMapping[1]);
                double FoodValue = double.Parse(foodAffinityMapping[2]);
                Food FoodAffinityMapping = new Food(FoodName, Affinity, FoodValue);
                Food.Add(FoodName, FoodAffinityMapping);
            }
        }

        private void tbCurrentFood_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbMaxFood_TextChanged(object sender, TextChangedEventArgs e)
        {
            double maxFood = int.Parse(tbMaxFood.Text);
            double targetFood = maxFood - FoodValueOfSuppliedFood;
            lblTargetFood.Content = targetFood;
        }


        private void iudTamingMultiplier_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            double newValue = double.Parse(e.NewValue.ToString());
            if (newValue == 0){
                newValue = 1;
            }
            TamingMultiplier = newValue;

            DoCalculate();
        }

        private void iudFoodRateMultiplier_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            double newValue = double.Parse(e.NewValue.ToString());
            if (newValue == 0)
            {
                newValue = 1;
            }
            FoodRateMultiplier = newValue;

            DoCalculate();
        }

        private void iudLevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int newValue = int.Parse(e.NewValue.ToString());
            if (newValue == 0)
            {
                newValue = 1;
            }
            Level = newValue;

            DoCalculate();
        }

        private void cbDinoChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoCalculate();
        }

        private void iudFood_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}
