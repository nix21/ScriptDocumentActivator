using System.ServiceModel;

namespace ScriptDocumentActivator
{
    [ServiceContract]
    public interface IScriptDocumentActivator
    {
        [OperationContract]
        void OpenDocument(string serverName, string dbName, string userName, string password);
    }
}
