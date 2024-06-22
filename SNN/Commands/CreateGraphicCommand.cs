using OxyPlot;
using SNN.Models;
using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNN.Commands
{
    public class CreateGraphicCommand : CommandBase
    {
        private readonly NetworkDynamicsViewModel _networkDynamicsViewModel;

        public CreateGraphicCommand(NetworkDynamicsViewModel networkDynamicsViewModel)
        {
            _networkDynamicsViewModel = networkDynamicsViewModel;
        }

        public override void Execute(object parameter)
        {
            _networkDynamicsViewModel.ListOfPoints.Clear();

            Neuron graphNeuronFirst = null;
            Neuron graphNeuronSecond = null;

            foreach (Neuron neuron in _networkDynamicsViewModel.Neurons)
            {
                if (neuron.ReadyForGraph == false)
                {
                    if (graphNeuronFirst == null)
                    {
                        graphNeuronFirst = neuron;
                    }
                    else if (graphNeuronSecond == null)
                    {
                        if (neuron.ImpulseGenerationList.Count > 0 && graphNeuronFirst.ImpulseGenerationList.Count > 0 &&
                            neuron.ImpulseGenerationList[0] > graphNeuronFirst.ImpulseGenerationList[0])
                        {
                            graphNeuronSecond = neuron;
                        }
                        else
                        {
                            graphNeuronSecond = graphNeuronFirst;
                            graphNeuronFirst = neuron;
                        }
                    }
                }

                if (graphNeuronFirst != null && graphNeuronSecond != null)
                {
                    break;
                }
            }

            if (graphNeuronFirst == null || graphNeuronSecond == null)
            {
                return;
            }

            _networkDynamicsViewModel.GraphNeuronFirst = graphNeuronFirst;
            _networkDynamicsViewModel.GraphNeuronSecond = graphNeuronSecond;
            int count = Math.Min(graphNeuronFirst.ImpulseGenerationList.Count, graphNeuronSecond.ImpulseGenerationList.Count);
            var data_points = new List<PointData>();
            if (graphNeuronSecond.Visualization.Name == "6"  && graphNeuronFirst.Visualization.Name == "1" )
            {
                for (int i = 0; i < count - 1; i++)
                {
                    var y = graphNeuronFirst.ImpulseGenerationList[i + 1] - graphNeuronSecond.ImpulseGenerationList[i];
                    data_points.Add(new PointData { XValue = i, YValue = (double)y });
                    _networkDynamicsViewModel.ListOfPoints = new ObservableCollection<PointData>(data_points);
                    
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    var y = graphNeuronSecond.ImpulseGenerationList[i] - graphNeuronFirst.ImpulseGenerationList[i];
                    data_points.Add(new PointData { XValue = i, YValue = (double)y });
                }

                _networkDynamicsViewModel.ListOfPoints = new ObservableCollection<PointData>(data_points);
            }
           
        }

    }
}
