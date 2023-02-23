# ScriptDocumentActivator
An extension to Sql Server Management Studio that creates a named pipe that can be used to create new script documents with a given connection.

# How to use

* Run VS as an administrator
* Build ScriptDocumentActivator (it will build and copy the vsix package into the `c:\Program Files (x86)\Microsoft SQL Server Management Studio 19\Common7\IDE\Extensions\ScriptDocumentActivator`
* Build ScriptDocumentActivatorCli
* Run `ScriptDocumentActivatorCli.exe DbServerAddress MyDbName MyUserName MyPassword`
