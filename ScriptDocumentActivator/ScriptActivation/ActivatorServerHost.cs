using System;
using System.ServiceModel;

namespace ScriptDocumentActivator.ScriptActivation
{
    internal class ActivatorServerHost
    {
        private static ServiceHost _serviceHost;

        private static DateTime _initializationTime;

        private static Exception _initializationError;

        public static void Initialize()
        {
            try
            {
                _serviceHost = new ServiceHost(typeof(ActivatorServer), new Uri(ActivatorProxy.NamedPipeBaseUrl));
                _serviceHost.AddServiceEndpoint(typeof(IScriptDocumentActivator), new NetNamedPipeBinding(), ActivatorProxy.ApplicationKey);
                _serviceHost.Open();
                _initializationTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                _initializationError = ex;
                _serviceHost = null;
            }
        }

        public static string GetHealthStatus()
        {
            if (_serviceHost !=null)
                return $"I am healthy since {_initializationTime:HH:mm:ss}";

            if (_initializationError != null)
                return "I am not healthy at all: " + _initializationError.Message;

            return "I am not healthy and I do not know why!";
        }
    }
}
