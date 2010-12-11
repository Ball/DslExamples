using System;
using System.Windows.Input;
using DelimiterDirected;

namespace TaskPaperClone.ViewModel
{
    public class InterpreteCommand : ICommand
    {
        private readonly ViewModel _vm;

        public InterpreteCommand(ViewModel viewModel)
        {
            _vm = viewModel;
        }

        public void Execute(object parameter)
        {
            var i = parameter is int ? (int) parameter : -1;
            var interpreter = _vm.Interpreters[i];
            _vm.Projects.ClearAndAddRange(interpreter.Interpret(_vm.DslText));
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
 }