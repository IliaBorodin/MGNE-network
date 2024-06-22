using SNN.Stores;
using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Класс позволяет осуществлять навигацию
namespace SNN.Services
{
    public class NavigationService<TViewModel>
      where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<object, ViewModelBase> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<object, ViewModelBase> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate(object parameter)
        {
            _navigationStore.CurrentViewModel = _createViewModel(parameter);
        }
    }

}
