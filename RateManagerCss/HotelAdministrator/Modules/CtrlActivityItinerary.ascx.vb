Imports PortalLibraries
Imports System.Data
Imports System.Data.SqlClient
Partial Public Class CtrlActivityItinerary
    Inherits System.Web.UI.UserControl
    Private Activities_ITR2_RS As Activities_ITR2_RS
    Private resp() As Activities_DSR2_RS
    Private ds As New System.Collections.Specialized.StringDictionary
    Private EventsActivity As DataTable
    Private PropertyID As String
    Public PenaltyAmount As Decimal = -1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'FillReservation("0908010926298046")
    End Sub
    Private Sub Idiomas()


        btnCancel.Value = PortalCulture.GetString("01621")
        Me.hplCancelRes.Text = PortalCulture.GetString("01622")
        Me.lnkNo.Text = PortalCulture.GetString("01623")
        lblConfirmTitle.Text = PortalCulture.GetString("01624")
        lblConfirmPrompt.Text = PortalCulture.GetString("01625")
        hplCancelRes.Text = PortalCulture.GetString("01626")
        lnkNo.Text = PortalCulture.GetString("01627")
        lnkBtnRulesRestrctions.InnerText = " " + PortalCulture.GetString("01628") + " "
        lblRR1.Text = PortalCulture.GetString("01629")

        Label4.Text = PortalCulture.GetString("01630")
        lblEFees.Text = PortalCulture.GetString("01631")
        lblETotalReservation.Text = "Total "
        lblRR2.Text = PortalCulture.GetString("01632")
        lblActivity.InnerText = PortalCulture.GetString("01633")

        lblmensajeReservated.Text = PortalCulture.GetString("01634")
    End Sub
    Public Sub FillReservation(ByVal idPackage As String) 'As Activities_DSR2_RS
        Try

            Idiomas()

            ' Dim rq As New Activities_ITR2_RQ
            'rq.ActivityHeader.AddActivityHeaderRow(idPackage)
            Dim servicio As New ActivitiesServiceV2(0)
            Activities_ITR2_RS = GetItr(idPackage) 'servicio.ActivitiesItr(rq)
            Dim cas As String()
            'Dim resp As Activities_DSR2_RS()
            ReDim resp(Activities_ITR2_RS.Reservation.Count)

            'sp_activities_itr() '0908010926298046'

            For indice As Integer = 0 To Activities_ITR2_RS.Reservation.Count - 1
                resp(indice) = New Activities_DSR2_RS
                'Dim rqd As New Activities_DSR2_RQ
                'rqd.ActivityHeader.AddActivityHeaderRow(Activities_ITR2_RS.Reservation(indice).ReservationID, PortalCulture.GetIDCulture)

                resp(indice) = GetDsr(Activities_ITR2_RS.Reservation(indice).ReservationID, PortalCulture.GetIDCulture) 'servicio.ActivitiesDsr(rqd)
                '   sp_activities_dsr() '09080109265318046',1
            Next
            Dim total As Decimal = 0
            Dim tax As Decimal = 0
            Dim subtotal As Decimal = 0
            Dim currency As String = resp(0)._Property(0).CurrencyCode
            For indice As Integer = 0 To Activities_ITR2_RS.Reservation.Count - 1
                total += CDec(resp(indice).Reservation(0).Total)
                tax += CDec(resp(indice).Reservation(0).Tax)
            Next
            subtotal = total - tax

            Dim tc As Decimal = 1
            lblMsgTotalMoneda.Visible = False
            If currency <> New PortalPartnersCfg().CurrencyCode Then
                lblMsgTotalMoneda.Visible = True
                tc = New MoneyExchangeService().GetTypeMoneyExchange(currency, New PortalPartnersCfg().CurrencyCode)
                lblMsgTotalMoneda.Text = PortalCulture.GetString("01635") & PortalCulture.GetString("01636")
                lblMsgTotalMoneda.Text = String.Format(lblMsgTotalMoneda.Text, (FormatCurrency(total) & " " & currency))
            End If


            lblTotal.Text = FormatCurrency(tc * subtotal)
            lblFees.Visible = True
            lblFees.Text = FormatCurrency(tc * tax)
            lblTotalReservation.Text = FormatCurrency(tc * total)



            LoadTableEvents()
            cargarEventos()
            CargaGridTraveler()

            'Try

            '    Dim sPath As String
            '    sPath = New PackageBasePage().NonSecureSite + "/Packages/PackageRulesActivity.aspx?stm=1&PropertyID=" + PropertyID
            '    lnkBtnRulesRestrctions.Attributes.Add("OnClick", "javascript:window.open('" & sPath & "','wSumarryCarRules','toolbars=no, directories=no, location=no, width= 650, height=600, scrollbars=yes, status=no, resizable=yes');return false;")
            '    lnkBtnRulesRestrctions.HRef = sPath
            '    lnkBtnRulesRestrctions.Target = "_blank"


            'Catch ex As Exception

            'End Try

            'If Not Request.QueryString("Page") Is Nothing Then
            '    Me.PanelCancel.Visible = False
            'End If
        Catch ex As Exception
            PortalServiceTracer.ServiceTracer("(FillReservation-Activity) Error: " + ex.ToString, PortalServiceTacerErrorTypes.Debug)
            Me.Visible = False
            'Return Nothing
        End Try
    End Sub
    Private Function GetDsr(ByVal reservationID As String, ByVal language As Integer) As DataSet
        Dim ds As New Activities_DSR2_RS
        With New SqlDataAdapter("sp_activities_dsr", New SqlConnection(ConfigurationManager.AppSettings("ActivityConnection")))
            .SelectCommand.CommandType = CommandType.StoredProcedure
            .SelectCommand.Parameters.Add("@reservationID", SqlDbType.NVarChar, 75).Value = reservationID
            .SelectCommand.Parameters.Add("@language", SqlDbType.Int).Value = language
            .TableMappings.Add("Table", ds.ActivityHeader.TableName)
            .TableMappings.Add("Table1", ds.Agency.TableName)
            .TableMappings.Add("Table2", ds._Property.TableName)
            .TableMappings.Add("Table3", ds.CreditCard.TableName)
            .TableMappings.Add("Table4", ds.Customer.TableName)
            .TableMappings.Add("Table5", ds.Reservation.TableName)
            .TableMappings.Add("Table6", ds._Event.TableName)
            .TableMappings.Add("Table7", ds.Price.TableName)
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
    Private Function GetItr(ByVal itinerary As String) As DataSet
        Dim ds As New Activities_ITR2_RS
        With New SqlDataAdapter("sp_activities_itr", New SqlConnection(ConfigurationManager.AppSettings("ActivityConnection")))
            .SelectCommand.CommandType = CommandType.StoredProcedure
            .SelectCommand.Parameters.Add("@Itinerary", SqlDbType.NVarChar, 24).Value = itinerary
            Try
                .SelectCommand.Connection.Open()
                .Fill(ds, ds.Reservation.TableName)
            Catch ex As Exception
            Finally
                .SelectCommand.Connection.Close()
            End Try
        End With
        Return ds
    End Function
    Private Sub cargarEventos()
        Dim dr As DataRow
        For indice As Integer = 0 To Activities_ITR2_RS.Reservation.Count - 1
            dr = EventsActivity.NewRow
            dr(0) = indice
            EventsActivity.Rows.Add(dr)

        Next
    End Sub
    Private Sub LoadTableEvents()
        EventsActivity = New DataTable
        EventsActivity.Columns.Add("Evento")

    End Sub

    Private Sub CargaGridTraveler()
        ds = New System.Collections.Specialized.StringDictionary
        lstTravelersFliht.DataSource = EventsActivity
        lstTravelersFliht.DataBind()

    End Sub

    Private Sub lstTravelersFliht_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles lstTravelersFliht.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            lblmensajeReservated.Text = "* " + PortalCulture.GetString("01634")
            Dim PropertyName As Label
            Dim lblRevNum As Label
            Dim lblRvaNum As Label
            Dim lblStatus As Label
            Dim Table1 As HtmlControls.HtmlTable

            Dim TblCustomer As HtmlControls.HtmlTable
            Dim LblMainContact As Label
            Dim lMainContact As Label
            Dim LblMainContactMail As Label
            Dim lMainContactMail As Label
            Dim LblMainContactPhone As Label
            Dim lMainContactPhone As Label

            Dim contenedor As HtmlGenericControl


            PropertyName = e.Item.FindControl("PropertyName")
            Table1 = e.Item.FindControl("Table1")

            lblRevNum = e.Item.FindControl("lblRevNum")
            lblRvaNum = e.Item.FindControl("lblRvaNum")
            lblStatus = e.Item.FindControl("lblStatus")

            TblCustomer = e.Item.FindControl("TblCustomer")

            lMainContact = e.Item.FindControl("MainContact")
            lMainContactPhone = e.Item.FindControl("MainContactPhone")
            lMainContactMail = e.Item.FindControl("MainContactMail")

            LblMainContactPhone = e.Item.FindControl("LblMainContactPhone")
            LblMainContactMail = e.Item.FindControl("LblMainContactMail")
            LblMainContact = e.Item.FindControl("LblMainContact")


            Try
                TblCustomer.Visible = False
                lblRevNum.Visible = False
                lblRvaNum.Visible = False
                'lnkActivityDetails.Visible = False
                Dim rvanumber As String = CStr(Activities_ITR2_RS.Reservation(e.Item.ItemIndex).ReservationNumber).ToString()
                PropertyName.Visible = False
                Table1.Visible = False
                If ds.ContainsKey(resp(e.Item.ItemIndex)._Property(0).PropertyID) = False Then
                    ds.Add(resp(e.Item.ItemIndex)._Property(0).PropertyID, "")
                    PropertyID += resp(e.Item.ItemIndex)._Property(0).PropertyID.ToString() + ","
                    PropertyName.Visible = True
                    lblRevNum.Visible = True
                    lblRvaNum.Visible = True
                    Table1.Visible = True
                    TblCustomer.Visible = True
                End If

                contenedor = e.Item.FindControl("contenedor")
                PropertyName.Text = resp(e.Item.ItemIndex)._Property(0).PropertyName
                lblRevNum.Text = PortalCulture.GetString("01637", True)
                lblRvaNum.Text = " " & rvanumber


                lMainContact.Text = resp(e.Item.ItemIndex).Customer(0).Name + " " + resp(e.Item.ItemIndex).Customer(0).LastName

                lMainContactMail.Text = resp(e.Item.ItemIndex).Customer(0).Email

                lMainContactPhone.Text = resp(e.Item.ItemIndex).Customer(0).Phone

                LblMainContact.Text = PortalCulture.GetString("01638", True)

                LblMainContactMail.Text = PortalCulture.GetString("01639", True)

                LblMainContactPhone.Text = PortalCulture.GetString("01640", True)
                lblEComision.Text = PortalCulture.GetString("M0BT0000322").Substring(0, 8)
                lblStatus.Visible = False
            Catch ex As Exception

            End Try
            'lblStatus.Text = showStatus(resp(e.Item.ItemIndex).Reservation(0).Status)
            If resp(e.Item.ItemIndex).Reservation(0).Status = 3 Then
                If Not resp(e.Item.ItemIndex).Reservation(0).Item("DeadLine") Then
                    Me.PenaltyAmount = CType(resp(e.Item.ItemIndex).Reservation(0).Item("DeadLine"), Decimal)
                End If
            End If
            Dim tabla As String
            tabla = "<TABLE class='BorderTable' id='Table4' cellSpacing='0' cellPadding='0' width='99%' border='0'>"
            For indice As Integer = 0 To resp(e.Item.ItemIndex)._Event.Rows.Count - 1
                tabla += "<TR>"
                tabla += "<TD width='40'>"
                tabla += "<li> <span id='lblEvento' class='LabelBold' >" + resp(e.Item.ItemIndex)._Event(indice).EventName + "</span></li>"
                tabla += "</td>"

                tabla += "<TD width='25%'>"
                tabla += "<span id='lblDate1' class='Label' >" + resp(e.Item.ItemIndex)._Event(indice).StartDate + "</span>"
                If resp(e.Item.ItemIndex)._Event(indice).IsStartsHourNull = False Then tabla += "&nbsp;<span id='lblHour1' class='Label' >" + FormatHour(resp(e.Item.ItemIndex)._Event(indice).StartsHour) + "</span>"
                tabla += "</td>"
                tabla += "<TD width='20%'>"
                tabla += "<span id='lbltickets' class='Label' >" + CInt(resp(e.Item.ItemIndex).Price(0).Quantity).ToString("00") + " " + resp(e.Item.ItemIndex).Price(0).PriceName + "</span>"
                tabla += "</td>"
                tabla += "<TD width='15%'>"
                '  tabla += "<b><span id='lblstatus' class='Label' >" + showStatus(resp(e.Item.ItemIndex).Reservation(0).Status) + "</span></b>"
                tabla += "</td>"
                tabla += "</tr>"
                If resp(e.Item.ItemIndex).Reservation(0).Status = 1 Then
                    lblmensajeReservated.Visible = True
                End If
            Next
            tabla += "</TABLE>"
            contenedor.InnerHtml = tabla
        End If
    End Sub
    Private Function FormatHour(ByVal hora)
        Dim hr As String
        Dim hr2 As String
        Dim min As String
        hora = IIf(Trim(hora) = "", "0", Trim(hora))
        If ((hora >= 1200) And (hora <= 2400)) Then
            hr = getFormat(hora)
            If (hr.Substring(0, 2) >= 13) Then
                hr2 = hr.Substring(0, 2) - 12
                min = hr.Substring(3, 2)
                Return hr2 + ":" + min + " pm"
            Else
                Return hr + " pm"
            End If
        Else
            hr = getFormat(hora)
            Return hr + " am"
        End If
    End Function
    Private Function getFormat(ByVal hora)
        Dim hr As String
        Dim hr2 As String
        Dim min As String
        If (hora.length = 3) Then
            hr = hora.substring(0, 1)
            min = hora.substring(1, 2)
        Else
            If (hora.length = 2) Then
                hr = 12
                min = hora
            Else
                If (hora.length = 1) Then
                    hr = 12
                    min = "0" & hora
                Else
                    hr = hora.substring(0, 2)
                    min = hora.substring(2, 2)
                End If
            End If
        End If
        Return hr + ":" + min
    End Function
    'Private Function showStatus(ByVal StatusReservation As Integer) As String

    '    Try
    '        Select Case StatusReservation
    '            Case 1
    '                Me.PanelCancel.Visible = True
    '                Return "* " + PackagesLanguage.GetString("PACKAGES000302")
    '                lblmensajeReservated.Visible = True
    '            Case 2
    '                Me.PanelCancel.Visible = True
    '                Return PackagesLanguage.GetString("PACKAGES000491")
    '            Case 3
    '                Me.PanelCancel.Visible = False
    '                Return PackagesLanguage.GetString("PACKAGES000303")
    '            Case Else
    '                Return PackagesLanguage.GetString("PACKAGES000302")
    '        End Select
    '        status = StatusReservation

    '    Catch ex As Exception
    '        PortalLibraries.PortalServiceTracer.ServiceTracer("(showStatus) Error: " & ex.Message, PortalLibraries.PortalServiceTacerErrorTypes.Debug)
    '        Return ""
    '    End Try
    'End Function

    'Private Sub Idiomas()


    '    'btnCancel.Value = PackagesLanguage.GetString("PACKAGES000531")


    '    'Me.hplCancelRes.Text = PackagesLanguage.GetString("PACKAGES000341")
    '    'Me.lnkNo.Text = PackagesLanguage.GetString("PACKAGES000342")




    '    'lblConfirmTitle.Text = PackagesLanguage.GetString("PACKAGES000359")
    '    'lblConfirmPrompt.Text = PackagesLanguage.GetString("PACKAGES000360")
    '    'hplCancelRes.Text = PackagesLanguage.GetString("PACKAGES000361")
    '    'lnkNo.Text = PackagesLanguage.GetString("PACKAGES000362")
    '    'lnkBtnRulesRestrctions.InnerText = " " + PackagesLanguage.GetString("PACKAGES000354") + " "
    '    'lblRR1.Text = PackagesLanguage.GetString("PACKAGES000355")

    '    'Label4.Text = PackagesLanguage.GetString("PACKAGES000439")
    '    'lblEFees.Text = PackagesLanguage.GetString("PACKAGES000440")
    '    'lblETotalReservation.Text = "Total "
    '    'lblRR2.Text = PackagesLanguage.GetString("PACKAGES000357")
    '    'lblActivity.InnerText = PackagesLanguage.GetString("PACKAGES000532")

    '    'lblmensajeReservated.Text = PackagesLanguage.GetString("PACKAGES000543")
    'End Sub

    'Private Sub hplCancelRes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hplCancelRes.Click
    '    CancelarReservacion()
    'End Sub
    'Private Function wsCancelReservations(ByRef pckRS As Package_Cancel_RS) As Boolean


    'End Function

    'Private Sub CancelarReservacion()
    '    Try

    '    Catch ex As Exception

    '    End Try
    'End Sub
    'Private Sub SendEmail()

    'End Sub
End Class