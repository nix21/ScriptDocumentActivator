﻿using System;
using System.Data.SqlClient;
using EnvDTE;
using Microsoft.SqlServer.Management.Smo.RegSvrEnum;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using Microsoft.VisualStudio.Shell;

namespace ScriptDocumentActivator.ScriptActivation
{
    public class ActivatorServer : IScriptDocumentActivator
    {
        public void OpenDocument(string serverName, string dbName, string userName, string password)
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate {
                await OpenDocumentAsync(serverName, dbName, userName, password);

                System.Windows.Application.Current.MainWindow.Activate();
            });
        }

        public async System.Threading.Tasks.Task OpenDocumentAsync(string serverName, string dbName, string userName, string password)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                string connectionString = $"Data Source={serverName}; Database={dbName};";
                if (userName != null)
                    connectionString += $"User ID={userName}; Password={password}";
                else
                    connectionString += "Integrated Security=true";

                using (var connection = new SqlConnection(connectionString))
                    await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                ScriptFactory.Instance.CreateNewBlankScript(ScriptType.Sql);

                DTE dte = await ScriptDocumentActivatorPackage.Instance.GetServiceAsync(typeof(DTE)) as DTE;
                if (dte != null)
                {
                    var doc = (TextDocument)dte.Application.ActiveDocument.Object(null);
                    doc.EndPoint.CreateEditPoint().Insert($"Unable to connect to '{dbName}' database on server '{serverName}' with given credentials." + Environment.NewLine + ex.Message);
                }

                //if we are unable to connect to given server, we won't try to open a new script document, because it would fail 
                //with an unrelated cross-thread whatever error (I tried to await to switch to UI thred, invoking through MainWindow.Dispather, but nothing helped...
                return;
            }

            UIConnectionInfo ci = new UIConnectionInfo();
            ci.ApplicationName = "ScriptDocumentActivator";
            ci.ServerName = serverName;
            ci.DisplayName = serverName;
            ci.ServerType = Guid.Parse("{8c91a03d-f9b4-46c0-a305-b5dcc79ff907}");
            ci.AdvancedOptions["DATABASE"] = dbName;
            if (string.IsNullOrEmpty(userName))
            {
                ci.AuthenticationType = (int)SqlAuthenticationMethod.NotSpecified;
                ci.UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            else
            {
                ci.AuthenticationType = (int)SqlAuthenticationMethod.SqlPassword;
                ci.Password = password;
                ci.UserName = userName;
            }
            ScriptFactory.Instance.CreateNewBlankScript(ScriptType.Sql, ci, null);
        }
    }
}
