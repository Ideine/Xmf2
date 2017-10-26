using System;
using System.Windows.Input;

public static class CommandExtensions
{
    public static void TryExecute(this ICommand command, object parameter = null)
    {
        if (command != null && command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }
}

