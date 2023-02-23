using System.ServiceModel;

namespace ScriptDocumentActivator.ScriptActivation
{
    [ServiceContract]
    public interface IScriptDocumentActivator
    {
        [OperationContract]
        void OpenDocument(string serverName, string dbName, string userName, string password);
    }
}
