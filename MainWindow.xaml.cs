using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace FuelConsumptionCalculator
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<CalculationHistory> History { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            History = new ObservableCollection<CalculationHistory>();

            HistoryDataGrid.ItemsSource = History;

            DataContext = this;
        }
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double distance = double.Parse(DistanceTextBox.Text);
                double fuel = double.Parse(FuelTextBox.Text);
                double price = double.Parse(PriceTextBox.Text);

                if (distance <= 0 || fuel <= 0 || price <= 0)
                {
                    MessageBox.Show("Все значения должны быть больше нуля!", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                double consumption = (fuel / distance) * 100;
                double cost = fuel * price;

                ConsumptionText.Text = $"{consumption:F2} л/100км";
                CostText.Text = $"{cost:F2} руб";

                //обновление ProgressBar
                ConsumptionProgress.Value = consumption;
                ConsumptionProgress.Minimum = 0;
                ConsumptionProgress.Maximum = Math.Max(30, consumption + 5);
                ProgressText.Text = $"{consumption:F1} л/100км";

                History.Add(new CalculationHistory
                {
                    Date = DateTime.Now,
                    Distance = distance,
                    Fuel = fuel,
                    Price = price,
                    Consumption = consumption,
                    Cost = cost
                });
            }
            catch (FormatException)
            {
                MessageBox.Show("Некорректные значения", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            DistanceTextBox.Text = "";
            FuelTextBox.Text = "";
            PriceTextBox.Text = "";
            ConsumptionText.Text = "";
            CostText.Text = "";
            ConsumptionProgress.Value = 0;
            ProgressText.Text = "0 л/100км";
        }
    }
    public class CalculationHistory
    {
        public DateTime Date { get; set; }
        public double Distance { get; set; }
        public double Fuel { get; set; }
        public double Price { get; set; }
        public double Consumption { get; set; }
        public double Cost { get; set; }
    }
}
