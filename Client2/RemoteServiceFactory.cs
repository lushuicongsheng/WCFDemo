using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

namespace Client2
{
    public class RemoteServiceFactory 
    {
        //need to get it from a config file after implemented a dynamic config manage module
        private readonly string serviceUri = "http://127.0.0.1:9999/calculatorservice";

        public ICalculator CreateService()
        {
            return this.CreateService<ICalculator>(serviceUri);
        }

        private T CreateService<T>(string uri)
        {
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = maxReceivedMessageSize;
            binding.ReaderQuotas = new XmlDictionaryReaderQuotas();
            binding.ReaderQuotas.MaxStringContentLength = maxReceivedMessageSize;
            binding.ReaderQuotas.MaxArrayLength = maxReceivedMessageSize;
            binding.ReaderQuotas.MaxBytesPerRead = maxReceivedMessageSize;

            ChannelFactory<T> chan = new ChannelFactory<T>(binding, new EndpointAddress(uri));
            chan.Endpoint.Behaviors.Add(new InspectorBehavior());
            foreach (var op in chan.Endpoint.Contract.Operations)
            {
                var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>() as DataContractSerializerOperationBehavior;
                if (dataContractBehavior != null)
                    dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
            }
            chan.Open();
            var service = chan.CreateChannel();
            return service;
        }

        private const int maxReceivedMessageSize = 2147483647;
    }
}
