Public Class UserControlBase
    Inherits UserControl

    Private _dataSource As Object

    Public Shared Sub BindList(ByVal list As BaseDataList, ByVal dataSource As Object, Optional ByVal dataMember As String = "")
        If Not list Is Nothing Then
            list.DataSource = dataSource
            list.DataMember = IIf(Not dataSource Is Nothing, dataMember, String.Empty)
            list.DataBind()
        End If
    End Sub

    Public Property DataSource() As Object
        Get
            Return _dataSource
        End Get
        Set(ByVal Value As Object)
            _dataSource = Value
        End Set
    End Property

    Protected Property CurrentOrder(ByVal dataContainer As String, ByVal sortExpression As String) As String
        Get
            If Session(String.Format("{0}:{1}:{2}", Me.GetType().Name, dataContainer, sortExpression)) Is Nothing Then
                Return String.Empty
            End If

            Return CStr(Session(String.Format("{0}:{1}:{2}", Me.GetType().Name, dataContainer, sortExpression)))
        End Get
        Set(ByVal Value As String)
            Session(String.Format("{0}:{1}:{2}", Me.GetType().Name, dataContainer, sortExpression)) = Value
        End Set
    End Property


    Public Function GeRequestApplicationPath(ByVal page As String) As String
        Dim sreq As String
        sreq = String.Concat(Request.ApplicationPath, page).Replace("//", "/").Replace("//", "/")
        Return sreq
    End Function

    Function FCurrency(ByVal src As Double, ByVal Decimales As Byte) As String
        Dim sfrm As String = ""
        Dim tmpCur As String
        Dim sCur As String

        Try
            For i As Integer = 1 To Decimales
                sfrm = sfrm.Insert(0, "0")
            Next
            If Not String.IsNullOrEmpty(sfrm) Then sfrm = "." & sfrm

            tmpCur = String.Format("###,###,##0{0}", sfrm)
            sCur = src.ToString(tmpCur)
        Catch ex As Exception
            sCur = src.ToString()
        End Try
        Return sCur
    End Function

    Function GetIdAsociation()
        Dim IdAsociation As Integer = -1
        Try
            If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
            IdAsociation = If(IdAsociation = 0, -1, IdAsociation)
        Catch ex As Exception
        End Try
        Return IdAsociation
    End Function

    Public Property EnConsulta() As Byte
        Get
            Return HttpContext.Current.Session("_EnConsulta_Session")
        End Get
        Set(ByVal value As Byte)
            HttpContext.Current.Session("_EnConsulta_Session") = value
        End Set
    End Property

End Class
