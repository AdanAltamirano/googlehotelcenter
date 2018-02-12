<%@ Page Language="vb" Inherits="RateManager.WaitList" %>
<%@ Import namespace="Portal.Hotel.DataAccess" %>
<%@ Import namespace="RateManager" %>
<%@ Import namespace="Portal.General.Facade" %>
<%@ Import namespace="Portal.General.Common.Data" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.IO" %>

<%@ Register Src="../../ListDate/Date.ascx" TagName="Date" TagPrefix="uc1" %>

<script runat="server">

    Private Const EXPIRETIME As Integer = 2 
    
    Private Enum Columns
        Id = 0
        Reference = 1
        HotelName = 2
        [Date] = 3
        Name = 4
        Email = 5
        CheckIn = 6
        Nights = 7
        Options = 8
    End Enum
        
    Private _currencyCodes As Dictionary(Of String, String)
    
    Private Const DATEFORMAT As String = "dd/MMM/yyy"
        
    Protected Function GetLabel(ByVal key As String) As String
        Return PortalCulture.GetString(key)
        'Return String.Empty
    End Function
    
    Protected ReadOnly Property CurrentHotel() As Integer
        Get
            Return Me.cInfoActual.Hotel
        End Get
    End Property
    
    Protected ReadOnly Property CurrenCompany() As Integer
        Get
            Return Me.cInfoActual.Empresa
        End Get
    End Property
    
    Protected ReadOnly Property CurrentUser() As String
        Get
            Dim temp As String = String.Empty
            Try
                temp = (New AuthUser()).UserInfoName
            Catch ex As Exception
            End Try
            Return temp
        End Get
    End Property
        
    Private Sub ShowLink(ByVal show As Boolean)
        Dim str As String = If(show, "block", "none")
        Dim str2 As String = If(Not show, "block", "none")

        Me.hplShow.Style.Add("display", str)
        Me.hplHide.Style.Add("display", str2)
    End Sub
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
                
        If Not Me.IsPostBack Then
            
            Me.lstTipoFechas.Items.Clear()
            Me.lstTipoFechas.Items.Add(New ListItem(Me.GetLabel("00368"), 1))
            Me.lstTipoFechas.Items.Add(New ListItem(Me.GetLabel("M0BT0000235"), 2))
            
            Me.txtHotelName.Text = Me.cInfoActual.HotelName
            
            Me.lstOrderField.Items.Clear()
            Me.lstOrderField.Items.Add(New ListItem(Me.GetLabel("00695"), "id asc"))
            Me.lstOrderField.Items.Add(New ListItem(Me.GetLabel("00158"), "hotelName asc"))
            Me.lstOrderField.Items.Add(New ListItem(Me.GetLabel("M0BT0000235"), "date asc"))
            Me.lstOrderField.Items.Add(New ListItem(Me.GetLabel("00073"), "firstName asc, lastName asc"))
            Me.lstOrderField.Items.Add(New ListItem(Me.GetLabel("00368"), "checkIn asc"))
            
            Me.txtDesde.maxYear = Today.Year + 2
            Me.txtDesde.minYear = Today.Year - 1
            Me.txtDesde.selectedDate = Today
            Me.txtHasta.maxYear = Today.Year + 2
            Me.txtHasta.minYear = Today.Year - 1
            Me.txtHasta.selectedDate = Today.AddDays(30)
            'Me.chkFechas.Checked = True
            Me.CargaMonedas()
            Me.RefreshData(sender, e)
                    ShowLink(True)
            
            Me.hplShow.Text = PortalCulture.GetString("01454")
            Me.hplHide.Text = PortalCulture.GetString("01455")

            Me.hplHide.NavigateUrl = String.Format("javascript:FireShowOrHidden('{0}','{1}','{2}','{3}',{4})", pnlSearch.ClientID, hplHide.ClientID, hplShow.ClientID, hdnSearch.ClientID, "false")
            Me.hplShow.NavigateUrl = String.Format("javascript:FireShowOrHidden('{0}','{1}','{2}','{3}',{4})", pnlSearch.ClientID, hplHide.ClientID, hplShow.ClientID, hdnSearch.ClientID, "true")

        End If
                
    End Sub
        
    Private Sub CargaMonedas()
        Dim data As MonedaDatos = (New MonedaSistema).GetMonedaListIdName()
        Me.lstBookingCurrencies.Items.Clear()
        Me.lstNotificationCurrencies.Items.Clear()
        Me._currencyCodes = New Dictionary(Of String, String)
        If data IsNot Nothing AndAlso data.Tables.Contains(MonedaDatos.MONEDA_TABLE) Then
            For Each item As Data.DataRow In data.Tables(MonedaDatos.MONEDA_TABLE).Rows
                Me.lstBookingCurrencies.Items.Add(New ListItem(item(MonedaDatos.FIELD_NAME), item(MonedaDatos.FIELD_CODIGO)))
                Me.lstNotificationCurrencies.Items.Add(New ListItem(item(MonedaDatos.FIELD_NAME), item(MonedaDatos.FIELD_CODIGO)))
                Me._currencyCodes.Add(item(MonedaDatos.FIELD_PKID), item(MonedaDatos.FIELD_CODIGO))
            Next
            Me.lstBookingCurrencies.SelectedIndex = Me.lstBookingCurrencies.Items.IndexOf(Me.lstBookingCurrencies.Items.FindByValue(Me._currencyCodes(Me.HotelInfo(HotelDatos.FIELD_IDMONEDA))))
            Me.lstNotificationCurrencies.SelectedIndex = Me.lstNotificationCurrencies.Items.IndexOf(Me.lstNotificationCurrencies.Items.FindByValue(Me._currencyCodes(Me.HotelInfo(HotelDatos.FIELD_IDMONEDA))))
        End If
    End Sub
    
    Protected Sub ChangePage(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles lstClientes.PageIndexChanged
        
        Dim table As Data.DataTable = Me.GetData()
        Dim pages As Integer = table.Rows.Count \ Me.lstClientes.PageSize
        If table.Rows.Count Mod Me.lstClientes.PageSize > 0 Then pages += 1
        
        If e.NewPageIndex >= pages Then
            Me.lstClientes.CurrentPageIndex = pages - 1
        Else
            Me.lstClientes.CurrentPageIndex = e.NewPageIndex
        End If
        Me.lstClientes.DataSource = table
        Me.lstClientes.DataBind()
        
    End Sub
    
    Private Function GetData() As Data.DataTable
        Dim data As New Data.DataTable
        Dim controller As New WaitListDataAccess()
        Dim status As Integer = 0
        Dim expiredHours As Integer = 24

        If Me.chkStatusInProcess.Checked Then status += 1
        If Me.chkStatusAccepted.Checked Then status += 2
        If Me.chkStatusRejected.Checked Then status += 4
        If Me.chkStatusExpired.Checked Then status += 8
        If status = 0 Then status = 1
        
        Dim hotelName As String = Me.txtHotelName.Text.Trim()
        If Me.chkAllHotels.Checked Then hotelName = String.Empty

        If Me.chkFechas.Checked Then
            data = controller.GetList(0, Me.txtDesde.selectedDate, Me.txtHasta.selectedDate, Me.lstTipoFechas.Items(Me.lstTipoFechas.SelectedIndex).Value, status, Me.txtNombre.Text, Me.txtReferencia.Text, expiredHours, hotelName)
        Else
            data = controller.GetList(0, status, Me.txtNombre.Text, Me.txtReferencia.Text, expiredHours, hotelName)
        End If
        
        data.DefaultView.Sort = Me.lstOrderField.SelectedItem.Value
        
        
        Dim currentCorporate As Integer = 0
        Dim currentAsociation As Integer = 0
        
        If ConfigurationManager.AppSettings("IdCorporate") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdCorporate"), currentCorporate)
        If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), currentAsociation)
        
        If currentCorporate > 0 OrElse currentAsociation > 0 OrElse Not Me.IsSupervisor Then
            
            Dim filter As String = String.Empty
            
            'Obtenemos los portales requeridos
            If Not Me.isUserChain AndAlso Not Me.IsUsuarioHotelAssociation Then
                filter += "idHotel IN ('" + Me.cInfoActual.Hotel.ToString() + "') AND "
            End If
            
            filter += "portal IN ("
            
            Dim portals As Portal.Catalogos.Common.Data.clsCommonPortales
            With New Portal.Catalogos.Facade.clsFacadePortales()
                If Me.isUserChain AndAlso Me.IdCorporativoUserChain > 0 Then
                    portals = .GetPortalsByCorporate(Me.IdCorporativoUserChain)
                ElseIf Me.IsUsuarioHotelAssociation AndAlso Me.IdAsociation > 0 Then
                    portals = .GetPortalsByAsociation(Me.IdAsociation)
                ElseIf currentCorporate > 0 Then
                    portals = .GetPortalsByCorporate(currentCorporate)
                ElseIf currentAsociation > 0 Then
                    portals = .GetPortalsByAsociation(currentAsociation)
                End If
            End With
            
            Dim sep As String = String.Empty
            If portals IsNot Nothing AndAlso portals.Tables.Contains(portals.TABLA_PORTALES) AndAlso portals.Tables(portals.TABLA_PORTALES).Rows.Count > 0 Then
                For Each item As Data.DataRow In portals.Tables(portals.TABLA_PORTALES).Rows
                    'If item("IsPublic") = 0 Then
                    filter += sep + "'" + item(portals.FLD_IDPORTAL).ToString() + "'"
                    sep = ","
                    'End If
                Next
                'Else
                'filter += "'0'"
            End If
            
            If sep.Length = 0 Then filter += "'0'"
            filter += ")"
            data.DefaultView.RowFilter = filter
        End If
        
        
        Return data.DefaultView.ToTable()
        
    End Function
        
    Private Sub Filtro()
        Dim sFiltro As String

        sFiltro = PortalCulture.GetString("01380") & ","
        sFiltro &= If(chkFechas.Checked, String.Format(PortalCulture.GetString("01370"), lstTipoFechas.SelectedItem.Text), " ")        
        sFiltro &= String.Format(PortalCulture.GetString("01374"), txtDesde.selectedDate.ToString("dd/MM/yyyy"), txtHasta.selectedDate.ToString("dd/MM/yyyy"))
        sFiltro &= String.Format(PortalCulture.GetString("01373"), If(Me.chkAllHotels.Checked, PortalCulture.GetString("00172"), cInfoActual.HotelName))
        sFiltro &= If(String.IsNullOrEmpty(Me.txtNombre.Text), "", String.Format(PortalCulture.GetString("01377"), txtNombre.Text))
        sFiltro &= If(String.IsNullOrEmpty(txtReferencia.Text), "", String.Format(PortalCulture.GetString("01381"), Me.txtReferencia.Text))
                        
        Dim schk As String = PortalCulture.GetString("01319") & ": ,"
        schk &= If(chkStatusInProcess.Checked, ", " & PortalCulture.GetString("00439"), "")
        schk &= If(chkStatusAccepted.Checked, ", " & PortalCulture.GetString("01268"), "")
        schk &= If(chkStatusRejected.Checked, ", " & PortalCulture.GetString("01318"), "")
        schk &= If(chkStatusExpired.Checked, ", " & PortalCulture.GetString("01267"), "")
        sFiltro &= schk
        lblFiltro.Text = sFiltro.Replace(",,", "").Replace(", ,", "")
        
    End Sub
    
    Protected Sub RefreshData(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click        
        Filtro()
        Me.lstClientes.CurrentPageIndex = 0
        Me.lstClientes.DataSource = Me.GetData()
        
        Me.lstClientes.Columns(Columns.HotelName).Visible = Not (Me.cInfoActual.HotelName.Trim().ToLower() = Me.txtHotelName.Text.Trim().ToLower())
        
        Me.lstClientes.DataBind()
        
    End Sub
    
    Protected Sub RemoveCustomer(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemove.Click
        Dim value As Integer = 0
        If Integer.TryParse(Me.varCurrentCustomer.Value, value) AndAlso value > 0 Then
            Try
                Dim controller As New WaitListDataAccess()
                controller.DeleteItem(value)
            Catch ex As Exception
            End Try
            
        End If
        Me.RefreshData(sender, e)
        
    End Sub
    
    Public Shared Function ParseAbsolutePath(ByVal path As String, ByVal Prov As PortalPartnersCfg) As String
        If (path.IndexOf("~") = 0) Then
            path = path.Replace("~", "")
            path = path.Replace("//", "/")
        End If
        Return Prov.UrlSite & path
    End Function
    
    Protected Sub SendNotification(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendNotify.Click
        Dim value As Integer = 0
        If Integer.TryParse(Me.varCurrentCustomer.Value, value) AndAlso value > 0 Then
            
            Try
                Dim controller As New WaitListDataAccess()
                Dim data As Data.DataTable = controller.GetItem(value, PortalCulture.GetIDCulture())
                Dim info As Data.DataRow = Nothing
                If data IsNot Nothing AndAlso data.Rows.Count > 0 Then
                    info = data.Rows(0)
                    
                    Dim url As String = info("source").ToString()
                    If Not url.EndsWith("/") Then url += "/"
                    
                    Dim guid As String = String.Empty
                    'url += "hotel/externalrequest.aspx?type=booking&param="
                    
                    Dim price As Double = 0.0
                    Dim plusTaxes As Boolean = True
                    If info("plusTax").ToString() = "0" Then plusTaxes = False
                    
                    Dim taxes As Double = Convert.ToDouble(info("tax").ToString())
                    If Double.TryParse(Me.txtNotificationPrice.Text, price) AndAlso Not plusTaxes Then
                        price = price * (1 + (taxes / 100))
                    End If
                    
                    Dim lifeDays As Integer = 0
                    
                    If ConfigurationManager.AppSettings("wlNotificationLifeDays_" + info("portal").ToString()) IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("wlNotificationLifeDays_" + info("portal").ToString()), lifeDays)
                                            
                    'ESTA VERSION CALCULABA LOS IMPUESTO CON LOS DEL HOTEL SELECCIONADO NO CON LSO DEL HOTEL DE LA PETICION
                    'Dim price As Double = 0.00
                    'Dim plusTaxes As Boolean = Not Convert.ToBoolean(Me.HotelInfo(HotelDatos.FIELD_PLUSTAX))
                    'Dim taxes As Double = Convert.ToDouble(Me.HotelInfo(HotelDatos.FIELD_IMPUESTO))
                    'If Double.TryParse(Me.txtNotificationPrice.Text,price) andalso plusTaxes then
                    '    price = price * (1 + (taxes/100))
                    'End If                    
                    
                    'params += "{"
                    'params += """checkIn"":""" + Convert.ToDateTime(info("checkIn")).ToString("yyyyMMdd") + ""","
                    'params += """checkOut"":""" + Convert.ToDateTime(info("checkOut")).ToString("yyyyMMdd") + ""","
                    'params += """hotel"":""" + info("idHotel").ToString() + ""","
                    'params += """rooms"":""" + info("rooms").ToString() + ""","
                    'params += """adults"":""" + info("adults").ToString() + ""","
                    'params += """children"":""" + info("children").ToString() + ""","
                    'params += """currency"":""" + info("currency").ToString() + ""","
                    'params += """rateCode"":""" + info("rateCode").ToString() + """"
                    guid = controller.SetNotifiedItem(value, price, Me.txtNotificationComment.Text, Me.CurrentUser, Me.lstNotificationCurrencies.Items(Me.lstNotificationCurrencies.SelectedIndex).Value, True, lifeDays)
                    'params += "}"
                    
                    'url += HttpUtility.UrlEncode(crypto.EncryptString128Bit(params, crypto.PublicKey))
                    
                    
                    Try
                        Dim Mail As emailTemplates.Template = New emailTemplates.Template()
                        
                        Mail.Idioma = PortalCulture.GetCulture().ToString()
                        
                        Try
                            Mail.TemplateName = "TH_AvailabilityMail"
                        Catch ex As Exception
                        End Try
                        
                        Mail.To = info("email").ToString()
                        Mail.Bcc = ConfigurationManager.AppSettings("UnivisitMail")
                        Mail.Html = True
                        
                        Dim Prov As New PortalPartnersCfg
                        Prov.LoadPartnerById(info("portal"))
                        
                        Mail.AddParameter("HEADER") = Prov.EmailHeader
                        Mail.AddParameter("FOOTER") = Prov.EmailFooter
                        Mail.AddParameter("LINKCSS") = "<link href='" & ParseAbsolutePath(Prov.StyleSheets, Prov) & "correos.css' type='text/css' rel='stylesheet'>"

                        Mail.AddParameter("CHECKIN") = Convert.ToDateTime(info("checkIn")).ToString(DATEFORMAT)
                        Mail.AddParameter("CHECKOUT") = Convert.ToDateTime(info("checkOut")).ToString(DATEFORMAT)
                        Mail.AddParameter("HOTELNAME") = info("hotelName")
                        Mail.AddParameter("CUSTOMER") = info("name")
                        Mail.AddParameter("URL") = url + "hotel/externalrequest.aspx?type=booking&param=" + HttpUtility.UrlEncode(crypto.EncryptString128Bit("{""guid"":""" + guid + """}", crypto.PublicKey))
                        Mail.AddParameter("REJECTURL") = url + "hotel/externalrequest.aspx?type=waitlist&param=" + HttpUtility.UrlEncode(crypto.EncryptString128Bit("{""op"":""reject"",""who"":""" + info("email").ToString() + """,""guid"":""" + guid + """}", crypto.PublicKey))
                        Mail.AddParameter("TOTAL") = price.ToString("###,###,##0.00")
                        Mail.AddParameter("CURRENCY") = Me.lstNotificationCurrencies.Items(Me.lstNotificationCurrencies.SelectedIndex).Value
                        Mail.AddParameter("COMMENT") = Me.txtNotificationComment.Text + " "
                        
                        Mail.Send()
                        
                        lifeDays = 0

                    Catch Emsg As Exception
                    Finally
                    End Try
                    
                End If
            Catch ex As Exception
        End Try
            
        End If
        Me.RefreshData(sender, e)
    End Sub
    
    Protected Sub Book(ByVal sender As Object, ByVal e As EventArgs) Handles btnBook.Click
        
        Dim value As Integer = 0
        If Integer.TryParse(Me.varCurrentCustomer.Value, value) AndAlso value > 0 Then
            
            Try
                Dim controller As New WaitListDataAccess()
                Dim data As Data.DataTable = controller.GetItem(value, PortalCulture.GetIDCulture())
                Dim info As Data.DataRow = Nothing
                If data IsNot Nothing AndAlso data.Rows.Count > 0 Then
                    info = data.Rows(0)
                    Dim plusTaxes As Boolean = True
                    If info("plusTax").ToString() = "0" Then plusTaxes = False
                    
                    Dim price As Double = 0
                    Dim taxes As Double = Convert.ToDouble(info("tax").ToString())
                    If Double.TryParse(Me.txtBookingPrice.Text, price) AndAlso Not plusTaxes Then
                        price = price * (1 + (taxes / 100))
                    End If
                
                    If price > 0 Then
                                            
                        Dim guid As String = controller.SetNotifiedItem(value, price, String.Empty, Me.CurrentUser,Me.lstBookingCurrencies.Items(Me.lstBookingCurrencies.SelectedIndex).Value)
                                    
                        Dim url As String = String.Empty
                        url += Me.GeRequestApplicationPath("/callcenter/PassiveBooking.aspx") + "?"
                        url += "guid=" + HttpUtility.UrlEncode(guid)
                    
                        Me.Response.Redirect(url, False)
                            
                    End If
                
                End If
                
            Catch ex As Exception
            End Try
            
        End If
        Me.RefreshData(sender, e)
        
    End Sub
    
    Protected Sub lstClientes_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim data As Data.DataRowView = Convert.ChangeType(e.Item.DataItem, gettype(Data.DataRowView))
            
            If data IsNot Nothing Then
                Select Case Convert.ToInt32(data("Status"))
                    Case 2
                        e.Item.CssClass += If(e.Item.ItemType = ListItemType.AlternatingItem, "dgAlternate", "dgItem") + "Not"
                    Case 4
                        e.Item.CssClass += If(e.Item.ItemType = ListItemType.AlternatingItem, "dgAlternate", "dgItem") + "Can"
                    Case 8
                        e.Item.CssClass += If(e.Item.ItemType = ListItemType.AlternatingItem, "dgAlternate", "dgItem") + "Exp"
                End Select
            End If
            
        End If
    End Sub
    
    Protected Sub btnActivate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim value As Integer = 0
        If Integer.TryParse(Me.varCurrentCustomer.Value, value) AndAlso value > 0 Then
            Try
                Dim controller As New WaitListDataAccess()
                Dim data As Data.DataTable = controller.GetItem(value, PortalCulture.GetIDCulture())
                Dim info As Data.DataRow = Nothing
                If data IsNot Nothing AndAlso data.Rows.Count > 0 Then
                    info = data.Rows(0)
                    Dim lifeDays As Integer = 0
                    If ConfigurationManager.AppSettings("wlNotificationLifeDays_" + info("portal").ToString()) IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("wlNotificationLifeDays_" + info("portal").ToString()), lifeDays)
                    
                    controller.ReactivateItem(value, lifeDays)
                End If
                                
            Catch ex As Exception
            End Try
            
        End If
        Me.RefreshData(sender, e)
    End Sub
    
    Protected Sub lstClientes_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(Columns.Id).Text = ""
            e.Item.Cells(Columns.Reference).Text = Me.GetLabel("01235")
            e.Item.Cells(Columns.HotelName).Text = Me.GetLabel("00158")
            e.Item.Cells(Columns.Date).Text = Me.GetLabel("M0BT0000235")
            e.Item.Cells(Columns.Name).Text = Me.GetLabel("00073")
            e.Item.Cells(Columns.Email).Text = Me.GetLabel("00163")
            e.Item.Cells(Columns.CheckIn).Text = Me.GetLabel("M000078")
            e.Item.Cells(Columns.Nights).Text = Me.GetLabel("M0BT0000070")
            e.Item.Cells(Columns.Options).Text = ""
            
        End If
    End Sub
    
    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim value As Integer = 0
        If Integer.TryParse(Me.varCurrentCustomer.Value, value) AndAlso value > 0 Then
            Try
                Dim controller As New WaitListDataAccess()
                controller.CancelItem(value, Me.CurrentUser, Me.txtRejectComment.Text)
                'envia el correo de rechazo
                Try
                    Dim data As Data.DataTable = controller.GetItem(value, PortalCulture.GetIDCulture())
                    Dim info As Data.DataRow = Nothing
                    If data IsNot Nothing AndAlso data.Rows.Count > 0 Then
                        info = data.Rows(0)
                
                        Dim Mail As emailTemplates.Template = New emailTemplates.Template()
                        
                        Mail.Idioma = PortalCulture.GetCulture().ToString()
                        
                        Try
                            Mail.TemplateName = "TH_WaitListCancel"
                        Catch ex As Exception
                        End Try
                        
                        Mail.To = info("email").ToString()
                        'Mail.Bcc = ConfigurationManager.AppSettings("UnivisitMail")
                        Mail.Html = True
                        
                        Dim Prov As New PortalPartnersCfg
                        Prov.LoadPartnerById(info("portal"))
                        
                        Mail.AddParameter("HEADER") = Prov.EmailHeader
                        Mail.AddParameter("FOOTER") = Prov.EmailFooter
                        Mail.AddParameter("LINKCSS") = "<link href='" & ParseAbsolutePath(Prov.StyleSheets, Prov) & "correos.css' type='text/css' rel='stylesheet'>"
                        
                        Mail.AddParameter("REFERENCE") = info("Id").ToString().PadLeft(8, "0")
                        Mail.AddParameter("HOTEL") = info("hotelName").ToString()
                        Mail.AddParameter("COMMENT") = Me.txtRejectComment.Text
                        Mail.AddParameter("CUSTOMER") = info("name")
                                                
                        Mail.Send()
                    End If
                Catch ex As Exception
                End Try
                
            Catch ex As Exception
            End Try
            
        End If
        Me.RefreshData(sender, e)
    End Sub
        
    Protected Sub btnExcelExport_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dim currenPaging As Integer = Me.lstClientes.PageSize
        Dim currenPage As Integer = Me.lstClientes.CurrentPageIndex
        
        Me.lstClientes.Columns(Columns.Options).Visible = False
        Me.lstClientes.AllowPaging = False
        Me.lstClientes.AllowSorting = False
        Me.lstClientes.DataSource = Me.GetData()
        Me.lstClientes.DataBind()
        
        'Me.lstClientes.Columns.RemoveAt(Columns.Options)
        
        Dim sb As StringBuilder = New StringBuilder()
        
        Dim sw As New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Dim pagina As Page = New Page
        'Dim form As New HtmlForm()
        'pagina.EnableEventValidation = False
        'pagina.DesignerInitialize()
        'pagina.Controls.Add(form)
        'form.Controls.Add(Me.lstClientes)
        'pagina.RenderControl(htw)
        
        Me.lstClientes.RenderControl(htw)
        Me.lstClientes.Columns(Columns.Options).Visible = True
        'Me.lstClientes.PageSize = currenPaging
        Me.lstClientes.CurrentPageIndex = currenPage
        Me.RefreshData(sender, e)
        
        Response.Clear()
        Response.ClearContent()
        Response.ClearHeaders()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Solicitudes.xls")
        Response.ContentType = "application/ms-excel"
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write("<style> .text { mso-number-format:\@; } </style>")
        Response.Write(sb.ToString())
        Response.End()
    End Sub
</script>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 transitional//EN" >--%><html>
<head id="Head1" runat="server">
    <title><%=Me.GetLabel("01233")%></title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.min.js").Replace("//","/") %>'></script>
    
    <style type="text/css">            
        .dgItemCan{background-color:rgb(254,189,180);}
        .dgAlternateCan{background-color:rgb(253,169,157);}      
        .dgItemExp{background-color:rgb(255,213,64);}
        .dgAlternateExp{background-color:rgb(255,232,149);}
        .dgItemNot{background-color:rgb(204,227,198);}
        .dgAlternateNot{background-color:rgb(175,211,165);}
        
        #pnlStatus .sample{display:inline-block;; height:12px; width:16px; border:1px solid; }
        .accepted {background-color:rgb(175,211,165);}
        .rejected {background-color:rgb(253,169,157);}
        .expired {background-color:rgb(255,232,149);}
        
        #boxInfo {border: 1px solid rgb(40, 111, 192); display: none; width:95%;}
        
        #BookingContainer p {margin:0;}
        
        td.Label{
            text-align: right; font-weight: bold;
        }
        
        .row *{
            vertical-align:middle;
        }
        
        @media screen
        {
            .printable{
                display:none;
            }
        }        
        
        /*Printing*/
        @media print
        {
            #BookingContainer
            {
                background-color:transparent;
                border:none;
                width:100%;
            }

            #BookingContainer .screen
            {
                display:none;
            }

            #boxInfo{
                border:none;
            }

            .dgHeader span{
                font-size:14px;
                font-weight:bold;
            }
                        
            .dgHeader .dgItem 
            {
                font-size:14px;
                font-weight:bold;
            }
            
        }
    </style>
    
</head>
<body>
    <%
        Dim sysCulture As System.Globalization.CultureInfo
        sysCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString())
    %>
    <form id="Form1" runat="server" method="post">
    <div class="clear">		    
		      <div class="mDiv"></div>
		        <div>
                    <asp:Label ID="Label3" runat="server" CssClass="tituloSeccion" EnableViewState="False"><%=Me.GetLabel("01233")%></asp:Label>
		        </div>
	</div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="900" border="0" style=" height: 600px;" >       
        <tr class="printable">
            <td class="Titulo" align="center">
                <asp:Label ID="Label1" runat="server" CssClass="TituloForma" EnableViewState="False"><%=Me.GetLabel("01325")%></asp:Label>
            </td>
        </tr>
        <tr class="screen">
            <td align="center">
            <table style="text-align:right; width:100%"><tr><td> <asp:hyperlink id="hplShow" runat="server" CssClass="showOptions">Mostrar</asp:hyperlink>
<asp:hyperlink id="hplHide" runat="server" CssClass="hideOptions">Ocultar</asp:hyperlink>
<asp:HiddenField ID="hdnSearch" runat="server" Value="0" />
</td></tr></table>

<asp:Panel ID= "pnlSearch" runat = server  style= "display:block;">
                <table id="Table2" border="0" cellspacing="1" cellpadding="1" width="100%">
                    <tr class="dgHeader">
                        <td class="dgitem" align="center" colspan="4">
                            <span><%=GetLabel("00479")%></span>
                        </td>
                    </tr>
                    <tr id="pnlHotelName">
                        <td align="right">
                            <span class="clsLabel"><%=GetLabel("00158")%>&nbsp;:&nbsp;</span>  
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="txtHotelName" runat="server" Width="400px" EnableViewState="true"></asp:TextBox> 
                            <% Me.chkAllHotels.Text = Me.GetLabel("M0BT0000248")%>
                            <asp:CheckBox ID="chkAllHotels" runat="server" Text="" />
                        </td>
                    </tr>                    
                    <tr>
                        <td align="right">
                            <span class="clsLabel"><%=GetLabel("M000114")%>&nbsp;:&nbsp;</span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtNombre" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td align="right">
                            <span class="clsLabel"><%=GetLabel("00695")%>&nbsp;:&nbsp;</span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReferencia" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>            
                    
                    <tr>
                        <td colspan="4" style="padding-left:20px;">
                            <asp:CheckBox ID="chkFechas" runat="server" Checked="false" /><%=Me.GetLabel("00517")%>
                          </td>
                    </tr>  
                    <tr id="pnlDatesSelection">
                        <td align="right">
                            <span class="clsLabel"><%=GetLabel("01246")%>&nbsp;:&nbsp;</span>
                        </td>
                        <td align="left" colspan="3">
                            <asp:DropDownList ID="lstTipoFechas" runat="server" EnableViewState="true"></asp:DropDownList>
                        </td>
                    </tr>                  
                    <tr id="pnlDates">
                        <td align="right">
                            <span class="clsLabel"><%=GetLabel("00108")%>&nbsp;:&nbsp;</span>
                        </td>
                        <td align="left">
                            <uc1:date ID="txtDesde" runat="server" />
                        </td>
                        <td align="right">
                            <span class="clsLabel"><%=GetLabel("00109")%>&nbsp;:&nbsp;</span>
                        </td>
                        <td align="left">
                            <uc1:date ID="txtHasta" runat="server" />
                        </td>
                    </tr>     
                    <tr id="pnlStatus">
                        <td colspan="4" style="padding-left:20px;">
                            <%=Me.GetLabel("01319")%>&nbsp;:&nbsp;
                            <input type="checkbox" id="chkStatusInProcess" checked="checked" runat="server" /><div class="sample inProcess"></div>&nbsp;<%=Me.GetLabel("00439")%>&nbsp;                            
                            <input type="checkbox" id="chkStatusAccepted" runat="server" /><div class="sample accepted"></div>&nbsp;<%=Me.GetLabel("01268")%>&nbsp;
                            <input type="checkbox" id="chkStatusRejected" runat="server" /><div class="sample rejected"></div>&nbsp;<%=Me.GetLabel("01318")%>&nbsp;
                            <input type="checkbox" id="chkStatusExpired" runat="server" /><div class="sample expired"></div>&nbsp;<%=Me.GetLabel("01267")%>&nbsp;
                        </td>
                    </tr>  
                    <tr>
                        <td colspan="3" style="padding-left:20px;">
                            <%=Me.GetLabel("01333")%>&nbsp;:&nbsp;
                            <asp:DropDownList style="vertical-align:middle;" ID="lstOrderField" runat="server" EnableViewState="true"></asp:DropDownList>
                        </td>
                    </tr>  
                    <tr>
                        <td colspan="4" Style = " text-align:right; padding-right:20px;  " >     
                            <% Me.btnFilter.Text = Me.GetLabel("M000637")%>                       
                            <asp:Button ID="btnFilter" runat="server" Text="Buscar" EnableViewState="False" 
                                CssClass="button"  CausesValidation="true" Height="24px"/>                            
                        </td>
                    </tr>
                    
                </table>
                </asp:Panel> 
                <script type="text/javascript">

                    var fechas = $('#<%=Me.chkFechas.ClientId %>');
                    fechas.click(
                        function() {
                            $('#pnlDatesSelection').css('display', ((this.checked) ? '' : 'none'));
                            $('#pnlDates').css('display', ((this.checked) ? '' : 'none')); 
                        }
                    );
                    $('#pnlDatesSelection').css('display', ((fechas.attr('checked')) ? '' : 'none'));
                    $('#pnlDates').css('display', ((fechas.attr('checked')) ? '' : 'none'));    
                                              
                </script>
            </td>
        </tr>
        <%--<tr height="5">
            <td style="padding-left:20px;">
                <asp:LinkButton ID="btnExcelExport" runat="server" OnClick="btnExcelExport_Click">Exportar a excel</asp:LinkButton>
            </td>
        </tr>--%>
        <tr>
            <td align=center >
            <asp:Label ID="lblFiltro" runat="server" Text="" CssClass="clsHelpLabel" ></asp:Label>
            </td>
        </tr>
        <tr class="screen">
            <td align="center" valign =top>
                <asp:DataGrid ID="lstClientes" runat="server" CssClass="DataGrid"  GridLines="None" 
                    ShowFooter="True" AutoGenerateColumns="False" AllowPaging="True" Width="95%" 
                    PageSize="20" OnItemDataBound="lstClientes_ItemDataBound" OnItemCreated="lstClientes_ItemCreated">                    
                    <SelectedItemStyle CssClass="dgSelected" Font-Bold="False" Font-Italic="False" 
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                        HorizontalAlign="Left"></SelectedItemStyle>
                    <AlternatingItemStyle CssClass="dgAlternate" Font-Bold="False" 
                        Font-Italic="False" Font-Overline="False" Font-Strikeout="False" 
                        Font-Underline="False" HorizontalAlign="Left"></AlternatingItemStyle>
                    <ItemStyle CssClass="dgItem" Font-Bold="False" Font-Italic="False" 
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                        HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle CssClass="dgHeader" Font-Bold="False" Font-Italic="False" 
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                        HorizontalAlign="Center"></HeaderStyle>
                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="id" HeaderText="ID"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Referencia">
                            <ItemTemplate>
                                <%#Eval("id").ToString().PadLeft(8, "0")%>
                            </ItemTemplate>
                            <HeaderStyle Width="40px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="hotelName" HeaderText="Hotel"></asp:BoundColumn>                   
                        <asp:BoundColumn DataFormatString="{0:dd/MMM/yyyy}" DataField="date" HeaderText="Fecha de registro">                            
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>                                   
                        <asp:TemplateColumn HeaderText="Nombre">
                            <ItemTemplate>
                                <%#Eval("firstName") + " " + Eval("lastName")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <%--<asp:BoundColumn DataField="phone" HeaderText="Phone"></asp:BoundColumn>--%>
                        <asp:BoundColumn DataField="email" HeaderText="Email">
                            <HeaderStyle Width="100px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataFormatString="{0:dd/MMM/yyyy}" DataField="checkIn" HeaderText="Fecha Llegada">                            
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>                                 
                        <asp:TemplateColumn HeaderText="Noches">
                            <ItemTemplate>
                                <%#Convert.ToDateTime(Eval("checkOut")).Subtract(Convert.ToDateTime(Eval("checkIn"))).TotalDays.ToString()%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="">
                            <ItemTemplate>
                                <a href="javascript:" onclick='<%# "ShowInfo(""" + Eval("id").toString()+""", "+Eval("plusTax").ToString()+", "+Eval("tax").ToString()+");" %>'><%=Me.GetLabel("01334")%></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                        Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr class="screen" height="5">
            <td>
            </td>
        </tr>
        <tr>
            <td align="center">            
            
                <script type="text/javascript">
                    $(document).ready(
                        function() {
                            $('#pnlStatus input:checkbox').click(
                                function() {
                                    var flag = false;
                                    $('#pnlStatus input:checkbox').each(
                                        function() {
                                            if (!flag) { flag = this.checked; }
                                        }
                                    );
                                    if (!flag) { $('#<%= me.chkStatusInProcess.clientId %>').attr('checked', 'checked'); }
                                }
                            );
                            if($('#<%= me.chkAllHotels.clientId %>:checked').length > 0) 
                            {
                                $('#<%= me.txtHotelName.clientId %>').attr('disabled','true');                                
                            }
                            
                            $('#<%= me.chkAllHotels.clientId %>').click(
                                function(){                                                                       
                                    if(this.checked)
                                    {
                                        $('#<%= me.txtHotelName.clientId %>').attr('disabled','true');
                                    }    
                                    else
                                        $('#<%= me.txtHotelName.clientId %>').removeAttr('disabled');
                                }
                            );
                            
                        }
                    );

                      $().ready(function() {
                             // alert(document.getElementById('hplShow').style.display);
                             var officeSelected = $('#<%= Me.hdnSearch.ClientId %>').val();
                             if (officeSelected == 1) {
                                 $('#<%= Me.hplShow.ClientId %>').hide();
                                 $('#<%= Me.hplHide.ClientId %>').show();
                                 $('#<%= Me.pnlSearch.ClientId %>').show();
                             }
                             
                         });  

                    function ShowInfo(id, taxIncluded, tax) {
                        if (id != null && id > 0) {
                            if (!$('#boxInfo').is(":hidden")) 
                                $('#boxInfo').slideUp("slow");
                            $('#pnlLoading').show();
                            
                            var baseUrl = "<%= Request.ApplicationPath %>";
                            
                            var d = new Date();
                            var temporal = d.getDate().toString() + d.getHours().toString() + d.getMinutes() + d.getSeconds();
                            if (!(baseUrl.match('/$') == '/')) { baseUrl += '/'; }
                            $.ajax({
                                url: baseUrl + 'servicios/waitlist.ashx',
                                dataType: 'json',                               
                                data: { item: id.toString(), lang: <%= "'" + PortalCulture.GetIDCulture().ToString() + "'" %>, noCache: temporal },
                                success: function(data) {
                                    if (data != null) {
                                        try {
                                            $('#<%= Me.varCurrentCustomer.ClientId %>').val(data.id);
                                            $('#lblName').html(data.name);
                                            $('#lblPhone').html(data.phone);
                                            $('#lblEmail').html(data.email);
                                            $('#lblCellPhone').html(data.cellPhone);
                                            $('#lblComment').html(data.comment);
                                            $('#lblCheckIn').html(data.checkIn);
                                            $('#lblCheckOut').html(data.checkOut);
                                            $('#lblAdults').html(data.adults);
                                            $('#lblRooms').html(data.rooms);
                                            $('#lblChildren').html(data.children);
                                            $('#lblRateCode').html(data.rateCode);
                                            
                                            $('#lblRegistrationDate').html(data.date);
                                            $('#lblRoomName').html(data.roomName);
                                            $('#lblRoomDescription').html(data.roomDescription);
                                            $('#lblRateName').html(data.rateName);
                                            $('#lblRateDescription').html(data.rateDescription);
                                                                                                                                    
                                            $('#lblRateDescription').html(data.rateDescription);
                                            $('#pnlActor').hide();
                                            $('#<%= me.btnRemove.ClientId %>').hide();                                                                                                                                   
                                            $('#btnReject').show();                                                                                                                                                                             
                                            $('#pnlCancellationReason').hide();
                                                                                                                                   
                                            var status = '';                                      
                                            switch(data.status)
                                            {
                                                case 1: case "1":
                                                    status = '<%= Me.GetLabel("00439")%>';                                                    
                                                    break;
                                                case 2: case "2":
                                                    status = '<%= Me.GetLabel("01268")%>';
                                                    $('#pnlActor').show();
                                                    $('#lblActorTitle').html('<%=Me.GetLabel("01336")%>');
                                                    $('#lblActor').html(data.actor);
                                                    break;
                                                case 4: case "4":
                                                    status = '<%= Me.GetLabel("M0BT0000061")%>';
                                                    $('#pnlActor').show();
                                                    $('#lblActorTitle').html('<%=Me.GetLabel("01335")%>');
                                                    $('#lblActor').html(data.actor);   
                                                    if(data.notifyComment.length > 0)
                                                    {                                                                                                                                                                                                                  
                                                        $('#pnlCancellationReason').show();
                                                        $('#lblConcellationReason').html(data.notifyComment);
                                                    }
                                                    break;
                                                case 8: case "8":
                                                    status = '<%= Me.GetLabel("01267")%>';
                                                    break;
                                                default:
                                                    status = 'Unknow';
                                            }
                                            $('#lblStatus').html(status);                                            
                                            
                                            //Calculando los impuestos
                                            
                                            if(taxIncluded != 1)
                                            {
                                                $('#lblSubTotal').html((parseFloat(data.notifyPrice) / ( 1 + (tax/100))).toFixed(2) + ' ' + data.notifyCurrency);                                            
                                                $('#lblTaxes').html((parseFloat(data.notifyPrice)-(parseFloat(data.notifyPrice)/(1+(tax/100)))).toFixed(2)+ ' ' + data.notifyCurrency);
                                                $('#valPrice').val((parseFloat(data.notifyPrice) / ( 1 + (tax/100))).toFixed(2));
                                                $('.taxesStatus').html('<%= Me.GetLabel("00610") %>');
                                            }
                                            else
                                            {
                                                $('#lblSubTotal').html(parseFloat(data.notifyPrice).toFixed(2) + ' ' + data.notifyCurrency);                                                       
                                                $('#lblTaxes').html('<%= Me.GetLabel("00433") %>');
                                                $('#valPrice').val(parseFloat(data.notifyPrice).toFixed(2));
                                                $('.taxesStatus').html('<%= Me.GetLabel("00611") %>');
                                            }
                                            var decimals = 0;
                                            if ((parseFloat(tax)%1) != 0)
                                            {
                                                decimals = 1;
                                                if ((parseFloat(tax)%.1) != 0)   
                                                    decimals = 2;                                      
                                            }
                                            
                                            $('#lblTaxPercent').html('('+parseFloat(tax).toFixed(decimals)+'%)');
                                            
                                            $('#lblTotal').html(parseFloat(data.notifyPrice).toFixed(2) + ' ' + data.notifyCurrency);
                                            
                                            //$('#lblSubTotal').html((parseFloat(data.notifyPrice) * ( 1 - (tax/100))).toFixed(2) + ' ' + data.notifyCurrency);
                                            //var taxes = (parseFloat(data.notifyPrice) * (tax/100));
                                            //var taxesLegend = "";
                                            //if(taxIncluded <> 1)
                                            //    taxesLegend = taxes.toFixed(2) + ' ' + data.notifyCurrency;
                                            //else
                                            //    taxesLegend = '<%= Me.GetLabel("00433") %>';
                                                                                        
                                            //$('#lblTaxes').html(taxesLegend);
                                            //$('#lblTotal').html(parseFloat(data.notifyPrice).toFixed(2) + ' ' + data.notifyCurrency);
                                                                            
                                            $('#valComment').val(data.notifyComment);
                                            $('#valCurrency').val(data.notifyCurrency);
                                            
                                            $('#pnlActivedButtons').hide(); 
                                            $('#pnlExpiredButtons').hide();
                                                                                        
                                            switch(data.status)
                                            {
                                                case 8: case "8":  
                                                    if(data.isOlder=="false")
                                                        $('#pnlExpiredButtons').show();                       
                                                    $('#btnReject').hide();
                                                    $('#<%= me.btnRemove.ClientId %>').show();
                                                    break;
                                                case 2: case"2": case 1: case "1":            
                                                    $('#btnShowNotification').hide();
                                                    $('#btnShowBooking').hide();
                                                    if (data.status == 2) {
                                                        $('#btnShowBooking').show();
                                                    } else {
                                                        $('#btnShowNotification').show();
                                                    }                                    
                                                    $('#pnlActivedButtons').show();
                                                    break;
                                                case 4: case "4":     
                                                    $('#<%= me.btnRemove.ClientId %>').show();                                                 
                                                    $('#btnReject').hide();
                                                    
                                            }
                                            
                                            $('#boxInfo').slideDown("slow");
                                            $('#pnlLoading').show();
                                        }
                                        catch (ex) { alert('<%= me.getlabel("M0BT0000012") %>'); }
                                    }  
                                },
                                error: function(XMLHttpRequest, textStatus, errorThrown){
                                    alert('<%= me.getlabel("M0BT0000012") %>');
                                },
                                complete: function(result) {
                                    $('#pnlLoading').hide();
                                }
                            });
                        }
                        onResizeIframe(200);
                     }

                    function HideInfo() {
                        $('#boxInfo').slideUp("slow");
                        $('#boxInfo').hide();
                        onResizeIframe();
                    }

                    function ShowNotifyWindow() {
                        $('#boxBackground').show();
                        $('#boxMessage').show();
                        $('#<%= Me.txtNotificationPrice.ClientId %>').val('');
                        $('#<%= Me.txtNotificationComment.ClientId %>').val('');
                        $('#<%= Me.txtNotificationPrice.ClientId %>').val($('#valPrice').val());
                        $('#<%= Me.txtNotificationComment.ClientId %>').val($('#valComment').val());

                        if ($('#valPrice').val() > 0.0) {
                            $('select#<%= Me.lstNotificationCurrencies.ClientId %> option').each(
                                function() { 
                                    this.selected = false; 
                                }
                            );
                            $("select#<%= Me.lstNotificationCurrencies.ClientId %> option[value='" + $('#valCurrency').val() + "']").attr('selected',true);
                        }
                        onResizeIframe();
                    }
                    
                    function CloseRejectWindow() {
                        $('#boxBackground').hide();
                        $('#boxReject').hide();
                        onResizeIframe();                        
                    }

                    function CloseNotifyWindow() {
                        $('#boxBackground').hide();
                        $('#boxMessage').hide();
                        onResizeIframe();
                    }

                    function CloseBookingWindow() {
                        $('#boxBackground').hide();
                        $('#boxBooking').hide();
                        onResizeIframe();
                    }

                    function ShowBookingWindow() {
                        $('#boxBackground').show();
                        $('#boxBooking').show();
                        $('#<%= Me.txtBookingPrice.ClientId %>').val('');
                        $('#<%= Me.txtBookingPrice.ClientId %>').val($('#valPrice').val());

                        if ($('#valPrice').val() > 0.0) {
                            $('select#<%= Me.lstBookingCurrencies.ClientId %> option').each(
                                function() {
                                    this.selected = false;
                                }
                            );
                            $("select#<%= Me.lstBookingCurrencies.ClientId %> option[value='" + $('#valCurrency').val() + "']").attr('selected', true);
                        }
                        onResizeIframe();
                    }
                    
                    function ShowRejectWindow() {
                        $('#boxBackground').show();
                        $('#boxReject').show();
                        $('#<%= Me.txtRejectComment.ClientId %>').val('');    
                        onResizeIframe();                   
                    }

                    function ValidateCurrency(tb, e) {
                    
                        var key = (document.all) ? e.keyCode : e.which;
                        if (key == 0 || key == 8 || key == 13) return true;

                        var chr = String.fromCharCode(key);

                        var cursorStart = PosicionCursor(tb, 'start');
                        var cursorEnd = PosicionCursor(tb, 'end');
                        var patron = /^[0-9]{0,6}(\.[0-9]{0,2})?$/;
                        var temp = tb.value.substring(0, cursorStart) + chr + tb.value.substring(cursorEnd);
                        //alert('start:' + cursorStart + '; end:' + cursorEnd + '; startSegment:'+tb.value.substring(0,cursorStart)+'; endSegment:' + tb.value.substring(cursorEnd) + '; cadena a evaluar:\''+temp+'\'.');        

                        return patron.test(temp);
                        
                    }

                    function PosicionCursor(tb, type) {
                    
                        var cursor = -1;
                        var inc = (type == 'end') ? 1 : -1;

                        if (document.selection && (document.selection != 'undefined')) // IE
                        {
                            var _range = document.selection.createRange();
                            var contador = 0;
                            if (inc == 1) {
                                contador = PosicionCursor(tb, 'start');
                                contador += _range.text.length;
                            }
                            else {
                                while (_range.move('character', -1))
                                    contador++;
                            }
                            cursor = contador;
                        }
                        else if (tb.selectionStart >= 0)// FF
                        {
                            if (inc == 1)
                                cursor = tb.selectionEnd;
                            else
                                cursor = tb.selectionStart;
                        }

                        return cursor;
                        
                    } 
                                        
                    function FireShowOrHidden(ID, IDlnk, IHDlnk, hdn, show) {
                        var e = document.getElementById(ID);
                        var l = document.getElementById(IDlnk);
                        var h = document.getElementById(IHDlnk);
                        var hd = document.getElementById(hdn);
                        if (e) {
                            e.style.display = show ? '' : 'none';        
                        }
                        if (l) {
                            l.style.display = show ? '' : 'none';
                        }
                        if (h) {
                            h.style.display = !show ? '' : 'none';
                        }
                        if (hd) {
                            hd.value = show ? 1 : 0; 
                        }
                         onResizeIframe();
                    }
                </script>
                
                <div id="pnlLoading" style="display:none;">
                    <img src="../../Images/HugeRotation.gif" />
                </div>
                
                <table cellspacing="5" cellpadding="2" border="0" id="boxInfo">
                    <tr class="dgHeader">
                        <td align="center" colspan="4" class="dgitem">
                            <%=Me.GetLabel("01236")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label"><%=Me.GetLabel("M0BT0000235")%>&nbsp;:&nbsp;</td>
                        <td><b><span id="lblRegistrationDate">00/XXX/00</span></b></td>
                        <td class="Label"><%=Me.GetLabel("00283")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblStatus">(612)141-77-71</span></td>                        
                    </tr>
                    <tr>
                        <td class="Label"><%=Me.GetLabel("00073")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblName">Giacomo Guilizzoni</span></td>
                        <td class="Label"><%=Me.GetLabel("00164")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblPhone">(612)141-77-71</span></td>
                    </tr>
                    <tr>
                        <td class="Label"><%=Me.GetLabel("00163")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblEmail">xxx</span></td>
                        <td class="Label"><%=Me.GetLabel("01237")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblCellPhone">(612)141-77-71</span></td>
                    </tr>
                    <tr>
                        <td valign="top" class="Label"><%=Me.GetLabel("01238")%>&nbsp;:&nbsp;</td>
                        <td colspan="3"><span id="lblComment">Giacomo Guilizzoni</span></td>
                    </tr>
                    <tr id="pnlActor">
                        <td class="Label"><span id="lblActorTitle">Confirmado Por</span>&nbsp;:&nbsp;</td>
                        <td colspan="3"><span id="lblActor">Giacomo Guilizzoni</span></td>
                    </tr>
                    <tr id="pnlCancellationReason">
                        <td class="Label"><%=Me.GetLabel("01345")%>&nbsp;:&nbsp;</td>
                        <td colspan="3"><span id="lblConcellationReason">Giacomo Guilizzoni</span></td>
                    </tr>
                    <tr class="dgHeader">
                        <td align="center" colspan="4" class="dgitem"><%=Me.GetLabel("00618")%></td>
                    </tr>
                    <tr>
                        <td class="Label"><%=Me.GetLabel("00368")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblCheckIn">Giacomo Guilizzoni</span></td>
                        <td class="Label"><%=Me.GetLabel("00369")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblCheckOut">(612)141-77-71</span></td>
                    </tr>
                    <tr>
                        <td class="Label"><%=Me.GetLabel("00060")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblAdults">Giacomo Guilizzoni</span></td>
                        <td class="Label"><%=Me.GetLabel("00061")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblChildren">(612)141-77-71</span></td>
                    </tr>
                    <tr>
                        <td class="Label"><%=Me.GetLabel("00074")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblRooms">Giacomo Guilizzoni</span></td>
                        <td class="Label"><%=Me.GetLabel("00122")%>&nbsp;:&nbsp;</td>
                        <td><span id="lblRateCode">Giacomo Guilizzoni</span></td>
                    </tr>
                    <tr>
                        <td valign="top" class="Label"><%=Me.GetLabel("00072")%>&nbsp;:&nbsp;</td>
                        <td colspan="3"><span id="lblRoomName">Giacomo Guilizzoni</span></td>
                    </tr>
                    <tr>
                        <td valign="top" class="Label"><i><%=Me.GetLabel("00002")%>&nbsp;:&nbsp;</i></td>
                        <td colspan="3"><span id="lblRoomDescription">Giacomo Guilizzoni</span></td>
                    </tr>
                    <tr>
                        <td valign="top" class="Label"><%=Me.GetLabel("00016")%>&nbsp;:&nbsp;</td>
                        <td colspan="3"><span id="lblRateName">Giacomo Guilizzoni</span></td>
                    </tr>
                    <tr>
                        <td valign="top" class="Label"><i><%=Me.GetLabel("00002")%>&nbsp;:&nbsp;</i></td>
                        <td colspan="3"><span id="lblRateDescription">Giacomo Guilizzoni</span></td>
                    </tr>
                    <tr>
                        <td colspan="3" class="Label"><%=Me.GetLabel("M0BT0000038")%>&nbsp;:&nbsp;</td>
                        <td align="right"><span id="lblSubTotal">Giacomo Guilizzoni</span></td>
                    </tr>
                    <tr>
                        <td colspan="3" class="Label"><%=Me.GetLabel("M0BT0000039")%>&nbsp;<span id="lblTaxPercent"></span>&nbsp;:&nbsp;</td>
                        <td align="right"><span id="lblTaxes">Giacomo Guilizzoni</span></td>
                    </tr>
                    <tr>
                        <td colspan="3" class="Label"><%=Me.GetLabel("00136").ToUpper()%>&nbsp;:&nbsp;</td>
                        <td align="right"><b><span id="lblTotal">Giacomo Guilizzoni</span></b></td>
                    </tr>
                    <tr class="screen">
                        <td style="text-align: left;">
                            <input type="button" class="button" id="btnPrint" onclick="window.print();return false;" value='<%= me.getlabel("01326") %>'/>
                        </td>
                        <td style="text-align: right;" colspan="3">
                            <%
                                Me.btnBook.Text = Me.GetLabel("00347")
                                
                                'Me.btnReject.Text = Me.GetLabel("01317")
                                'Me.btnReject.OnClientClick = "return confirm('" + Me.GetLabel("01330") + "');"
                                
                                Me.btnRemove.Text = Me.GetLabel("M000485")
                                Me.btnRemove.OnClientClick = "return confirm('" + Me.GetLabel("01239") + "');"
                                
                                Me.btnActivate.Text = Me.GetLabel("01328")
                                Me.btnActivate.OnClientClick = "return confirm('" + Me.GetLabel("01329") + "');"
                            %>                        
                            <input type="hidden" id="varCurrentCustomer" value="" runat="server" />
                            
                            <span id="pnlActivedButtons">
                                <input id="valComment" value="" type="hidden" />
                                <input id="valPrice" value="" type="hidden" />
                                <input id="valCurrency" value="" type="hidden" />
                                <input onclick="ShowNotifyWindow();" type="button" class="button" id="btnShowNotification" value='<%= Me.GetLabel("01327") %>'/>               
                                <input onclick="ShowBookingWindow();" type="button" class="button" id="btnShowBooking" value='<%= Me.GetLabel("00347") %>'/> 
                            </span>
                            <span id="pnlExpiredButtons">
                                <asp:Button ID="btnActivate" CssClass="button" runat="server" Text="" OnClick="btnActivate_Click" /> 
                            </span>
                            <asp:Button ID="btnRemove" CssClass="button" runat="server" Text="Eliminar" /> 
                            <input onclick="ShowRejectWindow();" type="button" class="button" id="btnReject" value='<%= Me.GetLabel("01317") %>'/> 
                            <input type="button" class="button" value='<%= Me.GetLabel("M000436") %>' onclick="HideInfo();"/>
                            
                        </td>
                    </tr>    
                </table>   
            </td>
        </tr>
        <tr height="5">
            <td>
            </td>
        </tr>
    </table>
    
    <div style="display:none; background-color:gray; opacity:0.4; filter:alpha(opacity=30); position:absolute; width:100%; height:200%; top:0; left:0; z-index:500;" id="boxBackground"></div>
    
    <div id="boxMessage" class=boxMsgCan  style="border: 1px solid #286FC0; position: absolute; width: 500px; height: 294px; top: 50%; left: 50%; margin-left: -250px; margin-top: -105px; z-index: 1000; background-color: white; display: none;">
            
        <table cellspacing="2" width="100%"  class ="Bookingcontainer">
		    <tr class="">
			    <td colspan="2" class ="Titulo">
			       <%=Me.GetLabel("01269")%> 
			    </td>
		    </tr>
		    <tr>
			    <td align="right">
			        <%=Me.GetLabel("01270")%>&nbsp;(<b><span class="taxesStatus"></span></b>)&nbsp;:&nbsp;
			    </td>
			    <td>
                    <asp:TextBox ID="txtNotificationPrice" runat="server" Width="61px" onkeypress="return ValidateCurrency(this,event);"
        MaxLength="9"></asp:TextBox>
                    <asp:DropDownList ID="lstNotificationCurrencies" runat="server"></asp:DropDownList>
			    </td>
		    </tr>
		    <tr>
			    <td align="right" valign="top" style="">
			        <%=Me.GetLabel("01238")%>&nbsp;:&nbsp;
			    </td>
			    <td>
		            <asp:TextBox ID="txtNotificationComment" TextMode="MultiLine" Rows="5" runat="server" width="350px"></asp:TextBox>
			    </td>
		    </tr>
		    <tr>
			    <td align="right" colspan="2">
			        <%
			            Me.btnSendNotify.Text = Me.GetLabel("01240")
			        %>
                    <asp:Button ID="btnSendNotify" CssClass="button" runat="server" Text="Enviar" />
                    <input type="button" class="button" id="btnNotificationCancel" value='<%= me.GetLabel("M000445") %>' onclick="CloseNotifyWindow();"/>        
			    </td>
		    </tr>
	    </table>
                     
    </div>
    
    
    <div id="boxReject" class=boxMsgCan style="border: 1px solid #286FC0; position: absolute; width: 500px; height: 260px; top: 50%; left: 50%; margin-left: -250px; margin-top: -82.5px; z-index: 1000; background-color: white; display: none;">
            
        <table cellspacing="2" width="100%" class ="Bookingcontainer">
		    <tr class="">
			    <td colspan="2" class=Titulo>
			        <%=Me.GetLabel("01269")%> 
			    </td>
		    </tr>
		    <tr>
			    <td align="right" valign="top" style="">
			        <%=Me.GetLabel("01345")%>&nbsp;:&nbsp;
			    </td>
			    <td>
		            <asp:TextBox ID="txtRejectComment" TextMode="MultiLine" Rows="5" runat="server" width="350px"></asp:TextBox>
			    </td>
		    </tr>
		    <tr>
			    <td align="right" colspan="2">
			        <%
			            Me.btnRejectSend.Text = Me.GetLabel("01317")
			        %>
                    <asp:Button ID="btnRejectSend" CssClass="button" runat="server" Text="Enviar" OnClick="btnReject_Click"/>
                    <input type="button" class="button" id="btnRejectCancel" value='<%= me.GetLabel("M000445") %>' onclick="CloseRejectWindow();"/>        
			    </td>
		    </tr>
	    </table>
                     
    </div>
    
    <div id="boxBooking" class=boxMsgCan style="border: 1px solid #286FC0; position: absolute; width: 500px; height: 160px; top: 50%; left: 50%; margin-left: -250px; margin-top: -50px; z-index: 1000; background-color: white; display: none;">
            
        <table cellspacing="2" width="100%" class ="Bookingcontainer">
		    <tr class="dgHeader">
			    <td class=titulo>
			        <%=Me.GetLabel("01271")%>
			    </td>
		    </tr>
		    <tr>
			    <td align="left">
			        <%=Me.GetLabel("01272")%>
			    </td>
		    </tr>
		    <tr>
			    <td align="center" class="row">
			        <%=Me.GetLabel("01270")%>&nbsp;(<b><span class="taxesStatus"></span></b>)&nbsp;:&nbsp;
		            <asp:TextBox ID="txtBookingPrice" runat="server" Width="61px" onkeypress="return ValidateCurrency(this,event);"
        MaxLength="9"></asp:TextBox>
                    <asp:DropDownList ID="lstBookingCurrencies" runat="server"></asp:DropDownList>
			    </td>
		    </tr>
		    <tr>
			    <td align="right">    
			        <%Me.btnBook.Text = Me.GetLabel("00347")%>                            
			        <asp:Button ID="btnBook" CssClass="button" runat="server" Text="Reservar" />
                    <input type="button" class="button" id="btnBookingCancel" value='<%= me.GetLabel("M000445") %>' onclick="CloseBookingWindow();"/>        
			    </td>
		    </tr>
	    </table>
                     
    </div>
    </form>
    <% System.Threading.Thread.CurrentThread.CurrentCulture = sysCulture%>
</body>
</html>
