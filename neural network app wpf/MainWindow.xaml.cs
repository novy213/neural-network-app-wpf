using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace neural_network_app_wpf
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int currentValue=0;
        int remainingValue;
        int numberOfItems;

        int[] diameter = new int[6];
        int[] length = new int[6];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            ItemsCountGrid.Visibility = Visibility.Visible;
        }

        private void NextNumberFiles_click(object sender, RoutedEventArgs e)
        {
            if (numberOfItemsXAML.Text.Length > 0)
            {
                ItemsCountGrid.Visibility = Visibility.Collapsed;
                InsertValues.Visibility = Visibility.Visible;
                numberOfItems = int.Parse(numberOfItemsXAML.Text);
                remainingValue = numberOfItems;
                CurrentValues.Text = "Currently " + currentValue + " values ​​have been taken";
                RemaningValues.Text = "There are " + (remainingValue-currentValue) + " more values ​​left";
            }
            else
            {
                Alert.Text = "This field cannot be empty";
            }
        }

        private void CancelNumberFiels_click(object sender, RoutedEventArgs e)
        {
            Alert.Text = null;
            MenuGrid.Visibility = Visibility.Visible;
            ItemsCountGrid.Visibility = Visibility.Collapsed;
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); 
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void numberOfItemsXAML_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void InsertValueBack_click(object sender, RoutedEventArgs e)
        {
            ItemsCountGrid.Visibility = Visibility.Visible;
            InsertValues.Visibility = Visibility.Collapsed;
        }

        private void InsertValueNext_click(object sender, RoutedEventArgs e)
        {

        }

        private void InsertValueAdd(object sender, RoutedEventArgs e)
        {
            if (currentValue != numberOfItems)
            {
                if (DiameterXAML.Text.Length>0 && LengthXAML.Text.Length>0)
                {
                    diameter[currentValue] = int.Parse(DiameterXAML.Text);
                    length[currentValue] = int.Parse(LengthXAML.Text);

                    currentValue++;
                    CurrentValues.Text = "Currently " + currentValue + " values ​​have been taken";
                    RemaningValues.Text = "There are " + (remainingValue - currentValue) + " more values ​​left";
                    LengthXAML.Text = null;
                    DiameterXAML.Text = null;
                    Alert1.Text = null;
                }
                else
                {
                    Alert1.Text = "Please complete all fields";
                }
            }
            else
            {
                Alert1.Text = "No more value can be added";
            }
        }
    }
}
