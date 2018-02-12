Imports System.Configuration.ConfigurationManager
Imports Portal.Hotel.Common.Data
Imports Portal.General.Facade
Imports System.Data.SqlClient
Partial Class Home
    Inherits PaginaBase


#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents ctrmenu1 As CtrMenu
    Protected WithEvents HyperLink9 As System.Web.UI.WebControls.HyperLink
    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        redirectTo(PaginaBase.pages.Welcome)
        'Introducir aquí el código de usuario para inicializar la página
        hplSoldOut.NavigateUrl = UrlPage(PaginaBase.pages.RatePlanInventory)
        hplPolicies.NavigateUrl = UrlPage(PaginaBase.pages.Policies)
        hplFood.NavigateUrl = UrlPage(PaginaBase.pages.FoodInformation)
        hplRoomAmenities.NavigateUrl = UrlPage(PaginaBase.pages.RoomAmenities)
        hplRooms.NavigateUrl = UrlPage(PaginaBase.pages.RoomsInformation)
        hplMapLocation.NavigateUrl = UrlPage(PaginaBase.pages.MapLocation)
        hplphotos.NavigateUrl = UrlPage(PaginaBase.pages.Photo)
        hplChangeLogo.NavigateUrl = UrlPage(PaginaBase.pages.ChangeLogo)
        hplRoomTypes.NavigateUrl = UrlPage(PaginaBase.pages.RoomType)
        hplInfGral.NavigateUrl = UrlPage(PaginaBase.pages.InfoHotel)
        hplListado.NavigateUrl = UrlPage(PaginaBase.pages.ReservationList)
        hplConfirmRes.NavigateUrl = UrlPage(PaginaBase.pages.ConfirmReservas)
        HplNoConfRes.NavigateUrl = UrlPage(PaginaBase.pages.NoConfirmReservas)
        hplByChanel.NavigateUrl = UrlPage(PaginaBase.pages.ResByChanel)
        hplBooking.NavigateUrl = UrlPage(PaginaBase.pages.Booking)
        hplChangePassword.NavigateUrl = UrlPage(PaginaBase.pages.ChangePassword)
        hplCreditCard.NavigateUrl = UrlPage(PaginaBase.pages.tarjetas)
        hplLog.NavigateUrl = UrlPage(PaginaBase.pages.Logs)
        lnkSearch.NavigateUrl = UrlPage(PaginaBase.pages.SearchHotel)
        lnkUsuarios.NavigateUrl = UrlPage(PaginaBase.pages.Users)
        lnkRooms.NavigateUrl = UrlPage(PaginaBase.pages.Rooms)
        lnkRoomslinks.NavigateUrl = UrlPage(PaginaBase.pages.linkroom)
        lnkRateplanRules.NavigateUrl = UrlPage(PaginaBase.pages.rateplanrules)
        lnkRatePlans.NavigateUrl = UrlPage(PaginaBase.pages.rateplans)
        lnkRatePlanLinks.NavigateUrl = UrlPage(PaginaBase.pages.rateplanlinks)
        lnkfaresCatalogue.NavigateUrl = UrlPage(PaginaBase.pages.FaresCatalogue)
        lnkAtraction.NavigateUrl = UrlPage(PaginaBase.pages.AreaAtraction)
        lnkActivities.NavigateUrl = UrlPage(PaginaBase.pages.Activities)
        lnkInfAmenities.NavigateUrl = UrlPage(PaginaBase.pages.InfAmenities)
        lnkInfProp.NavigateUrl = UrlPage(PaginaBase.pages.InfProp)
        lnkNoDefinedRates.NavigateUrl = UrlPage(PaginaBase.pages.NoRatesReport)
        lnkRatesChart.NavigateUrl = UrlPage(PaginaBase.pages.ratechart)
        lnkHomeStatus.NavigateUrl = UrlPage(PaginaBase.pages.HotelStatus)
        lnkRatePlanInventory.NavigateUrl = UrlPage(PaginaBase.pages.InventarioRatePlans)
        lnkHomeInventory.NavigateUrl = UrlPage(PaginaBase.pages.InventarioHotel)
        hplInvoicing.NavigateUrl = UrlPage(PaginaBase.pages.MainInvoicing) & "?cid=136"
        Me.hplFaresCopy.NavigateUrl = UrlPage(PaginaBase.pages.FaresCopy)
        lnkUsuarios.Visible = False

        If MyBase.IsSupervisor Then
            lnkUsuarios.Visible = True
            If HeaderPanel <> PaginaBase.eUsuario.uPortalAdministrador Then HeaderPanel = PaginaBase.eUsuario.uPortalAdministrador
            TblRegistro.Visible = False
            Me.lblRegistro.Visible = False
            'lnkRooms.Visible = True
            'lnkPlanes.Visible = True
            ' Me.pnlPortal.Visible = True
            If MyBase.cInfoActual.Hotel = 0 Then
                MyBase.redirectTo(PaginaBase.pages.SearchHotel)
            Else
                Session("urlCurrent") = GeRequestApplicationPath("/Portal/Pages/Home.aspx")
            End If
            Me.cmbHotels.Visible = False
            Me.lblNameHotel.Visible = True
            Me.lblNameHotel.Text = MyBase.cInfoActual.HotelName
            TblRegistro.Visible = True
            Me.lblRegistro.Visible = True
            tblPortal.Visible = True
        ElseIf MyBase.IsUsuarioHotel Then
            TblRegistro.Visible = False
            Me.lblRegistro.Visible = False
            'traer el hotel al que pertenece
            Me.cmbHotels.Visible = True
            Me.lblNameHotel.Visible = False
            clearviewstatepermisos()
            If Not IsPostBack Then
                Me.loadHotelUsuarioHotel()
                Me.cmbHotels.SelectedIndex = Me.cmbHotels.Items.IndexOf(Me.cmbHotels.Items.FindByValue(MyBase.cInfoActual.Hotel))
                Session("urlCurrent") = GeRequestApplicationPath("/Portal/Pages/Home.aspx")
            End If
            If Me.cmbHotels.SelectedIndex >= 0 Then
                Dim cInfo As companyInfo
                cInfo = viewstate.Item("cInfo_" & Me.cmbHotels.SelectedItem.Value)
                MyBase.cInfoActual = cInfo
            End If
            'leer permisos y activarlos en el menu y en los linkbuttons
            HabilitaLinks()
        Else
            tblPortal.Visible = False
            TblRegistro.Visible = False
            Me.lblRegistro.Visible = False
            Me.cmbHotels.Visible = True
            Me.lblNameHotel.Visible = False
            lblCallCenter.Visible = False
            hplBooking.Visible = False
            lnkUsuarios.Visible = True
            If Not IsPostBack Then
                Me.loadHotels()
                Me.cmbHotels.SelectedIndex = Me.cmbHotels.Items.IndexOf(Me.cmbHotels.Items.FindByValue(MyBase.cInfoActual.Hotel))
                Session("urlCurrent") = GeRequestApplicationPath("/Portal/Pages/Home.aspx")
            End If
            If Me.cmbHotels.SelectedIndex >= 0 Then
                Dim cInfo As companyInfo
                cInfo = viewstate.Item("cInfo_" & Me.cmbHotels.SelectedItem.Value)
                MyBase.cInfoActual = cInfo
            End If
        End If
    End Sub

    Private Sub loadHotels()
        If Usuario = 0 Then Exit Sub
        Dim reader As SqlDataReader
        With New HotelSistema
            reader = .GetHotelsCompanyByUserId(Usuario)
        End With
        While reader.Read
            Dim item As New ListItem(reader.Item("Nombre"), reader.Item("idHotel"))
            Me.cmbHotels.Items.Add(item)
            'Agregar al viewstate los datos del hotel
            Dim cInfo As New companyInfo
            cInfo.Rubro = reader.Item("idRubro")
            cInfo.Hotel = reader.Item("idHotel")
            cInfo.Empresa = reader.Item("idEmpresa")
            cInfo.HotelName = reader.Item("Nombre")
            cInfo.Address = reader.Item("Domicilio")
            cInfo.City = reader.Item("Ciudad")
            cInfo.State = reader.Item("Estado")
            cInfo.Contact = reader.Item("Contacto_Nombre")
            cInfo.Email = reader.Item("Contacto_Email")
            cInfo.Phone = reader.Item("Telefono")
            cInfo.idpais = reader.Item("idpais")
            cInfo.UserPerfil = PaginaBase.PerfilHotel.Avanzado

            If Not reader.Item("UserPerfil") Is DBNull.Value Then
                cInfo.UserPerfil = reader.Item("UserPerfil")
            End If
            viewstate.Item("cInfo_" & reader.Item("idHotel")) = cInfo
        End While
    End Sub
    Private Sub loadHotelUsuarioHotel()
        If Usuario = 0 Then Exit Sub
        Dim reader As SqlDataReader
        With New UsuarioHotelFacade
            reader = .LoadHotelsByUsuarioHotel(Usuario)
        End With
        While reader.Read
            Dim item As New ListItem(reader.Item("Nombre"), reader.Item("idHotel"))
            Me.cmbHotels.Items.Add(item)
            'Agregar al viewstate los datos del hotel
            Dim cInfo As New companyInfo
            cInfo.Rubro = reader.Item("idRubro")
            cInfo.Hotel = reader.Item("idHotel")
            cInfo.Empresa = reader.Item("idEmpresa")
            cInfo.HotelName = reader.Item("Nombre")
            cInfo.Address = reader.Item("Domicilio")
            cInfo.City = reader.Item("Ciudad")
            cInfo.State = reader.Item("Estado")
            cInfo.Contact = reader.Item("Contacto_Nombre")
            cInfo.Email = reader.Item("Contacto_Email")
            cInfo.Phone = reader.Item("Telefono")
            cInfo.idpais = reader.Item("IdPais")
            'cinfo.UserPerfil=reader.Item("")

            viewstate.Item("cInfo_" & reader.Item("idHotel")) = cInfo
        End While
        Dim DS As PermisosData
        With New PermisosFacade
            DS = .PermisosGetByUser(Usuario)
        End With
        For Each R As DataRow In DS.Tables(DS.PermisosTable).Rows
            Dim der As New DerechoUsuario
            der.Permiso = R(DS.PermisosField)
            der.PermisoName = R(DS.NameField)
            MyBase.PermisoUser(R(DS.NameField)) = der
        Next
        'aki hay que desabilitar los links
    End Sub
    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lnkSearch.Text = PortalCulture.GetString("00269")
        lnkUsuarios.Text = PortalCulture.GetString("00383")
        hplInfGral.Text = PortalCulture.GetString("00384")
        hplRoomTypes.Text = PortalCulture.GetString("00056")
        hplInvoicing.Text = PortalCulture.GetString("M000644")
        lblReservaciones.Text = PortalCulture.GetString("M000028")
        hplListado.Text = PortalCulture.GetString("00385")
        hplConfirmRes.Text = PortalCulture.GetString("00386")
        HplNoConfRes.Text = PortalCulture.GetString("00387")
        hplByChanel.Text = PortalCulture.GetString("00388")
        lblUsuarios.Text = PortalCulture.GetString("00389")
        hplChangePassword.Text = PortalCulture.GetString("M000038")
        lblReports.Text = PortalCulture.GetString("00259")
        lnkHomeStatus.Text = PortalCulture.GetString("00260")
        lnkNoDefinedRates.Text = PortalCulture.GetString("00261")
        lnkRatesChart.Text = PortalCulture.GetString("00262")
        lblInventory.Text = PortalCulture.GetString("00263")
        lnkHomeInventory.Text = "Hotel"
        lnkRatePlanInventory.Text = PortalCulture.GetString("00400")
        hplSoldOut.Text = PortalCulture.GetString("00326")
        lblRatesPlans.Text = PortalCulture.GetString("00016")
        lnkRatePlanLinks.Text = PortalCulture.GetString("00475")
        lnkRateplanRules.Text = PortalCulture.GetString("00015")
        lblRatesPlans.Text = PortalCulture.GetString("00047")
        lnkRatePlans.Text = PortalCulture.GetString("00047")
        lblRooms.Text = PortalCulture.GetString("00048")
        lnkRoomslinks.Text = PortalCulture.GetString("00475")
        lnkRooms.Text = PortalCulture.GetString("00048")
        lblCatalogo.Text = PortalCulture.GetString("00299")
        lnkfaresCatalogue.Text = PortalCulture.GetString("00267")
        hplCreditCard.Text = PortalCulture.GetString("00399")
        lblRegistro.Text = PortalCulture.GetString("00390")
        ' Aqui va el string de cierre de habitacioens lblRegistro.Text = PortalCulture.GetString("1647")
        hplChangeLogo.Text = PortalCulture.GetString("A00736")
        lnkInfProp.Text = PortalCulture.GetString("A00682")
        lnkInfAmenities.Text = PortalCulture.GetString("00402")
        lnkActivities.Text = PortalCulture.GetString("00403")
        lnkAtraction.Text = PortalCulture.GetString("00404")
        hplPolicies.Text = PortalCulture.GetString("A00732")
        hplFood.Text = PortalCulture.GetString("A00728")
        hplRoomAmenities.Text = PortalCulture.GetString("00405")
        hplRooms.Text = PortalCulture.GetString("A00734")
        hplMapLocation.Text = PortalCulture.GetString("00406")
        hplphotos.Text = PortalCulture.GetString("00407")
        hplLog.Text = "Logs"
        lblCallCenter.Text = "Call Center" 'PortalCulture.GetString("")
        hplBooking.Text = PortalCulture.GetString("00401")
        lblTitulo.Text = PortalCulture.GetString("00157")
        Me.hplFaresCopy.Text = PortalCulture.GetString("00488")
    End Sub

    Private Sub HabilitaLinks()
        lblRatesPlans.Visible = False
        lnkRatePlans.Visible = False
        lnkRatePlanLinks.Visible = False
        lnkRateplanRules.Visible = False
        lnkRooms.Visible = False
        lblRooms.Visible = False
        lnkRoomslinks.Visible = False
        lblCatalogo.Visible = False
        lnkfaresCatalogue.Visible = False
        lblInventory.Visible = False
        lnkHomeInventory.Visible = False
        lnkRatePlanInventory.Visible = False
        lblReports.Visible = False
        lnkHomeStatus.Visible = False
        lnkNoDefinedRates.Visible = False
        lnkRatesChart.Visible = False
        hplSoldOut.Visible = False
        lnkSearch.Visible = False
        lblPortal.Visible = False
        'estos no se habilitaran
        Me.TblRegistro.Visible = False
        tblListReservation.Visible = False
        hplRoomTypes.Visible = False
        hplInfGral.Visible = False
        hplLog.Visible = False
        hplCreditCard.Visible = False
        lblCallCenter.Visible = False
        hplBooking.Visible = False
        lblUsuarios.Visible = False
        hplChangePassword.Visible = False
        Me.hplFaresCopy.Visible = False

        'TRAER LOS DERECHOS DE LOS USUARIOS
        If Not Me.PermisoUser(permisos.RatePlan) Is Nothing Or Not Me.PermisoUser(permisos.RatePlanLinks) Is Nothing Or Not Me.PermisoUser(permisos.RatePlanRules) Is Nothing Then
            lblRatesPlans.Visible = True
            If Not Me.PermisoUser(permisos.RatePlan) Is Nothing Then
                lnkRatePlans.Visible = True
            End If
            If Not Me.PermisoUser(permisos.RatePlanLinks) Is Nothing Then
                lnkRatePlanLinks.Visible = True
            End If
            If Not Me.PermisoUser(permisos.RatePlanRules) Is Nothing Then
                lnkRateplanRules.Visible = True
            End If
        End If
        If Not Me.PermisoUser(permisos.Rooms) Is Nothing Or Not Me.PermisoUser(permisos.RoomsLinks) Is Nothing Then
            lblRooms.Visible = True
            If Not Me.PermisoUser(permisos.Rooms) Is Nothing Then
                lnkRooms.Visible = True
            End If
            If Not Me.PermisoUser(permisos.RoomsLinks) Is Nothing Then
                lnkRoomslinks.Visible = True
            End If
        End If
        If Not Me.PermisoUser(permisos.Tarifas) Is Nothing Then
            lblCatalogo.Visible = True
            lnkfaresCatalogue.Visible = True
        End If


        If Not Me.PermisoUser(permisos.RatePlanInventory) Is Nothing Or Not Me.PermisoUser(permisos.InventoryRooms) Is Nothing Or Not Me.PermisoUser(permisos.SoldOutInventory) Is Nothing Then
            lblInventory.Visible = True
            If Not Me.PermisoUser(permisos.RatePlanInventory) Is Nothing Then
                lnkHomeInventory.Visible = True
            End If
            If Not Me.PermisoUser(permisos.InventoryRooms) Is Nothing Then
                lnkRatePlanInventory.Visible = True
            End If
            If Not Me.PermisoUser(permisos.SoldOutInventory) Is Nothing Then
                hplSoldOut.Visible = True
            End If

        End If

        If Not Me.PermisoUser(permisos.Bloqueos) Is Nothing Or Not Me.PermisoUser(permisos.Tarifas) Is Nothing Then
            lblReports.Visible = True
            If Not Me.PermisoUser(permisos.Bloqueos) Is Nothing Then
                lnkHomeStatus.Visible = True
            End If
            If Not Me.PermisoUser(permisos.Tarifas) Is Nothing Then
                lnkNoDefinedRates.Visible = True
                lnkRatesChart.Visible = True
            End If
        End If
    End Sub


End Class
