Imports System.Runtime.Serialization
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Facade

Public Class HousesCatalogue
    Inherits PaginaBase


    Enum dgcolumns
        checkbox
        nombre
        idHotel
        idTipoHabitacion_hotel
        rates
        allRates
        locks
        allLocks
        edit
        delete
    End Enum
    Const KEY_MINPRICE As String = "mintarifaAdulto"
    Const KEY_MAXPRICE As String = "maxtarifaAdulto"
    Private Property dsRooms() As RoomsHotelData
        Get
            Return Session("_dsrooms")
        End Get
        Set(ByVal Value As RoomsHotelData)
            Session("_dsrooms") = Value
        End Set
    End Property
    Private Property idroom() As Integer
        Get
            Return ViewState("_idRoom")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_idRoom") = Value
        End Set
    End Property
    Private Property room() As String
        Get
            Return ViewState("_Room")
        End Get
        Set(ByVal Value As String)
            ViewState("_Room") = Value
        End Set
    End Property
    Public ReadOnly Property IdDg() As String
        Get
            Return Me.CtrlPlanFares2.iddg
        End Get
    End Property
    Public ReadOnly Property IdDgAdult() As String
        Get
            Return Me.CtrlPlanFares2.iddgAdult
        End Get
    End Property
    Public ReadOnly Property IdDgChild() As String
        Get
            Return Me.CtrlPlanFares2.iddgChild
        End Get
    End Property
    Public ReadOnly Property IdDgTeen() As String
        Get
            Return Me.CtrlPlanFares2.iddgTeen
        End Get
    End Property


    Property Editando() As Boolean
        Get
            Return ViewState("Editando")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Editando") = Value
        End Set
    End Property
    Public Property Ids As String
        Get
            Return ViewState("Ids")
        End Get
        Set(ByVal Value As String)
            ViewState("Ids") = Value
        End Set
    End Property
    Property OldStart As String
        Get
            Return ViewState("OldStart")
        End Get
        Set(value As String)
            ViewState("OldStart") = value
        End Set
    End Property
    Property OldEnd As String
        Get
            Return ViewState("OldEnd")
        End Get
        Set(value As String)
            ViewState("OldEnd") = value
        End Set
    End Property

    'Public Property SourceName() As String
    '    Get
    '        Return viewstate("_SN")
    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_SN") = Value
    '    End Set
    'End Property


    'Public Property SourceRateName() As String
    '    Get
    '        Return viewstate("_SRN")

    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_SRN") = Value
    '        Me.CtrRateAplication1.SourceRateName = Value
    '    End Set
    'End Property


#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Protected ReadOnly Property RateModeView() As Integer
        Get
            Dim value As Integer = 0

            If (Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Adult", Me.CtrRateAplication1.GetFareFor("Adult", False), False) _
                    OrElse _
                    Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Children", Me.CtrRateAplication1.GetFareFor("Child", False), False) _
                    OrElse _
                    Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Teen", Me.CtrRateAplication1.GetFareFor("Teen", False), False) _
                OrElse _
                      (Me.CtrlPlanFaresExc2.FieldException <> "NNNNNNN" _
                        AndAlso (Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Adult", Me.CtrRateAplication1.GetFareFor("Adult", False), False) _
                        OrElse _
                        Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Children", Me.CtrRateAplication1.GetFareFor("Child", False), False) _
                        OrElse _
                        Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Teen", Me.CtrRateAplication1.GetFareFor("Teen", False), False) _
                        ))) Then
                value = 1
            End If

            Return value
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        'If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then


            CtrRateAplication1.m_iHotelId = 0
            lblPriceError.Visible = False
            lblError.Visible = False
            hplShowRates.Style.Add("display", "none")
            hplShowRates.Visible = False
            hplHideRates.Style.Add("display", "none")
            hplHideRates.Visible = False
            pnlData.Style.Add("display", "none")

            loadDatos()
            idroom = Request.QueryString("Room")
            If idroom <> 0 Then
                Me.ddlRooms.SelectedValue = idroom
            Else
                Try
                    idroom = ddlRooms.SelectedValue
                Catch ex As Exception
                    idroom = 0
                End Try
            End If
            btnLoad_Click(sender, e)
            'Me.ddlRooms.Attributes.Add("onChange", "javascript:showRoomType('" & Me.ddlRooms.ClientID & "','" & Me.SourceName & "')")
            'Me.ddlratesplans.Attributes.Add("onChange", "javascript:showRatePlan('" & Me.ddlratesplans.ClientID & "','" & Me.SourceRateName & "','" & lblRatePlan.ClientID & "')")
            Me.hplHideRates.NavigateUrl = "javascript:Ocultar('0');"
            hplShowRates.NavigateUrl = "javascript:Ocultar('1');"
            cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", pnlData.ClientID, cmdNew.ClientID, "true"))
            Editando = False

            btnSave.Attributes.Add("onclick", String.Format("javascript:FireSave('{0}');", Me.CtrRateAplication1.GetClientID))
            'btnPublish.Attributes.Add("onclick", String.Format("javascript:FireSave('{0}');", Me.CtrRateAplication1.GetClientID))
            btnNew.Attributes.Add("onclick", String.Format("javascript:FireSave('{0}');", Me.CtrRateAplication1.GetClientID))
            Me.ResizefrmPrincipal()
        Else
            '   modalTable.Visible = True
            hideCols()
        End If
    End Sub
    Private Sub hideCols()
        For Each e As DataGridItem In Me.dgRooms.Items
            If e.ItemType = ListItemType.AlternatingItem Or e.ItemType = ListItemType.Item Or e.ItemType = ListItemType.SelectedItem Then
                If e.Cells(dgcolumns.rates).Text.Contains("Sin") Then
                    e.Cells(dgcolumns.allRates).Text = ""
                    e.Cells(dgcolumns.allRates).Enabled = False
                End If
                If e.Cells(dgcolumns.locks).Text.Contains("Sin") Then
                    e.Cells(dgcolumns.allLocks).Text = ""
                    e.Cells(dgcolumns.allLocks).Enabled = False
                End If
            End If
        Next
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadCulture()
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.Tarifas, Me.btnNew, "A")
        'CType(Me.Page, PaginaBase).Habilitaboton(permisos.Tarifas, Me.btnDelete, "D")
        Dim hpl As HyperLink
        For Each i As DataGridItem In Me.dgRooms.Items
            If i.ItemType = ListItemType.AlternatingItem Or i.ItemType = ListItemType.Item Then
                'Dim ibtnEdit As LinkButton = i.FindControl("lnkEdit")
                'CType(Me.Page, PaginaBase).Habilitaboton(permisos.Tarifas, ibtnEdit, "M")
                hpl = i.FindControl("lnkDelete")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.Tarifas, hpl, "D")
            End If
        Next
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        Page.ClientScript.RegisterForEventValidation(Me.btnSave.UniqueID)
        MyBase.Render(writer)
    End Sub

    Private Sub loadCulture()
        If Editando Then
            lblMsg.Text = String.Format("{0} {1}", PortalCulture.GetString("01250"), PortalCulture.GetString("00133"))
        Else
            lblMsg.Text = String.Format("{0} {1}", PortalCulture.GetString("00102"), PortalCulture.GetString("00133"))
        End If
        lblTitle.Text = PortalCulture.GetString("00133")
        Me.lblEName.Text = PortalCulture.GetString("00170", True)
        lblERatesPlans.Text = PortalCulture.GetString("00016", True)
        If Me.idroom = 0 Then
            Me.lblFaresTitle.Text = PortalCulture.GetString("00171") & PortalCulture.GetString("00423")
        Else
            Me.lblFaresTitle.Text = PortalCulture.GetString("00171") & " " & PortalCulture.GetString("00170") & " " & room
        End If
        Me.lblPreciosTarifa.Text = PortalCulture.GetString("00275")
        Me.btnNew.Text = PortalCulture.GetString("00009")
        Me.btnSave.Text = PortalCulture.GetString("00008")
        Me.cmdNew.Value = PortalCulture.GetString("00102")
        Me.btnLoad.Text = PortalCulture.GetString("00149")
        Me.hplShowRates.Text = PortalCulture.GetString("00280")
        Me.hplHideRates.Text = PortalCulture.GetString("00281")
        lblError.Text = PortalCulture.GetString("00634")
        ddlRooms.Items(0).Text = PortalCulture.GetString("M000272")
        Me.lblNoroomSelected.Text = PortalCulture.GetString("00436")
        Me.lblPriceError.Text = PortalCulture.GetString("00682")
        Me.chkOldDates.Text = PortalCulture.GetString("01050")
        lblPricingNE.Text = PortalCulture.GetString("M000391")
        lblPricingExc.Text = PortalCulture.GetString("M000402")
    End Sub
    Public Sub loadDatos()
        Dim Rooms As New Portal.Hotel.Common.Data.RoomsHotelData
        Dim RatesPlan As Portal.General.Common.Data.RatePlanData
        With New Portal.Hotel.Facade.RoomFacade
            Rooms = .getRooms(0, PortalCulture.GetIDCulture)
        End With
        Rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ",1,25)")
        RatesPlan = CtrRateAplication1.loadAllRatesplans(1)
        Try
            Me.ddlratesplans.DataSource = RatesPlan
            Me.ddlratesplans.DataValueField = Portal.General.Common.Data.RatePlanData.FIELD_IDRATEPLAN
            Me.ddlratesplans.DataTextField = "Nombre" 'Portal.General.Common.Data.RatePlanData.FIELD_CODIGOTARIFA


            ddlRatePlans.DataTextField = "Nombre" 'ds.FIELD_CODIGOTARIFA
            ddlRatePlans.DataValueField = Portal.General.Common.Data.RatePlanData.FIELD_IDRATEPLAN
            ddlRatePlans.DataSource = RatesPlan


            DropDownList1.DataTextField = "Nombre" 'ds.FIELD_CODIGOTARIFA
            DropDownList1.DataValueField = Portal.General.Common.Data.RatePlanData.FIELD_IDRATEPLAN
            DropDownList1.DataSource = RatesPlan

            Me.ddlratesplans.DataBind()
            ddlRatePlans.DataBind()
            DropDownList1.DataBind()
        Catch
            Me.ddlratesplans.DataTextField = "name"
            ddlRatePlans.DataTextField = "name"
            DropDownList1.DataTextField = "name"

            Me.ddlratesplans.DataBind()
            ddlRatePlans.DataBind()
            DropDownList1.DataBind()

        End Try

        'lblRatePlan.Text = "-"
        'SourceRateName = ""
        'For j As Integer = 0 To RatesPlan.Tables(RatesPlan.RATEPLAN_TABLE).Rows.Count - 1
        '    If RatesPlan.Tables(RatesPlan.RATEPLAN_TABLE).Rows(j).Item(RatesPlan.FIELD_NAME).ToString = "" Then
        '        Me.SourceRateName &= "--"
        '    Else
        '        Me.SourceRateName &= RatesPlan.Tables(RatesPlan.RATEPLAN_TABLE).Rows(j).Item(RatesPlan.FIELD_NAME).ToString & "//"
        '    End If

        '    lblRatePlan.Text = "-" '& RatesPlan.Tables(RatesPlan.RATEPLAN_TABLE).Rows(0)(RatesPlan.FIELD_NAME)
        'Next

        Me.ddlratesplans.Items.Insert(0, "Todos")
        ddlratesplans.Items(0).Value = "0"
        ddlratesplans.Items(0).Text = PortalCulture.GetString("00172")
        ddlratesplans.SelectedIndex = 0

        Me.ddlStatus.Items.Clear()
        Me.ddlStatus.Items.Add("Select")
        Me.ddlStatus.Items.Add("O")
        Me.ddlStatus.Items.Add("C")
        Me.ddlStatus.Items.Add("N")

        Me.ddlStatus.Items(0).Text = PortalCulture.GetString("M000272")
        Me.ddlStatus.Items(0).Value = "0"

        Me.ddlStatus.Items(1).Text = PortalCulture.GetString("00150")
        Me.ddlStatus.Items(2).Text = PortalCulture.GetString("00151")
        Me.ddlStatus.Items(3).Text = PortalCulture.GetString("00152")

        Me.ddlStatus.Items(1).Value = "O"
        Me.ddlStatus.Items(2).Value = "C"
        Me.ddlStatus.Items(3).Value = "N"

        Me.ddlEditStatus.Items.Clear()
        Me.ddlEditStatus.Items.Add("Select")
        Me.ddlEditStatus.Items.Add("O")
        Me.ddlEditStatus.Items.Add("C")
        Me.ddlEditStatus.Items.Add("N")

        Me.ddlEditStatus.Items(0).Text = PortalCulture.GetString("M000272")
        Me.ddlEditStatus.Items(0).Value = "0"

        Me.ddlEditStatus.Items(1).Text = PortalCulture.GetString("00150")
        Me.ddlEditStatus.Items(2).Text = PortalCulture.GetString("00151")
        Me.ddlEditStatus.Items(3).Text = PortalCulture.GetString("00152")

        Me.ddlEditStatus.Items(1).Value = "O"
        Me.ddlEditStatus.Items(2).Value = "C"
        Me.ddlEditStatus.Items(3).Value = "N"
        Dim links As New LinkRoomTypeData
        With New LinkRoomsFacade
            links = .getList(0, PortalCulture.GetIDCulture)
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
        ddlRooms.DataTextField = "texto" 'Rooms.FLD_ROOM_CODE
        ddlRooms.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
        ddlRooms.DataSource = Rooms
        ddlRooms.DataBind()
        ddlRooms.Items.Insert(0, PortalCulture.GetString("M000272"))
        ddlRooms.Items(0).Value = 0
        dsRooms = Rooms
        Try
            If ddlRooms.Items.Count > 1 Then
                ddlRooms.SelectedIndex = 1
            Else
                ddlRooms.SelectedIndex = 0
            End If
            idroom = ddlRooms.SelectedValue
            Dim ci As System.Globalization.CultureInfo
            ci = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
            Me.dgRooms.DataSource = getPropertiesData()
            Me.dgRooms.DataBind()
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
        Catch ex As Exception
        End Try
    End Sub

    Private Sub dgRooms_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRooms.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
            If e.Item.Cells(dgcolumns.rates).Text.Contains("Sin") Then
                e.Item.Cells(dgcolumns.allRates).Text = ""
                e.Item.Cells(dgcolumns.allRates).Enabled = False
            End If
            If e.Item.Cells(dgcolumns.locks).Text.Contains("Sin") Then
                e.Item.Cells(dgcolumns.allLocks).Text = ""
                e.Item.Cells(dgcolumns.allLocks).Enabled = False
            End If
        End If
    End Sub
    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Try
            idroom = ddlRooms.SelectedValue
            room = ddlRooms.SelectedItem.Text
        Catch ex As Exception
            idroom = 0
        End Try
        dgRooms.CurrentPageIndex = 0
        dgRooms.SelectedIndex = -1
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.dgRooms.DataSource = Nothing
        Me.dgRooms.DataSource = getPropertiesData(ddlratesplans.SelectedValue)
        Me.dgRooms.DataBind()
        System.Threading.Thread.CurrentThread.CurrentCulture = ci

        Me.CtrRateAplication1.Segment(ddlratesplans.SelectedValue)
        Me.CtrRateAplication1.LoadRooms(idroom)
        lblPriceError.Visible = False

    End Sub

    Protected Function link(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not sender Is Nothing AndAlso Not sender.GetType() = Me.Page.GetType() Then
            Dim b As Button
            Dim ib As ImageButton
            Try
                b = DirectCast(sender, Button)
            Catch ex As Exception
                ib = DirectCast(sender, ImageButton)
            End Try
            If b Is Nothing Then
                Dim ev As ImageButton = ib
                Select Case ev.CommandName
                    Case "ShowLocks"
                        showLocks(sender)
                    Case "ShowRates"
                        showRates(sender)
                    Case "Edit"
                        If CType(ev.Parent.Parent, DataGridItem).Cells(1).Text.Contains("Tarifa") Then
                            editRate(sender, e)
                        Else
                            editLock(sender, e)
                        End If
                    Case "Delete"
                        If CType(ev.Parent.Parent, DataGridItem).Cells(1).Text.Contains("Tarifa") Then
                            deleteRate(sender, e)
                        Else
                            deleteLock(sender, e)
                        End If
                    Case "EditRate", "EditLock"
                        Save(sender, ev.CommandName = "EditLock")
                End Select
            Else
                Dim ev As Button = b
                Select Case ev.CommandName
                    Case "ShowLocks"
                        showLocks(sender)
                    Case "ShowRates"
                        showRates(sender)
                    Case "Edit"
                        If CType(ev.Parent.Parent, DataGridItem).Cells(1).Text.Contains("Tarifa") Then
                            editRate(sender, e)
                        Else
                            editLock(sender, e)
                        End If
                    Case "Delete"
                        If CType(ev.Parent.Parent, DataGridItem).Cells(1).Text.Contains("Tarifa") Then
                            deleteRate(sender, e)
                        Else
                            deleteLock(sender, e)
                        End If
                        redirect()
                    Case "EditRate", "EditLock"
                        Save(sender, ev.CommandName = "EditLock")
                End Select
            End If
            
        End If
        Return False
    End Function
    Private Sub Save(ByVal s As Object, ByVal isLock As Boolean)
        If Not isLock Then
            SaveRate(Integer.Parse(editRateId.Text), editRateRoom.Text, editAdulto.Text, editMenor.Text, editRateTo.Text, editRateFrom.Text)
        Else
            SaveLock(Integer.Parse(EditIdHotel.Text), EditratePlan.SelectedValue, EditIdTipoH.Text, editLockFrom.Text, editLockTo.Text, ddlEditStatus.SelectedValue)
        End If
        redirect()
    End Sub
   
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


    Public Function getPropertiesData(Optional ByVal idRatePlan As String = "") As DataSet
        If dgRooms.DataSource Is Nothing Then
            If IsUsuarioHomeAgency Then
                'Si es usuario de agencia regresa todas sus propiedades
                With New FaresSystem
                    getPropertiesData = .GetTarifasByUserId(Me.Usuario, False, IIf(idRatePlan = "" Or idRatePlan = "0", Nothing, idRatePlan))
                End With
            Else
                'Si no, es administrador, asi que regresa todas
                With New FaresSystem
                    getPropertiesData = .GetTarifasByUserId(Me.Usuario, True, IIf(idRatePlan = "" Or idRatePlan = "0", Nothing, idRatePlan))
                End With
            End If

            Dim vLocks As Boolean = False
            Dim vRates As Boolean = False

            'Build DataSet
            Dim ds As New DataSet
            ds.Tables.Add(New DataTable)
            ds.Tables(0).Columns.Add("IdHotel")
            ds.Tables(0).Columns.Add("Nombre")
            ds.Tables(0).Columns.Add("IdTipoHabitacion_Hotel")
            ds.Tables(0).Columns.Add("Rate")
            ds.Tables(0).Columns.Add("Lock")
            Dim rate As String
            Dim lock As String
            'Por Cada Propiedad
            If Not getPropertiesData Is Nothing Then
                For Each dr As DataRow In getPropertiesData.Tables(0).Rows

                    rate = "Sin tarifas"
                    lock = "Sin cierres"
                    Dim dvProperties As DataView = getPropertiesData.Tables(0).DefaultView
                    Dim dvRates As DataView = getPropertiesData.Tables(1).DefaultView
                    Dim dvLocks As DataView = getPropertiesData.Tables(2).DefaultView

                    Dim filteredRates As DataRow() = getPropertiesData.Tables(1).Select(" IdTipoHabitacion_Hotel = " & dr.Item("IdTipoHabitacion_Hotel"))
                    If filteredRates.Length <> 0 Then
                        rate = "Precio: $" & Decimal.Parse(filteredRates(0).Item("Precio")).ToString("F2") & " del " & filteredRates(0).Item("FechaInicia") & " al " & filteredRates(0).Item("FechaFinaliza")
                        If filteredRates.Length > 1 Then
                            vRates = True
                        End If
                    End If

                    Dim filteredLocks As DataRow() = getPropertiesData.Tables(2).Select("IdHotel = " & dr.Item("IdHotel") & IIf(ddlratesplans.SelectedValue = "0", "", " AND IdRatePlan like  '" & ddlratesplans.SelectedValue & "'"))
                    If filteredLocks.Length <> 0 Then
                        Select Case filteredLocks(0).Item("StatusAvail")
                            Case "C"
                                lock = "Cierre"
                            Case "N"
                                lock = "No hay llegadas"
                        End Select
                        lock &= " del " & filteredLocks(0).Item("StartDate") & " al " & filteredLocks(0).Item("EndDate") & " en el RatePlan " & filteredLocks(0).Item("IdRatePlan")
                        If filteredLocks.Length > 1 Then
                            vLocks = True
                        End If
                    End If
                    Dim row As DataRow = ds.Tables(0).NewRow()
                    row.Item("IdHotel") = dr.Item("IdHotel")
                    row.Item("Nombre") = dr.Item("Nombre")
                    row.Item("IdTipoHabitacion_Hotel") = dr.Item("IdTipoHabitacion_Hotel")
                    row.Item("Lock") = lock
                    row.Item("Rate") = rate
                    ds.Tables(0).Rows.Add(row)
                Next
            End If
            Return ds
        End If
        Return dgRooms.DataSource
    End Function

    Private Sub dgRatePlans_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRooms.ItemCreated
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
    ' Handlers de los botones
    Public Sub addRates(ByVal sender As Object, ByVal e As EventArgs) Handles addRate.ServerClick
        Dim test As String
        Dim ids As String = getSelectedProperties()
        For Each id As String In ids.Split(",")
            If id <> "" Then
                idroom = id.Split(":")(1)
                id = id.Split(":")(0)
                Dim datFare As New FaresData
                Dim ExistCode As New FaresData
                Dim rowFare As DataRow
                Dim dv As DataView
                Dim room As RoomsHotelData
                With New RoomFacade
                    room = .getRoomByID(idroom)
                End With
                Try
                    With datFare.Tables(FaresData.FARES_TABLE)
                        rowFare = .NewRow()
                        rowFare(FaresData.ENDDATE_FIELD) = txtDateToRate.Text
                        rowFare(FaresData.PRICE_FIELD) = CDbl(Me.rateA.Text)
                        rowFare(FaresData.NINIORATE) = CDbl(Me.rateC.Text)
                        rowFare(FaresData.EXTRAADULTPRICE_FIELD) = 0
                        rowFare(FaresData.EXTRACHILDPRICE_FIELD) = 0
                        rowFare(FaresData.EXTRATEENPRICE_FIELD) = 0
                        rowFare(FaresData.RATEENPRICE_FIELD) = 0
                        rowFare(FaresData.STARTDATE_FIELD) = txtDateFromRate.Text
                        rowFare(FaresData.EXCEPTION_FIELD) = "NNNNNNN"
                        rowFare(FaresData.RULESDEFAULT) = True
                        rowFare(FaresData.NOARRIVOS_FIELD) = "NNNNNNN"
                        rowFare(FaresData.HOTELROOMTYPEID_FIELD) = idroom
                        rowFare(FaresData.RATETYPE_FIELD) = "R"
                        rowFare(FaresData.IDRATEPLAN_FIELD) = ddlRatePlans.SelectedValue
                        rowFare(FaresData.RATECODE_FIELD) = room.Tables(room.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ROOM_CODE) & ddlRatePlans.SelectedValue
                        .Rows.Add(rowFare)
                    End With
                    With New FaresSystem
                        If .InsertFares(datFare, "") Then
                        End If
                    End With
                Catch
                End Try
            End If
        Next
        redirect()
    End Sub
    Public Sub addLocks(ByVal sender As Object, ByVal e As EventArgs) Handles addLock.ServerClick
        Dim test As String
        Dim ids As String = getSelectedProperties()
        Dim a As New RoomClosure
        For Each id As String In ids.Split(",")
            If id <> "" Then
                idroom = id.Split(":")(1)
                id = id.Split(":")(0)
                If ddlStatus.SelectedValue <> "0" Then
                    a.CheckLockRoomTypes(id, DropDownList1.SelectedValue, txtDateFromLock.Text, txtDateToLock.Text, idroom, ddlStatus.SelectedValue)
                End If
            End If
        Next
        redirect()
    End Sub

    Public Function getSelectedProperties() As String
        getSelectedProperties = ""
        For Each dg As DataGridItem In dgRooms.Items
            Dim chk As CheckBox = dg.Cells(dgcolumns.checkbox).FindControl("check")
            If chk.Checked Then
                getSelectedProperties &= dg.Cells(dgcolumns.idHotel).Text & ":" & dg.Cells(dgcolumns.idTipoHabitacion_hotel).Text & ","
            End If
        Next
    End Function

    Public Sub showRates(ByVal s As Object)
        Dim row As DataGridItem = getRow(s)
        Dim idHotel As String = row.Cells(dgcolumns.idHotel).Text
        Dim idRoom As String = row.Cells(dgcolumns.idTipoHabitacion_hotel).Text
        Dim dsFares As DataSet, dsResult As New DataSet
        With New FaresSystem
            dsFares = .GetFaresByHotelRoomType(Integer.Parse(idRoom))
        End With
        dsResult.Tables.Add(New DataTable)
        dsResult.Tables(0).Columns.Add("id")
        dsResult.Tables(0).Columns.Add("data")
        For Each dr As DataRow In dsFares.Tables(0).Rows
            Dim rpName As String
            With New RatePlanFacade
                rpName = .GetDataRatePlan(dr.Item("idRatePlan"), idHotel).Tables(0).Rows(0).Item("name")
            End With
            Dim data As String = "Tarifa de $" & dr.Item("Precio") & " del " & CType(dr.Item("FechaInicia"), Date).ToString("dd/MMM/yyyy") & " al " & CType(dr.Item("FechaFinaliza"), Date).ToString("dd/MMM/yyyy") & " en el plan tarifario " & dr.Item("IdRatePlan")
            'Crear cadena   
            Dim newDr As DataRow = dsResult.Tables(0).NewRow()
            newDr.Item("id") = idHotel & "," & dr.Item("idTarifa") & ":" & dr.Item("idTipoHabitacion_Hotel")
            newDr.Item("data") = data
            dsResult.Tables(0).Rows.Add(newDr)
        Next
        modalTable.Visible = True
        modalGrid.Columns(1).HeaderText = row.Cells(dgcolumns.nombre).Text
        modalGrid.DataSource = dsResult
        modalGrid.DataBind()
    End Sub
    Public Sub showLocks(ByVal s As Object)
        Dim row As DataGridItem = getRow(s)
        Dim idHotel As String = row.Cells(dgcolumns.idHotel).Text
        Dim dsLocks As DataSet, dsResult As New DataSet
        dsLocks = GetLockRoomTypes(Integer.Parse(idHotel))
        dsResult.Tables.Add(New DataTable)
        dsResult.Tables(0).Columns.Add("id")
        dsResult.Tables(0).Columns.Add("data")
        For Each dr As DataRow In dsLocks.Tables(0).Rows
            Dim status As String
            Select Case dr.Item("StatusAvail")
                Case "C"
                    status = "Cierre"
                Case "N"
                    status = "No hay llegadas"
            End Select
            Dim data As String = status & " del " & CType(dr.Item("StartDate"), Date).ToString("dd/MMM/yyyy") & " al " & CType(dr.Item("EndDate"), Date).ToString("dd/MMM/yyyy") & " en el plan tarifario " & dr.Item("IdRatePlan")
            'Crear cadena
            Dim newDr As DataRow = dsResult.Tables(0).NewRow()
            newDr.Item("id") = idHotel & "," & dr.Item("IdRatePlan") & ":" & dr.Item("idTipoHabitacion_Hotel") & "¡" & dr.Item("StartDate") & "¿" & dr.Item("EndDate")
            newDr.Item("data") = data
            dsResult.Tables(0).Rows.Add(newDr)
        Next
        modalGrid.Columns(1).HeaderText = row.Cells(dgcolumns.nombre).Text
        modalTable.Visible = True
        modalGrid.DataSource = dsResult
        modalGrid.DataBind()
    End Sub

    Private Function GetLockRoomTypes(ByVal idHotel As Integer) As DataSet
        Dim data As New DataSet
        Dim dscommand As New SqlClient.SqlDataAdapter
        Dim ConnectionString As String
        ConnectionString = ConfigurationSettings.AppSettings("HotelConnectionString")
        dscommand.SelectCommand = New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim sqlConn As New SqlClient.SqlConnection(ConnectionString)
        sqlConn.Open()
        trans = sqlConn.BeginTransaction
        With dscommand
            Try
                .SelectCommand.CommandType = CommandType.StoredProcedure
                .SelectCommand.CommandText = "spGetAllLockRoomTypes"
                .SelectCommand.Connection = sqlConn
                .SelectCommand.Transaction = trans
                With .SelectCommand
                    .Parameters.Clear()
                    .Parameters.Add(New SqlClient.SqlParameter("@idhotel", SqlDbType.Int))
                    .Parameters("@idhotel").Value = idHotel
                End With
                .Fill(data)
            Catch ex As Exception
                Dim msg As String
                msg = ex.Message
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
        If sqlConn.State = ConnectionState.Open Then
            trans.Rollback()
            sqlConn.Close()
        End If
        Return data
    End Function

    Public Sub closeModal(ByVal sender As Object, ByVal e As EventArgs) Handles closeModalGrid.Click
        modalTable.Visible = False
        modalGrid.DataSource = Nothing
        modalGrid.DataBind()
    End Sub

    Public Sub editRate(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As DataGridItem = getRow(sender)
        Dim id As String = row.Cells(0).Text
        Dim ds As DataSet
        Dim rp As DataSet
        With New FaresSystem
            ds = .GetFareById(id.Split(",")(1).Split(":")(0))
        End With
        With New RatePlanFacade
            rp = .GetRatePlanByIdHotel(id.Split(",")(0))
        End With
        ddlEditRateRP.DataSource = rp
        ddlEditRateRP.DataTextField = "name"
        ddlEditRateRP.DataValueField = "IdRatePlan"
        ddlEditRateRP.DataBind()
        With ds.Tables(0).Rows(0)
            editAdulto.Text = .Item("Precio")
            editMenor.Text = .Item("niniosrate")
            editRateFrom.Text = .Item("FechaInicia")
            editRateTo.Text = .Item("FechaFinaliza")
            ddlEditRateRP.SelectedValue = .Item("IdRatePlan")
            editRateId.Text = .Item("idTarifa")
            editRateRoom.Text = .Item("idTipoHabitacion_Hotel")
        End With
        EditRates.Visible = True
    End Sub
    Public Sub deleteRate(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As DataGridItem = getRow(sender)
        Dim id As String = row.Cells(0).Text
        With New FaresSystem
            .DeleteFares(id.Split(",")(1).Split(":")(0))
        End With
    End Sub
    Public Sub editLock(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As DataGridItem = getRow(sender)
        Dim id As String = row.Cells(0).Text
        Dim text As String = row.Cells(1).Text
        Dim idHotel, idRatePlan, idTipoHabitacion, startDate, endDate As String
        'idh,idrp:idth¡sd¿ed
        Select Case text.Split(" ")(0)
            Case "Cierre"
                ddlEditStatus.SelectedValue = "C"
            Case Else
                ddlEditStatus.SelectedValue = "N"
        End Select
        With New RatePlanFacade
            EditratePlan.DataSource = .GetRatePlanByIdHotel(id.Split(",")(0))
            EditratePlan.DataTextField = "name"
            EditratePlan.DataValueField = "IdRatePlan"
            EditratePlan.DataBind()
        End With

        EditIdHotel.Text = id.Split(",")(0) : id = id.Split(",")(1)
        EditratePlan.SelectedValue = id.Split(":")(0) : id = id.Split(":")(1)
        EditIdTipoH.Text = id.Split("¡")(0) : id = id.Split("¡")(1)
        editLockFrom.Text = id.Split("¿")(0) : oldStart = id.Split("¿")(0) : id = id.Split("¿")(1)
        editLockTo.Text = id : oldEnd = id
        EditLocks.Visible = True
    End Sub
    Public Sub deleteLock(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As DataGridItem = getRow(sender)
        Dim id As String = row.Cells(0).Text
        Dim rc As New RoomClosure
        Dim idHotel, idRatePlan, idTipoHabitacion, startDate, endDate As String
        idHotel = id.Split(",")(0) : id = id.Split(",")(1)
        idRatePlan = id.Split(":")(0) : id = id.Split(":")(1)
        idTipoHabitacion = id.Split("¡")(0) : id = id.Split("¡")(1)
        startDate = id.Split("¿")(0) : id = id.Split("¿")(1)
        endDate = id
        rc.DeleteLockRoomType(idHotel, idRatePlan, endDate, idTipoHabitacion, startDate)

    End Sub

    Public Sub SaveLock(ByVal id As Integer, ByVal rp As String, ByVal h As String, ByVal f1 As String, ByVal f2 As String, ByVal status As String)
        With New RoomClosure
            .UpdateLockRoomType(id, rp, f1, f2, h, status, OldStart, OldEnd)
            'Método que borra los que estan abiertos
            .DeleteLockRoomType(id, rp, f2, h)
        End With
    End Sub
    Public Sub SaveRate(ByVal idTarifa As Integer, ByVal idroom As String, ByVal adult As Decimal, ByVal child As Decimal, ByVal f1 As String, ByVal f2 As String)
        Dim datFare As New FaresData
        Dim rowFare As DataRow
        Try
            With datFare.Tables(FaresData.FARES_TABLE)
                rowFare = .NewRow()
                rowFare(FaresData.ENDDATE_FIELD) = f2
                rowFare(FaresData.PRICE_FIELD) = CDbl(adult)
                rowFare(FaresData.NINIORATE) = CDbl(child)
                rowFare(FaresData.EXTRAADULTPRICE_FIELD) = 0
                rowFare(FaresData.EXTRACHILDPRICE_FIELD) = 0
                rowFare(FaresData.EXTRATEENPRICE_FIELD) = 0
                rowFare(FaresData.RATEENPRICE_FIELD) = 0
                rowFare(FaresData.STARTDATE_FIELD) = f1
                rowFare(FaresData.EXCEPTION_FIELD) = "NNNNNNN"
                rowFare(FaresData.RULESDEFAULT) = True
                rowFare(FaresData.NOARRIVOS_FIELD) = "NNNNNNN"
                rowFare(FaresData.HOTELROOMTYPEID_FIELD) = idroom
                rowFare(FaresData.RATETYPE_FIELD) = "R"
                rowFare(FaresData.IDRATEPLAN_FIELD) = idTarifa
                rowFare(FaresData.RATECODE_FIELD) = "A0S" & idTarifa
                .Rows.Add(rowFare)
            End With
            With New FaresSystem
                .UpdateFares(datFare, False, False)
            End With
        Catch
        End Try
    End Sub

    Public Sub closeEditLocks_click() Handles closeEditLocks.Click
        EditLocks.Visible = False
    End Sub
    Public Sub closeEditRates_click() Handles closeEditRates.Click
        EditRates.Visible = False
    End Sub

    Public Function getRow(ByVal s As Object) As DataGridItem
        Dim b As Button = DirectCast(s, Button)
        getRow = DirectCast(b.Parent.Parent, DataGridItem)
    End Function
    Public Sub redirect()
        Response.Redirect("../Casas/HousesCatalogue.aspx")
    End Sub
End Class



