using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Contexts;
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

//using static Calculator.Command;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PreviewTextInput += KeyboardTextInput;
        }
        public  List<string> history = new List<string>();

        public void StoreAndExecute(ICommand command)
        {
            history.Add(TextOutput.Text.ToString());
            command.Execute();
        }

        private void OpenExtendedMenu_Click(object sender, RoutedEventArgs e)
        {
            GridLength open = new GridLength(1, GridUnitType.Star);
            if (additionalOptions.Width == open)
            {
                GridLength close = new GridLength(0, GridUnitType.Star);
                additionalOptions.Width = close;
            }
            else
            {
                additionalOptions.Width = open;
            }
        }
        private void Num_Click(object sender, RoutedEventArgs e)
        {
            Error_Clear();
            string content = (sender as Button).Content.ToString();
            string expression = TextOutput.Text.ToString();

            if (content == ",")
            {
                if (expression.Length > 0 && char.IsDigit(expression[expression.Length - 1]))
                {
                    char[] operators = { '+', '-', '*', '/' };
                    if (!expression.Split(operators).Last().Contains(','))
                    {
                        TextOutput.Text += content;
                    }
                }
            }
            else
            {
                TextOutput.Text += content;
            }
        }
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            Error_Clear();
            string content = (sender as Button).Content.ToString();
                        
            switch (content)
            {
                case "Backspace":
                    ICommand BackspaceCommand = new DeleteOneCharacterCommand(new ReciverCalculator(TextOutput));
                    StoreAndExecute(BackspaceCommand);
                    break;
                case "C":
                    ICommand ClearCommand = new ClearAllCommand(new ReciverCalculator(TextOutput));
                    StoreAndExecute(ClearCommand);
                    break;
                case "CE":
                    ICommand ClearEntery = new ClearEntryCommand(ref history, TextOutput);
                    ClearEntery.Execute();
                    break;
                case "=":
                    ICommand EvaluateCommand = new EvaluateExpressionCommand(new ReciverCalculator(TextOutput));
                    StoreAndExecute(EvaluateCommand);
                    break;
                default:
                    ICommand AddCharacterCommand = new AddCharacterCommand(new ReciverCalculator(TextOutput), content);
                    StoreAndExecute(AddCharacterCommand);
                    break;
            }
        }
        private void ExtendedOperation_Click(object sender, RoutedEventArgs e)
        {
            Error_Clear();
            string content = (sender as Button).Content.ToString();

            ICommand EvaluateCommand = new EvaluateExpressionCommand(new ReciverCalculator(TextOutput));
            StoreAndExecute(EvaluateCommand);

            switch (content)
            {
                case "π":
                    ICommand AddPi = new WritePiCommand(TextOutput);
                    StoreAndExecute(AddPi);
                    break;
                case "log":
                    ICommand FindLogCommand = new LogCommand(new ReciverCalculator(TextOutput));
                    StoreAndExecute(FindLogCommand);
                    break;
                case "√":
                    ICommand FindSquareRootCommand = new SquareRootCommand(new ReciverCalculator(TextOutput));
                    StoreAndExecute(FindSquareRootCommand);
                    break;
                case "^2":
                    ICommand ElevationToSquareCommand = new ElevationToSquareCommand(new ReciverCalculator(TextOutput));
                    StoreAndExecute(ElevationToSquareCommand);
                    break;
                default:
                    TextOutput.Text = "Error";
                    break;
            }
        }
        private void Error_Clear()
        {
            if (TextOutput.Text.Contains("Error"))
            {
                TextOutput.Text = "";
            }
        }
        private void KeyboardTextInput(object sender, TextCompositionEventArgs e)
        {
            Error_Clear();

            if (char.IsDigit(e.Text, 0) || "+-*/".Contains(e.Text) || e.Text == "\b" || e.Text == "\r")
            {
                if (e.Text == "\b")
                {
                    ICommand BackspaceCommand = new DeleteOneCharacterCommand(new ReciverCalculator(TextOutput));
                    StoreAndExecute(BackspaceCommand);
                }
                else if (e.Text == "\r")
                {
                    ICommand EvaluateCommand = new EvaluateExpressionCommand(new ReciverCalculator(TextOutput));
                    StoreAndExecute(EvaluateCommand);
                }
                else if (char.IsDigit(e.Text, 0) || e.Text == ",")
                {
                    string content = e.Text;
                    string expression = TextOutput.Text.ToString();

                    if (content == ",")
                    {
                        if (expression.Length > 0 && char.IsDigit(expression[expression.Length - 1]))
                        {
                            char[] operators = { '+', '-', '*', '/' };
                            if (!expression.Split(operators).Last().Contains(','))
                            {
                                TextOutput.Text += content;
                            }
                        }
                    }
                    else
                    {
                        TextOutput.Text += content;
                    }
                }
                else
                {
                    ICommand AddCharacterCommand = new AddCharacterCommand(new ReciverCalculator(TextOutput), e.Text);
                    StoreAndExecute(AddCharacterCommand);
                }
            }
        }
    }
}
