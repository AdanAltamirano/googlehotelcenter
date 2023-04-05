Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Portal.General.Facade
Imports Portal.General.Common
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports System.Text

Partial Class Deposits
    Inherits PaginaBase

    Public Const PRM_ID_HOTEL As String = "@idHotel"
    Public Const PRM_CHECKIN As String = "@checkIn"
    Public Const PRM_CHECKOUT As String = "@checkOut"
    Public Const PRM_NAME_CLIENT As String = "@NameClient"
    Public Const PRM_FILTER As String = "@filter"
    Public Const PRM_IDIOMA As String = "@idioma"
    Public Const PRM_TARGET As String = "@DepositTarget"
    Public Const PRM_IDUSUARIO As String = "@idUsuario"
    Public Const PRM_ISNETUV As String = "@isNetUV"

    Public Const PRM_NORESERVATION As String = "@NoReservation"

    Private Enum Columns As Integer
        Itinerario
        Hotel
        Fecha
        Cliente
        Llegada
        Salida
        Cantidad
        Status
        StatusNew
        StatusConf
        WizcomPassOn
        Registrar
        Cancelar
        pmscode
        id
        noreservacion
        IsNetRateUv
    End Enum

    Private Property fecha1() As Date
        Get
            Return CDate(viewstate("fecha1"))
        End Get
        Set(ByVal Value As Date)
            viewstate("fecha1") = Value
        End Set
    End Property
    Private Property fecha2() As Date
        Get
            Return CDate(viewstate("fecha2"))
        End Get
        Set(ByVal Value As Date)
            viewstate("fecha2") = Value
        End Set
    End Property
    Private Property filtro() As Byte
        Get
            Return CByte(viewstate("filtro"))
        End Get
        Set(ByVal Value As Byte)
            viewstate("filtro") = Value
        End Set
    End Property
    Private Property cliente() As String
        Get
            Return viewstate("cliente")
        End Get
        Set(ByVal Value As String)
            viewstate("cliente") = Value
        End Set
    End Property
    Private Property NoReservacion() As String
        Get
            Return viewstate("NoReservacion")
        End Get
        Set(ByVal Value As String)
            viewstate("NoReservacion") = Value
        End Set
    End Property
    Private Property SearchByDate() As Boolean
        Get
            Return CBool(viewstate("SearchBy"))
        End Get
        Set(ByVal Value As Boolean)
            viewstate("SearchBy") = Value
        End Set
    End Property

    Protected ReadOnly Property IsOtherPayment() As Boolean
        Get
            Dim value As String = Me.Request.Params("payment")
            If value IsNot Nothing AndAlso value.Contains("?") Then value = value.Substring(0, value.IndexOf("?"))
            Return (value IsNot Nothing AndAlso value.ToLower() = "other")
        End Get
    End Property

    Private ReadOnly Property SpGetReservations() As String
        Get
            Return If(Me.IsOtherPayment, "spReservationsByPendindgPayments_GetByDates", "spReservationsByDeposit_GetByDates")
        End Get
    End Property

    Private ReadOnly Property SpGetReservationsByNo() As String
        Get
            Return If(Me.IsOtherPayment, "spReservationsByPendindgPayments_GetByNoReservation", "spReservationsByDeposit_GetByNoReservation")
        End Get
    End Property


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

    Protected WithEvents ctrlDeposits1 As ctrlDeposits
    Protected WithEvents ctrlReservationQueryDeposits1 As ctrlReservationQueryDeposits

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        'If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not MyBase.IsSupervisor AndAlso MyBase.cInfoActual.Hotel <> 0 AndAlso MyBase.cInfoActual.EsMoroso Then
            MyBase.redirectTo(PaginaBase.pages.IsDefaulter)
        End If

        ctrlReservationQueryDeposits1.UserChain = MyBase.isUserChain
        ctrlReservationQueryDeposits1.idUsuario = Session("idUsuario")

        If Not IsPostBack Then
            'Me.txtInicio.Text = Today.Date.ToString("MM/dd/yyyy")
            'loaddatos()

            Me.ctrlDeposits1.Visible = Not Me.IsOtherPayment
            Me.pnlPayments.Visible = Me.IsOtherPayment
            btnSave.Visible = False

        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.dgReservas.CurrentPageIndex = 0
        Call SearchByDates()
    End Sub



    Private Sub dgReservas_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReservas.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim status() As String = {"undefined", PortalCulture.GetString("M000331"), PortalCulture.GetString("M000592"), PortalCulture.GetString("M000333")}
            Dim lb As Label, ck As CheckBox, txt As TextBox, lnk As HyperLink

            Dim add As Integer
            add = 1
            If CDate(e.Item.Cells(Columns.Fecha).Text).DayOfWeek = DayOfWeek.Saturday Then add = 2
            If CDate(e.Item.Cells(Columns.Fecha).Text).DayOfWeek = DayOfWeek.Friday Then add = 3

            Dim fechar As Date
            fechar = DateAdd("d", add, CDate(CDate(e.Item.Cells(Columns.Fecha).Text).ToString("yyyy/MM/dd")))

            e.Item.Cells(Columns.Fecha).Text = CDate(e.Item.Cells(Columns.Fecha).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(Columns.Llegada).Text = CDate(e.Item.Cells(Columns.Llegada).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(Columns.Salida).Text = CDate(e.Item.Cells(Columns.Salida).Text).ToString("MMM/dd/yyyy")

            lb = e.Item.Cells(Columns.Cancelar).FindControl("lblCancel")
            ck = e.Item.Cells(Columns.Cancelar).FindControl("chkCancel")

            If fechar < CDate(Now.ToString("yyyy/MM/dd")) Then
                'e.Item.BackColor = Color.FromName("#F88158") 'dgReservas.BackColor.LightPink
                e.Item.ForeColor = Color.FromKnownColor(KnownColor.Red)
            Else
                ck.Visible = False
            End If

            txt = e.Item.Cells(Columns.Cancelar).FindControl("txtMotivoCancelacion")
            lnk = e.Item.Cells(Columns.Itinerario).FindControl("Itinerary")
            If ConfigurationManager.AppSettings("idSegmento") = "4" Then
                lnk.NavigateUrl = GeRequestApplicationPath(String.Concat("/HotelAdministrator/Pages/ReservationDetailsV2.aspx?qs=", e.Item.Cells(Columns.id).Text))
            Else
                lnk.NavigateUrl = GeRequestApplicationPath(String.Concat("/HotelAdministrator/Pages/ReservationDetails.aspx?qs=", e.Item.Cells(Columns.id).Text))
            End If

            lnk.Target = "_blank"
            ck.Attributes.Add("onclick", "javascript:ShowControls('" & ck.ClientID & "','" & txt.ClientID & "','" & lb.ClientID & "','" & e.Item.Cells(Columns.pmscode).Text & "')")
            lb.Text = ""
            If e.Item.Cells(Columns.pmscode).Text <> "&nbsp;" Then
                lb.Text = e.Item.Cells(Columns.pmscode).Text
            End If
            ck.Checked = False
            lb.Style.Add("display", "none")
            txt.Style.Add("display", "none")
            If lb.Text <> "" Then
                txt.Text = lb.Text
                ck.Checked = True
                txt.Style.Add("display", "none")
                lb.Style.Add("display", "block")
            End If
            Dim btn As Button
            btn = e.Item.Cells(Columns.Registrar).FindControl("btnRegistrar")
            btn.Text = PortalCulture.GetString("00697")

            If (e.Item.Cells(Columns.IsNetRateUv).Text = "True") Then
                If Not (MyBase.IsSupervisor Or (MyBase.IsUsuarioHotelAssociation And MyBase.IdAsociation = 1)) Then
                    btn.Attributes.Add("style", "display: none")
                End If
            End If

            'If (Not Me.IsOtherPayment) Then
            '    If (MyBase.IsSupervisor Or (MyBase.IsUsuarioHotelAssociation And MyBase.IdAsociation = 1)) Then
            '    Else
            '        .Parameters.Add(New SqlParameter(PRM_ISNETUV, SqlDbType.TinyInt)).Value = 0
            '    End If
            'End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(Columns.Itinerario).Text = PortalCulture.GetString("M000119")
            e.Item.Cells(Columns.Fecha).Text = PortalCulture.GetString("M000120")
            e.Item.Cells(Columns.Cliente).Text = PortalCulture.GetString("M000121")
            e.Item.Cells(Columns.Llegada).Text = PortalCulture.GetString("M000122")
            e.Item.Cells(Columns.Salida).Text = PortalCulture.GetString("M000123")
            e.Item.Cells(Columns.Cantidad).Text = PortalCulture.GetString("00062")
            e.Item.Cells(Columns.Registrar).Text = PortalCulture.GetString("M000486")
            e.Item.Cells(Columns.Cancelar).Text = PortalCulture.GetString("00095")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(Columns.Cancelar).Text = CType(dgReservas.DataSource, DataSet).Tables(0).Rows.Count & " " & PortalCulture.GetString("M000028")
        End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim lb As Label, ck As CheckBox, txt As TextBox ', lnk As HyperLink

        For Each it As DataGridItem In Me.dgReservas.Items
            If it.ItemType = ListItemType.Item Or it.ItemType = ListItemType.AlternatingItem Then
                ck = it.Cells(Columns.Cancelar).FindControl("chkCancel")
                lb = it.Cells(Columns.Cancelar).FindControl("lblCancel")
                txt = it.Cells(Columns.Cancelar).FindControl("txtMotivoCancelacion")
                If ck.Checked AndAlso txt.Text <> "" Then 'AndAlso txt.Text <> it.Cells(Columns.pmscode).Text Then
                    Cancelar(txt.Text, it.Cells(Columns.id).Text)
                    'guardalog("/HotelAdministrator/Pages/Deposits", PaginaBase.acciones.Modificar, "Cancelo la reservación :" & it.Cells(Columns.noreservacion).Text)
                End If
            End If
        Next
        btnSearch_Click(sender, e)
    End Sub

    Private Sub dgReservas_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgReservas.PageIndexChanged
        dgReservas.CurrentPageIndex = e.NewPageIndex
        If Me.SearchByDate Then
            Call SearchByDates()
        Else
            Call SearchByNoRes()
        End If

    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'lblFecha.Text = PortalCulture.GetString("M000653", True)
        'Me.btnSearch.Text = PortalCulture.GetString("M000637")
        Me.btnSave.Text = PortalCulture.GetString("00692")
        Me.btnGuardar.Text = PortalCulture.GetString("M000060")
        Me.btnCancelar.Text = PortalCulture.GetString("00095")
        dgReservas.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("M000056")
        dgReservas.PagerStyle.NextPageText = PortalCulture.GetString("M000057") & " >>"
        dgReservas.Columns(Columns.Itinerario).HeaderText = PortalCulture.GetString("M000119")
        dgReservas.Columns(Columns.Fecha).HeaderText = PortalCulture.GetString("M000120")
        dgReservas.Columns(Columns.Cliente).HeaderText = PortalCulture.GetString("M000121")
        dgReservas.Columns(Columns.Llegada).HeaderText = PortalCulture.GetString("M000122")
        dgReservas.Columns(Columns.Salida).HeaderText = PortalCulture.GetString("M000123")
        dgReservas.Columns(Columns.Cantidad).HeaderText = PortalCulture.GetString("00062")
        dgReservas.Columns(Columns.Registrar).HeaderText = PortalCulture.GetString("M000486")
        dgReservas.Columns(Columns.Cancelar).HeaderText = PortalCulture.GetString("00095")
        lblTitle.Text = PortalCulture.GetString("00691")
        If Me.dgReservas.Items.Count > 0 Then
            lblHelp2.Text = PortalCulture.GetString("00698")
        Else
            lblHelp2.Text = ""
        End If
        If Not IsPostBack Then
            If Session("welcome_noReservacion") <> String.Empty Then

                Me.SearchByDate = False
                Me.NoReservacion = Session("welcome_noReservacion")
                Me.dgReservas.CurrentPageIndex = 0
                Call SearchByNoRes()
                Session("welcome_noReservacion") = String.Empty
                If Not Session("welcome_idhotel") Is Nothing Then
                    Session("welcome_idhotel") = String.Empty
                End If

            End If
        End If
    End Sub

    Private Sub dgReservas_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReservas.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If Me.dgReservas.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If Me.dgReservas.CurrentPageIndex < Me.dgReservas.PageCount - 1 Then
                Dim _next As New System.Web.UI.WebControls.LinkButton
                _next.CommandArgument = "Next"
                _next.CommandName = "Page"
                _next.Text = PortalCulture.GetString("00011") & "&nbsp;>"
                _next.CausesValidation = False

                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, _next)
            End If
        End If
    End Sub

    Private Sub dgReservas_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgReservas.ItemCommand
        If e.CommandName = "Registrar" Then
            Panel1.Visible = False
            Panel2.Visible = True
            'Meter la validacion, si es otra forma de pago mostrar el control crtlPayment
            inputReserva.Value = e.Item.Cells(Columns.noreservacion).Text
            If Me.IsOtherPayment Then
                pnlPayments.Load(e.Item.Cells(Columns.noreservacion).Text)
            Else
                ctrlDeposits1.GetReservationData(e.Item.Cells(Columns.noreservacion).Text)
            End If
        End If
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Panel1.Visible = True
        Panel2.Visible = False
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Dim sDatos As String = ""
        Dim spayment As String = ""
        If Page.IsValid Then
            'seleccionar entre los dos controles de registro dependiendo del tipo de peticion
            Dim flag As Boolean = False

            If Me.IsOtherPayment Then
                flag = pnlPayments.Save(inputReserva.Value, sDatos, spayment)
                If flag Then pnlPayments.SendConfirmationEmail()
                Me.guardalog("/HotelAdministrator/Pages/Deposits.aspx", PaginaBase.acciones.Crear, String.Format("Creación de depósito {0}, no.reservacion: {1}", spayment, inputReserva.Value), "", "", sDatos, noReservacion:=inputReserva.Value)
            Else
                flag = ctrlDeposits1.Save(inputReserva.Value, sDatos)
                If flag Then
                    ctrlDeposits1.ConfirmPaymentRequest(inputReserva.Value)
                    ctrlDeposits1.enviarcorreo_conf()
                End If
                Me.guardalog("/HotelAdministrator/Pages/Deposits.aspx", PaginaBase.acciones.Crear, String.Format("Creación de depósito, no.reservacion: {0}", inputReserva.Value), "", "", sDatos, noReservacion:=inputReserva.Value)
            End If


            If flag Then
                Panel1.Visible = True
                Panel2.Visible = False
                If Me.SearchByDate Then
                    Call SearchByDates()
                Else
                    Call SearchByNoRes()
                End If
            End If
        End If
    End Sub

#Region "Cancelacion"
    Private Sub Cancelar(ByVal Motivo As String, ByVal idRes As String)
        Dim dsReservaciones As ReservaDatos
        Dim GalileoConfCancelNumber As String = ""
        Dim NoCancelacion As String = ""

        With New ReservaFacade
            dsReservaciones = .GetDataReserva(idRes)
        End With

        'si existe en galileo hay que cancelarla alla
        If Not dsReservaciones Is Nothing AndAlso dsReservaciones.Tables(ReservaDatos.RESERVA_TABLE).Rows.Count > 0 Then
            With dsReservaciones.Tables(ReservaDatos.RESERVA_TABLE).Rows(0)
                Dim CancelNumber As String = Nothing
                If LocalCancel(.Item(ReservaDatos.FIELD_IDRESERVACION), GalileoConfCancelNumber, CInt(Val(.Item("idUsuario").ToString)), NoCancelacion, Motivo, CancelNumber) Then
                    enviarcorreo(dsReservaciones, Motivo, CancelNumber)
                    Me.guardalog("/HotelAdministrator/Pages/Deposits.aspx", PaginaBase.acciones.Eliminar, "Canceló la reservacion " & .Item(ReservaDatos.FIELD_NORESERVACION).ToString)
                    'Return True
                Else
                    'Return False
                End If
            End With
        End If
    End Sub

    Private Function LocalCancel(ByVal idReservacion As Long, ByVal ConfNumCancel As String, ByVal IDUser As Integer, ByRef refNoCancelacion As String, ByVal Motivo As String, ByRef CancelNumber As String) As Boolean
        Dim trans As Boolean
        trans = False
        Dim sqlconn As New SqlConnection(AppSettings("HotelConnection"))
        sqlconn.Open()
        Dim transacc As SqlTransaction = sqlconn.BeginTransaction
        trans = True
        Try
            Dim sqlcmd As New SqlCommand("spReservationCancel", sqlconn)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Transaction = transacc
            'Reservacion
            sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idReservacion

            '//generar el numero de cancelacion @NoCancelacion
            CancelNumber = "CX" & Format(Now(), "yy") & Format(Now(), "MM") & Format(Now(), "dd") & Format(Now(), "HH") & Format(Now(), "mm") & Format(Now(), "ss") & IDUser
            refNoCancelacion = CancelNumber
            sqlcmd.Parameters.Add("@NoCancelacion", SqlDbType.NVarChar, 50).Value = CancelNumber
            sqlcmd.Parameters.Add("@MotivoCancelacion", SqlDbType.NVarChar, 50).Value = Motivo

            sqlcmd.Parameters.Add("@iduser", SqlDbType.Int).Value = MyBase.UserIdentityName
            If ConfNumCancel.Trim <> "" Then sqlcmd.Parameters.Add("@NoConfCancelGalileo", SqlDbType.NVarChar, 50).Value = ConfNumCancel

            Dim afec As Integer = sqlcmd.ExecuteNonQuery()
            transacc.Commit()
            trans = False
            If afec > 0 Then
                Return True
            Else
                Return False
            End If
        Catch e As Exception
            If trans Then transacc.Rollback()
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
            Return False
        Finally
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
        End Try
    End Function

    Private Sub enviarcorreo(ByVal dsreservaciones As ReservaDatos, ByVal Motivo As String, ByVal CancelNumber As String)
        Dim ci As System.Globalization.CultureInfo
        Dim idioma As String = ""
        Dim Mail As emailTemplates.Template
        Dim cc As String
        Dim source As String
        Try

            With dsreservaciones.Tables(ReservaDatos.RESERVA_TABLE).Rows(0)

                If .Item(ReservaDatos.FIELD_IDHOTEL) <> 0 Then
                    Dim hotel As HotelDatos
                    With New HotelSistema
                        hotel = .GetHotelById(dsreservaciones.Tables(ReservaDatos.RESERVA_TABLE).Rows(0).Item(ReservaDatos.FIELD_IDHOTEL))
                    End With
                    If hotel.Tables.Count > 0 AndAlso hotel.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 AndAlso hotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_EMAIL_RESERVAS) <> "" Then
                        cc = hotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_EMAIL_RESERVAS)
                        idioma = hotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_IDIOMAEMAIL).ToString
                        source = dsreservaciones.Tables(ReservaDatos.RESERVA_TABLE).Rows(0)(ReservaDatos.FIELD_SOURCE)
                    End If
                End If
                If idioma = "" Then
                    idioma = PortalCulture.GetCulture.ToString
                End If

                ci = System.Threading.Thread.CurrentThread.CurrentCulture
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(idioma)
                PortalCulture.SetCulture(System.Threading.Thread.CurrentThread.CurrentCulture.Name)

                Mail = New emailTemplates.Template
                If source = "UNI" Or source = "IDS" Or source = "ADS" Or source = "CCT" Then
                    Mail.TemplateName = "T12_HOTELCANCELLATION_LOGO"
                Else
                    Mail.TemplateName = "T12_HOTELCANCELLATION"
                End If


                If cc <> "" Then
                    Mail.To = cc
                End If

                Mail.SubjectParam = .Item(ReservaDatos.FIELD_NORESERVACION)
                Mail.Cc = AppSettings("UnivisitMail")
                If .Item(ReservaDatos.FIELD_EMAILCL).ToString <> "" Then
                    Mail.Bcc = .Item(ReservaDatos.FIELD_EMAILCL)
                End If

                Mail.Html = True
                Mail.SubjectParam = .Item(ReservaDatos.FIELD_NORESERVACION)
                Mail.Idioma = System.Threading.Thread.CurrentThread.CurrentCulture.Name
                Mail.AddParameter("UNIVISITPORTAL") = PortalCulture.GetString("00516")

                If source = "UNI" Or source = "IDS" Or source = "ADS" Or source = "CCT" Then
                    Mail.AddParameter("PROPERTY_URLLOGO") = Util.Utility.LoadImagen(cInfoActual.Empresa)
                    Mail.AddParameter("PROPERTY_NAME") = cInfoActual.HotelName
                End If

                If Motivo <> "" Then
                    Mail.AddParameter("MOTIVO") = Motivo
                Else
                    Mail.AddParameter("MOTIVO") = PortalCulture.GetString("M000604")
                End If

                If .Item(ReservaDatos.FIELD_IDHOTEL) = "0" Then
                    Mail.AddParameter("HOTELNAME") = IIf(.IsNull(ReservaDatos.FIELD_HOTELGNOMBRE), PortalCulture.GetString("M000604"), .Item(ReservaDatos.FIELD_HOTELGNOMBRE))
                    Mail.AddParameter("CITY") = IIf(.IsNull(ReservaDatos.FIELD_HOTELGCIUDAD), PortalCulture.GetString("M000604"), .Item(ReservaDatos.FIELD_HOTELGCIUDAD))
                Else
                    Mail.AddParameter("HOTELNAME") = .Item(ReservaDatos.FIELD_HOTELNOMBRE)
                    Dim ciudad As String = .Item(ReservaDatos.FIELD_HOTELCIUDAD) & ", " & .Item(ReservaDatos.FIELD_HOTELESTADO)
                    Mail.AddParameter("CITY") = ciudad
                End If


                Mail.AddParameter("RESERVATIONNUMBER") = .Item(ReservaDatos.FIELD_NORESERVACION)
                Mail.AddParameter("CANCELLATIONNUMBER") = CancelNumber
                Mail.AddParameter("STARTDATE") = CDate(.Item(ReservaDatos.FIELD_CHECKIN)).ToString("dd/MMM/yyyy")
                Mail.AddParameter("ENDDATE") = CDate(.Item(ReservaDatos.FIELD_CHECKOUT)).ToString("dd/MMM/yyyy")
                Mail.AddParameter("CUSTOMERNAME") = .Item(ReservaDatos.FIELD_CLIENTE)
                Mail.AddParameter("REGDATE") = CDate(.Item("FechaReservacion")).ToString("dd/MMM/yyyy")
                Mail.AddParameter("CANCELLEDDATE") = Now.Date.ToString("dd/MMM/yyyy")
                Mail.AddParameter("DetCuartos") = GetTable(dsreservaciones)
                Mail.Send()

            End With
        Catch ex As Exception
            Dim sErrorMessage As String = String.Format("The HTML fragment file '{0}' ", ex.ToString)
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
            PortalCulture.SetCulture(System.Threading.Thread.CurrentThread.CurrentCulture.Name)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
            PortalCulture.SetCulture(System.Threading.Thread.CurrentThread.CurrentCulture.Name)
        End Try
    End Sub

    Public Function GetTable(ByVal dsreservaciones As ReservaDatos) As String
        Dim table As New StringBuilder
        Dim br As Object = ControlChars.CrLf
        table.Append("<TABLE cellSpacing=""0"" cellPadding=""0"" width=""270px"" border=""0"">")
        table.Append(" <TR>")
        table.Append("   <TD>")
        For i As Integer = 0 To dsreservaciones.Tables(0).Rows.Count - 1
            table.Append("<TABLE cellSpacing=""0"" cellPadding=""0"" width=""270px"" border=""0"">")
            table.Append(" <TR>" & br)
            table.Append("     <TD width=""50%"" class=""txtEtiqueta"" align=""right""><span>" & PortalCulture.GetString("M000066"))
            table.Append("</span>     </TD >")
            table.Append("     <TD width=""50%"" >: <span class=""txtdato"">" & dsreservaciones.Tables(0).Rows(i).Item("NombreHabitacion"))
            table.Append("</span>     </TD>")
            table.Append(" </TR>" & br)
            table.Append(" <TR>" & br)
            table.Append("     <TD width=""50%""  class=""txtEtiqueta"" align=""right""><span>" & PortalCulture.GetString("M000585"))
            table.Append("</span></TD>")
            table.Append("     <TD width=""50%"" >: <span class=""txtdato"">" & dsreservaciones.Tables(0).Rows(i).Item("viajero"))
            table.Append("</span>     </TD>")
            table.Append(" </TR>" & br)
            table.Append("</TABLE>" & br)
        Next
        table.Append("   </TD>" & br)
        table.Append("  </TR>" & br)
        table.Append("</TABLE>" & br)
        Return table.ToString
    End Function
#End Region

    Private Sub ctrlReservationQueryDeposits1_SearchByDates(ByVal fecha1 As Date, ByVal fecha2 As Date, ByVal filtro As Byte, ByVal Cliente As String) Handles ctrlReservationQueryDeposits1.SearchByDates
        Me.fecha1 = fecha1
        Me.fecha2 = fecha2
        Me.filtro = filtro
        Me.cliente = Cliente
        Me.NoReservacion = ""
        Me.SearchByDate = True
        Me.dgReservas.CurrentPageIndex = 0
        Call SearchByDates()
    End Sub

    Private Sub ctrlReservationQueryDeposits1_SearchByNoRes(ByVal NoReservacion As String) Handles ctrlReservationQueryDeposits1.SearchByNoRes
        Me.SearchByDate = False
        Me.NoReservacion = NoReservacion
        Me.dgReservas.CurrentPageIndex = 0
        Call SearchByNoRes()
    End Sub

    Private Sub SearchByDates()
        Dim ds As New DataSet
        ' Dim dv As DataView
        'Dim fecha = CDate(Me.txtInicio.Text)
        Dim ConnectionString As String = AppSettings("HotelConnectionString")
        btnSave.Visible = False
        Dim dsCommand As New SqlDataAdapter
        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = SPGETRESERVATIONS
                    .Connection = New SqlConnection(ConnectionString)
                    If filtro > 0 Then
                        .Parameters.Add(New SqlParameter(PRM_FILTER, SqlDbType.Int)).Value = CInt(filtro)
                        .Parameters.Add(New SqlParameter(PRM_CHECKIN, SqlDbType.DateTime)).Value = Format(fecha1, "yyyy/MM/dd")
                        .Parameters.Add(New SqlParameter(PRM_CHECKOUT, SqlDbType.DateTime)).Value = Format(fecha2, "yyyy/MM/dd")
                    End If
                    .Parameters.Add(New SqlParameter(PRM_NAME_CLIENT, SqlDbType.NVarChar, 80)).Value = cliente
                    .Parameters.Add(New SqlParameter(PRM_IDIOMA, SqlDbType.Int)).Value = PortalCulture.GetIDCulture

                    If MyBase.isUserChain Then
                        If ctrlReservationQueryDeposits1.isHotelSelected Then
                            .Parameters.Add(New SqlParameter(PRM_ID_HOTEL, SqlDbType.Int)).Value = ctrlReservationQueryDeposits1.idHotelSelected
                            .Parameters.Add(New SqlParameter(PRM_IDUSUARIO, SqlDbType.Int)).Value = CType(Session("idUsuario"), Integer)
                        Else
                            .Parameters.Add(New SqlParameter(PRM_ID_HOTEL, SqlDbType.Int)).Value = MyBase.cInfoActual.Hotel
                        End If
                    Else
                        .Parameters.Add(New SqlParameter(PRM_ID_HOTEL, SqlDbType.Int)).Value = MyBase.cInfoActual.Hotel
                    End If

                    If Not MyBase.IsSupervisor Then
                        .Parameters.Add(New SqlParameter(PRM_TARGET, SqlDbType.NVarChar, 3)).Value = "HTL"
                    End If
                    If Not MyBase.GetIdAsociation Then
                        .Parameters.Add(New SqlParameter("@idAsociacion", SqlDbType.Int)).Value = MyBase.GetIdAsociation
                    End If

                End With
                .Fill(ds)
            Catch ex As Exception
                Dim s As String = ex.Message.ToString
                ds = Nothing 'TODO VS2008
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

        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.dgReservas.DataSource = ds
        Me.dgReservas.DataBind()
        If Me.dgReservas.Items.Count > 0 Then btnSave.Visible = True
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub

    Private Sub SearchByNoRes()
        Dim ds As New DataSet
        'Dim dv As DataView
        'Dim fecha = CDate(Me.txtInicio.Text)
        Dim ConnectionString As String = AppSettings("HotelConnectionString")
        btnSave.Visible = False
        Dim dsCommand As New SqlDataAdapter
        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = SPGETRESERVATIONSBYNO
                    .Connection = New SqlConnection(ConnectionString)

                    .Parameters.Add(New SqlParameter(PRM_NORESERVATION, SqlDbType.NVarChar, 24)).Value = Me.NoReservacion
                    .Parameters.Add(New SqlParameter(PRM_IDIOMA, SqlDbType.Int)).Value = PortalCulture.GetIDCulture
                    .Parameters.Add(New SqlParameter(PRM_ID_HOTEL, SqlDbType.Int)).Value = MyBase.cInfoActual.Hotel
                    If Not MyBase.IsSupervisor Then
                        .Parameters.Add(New SqlParameter(PRM_TARGET, SqlDbType.NVarChar, 3)).Value = "HTL"
                    End If
                    If Me.GetIdAsociation > 0 Then
                        .Parameters.Add(New SqlParameter("@idAsociacion", SqlDbType.Int)).Value = Me.GetIdAsociation
                    End If


                End With
                .Fill(ds)
            Catch
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

        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.dgReservas.DataSource = ds
        Me.dgReservas.DataBind()
        If Me.dgReservas.Items.Count > 0 Then btnSave.Visible = True
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
        Me.ctrlReservationQueryDeposits1.SetReservacion = Me.NoReservacion
    End Sub

End Class
