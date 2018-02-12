Imports System.Configuration.ConfigurationManager

Module Config

    Public ReadOnly Property RutaLogs() As String
        Get
            Return AppSettings("RutaLogs")
        End Get
    End Property

    Public ReadOnly Property ConexionSQL() As String
        Get
            Return AppSettings("ConexionWizcom")
        End Get
    End Property

    Public ReadOnly Property PrefijoLogs() As String
        Get
            Return AppSettings("PrefijoLogs")
        End Get
    End Property

End Module
