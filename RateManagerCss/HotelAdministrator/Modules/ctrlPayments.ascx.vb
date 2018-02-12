Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Xml
Imports System.Text
Imports System.Globalization
Imports System.IO

Imports WSHotelCommon
Imports WSHotelFacade
Imports Portal.General.Facade

Partial Public Class ctrlPayments
    Inherits UserControlBase

    Public Property ReservationNumber() As String
        Get
            Dim temp As String = String.Empty
            If Me.ViewState("idReservacion") IsNot Nothing Then temp = Me.ViewState("idReservacion")
            Return temp
        End Get
        Set(ByVal value As String)
            Me.ViewState("idReservacion") = value
        End Set
    End Property

    Public Enum PaymentTypes
        Unknow = 0
        DineroMail = 1
        Online = 2
        Paypal = 3
        DepositBank = 4
    End Enum

    Sub LoadResources()
        lblFechaRes.Text = PortalCulture.GetString("00392", True)
        lblCiudad.Text = PortalCulture.GetString("00254", True)
        lblReservacion.Text = PortalCulture.GetString("M000119", True)
        lblCliente.Text = PortalCulture.GetString("00616", True)
        lblCheckin.Text = PortalCulture.GetString("M000078", True)
        lblCheckout.Text = PortalCulture.GetString("M000079", True)
        lblAmount.Text = PortalCulture.GetString("00699", True)
        lblAmountDep.Text = PortalCulture.GetString("00700", True)
        lblCuenta.Text = PortalCulture.GetString("01281", True)
        lblReferencia.Text = PortalCulture.GetString("00695", True)
        lblObservacion.Text = PortalCulture.GetString("00696", True)
        lblMetodo.Text = PortalCulture.GetString("M0BT0000202", True)
        rfvMonto.Text = PortalCulture.GetString("00071")
        rngvMonto.Text = PortalCulture.GetString("M000242")
        rvLstMetodo.Text = PortalCulture.GetString("01280")
    End Sub

    Function loadListPaymentType()
        Dim strPayment As String
        Dim litem As ListItem
        Dim i As Integer = 0

        lstMetodo.Items.Clear()
        strPayment = PortalCulture.GetString("01279")
        For Each itm As String In strPayment.Split(",")
            litem = New ListItem
            litem.Value = CType(i, PaymentTypes)
            litem.Text = itm
            lstMetodo.Items.Add(litem)
            i += 1
        Next
        If lstMetodo.Items.Count > 0 Then lstMetodo.SelectedIndex = 0
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Me.IsPostBack Then
            Me.Clear()
            Me.FillCurrenciesList()
            loadListPaymentType()
        End If
    End Sub

    Private Sub FillCurrenciesList()
        ddlMoneda.DataSource = (New MonedaSistema).GetMonedaListIdName
        ddlMoneda.DataTextField = "Codigo"
        ddlMoneda.DataValueField = "idMoneda"
        ddlMoneda.DataBind()
    End Sub

    Public Sub Load(ByVal noReservation As String)

        Me.Clear()
        Dim ds As DataSet
        With New despositFacade
            ds = .GetReservationData(noReservation)
        End With

        If ds IsNot Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
            Me.txtReservacion.Text = noReservation
            Me.txtCliente.Text = ds.Tables(0).Rows(0).Item("Cliente")
            Me.txtFechaRes.Text = Format(ds.Tables(0).Rows(0).Item("FechaReservacion"), "dd/MMM/yyy HH:mm")

            Me.txtCheckin.Text = Format(ds.Tables(0).Rows(0).Item("checkin"), "dd/MMM/yyy")
            Me.txtCheckout.Text = Format(ds.Tables(0).Rows(0).Item("checkout"), "dd/MMM/yyy")
            Me.txtHotel.Text = ds.Tables(0).Rows(0).Item("hotel")
            Me.txtCiudad.Text = ds.Tables(0).Rows(0).Item("ciudad")
            Me.lblEmailCli.Text = ds.Tables(0).Rows(0).Item("cli_email")
            Me.txtAmount.Text = FCurrency(ds.Tables(0).Rows(0).Item("monto"), 2) & " " & ds.Tables(0).Rows(0).Item("moneda")
            Me.ReservationNumber = ds.Tables(0).Rows(0).Item("idReservacion")
        End If

    End Sub

    Private Sub Clear()
        Me.txtReservacion.Text = String.Empty
        Me.txtFechaRes.Text = String.Empty
        Me.txtCliente.Text = String.Empty
        Me.txtHotel.Text = String.Empty
        Me.txtCiudad.Text = String.Empty
        Me.txtCheckin.Text = String.Empty
        Me.txtCheckout.Text = String.Empty
        Me.txtAmount.Text = String.Empty
        Me.txtReferencia.Text = String.Empty
        Me.txtCuenta.Text = String.Empty
        Me.txtFolio.Text = String.Empty
        Me.lstMetodo.SelectedIndex = -1
        Me.txtAmountDep.Text = String.Empty
        Me.ddlMoneda.SelectedIndex = -1
        Me.txtObservacion.Text = String.Empty
        Me.ReservationNumber = String.Empty
    End Sub

    Function CreaDsLog(ByVal noReserva As String, ByVal noCuenta As String, ByVal paymSource As String, ByVal depMonto As String, _
                     ByVal folio As String, ByVal monto As String, _
                     ByVal moneda As String, ByVal idusuario As String) As String
        Dim ds As New DataSet
        Dim dt As DataTable = New DataTable("Deposit")
        Dim dr As DataRow

        Try
            With dt.Columns
                .Add(New DataColumn("noReservacion", GetType(System.String)))
                .Add(New DataColumn("NoAutorizacion", GetType(System.String)))
                .Add(New DataColumn("paymentSource", GetType(System.String)))
                .Add(New DataColumn("Referencia", GetType(System.String)))
                .Add(New DataColumn("Folio", GetType(System.String)))
                .Add(New DataColumn("Monto", GetType(System.String)))
                .Add(New DataColumn("Moneda", GetType(System.String)))
                .Add(New DataColumn("idUsuario", GetType(System.String)))
            End With
            ds.Tables.Add(dt)

            dr = ds.Tables(0).NewRow
            dr("noReservacion") = noReserva
            dr("NoAutorizacion") = noCuenta
            dr("paymentSource") = paymSource
            dr("Referencia") = depMonto
            dr("Folio") = folio
            dr("Monto") = monto
            dr("Moneda") = moneda
            dr("idUsuario") = idusuario
            ds.Tables(0).Rows.Add(dr)

        Catch ex As Exception
        End Try
        Return Util.Utility.GetXml("Deposit", "UpdatePayment", ds)

    End Function

    Public Function Save(ByVal noReserva As String, ByRef sDatos As String, ByRef spayment As String) As Boolean
        'registra el pago en la BD
        Save = False

        Dim ConnectionString As String = ConfigurationManager.AppSettings("HotelConnectionString")
        Dim dsCommand As New SqlCommand
        Dim conex As SqlConnection
        conex = New SqlConnection(ConnectionString)
        conex.Open()
        Dim trans As SqlTransaction
        Dim tr As Boolean
        Dim monto As Double
        trans = conex.BeginTransaction
        tr = True

        Try

            dsCommand.CommandType = CommandType.StoredProcedure
            dsCommand.CommandText = "spPagosreservacionesInsert"
            dsCommand.Connection = conex
            dsCommand.Transaction = trans
            dsCommand.Parameters.Add(New SqlParameter("@idReservacion", SqlDbType.Int)).Value = ViewState("idReservacion")
            dsCommand.Parameters.Add(New SqlParameter("@NoAutorizacion", SqlDbType.NVarChar, 50)).Value = Me.txtCuenta.Text
            dsCommand.Parameters.Add(New SqlParameter("@paymentSource", SqlDbType.Int)).Value = lstMetodo.SelectedValue
            dsCommand.Parameters.Add(New SqlParameter("@Referencia", SqlDbType.NVarChar, 50)).Value = Me.txtReferencia.Text
            dsCommand.Parameters.Add(New SqlParameter("@Folio", SqlDbType.NVarChar, 50)).Value = txtFolio.Text
            dsCommand.Parameters.Add(New SqlParameter("@Monto", SqlDbType.Money)).Value = txtAmountDep.Text
            dsCommand.Parameters.Add(New SqlParameter("@Moneda", SqlDbType.NVarChar, 3)).Value = ddlMoneda.SelectedItem.Text

            Dim r As Long
            r = dsCommand.ExecuteNonQuery()

            If r > 0 Then
                'Correcto
                Dim dsCommand2 As New SqlCommand
                dsCommand2.Connection = conex
                dsCommand2.Transaction = trans
                dsCommand2.CommandType = CommandType.StoredProcedure
                dsCommand2.Parameters.Clear()
                dsCommand2.CommandText = "spReservationUpdateStatus"
                dsCommand2.Parameters.Add(New SqlParameter("@idReservacion", SqlDbType.Int)).Value = ViewState("idReservacion")
                dsCommand2.Parameters.Add(New SqlParameter("@status", SqlDbType.TinyInt)).Value = 1
                r = dsCommand2.ExecuteNonQuery
                If r > 1 Then
                    trans.Commit()
                    tr = False
                    Save = True
                    Double.TryParse(txtAmountDep.Text, monto)                    
                    spayment = lstMetodo.SelectedItem.Text
                    sDatos = CreaDsLog(noReserva, Me.txtCuenta.Text, lstMetodo.SelectedItem.Text, txtReferencia.Text, _
                              txtFolio.Text, monto.ToString("########0.00"), ddlMoneda.SelectedItem.Text, (New AuthUser).UserInfoName)
                Else
                    trans.Rollback()
                    tr = False
                End If
            Else
                trans.Rollback()
                tr = False
            End If
        Catch e As Exception
            If tr Then trans.Rollback()
            lblMens.Text = e.Message
            lblMens.Visible = True
        Finally
            If conex.State = ConnectionState.Broken Or conex.State = ConnectionState.Open Then
                conex.Close()
            End If
        End Try
    End Function

    Private Sub GetDataWS(ByVal nores As String)
        Try
            Dim xdoc As New XmlDataDocument(New reqHotelDisplay)
            Dim dsreq As reqHotelDisplay = CType(xdoc.DataSet, reqHotelDisplay)
            Dim xml As resHotelDisplay
            Dim drR As reqHotelDisplay.HotelDisplayRow

            drR = dsreq.HotelDisplay.NewHotelDisplayRow
            drR.ConfirmNumber = nores
            drR.Language = "en-US"
            dsreq.HotelDisplay.AddHotelDisplayRow(drR)

            With New WSHotelFacade.clsFADisplay
                xml = .GetHotelDisplay(xdoc.DocumentElement)
            End With

            If Not xml Is Nothing AndAlso xml.Reservation.Rows.Count > 0 Then
                Dim idioma As String = PortalCulture.GetCulture.ToString
                If Not xml.Reservation(0).IsNull("IdIdiomaReservation") Then
                    If xml.Reservation(0).IdIdiomaReservation = 2 Then
                        idioma = "en-US"
                    Else
                        idioma = "es-MX"
                    End If
                End If

                With New Miscelaneos.SendHotelEmails
                    .SendCustomerEmailReservation(xml, idioma)
                    .SendEmailtoAlHotel(xml, idioma)
                End With
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function SendConfirmationEmail() As Boolean

        Call GetDataWS(Me.txtReservacion.Text)
        If lblMens.Visible Then Return False

        Return True
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        LoadResources()
    End Sub

End Class