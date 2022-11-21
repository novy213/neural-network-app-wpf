using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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

        int[] correctAnswer = new int[100];
        bool[] correct = new bool[100];
        int NumberOfNeurons = 10;
        double[] result = new double[100];
        int[] correctThings = new int[100];
        string[] thingName = new string[100];
        string[] computerThingName = new string[100];
        bool allCorrect = false;
        int correctCounter = 0;

        int[] diameter = new int[100];
        int[] length = new int[100];
        int idWrong = 0;
        double learningRate = 0.2;       
       
        Random r = new Random((int)DateTime.Now.Ticks);

        double[] weights = new double[2];
        double[] oldWeights = new double[2];
        double mutation;
        int idWrongCounter = 0;
        int attempCounter=1;
        int skipAttempCounter=1;


        bool MultiNeuronsCorrect = false;
        Neuron[] neurons = new Neuron[100];
        int b;

        public MainWindow()
        {
            InitializeComponent();
            mutation = r.Next(-100, 100);
            weights[0] = r.Next(-100, 100);
            weights[1] = r.Next(-100, 100);          
        }

        private void StartButton_click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            ItemsCountGrid.Visibility = Visibility.Visible;
            numberOfItemsXAML.Focus();
        }

        private void NextNumberFiles_click(object sender, RoutedEventArgs e)
        {
            if (numberOfItemsXAML.Text.Length > 0)
            {
                if (int.Parse(numberOfItemsXAML.Text) <= 100)
                {                    
                    ItemsCountGrid.Visibility = Visibility.Collapsed;
                    InsertValues.Visibility = Visibility.Visible;
                    numberOfItems = int.Parse(numberOfItemsXAML.Text);
                    remainingValue = numberOfItems;
                    CurrentValues.Text = "Currently " + currentValue + " values ​​have been taken";
                    RemaningValues.Text = "There are " + (remainingValue - currentValue) + " more values ​​left";
                    DiameterXAML.Focus();
                }
                else
                {
                    Alert.Text = "The maximum number of items is 100";
                    numberOfItemsXAML.Text = "100";
                }
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
            if (remainingValue - currentValue != 0)
            {
                Alert1.Text = "Insert all items";
            }
            else
            {
                InsertValues.Visibility = Visibility.Collapsed;
                NeuronSelect.Visibility = Visibility.Visible;
            }
        }
        private void NextAttempt_click(object sender, RoutedEventArgs e)
        {
            while (idWrong == 0)
            {
                if (correct[idWrongCounter] == true)
                {
                    idWrongCounter++;                                       
                }
                else if(correct[idWrongCounter] == false)
                {
                    idWrong = idWrongCounter;
                    break;
                }                
            }            
            CurrentAttempt.Items.Clear();
            oldWeights[0] = weights[0];
            oldWeights[1] = weights[1];
            weights[0] = WeightChange(correctThings[idWrong], diameter[idWrong], learningRate, oldWeights[0]);
            weights[1] = WeightChange(correctThings[idWrong], length[idWrong], learningRate, oldWeights[1]);
            for (int i = 0; i < numberOfItems; i++)
            {
                result[i] = ((diameter[i] * weights[0]) + (length[i] * weights[1])) + mutation;
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
                    this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                    this.CurrentAttempt.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                }
                else if (result[i] < 0 && correctAnswer[i] == 2)
                {
                    correct[i] = true;
                    this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                    this.CurrentAttempt.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                }
                else
                {
                    correct[i] = false;
                    this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                    this.CurrentAttempt.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                }
            }            
            if (idWrong != numberOfItems)
            {
                skipAttempCounter = attempCounter;
            }
            for (int i = 0; i < correct.Length; i++)
            {
                if (correct[i])
                {
                    correctCounter++;
                }
            }
            if (correctCounter == numberOfItems)
            {
                allCorrect = true;
                Skip.Text = "All items are known in the " + skipAttempCounter + " attempt";
            }
            attempCounter++;
            idWrong = 0;
            idWrongCounter = 0;
            correctCounter = 0;
            this.ResultListView.Items.Add(new MyItem { Computer = null, We = null, Compare = null, Id = null });
        }

        private void InsertValueAdd(object sender, RoutedEventArgs e)
        {
            if (currentValue != numberOfItems)
            {
                if (DiameterXAML.Text.Length>0 && LengthXAML.Text.Length>0)
                {
                    if (DiameterXAML.Text != LengthXAML.Text)
                    {
                        diameter[currentValue] = int.Parse(DiameterXAML.Text);
                        length[currentValue] = int.Parse(LengthXAML.Text);
                        if (length[currentValue] > diameter[currentValue])
                        {
                            correctAnswer[currentValue] = 2;
                        }
                        else if (length[currentValue] < diameter[currentValue])
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
                        Alert1.Text = "These values ​​cannot be the same";
                    }
                    DiameterXAML.Focus();
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
            public bool? Compare { get; set; }
            public int? Id { get; set; }
            public int? Diameter { get; set; }
            public int? Length { get; set; }        
        }

        private void MainGridStop_click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            MenuGrid.Visibility = Visibility.Visible;
            MultiNeuronsGrid.Visibility = Visibility.Collapsed;
            numberOfNeurons.Visibility = Visibility.Collapsed;
            ApplyNumberOfNeurons.Visibility = Visibility.Collapsed;
            NeuronsList.Items.Clear();
            SelectedNeuronAttemp.Items.Clear();
            mutation = r.Next(-100, 100);
            weights[0] = r.Next(-100, 100);
            weights[1] = r.Next(-100, 100);
            correctAnswer = new int[100];
            correct = new bool[100];
            result = new double[100];
            correctThings = new int[100];
            thingName = new string[100];
            computerThingName = new string[100];
            diameter = new int[100];
            length = new int[100];
            numberOfItemsXAML.Text = null;
            LengthXAML.Text = null;
            DiameterXAML.Text = null;
            currentValue = 0;
            remainingValue=0;
            numberOfItems=0;
            Alert.Text = null;
            Alert1.Text = null;
            Skip.Text = null;
            CurrentAttempt.Items.Clear();
            ResultListView.Items.Clear();
            attempCounter = 1;
            skipAttempCounter = 1;
            allCorrect = false;
            idWrong = 0;
            idWrongCounter = 0;            
            for(int i = 0; i < 99;i++)
            {
                neurons[i] = null;
            }
        }
        
        public static double WeightChange(int correctThing, double arm, double learningRate, double weigth)
        {
            double result = (correctThing * arm * learningRate) + weigth;
            return result;
        }

        private void Skip_click(object sender, RoutedEventArgs e)
        {
            do
            {                
                while (idWrong == 0)
                {
                    if (correct[idWrongCounter] == false)
                    {
                        idWrong = idWrongCounter;
                        break;
                    }
                    idWrongCounter++;
                }
                if (idWrong == numberOfItems)
                {
                    allCorrect = true;
                    Skip.Text = "All items are known in the " + skipAttempCounter + " attempt";
                    break;
                }
                CurrentAttempt.Items.Clear();
                oldWeights[0] = weights[0];
                oldWeights[1] = weights[1];
                weights[0] = WeightChange(correctThings[idWrong], diameter[idWrong], learningRate, oldWeights[0]);
                weights[1] = WeightChange(correctThings[idWrong], length[idWrong], learningRate, oldWeights[1]);
                for (int i = 0; i < numberOfItems; i++)
                {
                    result[i] = ((diameter[i] * weights[0]) + (length[i] * weights[1])) + mutation;
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
                        this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                        this.CurrentAttempt.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                    }
                    else if (result[i] < 0 && correctAnswer[i] == 2)
                    {
                        correct[i] = true;
                        this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                        this.CurrentAttempt.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                    }
                    else
                    {
                        correct[i] = false;                                                             
                        this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                        this.CurrentAttempt.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });                        
                    }
                }
                if (idWrong != numberOfItems)
                {
                    skipAttempCounter = attempCounter;
                }
                attempCounter++;
                idWrong = 0;
                idWrongCounter = 0;
                this.ResultListView.Items.Add(new MyItem { Computer = null, We = null, Compare = null, Id = null });
            } while (!allCorrect);
        }


        public class Neuron
        {
            public List<MyItem> neuronList = new List<MyItem>();
            public bool neuronCorrect;

            public int numberOfItems;           

            public int[] correctAnswer = new int[100];
            public bool[] correct = new bool[100];
            public int NumberOfNeurons = 10;
            public double[] result = new double[100];
            public int[] correctThings = new int[100];
            public string[] thingName = new string[100];
            public string[] computerThingName = new string[100];
            public bool allCorrect = false;
            public int correctCounter = 0;

            public int[] diameter = new int[100];
            public int[] length = new int[100];
            public int idWrong = 0;
            public double learningRate = 0.2;

            public Random r = new Random();

            public double[] weights = new double[2];
            public double[] oldWeights = new double[2];
            public double mutation;
            public int idWrongCounter = 0;
            public int attempCounter = 1;
            public int skipAttempCounter;

            public Neuron(int[] diameter, int[] length, int numberOfItems, string[] thingName, int[] correctThing, int[] correctAnswer)
            {           
                this.numberOfItems = numberOfItems;
                for (int i = 0; i < numberOfItems; i++)
                {
                    this.diameter[i] = diameter[i];
                    this.correctThings[i] = correctThing[i];
                    this.length[i] = length[i];
                    this.correctAnswer[i] = correctAnswer[i];
                    this.thingName[i] = thingName[i];                    
                }
                mutation = r.Next(-100, 100);
                Thread.Sleep(10);
                weights[0] = r.Next(-100, 100);
                Thread.Sleep(10);
                weights[1] = r.Next(-100, 100);
                neuronCorrect = false;
                
            }            

            public void NextAttempt()
            {                
                while (idWrong == 0)
                {
                    if (correct[idWrongCounter] == true)
                    {
                        idWrongCounter++;
                    }
                    else if (correct[idWrongCounter] == false)
                    {
                        idWrong = idWrongCounter;
                        break;
                    }
                }                
                oldWeights[0] = weights[0];
                oldWeights[1] = weights[1];
                weights[0] = WeightChange(correctThings[idWrong], diameter[idWrong], learningRate, oldWeights[0]);
                weights[1] = WeightChange(correctThings[idWrong], length[idWrong], learningRate, oldWeights[1]);
                for (int i = 0; i < numberOfItems; i++)
                {
                    result[i] = ((diameter[i] * weights[0]) + (length[i] * weights[1])) + mutation;
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
                        this.neuronList.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i]});
                    }
                    else if (result[i] < 0 && correctAnswer[i] == 2)
                    {
                        correct[i] = true;
                        this.neuronList.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i]});
                    }
                    else
                    {
                        correct[i] = false;
                        this.neuronList.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i]});
                    }                    
                }
                this.neuronList.Add(new MyItem { Computer = null, We = null, Compare = null, Id = null, Length = null, Diameter = null});
                if (idWrong != numberOfItems)
                {
                    skipAttempCounter = attempCounter;
                }
                for (int i = 0; i < correct.Length; i++)
                {
                    if (correct[i])
                    {
                        correctCounter++;
                    }
                }
                if (correctCounter == numberOfItems)
                {
                    allCorrect = true;                    
                }
                if (allCorrect)
                {
                    neuronCorrect = true;                      
                }
                attempCounter++;
                idWrong = 0;
                idWrongCounter = 0;
                correctCounter = 0;
            }           
        }
        public void Random(Neuron neuron)
        {
            neuron.mutation = r.Next(-100, 100);
            neuron.weights[0] = r.Next(-100, 100);
            neuron.weights[1] = r.Next(-100, 100);
        }
        public class NeuronList
        {
            public int Id { get; set; }
            public bool Correct { get; set; }
            public int IdCorrect { get; set; }
        }
        
        private void SingleNeuron_click(object sender, RoutedEventArgs e)
        {
            if (currentValue == numberOfItems)
            {
                NeuronSelect.Visibility = Visibility.Collapsed;
                MainGrid.Visibility = Visibility.Visible;
                for (int i = 0; i < numberOfItems; i++)
                {
                    result[i] = ((diameter[i] * weights[0]) + (length[i] * weights[1])) + mutation;
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
                        this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                        this.CurrentAttempt.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                    }
                    else if (result[i] < 0 && correctAnswer[i] == 2)
                    {
                        correct[i] = true;
                        this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                        this.CurrentAttempt.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                    }
                    else
                    {
                        correct[i] = false;
                        this.ResultListView.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                        this.CurrentAttempt.Items.Add(new MyItem { Computer = computerThingName[i], We = thingName[i], Compare = correct[i], Id = attempCounter, Diameter = diameter[i], Length = length[i] });
                    }
                }
                this.ResultListView.Items.Add(new MyItem { Computer = null, We = null, Compare = null, Id = null });
                while (idWrong == 0)
                {
                    if (correct[idWrongCounter] == false)
                    {
                        idWrong = idWrongCounter;
                        break;
                    }
                    idWrongCounter++;
                }
                if (idWrong == numberOfItems)
                {
                    allCorrect = true;
                    Skip.Text = "All items are known in the " + skipAttempCounter + " attempt";
                }
            }
            else
            {
                Alert1.Text = "No value was given";
            }
            attempCounter++;
        }
        int clickCounter=0;
        private void MultiNeurons_click(object sender, RoutedEventArgs e)
        {
            numberOfNeurons.Visibility = Visibility.Visible;
            ApplyNumberOfNeurons.Visibility = Visibility.Visible;
            numberOfNeurons.Text = NumberOfNeurons.ToString();
            numberOfNeurons.Focus();
            numberOfNeurons.SelectAll();
        }

        private void ApplyNumberOfNeurons_Click(object sender, RoutedEventArgs e)
        {
            NumberOfNeurons = int.Parse(numberOfNeurons.Text);
            for (int i = 0; i < NumberOfNeurons; i++)
            {
                neurons[i] = new Neuron(diameter, length,numberOfItems,thingName,correctThings,correctAnswer);
                neurons[i].NextAttempt();
                NeuronsList.Items.Add(new NeuronList { Correct = neurons[i].neuronCorrect, Id = i+1, IdCorrect = neurons[i].skipAttempCounter });                                              
            }
            NeuronSelect.Visibility = Visibility.Collapsed;
            MultiNeuronsGrid.Visibility=Visibility.Visible;
        }      
        private void MultiNeuronNext_click(object sender, RoutedEventArgs e)
        {                      
            clickCounter++;                 
            NeuronsList.Items.Clear();            
            for (int i = 0; i < NumberOfNeurons; i++)
            {
                neurons[i].NextAttempt();
                NeuronsList.Items.Add(new NeuronList { Correct = neurons[i].neuronCorrect, Id = i + 1, IdCorrect = neurons[i].skipAttempCounter });
            }
        }
        private void NeuronsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView btn = (ListView)sender;
            int a = btn.SelectedIndex;
            if (a > 0)
            {
                SelectedNeuronAttemp.ItemsSource = neurons[a].neuronList;
                b = a;
            }
            else
            {
                SelectedNeuronAttemp.ItemsSource = neurons[b].neuronList;
            }
        }        

        private void MultiNeuronSkip_Click(object sender, RoutedEventArgs e)
        {            
            int correctId=0;
            do
            {
                for (int i = 0; i < NumberOfNeurons; i++)
                {
                    neurons[i].NextAttempt();
                    if (neurons[i].neuronCorrect)
                    {
                        MultiNeuronsCorrect = true;
                        correctId = i;
                    }
                }               
            } while (!MultiNeuronsCorrect);
            NeuronsList.Items.Clear();
            for(int i=0; i < NumberOfNeurons; i++)
            {
                NeuronsList.Items.Add(new NeuronList { Correct = neurons[i].neuronCorrect, Id = i + 1, IdCorrect = neurons[i].skipAttempCounter });
            }
            Alert2.Text = "Neuron " + (correctId+1).ToString() + " is correct at " + neurons[correctId].skipAttempCounter + " attemp";
        }
    }
}
