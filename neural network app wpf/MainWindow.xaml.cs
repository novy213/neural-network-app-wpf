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

        int[] correctAnswer = new int[6];
        bool[] correct = new bool[6];
        double[] result = new double[6];
        int[] correctThings = new int[6];

        string[] thingName = new string[6];
        string[] computerThingName = new string[6];

        int[] diameter = new int[6];
        int[] length = new int[6];

        Random r =new Random();

        double[] weights = new double[2];
        double[] oldWeights = new double[2];
        double mutaion;
        int idWrongCounter = 0;
        int attempCounter = 2;

        public MainWindow()
        {
            InitializeComponent();
            mutaion = r.Next(-1000, 1000);
            weights[0] = r.Next(-1000, 1000);
            weights[1] = r.Next(-1000, 1000);
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
            if (currentValue == numberOfItems)
            {
                InsertValues.Visibility = Visibility.Collapsed;
                MainGrid.Visibility = Visibility.Visible;
                for (int i = 0; i < numberOfItems; i++)
                {
                    result[i] = ((diameter[i] * weights[0]) + (length[i] * weights[1])) + mutaion;
                    if (result[i] > 0)
                    {
                        computerThingName[i] = "Ring";
                    }
                    else if (result[i] < 0)
                    {
                        computerThingName[i] = "Pen";
                    }
                    if (result[i] > 0 && correctAnswer[i] == 1)
                    {
                        correct[i] = true;
                        /*Console.Write(result[i] + " " + "Computer {0}, we {1}", computerThingName[i], thingName[i] + " ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(correct[i]);
                        Console.ForegroundColor = ConsoleColor.White;*/
                        this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i] });
                    }
                    else if (result[i] < 0 && correctAnswer[i] == 2)
                    {
                        correct[i] = true;
                        /*Console.Write(result[i] + " " + "Computer {0}, we {1}", computerThingName[i], thingName[i] + " ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(correct[i]);
                        Console.ForegroundColor = ConsoleColor.White;*/
                        this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i] });
                    }
                    else
                    {
                        correct[i] = false;
                        /*Console.Write(result[i] + " " + "Computer {0}, we {1}", computerThingName[i], thingName[i] + " ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(correct[i]);
                        Console.ForegroundColor = ConsoleColor.White;*/
                        this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i] });
                    }
                }
            }
            else
            {
                Alert1.Text = "No value was given";
            }
        }

        private void InsertValueAdd(object sender, RoutedEventArgs e)
        {
            if (currentValue != numberOfItems)
            {
                if (DiameterXAML.Text.Length>0 && LengthXAML.Text.Length>0)
                {
                    diameter[currentValue] = int.Parse(DiameterXAML.Text);
                    length[currentValue] = int.Parse(LengthXAML.Text);
                    if(length[currentValue] > diameter[currentValue])
                    {
                        correctAnswer[currentValue] = 2;
                    }
                    else if(length[currentValue] < diameter[currentValue])
                    {
                        correctAnswer[currentValue] = 1;
                    }
                    if (correctAnswer[currentValue] == 1)
                    {
                        thingName[currentValue] = "Ring";
                        correctThings[currentValue] = 1;
                    }
                    else if (correctAnswer[currentValue] == 2)
                    {
                        thingName[currentValue] = "Pen";
                        correctThings[currentValue] = -1;
                    }
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
        public class MyItem
        {
            public string Computer { get; set; }

            public string We { get; set; }
            public bool Compare { get; set; }
        }

        private void MainGridStop_click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            MenuGrid.Visibility = Visibility.Visible;
        }
    }
}
