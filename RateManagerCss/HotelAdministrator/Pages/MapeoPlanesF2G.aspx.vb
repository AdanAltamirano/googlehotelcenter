Imports System.Data.SqlClient
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.Hotel.DataAccess
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade

Public Class MapeoPlanesF2G
    Inherits PaginaBase

    Private dsCommand As SqlDataAdapter

    Private insertCommand As SqlCommand
    Private updateCommand As SqlCommand
    Private loadCommand As SqlCommand
    Private deleteCommand As New SqlCommand

    Private Property hotelId() As Integer
        Get
            Return ViewState("hotelId")
        End Get
        Set(ByVal Value As Integer)
            ViewState("hotelId") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then

            CargarPlanes()
            hotelId = cInfoActual.Hotel
            If Not Session("idCorporativoUserChain") Is Nothing AndAlso Session("idCorporativoUserChain") <> "-1" Then
                Dim ds As DataSet
                'With (New Hoteles)
                '    ds = .LoadHotelsByIdCorp(Session("idCorporativoUserChain"))
                'End With
                With (New Hoteles)
                    ds = .LoadHotelsByUser(Session("idUsuario"))
                End With
                renglonEtiquetas.Style.Add("display", "none")
                renglonDatos.Style.Add("display", "none")
                renglonBotones.Style.Add("display", "none")
                lblHotelSelected.Style.Add("display", "none")
                lblHotelSelected.Text = ""
            End If
        End If
    End Sub

    Private Sub btnCargar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargar.Click
        CargarPlanes()
    End Sub

    Private Sub CargarPlanes()
        lblMsgActualizacion.Visible = False
        If True Then 'ddlHoteles.SelectedValue <> -1 Then
            'Buscamos los ratesPlan del hotel
            Dim ds As DataSet

            ds = LoadPlansF2G(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 0, 1)
            dgRatesPlan.DataSource = ds
            dgRatesPlan.DataKeyField = "idRatePlan"
            dgRatesPlan.DataBind()

            Dim dsRooms As DataSet
            With New RoomAccess
                dsRooms = .LoadRoomsPMS(Me.hotelId, PortalCulture.GetIDCulture)
            End With

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
                Dim txtPromoCodePms As TextBox
                'Buscar TextBox en la fila del datagrid
                txtidRatePlanPMS = row.FindControl("txtidRatePlanF2G")
                txtPromoCodePms = row.FindControl("txtIdPromoCodePMS")

                If Not txtidRatePlanPMS Is Nothing AndAlso Not txtPromoCodePms Is Nothing Then
                    Dim idRatePlan As String = row.Cells(0).Text
                    'Param HotelId
                    'Param IdRatePlanUv
                    'Param IdRatePlanF2g
                    'Param PromoCode
                    UpdateRatesPlanF2G(Me.hotelId, idRatePlan, txtidRatePlanPMS.Text.Trim, txtPromoCodePms.Text.Trim)

                End If
            Next
            lblMsgActualizacion.Visible = True
        End If
    End Sub

    Private Sub dgRatesPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRatesPlan.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            Dim txtidRatePlanPMS As TextBox
            Dim txtPromoCodePMS As TextBox
            txtidRatePlanPMS = e.Item.Cells(3).FindControl("txtidRatePlanF2G")
            txtPromoCodePMS = e.Item.Cells(5).FindControl("txtIdPromoCodePMS")
            If Not txtidRatePlanPMS Is Nothing Then
                txtidRatePlanPMS.Text = String.Empty
                txtidRatePlanPMS.Text = IIf(e.Item.Cells(2).Text = "&nbsp;", "", e.Item.Cells(2).Text)
            End If
            If Not txtPromoCodePMS Is Nothing Then
                txtPromoCodePMS.Text = String.Empty
                txtPromoCodePMS.Text = IIf(e.Item.Cells(4).Text = "&nbsp;", "", e.Item.Cells(4).Text)
            End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = "Codigo"
            e.Item.Cells(1).Text = "Nombre"
            e.Item.Cells(3).Text = "Código F2G"
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        cancelar()
    End Sub
    Private Sub cancelar()
        renglonDatos.Style.Add("display", "none")
        renglonEtiquetas.Style.Add("display", "none")
        renglonBotones.Style.Add("display", "none")
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
        lblMsgActualizacion.Text = PortalCulture.GetString("01151", False)
        btnAceptar.Text = PortalCulture.GetString("M000060", False)
        btnCancel.Text = PortalCulture.GetString("M000143", False)
        'lblTitle.Text = PortalCulture.GetString("01152", False)
    End Sub

    Private Function UpdateRatesPlanF2G(ByVal idHotel As Integer, ByVal idRatePlanUv As String, ByVal idRatePlanF2G As String, ByVal promoCodePms As String) As Boolean
        Dim conStr As String = ConfigurationSettings.AppSettings("HotelConnection")
        Dim sqlCon As SqlConnection = New SqlConnection(conStr)
        Dim sqlCommand As SqlCommand = New SqlCommand("spUpdF2GRatePlan", sqlCon)
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Parameters.Add("@idRatePlanUv", SqlDbType.NVarChar).Value = idRatePlanUv
        sqlCommand.Parameters.Add("@idHotel", SqlDbType.Int).Value = idHotel
        sqlCommand.Parameters.Add("@idRatePlanF2G", SqlDbType.NVarChar).Value = idRatePlanF2G
        sqlCommand.Parameters.Add("@promoCodePms", SqlDbType.NVarChar).Value = promoCodePms
        Try
            sqlCommand.Connection.Open()
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
            Return False
        Finally
            If Not sqlCommand Is Nothing Then
                If Not sqlCommand.Connection Is Nothing Then
                    sqlCommand.Connection.Close()
                    sqlCommand.Connection.Dispose()
                End If
            End If
            sqlCommand.Dispose()
        End Try
        Return True
    End Function

    'TODO: Move To Portal Facade dll
    Private Function LoadPlansF2G(ByVal idHotel As Integer, ByVal idioma As Integer, ByVal IncluirPaquetesSegmentoK As Integer, ByVal incluirRatePlanNR As Integer) As DataSet

        Dim data As New DataSet
        dsCommand = New SqlDataAdapter

        With dsCommand

            If loadCommand Is Nothing Then
                loadCommand = New SqlCommand("spGetRatesPlansF2G", New SqlConnection(ConfigurationSettings.AppSettings("HotelConnection")))
                loadCommand.CommandType = CommandType.StoredProcedure
                loadCommand.Parameters.Add(New SqlParameter("@idHotel", SqlDbType.Int))
                loadCommand.Parameters.Add(New SqlParameter("@idioma", SqlDbType.Int))
                loadCommand.Parameters.Add(New SqlParameter("@incluirPaquetesSegK", SqlDbType.Int))
                loadCommand.Parameters.Add(New SqlParameter("@incluirNetRatesPlan", SqlDbType.Int))
            End If

            Try
                .SelectCommand = loadCommand
                .SelectCommand.Parameters("@idHotel").Value = idHotel
                .SelectCommand.Parameters("@idioma").Value = idioma
                .SelectCommand.Parameters("@incluirPaquetesSegK").Value = IncluirPaquetesSegmentoK
                .SelectCommand.Parameters("@incluirNetRatesPlan").Value = incluirRatePlanNR
                .Fill(data)
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()
            End Try
        End With
        LoadPlansF2G = data
    End Function
End Class