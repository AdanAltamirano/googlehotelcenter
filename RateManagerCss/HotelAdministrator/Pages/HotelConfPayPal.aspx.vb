Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Text.RegularExpressions

Partial Class HotelConfPayPal
    Inherits PaginaBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents CtlMensajes1 As ctlMensajes
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Const SPGETHOTELS As String = "spGetHotels"

    Private Property i_idHotel() As Integer
        Get
            Return viewstate("idHotel")
        End Get
        Set(ByVal Value As Integer)
            viewstate("idHotel") = Value
        End Set
    End Property

    Private Property i_idChannel() As Integer
        Get
            Return viewstate("idChannel")
        End Get
        Set(ByVal Value As Integer)
            viewstate("idChannel") = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then
            Limpiar()
            LoadHotels()
            loadChannels()
        End If
        Me.ResizefrmPrincipal()
    End Sub
    Private Sub loadChannels()
        Dim item As ListItem = New ListItem("Portal", 1)
        ddlChannel.Items.Add(item)
        item = Nothing
        item = New ListItem(PortalCulture.GetString("00457", False), 2)
        ddlChannel.Items.Add(item)
        item = Nothing
        item = New ListItem(PortalCulture.GetString("M000640", False), -1)
        ddlChannel.Items.Insert(0, item)
    End Sub

    Private Sub LoadHotels(Optional ByVal hotel As String = "")
        Dim ds As DataSet
        ds = getHotels(hotel)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
            ddlHotels.Items.Clear()
            ddlHotels.DataSource = ds.Tables(0).DefaultView
            ddlHotels.DataTextField = "Nombre"
            ddlHotels.DataValueField = "idHotel"
            ddlHotels.DataBind()
            Dim item As ListItem = New ListItem(PortalCulture.GetString("M000640", False), -1)
            ddlHotels.Items.Insert(0, item)
        End If
    End Sub

    Private Function getHotels(Optional ByVal hotel As String = "") As DataSet
        Dim ds As DataSet = New DataSet
        Dim conStr As String = AppSettings("HotelConnectionString")
        Dim sqlCon As SqlConnection = New SqlConnection(conStr)
        Dim sqlCommand As SqlCommand = New SqlCommand(SPGETHOTELS, sqlCon)
        Dim sqlDA As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure
        If hotel <> String.Empty Then
            sqlDA.SelectCommand.Parameters.Add("@hotel", SqlDbType.NVarChar).Value = hotel
        End If

        Try
            sqlDA.Fill(ds)
        Catch ex As Exception
            Return Nothing
        Finally
            If Not sqlDA.SelectCommand Is Nothing Then
                If Not sqlDA.SelectCommand.Connection Is Nothing Then
                    sqlDA.SelectCommand.Connection.Dispose()
                End If
                sqlDA.SelectCommand.Dispose()
            End If
            sqlDA.Dispose()
        End Try
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
            Return ds
        Else
            Return Nothing
        End If
    End Function

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Limpiar()
        LoadHotels(txtBuscarHotel.Text.Trim)
    End Sub
    Private Sub Limpiar(Optional ByVal idHotel As Integer = -1, Optional ByVal idChannel As Integer = -1, Optional ByVal conservarhotelseleccionado As Boolean = False)
        If Not conservarhotelseleccionado Then
            If ddlHotels.Items.Count > 0 Then
                If idHotel = -1 Then
                    ddlHotels.SelectedIndex = 0
                End If
            End If
        End If
        txtbussinesEmail.Text = String.Empty
        txtsuccessReturn.Text = String.Empty
        txtcancelReturn.Text = String.Empty
        'msgErrorEmail.Visible = False
        msgErrorReturnUrl.Visible = False
        msgErrorCancelUrl.Visible = False
        btnEliminar.Visible = False
        Me.i_idHotel = idHotel
        Me.i_idChannel = idChannel
        lblMsgActualizacion.Visible = False
    End Sub

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Limpiar(ddlHotels.SelectedValue, ddlChannel.SelectedValue)
        If Me.i_idHotel <> -1 Then
            If Me.i_idChannel <> -1 Then
                Dim ds As DataSet
                ds = (New Portal.Hotel.DataAccess.PayPalHotelDataAccess).GetHotelConfPayPalByIdHotel(Me.i_idHotel, Me.i_idChannel)
                If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                    If Not ds.Tables(0).Rows(0)("bussinessEmail") Is DBNull.Value Then
                        txtbussinesEmail.Text = ds.Tables(0).Rows(0)("bussinessEmail")
                    End If
                    If Not ds.Tables(0).Rows(0)("successReturn") Is DBNull.Value Then
                        txtsuccessReturn.Text = ds.Tables(0).Rows(0)("successReturn")
                    End If
                    If Not ds.Tables(0).Rows(0)("cancelReturn") Is DBNull.Value Then
                        txtcancelReturn.Text = ds.Tables(0).Rows(0)("cancelReturn")
                    End If
                    'Mostramos el Boton Eliminar
                    btnEliminar.Visible = True
                Else
                    'Ocultamos el boton Eliminar
                    btnEliminar.Visible = False
                End If
                msgErrorCargar.Visible = False
                msgErrorCargarCanal.Visible = False
            Else
                msgErrorCargarCanal.Visible = True
            End If
        Else
            msgErrorCargar.Visible = True
        End If
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If Me.i_idHotel <> -1 Then
            'If txtbussinesEmail.Text = String.Empty Then
            '    msgErrorEmail.Visible = True
            '    Return
            'Else
            '    msgErrorEmail.Visible = False
            'End If

            lblMsgActualizacion.Visible = False

            If txtsuccessReturn.Text = String.Empty Or Not (Regex.IsMatch(txtsuccessReturn.Text.Trim, "^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$") Or Regex.IsMatch(txtsuccessReturn.Text.Trim, "^([\/][a-zA-Z]+)+\.+[a-zA-Z]*(((\?)([a-zA-Z]*=\w*)){1}((&)([a-zA-Z]*=\w*))*)?$")) Then
                msgErrorReturnUrl.Visible = True
                Return
            Else
                msgErrorReturnUrl.Visible = False
            End If
            If txtcancelReturn.Text = String.Empty Or Not (Regex.IsMatch(txtcancelReturn.Text.Trim, "^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$") Or Regex.IsMatch(txtcancelReturn.Text.Trim, "^([\/][a-zA-Z]+)+\.+[a-zA-Z]*(((\?)([a-zA-Z]*=\w*)){1}((&)([a-zA-Z]*=\w*))*)?$")) Then
                msgErrorCancelUrl.Visible = True
                Return
            Else
                msgErrorCancelUrl.Visible = False
            End If

            'If CheckBoxPortal.Checked = False AndAlso CheckBoxUnipantalla.Checked = False Then
            '    msgErrorPortalUnipantalla.Visible = True
            '    Return
            'Else
            '    msgErrorPortalUnipantalla.Visible = False
            'End If


            'Insertamos la configuracion PayPal
            If (New Portal.Hotel.DataAccess.PayPalHotelDataAccess).UpdConfHotelPayPal(Me.i_idHotel, txtbussinesEmail.Text, txtsuccessReturn.Text, txtcancelReturn.Text, Me.i_idChannel) Then
                'Se inserto la configuracion exitosamente
                btnEliminar.Visible = True
                lblMsgActualizacion.Visible = True
            Else
                'No se logro insertar la configuracion
                lblMsgActualizacion.Visible = False
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Limpiar(-1, -1, True)
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If Me.i_idHotel <> -1 Then
            If (New Portal.Hotel.DataAccess.PayPalHotelDataAccess).DelConfHotelPayPal(Me.i_idHotel, Me.i_idChannel) Then
                'Se inserto la configuracion exitosamente
                Limpiar(-1, -1, True)
            Else
                'No se logro insertar la configuracion
            End If
        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim script As String
        script = CtlMensajes1.getShow(btnEliminar2.ClientID, PortalCulture.GetString("01158"), PortalCulture.GetString("01159"))
        script = script.Split(":")(1)
        btnEliminar.Attributes.Add("onclick", "javascript:" & script & ";return false;")
        btnEliminar2.Style.Add("display", "none")
        lblMsgActualizacion.Text = PortalCulture.GetString("01151", False)

        lblBuscar.Text = PortalCulture.GetString("M0BT0000115", True)
        Label1.Text = PortalCulture.GetString("M0BT0000150", True)
        btnBuscar.Text = PortalCulture.GetString("M0BT0000115", False)
        btnLoad.Text = PortalCulture.GetString("00149", False)
        lblNombre.Text = PortalCulture.GetString("01153", True)
        'msgErrorEmail.Text = PortalCulture.GetString("01156", False)
        RegularExpressionValidatorEmail.ErrorMessage = PortalCulture.GetString("01156", False)
        lblPorcMinimo.Text = PortalCulture.GetString("01154", True)
        msgErrorReturnUrl.Text = PortalCulture.GetString("01157", False)
        'RegularexpressionvalidatorReturnUrl.ErrorMessage = PortalCulture.GetString("01157", False)
        lblPorcMaximo.Text = PortalCulture.GetString("01155", True)
        msgErrorCancelUrl.Text = PortalCulture.GetString("01157", False)
        'RegularexpressionvalidatorCancel.ErrorMessage = PortalCulture.GetString("01157", False)
        btnAceptar.Text = PortalCulture.GetString("A00153", False)
        btnEliminar.Text = PortalCulture.GetString("00103", False)
        btnEliminar2.Text = PortalCulture.GetString("00103", False)
        btnCancel.Text = PortalCulture.GetString("M000143", False)
        msgErrorCargar.Text = PortalCulture.GetString("M0BT0000231", False)
        lblTitle.Text = PortalCulture.GetString("01158", False)
        msgErrorCargarCanal.Text = PortalCulture.GetString("01163", False)
        lblChannel.Text = PortalCulture.GetString("00791", False)
    End Sub


    Private Sub btnEliminar2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar2.Click
        btnEliminar_Click(sender, e)
    End Sub
End Class

