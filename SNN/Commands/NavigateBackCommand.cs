using SNN.Stores;
using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SNN.Commands
{
    

    public class NavigateBackCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ViewModelBase _viewModel;

        public NavigateBackCommand(NavigationStore navigationStore, ViewModelBase viewModel)
        {
            _navigationStore = navigationStore;
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = _viewModel;
        }
    }

}
