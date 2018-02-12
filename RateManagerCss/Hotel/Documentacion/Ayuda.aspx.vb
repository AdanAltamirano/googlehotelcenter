Imports System.IO
Partial Class Ayuda
    Inherits System.Web.UI.Page

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
    End Sub

    Private Sub BajaPDF(ByVal FileName As String)
        Try
            'FileName es la ruta del archivo con el nombre del archivo y su extension..
            Dim fileStream As New FileStream(FileName, FileMode.Open)
            Dim fileSize As Long = fileStream.Length
            Dim inta As Integer = CInt(fileSize)

            Context.Response.ContentType = "application/octet-stream"
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8))
            Context.Response.AddHeader("Content-Length", fileSize.ToString())
            Dim fileBuffer(inta) As Byte
            fileStream.Read(fileBuffer, 0, inta)
            fileStream.Close()
            Context.Response.BinaryWrite(fileBuffer)
            Context.Response.End()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnguiarapida_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnguiarapida.Click
        BajaPDF(Request.PhysicalApplicationPath & "/Documentacion/CRS-guia_Rapida_" & PortalCulture.GetCulture.Name.Substring(0, 2) & ".pdf")
    End Sub

    Private Sub btnmanual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmanual.Click
        BajaPDF(Request.PhysicalApplicationPath & "/Documentacion/CRS_ratemanager_basico_" & PortalCulture.GetCulture.Name.Substring(0, 2) & ".pdf")
    End Sub

    Private Sub btnmanualavanzado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmanualavanzado.Click
        BajaPDF(Request.PhysicalApplicationPath & "/Documentacion/CRS_ratemanager_avanzado_" & PortalCulture.GetCulture.Name.Substring(0, 2) & ".pdf")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("00565")
        lblsubtitle.InnerHtml = PortalCulture.GetString("00563")
        lblpaso1.InnerHtml = PortalCulture.GetString("00564")
        btnguiarapida.Text = PortalCulture.GetString("00566")
        lblpaso2.InnerHtml = PortalCulture.GetString("00567")
        btnmanual.Text = PortalCulture.GetString("00568")
        lblpaso3.InnerHtml = PortalCulture.GetString("00569")
        btnmanualavanzado.Text = PortalCulture.GetString("00571")
        lblRequired.InnerHtml = PortalCulture.GetString("00570")
        lblpaso4.InnerHtml = PortalCulture.GetString("00572")
        hplABCReservas.Text = PortalCulture.GetString("00573")
        lblcontactanos.InnerHtml = PortalCulture.GetString("00574")
        lblpaso5.InnerHtml = PortalCulture.GetString("00805")
        lblpaso6.InnerHtml = PortalCulture.GetString("00807")
        Me.btnconciliacion.Text = PortalCulture.GetString("00806")
        Me.btnPromociones.Text = PortalCulture.GetString("00808")
        If PortalCulture.GetCulture.ToString = "en-US" Then
            Me.lblpaso5.Visible = False
            Me.lblpaso6.Visible = False
            Me.btnconciliacion.Visible = False
            Me.btnPromociones.Visible = False
        End If
    End Sub

    Private Sub btnPromociones_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPromociones.Click
        BajaPDF(Request.PhysicalApplicationPath & "/Documentacion/promocionesypaquetes_" & PortalCulture.GetCulture.Name.Substring(0, 2) & ".pdf")
    End Sub

    Private Sub btnconciliacion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnconciliacion.Click
        BajaPDF(Request.PhysicalApplicationPath & "/Documentacion/ConciliacionYPagos_" & PortalCulture.GetCulture.Name.Substring(0, 2) & ".pdf")
    End Sub
End Class
