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
        static Dictionary<string, FoodAffinityMapping> FoodAffinityMappings = new Dictionary<string, FoodAffinityMapping>();
        static Dictionary<string, List<string>> FoodMapping = new Dictionary<string, List<string>>();
        //static Dictionary<string, Tuple<int, int>> FoodAffinityMapping = new Dictionary<string,Tuple<int, int>>();
        static List<string> mapping;

        // Adjust affinity gain per food
        static double TamingMultiplier = 1;

        // Adjust how fast food drains
        static double FoodRateMultiplier = 1;

        public double FoodValueOfSuppliedFood;
        public double TargetFood { get; set; }
        public double CurrentFood { get; set; }
        public double MaxFood { get; set; }
        public int Level;

        public MainWindow()
        {
            InitializeComponent();

            iudTamingMultiplier.ValueChanged += iudTamingMultiplier_ValueChanged;
            iudFoodRateMultiplier.ValueChanged += iudFoodRateMultiplier_ValueChanged;
            iudLevel.ValueChanged += iudLevel_ValueChanged;

            // Type of food, list of relevant food
            FoodMapping.Add("Carnivore", new List<string>() { "Kibble", "Raw Prime Meat", "Raw Prime Fish Meat", "Cooked Prime Meat", "Raw Meat", "Raw Fish Meat", "Cooked Meat" });
            FoodMapping.Add("Herbivore", new List<string>() { "Kibble", "Mejoberries", "Vegetables", "Other berries" });

            LoadData();

            cbDinoChooser.ItemsSource = Dinosaurs.Keys;
            // Name, level
            //DoCalculate();
        }

        public void DoCalculate()
        {
            string dinoName = cbDinoChooser.SelectedValue.ToString();
            if (Dinosaurs.ContainsKey(dinoName))
            {
                Dinosaur dino = Dinosaurs[dinoName];
                double affinityTotal = dino.AffinityBase + dino.AffinityPerLevel * Level;


                mapping = FoodMapping[dino.FoodType];
                FoodLabel.ItemsSource = mapping;
                FoodUpDown.ItemsSource = mapping;

                List<double> listMaxFoodItemsNeeded = new List<double>();
                List<string> durations = new List<string>();
                double foodAmountNeeded = 0;
                foreach (string food in FoodMapping[dino.FoodType])
                {
                    double affinityPerFood = FoodAffinityMappings[food].Affinity * TamingMultiplier;
                    double maxFoodItemsNeeded = Math.Ceiling(affinityTotal / affinityPerFood);
                    listMaxFoodItemsNeeded.Add(maxFoodItemsNeeded);

                    foodAmountNeeded = maxFoodItemsNeeded * FoodAffinityMappings[food].FoodValue;
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

        public void LoadData()
        {
            LoadDinosaurs();
            LoadFoodAffinityMapping();
        }

        public void LoadDinosaurs()
        {
            string[] allDinosaurInfo = File.ReadAllLines("DinosaurValues.csv");
            foreach (string dinosaurInfo in allDinosaurInfo)
            {
                string[] dinosaur = dinosaurInfo.Split(',');

                string Name = dinosaur[0];
                string FoodType = dinosaur[1];
                double AffinityBase = Double.Parse(dinosaur[2]);
                double AffinityPerLevel = Double.Parse(dinosaur[3]);
                double FoodRate = Double.Parse(dinosaur[4], CultureInfo.InvariantCulture);
                Dinosaur dino = new Dinosaur(Name, FoodType, AffinityBase, AffinityPerLevel, FoodRate);
                Dinosaurs.Add(Name, dino);
            }
        }

        public void LoadFoodAffinityMapping()
        {
            string[] allFoodAffinityInfo = File.ReadAllLines("FoodAffinityMapping.csv");
            foreach (string foodAffinityInfo in allFoodAffinityInfo)
            {
                string[] foodAffinityMapping = foodAffinityInfo.Split(',');

                string FoodName = foodAffinityMapping[0];
                double Affinity = double.Parse(foodAffinityMapping[1]);
                double FoodValue = double.Parse(foodAffinityMapping[2]);
                FoodAffinityMapping FoodAffinityMapping = new FoodAffinityMapping(FoodName, Affinity, FoodValue);
                FoodAffinityMappings.Add(FoodName, FoodAffinityMapping);
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
            TamingMultiplier = double.Parse(e.NewValue.ToString());
            DoCalculate();
        }

        private void iudFoodRateMultiplier_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FoodRateMultiplier = double.Parse(e.NewValue.ToString());
            DoCalculate();
        }

        private void iudLevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Level = int.Parse(e.NewValue.ToString());
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
