using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SNN.Commands
{
    public class StartNetwork : CommandBase
    {
        private readonly NetworkDynamicsViewModel _networkDynamicsViewModel;

        public StartNetwork(NetworkDynamicsViewModel networkDynamicsViewModel)
        {
            _networkDynamicsViewModel = networkDynamicsViewModel;
        }


        public override void Execute(object parameter)
        {
            if(_networkDynamicsViewModel.Flag == false)
            {
                _networkDynamicsViewModel.TextBtn = "Пауза";
                _networkDynamicsViewModel.Flag = true;
                _networkDynamicsViewModel.StartDynamics();
            }
            else if(_networkDynamicsViewModel.Flag == true)
            {
                _networkDynamicsViewModel.TextBtn = "Старт";
                _networkDynamicsViewModel.Flag = false;
            }
            

        }



    }
}
