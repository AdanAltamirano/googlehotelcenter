Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Data.SqlClient
Partial Class DisplayCarReservation
    Inherits PaginaBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents hplListRes As System.Web.UI.WebControls.HyperLink


    Protected WithEvents lblDirHotel As System.Web.UI.WebControls.Label
    Protected WithEvents lblCdHotel As System.Web.UI.WebControls.Label
    Protected WithEvents lblTitleRollAway As System.Web.UI.WebControls.Label
    Protected WithEvents LblCarName As System.Web.UI.WebControls.Label
    Protected WithEvents lblEImpuesto As System.Web.UI.WebControls.Label
    Protected WithEvents lblImpuestos As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblEFees As System.Web.UI.WebControls.Label
    Protected WithEvents lblFees As System.Web.UI.WebControls.Label
    Protected WithEvents lblETotalH As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotalH As System.Web.UI.WebControls.Label
    Protected WithEvents DivRollAwayData As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divClientportal As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents AgencyInfo As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub
    Private EventsActivity As DataTable
    Private ds As New System.Collections.Specialized.StringDictionary
    Dim dat As DataSet()
#End Region
    'spReservationGetData
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Me.IsPostBack = False Then
            Me.loadReservation(Me.Request.QueryString("id"))
        End If

    End Sub

    Public Property carReservation() As DataSet
        Get
            Return CType(HttpContext.Current.Session("CarsReservationV2"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            HttpContext.Current.Session("CarsReservationV2") = Value
        End Set
    End Property
    Public Property CarReservationId() As String
        Get
            Return ViewState("CarReservationId")
        End Get
        Set(ByVal Value As String)
            ViewState("CarReservationId") = Value
        End Set
    End Property
    Public Property FlightReservationId() As String
        Get
            Return ViewState("FlightReservationId")
        End Get
        Set(ByVal Value As String)
            ViewState("FlightReservationId") = Value
        End Set
    End Property
    Public Property ActivityReservationId() As String
        Get
            Return ViewState("ActivityReservationId")
        End Get
        Set(ByVal Value As String)
            ViewState("ActivityReservationId") = Value
        End Set
    End Property
    Public Property PckReservationId() As String
        Get
            Return ViewState("PckReservationId")
        End Get
        Set(ByVal Value As String)
            ViewState("PckReservationId") = Value
        End Set
    End Property
    Public Property PropertyID() As String
        Get
            If ViewState("PropertyID") Is Nothing Then ViewState("PropertyID") = "0"
            Return ViewState("PropertyID")
        End Get
        Set(ByVal Value As String)
            ViewState("PropertyID") = Value
        End Set
    End Property
    Private Sub showStatus(ByVal Status As Integer)
        Try


            Select Case Status
                Case 1
                    'Me.PanelCancel.Visible = True
                    lblStatus.Text = PortalCulture.GetString("M000331")


                    lblNoCanc.Visible = False
                Case 3
                    lblStatus.Text = PortalCulture.GetString("M000333")

                    lblNoCanc.Visible = True
                    Try
                        lblNoCancelacion.Text = carReservation.Tables("Reservation").Rows(0)("NoCancelacion")

                    Catch ex As Exception

                        lblNoCancelacion.Visible = False
                    End Try




                Case 10
                    lblStatus.Text = PortalCulture.GetString("M000331")
                    lblNoCanc.Visible = False

            End Select
        Catch ex As Exception
            ReservationNoFound("ShowStatus " + ex.ToString)
        End Try
    End Sub
    Private Function showStatusStr(ByVal Status As Integer) As String
        Try


            Select Case Status
                Case 1
                    'Me.PanelCancel.Visible = True
                    Return PortalCulture.GetString("M000331")



                Case 3
                    Return PortalCulture.GetString("M000333")



                Case 10
                    Return PortalCulture.GetString("M000331")

            End Select
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Private Sub ReservationNoFound(ByVal mensaje As String)
        Trace.Warn(mensaje)
        Response.Write("Reservacion No Disponible")
    End Sub


    Private Sub loadReservation(ByVal id As String)
        CheckReservation(id)
        loadCarReservation(id)
        loadActivitieReservation(id)
    End Sub
    Private Sub loadActivitieReservation(ByVal id As String)
        Try
            If ActivityReservationId = "0" Then
                TblActivitieReservation.Visible = False
                Exit Sub
            End If

            Dim activities As New DataSet
            activities = GetActivitiesReservations(PckReservationId)

            If Not activities Is Nothing Then
                If activities.Tables(0).Rows.Count > 0 Then
                    LoadTableEvents()

                    ReDim dat(activities.Tables(0).Rows.Count)
                    For indice As Integer = 0 To activities.Tables(0).Rows.Count - 1
                        dat(indice) = New DataSet
                        dat(indice) = GetDetailsActivitiesReservations(activities.Tables(0).Rows(indice)("ReservationID"))
                        Dim dr As DataRow
                        dr = EventsActivity.NewRow
                        dr(0) = indice
                        EventsActivity.Rows.Add(dr)
                        Trace.Warn(dat(indice).GetXml)
                    Next




                    'dat aqui tengo n arreglo con todas las reservacion de actividades de este paquete
                    Dim total As Decimal
                    Dim tax As Decimal
                    Dim subtotal As Decimal
                    Try
                        For indice As Integer = 0 To dat.Length - 1
                            total += CDec(dat(indice).Tables("Table5").Rows(0)("Total"))
                            tax += CDec(dat(indice).Tables("Table5").Rows(0)("Tax"))
                        Next


                        

                    Catch ex As Exception
                        Trace.Warn("Error detalle actividades Cargar totales: " + ex.ToString)
                    End Try
                    subtotal = total - tax
                    SubtotalActivitie.Text = FCurrency(subtotal, 2)
                    TaxActivitie.Text = FCurrency(tax, 2)
                    TotalActivitie.Text = FCurrency(total, 2) + " " + CStr(dat(0).Tables("Table2").Rows(0)("CurrencyCode"))



                    CargaGridTraveler()
                End If
            End If

        Catch ex As Exception
            Trace.Warn("Error detalle actividades: " + ex.ToString)
        End Try
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
    Private Sub loadCarReservation(ByVal id As String)
        Try
            If CarReservationId = "0" Then
                TblCarReservation.Visible = False
                Exit Sub
            End If
            Dim dat As New DataSet

            carReservation = Me.GetData("spReservationGetData", "@idReservacion", CarReservationId)

            Trace.Warn(carReservation.GetXml)
            Try
                showStatus(CInt(carReservation.Tables("Reservation").Rows(0)("Status")))

            Catch ex As Exception

            End Try

            'lblStatus.Text = 

            'Gal_Type
            Dim galtype As String = ""
            Dim currency As String = "USD"
            Try
                If CStr(Me.carReservation.Tables("Reservation").Rows(0)("idAutorenta")) = "0" Then
                    galtype = carReservation.Tables("Reservation").Rows(0)("Gal_Type")
                    lblDetCar.Text = getcardetail(galtype)
                    lblRecordLocator.Text = carReservation.Tables("Reservation").Rows(0)("NoReservacionGalileo")
                Else
                    currency = ""
                    lblERecordLocator.Visible = False
                    lblRecordLocator.Visible = False
                    lblDetCar.Text = CStr(Me.carReservation.Tables("Car").Rows(0)("Nombre")) + " " + CStr(Me.carReservation.Tables("Car").Rows(0)("texto"))
                End If


            Catch ex As Exception

                galtype = ""
            End Try

            Try
                lblDetCarSure.Text = carReservation.Tables("Insure").Rows(0)("descripcion")

            Catch ex As Exception
                lblDetCarSure.Text = ""
            End Try


            lblNoCancelacion.Text = ""


            Try
                lblIn.Text = Format(CDate(carReservation.Tables("Reservation").Rows(0)("Checkin")), "MMM/dd/yyyy")
                lblOut.Text = Format(CDate(carReservation.Tables("Reservation").Rows(0)("CheckOut")), "MMM/dd/yyyy")

            Catch ex As Exception
                lblIn.Visible = False
                lblOut.Visible = False
            End Try

            Try
                lblEDriver.Text = carReservation.Tables("Reservation").Rows(0)("NombreConductor")

            Catch ex As Exception
                lblEDriver.Visible = False
            End Try

            Try
                lblID.Text = carReservation.Tables("Reservation").Rows(0)("NoReservacion")


            Catch ex As Exception

            End Try

            Try
                CompanyName.Text = carReservation.Tables("Reservation").Rows(0)("NombreAutorenta")

            Catch ex As Exception
                CompanyName.Visible = False
            End Try

            Try
                If CStr(Me.carReservation.Tables("Reservation").Rows(0)("idAutorenta")) = "0" Then
                    lblAdressValue.Text = carReservation.Tables("Reservation").Rows(0)("Gal_AddressPickup")
                Else
                    Dim [of] As DataSet
                    [of] = GetCarOffice(CStr(Me.carReservation.Tables("Reservation").Rows(0)("Oficina_Checkin")))
                    lblAdressValue.Text = [of].Tables(0).Rows(0)("Domicilio")
                End If
            Catch ex As Exception
                lblAdressValue.Text = ""
                lblAdress.Text = ""
            End Try



            lblBase.Text = FCurrency(GetSubTotal, 2)
            lblSeguros.Text = FCurrency(GetSure, 2)
            lblTax.Text = FCurrency(GetTaxes, 2)
            lblTotal.Text = FCurrency(GetSubTotal() + GetTaxes() + GetSure(), 2) + " " + currency

        Catch ex As Exception
            Trace.Warn("Error General Error: " + ex.ToString)
        End Try
    End Sub


    Private Function GetSubTotal() As Decimal
        Try
            Dim Tot As Double
            Tot += CDbl(carReservation.Tables("Rate").Rows(0)("Precio")) * CInt(carReservation.Tables("Rate").Rows(0)("PrecioCant"))
            Tot += CDbl(carReservation.Tables("Rate").Rows(0)("PrecioDiaExtra")) * CInt(carReservation.Tables("Rate").Rows(0)("PrecioDiaExtraCant"))
            Tot += CDbl(carReservation.Tables("Rate").Rows(0)("PrecioHoraExtra")) * CInt(carReservation.Tables("Rate").Rows(0)("PrecioHoraExtraCant"))
            Tot += CDbl(carReservation.Tables("Rate").Rows(0)("PrecioSemana")) * CInt(carReservation.Tables("Rate").Rows(0)("PrecioSemanaCant"))
            Tot += CDbl(carReservation.Tables("Rate").Rows(0)("DropOff"))

            Return CDec(Tot)
        Catch ex As Exception
            Return 0.0
        End Try
    End Function

    Private Function GetSure() As Decimal
        Dim Tot As Double
        Try
            
            Dim dia As Integer
            dia = DateDiff(DateInterval.Day, CDate(carReservation.Tables("Reservation").Rows(0)("Checkin")), CDate(carReservation.Tables("Reservation").Rows(0)("CheckOut")))
            If dia = 0 Then dia = 1
            Tot += CDbl(carReservation.Tables("Insure").Rows(0)("monto")) * dia
            Return CDec(Tot)
        Catch ex As Exception
            Return 0
        End Try

    End Function

    Private Function GetTaxes() As Decimal
        Try
            Dim subtotal As Decimal = Me.GetSure + Me.GetSubTotal
            Dim imp As Decimal
            For indice As Integer = 0 To carReservation.Tables("Taxes").Rows.Count - 1
                imp += subtotal * CDbl(CDbl(carReservation.Tables("Taxes").Rows(indice)("Porcentaje")) / 100)
                imp += CDbl(carReservation.Tables("Taxes").Rows(indice)("monto"))
                subtotal += subtotal * CDbl(CDbl(carReservation.Tables("Taxes").Rows(indice)("Porcentaje")) / 100)
            Next
            Try
                If CStr(Me.carReservation.Tables("Reservation").Rows(0)("idAutorenta")) <> "0" Then
                    imp += subtotal * CDbl(CDbl(carReservation.Tables("Reservation").Rows(0)("Impuesto")) / 100)

                End If
            Catch ex As Exception

            End Try
            

            Return imp
        Catch ex As Exception
            Trace.Warn("impuestos Car : " + ex.ToString)
            Return 0
        End Try
    End Function

    Private Function GetData(ByVal cmdText As String, Optional ByVal paramName As String = Nothing, Optional ByVal paramValue As String = Nothing) As DataSet
        Dim data As New DataSet
        Dim dsCommand As New SqlDataAdapter
        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = cmdText
                    .Connection = New SqlConnection(AppSettings("AutoConnection"))
                    If Not paramName Is Nothing And Not paramValue Is Nothing Then
                        Dim param As SqlParameter = New SqlParameter(paramName, SqlDbType.NVarChar, 255)
                        param.Value = paramValue
                        .Parameters.Add(param)
                    End If
                    Dim param2 As SqlParameter = New SqlParameter("@idIdioma", SqlDbType.Int)
                    param2.Value = 1
                End With
                .Fill(data)
                data.Tables(0).TableName = "Reservation"
                data.Tables(1).TableName = "Taxes"
                data.Tables(2).TableName = "Rate"
                data.Tables(3).TableName = "Insure"
                data.Tables(4).TableName = "Car"
                data.Tables(5).TableName = "Packages"
                data.Tables(6).TableName = "RatePlanes"
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
        GetData = data
    End Function
    Private Sub CheckReservation(ByVal idReservacion As String)
        Try
            Dim displaymessaje As Boolean = False
            Dim dat As New DataSet
            dat = GetCheckPackages(idReservacion)
            If dat.Tables(0).Rows.Count > 0 Then

                'Autos
                Try
                    If Not dat.Tables(0).Rows(0)("CarReservationId") Is Nothing Then
                        CarReservationId = CStr(dat.Tables(0).Rows(0)("CarReservationId"))
                    End If
                Catch ex As Exception
                    CarReservationId = 0
                End Try


                'Vuelos
                Try

                    If Not dat.Tables(0).Rows(0)("FlightReservationId") Is Nothing Then

                        FlightReservationId = CStr(dat.Tables(0).Rows(0)("FlightReservationId"))
                    End If
                Catch ex As Exception
                    FlightReservationId = 0
                End Try


                'Actividades
                Try

                    If Not dat.Tables(0).Rows(0)("ActivityReservationId") Is Nothing Then

                        ActivityReservationId = CStr(dat.Tables(0).Rows(0)("ActivityReservationId"))
                        If ActivityReservationId = "1" Then
                            PckReservationId = CStr(dat.Tables(0).Rows(0)("Itinerary"))
                        End If

                    End If
                Catch ex As Exception
                    ActivityReservationId = 0
                End Try


            End If



        Catch ex As Exception
        End Try
    End Sub


    Private Function GetCheckPackages(ByVal idReservacion As String) As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spCheckReservvationPackgages", conection)
        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@IdReservacion", idReservacion))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Private Function GetCarOffice(ByVal idOficina As String) As DataSet
        Dim conection As New SqlConnection(AppSettings("AutoConnection"))
        Dim command As New SqlCommand("spOficinasGetByID", conection)
        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idOficina", idOficina))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function
    Private Function GetActivitiesReservations(ByVal idpk As String) As DataSet
        Dim conection As New SqlConnection(AppSettings("ActivityConnection"))
        Dim command As New SqlCommand("sp_activities_itr", conection)
        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@Itinerary", idpk))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Private Function GetDetailsActivitiesReservations(ByVal idreservation As String) As DataSet
        Dim conection As New SqlConnection(AppSettings("ActivityConnection"))
        Dim command As New SqlCommand("sp_activities_dsr", conection)
        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ReservationID", idreservation))
            .Parameters.Add(New SqlParameter("@language", PortalCulture.GetIDCulture))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Private Function getcardetail(ByVal sippcode As String) As String
        Try
            Return GetClase(sippcode.Substring(0, 1)) + ", " + Me.GetDescription(sippcode.Substring(1, 1)) + ", " + Me.GetTrassmision(sippcode.Substring(2, 1)) + ", " + Me.GetAC(sippcode.Substring(3, 1))
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Private Function GetTrassmision(ByVal Trassmision As String) As String
        Select Case Trassmision
            Case "M"
                Return PortalCulture.GetString("01080")
            Case Else
                Return PortalCulture.GetString("01081")
        End Select
    End Function
    Private Function GetAC(ByVal AC As String) As String
        Select Case AC
            Case "R"
                Return "AC"
            Case Else
                Return ""
        End Select
    End Function
    Private Function GetClase(ByVal clase As String) As String
        Select Case clase
            Case "M"
                Return PortalCulture.GetString("01059")
            Case "E"
                Return PortalCulture.GetString("01060")
            Case "C"
                Return PortalCulture.GetString("01061")
            Case "I"
                Return PortalCulture.GetString("01062")
            Case "S"
                Return PortalCulture.GetString("01063")
            Case "F"
                Return PortalCulture.GetString("01064")
            Case "P"
                Return PortalCulture.GetString("01065")
            Case "L"
                Return PortalCulture.GetString("01066")
            Case "X"
                Return PortalCulture.GetString("01067")
        End Select

        Return ""
    End Function

    Private Function GetDescription(ByVal description As String) As String
        Select Case description
            Case "B"
                Return PortalCulture.GetString("01068")
            Case "C"
                Return PortalCulture.GetString("01069")
            Case "D"
                Return PortalCulture.GetString("01070")
            Case "W"
                Return PortalCulture.GetString("01071")
            Case "V"
                Return PortalCulture.GetString("01072")
            Case "L"
                Return PortalCulture.GetString("01073")
            Case "S"
                Return PortalCulture.GetString("01074")
            Case "T"
                Return PortalCulture.GetString("01075")
            Case "F"
                Return PortalCulture.GetString("01076")
            Case "P"
                Return PortalCulture.GetString("01077")
            Case "J"
                Return PortalCulture.GetString("01078")
            Case "X"
                Return PortalCulture.GetString("01079")
        End Select

        Return ""
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        idiomas()
    End Sub

    Private Sub idiomas()
        lblEIn.Text = PortalCulture.GetString("M000078", True) 'This reservation is part of a package
        lblEOut.Text = PortalCulture.GetString("M000079", True) 'This reservation is part of a package
        lblEStatus.Text = PortalCulture.GetString("M000562", True) 'This reservation is part of a package
        lblReservationData.Text = PortalCulture.GetString("00352") 'This reservation is part of a package
        lblEID.Text = PortalCulture.GetString("M000325") 'This reservation is part of a package
        'lblERecordLocator.Text = PortalCulture.GetString("M000325") 'This reservation is part of a package
        'lblERecordLocator.Visible = False
        lblNHotel.Text = PortalCulture.GetString("01082")
        lblAdress.Text = PortalCulture.GetString("01083")
        lblCostos.Text = PortalCulture.GetString("M000330", True) 'M000330
        lblTarifBaseo.Text = PortalCulture.GetString("M0BT0000038", True) 'M000330
        lblSegurosText.Text = PortalCulture.GetString("01084", True) 'M000330
        lblETax.Text = PortalCulture.GetString("M0BT0000039", True) 'M000330
        lblETotal.Text = PortalCulture.GetString("M0BT0000040", True) 'M000330
        lblRaG.Text = PortalCulture.GetString("01085") 'M000330

        lblSubtotalActivitie.Text = PortalCulture.GetString("M0BT0000038", True) 'M000330
        lblTaxActivitie.Text = PortalCulture.GetString("M0BT0000039", True) 'M000330
        lblTotalActivitie.Text = PortalCulture.GetString("M0BT0000040", True) 'M000330
        lbltitulototalactividad.Text = PortalCulture.GetString("M000330", True)   'M000330

        lblConfirm.Text = PortalCulture.GetString("01087")
        lbltituloactividad.Text = PortalCulture.GetString("01057")
    End Sub

    Private Sub lstTravelersFliht_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles lstTravelersFliht.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            'lblmensajeReservated.Text = "* " + PortalCulture.GetString("PACKAGES000543")
            Dim lblEvento As Label
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
            lblEvento = e.Item.FindControl("lblEventoDescriptcion")
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
                Dim rvanumber As String = CStr(dat(0).Tables("Table5").Rows(0)("ReservationNumber")).ToString()
                PropertyName.Visible = False
                Table1.Visible = False
                If ds.ContainsKey(dat(e.Item.ItemIndex).Tables("Table2").Rows(0)("PropertyID")) = False Then
                    ds.Add(dat(e.Item.ItemIndex).Tables("Table2").Rows(0)("PropertyID"), "")
                    PropertyID += CStr(dat(e.Item.ItemIndex).Tables("Table2").Rows(0)("PropertyID")) + ","
                    PropertyName.Visible = True
                    'lblRevNum.Visible = True
                    'lblRvaNum.Visible = True
                    Table1.Visible = True
                    TblCustomer.Visible = True
                End If

                contenedor = e.Item.FindControl("contenedor")
                If Not lblEvento Is Nothing Then lblEvento.Text = PortalCulture.GetString("01086")
                PropertyName.Text = dat(e.Item.ItemIndex).Tables("Table2").Rows(0)("PropertyName")
                'lblRevNum.Text = PortalCulture.GetString("M000325")
                'lblRvaNum.Text = " " & rvanumber


                lMainContact.Text = dat(e.Item.ItemIndex).Tables("Table4").Rows(0)("Name") + " " + dat(0).Tables("Table4").Rows(0)("LAstName")

                lMainContactMail.Text = dat(e.Item.ItemIndex).Tables("Table4").Rows(0)("Email")

                lMainContactPhone.Text = dat(e.Item.ItemIndex).Tables("Table4").Rows(0)("Phone")

                ' LblMainContact.Text = PortalCulture.GetString("PACKAGES000540", True)

                'LblMainContactMail.Text = PortalCulture.GetString("PACKAGES000541", True)

                'LblMainContactPhone.Text = PortalCulture.GetString("PACKAGES000542", True)
                lblStatus.Visible = False

            Catch ex As Exception
                Trace.Warn("Detalles Datos principales Actividades : " + ex.ToString)
            End Try
            'lblStatus.Text = showStatus(resp(e.Item.ItemIndex).Reservation(0).Status)
            Dim tabla As String
            tabla = "<center><span id='idReserva'>" & _
                        PortalCulture.GetString("M000325") & dat(e.Item.ItemIndex).Tables("Table5").Rows(0)("ReservationNumber") & _
                    "</span></center>" & _
                    "<TABLE class='BorderTable' id='Table4' cellSpacing='0' cellPadding='0' width='99%' border='0'>"
            For indice As Integer = 0 To dat(e.Item.ItemIndex).Tables("Table6").Rows.Count - 1
                tabla += "<TR>"
                tabla += "<TD width='50%'>"
                tabla += "<li> <span id='lblEvento' class='LabelBold' >" + dat(e.Item.ItemIndex).Tables("Table6").Rows(indice)("EventName") + "</span></li>"
                tabla += "</td>"

                tabla += "<TD width='25%'>"
                tabla += "<span id='lblDate1' class='Label' >" + dat(e.Item.ItemIndex).Tables("Table6").Rows(indice)("StartDate") + "</span>"

                tabla += "</td>"
                tabla += "<TD width='20%'>"
                tabla += "<span id='lbltickets' class='Label' >" + CInt(dat(e.Item.ItemIndex).Tables("Table7").Rows(indice)("Quantity")).ToString("00") + " " + dat(e.Item.ItemIndex).Tables("Table7").Rows(indice)("PriceName") + "</span>"
                tabla += "</td>"
                tabla += "<TD width='5%'>"
                tabla += "<b><span id='lblstatus' class='Label' >" + showStatusStr(CInt(dat(e.Item.ItemIndex).Tables("Table5").Rows(0)("Status"))) + "</span></b>"
                tabla += "</td>"
                tabla += "</tr>"

            Next
            tabla += "</TABLE>"
            Trace.Warn(tabla)
            Try
                contenedor.InnerHtml = tabla
            Catch ex As Exception

            End Try

        End If
    End Sub
End Class
