using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace neural_network_app_wpf
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
    }
}
