<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="XCrypt" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="RateManager" %>
<%@ Import Namespace="Portallibraries" %>
<%@ Import Namespace="PaymentLibrary" %>
<%@ Import Namespace="WSHotelCommon" %>
<%@ Import Namespace="WSHotelFacade" %>

<%@ Page Language="vb" AutoEventWireup="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    Private confirmnum As String = ""
    Dim Curr As String = ""
    Dim total As Double = 0
    
    Protected Enum Actions
        Unknow = 0
        TryAgain = 1
        Cancel = 2
    End Enum
    
    Protected ReadOnly Property PaymentType() As paymentSources
        Get
            Dim result As PaymentTypes
            If Request.QueryString("BancomerPayment") IsNot Nothing andalso Request.QueryString("BancomerPayment") ="true" Then
                result = paymentSources.OnlinePaymentBancomer
            Else
                result = paymentSources.OnlinePaymentBanamex
            End If
            Return result
        End Get
    End Property
    
    'cambiar el status de la reservación a activa.
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim cfg As New PortalLibraries.PortalPartnersCfg
        Dim ds As New DataSet        
        Dim confirmnum As String = ""
        Dim idReserva As Integer
        
        If Not Request.QueryString("idReservation") Is Nothing Then
            confirmnum = Request.QueryString("idReservation")
        End If
		If Not Request.QueryString("ReservationId") Is Nothing Then
            confirmnum = Request.QueryString("ReservationId")
        End If        
                        
        If confirmnum = "" Then
            '' Response.Redirect(cfg.NonSecureSite & "/home.aspx")
        Else
            Try
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(Request.QueryString("Language"))
            Catch ex As Exception
                'Si ocurre un error al intentar cambiar la cultura no hacemos nada
            End Try
            'Guardamos en session la cultura establecida
            Session("CultureUI") = System.Threading.Thread.CurrentThread.CurrentUICulture
        End If
        If Not Page.IsPostBack Then
            Dim AutorizationNumber As String = ""
            If Not Request.QueryString("AuthorizeId") Is Nothing Then
                AutorizationNumber = Request.QueryString("AuthorizeId")
            End If
        
            If AutorizationNumber <> "" AndAlso AutorizationNumber.Trim.ToLower <> "null" Then
                'terminó de pagar la reservación hay que activarla
                lblMsg.Text = RateManager.PortalCulture.GetString("01434")                                                
                btnCancelar.Visible =False 
                btnReintentar.Visible = False
                
                If PaymentType = paymentSources.OnlinePaymentBanamex Then
                    ds = GetRes(confirmnum)'GetRes(Request.QueryString("idReservation"))
                    If (Not ds Is Nothing) AndAlso (ds.Tables(0).Rows.Count > 0) Then
                        idReserva = ds.Tables(0).Rows(0)("idReservacion")
                        UpdateReservation(idReserva, AutorizationNumber, Request.QueryString("Amount"), Request.QueryString("Currency"), paymentSources.OnlinePaymentBanamex)
                    End If
                Else
                    idReserva = Regex.Replace(Request.QueryString("ReservationId"), "[^\d]", "")
                    UpdateReservation(idReserva, AutorizationNumber, Request.QueryString("Amount"), Request.QueryString("Currency"), paymentSources.OnlinePaymentBancomer)
                End If
                'UpdateReservation(Request.QueryString("ReservationId"), AutorizationNumber, Request.QueryString("Amount"), Request.QueryString("Currency"))
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectDefault", "<script>FireClose(1);" & "</scr" + "ipt>")
            Else
                lblMsg.Text = RateManager.PortalCulture.GetString("01427")
            End If
        End If
        
    End Sub
            
    Private Function GetRes(ByVal noReservacion As String) As DataSet
        Dim ds As New DataSet
        With New SqlDataAdapter("spReservationGetByNumber", New SqlConnection(ConfigurationManager.AppSettings("HotelConnection")))
            .SelectCommand.CommandType = CommandType.StoredProcedure
            .SelectCommand.Parameters.Add("@NoReservacion", SqlDbType.NVarChar, 24).Value = noReservacion
            Try
                .SelectCommand.Connection.Open()
                .Fill(ds)
            Catch ex As Exception
            Finally
                .SelectCommand.Connection.Close()
            End Try
        End With
        Return ds
    End Function
    
    Private Function GetNoRes(ByVal idReservacion As String) As String
        Dim noReservacion As String = ""
        With New SqlDataAdapter("Select noreservacion From Reservaciones Where idreservacion=" + idReservacion, New SqlConnection(ConfigurationManager.AppSettings("HotelConnection")))
            .SelectCommand.CommandType = CommandType.Text
            .SelectCommand.Connection.Open()
            Try
                ''.SelectCommand.Connection.Open()
                noReservacion = .SelectCommand.ExecuteScalar()
                ''.Fill(ds)
            Catch ex As Exception
            Finally
                .SelectCommand.Connection.Close()
            End Try
        End With
        Return noReservacion
    End Function
        
    Private Sub UpdateReservation(ByVal idreservacion As String, ByVal NoConfirmacion As String, ByVal sAmount As String, ByVal sCurrency As String, ByVal Paysource As PortalLibraries.paymentSources)
        Try
            Dim reqDisp As New Hotel_Display_RQ
            Dim ds As New DataSet
            
            With New SqlCommand("spPagosReservacionesInsert", New SqlConnection(ConfigurationManager.AppSettings("HotelConnection")))
                .CommandType = Data.CommandType.StoredProcedure
                .Parameters.Add("@idReservacion", SqlDbType.Int).Value = idreservacion
                .Parameters.Add("@Status", SqlDbType.TinyInt).Value = 1 '1= Confirmada
                .Parameters.Add("@NoAutorizacion", SqlDbType.NVarChar, 50).Value = NoConfirmacion
                .Parameters.Add("@PaymentSource", SqlDbType.Int).Value = Paysource
                'Obtiene el monto total
                'Dim amount As Double = 0
                'Double.TryParse(sAmount, amount)
                'amount /= 100
                '.Parameters.Add("@Monto", SqlDbType.Money).Value = amount
                '.Parameters.Add("@Moneda", SqlDbType.NVarChar, 3).Value = sCurrency
                Try
                    .Connection.Open()
                    .ExecuteNonQuery()
                Catch ex As Exception
                    Throw New Exception("Error", ex)
                Finally
                    .Connection.Close()
                End Try
            End With
            
        Catch ex As Exception
            While ex IsNot Nothing
                ex = ex.InnerException
            End While
						
        End Try
    End Sub
           
    Private Function LocalCancel(ByVal idReservacion As String, ByVal idUser As String) As Boolean
        Dim ds As New DataSet
        Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("HotelConnection"))
        Dim CancelNumber As String
        Dim trans As Boolean
        trans = False
               
        Try
            sqlconn.Open()
            Dim transacc As SqlTransaction = sqlconn.BeginTransaction
            trans = True
            Try
                Dim sqlcmd As New SqlCommand("spReservationCancel", sqlconn)
                sqlcmd.CommandType = CommandType.StoredProcedure
                sqlcmd.Transaction = transacc
                sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idReservacion
           
                '// Genera el numero de cancelacion @NoCancelacion
                CancelNumber = "CX" & Format(Now(), "yy") & Format(Now(), "MM") & Format(Now(), "dd") & Format(Now(), "HH") & Format(Now(), "mm") & Format(Now(), "ss") & idUser
                sqlcmd.Parameters.Add("@NoCancelacion", SqlDbType.NVarChar, 50).Value = CancelNumber
                sqlcmd.Parameters.Add("@MotivoCancelacion", SqlDbType.NVarChar, 250).Value = "Cancelacion en pago en linea (BMX). " & PortalCulture.GetString("00780", True)
                
                Dim idUsuario  As Integer 
                If (Not HttpContext.Current.Session("idUsuario") Is Nothing AndAlso HttpContext.Current.Session("idUsuario") <> String.Empty) Then
                 idUsuario = (HttpContext.Current.Session("idUsuario"))
                 sqlcmd.Parameters.Add("@iduser", SqlDbType.Int).Value = idUsuario
                End If                
                
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
            
        Catch ex As Exception
        End Try
       
    End Function
    
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Dim ds As New DataSet
        Dim idreserva As Integer
        If PaymentType = paymentSources.OnlinePaymentBanamex Then            
			Dim confirmnum As String = ""
			If Not Request.QueryString("idReservation") Is Nothing Then
				confirmnum = Request.QueryString("idReservation")
			End If
			If Not Request.QueryString("ReservationId") Is Nothing Then
				confirmnum = Request.QueryString("ReservationId")
			End If		
            If confirmnum<>"" Then
                ds = GetRes(confirmnum)'GetRes(Request.QueryString("idReservation"))
                If (Not ds Is Nothing) AndAlso (ds.Tables(0).Rows.Count > 0) Then
                    idreserva = ds.Tables(0).Rows(0)("idReservacion")
                    LocalCancel(idreserva, Request.QueryString("idUser"))
                End If
                
            End If
        Else
            If Not Request.QueryString("ReservationId") Is Nothing Then
                idreserva = Regex.Replace(Request.QueryString("ReservationId"), "[^\d]", "")
                LocalCancel(idreserva, Request.QueryString("idUser"))
            End If
        End If        
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectDefault", "<script>FireClose(1);" & "</scr" + "ipt>")
    End Sub
    
    Protected Sub btnReintentar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReintentar.Click
    
    dim sBankOnlineCCPayp as string = ConfigurationManager.AppSettings("BankOnlineCCPayp")
    
        'If PaymentType = paymentSources.OnlinePaymentBanamex Then
        IF sBankOnlineCCPayp = "BMX" Then
            Try
				Dim confirmnum As String = ""
				If Not Request.QueryString("idReservation") Is Nothing Then
					confirmnum = Request.QueryString("idReservation")
				End If
				If Not Request.QueryString("ReservationId") Is Nothing Then
					confirmnum = Request.QueryString("ReservationId")
				End If	
			
                Dim MerchantService_Currency As String
                Dim MerchantService_Url As String
                Dim MerchantService_Port As String
                Dim MerchantService_Account As String
                Dim UrlPayment As String
                Dim idUrlPayment As String
                Dim MerchantService_SecureHash As String
                Dim MerchantService_AccessCode  AS string
            
                UrlPayment = ConfigurationManager.AppSettings("UrlPayment")
                MerchantService_Currency = ConfigurationManager.AppSettings("MerchantService_Currency")
                MerchantService_Url = ConfigurationManager.AppSettings("MerchantService_Url")
                MerchantService_Port = ConfigurationManager.AppSettings("MerchantService_Port")
				
				MerchantService_SecureHash = ConfigurationManager.AppSettings("MerchantService_SecureHash")
				MerchantService_AccessCode = ConfigurationManager.AppSettings("MerchantService_AccessCode")
				
                idUrlPayment = ConfigurationManager.AppSettings("idUrlPayment")
                MerchantService_Account = ConfigurationManager.AppSettings("MerchantService_Account")
                Dim sCultureFormat As String = "es-MX"
                    
                'Dim DisplayRs As resHotelDisplay
                'DisplayRs = (New clsHelperHOS).getReservation(Request.QueryString("idReservation"), Request.QueryString("PropertyNumber"), Request.QueryString("Currency"))
        
                'Dim payItemOrder As New PaymentLibrary.PaymentSockets
                Dim Card As New PaymentLibrary.CardPayment
            
                '''''''''''''''''''''''''''''''
            
                Dim xdoc As New XmlDataDocument(New reqHotelDisplay)
                Dim dsreq As reqHotelDisplay = CType(xdoc.DataSet, reqHotelDisplay)
                Dim DisplayRs As resHotelDisplay

                Dim drR As reqHotelDisplay.HotelDisplayRow
                drR = dsreq.HotelDisplay.NewHotelDisplayRow
                drR.ConfirmNumber = confirmnum 'Request.QueryString("idReservation")
                drR.Language = "en-US"
                dsreq.HotelDisplay.AddHotelDisplayRow(drR)

                Card.ccGateway = "ssl"
                Dim Amount As Decimal = 0
                With New WSHotelFacade.clsFADisplay
                    DisplayRs = .GetHotelDisplay(xdoc.DocumentElement)
                End With
            
                If Not DisplayRs Is Nothing AndAlso DisplayRs.Reservation.Rows.Count > 0 Then
                                              
                    If Not DisplayRs.Reservation(0).IsPrepaymentNull AndAlso DisplayRs.Reservation(0).Prepayment > 0 Then
                        If DisplayRs.Reservation(0).Money = MerchantService_Currency Then
                            Amount = DisplayRs.Reservation(0).Prepayment
                        Else
                            With New PortalLibraries.MoneyExchangeService
                                Amount = .GetMoneyExchange(DisplayRs.Reservation(0).Money, MerchantService_Currency, DisplayRs.Reservation(0).Prepayment)
                            End With
                        End If
	                                    
                    Else
                        If DisplayRs.Reservation(0).Money.ToUpper = MerchantService_Currency Then
                            Amount = DisplayRs.Reservation(0).Total
                            'ElseIf DisplayRs.Reservation(0).AltMoney.ToUpper = cfg.CurrencyOnlineCCPaypment Then
                            '    Amount = DisplayRs.Reservation(0).AltTotal
                        Else
                            With New PortalLibraries.MoneyExchangeService
                                Amount = .GetMoneyExchange(DisplayRs.Reservation(0).Total, MerchantService_Currency, DisplayRs.Reservation(0).Total)
                            End With
                        End If
                    End If
                
                    Dim ReturnUrl As String = UrlPayment
                    ' If payItemOrder.PayOrderSocket(MerchantService_Url, MerchantService_Port, _
                                                   ' MerchantService_Account, _
                                                   ' DisplayRs.Reservation(0).ConfirmNumber, _
                                                   ' DisplayRs.Reservation(0).ConfirmNumber, "", _
                                                   ' Amount, Card, _
                                                   ' sCultureFormat.Substring(0, 2), _
                                                   ' ReturnUrl & "?idURL=" & idUrlPayment & _
                                                   ' "&idUser=" & Request.QueryString("idUser") & _
                                                   ' "&idReservation=" & DisplayRs.Reservation.Rows(0).Item("ConfirmNumber") & _
                                                   ' "&Language=" & Request.QueryString("Language")) Then
					Dim pay As New PaymentLibrary.PaymentBanamex2(MerchantService_Account, MerchantService_AccessCode, MerchantService_SecureHash, "https://banamex.dialectpayments.com/vpcpay")
					Dim Pay_Url As String =""
					Pay_Url = pay.PayOrder(DisplayRs.Reservation(0).ConfirmNumber, _
                                                    DisplayRs.Reservation(0).ConfirmNumber, "", _
                                                    Amount, Card, _
                                                    sCultureFormat.Substring(0, 2), _
                                                    ReturnUrl & "?idURL=" & idUrlPayment & _
                                                    "&idUser=" & Request.QueryString("idUser") & _
                                                    "&idReservation=" & DisplayRs.Reservation.Rows(0).Item("ConfirmNumber") & _
                                                    "&Language=" & Request.QueryString("Language")& "&sh=" & MerchantService_SecureHash, "MXN")
            												   
                    If Pay_Url<>"" Then
                        'Response.Redirect(payItemOrder.digitalOrder)
                        Response.Redirect(Pay_Url)
                        
                    Else
                        lblMsg.Text = RateManager.PortalCulture.GetString("01428")
                        lblMsg.ForeColor = Drawing.Color.Red
                    End If
                
                End If
                        
            Catch ex As Exception
                lblMsg.Text = RateManager.PortalCulture.GetString("01429")
                lblMsg.ForeColor = Drawing.Color.Red
            End Try
        Else
            Try
                Dim sCurrency As String = ConfigurationManager.AppSettings("MerchantService_Currency")
                Dim Card As New PaymentLibrary.CardPayment
                Dim pagina As String
                Dim url As String
                Dim noReservacion As String
                Dim idReservacion As String
                Dim xdoc As New XmlDataDocument(New reqHotelDisplay)
                Dim dsreq As reqHotelDisplay = CType(xdoc.DataSet, reqHotelDisplay)
                Dim DisplayRs As resHotelDisplay

                idReservacion = Regex.Replace(Request.QueryString("ReservationId"), "[^\d]", "")
                noReservacion = GetNoRes(idReservacion)
                Dim drR As reqHotelDisplay.HotelDisplayRow
                drR = dsreq.HotelDisplay.NewHotelDisplayRow
                drR.ConfirmNumber = noReservacion
                drR.Language = "en-US"
                dsreq.HotelDisplay.AddHotelDisplayRow(drR)

                Card.ccGateway = "ssl"
                Dim Amount As Decimal = 0
                With New WSHotelFacade.clsFADisplay
                    DisplayRs = .GetHotelDisplay(xdoc.DocumentElement)
                End With
            
                If Not DisplayRs Is Nothing AndAlso DisplayRs.Reservation.Rows.Count > 0 Then
                                              
                    If Not DisplayRs.Reservation(0).IsPrepaymentNull AndAlso DisplayRs.Reservation(0).Prepayment > 0 Then
                        If DisplayRs.Reservation(0).Money = sCurrency Then
                            Amount = DisplayRs.Reservation(0).Prepayment
                        Else
                            With New PortalLibraries.MoneyExchangeService
                                Amount = .GetMoneyExchange(DisplayRs.Reservation(0).Money, sCurrency, DisplayRs.Reservation(0).Prepayment)
                            End With
                        End If
	                                    
                    Else
                        If DisplayRs.Reservation(0).Money.ToUpper = sCurrency Then
                            Amount = DisplayRs.Reservation(0).Total
                            'ElseIf DisplayRs.Reservation(0).AltMoney.ToUpper = cfg.CurrencyOnlineCCPaypment Then
                            '    Amount = DisplayRs.Reservation(0).AltTotal
                        Else
                            With New PortalLibraries.MoneyExchangeService
                                Amount = .GetMoneyExchange(DisplayRs.Reservation(0).Total, sCurrency, DisplayRs.Reservation(0).Total)
                            End With
                        End If
                    End If
                End If
                       
                pagina = String.Format("/CallCenter/PayBancomer.aspx?idReservation={0}&Amount={1}&Currency={2}", Request.QueryString("ReservationId"), Amount, sCurrency)
                url = String.Concat(Request.ApplicationPath, pagina).Replace("//", "/").Replace("//", "/")
                Response.Redirect(url)
            Catch ex As Exception
                lblMsg.Text = RateManager.PortalCulture.GetString("01429")
                lblMsg.ForeColor = Drawing.Color.Red
            End Try
            
        End If
        
    End Sub
    
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Title = RateManager.PortalCulture.GetString("01430")
        lbltitle.Text = RateManager.PortalCulture.GetString("01430")
        btnCancelar.Text = RateManager.PortalCulture.GetString("01431")
        btnReintentar.Text = RateManager.PortalCulture.GetString("01432")
    End Sub
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="JavaScript" name="vs_defaultClientScript">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet"></link>
    <script type="text/javascript">
        function FireClose(lnk) {
            var w = parent.document.getElementById('cmdPayment');
            if (w) w.click();
        }
    </script>

</head>
<body>
    <form id="Form1" method="post" runat="server" style="position: absolute; left: 50%;
    top: 50%; margin-top: -58px; margin-left: -200px; width: 400px;">
    <div style="width: 400px; visibility: visible;" class="yui-module yui-overlay yui-panel">
        <div style="cursor: auto" class="hd">
            <div class="tl">
                <asp:Label ID="lbltitle" runat="server" Text="Pago de Reservación" 
                    CssClass="Titulo"></asp:Label>
            </div>
            <div class="tr">
            </div>
        </div>
        <div class="bd" style="width: 100%">
            <table align="center" style="width: 100%">
                <tr>
                    <td style="padding: 10px;">
                        <asp:Label ID="lblMsg" runat="server" Text="No se pudo realizar el pago."></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar Reservación" CssClass=Button />
            <asp:Button ID="btnReintentar" runat="server" Text="Reintentar Pago" CssClass="Button" />
        </div>
    </div>
    </form>
</body>
</html>
