using System;
using System.ServiceModel;

namespace ScriptDocumentActivator
{
    public class ActivatorProxy: IScriptDocumentActivator, IDisposable
    {
        public const string NamedPipeBaseUrl = "net.pipe://localhost/ScriptDocumentActivator";

        public static string ApplicationKey => @"ScriptDocumentActivator";

        private readonly ChannelFactory<IScriptDocumentActivator> _pipeFactory;

        private bool _isDisposed = false;

        public ActivatorProxy()
        {
            string endPointUri = NamedPipeBaseUrl + "/" + ApplicationKey;
            _pipeFactory = new ChannelFactory<IScriptDocumentActivator>(new NetNamedPipeBinding(), new EndpointAddress(endPointUri));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                    _pipeFactory.Close();

                _isDisposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

        public void OpenDocument(string serverName, string dbName, string userName, string password)
        {
            IScriptDocumentActivator channel = _pipeFactory.CreateChannel();
            channel.OpenDocument(serverName, dbName, userName, password);
        }
    }
}
