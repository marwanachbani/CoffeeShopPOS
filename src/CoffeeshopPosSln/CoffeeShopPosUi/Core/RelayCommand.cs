using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeShopPosUi.Core
{
   public class RelayCommand : ICommand 
   {
   private readonly Func<object, Task> _executeAsync; private readonly Predicate<object> _canExecute; public RelayCommand(Func<object, Task> executeAsync) : this(executeAsync, null) { } public RelayCommand(Func<object, Task> executeAsync, Predicate<object> canExecute) { _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync)); _canExecute = canExecute; } public bool CanExecute(object parameter) { return _canExecute == null || _canExecute(parameter); } public async void Execute(object parameter) { await _executeAsync(parameter); } public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
    }}