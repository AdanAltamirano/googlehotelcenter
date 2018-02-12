<%@ Page Language="VB" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    Public ReadOnly Property Cadena() As String
        Get
            Return Session("OnlinePaymentReservationsInProcess").ToString()
        End Get
    End Property

    Private Function CreaReferencia(ByVal value As String) As String
        Dim controlChars As New System.Collections.Generic.List(Of Char)
        controlChars.AddRange(System.Text.RegularExpressions.Regex.Replace(value, "^\d*(.*)?$", "$1").ToUpper().ToCharArray())
        value = System.Text.RegularExpressions.Regex.Replace(value, "^(\d*).*$", "$1")
        Dim i As Integer = 0
        For i = controlChars.Count To 1 Step -1
            Dim numeric As Integer = Convert.ToInt16(controlChars(i - 1))
            numeric += 1
            If numeric > 90 Then
                controlChars(i - 1) = Convert.ToChar("A")
            Else
                controlChars(i - 1) = Convert.ToChar(numeric)
                Exit For
            End If
        Next
        If i = 0 Then controlChars.Add("A")
        value += controlChars.ToArray()
        Return value
    End Function

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
    
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ds As New DataSet
        Dim MerchantIdOnlineCCPaypment As String
        Dim idUrlPaymentBancomer As String
        Dim HotelMerchantNamePayment As String
        Dim sNoReservacion As String = ""
        Dim sIdReservacion As String = ""
        Dim sCurrency As String = ""
        Dim sAmount As String = ""
        Dim MerchantResponseURL As String
        Dim MerchantPosURL As String
        Dim Paysource As PortalLibraries.paymentSources
        Dim dtotal As Double 
        Try
            If Not Request.QueryString("idReservation") Is Nothing Then
                sIdReservacion = Request.QueryString("idReservation")
            End If
            If Not Request.QueryString("Currency") Is Nothing Then
                sCurrency = Request.QueryString("Currency")
            End If
             If Not Request.QueryString("Currency") Is Nothing Then
                sAmount = Request.QueryString("Amount")
            End If
            
            If Not Request.QueryString("noConfirmacion") Is Nothing Then
                sNoReservacion = Request.QueryString("noConfirmacion")
            End If
            
            
            Double.TryParse(sAmount ,dtotal )
            MerchantIdOnlineCCPaypment = ConfigurationManager.AppSettings("MerchantIdOnlineCCPaypment")
            idUrlPaymentBancomer = ConfigurationManager.AppSettings("idUrlPayment")
            HotelMerchantNamePayment = ConfigurationManager.AppSettings("HotelMerchantNamePayment")
            MerchantResponseURL = ConfigurationManager.AppSettings("MerchantResponseURL")
            MerchantPosURL = ConfigurationManager.AppSettings("MerchantPosURL")
            
            
            If Not String.IsNullOrEmpty(sIdReservacion) Then
                sAmount= (dtotal*100)
                Ds_Merchant_Amount.Value = sAmount
                Ds_Merchant_Currency.Value = "484" 'MXN
                Ds_Merchant_Order.Value = Me.CreaReferencia(sIdReservacion) 'podria ser el idReservacion  ya que debe ser unico y max 12 caracteres
                Ds_Merchant_ProductDescription.Value = String.Format("Reservacion {0} Callcenter", sIdReservacion)
                Ds_Merchant_Titular.Value = "????"
                Ds_Merchant_MerchantCode.Value = MerchantIdOnlineCCPaypment
                'Ds_Merchant_MerchantURL.Value = "https://crs.univisit.com/webpayment/paybancomerresponse.aspx?idUrl=101" '"http://localhost:50606/PayBancomerResponse.aspx?idUrl=2222"
                'Ds_Merchant_MerchantURL.Value = String.Format("https://crs.univisit.com/webpayment/paybancomerresponsepost.aspx?idUrl={5}&BancomerPayment=true&ConfirmNum={0}&ReservationId={1}&amount={2}&merchant={3}&Currency={4}", sNoReservacion, Ds_Merchant_Order.Value, sAmount, Ds_Merchant_MerchantCode.Value, sCurrency, HotelMerchantIdOnlinePayment)
                'Ds_Merchant_UrlOK.Value = String.Format("https://crs.univisit.com/webpayment/paybancomerresponse.aspx?idUrl={6}&BancomerPayment=true&ConfirmNum={0}&ReservationId={1}&amount={2}&merchant={3}&Currency={4}&result={5}", sNoReservacion, Ds_Merchant_Order.Value, sAmount, Ds_Merchant_MerchantCode.Value, sCurrency, EncriptSha1("1_" & Ds_Merchant_Order.Value & "_" & sAmount & "_" & Ds_Merchant_MerchantCode.Value & "_h073l35r353rvnm3x1c0"), idUrlPaymentBancomer)
                'Ds_Merchant_UrlKO.Value = String.Format("https://crs.univisit.com/webpayment/paybancomerresponse.aspx?idUrl={6}&BancomerPayment=true&ConfirmNum={0}&ReservationId={1}&amount={2}&merchant={3}&Currency={4}&result={5}", sNoReservacion, Ds_Merchant_Order.Value, sAmount, Ds_Merchant_MerchantCode.Value, sCurrency, EncriptSha1("0_" & Ds_Merchant_Order.Value & "_" & sAmount & "_" & Ds_Merchant_MerchantCode.Value & "_h073l35r353rvnm3x1c0"), idUrlPaymentBancomer)                                

                Ds_Merchant_MerchantURL.Value = String.Format(MerchantPosURL + "?idUrl={5}&BancomerPayment=true&ConfirmNum={0}&ReservationId={1}&amount={2}&merchant={3}&Currency={4}", sNoReservacion, Ds_Merchant_Order.Value, sAmount, Ds_Merchant_MerchantCode.Value, sCurrency, idUrlPaymentBancomer)
                Ds_Merchant_UrlOK.Value = String.Format(MerchantResponseURL + "?idUrl={6}&BancomerPayment=true&ConfirmNum={0}&ReservationId={1}&amount={2}&merchant={3}&Currency={4}&result={5}", sNoReservacion, Ds_Merchant_Order.Value, sAmount, Ds_Merchant_MerchantCode.Value, sCurrency, EncriptSha1("1_" & Ds_Merchant_Order.Value & "_" & sAmount & "_" & Ds_Merchant_MerchantCode.Value & "_h073l35r353rvnm3x1c0"), idUrlPaymentBancomer)
                Ds_Merchant_UrlKO.Value = String.Format(MerchantResponseURL + "?idUrl={6}&BancomerPayment=true&ConfirmNum={0}&ReservationId={1}&amount={2}&merchant={3}&Currency={4}&result={5}", sNoReservacion, Ds_Merchant_Order.Value, sAmount, Ds_Merchant_MerchantCode.Value, sCurrency, EncriptSha1("0_" & Ds_Merchant_Order.Value & "_" & sAmount & "_" & Ds_Merchant_MerchantCode.Value & "_h073l35r353rvnm3x1c0"), idUrlPaymentBancomer)
                
                Ds_Merchant_MerchantName.Value = HotelMerchantNamePayment
                Ds_Merchant_ConsumerLanguage.Value = "1"
                Ds_Merchant_Terminal.Value = "1"
                Ds_Merchant_TransactionType.Value = "0"
                Ds_Merchant_MerchantData.Value = String.Format("Reservacion: {0}", sNoReservacion)
                Ds_Merchant_MerchantSignature.Value = EncriptSha1(Ds_Merchant_Amount.Value + Ds_Merchant_Order.Value + Ds_Merchant_MerchantCode.Value + Ds_Merchant_Currency.Value + Ds_Merchant_TransactionType.Value + "h073l35r353rvnm3x1c0")
                    
                'AQUI GRABAR LA REFERENCIA EN EL BD
                Try
                    Paysource = PortalLibraries.paymentSources.OnlinePaymentBancomer
                    'ds = GetRes(sIdReservacion)
                    'If (Not ds Is Nothing) AndAlso (ds.Tables(0).Rows.Count > 0) Then
                    '    idReserv = ds.Tables(0).Rows(0)("idReservacion")
                    sIdReservacion = Regex.Replace(sIdReservacion, "[^\d]", "")
                    With New SqlCommand("spPagosReservacionesInsert", New SqlConnection(ConfigurationManager.AppSettings("HotelConnection")))
                        .CommandType = Data.CommandType.StoredProcedure
                        .Parameters.Add("@idReservacion", SqlDbType.Int).Value = sIdReservacion
                        .Parameters.Add("@Status", SqlDbType.TinyInt).Value = 4
                        .Parameters.Add("@NoAutorizacion", SqlDbType.NVarChar, 50).Value = Ds_Merchant_Order.Value
                        .Parameters.Add("@PaymentSource", SqlDbType.Int).Value = Paysource
                        'Obtiene el monto total
                        Dim amount As Double = 0
                        Double.TryParse(sAmount, amount)
                        amount /= 100
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
                    ' End If
                    Me.lblError.InnerText = ""
                Catch ex As Exception
                    Me.lblError.InnerText = ""
                    While ex IsNot Nothing
                        Me.lblError.InnerHtml += "<p><b>" + ex.Message + "</b>" + ex.StackTrace + "</p>"
                        ex = ex.InnerException
                    End While
						
                End Try
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Sub
    
    Public Shared Function EncriptSha1(ByVal Input As String) As String
        Dim alg As System.Security.Cryptography.SHA1CryptoServiceProvider = New System.Security.Cryptography.SHA1CryptoServiceProvider
        Dim inputValue As Byte() = System.Text.Encoding.UTF8.GetBytes(Input)
        Dim outputValue As Byte() = alg.ComputeHash(inputValue)
        Return ByteArrayToString(outputValue)
    End Function

    Public Shared Function ByteArrayToString(ByVal ba As Byte()) As String
        Dim hex As System.Text.StringBuilder = New System.Text.StringBuilder(ba.Length * 2)
        For Each b As Byte In ba
            hex.AppendFormat("{0:x2}", b)
        Next
        Return hex.ToString()
    End Function
</script>

<%--<script>    if (confirm('')) { document.getElementById("formBancomer").submit(); } </script>--%>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Bancomer</title>
</head>
<body>

<%
    Dim url As String = "https://www.bancomer.eglobal.com.mx/sis/entradaPagos"
%>
    <form action="<%=url %>" method="post" id="formBancomer">
     <div style="">
             <input id="Ds_Merchant_Amount" name="Ds_Merchant_Amount" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_Currency" name="Ds_Merchant_Currency" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_Order"  name="Ds_Merchant_Order" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_ProductDescription"  name="Ds_Merchant_ProductDescription" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_Titular"  name="Ds_Merchant_Titular" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_MerchantCode"  name="Ds_Merchant_MerchantCode" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_MerchantURL"  name="Ds_Merchant_MerchantURL" runat="server" type="hidden" value=""/>
			 <input id="Ds_Merchant_UrlOK"  name="Ds_Merchant_UrlOK" runat="server" type="hidden" value=""/>
			 <input id="Ds_Merchant_UrlKO"  name="Ds_Merchant_UrlKO" runat="server" type="hidden" value=""/>       
             <input id="Ds_Merchant_MerchantName"  name="Ds_Merchant_MerchantName" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_ConsumerLanguage"  name="Ds_Merchant_ConsumerLanguage" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_MerchantSignature"  name="Ds_Merchant_MerchantSignature" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_Terminal"  name="Ds_Merchant_Terminal" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_TransactionType"  name="Ds_Merchant_TransactionType" runat="server" type="hidden" value=""/>
             <input id="Ds_Merchant_MerchantData"  name="Ds_Merchant_MerchantData" runat="server" type="hidden" value=""/>
	 </div>         
	 <div id="lblError" runat="server"></div>
	</form>
	<script>  document.getElementById("formBancomer").submit(); </script>
</body>
</html>
