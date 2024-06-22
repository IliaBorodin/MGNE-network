using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SNN.Commands
{
    public abstract class CommandBase : ICommand
    {
        // Уведомляет об изменениях в возможности выполнения команды
        public event EventHandler CanExecuteChanged;

        //Метод, отвечающий за возможность выполения команды
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }
        //Конкретные действия обработчика
        public abstract void Execute(object parameter);
        //Вызывает CanExecuteChanged
        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

    }
}
