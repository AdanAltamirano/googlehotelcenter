Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data
Imports Portal.General.Facade

Partial Class NoRatesReport
    Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

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
    '    End Set
    'End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            Me.txtDateFrom.Text = Date.Today.ToString("MM/dd/yyyy")
            Me.txtDateTo.Text = Date.Today.AddYears(1).ToString("MM/dd/yyyy")
            cargardatos()
            loaddatos()
        End If
        btnload.OnClientClick = "return FireUpdateStatus();"
    End Sub

    Private Function RatePlanFilter(ByVal segmentType As String, ByVal RatesPlan As Portal.General.Common.Data.RatePlanData) As Portal.General.Common.Data.RatePlanData
        'Elimina los planes tarifarios que contengan el tipo de segmento especificado
        Dim dv2 As DataView
        For Each r As DataRow In RatesPlan.Tables("RatePlans").Rows()
            dv2 = RatesPlan.Tables("RatePlans").DefaultView
            dv2.RowFilter = "Segment" & "=" & "'" & segmentType & "'"
            If dv2.Count > 0 AndAlso r("Segment").ToString() = segmentType Then
                r.Delete()
            End If
        Next
        RatesPlan.AcceptChanges()
        Return RatesPlan
    End Function

    Private Sub cargardatos()
        Dim dsrooms As RoomsHotelData
        Dim idAsoc As Integer = Me.GetIdAsociation
        With New RoomFacade
            dsrooms = .getRooms(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With
        dsrooms.Tables(dsrooms.TBL_ROOM_HOTEL).Columns.Add("texto", GetType(System.String), dsrooms.FLD_ROOM_CODE & "+' - ' +" & dsrooms.FLD_NOMBRE)

        'For i As Integer = 0 To dsrooms.Tables(dsrooms.TBL_ROOM_HOTEL).Rows.Count - 1
        '    Me.SourceName &= "//" & dsrooms.Tables(dsrooms.TBL_ROOM_HOTEL).Rows(i).Item(dsrooms.FLD_NOMBRE).ToString
        'Next

        ddlRooms.DataTextField = "texto" 'dsrooms.FLD_ROOM_CODE
        ddlRooms.DataValueField = dsrooms.FLD_ID_ROOM_HOTEL
        ddlRooms.DataSource = dsrooms
        ddlRooms.DataBind()
        ddlRooms.Items.Insert(0, PortalCulture.GetString("00144"))
        ddlRooms.Items(0).Value = 0


        Dim ds As RatePlanData
        With New RatePlanFacade
            If MyBase.IsSupervisor Then
                ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 0, 1, idAsociacion:=idAsoc, DeleteFilter:=1)
            Else
                ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc, DeleteFilter:=1)
            End If
        End With

        If MyBase.IdCorporativoUserChain = 4 AndAlso (MyBase.IsHotel Or MyBase.IsUsuarioHotel) Then
            ds = RatePlanFilter("C", ds)
        End If

        ds.Tables(ds.RATEPLAN_TABLE).Columns.Add("texto", GetType(System.String), ds.FIELD_CODIGOTARIFA & " + ' -' + " & ds.FIELD_NAME)

        'For j As Integer = 0 To ds.Tables(ds.RATEPLAN_TABLE).Rows.Count - 1
        '    Me.SourceRateName &= "//" & ds.Tables(ds.RATEPLAN_TABLE).Rows(j).Item(ds.FIELD_NAME).ToString
        '    lblDescRatePlan.Text = "" & ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0)(ds.FIELD_NAME)
        'Next

        ddlrateplans.DataTextField = "texto" 'ds.FIELD_CODIGOTARIFA
        ddlrateplans.DataValueField = ds.FIELD_IDRATEPLAN
        ddlrateplans.DataSource = ds
        ddlrateplans.DataBind()
        ddlrateplans.Items.Insert(0, PortalCulture.GetString("00172"))
        ddlrateplans.Items(0).Value = ""

    End Sub

    Private Sub loaddatos()
        Dim ds As DataSet, dg As New DataTable
        Dim idroom As Integer, idrp As String, code As String
        With New FaresSystem
            ds = .GetFaresListbyDate(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), Me.cInfoActual.Hotel, Me.ddlrateplans.SelectedValue, Me.ddlRooms.SelectedValue)
        End With
        dg.Columns.Add(RoomsHotelData.FLD_ID_ROOM_HOTEL, GetType(System.Int32))
        dg.Columns.Add(RoomsHotelData.FLD_ROOM_CODE, GetType(System.String))
        dg.Columns.Add(FaresData.STARTDATE_FIELD, GetType(System.DateTime))
        dg.Columns.Add(FaresData.ENDDATE_FIELD, GetType(System.DateTime))
        dg.Columns.Add(RatePlanData.FIELD_IDRATEPLAN, GetType(System.String))
        If Not ds Is Nothing Then


        For _rooms As Integer = 1 To ddlRooms.Items.Count - 1
            idroom = ddlRooms.Items(_rooms).Value

            If ddlRooms.SelectedValue <> 0 Then
                idroom = ddlRooms.SelectedValue
            End If
            For _rp As Integer = 1 To ddlrateplans.Items.Count - 1
                idrp = ddlrateplans.Items(_rp).Value
                code = ddlrateplans.Items(_rp).Text
                If ddlrateplans.SelectedValue <> "" Then
                    idrp = ddlrateplans.SelectedValue
                    code = ddlrateplans.Items(ddlrateplans.SelectedIndex).Text
                End If

                Dim dv As DataView = ds.Tables(0).DefaultView
                dv.RowFilter = RoomsHotelData.FLD_ID_ROOM_HOTEL & "=" & idroom & " and " & RatePlanData.FIELD_IDRATEPLAN & "='" & idrp & "'" & " and adultos=1 and ninios=0"
                If dv.Count > 0 Then
                    dv.Sort = FaresData.STARTDATE_FIELD & " asc"
                    Dim d1 As Date = CDate(Me.txtDateFrom.Text)
                    For i As Integer = 0 To dv.Count - 1
                        If d1 < dv(i)(FaresData.STARTDATE_FIELD) Then
                            addrow(dg, idroom, dv(i)(RoomsHotelData.FLD_ROOM_CODE), d1, CDate(dv(i)(FaresData.STARTDATE_FIELD).ToString).AddDays(-1), dv(i)(RatePlanData.FIELD_CODIGOTARIFA).ToString)
                        Else
                        End If
                        d1 = CDate(dv(i)(FaresData.ENDDATE_FIELD).ToString).AddDays(1)
                    Next
                    If dv.Count > 0 AndAlso dv(dv.Count - 1)(FaresData.ENDDATE_FIELD) < CDate(Me.txtDateTo.Text) Then
                        addrow(dg, idroom, dv(0)(RoomsHotelData.FLD_ROOM_CODE), CDate(dv(dv.Count - 1)(FaresData.ENDDATE_FIELD).ToString).AddDays(1), CDate(Me.txtDateTo.Text), dv(dv.Count - 1)(RatePlanData.FIELD_CODIGOTARIFA).ToString)
                    End If
                Else
                    If ddlRooms.SelectedValue <> 0 Then
                        addrow(dg, idroom, ddlRooms.SelectedItem.Text, CDate(Me.txtDateFrom.Text), CDate(Me.txtDateTo.Text), code)
                    Else
                        addrow(dg, idroom, ddlRooms.Items(_rooms).Text, CDate(Me.txtDateFrom.Text), CDate(Me.txtDateTo.Text), code)
                    End If

                End If
                If ddlrateplans.SelectedValue <> "" Then
                    Exit For
                End If
            Next

            If ddlRooms.SelectedValue <> 0 Then
                Exit For
            End If
            Next
        End If
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        dgnorates.DataSource = dg
        dgnorates.DataBind()
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub


    Private Sub addrow(ByRef ds As DataTable, ByVal idroom As Integer, ByVal coderoom As String, ByVal f1 As Date, ByVal f2 As Date, ByVal idrateplan As String)
        Dim dr As DataRow = ds.NewRow
        dr(RoomsHotelData.FLD_ID_ROOM_HOTEL) = idroom
        dr(RoomsHotelData.FLD_ROOM_CODE) = coderoom
        dr(FaresData.STARTDATE_FIELD) = f1
        dr(FaresData.ENDDATE_FIELD) = f2
        dr(RatePlanData.FIELD_IDRATEPLAN) = idrateplan
        ds.Rows.Add(dr)
    End Sub


    Private Sub btnload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnload.Click
        loaddatos()
    End Sub

    Private Sub dgnorates_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgnorates.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            e.Item.Cells(2).Text = CDate(e.Item.Cells(2).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(3).Text = CDate(e.Item.Cells(3).Text).ToString("MMM/dd/yyyy")
        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text() = PortalCulture.GetString("00170")
            e.Item.Cells(1).Text() = PortalCulture.GetString("00006")
            e.Item.Cells(2).Text() = PortalCulture.GetString("00108")
            e.Item.Cells(3).Text() = PortalCulture.GetString("00109")
        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblstart.Text = PortalCulture.GetString("00108", True)
        Me.lblEnd.Text = PortalCulture.GetString("00109", True)
        Me.lblrateplan.Text = PortalCulture.GetString("00016", True)
        Me.lblroom.Text = PortalCulture.GetString("00170", True)
        Me.btnload.Text = PortalCulture.GetString("00149")
        Me.lblTitle.Text = PortalCulture.GetString("00173")
        Me.dgnorates.Columns(0).HeaderText = PortalCulture.GetString("00170")
        Me.dgnorates.Columns(1).HeaderText = PortalCulture.GetString("00006")
        Me.dgnorates.Columns(2).HeaderText = PortalCulture.GetString("00108")
        Me.dgnorates.Columns(3).HeaderText = PortalCulture.GetString("00109")
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.Tarifas, Me.btnload, "R")

        Dim sFiltro As String
        sFiltro = PortalCulture.GetString("00202") & ","
        sFiltro &= String.Format(PortalCulture.GetString("01374"), txtDateFrom.Text, txtDateTo.Text)
        sFiltro &= If(ddlRooms.SelectedIndex = 0, PortalCulture.GetString("01375"), String.Format(PortalCulture.GetString("01376"), ddlRooms.SelectedItem.Text))
        sFiltro &= If(ddlrateplans.SelectedIndex = 0, PortalCulture.GetString("01384"), String.Format(PortalCulture.GetString("01383"), ddlrateplans.SelectedItem.Text))
        lblHelpFiltro.Text = sFiltro.Replace(",,", "").Replace(", ,", "")
        If Not Me.IsPostBack Then
            loaddatos()
        End If

        'Me.ddlrateplans.Attributes.Add("onChange", "javascript:showRatePlan('" & Me.ddlrateplans.ClientID & "','" & Me.SourceRateName & "','" & lblDescRatePlan.ClientID & "')")
        '        Me.ddlRooms.Attributes.Add("onChange", "javascript:showRoomType('" & Me.ddlRooms.ClientID & "','" & Me.SourceName & "')")

    End Sub
End Class
