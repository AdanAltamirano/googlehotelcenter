Imports System
Imports System.Xml
Imports System.Data
Imports System.IO

Partial Public Class LogDetalle
    Inherits PaginaBase


    Function SetTableName(ByRef dst As DataSet) As Boolean
        For i As Integer = 0 To dst.Tables.Count - 1
            dst.Tables(i).TableName = String.Format("_{0}", dst.Tables(i).TableName)
        Next
    End Function


    Function LeeXml(ByVal dsLog As LogData) As Boolean
        Dim trans As New Xsl.XslCompiledTransform
        Dim oXmlTextReader As XmlTextReader
        Dim writer As New StringWriter
        Dim xdd As XmlDataDocument
        Dim rx As XmlTextReader
        Dim ds As DataSet
        Dim dst As DataSet
        Dim fileXsl As String
        Dim sname As String
        Dim shtml As String
        Dim str2 As String
        Dim str As String
        Dim rowArray(1) As Object


        Try

            If dsLog.Tables(0).Rows(0).IsNull(LogData.FIELD_DATOS) And dsLog.Tables(0).Rows(0).IsNull(LogData.FIELD_DATOSDESPUES) Then
                Literal1.Text = PortalCulture.GetString("01359")   '"NO HAY DETALLE PARA ESTE LOG"
                Return False
            End If

            ds = New DataSet
            dst = New DataSet
            str = "<Inventario> </Inventario>"
            str2 = "<Inventario> </Inventario>"
            If Not dsLog.Tables(0).Rows(0).IsNull(LogData.FIELD_DATOS) Then
                str = dsLog.Tables(0).Rows(0)(LogData.FIELD_DATOS)
            End If
            If Not dsLog.Tables(0).Rows(0).IsNull(LogData.FIELD_DATOSDESPUES) Then
                str2 = dsLog.Tables(0).Rows(0)(LogData.FIELD_DATOSDESPUES)
            End If

            rx = New XmlTextReader(str, XmlNodeType.Document, Nothing)
            ds.ReadXml(rx)

            rx = New XmlTextReader(str2, XmlNodeType.Document, Nothing)
            dst.ReadXml(rx)
            sname = dst.DataSetName
            SetTableName(dst)
            'dst.Tables(0).TableName = String.Format("_{0}", dst.Tables(0).TableName)
            ds.Merge(dst)
            If dsLog.Tables(0).Rows(0).IsNull(LogData.FIELD_DATOS) Then
                ds.DataSetName = dst.DataSetName
            End If

            fileXsl = String.Concat(ConfigurationManager.AppSettings("pathRM"), String.Format("xslt\{0}\{1}.xslt", PortalCulture.GetCulture.ToString.Substring(0, 2), sname))
            'fileXsl = String.Concat("C:\ProyectosNet35\RateManager\RateManager\RateManager\Correos\xslt\es\", String.Format("{0}.xslt", sname))
            If System.IO.File.Exists(fileXsl) Then
                oXmlTextReader = New XmlTextReader(ds.GetXml(), XmlNodeType.Document, Nothing)
                xdd = New XmlDataDocument(ds)
                trans.Load(fileXsl)
                trans.Transform(xdd, Nothing, writer)
                shtml = writer.ToString()
                Literal1.Text = shtml
            Else
                Literal1.Text = PortalCulture.GetString("01360")   '"NO HAY DETALLE PARA ESTE LOG, CONSULTE CON EL ADMINISTRADOR (XSLT)"
            End If
        Catch ex As Exception
            Literal1.Text = PortalCulture.GetString("01361")   '"NO HAY DETALLE PARA ESTE LOG, CONSULTE CON EL ADMINISTRADOR"
        End Try
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
            Dim ds As LogData
            Dim idLog As Integer
            If Not String.IsNullOrEmpty(Me.Request.QueryString("Redirect")) Then

            End If
            Integer.TryParse(Request.QueryString("id"), idLog)
            'idLog = 81451
            ds = (New LogFacade).GetLogByID(idLog)
            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                LeeXml(ds)
            End If
        End If
    End Sub

    Private Sub LogDetalle_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        lblTitle.Text = PortalCulture.GetString("01363")
        LinkButton1.Text = PortalCulture.GetString("01362")

        LinkButton1.OnClientClick = String.Concat("javascript: history.back(); return false;")
    End Sub

    Private Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        'Me.redirectTo(PaginaBase.pages.Logs)
    End Sub
End Class