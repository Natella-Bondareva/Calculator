using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Calculator
{
    public interface ICommand
    {
        void Execute();
    }

    public class ClearEntryCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        TextBox _outputTextBox;
        private readonly List<string> _history = new List<string>();

        public ClearEntryCommand(ref List<string> history,TextBox outputTextBox)
        {
             _history = history;
            _outputTextBox = outputTextBox;
        }

        public void Execute()
        {
            if (_history.Count > 0)
            {
                var lastCommand = _history[_history.Count - 1];
                _history.RemoveAt(_history.Count - 1);
                _outputTextBox.Text = _history[_history.Count - 1].ToString();
            }
        }
    }

    public class WritePiCommand : ICommand 
    {
        private TextBox _outputTextBox;

        public WritePiCommand(TextBox outputTextBox)
        {
            _outputTextBox = outputTextBox;
        }

        public void Execute()
        {
            if (string.IsNullOrEmpty(_outputTextBox.Text))
            {
                _outputTextBox.Text += Math.PI.ToString();
            }
            else
            {
                char lastChar = _outputTextBox.Text[_outputTextBox.Text.Length - 1];
                if (lastChar == '+' || lastChar == '-' || lastChar == '*' || lastChar == '/' || lastChar == '^')
                {
                    _outputTextBox.Text += Math.PI.ToString();
                }
            }
        }
    }

    class SquareRootCommand : ICommand
    {
        private ReciverCalculator _calculator;

        public SquareRootCommand(ReciverCalculator calculator)
        {
            _calculator = calculator;
        }

        public void Execute()
        {
            _calculator.FindSquareRoot();
        }
    }

    class LogCommand : ICommand
    {
        private ReciverCalculator _calculator;

        public LogCommand(ReciverCalculator calculator)
        {
            _calculator = calculator;
        }

        public void Execute()
        {
            _calculator.FindLog();
        }
    }
    class ElevationToSquareCommand : ICommand
    {
        private ReciverCalculator _calculator;

        public ElevationToSquareCommand(ReciverCalculator calculator)
        {
            _calculator = calculator;
        }

        public void Execute()
        {
            _calculator.ElevationToSquare();
        }
    }
    public class EvaluateExpressionCommand : ICommand
    {

        private ReciverCalculator _calculator;

        public EvaluateExpressionCommand(ReciverCalculator calculator)
        {
            _calculator = calculator;
        }

        public void Execute()
        {
            _calculator.EvaluateExpression();
        }
    }

    class DeleteOneCharacterCommand : ICommand
    {
        private ReciverCalculator _calculator;

        public DeleteOneCharacterCommand(ReciverCalculator calculator)
        {
            _calculator = calculator;
        }

        public void Execute()
        {
            _calculator.Backspace();
        }
    }
    class ClearAllCommand : ICommand
    {
        private ReciverCalculator _calculator;

        public ClearAllCommand(ReciverCalculator calculator)
        {
            _calculator = calculator;
        }

        public void Execute()
        {
            _calculator.Clear();
        }
    }
    class AddCharacterCommand : ICommand
    {
        private ReciverCalculator _calculator;
        string _character;

        public AddCharacterCommand(ReciverCalculator calculator, string character)
        {
            _calculator = calculator;
            _character = character;
        }

        public void Execute()
        {
            _calculator.Add(_character);
        }
    }
}
