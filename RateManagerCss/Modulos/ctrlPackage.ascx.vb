Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade
Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports Portal.Hotel.DataAccess
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging


Partial Class ctrlPackage
    Inherits UserControlBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents RVPromotion As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents rfvPrice As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents lblERatesPlans As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlratesplans99 As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents chkAppTemp As System.Web.UI.WebControls.CheckBox
    Protected WithEvents dgSeasons As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblTDesde As System.Web.UI.WebControls.Label
    Protected WithEvents chkFlight As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkCar As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkActivity As System.Web.UI.WebControls.CheckBox
    Protected WithEvents imgRubroClose As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents txtVuelo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblFligh As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents ddlAerolinea As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlFranquicia As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlActEmp As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtSipp As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCarProm As System.Web.UI.WebControls.TextBox
    Protected WithEvents lstActividades As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents txtActProm As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSipp As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblCarProm As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblActProm As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Enum dgcolumns
        codigohabitacion
        idtipohabitacion_hotel
        FechaInicia
        FechaFinaliza
        TarifaAdulto
        Eliminar
        IdTarifa
        EditarTarifas
        Excepciones
        packageType
        Precio
    End Enum

    Public ReadOnly Property ddlRulesClientID() As String
        Get
            Return ddlRules.ClientID
        End Get
    End Property


    Const KEY_HOTELID As String = "HotelId"

    Public Property IdRatePlan() As String
        Get
            Return ViewState("_IdRatePlan")
        End Get
        Set(ByVal Value As String)
            ViewState("_IdRatePlan") = Value
        End Set
    End Property

    Public Property idRateCode() As String
        Get
            Return ViewState("idRateCode")
        End Get
        Set(ByVal Value As String)
            ViewState("idRateCode") = Value
        End Set
    End Property

    Public ReadOnly Property isSourceSelected() As Boolean
        Get
            Return Me.chkGDS.Checked Or Me.chkPortal.Checked Or Me.chkUnipantalla.Checked Or Me.chkADS.Checked
        End Get
    End Property
    Public Property edicion() As Boolean
        Get
            Return ViewState("Edicion")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Edicion") = Value
           
        End Set
    End Property
    Public Property IdPaquete() As Integer
        Get
            Return ViewState("_IdPaquete")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_IdPaquete") = Value
        End Set
    End Property

    Private Property idDicc() As Integer
        Get
            Return ViewState("idDicc")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idDicc") = Value
        End Set
    End Property
    Private Property idShortDesc() As Integer
        Get
            Return ViewState("idShortDesc")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idShortDesc") = Value
        End Set
    End Property

    Public Property idCompany() As Integer
        Get
            If ViewState.Item("idCompany") Is Nothing Then
                Return 0
            Else
                Return ViewState.Item("idCompany")
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState("idCompany") = Value
        End Set
    End Property

    Private Property dsRooms() As RoomsHotelData
        Get
            Return Session("_dsrooms")
        End Get
        Set(ByVal Value As RoomsHotelData)
            Session("_dsrooms") = Value
        End Set
    End Property

    Public Property TipoPrecio() As Byte
        Get
            Return ViewState.Item("TipoPrecio")
        End Get
        Set(ByVal Value As Byte)
            ViewState("TipoPrecio") = Value
        End Set
    End Property

    Public Property Adultos() As Byte
        Get
            Return ViewState.Item("Adultos")
        End Get
        Set(ByVal Value As Byte)
            ViewState("Adultos") = Value
        End Set
    End Property
    Public Property Ninios() As Byte
        Get
            Return ViewState.Item("Ninios")
        End Get
        Set(ByVal Value As Byte)
            ViewState("Ninios") = Value
        End Set
    End Property


    Const KEY_MINPRICE As String = "mintarifaAdulto"
    Const KEY_MAXPRICE As String = "maxtarifaAdulto"

    Protected WithEvents txtDescripcion As CtrlIdioma
    Protected WithEvents txtShortDescription As CtrlIdioma
    Protected WithEvents ctrPortal1 As ctrPortal
    Protected WithEvents PackageRubros As CtrlPackageRubros

    Public descripcionError As String = String.Empty
    Private faresrest As FaresRestrictionsData

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        '''''''''''''''''''''''''''''''''''''''
        Me.txtDescripcion.RequiredText = False
        '''''''''''''''''''''''''''''''''''''''
        If Not IsPostBack Then
            loadRules()
            cargarDatosHotel()
            loadrooms()
        End If
        cvMaxPeople.IsValid = True
        chkPortal.Attributes.Add("onclick", "javascript:onCheckBoxesClick('" & Me.chkPortal.ClientID & "','" & trPortal.ClientID & "');")
        Me.chkGDS.Attributes.Add("onclick", "javascript:onCheckBoxesClick('" & Me.chkGDS.ClientID & "','trApplyGDS');")
        Me.RdbOcupacion.Attributes.Add("onclick", "javascript:ShowOccupation('" & Me.RdbOcupacion.ClientID & "','" & Me.txtMaxAd.ClientID & "');")
        Me.RdbPaquete.Attributes.Add("onclick", "javascript:ShowOccupation('" & Me.RdbOcupacion.ClientID & "','" & Me.txtMaxAd.ClientID & "');")
        Me.RdbPersona.Attributes.Add("onclick", "javascript:ShowOccupation('" & Me.RdbOcupacion.ClientID & "','" & Me.txtMaxAd.ClientID & "');")
        Me.txtMaxAd.Attributes.Add("onchange", "javascript:CreateDsOcupattion('" & Me.RdbOcupacion.ClientID & "','tblPrices','" & txtMaxAd.ClientID & "','" & Me.btnAddOccRate.ClientID & "');CreateDsOcupattion('" & Me.RdbOcupacion.ClientID & "','tblPricesExc','" & txtMaxAd.ClientID & "','" & Me.btnAddOccRate.ClientID & "');")

        imgAddDate.Attributes.Add("onclick", "javascript:AddDatesLocal('" & Me.txtDateFrom.ClientID & "','" & Me.txtDateTo.ClientID & "','" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "','" & PortalCulture.GetString("M000197") & "','" & PortalCulture.GetString("00514") & "');")
        imgDeleteDate.Attributes.Add("onclick", "javascript:DeleteDate('" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "','" & Me.txtPrices.ClientID & "','" & Me.txtPricesE.ClientID & "');")

        'Me.Page.RegisterStartupScript("CallScripts", "<script>onCheckBoxesClick('" & Me.chkPortal.ClientID & "','" & trPortal.ClientID & "');" & _
        '        "onCheckBoxesClick('" & Me.chkGDS.ClientID & "','trApplyGDS');" & _
        '                "ShowOccupation('" & Me.RdbOcupacion.ClientID & "','" & Me.txtMaxAd.ClientID & "');" & _
        '        "CreateDsOcupattion('" & Me.RdbOcupacion.ClientID & "','tblPrices','" & txtMaxAd.ClientID & "','" & Me.btnAddOccRate.ClientID & "');CreateDsOcupattion('" & Me.RdbOcupacion.ClientID & "','tblPricesExc','" & txtMaxAd.ClientID & "','" & Me.btnAddOccRate.ClientID & "');ShowPageDg('" & Me.dgRooms.ClientID & "',1);</script>")

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "CallScripts", "<script>onCheckBoxesClick('" & Me.chkPortal.ClientID & "','" & trPortal.ClientID & "');" & _
                "onCheckBoxesClick('" & Me.chkGDS.ClientID & "','trApplyGDS');" & _
                        "ShowOccupation('" & Me.RdbOcupacion.ClientID & "','" & Me.txtMaxAd.ClientID & "');" & _
                "CreateDsOcupattion('" & Me.RdbOcupacion.ClientID & "','tblPrices','" & txtMaxAd.ClientID & "','" & Me.btnAddOccRate.ClientID & "');CreateDsOcupattion('" & Me.RdbOcupacion.ClientID & "','tblPricesExc','" & txtMaxAd.ClientID & "','" & Me.btnAddOccRate.ClientID & "');ShowPageDg('" & Me.dgRooms.ClientID & "',1);</script>")

        Me.btnCancelOccRate.Attributes.Add("onclick", "javascript:CancelEditDgOcupancyRate('" & Me.chk1.ClientID & "','" & Me.chk2.ClientID & "','" & Me.chk3.ClientID & "','" & Me.chk4.ClientID & "','" & Me.chk5.ClientID & "','" & Me.chk6.ClientID & "','" & Me.chk7.ClientID & "','" & btnCancelOccRate.ClientID & "','" & btnAddOccRate.ClientID & "','" & chkHabitaciones.ClientID & "');")
        Me.btnAddOccRate.Attributes.Add("onclick", "javascript:addOcupancyRate('" & txtPrices.ClientID & "','" & txtPricesE.ClientID & "','" & chkHabitaciones.ClientID & "','" & Me.chk1.ClientID & "','" & Me.chk2.ClientID & "','" & Me.chk3.ClientID & "','" & Me.chk4.ClientID & "','" & Me.chk5.ClientID & "','" & Me.chk6.ClientID & "','" & Me.chk7.ClientID & "','" & Me.txtDateFrom.ClientID & "','" & Me.txtDateTo.ClientID & "','" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "','" & Me.dgRooms.ClientID & "','" & Me.txtStart.ClientID & "','" & Me.txtEnd.ClientID & "','" & Me.RdbOcupacion.ClientID & "','" & Me.RdbPaquete.ClientID & "','" & Me.RdbPersona.ClientID & "','" & btnCancelOccRate.ClientID & "','" & btnAddOccRate.ClientID & "');")
        Me.ADetails.Attributes("OnClick") = "javascript:ShowDetails('divRoomsDetails', '');ShowDetails('" & Me.lstDates.ClientID & "', 'none');"
        Me.imgclose.Attributes("OnClick") = "javascript:ShowDetails('divRoomsDetails', 'none');ShowDetails('" & Me.lstDates.ClientID & "', '');"
        aPaqueteRubro.Attributes.Add("onclick", "javascript:ShowDetails('dvPaqueteRubro', '');ShowDetails('" & ddlTypePrice.ClientID & "', 'none');ShowDetails('" & Me.lstDates.ClientID & "', 'none');")
        Me.PackageRubros.AddScripts(Me.ddlTypePrice.ClientID, Me.lstDates.ClientID)
        txtRateCode.Attributes.Add("onkeypress", "return validarkeyCode(event);")

        If CType(Me.Page, PaginaBase).IsSupervisor Or CType(Me.Page, PaginaBase).isUserChain Then
            Me.aPaqueteRubro.Visible = True
        Else
            Me.aPaqueteRubro.Visible = False
        End If

    End Sub
    Public Function lstDatesCount() As Integer
        Return Me.txtFechas.Text.Split("$").Length() - 1
    End Function
    Public Function lstDatesItemI(ByVal i As Integer) As String
        Return Me.txtFechas.Text.Split("$").GetValue(i)
    End Function

    Public Sub lstDatesAdd()
        Me.txtFechas.Text = "$" & txtDateFrom.Text & "-" & Me.txtDateTo.Text
    End Sub

    Private Sub cargarDatosHotel()
        Dim dsHotel As HotelDatos
        ' Dim dsEtiq As MonedaDatos

        With New HotelSistema
            dsHotel = .GetHotelById(CType(Me.Page, PaginaBase).cInfoActual.Hotel)
        End With

        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 Then
            With dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0)
                Me.chkGDS.Checked = IIf(.IsNull(HotelDatos.FIELD_AvailOnGDS) OrElse .Item(HotelDatos.FIELD_AvailOnGDS) = 0, False, True)
                Me.chkPortal.Checked = IIf(.IsNull(HotelDatos.FIELD_AvailOnPortal) OrElse .Item(HotelDatos.FIELD_AvailOnPortal) = 0, False, True)
                Me.chkUnipantalla.Checked = IIf(.IsNull(HotelDatos.FIELD_AvailOnOnePage) OrElse .Item(HotelDatos.FIELD_AvailOnOnePage) = 0, False, True)
                Me.chkADS.Checked = IIf(.IsNull(HotelDatos.FIELD_AvailOnADS) OrElse .Item(HotelDatos.FIELD_AvailOnADS) = 0, False, True)
                Me.PackageRubros.CityId = .Item("IdCiudadEmpresa")

            End With
        End If
    End Sub



    Private Sub loadRules()
        Dim rules As RatesPlanRulesData
        With New RatesPlanRulesFacade
            rules = .getList(CType(Me.Page, PaginaBase).cInfoActual.Hotel)
        End With
        Me.ddlRules.DataSource = rules.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES)
        ddlRules.DataTextField = RatesPlanRulesData.FIELD_DESCRIPTION
        ddlRules.DataValueField = RatesPlanRulesData.FIELD_IDRULE
        ddlRules.DataBind()
        ddlRules.Items.Insert(0, PortalCulture.GetString("00027"))
        ddlRules.Items(0).Value = 0
    End Sub

    Private Function IsValidPackage() As Boolean
        If IsValidRoom() Then
            'Dim dsRooms As RoomsHotelData
            'Dim dr As DataRow

            descripcionError = String.Empty
            Dim fecha As Date
            Try
                fecha = CType(txtEnd.Text, Date)
                fecha = CType(txtStart.Text, Date)
            Catch ex As Exception
                descripcionError = PortalCulture.GetString("00313")
                Return False
            End Try

            If Not Regex.IsMatch(txtRateCode.Text.Trim, "^[A-Z0-9 a-z]*$") Then
                descripcionError = PortalCulture.GetString("01512")
                Return False
            End If

            If chkPortal.Checked = True And (ctrPortal1.validaSeleccionPortal(Me.chkPortal.Checked)) = False Then
                descripcionError = PortalCulture.GetString("01012")
                Return False
            End If

            Return True
        Else
            Return False
        End If
    End Function

    Private Sub GuardaImagen(ByVal RatePlan As String, ByVal idCompany As Integer)
        '**********************************************************************************************
        'GUARDA LA IMAGEN DEL TAMAÑO DEL MODULO  
        'AppSettings("DIR_TIPO_HAB") + IdCompamy + Tipohabitacion
        'Dim oImg As System.Drawing.Image

        Dim Bit As Bitmap
        Try
            If Not Directory.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_Package") & idCompany & "/") Then
                Directory.CreateDirectory(AppSettings("MapPathCRS") & AppSettings("DIR_Package") & idCompany & "/")
            End If

            Dim BitImage As New Bitmap(fileImagen.PostedFile.InputStream)
            Dim newSize As Size
            'ORIGINAL - SALVANDO IMAGEN  (IdRoom)
            fileImagen.PostedFile.SaveAs(AppSettings("MapPathCRS") & AppSettings("DIR_Package") & idCompany & "/" & RatePlan)
            'MODULO - SALVANDO IMAGEN    (IdRoom + "_M")
            newSize = New Size(160, 120)
            Bit = BitImage.GetThumbnailImage(newSize.Width, newSize.Height, Nothing, Nothing)
            Bit.Save(AppSettings("MapPathCRS") & AppSettings("DIR_Package") & idCompany & "/" & RatePlan & "_M", ImageFormat.Jpeg)
            'THUMBNAL - SALVANDO IMAGEN  (IdRoom + "_T")
            newSize = New Size(70, 70)
            Bit = BitImage.GetThumbnailImage(newSize.Width, newSize.Height, Nothing, Nothing)
            Bit.Save(AppSettings("MapPathCRS") & AppSettings("DIR_Package") & idCompany & "/" & RatePlan & "_T", ImageFormat.Jpeg)

        Catch ex As Exception
            'saveImage = False
        Finally
            If Not Bit Is Nothing Then
                Bit.Dispose()
            End If
        End Try
    End Sub

    Public Function IMG_Exists(ByVal IdImg As String, ByRef img As System.Web.UI.WebControls.Image) As Boolean
        Dim tmpCheckExists As Boolean = File.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_Package") & idCompany & "/" & IdImg & "_T")

        If tmpCheckExists Then
            img.ImageUrl = AppSettings("urlCRS") & AppSettings("DIR_Package") & idCompany & "/" & IdImg & "_T?" & Now.ToString
            IMG_Exists = tmpCheckExists
        Else
            img.ImageUrl = ""
            Return False
        End If
    End Function

    Public Function SavePlan(ByVal publish As Boolean) As Integer
        Dim sData As String = ""
        Dim sDataPrev As String = ""

        If Not IsValidPackage() Then
            If descripcionError <> String.Empty Then
                Return -3
            Else
                Return -1
            End If
        End If

        Dim dsRate As New RatePlanData
        Dim Rp As RatePlanData
        Dim rRate As DataRow
        'Dim val As Boolean
        'Dim NuevoIdRate As String
        Dim dvRp As DataView
        Dim idAsoc As Integer = Me.GetIdAsociation

        Dim QueryDinamicPackage As String = PackageRubros.GetLinkUrl

        With New RatePlanFacade
            Rp = .GetRatePlanByIdHotel(CType(Me.Page, PaginaBase).cInfoActual.Hotel, idAsociacion:=idAsoc)
        End With
        sDataPrev = Util.Utility.GetXml(Rp.RATEPLAN_TABLE, "UpdatePlanFaresNR", Rp)

        dvRp = Rp.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView

        If Not Me.edicion Then
            dvRp.RowFilter = RatePlanData.FIELD_CODIGOTARIFA & "='" & Me.txtRateCode.Text.Trim & "' and " & RatePlanData.FIELD_IDHOTEL & "=" & CType(Me.Page, PaginaBase).cInfoActual.Hotel
        Else
            dvRp.RowFilter = RatePlanData.FIELD_CODIGOTARIFA & "='" & Me.txtRateCode.Text.Trim & "' and " & RatePlanData.FIELD_IDHOTEL & "=" & CType(Me.Page, PaginaBase).cInfoActual.Hotel & " and " & RatePlanData.FIELD_IDRATEPLAN & "<>'" & Me.IdRatePlan & "'"
        End If

        If dvRp.Count > 0 Then
            Return 2
        End If

        rRate = dsRate.Tables(RatePlanData.RATEPLAN_TABLE).NewRow()

        With rRate
            If Me.edicion = False Then
                .Item(RatePlanData.FIELD_IDRATEPLAN) = Me.txtRateCode.Text.ToUpper 'Me.txtCodigo.Text.ToUpper
            Else
                .Item(RatePlanData.FIELD_IDRATEPLAN) = IdRatePlan
            End If

            .Item(RatePlanData.FIELD_DESCRIPTION) = Me.txtDescripcion.textodefault

            .Item(RatePlanData.FIELD_IDHOTEL) = CType(Me.Page, PaginaBase).cInfoActual.Hotel
            .Item(RatePlanData.FIELD_SEGMENT) = "K" 'paquete
            .Item(RatePlanData.FIELD_IDDICDESC) = Me.txtDescripcion.IdIndice
            .Item(RatePlanData.FIELD_CODIGOTARIFA) = Me.txtRateCode.Text.ToUpper
            .Item(RatePlanData.FIELD_NAME) = Me.txtShortDescription.textodefault
            .Item(RatePlanData.FIELD_IDDICSHORTDESC) = Me.txtShortDescription.IdIndice
            .Item(RatePlanData.FIELD_GDS) = chkGDS.Checked
            .Item(RatePlanData.FIELD_PORTAL) = chkPortal.Checked
            .Item(RatePlanData.FIELD_UNIPANTALLA) = chkUnipantalla.Checked
            .Item(RatePlanData.FIELD_ADS) = chkADS.Checked


            .Item(RatePlanData.FIELD_IDRULE) = System.DBNull.Value

            If ddlRules.SelectedValue <> 0 Then
                .Item(RatePlanData.FIELD_IDRULE) = ddlRules.SelectedValue
            End If

            Dim GDSAplicado As String = "NNNN"

            If chkGDS.Checked Then
                GDSAplicado = ""
                If chkGDSAmadeus.Checked = True Then
                    GDSAplicado = GDSAplicado + "Y"
                Else
                    GDSAplicado = GDSAplicado + "N"
                End If

                If chkGDSGalileo.Checked = True Then
                    GDSAplicado = GDSAplicado + "Y"
                Else
                    GDSAplicado = GDSAplicado + "N"
                End If

                If chkGDSSabre.Checked = True Then
                    GDSAplicado = GDSAplicado + "Y"
                Else
                    GDSAplicado = GDSAplicado + "N"
                End If

                If chkGDSWorldSpan.Checked = True Then
                    GDSAplicado = GDSAplicado + "Y"
                Else
                    GDSAplicado = GDSAplicado + "N"
                End If
            End If
            .Item(RatePlanData.FIELD_GDS) = chkGDS.Checked
            .Item(RatePlanData.FIELD_GDSAPPLY) = GDSAplicado

            If QueryDinamicPackage <> "" Then
                'solo disponible para portal
                .Item(RatePlanData.FIELD_GDS) = False
                .Item(RatePlanData.FIELD_PORTAL) = True
                .Item(RatePlanData.FIELD_UNIPANTALLA) = False
                .Item(RatePlanData.FIELD_ADS) = False
            End If
        End With

        dsRate.Tables(RatePlanData.RATEPLAN_TABLE).Rows.Add(rRate)

        Dim dsPackage As New PackageData
        Dim dr As DataRow
        Dim f As String
        dr = dsPackage.Tables(PackageData.Package_TABLE).NewRow
        f = txtEnd.Text
        dr(PackageData.FIELD_enddate) = New Date(f.Split("/")(2), f.Split("/")(0), f.Split("/")(1))
        f = txtStart.Text
        dr(PackageData.FIELD_startdate) = New Date(f.Split("/")(2), f.Split("/")(0), f.Split("/")(1))

        dr(PackageData.FIELD_RateCode) = txtRateCode.Text


        dr(PackageData.FIELD_Price) = GetLowestRate() ' CDbl(txtPrecio.Text) 'txtRateCode.Text
        dr(PackageData.FIELD_TypePrice) = 0

        dr(PackageData.FIELD_MaxAdultos) = CDbl(txtMaxAd.Text)
        dr(PackageData.FIELD_MaxChildren) = CDbl(txtMaxChild.Text)
        dr(PackageData.FIELD_IDHOTEL) = CDbl(CType(Me.Page, PaginaBase).cInfoActual.Hotel)
        dr(PackageData.FIELD_Nights) = CInt(txtDaysFree.Text)

        dr(PackageData.FIELD_LinkpaqueteArmado) = QueryDinamicPackage
        If dr(PackageData.FIELD_LinkpaqueteArmado) <> "" Then
            dr(PackageData.FIELD_Price) = CDbl(txtPrecio.Text)
            dr(PackageData.FIELD_TypePrice) = ddlTypePrice.SelectedIndex
            dr(PackageData.FIELD_LinkpaqueteArmado) &= "&HtlProvider=0" & "&PropertyNumber=" & CType(Me.Page, PaginaBase).cInfoActual.Hotel & "&HtlCode=" & Me.txtRateCode.Text.ToUpper & "&Adults=" & Me.txtMaxAd.Text & "&Children=" & Me.txtMaxChild.Text
        End If

        dsPackage.Tables(PackageData.Package_TABLE).Rows.Add(dr)

        If Me.edicion = False Then
            With New RatePlanAccess
                If .InsertRtPlan(dsRate, Me.idDicc, Me.idShortDesc) Then
                    sData = dsRate.GetXml
                    sDataPrev = ""
                    CType(Me.Page, PaginaBase).guardalog("/Pages/package.aspx", PaginaBase.acciones.Crear, "Creó el paquete con el id " & rRate(RatePlanData.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(RatePlanData.FIELD_CODIGOTARIFA), "", sDataPrev, sData)
                    If publish Then
                        CType(Me.Page, PaginaBase).guardalog("/Pages/package.aspx", PaginaBase.acciones.Publicar, "Modifico el paquete con el id " & rRate(RatePlanData.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(RatePlanData.FIELD_CODIGOTARIFA), "", sDataPrev, sData)
                    End If
                    Me.txtDescripcion.Update(dsRate.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0)(RatePlanData.FIELD_IDDICDESC))
                    Me.txtShortDescription.Update(dsRate.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0)(RatePlanData.FIELD_IDDICSHORTDESC))
                    If Me.txtDescripcion.HasChanges OrElse Me.txtShortDescription.HasChanges Then
                        CType(Me.Page, PaginaBase).NotifyContentModification("Paquete con el codigo " & rRate(RatePlanData.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "Paquetes")
                    End If
                    With New PackageDataAccess
                        If .InsertPackage(dsPackage) Then
                            ctrPortal1.InsertarPortales(CType(Me.Page, PaginaBase).cInfoActual.Hotel, 2, txtRateCode.Text, chkPortal.Checked)
                            PackageRubros.InsertPaqueteArmado(dsPackage.Tables(PackageData.Package_TABLE).Rows(0)(PackageData.FIELD_IdPaquete))
                        End If
                    End With
                    SaveTarifas()
                    GuardaImagen(txtRateCode.Text.Trim, Me.idCompany)
                    '  paqueteNuevo = txtRateCode.Text
                    ClearData()
                    Return 0
                End If
            End With
        Else
            With New RatePlanAccess
                If Me.idDicc <> 0 Then
                    Me.txtDescripcion.Update(Me.idDicc, publish)
                Else
                    idDicc = Me.txtDescripcion.Insert()
                End If
                If Me.idShortDesc <> 0 Then
                    Me.txtShortDescription.Update(Me.idShortDesc, publish)
                Else
                    idShortDesc = Me.txtShortDescription.Insert()
                End If

                dsRate.Tables(RatePlanData.RATEPLAN_TABLE).AcceptChanges()
                dsRate.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0).Item(RatePlanData.FIELD_IDHOTEL) = dsRate.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0).Item(RatePlanData.FIELD_IDHOTEL)
                If idDicc <> 0 Then
                    dsRate.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0).Item(RatePlanData.FIELD_IDDICDESC) = idDicc
                End If

                If idShortDesc <> 0 Then
                    dsRate.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0).Item(RatePlanData.FIELD_IDDICSHORTDESC) = idShortDesc
                End If
                dsPackage.AcceptChanges()
                dsPackage.Tables(PackageData.Package_TABLE).Rows(0).Item(PackageData.FIELD_IDHOTEL) = dsPackage.Tables(PackageData.Package_TABLE).Rows(0).Item(PackageData.FIELD_IDHOTEL)
                With New RatePlanAccess
                    If .UpdateRtPlan(dsRate) Then
                        sData = Util.Utility.GetXml(dsRate.RATEPLAN_TABLE, "UpdatePlanFaresNR", dsRate)
                        CType(Me.Page, PaginaBase).guardalog("/Pages/package.aspx", If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar), "Modifico el paquete con el id " & rRate(RatePlanData.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(RatePlanData.FIELD_CODIGOTARIFA), "", sDataPrev, sData)
                        If Me.txtDescripcion.HasChanges OrElse Me.txtShortDescription.HasChanges Then
                            CType(Me.Page, PaginaBase).NotifyContentModification("Paquete con el codigo " & rRate(RatePlanData.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "Paquetes")
                        End If
                        With New PackageDataAccess
                            .UpdatePackage(dsPackage, Me.idRateCode)
                            PackageRubros.InsertPaqueteArmado(Me.IdPaquete)
                            ctrPortal1.ModificarPortales(chkPortal.Checked)
                        End With
                        SaveTarifas()
                        GuardaImagen(txtRateCode.Text.Trim, Me.idCompany)
                        'paqueteNuevo = ""
                        ClearData()
                        Return 0
                    End If
                End With

            End With
        End If

        Return 2
    End Function

#Region "para las tarifas"
    Private Function SaveTarifas() As Boolean
        'Dim idRoom As Integer
        'buscar el codigo que le pertenece
        ' Dim dsFar As FaresData
        'Dim dv As DataView
        Dim idx As Double = -1
        Dim datFares As New FaresData
        Dim datRestrictions As New FaresRestrictionsData
        Dim datRestrictionsDelete As New FaresRestrictionsData
        Dim idxModified As String = ","

        'TODO: ver las habitaciones que están agregadas en el arreglo
        If Not Me.edicion Then
            For Each drRoom As ListItem In Me.chkHabitaciones.Items
                Dim sw As Boolean = False
                For i As Integer = 0 To Me.lstDatesCount
                    If Me.lstDatesItemI(i).IndexOf(":" & drRoom.Text.Split("--")(0)) > 0 Then
                        Dim f1, f2 As Date
                        f1 = CDate(lstDatesItemI(i).Split("-")(0))
                        Dim str As String = lstDatesItemI(i).Split("-")(1)
                        If str.ToString.IndexOf(":") > 0 Then
                            str = str.Substring(0, str.IndexOf(":"))
                        End If
                        f2 = CDate(str)
                        'buscarla en el txt
                        idx = -1
                        For Each str1 As String In Me.txtPrices.Text.Split("$")
                            idx += 1
                            If str1 <> "" Then
                                If str1.Split(",")(1) = lstDatesItemI(i).Split("-")(0) AndAlso str1.Split(",")(2) = str Then
                                    Dim Auxroom As String = str1
                                    Auxroom = str1.Substring(0, Auxroom.ToString.IndexOf(","))
                                    If Auxroom = drRoom.Text.Split("--")(0) Then
                                        saveRoomData(drRoom.Value, str1.Split(",")(0), True, f1, f2, idx)
                                        Exit For
                                    End If
                                End If
                            End If
                        Next

                        sw = True
                    End If
                Next
                If Not sw Then
                    'va a a eliminar la tarifa
                    saveRoomData(drRoom.Value, drRoom.Text.Split("--")(0), False, "", "")
                End If
            Next
        Else
            'edición por temporatas
            GetDgFares(datFares, datRestrictions, datRestrictionsDelete, idxModified, idx)
            Dim rowFare As DataRow
            For Each drRoom As ListItem In Me.chkHabitaciones.Items
                For i As Integer = 0 To Me.lstDatesCount
                    If Me.lstDatesItemI(i).IndexOf(":" & drRoom.Text.Split("--")(0)) > 0 Then
                        Dim f1, f2 As Date
                        f1 = CDate(lstDatesItemI(i).Split("-")(0))
                        Dim str As String = lstDatesItemI(i).Split("-")(1)
                        If str.ToString.IndexOf(":") > 0 Then
                            str = str.Substring(0, str.IndexOf(":"))
                        End If
                        f2 = CDate(str)
                        ' If Me.RdbOcupacion.Checked Then
                        idx = -1
                        For Each strP As String In Me.txtPrices.Text.Split("$")
                            idx += 1
                            If strP <> "" Then
                                If strP.Split(",")(1) <> "" Then
                                    Dim Pf1, Pf2 As Date
                                    Pf1 = CDate(strP.Split(",")(1))
                                    Pf2 = CDate(strP.Split(",")(2))
                                    If Pf1 = f1 And Pf2 = f2 Then
                                        Dim Auxroom As String = strP
                                        Auxroom = strP.Substring(0, Auxroom.ToString.IndexOf(","))
                                        If Auxroom = drRoom.Text.Split("--")(0) Then
                                            Dim strE As String = Me.txtPricesE.Text.Split("$")(idx)
                                            strE = strE.Split(",")(3)

                                            Dim StrTipoPaquete As String = ""
                                            For Each strTipoPrecio As String In itxtTipoPrecio.Value.Split("$")
                                                If strTipoPrecio <> "" Then
                                                    If strTipoPrecio.Split(",")(0) = drRoom.Text.Split("--")(0) AndAlso CDate(strTipoPrecio.Split(",")(1)) = f1 AndAlso CDate(strTipoPrecio.Split(",")(2)) = f2 Then
                                                        StrTipoPaquete = strTipoPrecio
                                                        Exit For
                                                    End If
                                                End If
                                            Next

                                            rowFare = AddrowFare(datFares, f1, f2, drRoom.Value, drRoom.Text.Split("--")(0), strE, StrTipoPaquete)
                                            datFares.Tables(FaresData.FARES_TABLE).Rows.Add(rowFare)
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
            Next
            EditFaresByOcupation(datFares, datRestrictions, datRestrictionsDelete, idxModified, idx)
        End If


    End Function
    Private Function EditFaresByPackageOrPerson(ByRef datFares As FaresData, ByRef datRestrictions As FaresRestrictionsData, ByRef datRestrictionsDelete As FaresRestrictionsData, ByRef idxModified As String, ByVal idx As Double)
        With New FaresDataAccess
            Dim sqlcon As System.Data.SqlClient.SqlConnection
            Dim trans As System.Data.SqlClient.SqlTransaction
            trans = .BeginTransaction(sqlcon)

            If .UpdateFares(datFares, sqlcon, trans) Then
                'falta insertar sus tarifas restricciones
                For i As Integer = 0 To datFares.Tables(FaresData.FARES_TABLE).Rows.Count - 1
                    If idxModified.IndexOf("," & datFares.Tables(FaresData.FARES_TABLE).Rows(i)(FaresData.PKIDFARES_FIELD) & ",") < 0 Then
                        For idxAdults As Integer = 1 To Me.txtMaxAd.Text
                            For idxChild As Integer = 0 To Me.txtMaxChild.Text
                                'Combinaciond de adultos - niños
                                Dim newRow As DataRow
                                newRow = GetFaresRestrictionRow(datFares.Tables(FaresData.FARES_TABLE).Rows(i)(FaresData.PKIDFARES_FIELD), idx, datRestrictions, idxAdults, idxChild)
                                datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
                            Next
                        Next
                    End If
                Next

                If datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Count > 0 Then
                    Dim UpdateRestriction As New FaresRestrictionSystem

                    If UpdateRestriction.DeleteRestrictionsByIdRate(datRestrictionsDelete, sqlcon, trans) AndAlso UpdateRestriction.UpdateAndInsertRestrictions(datRestrictions, sqlcon, trans) Then
                        .commitTransaction(sqlcon, trans)
                    Else
                        .RollBackTransaction(sqlcon, trans)
                    End If
                Else
                    'no hay nada por actualizar
                    .commitTransaction(sqlcon, trans)
                End If
            Else
                .RollBackTransaction(sqlcon, trans)
            End If
        End With
    End Function
    Private Function EditFaresByOcupation(ByRef datFares As FaresData, ByRef datRestrictions As FaresRestrictionsData, ByRef datRestrictionsDelete As FaresRestrictionsData, ByRef idxModified As String, ByVal idx As Double)
        With New FaresDataAccess
            Dim sqlcon As System.Data.SqlClient.SqlConnection
            Dim trans As System.Data.SqlClient.SqlTransaction
            trans = .BeginTransaction(sqlcon)

            If .UpdateFares(datFares, sqlcon, trans) Then
                'falta insertar sus tarifas restricciones
                For i As Integer = 0 To datFares.Tables(FaresData.FARES_TABLE).Rows.Count - 1
                    If idxModified.IndexOf("," & datFares.Tables(FaresData.FARES_TABLE).Rows(i)(FaresData.PKIDFARES_FIELD) & ",") < 0 Then
                        'es un idtarifa nuevo
                        For Each drRoom As ListItem In Me.chkHabitaciones.Items
                            idx = -1
                            For Each str As String In Me.txtPrices.Text.Split("$")
                                idx += 1
                                If str <> "" Then

                                    Dim Auxroom As String = str
                                    Auxroom = str.Substring(0, Auxroom.ToString.IndexOf(","))
                                    If Auxroom = drRoom.Text.Split("--")(0) AndAlso drRoom.Value = datFares.Tables(FaresData.FARES_TABLE).Rows(i)(FaresData.HOTELROOMTYPEID_FIELD) Then
                                        'la fecha debe coincidir con el row agregado
                                        If str.Split(",")(1) <> "" Then
                                            Dim Pf1, Pf2 As Date
                                            Pf1 = CDate(str.Split(",")(1))
                                            Pf2 = CDate(str.Split(",")(2))
                                            If Pf1 = CDate(datFares.Tables(FaresData.FARES_TABLE).Rows(i)(FaresData.STARTDATE_FIELD)) And Pf2 = CDate(datFares.Tables(FaresData.FARES_TABLE).Rows(i)(FaresData.ENDDATE_FIELD)) Then
                                                For idxAdults As Integer = 1 To Me.txtMaxAd.Text
                                                    For idxChild As Integer = 0 To Me.txtMaxChild.Text
                                                        Dim newRow As DataRow
                                                        newRow = GetFaresRestrictionRow(datFares.Tables(FaresData.FARES_TABLE).Rows(i)(FaresData.PKIDFARES_FIELD), idx, datRestrictions, idxAdults, idxChild)
                                                        datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
                                                    Next
                                                Next
                                                Exit For
                                            End If
                                        End If

                                    End If
                                End If
                            Next
                        Next
                    End If
                Next

                If datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Count > 0 Then
                    Dim UpdateRestriction As New FaresRestrictionSystem

                    If UpdateRestriction.DeleteRestrictionsByIdRate(datRestrictionsDelete, sqlcon, trans) AndAlso UpdateRestriction.UpdateAndInsertRestrictions(datRestrictions, sqlcon, trans) Then
                        .commitTransaction(sqlcon, trans)
                    Else
                        .RollBackTransaction(sqlcon, trans)
                    End If
                Else
                    'no hay nada por actualizar
                    .commitTransaction(sqlcon, trans)
                End If
            Else
                .RollBackTransaction(sqlcon, trans)
            End If
        End With
    End Function
    Private Sub GetDgFares(ByRef datFares As FaresData, ByRef datRestrictions As FaresRestrictionsData, ByRef datRestrictionsDelete As FaresRestrictionsData, ByRef idxModified As String, ByVal idx As Double)
        Dim rowFare As DataRow
        For Each item As DataGridItem In Me.dgRooms.Items

            Dim chk As CheckBox = item.FindControl("chkDelete")
            If chk.Checked Then
                rowFare = AddrowFare(datFares, txtStart.Text, txtEnd.Text, item.Cells(dgcolumns.idtipohabitacion_hotel).Text, item.Cells(dgcolumns.codigohabitacion).Text, "NNNNNNN", "")
                rowFare.Item(FaresData.PKIDFARES_FIELD) = item.Cells(dgcolumns.IdTarifa).Text
                datFares.Tables(FaresData.FARES_TABLE).Rows.Add(rowFare)
                rowFare.AcceptChanges()
                rowFare.Delete()
            Else
                chk = item.FindControl("chkEditOcc")
                Dim newRow As DataRow
                If chk.Checked Then

                    Dim txtE As TextBox = item.FindControl("txtPriceEModified")
                    Dim txtTipoPaquete As TextBox = item.FindControl("txtTipoPrecioModified")

                    rowFare = AddrowFare(datFares, CDate(item.Cells(dgcolumns.FechaInicia).Text), CDate(item.Cells(dgcolumns.FechaFinaliza).Text), item.Cells(dgcolumns.idtipohabitacion_hotel).Text, item.Cells(dgcolumns.codigohabitacion).Text, txtE.Text.Split(",")(3), txtTipoPaquete.Text)
                    datFares.Tables(FaresData.FARES_TABLE).Rows.Add(rowFare)
                    rowFare.AcceptChanges()
                    rowFare.Item(FaresData.PKIDFARES_FIELD) = item.Cells(dgcolumns.IdTarifa).Text

                    For idxAdults As Integer = 1 To Me.txtMaxAd.Text
                        For idxChild As Integer = 0 To Me.txtMaxChild.Text
                            Dim txtS As TextBox = item.FindControl("txtPriceModified")

                            newRow = GetFaresRestrictionRow(item.Cells(dgcolumns.IdTarifa).Text, 1, datRestrictions, idxAdults, idxChild, txtS, txtE)
                            datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
                        Next
                    Next
                    newRow = GetFaresRestrictionRow(item.Cells(dgcolumns.IdTarifa).Text, -1, datRestrictionsDelete, 1, 0)
                    datRestrictionsDelete.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
                    newRow.AcceptChanges()
                    newRow.Delete()
                Else
                    If Me.Adultos <> txtMaxAd.Text Or Me.Ninios <> Me.txtMaxChild.Text Then
                        'o el precio cambió ó la ocupación
                        For idxAdults As Integer = 1 To Me.txtMaxAd.Text
                            For idxChild As Integer = 0 To Me.txtMaxChild.Text
                                Dim txtS As TextBox = item.FindControl("tPrices")
                                Dim txtE As TextBox = item.FindControl("tPricesE")
                                newRow = GetFaresRestrictionRow(item.Cells(dgcolumns.IdTarifa).Text, 1, datRestrictions, idxAdults, idxChild, txtS, txtE)
                                datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
                            Next
                        Next
                        newRow = GetFaresRestrictionRow(item.Cells(dgcolumns.IdTarifa).Text, -1, datRestrictionsDelete, 1, 0)
                        datRestrictionsDelete.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
                        newRow.AcceptChanges()
                        newRow.Delete()

                    End If

                End If

            End If
        Next
    End Sub
    Private Sub saveRoomData(ByVal idRoomType As Integer, ByVal roomCode As String, ByVal save As Boolean, ByVal start As String, ByVal _end As String, Optional ByVal idx As Double = -1)
        Dim dsFar As FaresData
        Dim dv As DataView
        With New FaresSystem
            dsFar = .GetFaresByRoomTypeId(idRoomType)
        End With
        dv = dsFar.Tables(0).DefaultView
        dv.RowFilter = "idrateplan='" & Me.IdRatePlan & "'"
        If save Then
            If dv.Count > 0 Then
                'UpdateFare(dv(0)(dsFar.PKIDFARES_FIELD), txtStart.Text, txtEnd.Text, dv(0)(dsFar.HOTELROOMTYPEID_FIELD))
            Else
                SaveNewFare(start, _end, idRoomType, roomCode, idx)
            End If
        Else
            If dv.Count > 0 Then
                '' borrar tarifa ''
                With New FaresSystem
                    .DeleteFares(dv(0)("idtarifa"))
                End With
            End If
        End If
    End Sub
    Private Function GetFaresRestrictionRow(ByVal idTarifa As Integer, ByVal Idx As Double, ByVal datrestrictions As FaresRestrictionsData, ByVal idxAdults As Integer, ByVal idxChild As Integer, ByVal iPrices As TextBox, ByVal iPricesE As TextBox) As DataRow
        Dim newRow As DataRow = datrestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).NewRow()
        With newRow
            Dim precio As Double
            Dim precioExc As Double = 0

            'el idx será de la posición del elemento en txtpriceç
            If Idx > -1 Then
                Dim str As String = iPrices.Text.Split("$")(Idx)
                'si existe el precio se lo ponemos sino tomamos el último precio
                If str.Split(",").Length > 2 + idxAdults Then
                    precio = str.Split(",")(2 + idxAdults)
                    'en caso contrario quedará el último precio válido
                Else
                    precio = str.Split(",")(str.Split(",").Length - 1)
                End If

                Dim strE As String = iPricesE.Text.Split("$")(Idx)
                If strE.Split(",").Length > 3 + idxAdults Then
                    precioExc = CDbl(strE.Split(",")(3 + idxAdults))
                    'en caso contrario quedará el último precio válido
                Else
                    precioExc = CDbl(strE.Split(",")(strE.Split(",").Length - 1))
                End If
                'End If
            Else
                'aqui no deberia de entrar
                precio = 0
            End If

            .Item(FaresRestrictionsData.ADULTFARE_FIELD) = precio
            .Item(FaresRestrictionsData.ADULTNUMBER_FIELD) = idxAdults
            .Item(FaresRestrictionsData.CHILDFARE_FIELD) = 0
            .Item(FaresRestrictionsData.EXCNINIOFARE_FIELD) = 0
            .Item(FaresRestrictionsData.EXCADULTFARE_FIELD) = precioExc
            .Item(FaresRestrictionsData.CHILDNUMBER_FIELD) = idxChild
            .Item(FaresRestrictionsData.IDFARE_FIELD) = idTarifa
            .Item(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = 0
        End With
        Return newRow
    End Function
    Private Function GetFaresRestrictionRow(ByVal idTarifa As Integer, ByVal Idx As Double, ByVal datrestrictions As FaresRestrictionsData, ByVal idxAdults As Integer, ByVal idxChild As Integer) As DataRow
        Return GetFaresRestrictionRow(idTarifa, Idx, datrestrictions, idxAdults, idxChild, Me.txtPrices, Me.txtPricesE)
    End Function

    Private Function SaveFaresRestrictions(ByVal idTarifa As Integer, ByVal Idx As Double) As Boolean
        Dim datRestrictions As New FaresRestrictionsData
        For idxAdults As Integer = 1 To Me.txtMaxAd.Text
            For idxChild As Integer = 0 To Me.txtMaxChild.Text
                'Combinaciond de adultos - niños
                Dim newRow As DataRow
                newRow = GetFaresRestrictionRow(idTarifa, Idx, datRestrictions, idxAdults, idxChild)
                datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
            Next
        Next

        With New FaresRestrictionSystem
            SaveFaresRestrictions = .InsertFaresRestrictions(datRestrictions)
        End With
    End Function

    Private Function AddrowFare(ByRef datfare As FaresData, ByVal f1 As Date, ByVal f2 As Date, ByVal idRoom As Integer, ByVal roomcode As String, ByVal Exception As String, ByVal tipoprecio As String) As DataRow
        Dim rowFare As DataRow
        With datfare.Tables(FaresData.FARES_TABLE)
            rowFare = .NewRow()
            rowFare(FaresData.ENDDATE_FIELD) = f2
            rowFare(FaresData.EXTRAADULTPRICE_FIELD) = 0
            rowFare(FaresData.EXTRACHILDPRICE_FIELD) = 0

            If tipoprecio <> "" Then
                rowFare(FaresData.PRICE_FIELD) = tipoprecio.Split(",")(4)
                rowFare(FaresData.PACKAGETYPE) = tipoprecio.Split(",")(3)
            End If

            rowFare(FaresData.NINIORATE) = 0
            rowFare(FaresData.STARTDATE_FIELD) = f1

            rowFare(FaresData.EXCEPTION_FIELD) = Exception

            rowFare(FaresData.RULESDEFAULT) = True
            rowFare(FaresData.NOARRIVOS_FIELD) = "NNNNNNN"
            rowFare(FaresData.HOTELROOMTYPEID_FIELD) = idRoom
            rowFare(FaresData.RATETYPE_FIELD) = "K"
            If Me.edicion = False Then
                rowFare(FaresData.IDRATEPLAN_FIELD) = txtRateCode.Text
            Else
                rowFare(FaresData.IDRATEPLAN_FIELD) = Me.IdRatePlan
            End If

            rowFare(FaresData.RATECODE_FIELD) = roomcode & txtRateCode.Text

        End With
        Return rowFare

    End Function

    '''''''''  para guardar las tarifas que se ocupan '''''''''''''''''''' en las fechas del plan '''''''''''''''''''''''''''''''''''''''''
    Public Function SaveNewFare(ByVal f1 As Date, ByVal f2 As Date, ByVal idRoom As Integer, ByVal roomcode As String, ByVal idx As Double) As Boolean
        Dim datFare As New FaresData
        Dim idtar As Integer
        Dim strE As String = "NNNNNNN"
        If idx > -1 Then
            strE = Me.txtPricesE.Text.Split("$")(idx)
            strE = strE.Split(",")(3)

        End If
        Dim rowFare As DataRow
        Dim StrTipoPaquete As String = ""
        For Each strTipoPrecio As String In itxtTipoPrecio.Value.Split("$")
            If strTipoPrecio <> "" Then
                If strTipoPrecio.Split(",")(0) = roomcode AndAlso CDate(strTipoPrecio.Split(",")(1)) = f1 AndAlso CDate(strTipoPrecio.Split(",")(2)) = f2 Then
                    StrTipoPaquete = strTipoPrecio
                    Exit For
                End If
            End If
        Next

        rowFare = AddrowFare(datFare, f1, f2, idRoom, roomcode, strE, StrTipoPaquete)
        datFare.Tables(FaresData.FARES_TABLE).Rows.Add(rowFare)
        Try
            With New FaresSystem
                Try
                    If .InsertFares(datFare) Then
                        idtar = datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.PKIDFARES_FIELD)
                        'esta condición es para cuando se autollenaran las tarifasrestricciones
                        If Not SaveFaresRestrictions(idtar, idx) Then Return False

                    End If
                Catch ex As OverflowException
                    'Me.lblDateError.Visible = True
                    Return False
                End Try
            End With

        Catch ex As Exception
            Return False
        End Try

        ' End If

        Return True

    End Function

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
        Response.Write("<script> var updateSeasson='" & PortalCulture.GetString("00609") & "';</script>")
        lstDates.Items.Clear()
        Me.lstDates.Items.Add(PortalCulture.GetString("00317", True))
        For i As Integer = 1 To lstDatesCount()
            Dim it As String = lstDatesItemI(i)
            Me.lstDates.Items.Add(it)
        Next
        For i As Integer = 1 To Me.txtPrices.Text.Split("$").Length() - 1
            Dim str As String = Me.txtPrices.Text.Split("$").GetValue(i)
            If str.Split(",")(1) = "" Then
                Dim Auxroom As String = str
                Dim it As String = str.Substring(0, Auxroom.ToString.IndexOf(","))
                Me.lstDates.Items.Add(it)
            End If

        Next
    End Sub

    Private Sub loadResources()
        lblPortal.Text = PortalCulture.GetString("01009")
        lblDescripcion.Text = PortalCulture.GetString("00002", True)

        lblRatecode.Text = PortalCulture.GetString("00594", True)

        Me.lblRule.Text = PortalCulture.GetString("00298", True)
        Me.ddlRules.Items(0).Text = PortalCulture.GetString("00027")
        lblname.Text = PortalCulture.GetString("00073", True)
        lblRateApply.Text = PortalCulture.GetString("00323", True)
        chkUnipantalla.Text = PortalCulture.GetString("00457")
        Me.lblNoches.Text = PortalCulture.GetString("M000626", True)
        Me.RVNochesgratis.ErrorMessage = PortalCulture.GetString("00561")

        lblStartDate.Text = PortalCulture.GetString("M000442", True)
        lblEndDate.Text = PortalCulture.GetString("M000443", True)

        lblPrice.Text = PortalCulture.GetString("00090", True)

        lblMaxAdultos.Text = PortalCulture.GetString("M0UT00480", True)
        lblMaxChildren.Text = PortalCulture.GetString("M0UT00479", True)

        rfvCodePack.ErrorMessage = PortalCulture.GetString("00071")
        rfvMaxAdultos.ErrorMessage = PortalCulture.GetString("00071")
        rfvMaxChild.ErrorMessage = PortalCulture.GetString("00071")
        rfvNoches.ErrorMessage = PortalCulture.GetString("00071")


        ddlTypePrice.Items(0).Text = PortalCulture.GetString("00660")
        ddlTypePrice.Items(1).Text = "Total"

        lblImagen.Text = PortalCulture.GetString("00080")
        Dim dsHotel As HotelDatos
        With New HotelSistema
            dsHotel = .GetHotelById(CType(Me.Page, PaginaBase).cInfoActual.Hotel)
        End With

        Dim strIncTax As String

        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 Then
            lblmoney.Text = PortalCulture.GetString("M000263") & " " & dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0)("Codigo")
            strIncTax = " " & PortalCulture.GetString("00610") & " "

            If Not dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).IsNull(HotelDatos.FIELD_PLUSTAX) Then
                If dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0)(HotelDatos.FIELD_PLUSTAX) Then
                    strIncTax = " " & PortalCulture.GetString("00611") & " "
                End If
            End If

            If dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0)("Codigo") = "MXN" Then
                strIncTax &= "<BR> " & PortalCulture.GetString("00667")
            End If

            lblmoney.Text &= strIncTax
        End If
        Me.RdbPaquete.Text = PortalCulture.GetString("01020")
        Me.RdbPersona.Text = PortalCulture.GetString("01021")
        Me.RdbOcupacion.Text = PortalCulture.GetString("01022")
        Me.lblAplicaGDS.Text = PortalCulture.GetString("01023")
        lblSeasons.InnerHtml = PortalCulture.GetString("01025")
        lblOHasta.Text = PortalCulture.GetString("00109", True)
        lblODesde.Text = PortalCulture.GetString("00108", True)
        btnAddOccRate.Value = PortalCulture.GetString("01026")
        Me.btnCancelOccRate.Value = PortalCulture.GetString("A00143")
        Me.lblPricingNE.Text = PortalCulture.GetString("M000391")
        Me.lblPricingExc.Text = PortalCulture.GetString("M000402")
        Me.chk1.Text = PortalCulture.GetString("00300")
        Me.chk2.Text = PortalCulture.GetString("00301")
        Me.chk3.Text = PortalCulture.GetString("00302")
        Me.chk4.Text = PortalCulture.GetString("00303")
        Me.chk5.Text = PortalCulture.GetString("00304")
        Me.chk6.Text = PortalCulture.GetString("00305")
        Me.chk7.Text = PortalCulture.GetString("00306")
        Me.ADetails.InnerHtml = PortalCulture.GetString("01036")
        Me.lblRoomsNames.Text = PortalCulture.GetString("A00041")
        Me.aPaqueteRubro.InnerHtml = PortalCulture.GetString("01037")

        'TODO VA AL CONTROL RUBROS

    End Sub

    Public Sub loadRatePlan(ByVal id As String, ByVal Principal As Boolean)
        Dim dsRatePlan As RatePlanData
        With New RatePlanFacade
            dsRatePlan = .GetDataRatePlan(id, CType(Me.Page, PaginaBase).cInfoActual.Hotel)
        End With

        If dsRatePlan.Tables(RatePlanData.RATEPLAN_TABLE).Rows.Count > 0 Then
            With dsRatePlan.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0)

                If Not .IsNull(RatePlanData.FIELD_IDDICDESC) Then
                    idDicc = .Item(RatePlanData.FIELD_IDDICDESC)
                Else
                    idDicc = 0
                End If

                If Not .IsNull(RatePlanData.FIELD_IDDICSHORTDESC) Then
                    idShortDesc = .Item(RatePlanData.FIELD_IDDICSHORTDESC)
                Else
                    idShortDesc = 0
                End If

                txtDescripcion.CargaDatos(idDicc)
                Me.txtShortDescription.CargaDatos(idShortDesc)
                Me.txtDescripcion.textodefault = .Item(RatePlanData.FIELD_DESCRIPTION).ToString
                Me.txtShortDescription.CargaDatos(idShortDesc)
                Me.txtShortDescription.textodefault = .Item(RatePlanData.FIELD_NAME).ToString

                Me.chkGDS.Checked = False
                Me.chkPortal.Checked = False
                Me.chkUnipantalla.Checked = False
                chkADS.Checked = False
                If Not .IsNull(RatePlanData.FIELD_GDS) Then
                    Me.chkGDS.Checked = .Item(RatePlanData.FIELD_GDS)
                End If
                If Not .IsNull(RatePlanData.FIELD_PORTAL) Then
                    Me.chkPortal.Checked = .Item(RatePlanData.FIELD_PORTAL)
                End If
                If Not .IsNull(RatePlanData.FIELD_UNIPANTALLA) Then
                    Me.chkUnipantalla.Checked = .Item(RatePlanData.FIELD_UNIPANTALLA)
                End If
                If Not .IsNull(RatePlanData.FIELD_ADS) Then
                    Me.chkADS.Checked = .Item(RatePlanData.FIELD_ADS)
                End If
                'si todos son null por default ponemos el de gds
                If .IsNull(RatePlanData.FIELD_GDS) AndAlso .IsNull(RatePlanData.FIELD_PORTAL) AndAlso .IsNull(RatePlanData.FIELD_UNIPANTALLA) AndAlso .IsNull(RatePlanData.FIELD_ADS) Then
                    Me.chkGDS.Checked = True
                End If



                Me.txtRateCode.Text = .Item(RatePlanData.FIELD_CODIGOTARIFA).ToString
                Try
                    ddlRules.SelectedValue = .Item(RatePlanData.FIELD_IDRULE)
                Catch ex As Exception
                    ddlRules.SelectedValue = 0
                End Try

                Try
                    If Not .IsNull(RatePlanData.FIELD_GDSAPPLY) Then
                        Dim dato As String = .Item(RatePlanData.FIELD_GDSAPPLY)
                        dato = dato.ToUpper

                        If dato.Chars(0) = "N" Then
                            Me.chkGDSAmadeus.Checked = False
                        End If

                        If dato.Chars(1) = "N" Then
                            Me.chkGDSGalileo.Checked = False
                        End If

                        If dato.Chars(2) = "N" Then
                            Me.chkGDSSabre.Checked = False
                        End If

                        If dato.Chars(3) = "N" Then
                            Me.chkGDSWorldSpan.Checked = False
                        End If
                    End If
                Catch ex As Exception
                End Try

            End With

            Dim dsPack As PackageData
            With New PackageDataAccess

                dsPack = .LoadPackageByID(idRateCode, CType(Me.Page, PaginaBase).cInfoActual.Hotel)
            End With

            If Not dsPack Is Nothing AndAlso dsPack.Tables(PackageData.Package_TABLE).Rows.Count > 0 Then
                ImagenHabitacion.Visible = IMG_Exists(dsPack.Tables(PackageData.Package_TABLE).Rows(0).Item(PackageData.FIELD_RateCode), ImagenHabitacion)
                With dsPack.Tables(PackageData.Package_TABLE).Rows(0)
                    txtEnd.Text = CDate(.Item(PackageData.FIELD_enddate)).ToString("MM/dd/yyyy")
                    txtStart.Text = CDate(.Item(PackageData.FIELD_startdate)).ToString("MM/dd/yyyy")
                    ddlTypePrice.SelectedIndex = .Item(PackageData.FIELD_TypePrice)

                    txtPrecio.Text = CDbl(.Item(PackageData.FIELD_Price)).ToString("#0.00") 'txtRateCode.Text
                    txtMaxAd.Text = .Item(PackageData.FIELD_MaxAdultos)
                    txtMaxChild.Text = .Item(PackageData.FIELD_MaxChildren)
                    Me.txtDaysFree.Text = "" & .Item(PackageData.FIELD_Nights)
                    Me.RdbPaquete.Checked = True
                    Me.RdbPersona.Checked = False
                    Me.RdbOcupacion.Checked = False
                    Me.TipoPrecio = .Item(PackageData.FIELD_TypePrice)
                    Me.Adultos = txtMaxAd.Text
                    Me.Ninios = Me.txtMaxChild.Text
                    Me.PackageRubros.LoadData(.Item(PackageData.FIELD_IdPaquete))
                End With

            End If

            ctrPortal1.LoadPortales(CType(Me.Page, PaginaBase).cInfoActual.Hotel, 2, IdRatePlan)
            Dim ci As System.Globalization.CultureInfo
            ci = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
            Me.dgRooms.DataSource = GetRoomFares()
            Me.dgRooms.DataBind()
            System.Threading.Thread.CurrentThread.CurrentCulture = ci


        End If
    End Sub
    Private Function GetRoomFares() As FaresData
        With New FaresRestrictionSystem
            faresrest = .GetFaresRestriccionsByPackage(IdRatePlan, CType(Me.Page, PaginaBase).cInfoActual.Hotel)
        End With

        'Dim datFares As FaresData
        With New FaresSystem
            Return .GetTarifasByRatePlan(CType(Me.Page, PaginaBase).cInfoActual.Hotel, IdRatePlan)
        End With



    End Function

    Public Sub ClearData()
        txtDescripcion.Limpia()
        edicion = False
        Me.txtShortDescription.Limpia()
        Me.chkPortal.Checked = False
        Me.chkUnipantalla.Checked = False
        Me.chkADS.Checked = False
        txtDaysFree.Text = ""

        txtEnd.Text = ""
        txtStart.Text = ""
        txtMaxAd.Text = ""
        txtMaxChild.Text = ""
        txtPrecio.Text = ""

        txtDaysFree.Text = 1
        Me.IdRatePlan = ""
        Me.idRateCode = ""
        Me.ImagenHabitacion.ImageUrl = ""

        Me.ImagenHabitacion.Visible = False
        Me.lblPrice.Visible = True
        Me.txtPrecio.Visible = True
        Me.ddlTypePrice.Visible = True
        Me.lblmoney.Visible = True
        cargarDatosHotel()
        ctrPortal1.Limpiar()
        txtRateCode.Text = ""
        lstDates.Items.Clear()
        Me.lstDates.Items.Add(PortalCulture.GetString("00317", True))
        Me.txtFechas.Text = ""
        txtPrices.Text = ""
        txtPricesE.Text = ""
        itxtTipoPrecio.Value = ""
        Me.RdbPaquete.Checked = True
        Me.RdbOcupacion.Checked = False
        Me.RdbPersona.Checked = False
        Me.dgRooms.DataSource = Nothing
        Me.dgRooms.DataBind()
        For i As Integer = 1 To 7
            Dim chk As CheckBox = Me.FindControl("chk" & i.ToString)
            If Not chk Is Nothing Then chk.Checked = False
        Next
        If chkGDS.Checked Then
            Me.chkGDSAmadeus.Checked = True
            Me.chkGDSGalileo.Checked = True
            Me.chkGDSSabre.Checked = True
            Me.chkGDSWorldSpan.Checked = True
        End If

    End Sub


    Public Function eliminarPortales(ByVal IdHotel As Integer, ByVal codigo As String) As Boolean
        ctrPortal1.EliminarPortales(IdHotel, 2, codigo)
    End Function

    Private Sub loadrooms()
        Dim Rooms As New Portal.Hotel.Common.Data.RoomsHotelData
        ' Dim RatesPlan As Portal.General.Common.Data.RatePlanData

        With New Portal.Hotel.Facade.RoomFacade
            Rooms = .getRooms(CType(Me.Page, PaginaBase).cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With

        Dim links As New LinkRoomTypeData
        With New LinkRoomsFacade
            links = .getList(CType(Me.Page, PaginaBase).cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With

        Dim dv As DataView
        For Each r As DataRow In Rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows
            dv = links.Tables(LinkRoomTypeData.TABLE_LINKROOM).DefaultView
            dv.RowFilter = LinkRoomTypeData.FIELD_TargetRoom & "=" & r(RoomsHotelData.FLD_ID_ROOM_HOTEL)
            If dv.Count > 0 Then
                r.Delete()
            End If
        Next
        Rooms.AcceptChanges()

        chkHabitaciones.DataTextField = RoomsHotelData.FLD_ROOM_CODE
        chkHabitaciones.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
        chkHabitaciones.DataSource = Rooms
        chkHabitaciones.DataBind()
        For Each dr As DataRow In Rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows
            dvRooms.InnerHtml &= dr(RoomsHotelData.FLD_ROOM_CODE) & "-" & dr(RoomsHotelData.FLD_NOMBRE) & "<br/>"
        Next




    End Sub

    Public Function IsValidRoom() As Boolean
        Dim RoomsValids As String = ","
        Dim dr As DataRow
        Dim RoomsValided As String = ","

        For Each drRoom As ListItem In Me.chkHabitaciones.Items
            For i As Integer = 0 To Me.lstDatesCount
                If Me.lstDatesItemI(i).Split(":").Length > 1 AndAlso Me.lstDatesItemI(i).Split(":")(1).ToUpper = drRoom.Text.Split("--")(0).ToUpper Then
                    If RoomsValided.IndexOf("," & drRoom.Value & ",") < 0 Then
                        With New Portal.Hotel.Facade.RoomFacade
                            dsRooms = .getRoomByID(drRoom.Value)
                            If Not dsRooms Is Nothing AndAlso dsRooms.Tables(0).Rows.Count > 0 Then
                                RoomsValided = RoomsValided & drRoom.Value & ","
                                dr = dsRooms.Tables(0).Rows(0)
                                If Not allowOcupation(dr, drRoom.Text) Then
                                    Return False
                                End If
                            End If
                        End With
                        Exit For
                    End If
                End If
            Next
        Next
        'aparte validar
        If Me.edicion Then
            'comparar tambien los del datagrid por si cambió la ocupación de una vez podemos comparar los overlapped
            If Not Me.ValidDgRooms(RoomsValids) Then
                Return False
            End If
        End If

        Return True
    End Function
    Public Function ValidDgRooms(ByRef RoomsValids As String) As Boolean
        Dim dr As DataRow
        Dim tmpChk As CheckBox
        For Each item As DataGridItem In Me.dgRooms.Items
            Dim chk As CheckBox = item.FindControl("chkDelete")
            If Not chk Is Nothing AndAlso Not chk.Checked Then
                If RoomsValids.IndexOf("," & item.Cells(dgcolumns.idtipohabitacion_hotel).Text & ",") < 0 Then
                    With New Portal.Hotel.Facade.RoomFacade
                        dsRooms = .getRoomByID(item.Cells(dgcolumns.idtipohabitacion_hotel).Text)
                        RoomsValids &= item.Cells(dgcolumns.idtipohabitacion_hotel).Text & ","
                        dr = dsRooms.Tables(0).Rows(0)
                        If Not allowOcupation(dr, item.Cells(dgcolumns.codigohabitacion).Text) Then
                            Return False
                        End If
                    End With
                End If
                Dim Start As Date = CDate(item.Cells(dgcolumns.FechaInicia).Text)
                Dim _End As Date = CDate(item.Cells(dgcolumns.FechaFinaliza).Text)
                Dim newStart As Date
                Dim newEnd As Date
                For i As Integer = 0 To Me.lstDatesCount
                    If Me.lstDatesItemI(i).Split(":").Length > 1 AndAlso Me.lstDatesItemI(i).Split(":")(1).ToUpper = item.Cells(dgcolumns.codigohabitacion).Text.ToUpper Then
                        newStart = CDate(lstDatesItemI(i).Split("-")(0))
                        Dim str As String = lstDatesItemI(i).Split("-")(1)
                        If str.ToString.IndexOf(":") > 0 Then
                            str = str.Substring(0, str.IndexOf(":"))
                        End If
                        newEnd = CDate(str)
                        If (((newStart >= Start And newStart <= _End) Or ((newEnd >= Start) And newEnd <= _End)) Or ((Start >= newStart And Start <= newEnd) Or ((_End >= newStart) And _End <= newEnd))) Then
                            descripcionError = PortalCulture.GetString("00514")  '  "se sobrelapan las tarifas"
                            Return False
                        End If
                    End If
                Next
                'End If
                For i As Integer = 0 To Me.dgRooms.Items.Count - 1
                    Dim NewItem As DataGridItem = Me.dgRooms.Items.Item(i)
                    tmpChk = NewItem.FindControl("chkDelete")
                    If (NewItem.ItemType = ListItemType.Item Or NewItem.ItemType = ListItemType.AlternatingItem) And Not tmpChk.Checked Then
                        If NewItem.ItemIndex <> item.ItemIndex Then
                            If item.Cells(dgcolumns.idtipohabitacion_hotel).Text = NewItem.Cells(dgcolumns.idtipohabitacion_hotel).Text Then
                                newStart = CDate(NewItem.Cells(dgcolumns.FechaInicia).Text)
                                newEnd = CDate(NewItem.Cells(dgcolumns.FechaFinaliza).Text)
                                If (((newStart >= Start And newStart <= _End) Or ((newEnd >= Start) And newEnd <= _End)) Or ((Start >= newStart And Start <= newEnd) Or ((_End >= newStart) And _End <= newEnd))) Then
                                    descripcionError = PortalCulture.GetString("00514") ' "se sobrelapan las tarifas"
                                    Return False
                                End If
                            End If
                        End If
                    End If
                Next
            End If
        Next
        Return True
    End Function
    Private Function allowOcupation(ByVal dr As DataRow, ByVal hab As String) As Boolean
        If CInt(txtMaxAd.Text) > dr(RoomsHotelData.FLD_NUMBER_MAXADULTS) OrElse _
                                               CInt(txtMaxChild.Text) > dr(RoomsHotelData.FLD_NUMBER_MAXCHILDREN) OrElse _
                                               CInt(txtMaxChild.Text) + CInt(txtMaxAd.Text) > dr(RoomsHotelData.FLD_NUMBER_PEOPLESINROOM) Then
            cvMaxPeople.ErrorMessage = PortalCulture.GetString("00657")
            cvMaxPeople.ErrorMessage = String.Format(cvMaxPeople.ErrorMessage, hab, dr(RoomsHotelData.FLD_NUMBER_MAXADULTS), _
                dr(RoomsHotelData.FLD_NUMBER_MAXCHILDREN), dr(RoomsHotelData.FLD_NUMBER_PEOPLESINROOM))
            cvMaxPeople.IsValid = False
            Return False
        End If
        Return True
    End Function

    Private Sub dgRooms_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRooms.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dgRooms.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgRooms.CurrentPageIndex < dgRooms.PageCount - 1 Then
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


    Private Sub dgRooms_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRooms.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then

        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim _d As Date = CDate(e.Item.Cells(dgcolumns.FechaInicia).Text)
            e.Item.Cells(dgcolumns.FechaInicia).Text = _d.ToString("MM/dd/yyyy")
            _d = CDate(e.Item.Cells(dgcolumns.FechaFinaliza).Text)
            e.Item.Cells(dgcolumns.FechaFinaliza).Text = _d.ToString("MM/dd/yyyy")
            Dim str As String = "", strE As String = ""
            Dim dv As DataView = faresrest.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).DefaultView
            dv.RowFilter = FaresRestrictionsData.IDFARE_FIELD & "=" & e.Item.Cells(dgcolumns.IdTarifa).Text
            dv.Sort = "adultos asc"
            For i As Integer = 0 To dv.Count - 1
                str &= "," & dv(i)(FaresRestrictionsData.ADULTFARE_FIELD)
                strE &= "," & dv(i)(FaresRestrictionsData.EXCADULTFARE_FIELD)
            Next

            Dim input As TextBox, input2 As TextBox, TipoPaquete As TextBox, TipoPaqueteM As TextBox
            input = e.Item.FindControl("tPrices")
            input.Text = "$" & e.Item.Cells(dgcolumns.codigohabitacion).Text & ",," & str
            input = e.Item.FindControl("tPricesE")
            input.Text = "$" & e.Item.Cells(dgcolumns.codigohabitacion).Text & ",,," & e.Item.Cells(dgcolumns.Excepciones).Text & strE

            input = e.Item.FindControl("txtPriceModified")
            input.Text = "$" & e.Item.Cells(dgcolumns.codigohabitacion).Text & ",," & str
            input2 = e.Item.FindControl("txtPriceEModified")
            input2.Text = "$" & e.Item.Cells(dgcolumns.codigohabitacion).Text & ",,," & e.Item.Cells(dgcolumns.Excepciones).Text & strE

            TipoPaquete = e.Item.FindControl("txtTipoPrecio")
            TipoPaqueteM = e.Item.FindControl("txtTipoPrecioModified")

            Dim chk As CheckBox
            chk = e.Item.FindControl("chkEditOcc")
            chk.Attributes.Add("onclick", "javascript:EditDgOcupancyRate('" & input.ClientID & "','" & input2.ClientID & "','" & Me.chk1.ClientID & "','" & Me.chk2.ClientID & "','" & Me.chk3.ClientID & "','" & Me.chk4.ClientID & "','" & Me.chk5.ClientID & "','" & Me.chk6.ClientID & "','" & Me.chk7.ClientID & "','" & chk.ClientID & "','" & e.Item.Cells(dgcolumns.codigohabitacion).Text & "','" & btnCancelOccRate.ClientID & "','" & TipoPaquete.ClientID & "','" & TipoPaqueteM.ClientID & "','" & btnAddOccRate.ClientID & "','" & chkHabitaciones.ClientID & "');")
            If e.Item.Cells(dgcolumns.TarifaAdulto).Text <> "&nbsp;" Then
                e.Item.Cells(dgcolumns.TarifaAdulto).Text = FCurrency(e.Item.Cells(dgcolumns.TarifaAdulto).Text * CDbl(Me.txtDaysFree.Text), 2)
            End If


            str = "N2"
            If e.Item.Cells(dgcolumns.packageType).Text <> "&nbsp;" Then
                str = e.Item.Cells(dgcolumns.packageType).Text
            End If
            If e.Item.Cells(dgcolumns.Precio).Text <> "&nbsp;" Then
                str = str & "," + e.Item.Cells(dgcolumns.Precio).Text
            End If

            TipoPaquete.Text = "$" & e.Item.Cells(dgcolumns.codigohabitacion).Text & ",,," & str

            TipoPaqueteM.Text = "$" & e.Item.Cells(dgcolumns.codigohabitacion).Text & ",,," & str


        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.codigohabitacion).Text = PortalCulture.GetString("00170")
            e.Item.Cells(dgcolumns.FechaInicia).Text = PortalCulture.GetString("M000230")
            e.Item.Cells(dgcolumns.FechaFinaliza).Text = PortalCulture.GetString("M000231")
            e.Item.Cells(dgcolumns.EditarTarifas).Text = PortalCulture.GetString("01028")
            e.Item.Cells(dgcolumns.Eliminar).Text = PortalCulture.GetString("01029")
            e.Item.Cells(dgcolumns.TarifaAdulto).Text = String.Format(PortalCulture.GetString("01035"), txtMaxAd.Text)
        End If


    End Sub

    Public Function LoadVariables() As String
        Dim str As New System.Text.StringBuilder
        str.Append("<script>")
        str.Append("var TitleEditing='" & PortalCulture.GetString("01019") & "';")
        str.Append("var NowYear='" & Now.Year & "';")
        str.Append("var InvalidAdRate='" & PortalCulture.GetString("01030") & "';")
        str.Append("var InvalidAdExcRate='" & PortalCulture.GetString("01031") & "';")
        str.Append("var InvalidDate='" & PortalCulture.GetString("00110") & "';")
        str.Append("var SpecifyRoom='" & PortalCulture.GetString("01032") & "';")
        str.Append("var Overlapped='" & PortalCulture.GetString("01033") & "';")
        str.Append("var UncheckRoom='" & PortalCulture.GetString("01034") & "';")
        str.Append("var rdbOccId='" & Me.RdbOcupacion.ClientID & "';")
        str.Append("var rdbPaqId='" & Me.RdbPaquete.ClientID & "';")
        str.Append("var rdbPerId='" & Me.RdbPersona.ClientID & "';")
        str.Append("var txtPrecioId='" & txtPrecio.ClientID & "';")
        str.Append("var ddlTypePrecioId='" & ddlTypePrice.ClientID & "';")
        str.Append("var txtMaxAdId='" & txtMaxAd.ClientID & "';")
        str.Append("var txtNightsId='" & txtDaysFree.ClientID & "';")
        str.Append("var txtPriceId='" & txtPrices.ClientID & "';")
        str.Append("var txtPriceEId='" & txtPricesE.ClientID & "';")
        str.Append("var lstDatesId='" & Me.lstDates.ClientID & "';")
        str.Append("var dgRoomsId='" & dgRooms.ClientID & "';")
        str.Append("var txtTipoPrecioId='" & itxtTipoPrecio.ClientID & "';")

        str.Append("</script>")

        Return str.ToString
    End Function

    Private Function GetLowestRate() As Double
        Dim minRate As Double = Double.MaxValue
        Try
            For Each item As DataGridItem In Me.dgRooms.Items
                Dim chk As CheckBox = item.FindControl("chkDelete")
                If Not chk.Checked Then
                    chk = item.FindControl("chkEditOcc")
                    Dim txtS As TextBox = item.FindControl("tPrices")
                    If chk.Checked Then
                        txtS = item.FindControl("txtPriceModified")
                    End If
                    Dim str As String = txtS.Text.Split("$")(1)

                    If str.Split(",").Length > 2 + 1 Then
                        If str.Split(",")(2 + 1) < minRate Then
                            minRate = CDbl(str.Split(",")(2 + 1))
                        End If
                    End If
                End If
            Next
            For Each str As String In Me.txtPrices.Text.Split("$")
                If str <> "" Then
                    If str.Split(",").Length > 2 + 1 Then
                        If str.Split(",")(2 + 1) < minRate Then
                            minRate = CDbl(str.Split(",")(2 + 1))
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
        End Try

        If minRate <> Double.MaxValue Then Return minRate
        Return 0
    End Function
End Class
