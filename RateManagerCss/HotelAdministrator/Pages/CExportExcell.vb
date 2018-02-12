Public Class CExportExcell
    Protected mvalueCollection As NameValueCollection  'Valores del xml y parametros.
    Protected mds As DataSet
    Protected mdt As DataTable
    Protected mdv As DataView

    Sub New()
        mvalueCollection = New NameValueCollection
        mds = New DataSet
    End Sub

    Public Sub AddParameter(ByVal name As String, ByVal value As String)
        If mvalueCollection Is Nothing Then
            mvalueCollection = New NameValueCollection
        End If
        mvalueCollection.Add(name, value)
    End Sub

    Public ReadOnly Property GetColumns() As NameValueCollection
        Get
            Return mvalueCollection
        End Get
    End Property

    Public Property DataSource() As DataSet
        Get
            Return mds
        End Get
        Set(ByVal value As DataSet)
            mds = value
        End Set
    End Property

    Public Property DataViewSource() As DataView
        Get
            Return mdv
        End Get
        Set(ByVal value As DataView)
            mdv = value
        End Set
    End Property

    Public Property DataTableSource() As DataTable
        Get
            Return mdt
        End Get
        Set(ByVal value As DataTable)
            mdt = value
        End Set
    End Property

End Class
