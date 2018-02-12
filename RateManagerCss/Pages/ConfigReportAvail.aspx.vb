Imports Portal.Hotel.Facade


Partial Class ConfigReportAvail
    Inherits PaginaBase

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
    Private Sub FillData()
        Dim ds As ReportsConfiguration
        

        With New ReportsConfigurationFacade
            ds = .GetConfiguration(Me.cInfoActual.Hotel)
        End With
        If Not ds Is Nothing AndAlso ds.Tables(ds.ConfigurationTable).Rows.Count > 0 Then
            With ds.Tables(ds.ConfigurationTable).Rows(0)
                Me.txtEmails.Text = .Item(ds.field_Emails)
                ddlGenerarTime.SelectedValue = .Item(ds.field_DaysGenerate)
                ddlDisplayTime.SelectedValue = .Item(ds.field_DaysDisplay)
                Me.ddlGenerarType.SelectedValue = .Item(ds.field_UnitTypeGenerate).ToString
                Me.ddlDisplayType.SelectedValue = .Item(ds.field_UnitTypeDisplay).ToString
                Me.chkSendMail.Checked = .Item(ds.field_SendEmailHotel)
            End With
        End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds As New ReportsConfiguration
        Dim dr As DataRow
        dr = ds.Tables(ds.ConfigurationTable).NewRow
        dr(ds.field_idHotel) = Me.cInfoActual.Hotel
        dr(ds.field_Emails) = Me.txtEmails.Text.Trim
        dr(ds.field_DaysDisplay) = Me.ddlDisplayTime.SelectedValue
        dr(ds.field_DaysGenerate) = Me.ddlGenerarTime.SelectedValue
        dr(ds.field_SendEmailHotel) = Me.chkSendMail.Checked
        dr(ds.field_GenerateDate) = Now.AddMinutes(-1)
        dr(ds.field_UnitTypeDisplay) = Me.ddlDisplayType.SelectedValue
        dr(ds.field_UnitTypeGenerate) = Me.ddlGenerarType.SelectedValue
        ds.Tables(ds.ConfigurationTable).Rows.Add(dr)
        With New ReportsConfigurationFacade
            If .ConfigurationUpdate(ds) Then
                Me.guardalog("/Pages/ConfigReportAvail.aspx", PaginaBase.acciones.Modificar, "Se modificó la configuracion de los reportes de disponibilidad para el hotel " & Me.cInfoActual.HotelName)
            End If
        End With
    End Sub


    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        FillData()
        Me.ddlDisplayType.Items(0).Text = PortalCulture.GetString("00533")
        Me.ddlDisplayType.Items(1).Text = PortalCulture.GetString("00534")
        Me.ddlDisplayType.Items(2).Text = PortalCulture.GetString("00535")
        Me.ddlDisplayType.Items(3).Text = PortalCulture.GetString("00536")
        Me.ddlGenerarType.Items(0).Text = PortalCulture.GetString("00533")
        Me.ddlGenerarType.Items(1).Text = PortalCulture.GetString("00534")
        Me.ddlGenerarType.Items(2).Text = PortalCulture.GetString("00535")
        Me.ddlGenerarType.Items(3).Text = PortalCulture.GetString("00536")
        lblTitulo.Text = PortalCulture.GetString("00537")
        lblEmails.Text = PortalCulture.GetString("00538", True)
        chkSendMail.Text = PortalCulture.GetString("00539")
        lblGenerar.Text = PortalCulture.GetString("00540", True)
        lblDisplay.Text = PortalCulture.GetString("00541", True)
    End Sub

    Private Sub btnEjecutar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEjecutar.Click
        Dim dt As DataTable
        btnSave_Click(Nothing, Nothing)
        'With New clsGetAvail
        '    .GenerateReportAvail(Me.cInfoActual.Hotel, Me.cInfoActual.HotelName, Me.cInfoActual.Empresa)
        'End With



    End Sub
End Class
