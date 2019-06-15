using System;
using System.Windows.Input;

namespace GetOsuFile
{
    public class PathCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action Action;

        public PathCommand(Action func)
        { 
            Action = func;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Action();
        }
    }
}
