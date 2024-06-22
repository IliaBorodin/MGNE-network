using SNN.Services;
using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNN.Commands
{
    public class CreateNetworkConfigurationCommand : CommandBase
    {
        private readonly NetworkConfigurationViewModel _networkParams;
       // private readonly NavigationService _networkDynamicsNavigationService;

        public CreateNetworkConfigurationCommand(NetworkConfigurationViewModel networkParams)
        {
            _networkParams = networkParams;
           // _networkDynamicsNavigationService = networkDynamicsNavigationService;
        }
   
        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
