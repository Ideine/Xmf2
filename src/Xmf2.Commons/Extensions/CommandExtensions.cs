using System;
using System.Windows.Input;

//TODO: revoir les namespaces des différentes classes d'extensions
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

