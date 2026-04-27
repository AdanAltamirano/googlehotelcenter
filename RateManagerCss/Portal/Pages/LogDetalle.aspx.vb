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

            ' Caso ELIMINACION: solo hay datos en FIELD_DATOS (antes) y FIELD_DATOSDESPUES
            ' viene vacio (placeholder "Inventario"). Usar el DataSetName de DATOS para
            ' resolver correctamente el XSLT (p.ej. "Tarifas").
            If sname = "Inventario" AndAlso Not String.IsNullOrEmpty(ds.DataSetName) AndAlso ds.DataSetName <> "Inventario" AndAlso ds.DataSetName <> "NewDataSet" Then
                sname = ds.DataSetName
            End If

            fileXsl = ResolveXsltPath(sname)
            If Not String.IsNullOrEmpty(fileXsl) AndAlso System.IO.File.Exists(fileXsl) Then
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

    ''' <summary>
    ''' Resuelve la ruta al archivo XSLT intentando varias ubicaciones en orden:
    '''   1) Ruta relativa a la app (~/Data/xslt/{cultura}/{nombre}.xslt)  → funciona en local y producción
    '''   2) Config "pathRM" (compatibilidad con despliegues existentes)
    '''   3) Cultura "en" como último fallback si la cultura actual no tiene XSLT
    ''' Devuelve la primera ruta que exista, o String.Empty si ninguna existe.
    ''' </summary>
    Private Function ResolveXsltPath(ByVal sname As String) As String
        Dim culture As String = PortalCulture.GetCulture.ToString.Substring(0, 2).ToLower()
        Dim relative As String = String.Format("xslt\{0}\{1}.xslt", culture, sname)
        Dim candidate As String

        ' 1) Ruta relativa a la aplicación (la más portable)
        Try
            candidate = Server.MapPath("~/Data/" & relative)
            If System.IO.File.Exists(candidate) Then Return candidate
        Catch
            ' Server.MapPath puede fallar en contextos especiales; ignorar y seguir
        End Try

        ' 2) Config pathRM (usado en producción)
        Dim configPath As String = ConfigurationManager.AppSettings("pathRM")
        If Not String.IsNullOrEmpty(configPath) Then
            candidate = String.Concat(configPath, relative)
            If System.IO.File.Exists(candidate) Then Return candidate
        End If

        ' 3) Fallback a la cultura "en" si no existe en la cultura actual
        If culture <> "en" Then
            Dim relativeEn As String = String.Format("xslt\en\{0}.xslt", sname)
            Try
                candidate = Server.MapPath("~/Data/" & relativeEn)
                If System.IO.File.Exists(candidate) Then Return candidate
            Catch
            End Try
            If Not String.IsNullOrEmpty(configPath) Then
                candidate = String.Concat(configPath, relativeEn)
                If System.IO.File.Exists(candidate) Then Return candidate
            End If
        End If

        Return String.Empty
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