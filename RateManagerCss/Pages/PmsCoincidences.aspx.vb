Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.Hotel.DataAccess
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade



Partial Class PmsCoincidences
    Inherits PaginaBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Property hotelId() As Integer
        Get
            Return viewstate("hotelId")
        End Get
        Set(ByVal Value As Integer)
            viewstate("hotelId") = Value
        End Set
    End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then
            If Not Session("idCorporativoUserChain") Is Nothing AndAlso Session("idCorporativoUserChain") <> "-1" Then
                Dim ds As DataSet
                'With (New Hoteles)
                '    ds = .LoadHotelsByIdCorp(Session("idCorporativoUserChain"))
                'End With
                With (New Hoteles)
                    ds = .LoadHotelsByUser(Session("idUsuario"))
                End With
                If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count Then
                    ddlHoteles.DataSource = ds.Tables(0).DefaultView
                    ddlHoteles.DataTextField = "nombre"
                    ddlHoteles.DataValueField = "idHotel"
                    ddlHoteles.DataBind()
                End If
                Dim item As ListItem = New ListItem
                item.Text = PortalCulture.GetString("M000272", False)
                item.Value = -1
                ddlHoteles.Items.Insert(0, item)
                renglonEtiquetas.Style.Add("display", "none")
                renglonDatos.Style.Add("display", "none")
                renglonBotones.Style.Add("display", "none")
                lblHotelSelected.Style.Add("display", "none")
                lblHotelSelected.Text = ""
            End If
        End If
    End Sub

    Private Sub btnCargar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargar.Click
        lblMsgActualizacion.Visible = False
        If ddlHoteles.SelectedValue <> -1 Then
            'Buscamos los ratesPlan del hotel
            Me.hotelId = ddlHoteles.SelectedValue
            lblHotelSelected.Text = ddlHoteles.SelectedItem.Text
            lblHotelSelected.Style.Add("display", "block")
            Dim ds As DataSet
            With New RatePlanAccess
                ds = .LoadRatesPlanPMS(Me.hotelId, PortalCulture.GetIDCulture, 1, 1)
            End With
            dgRatesPlan.DataSource = ds
            dgRatesPlan.DataKeyField = "idRatePlan"
            dgRatesPlan.DataBind()

            Dim dsRooms As DataSet
            With New RoomAccess
                dsRooms = .LoadRoomsPMS(Me.hotelId, PortalCulture.GetIDCulture)
            End With
            dgRooms.DataSource = dsRooms
            dgRooms.DataBind()

            renglonEtiquetas.Style.Add("display", "block")
            renglonDatos.Style.Add("display", "block")
            renglonBotones.Style.Add("display", "block")
            Dim ban As Boolean = False

            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                dgRatesPlan.Visible = True
                msgRatesPlan.Visible = False
                ban = True
            Else
                dgRatesPlan.Visible = False
                msgRatesPlan.Visible = True
            End If
            If Not dsRooms Is Nothing AndAlso dsRooms.Tables(0).Rows.Count > 0 Then
                dgRooms.Visible = True
                msgRooms.Visible = False
                ban = True
            Else
                dgRooms.Visible = False
                msgRooms.Visible = True
            End If
            If Not ban Then
                renglonBotones.Style.Add("display", "none")
            End If
        Else
            renglonEtiquetas.Style.Add("display", "none")
            renglonDatos.Style.Add("display", "none")
            renglonBotones.Style.Add("display", "none")
            lblHotelSelected.Text = ""
            lblHotelSelected.Style.Add("display", "none")
            Me.hotelId = -1
        End If
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Call guardar()
    End Sub
    Private Sub guardar()
        If Me.hotelId <> -1 Then
            'Actualizamos los idratesplan del PMS
            For Each row As DataGridItem In dgRatesPlan.Items
                Dim txtidRatePlanPMS As TextBox
                txtidRatePlanPMS = row.FindControl("txtidRatePlanPMS")
                If Not txtidRatePlanPMS Is Nothing Then
                    Dim idRatePlan As String = row.Cells(0).Text
                    With New RatePlanAccess
                        .UpdRatesPlanPMS(Me.hotelId, idRatePlan, txtidRatePlanPMS.Text.Trim)
                    End With
                End If
            Next
            'Actualizamos los roomsCodes del PMS
            For Each row As DataGridItem In dgRooms.Items
                Dim txtCodigoHabitacionPMS As TextBox
                txtCodigoHabitacionPMS = row.FindControl("txtCodigoHabitacionPMS")
                If Not txtCodigoHabitacionPMS Is Nothing Then
                    Dim CodigoHabitacion As String = row.Cells(0).Text
                    With New RoomAccess
                        .UpdRoomsPMS(Me.hotelId, CodigoHabitacion, txtCodigoHabitacionPMS.Text.Trim)
                    End With
                End If
            Next
            'cancelar()
            lblMsgActualizacion.Visible = True
        End If
    End Sub

    Private Sub dgRatesPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRatesPlan.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            Dim txtidRatePlanPMS As TextBox
            txtidRatePlanPMS = e.Item.Cells(3).FindControl("txtidRatePlanPMS")
            If Not txtidRatePlanPMS Is Nothing Then
                txtidRatePlanPMS.Text = IIf(e.Item.Cells(2).Text = "&nbsp;", "", e.Item.Cells(2).Text)
            End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = "Codigo"
            e.Item.Cells(1).Text = "Nombre"
            e.Item.Cells(3).Text = "Hotel"
        End If
    End Sub

    Private Sub dgRooms_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRooms.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            Dim txtCodigoHabitacionPMS As TextBox
            txtCodigoHabitacionPMS = e.Item.Cells(3).FindControl("txtCodigoHabitacionPMS")
            If Not txtCodigoHabitacionPMS Is Nothing Then
                txtCodigoHabitacionPMS.Text = IIf(e.Item.Cells(2).Text = "&nbsp;", "", e.Item.Cells(2).Text)
            End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = "Codigo"
            e.Item.Cells(1).Text = "Nombre"
            e.Item.Cells(3).Text = "Hotel"
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        cancelar()
    End Sub
    Private Sub cancelar()
        renglonDatos.Style.Add("display", "none")
        renglonEtiquetas.Style.Add("display", "none")
        renglonBotones.Style.Add("display", "none")
        ddlHoteles.SelectedIndex = 0
        Me.hotelId = -1
        lblHotelSelected.Text = ""
        lblHotelSelected.Style.Add("display", "none")
        lblMsgActualizacion.Visible = False
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Label1.Text = PortalCulture.GetString("M0BT0000150", False)
        btnCargar.Text = PortalCulture.GetString("00149", False)
        lblRatesPlan.Text = PortalCulture.GetString("00047", False)
        lblHoteles.Text = PortalCulture.GetString("00048", False)
        msgRatesPlan.Text = PortalCulture.GetString("01149", False)
        msgRooms.Text = PortalCulture.GetString("01150", False)
        lblMsgActualizacion.Text = PortalCulture.GetString("01151", False)
        btnAceptar.Text = PortalCulture.GetString("M000060", False)
        btnCancel.Text = PortalCulture.GetString("M000143", False)
        lblTitle.Text = PortalCulture.GetString("01152", False)
    End Sub
End Class
