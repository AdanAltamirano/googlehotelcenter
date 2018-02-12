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
    
    'cambiar el status de la reservación a activa.
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim cfg As New PortalLibraries.PortalPartnersCfg
        Dim confirmnum As String = ""
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
                btnCancelar.Visible = False
                btnReintentar.Visible = False
                UpdateReservation(Request.QueryString("ReservationId"), AutorizationNumber, Request.QueryString("Amount"), Request.QueryString("Currency"))
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectDefault", "<script>FireClose(1);" & "</scr" + "ipt>")
            Else                
                lblMsg.Text = RateManager.PortalCulture.GetString("01427")
            End If
        End If        
        'btnCancelar.Attributes.Add("onclick", "FireClose('1');")
    End Sub
                        
    Private Sub UpdateReservation(ByVal idreservacion As String, ByVal NoConfirmacion As String, ByVal sAmount As String, ByVal sCurrency As String, Optional ByVal AutorizationNumber As String = "", Optional ByVal ReceiptId As String = "")
        Try
            Dim Paysource As PortalLibraries.paymentSources
            Dim reqDisp As New Hotel_Display_RQ
            Dim ds As New DataSet
            
            Paysource = paymentSources.OnlinePaymentBancomer                   
            With New SqlCommand("spPagosReservacionesInsert", New SqlConnection(ConfigurationManager.AppSettings("HotelConnection")))
                .CommandType = Data.CommandType.StoredProcedure
                .Parameters.Add("@idReservacion", SqlDbType.Int).Value = Regex.Replace(idreservacion.ToString, "[^\d]", "")
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
            
            idReservacion = Regex.Replace(idReservacion, "[^\d]", "")
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
                sqlcmd.Parameters.Add("@MotivoCancelacion", SqlDbType.NVarChar, 250).Value = "Cancelacion en pago en linea (BBVA). " & PortalCulture.GetString("00780", True)
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
        If Not Request.QueryString("ReservationId") Is Nothing Then
            LocalCancel(Request.QueryString("ReservationId"), Request.QueryString("idUser"))
        End If
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectDefault", "<script>FireClose(1);" & "</scr" + "ipt>")
    End Sub
    
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
    
    
    Protected Sub btnReintentar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReintentar.Click
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
    End Sub
    
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
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
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</link>
		
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
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar Reservación" CssClass="Button" />
            <asp:Button ID="btnReintentar" runat="server" Text="Reintentar Pago" CssClass="Button" />
        </div>
    </div>
    </form>
</body>
</html>
