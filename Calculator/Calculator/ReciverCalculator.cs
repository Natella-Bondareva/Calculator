using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Calculator
{
    public class ReciverCalculator
    {
        private TextBox _outputTextBox;
        string pattern = @"(^-?\d+(\,\d+)?\s*)([\+\-\*\/]\s*)(\d+(\,\d+)?$)";

        public ReciverCalculator(TextBox textOutput)
        {
            _outputTextBox = textOutput;
        }

        public void ElevationToSquare()
        {
            if (double.TryParse(_outputTextBox.Text, out double value))
            {
                _outputTextBox.Text = Math.Pow(value, 2).ToString();
            }
            else
            {
                _outputTextBox.Text = "Error";
            }
        }

        public void FindLog()
        {
            if (double.TryParse(_outputTextBox.Text, out double value))
            {
                if (value > 0)
                {
                    _outputTextBox.Text = Math.Log10(value).ToString();
                }
                else
                {
                    _outputTextBox.Text = "Invalid Input";
                }
            }
            else
            {
                _outputTextBox.Text = "Error";
            }
        }

        public void FindSquareRoot()
        {
            if (double.TryParse(_outputTextBox.Text, out double value))
            {
                if (value >= 0)
                {
                    _outputTextBox.Text = Math.Sqrt(value).ToString();
                }
                else
                {
                    _outputTextBox.Text = "Invalid Input";
                }
            }
            else
            {
                _outputTextBox.Text = "Error";
            }
        }

        public void Add(string character)
        {
            string expression = _outputTextBox.Text;

            if (expression.Length >= 1)
            {
                string previousCharacter = expression[expression.Length - 1].ToString();
                string[] operators = { "+", "-", "*", "/" };

                if (operators.Contains(character) && operators.Contains(previousCharacter))
                {
                    _outputTextBox.Text = expression.Substring(0, expression.Length - 1) + character;
                    return;
                }
                _outputTextBox.Text += character;
            }

            string computingPattern = @"(?<!\d\s*[-+*/])\d+\s*[+\-/*]\s*\d+\s*[+\-/*]";
            if (Regex.IsMatch(_outputTextBox.Text, computingPattern))
            {
                _outputTextBox.Text = _outputTextBox.Text.Substring(0, _outputTextBox.Text.Length - 1);
                new EvaluateExpressionCommand(new ReciverCalculator(_outputTextBox)).Execute();
                _outputTextBox.Text += character;
            }
        }

        public void Backspace()
        {
            string expression = _outputTextBox.Text;
            if (expression.Length > 0)
            {
                _outputTextBox.Text = expression.Remove(expression.Length - 1);
            }
        }

        public void Clear()
        {
            _outputTextBox.Text = "";
        }

        public void EvaluateExpression()
        {
            string input = _outputTextBox.Text;
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                double num1 = double.Parse(match.Groups[1].Value);
                char operation = char.Parse(match.Groups[3].Value);
                double num2 = double.Parse(match.Groups[4].Value);

                PerformOperation(num1, operation, num2);
            }

        }
        private void PerformOperation(double num1, char operation, double num2)
        {
            switch (operation)
            {
                case '+':
                    _outputTextBox.Text = Convert.ToString(num1 + num2);
                    break;
                case '-':
                    _outputTextBox.Text = Convert.ToString(num1 - num2);
                    break;
                case '*':
                    _outputTextBox.Text = Convert.ToString(num1 * num2);
                    break;
                case '/':
                    if (num2 != 0)
                        _outputTextBox.Text = Convert.ToString(num1 / num2);
                    else
                        _outputTextBox.Text = "Error";
                    break;
                default:
                    _outputTextBox.Text = "Error";
                    break;
            }
        }
    }
}
