Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Data.SqlClient

Public Class Vouchers
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                Dim Invoice As String = Oz.UniBilling.Common.Cryptography.DecryptUnivisitString(Request.QueryString("Invoice").Replace(" ", "+"))
                GetVouchers(Invoice)
                lblTitle.Text = "Pagos de la Factura: " & Invoice
            Catch ex As Exception
                lblError.Visible = True
                lblError.Text = PortalCulture.GetString("M0BT0000012")
            End Try
            
        End If
        

    End Sub

    Private Sub GetVouchers(ByVal Invoice As String)
        If Invoice.IndexOf("-") >= 0 Then
            Dim str() As String = Invoice.Split("-")
            Dim Serie As String = str(0)
            Dim Folio As String = str(1)

            Dim conection As New SqlConnection(AppSettings("UniFacturaDigital"))
            Dim spname As String = "SeleccionarComprobantesCompletos"
            Dim command As New SqlCommand(spname, conection)
            Try
                With command
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@Serie", Serie))
                    .Parameters.Add(New SqlParameter("@Folio", Folio))
                End With
                Dim adapter As New SqlDataAdapter(command)
                Dim Comprobantes As New DataSet
                adapter.Fill(Comprobantes)

                If Not Comprobantes Is Nothing AndAlso Comprobantes.Tables.Count > 0 Then
                    If Comprobantes.Tables(19).Rows.Count > 0 Then
                        dgComprobantes.DataSource = Comprobantes.Tables(19)
                        dgComprobantes.DataBind()
                    Else
                        lblError.Visible = True
                        lblError.Text = "No se encontraron pagos para esta factura."
                    End If
                End If
            Catch ex As Exception
                lblError.Visible = True
                lblError.Text = PortalCulture.GetString("M0BT0000012")
                If conection.State = ConnectionState.Open Then
                    conection.Close()
                End If
            Finally
                If conection.State = ConnectionState.Open Then
                    conection.Close()
                End If
            End Try
            
        End If
    End Sub

    Private Sub dgComprobantes_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgComprobantes.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim lnkDescargar As HyperLink
            Dim lbl As Label
            Dim FileName As String = Oz.UniBilling.Common.Cryptography.EncryptUnivisitString((Request.QueryString("Invoice")))

            Dim AppliedPaymentID As String = String.Empty
            If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "AppliedPaymentID")) Then
                AppliedPaymentID = DataBinder.Eval(e.Item.DataItem, "AppliedPaymentID")
            End If

            If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "Monto")) Then
                lbl = CType(e.Item.FindControl("glblTotal2"), Label)
                lbl.Text = CType(DataBinder.Eval(e.Item.DataItem, "Monto"), Decimal).ToString("c") & " MXN"
            End If

            If AppliedPaymentID = String.Empty Then
                lnkDescargar.Visible = False
            Else
                lnkDescargar = e.Item.FindControl("hypPdfComp")

                lnkDescargar.NavigateUrl = String.Concat(AppSettings("DIR_INVOICE_PRINTING"), "DigitalInvoiceHelper.aspx") & "?action=Download&Invoice=" & Server.UrlEncode(Request.QueryString("Invoice").Replace(" ", "+")) & "&format=" & Oz.UniBilling.Common.DigitalInvoiceFormat.Pdf.ToString & "&PaymentID=" & AppliedPaymentID
                lnkDescargar.ToolTip = PortalCulture.GetString("00906")
                lnkDescargar.Visible = True

                lnkDescargar = e.Item.FindControl("hypXmlFC")

                lnkDescargar.NavigateUrl = String.Concat(AppSettings("DIR_INVOICE_PRINTING"), "DigitalInvoiceHelper.aspx") & "?action=Download&Invoice=" & Server.UrlEncode(Request.QueryString("Invoice").Replace(" ", "+")) & "&format=" & Oz.UniBilling.Common.DigitalInvoiceFormat.Xml.ToString & "&PaymentID=" & AppliedPaymentID
                lnkDescargar.ToolTip = PortalCulture.GetString("00907")
                lnkDescargar.Visible = True

            End If

            CType(e.Item.FindControl("lblMsgSelloSat"), Label).Text = String.Empty
        End If
    End Sub

End Class