Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade
Partial Class RatesPlans
    Inherits PaginaBase
    Dim dsegmentos As DataSet
    Dim dsRatesPlan As DataSet
    Protected WithEvents ctrrateplan1 As ctrRatePlan
    Protected WithEvents CtlMensajes1 As ctlMensajes
    Protected WithEvents CtlMensajes2 As ctlMensajes
    Protected WithEvents CtlMensajes3 As ctlMensajes
    Enum dgcolumns
        idrateplan
        orden
        code
        name
        segment
        edit
        eliminar
        activar
        principalSegmentRac
        codigotarifa
        deleted
    End Enum

    Private Property Cerror() As Integer
        Get
            Return viewstate("_cerror")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_cerror") = Value
        End Set
    End Property

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

    Sub MostrarCmdNew(ByVal show As Boolean)
        cmdNew.Style.Add("display", IIf(show, "block", "none"))
        divContenedor.Style.Add("display", IIf(show, "none", "block"))
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página        
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If MyBase.IsSupervisor Or MyBase.isUserChain Or MyBase.IsUsuarioHotelAssociation Then
            ctrrateplan1.Supervisor = True
        Else
            ctrrateplan1.Supervisor = False
        End If

        If Not IsPostBack Then
            Cerror = 0            
            ctrrateplan1.m_iHotelId = Me.cInfoActual.Hotel
            loadrateplans("")
            Me.ctrrateplan1.edicion = False
            lblErrorSource.Visible = False
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
            MostrarCmdNew(True)

            ddlDeletedFilter.Items.Clear()
            ddlDeletedFilter.Items.Add(New ListItem(PortalCulture.GetString("01541"), 1))
            ddlDeletedFilter.Items.Add(New ListItem(PortalCulture.GetString("01542"), 0))
            ddlDeletedFilter.Items.Add(New ListItem(PortalCulture.GetString("01543"), -1))
        End If        
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))
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

    Private Sub loadrateplans(ByVal sFiltro As String)
        Dim ds As RatePlanData
        Dim idAsoc As Integer = GetIdAsociation()
        Dim dv As DataView

        With New RatePlanFacade
            If MyBase.IsUsuarioHomeAgency Then
                ds = .GetRatePlanByConvenio(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, 1, MyBase.Usuario, idAsociacion:=idAsoc, DeleteFilter:=Integer.Parse(ddlDeletedFilter.SelectedValue))
            Else
                ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, 1, idAsociacion:=idAsoc, DeleteFilter:=Integer.Parse(ddlDeletedFilter.SelectedValue))

                'Se comentó para que se muestren todos los rates plan, ya que los hoteles requieren manipular información aunque sean netrate. Solo se les restringe la parte de los contratos
                'If MyBase.IsSupervisor Or MyBase.IsUsuarioHotelAssociation Then
                '    'Mostramos todos los rateplans incluidos los de tarifas netas.
                '    ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, 1, idAsociacion:=idAsoc, DeleteFilter:=Integer.Parse(ddlDeletedFilter.SelectedValue))
                'Else
                '    'Mostramos solamente los ratesplans que no sean de tarifas netas.
                '    ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, 0, idAsociacion:=idAsoc, DeleteFilter:=Integer.Parse(ddlDeletedFilter.SelectedValue))
                'End If
            End If

        End With

        If MyBase.IdCorporativoUserChain = 4 AndAlso (MyBase.IsHotel Or MyBase.IsUsuarioHotel) Then
            ds = RatePlanFilter("C", ds)
        End If

        Dim dr As DataRow
        For Each dr In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            'If dr(ds.FIELD_SEGMENT) = "K" Or dr(ds.FIELD_SEGMENT) = "I" Then
            '    dr.Delete()
            'End If
            If dr(RatePlanData.FIELD_SEGMENT) = "I" Then
                dr.Delete()
            End If
        Next

        ds.AcceptChanges()

        With grid
            dv = ds.Tables(0).DefaultView
            'dv.RowFilter = sFiltro
            dv.RowFilter = "isPromo is null " & IIf(sFiltro = "", "", " and " & sFiltro)
            'dsegmentos se utilizará en el databound
            dsegmentos = New DataSet
            dsegmentos.ReadXml(Server.MapPath(Request.ApplicationPath & "/Data/Segmentos.xml"))
            .DataKeyField = RatePlanData.FIELD_IDRATEPLAN
            .DataSource = ds
            .DataSource = dv
            .DataBind()
        End With

        If ds.Tables(0).Rows.Count > 0 Then
            dsRatesPlan = New DataSet
            dsRatesPlan.Merge(ds)
            ctrrateplan1.totalRatePlan = ds.Tables(0).Rows.Count
        Else
            ctrrateplan1.totalRatePlan = 0
        End If

        ctrrateplan1.cargarOrden()
    End Sub


    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPublish.Click
        If Not Page.IsValid Then
            MostrarCmdNew(False)
            Return
        End If


        Dim publish As Boolean = (CType(sender, Button).ID = Me.btnPublish.ID)

        lblError.Visible = False
        lblErrorSource.Visible = False
        If ctrrateplan1.isSourceSelected() Then
            Cerror = ctrrateplan1.SavePlan(publish)
            If Cerror <> 0 Then
                lblError.Visible = True
                MostrarCmdNew(False)
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
            Else
                grid.SelectedIndex = -1
                loadrateplans(ctrlAutoComplete1.GetFilter)
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
                MostrarCmdNew(True)
            End If
        Else
            lblErrorSource.Text = ctrrateplan1.descripcionError
            lblErrorSource.Visible = True
            MostrarCmdNew(False)
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If ctrrateplan1.edicion = False Then
            CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlan, btnSave, "A")
        End If
        Dim LK As HyperLink
        Dim lk2 As LinkButton
        For Each i As DataGridItem In Me.grid.Items
            If i.ItemType = ListItemType.AlternatingItem Or i.ItemType = ListItemType.Item Then
                LK = i.Cells(dgcolumns.eliminar).FindControl("lnkEliminar")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlan, LK, "D")
                lk2 = i.Cells(dgcolumns.code).FindControl("lnkedit")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlan, lk2, "M")
            End If
        Next
        loadResources()



        btnSave2.Style.Add("display", "none")
        'Script para el mensaje de tarifas netas
        If MyBase.IsSupervisor Or MyBase.IsUsuarioHotelAssociation Then
            Dim script As String = String.Empty
            Dim tcId As String = ctrrateplan1.tarifaComisionableId
            Dim cNr As String = ctrrateplan1.ContratosNRId
            Dim scriptMsg As String = CtlMensajes2.getShow(btnSave2.ClientID, PortalCulture.GetString("00047"), PortalCulture.GetString("01145"))
            scriptMsg = scriptMsg.Split(":")(1)
            Dim scriptvalidahora As String = ctrrateplan1.validaHora()
            script = "javascript:if ((document.getElementById('" & cNr & "').selectedIndex != 0) && (document.getElementById('" & tcId & "').value == '1')) { " & scriptMsg & ";window.scrollTo(0,0); return false; } return " & scriptvalidahora
            'script = "javascript:if ((document.getElementById('" & cNr & "').selectedIndex != 0) && (document.getElementById('" & tcId & "').value == '1')) { document.getElementById('" & lblMsgRatePlan.ClientID & "').style.display='';}else{document.getElementById('" & lblMsgRatePlan.ClientID & "').style.display='none';}"
            btnSave.Attributes.Add("onClick", script)
            PortalCulture.GetIDCulture()

            'ctrrateplan1.SetMsgTarifasCom(script)            
        End If
    End Sub
    Private Sub loadResources()
        cmdNew.Value = PortalCulture.GetString("00102")
        Me.btncancel.Text = PortalCulture.GetString("00009")
        Me.btnSave.Text = PortalCulture.GetString("00008")
        grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        grid.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        Me.lblTitle.Text = PortalCulture.GetString("00047")
        grid.Columns(dgcolumns.name).HeaderText = PortalCulture.GetString("00073")
        grid.Columns(dgcolumns.code).HeaderText = PortalCulture.GetString("00001")
        grid.Columns(dgcolumns.segment).HeaderText = PortalCulture.GetString("00003")
        '//Me.lblErrorSource.Text = PortalCulture.GetString("00324")
        Select Case Cerror
            Case 1
                lblError.Text = PortalCulture.GetString("00308")
            Case 2
                lblError.Text = PortalCulture.GetString("00127")
            Case 3
                lblError.Text = PortalCulture.GetString("00309")
            Case 4
                lblError.Text = "ya hay un rateplan con ese plan tarifario y ese segmento"
            Case 5
                lblError.Text = PortalCulture.GetString("00344")
            Case 6
                lblError.Text = PortalCulture.GetString("00466")
            Case 7
                'Codigo de error para la comsion GDS
                lblError.Text = PortalCulture.GetString("01043")
            Case 8
                'Codigo de error para la comision Portal
                lblError.Text = PortalCulture.GetString("01044")
            Case 9
                'Codigo de error para la comision One Page
                lblError.Text = PortalCulture.GetString("01045")
            Case 10
                'Codigo de error para la comision de ADS
                lblError.Text = PortalCulture.GetString("01046")
            Case 11
                lblError.Text = PortalCulture.GetString("01444")
            Case 12
                lblError.Text = PortalCulture.GetString("01545")
        End Select
        If Me.ctrrateplan1.edicion = True Then
            lblMsg.Text = PortalCulture.GetString("00013")
        Else
            lblMsg.Text = PortalCulture.GetString("00012")
        End If
        lblFilter.Text = PortalCulture.GetString("01100")
        
    End Sub

    Private Sub dgRatePlans_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grid.PageIndexChanged
        Me.grid.CurrentPageIndex = e.NewPageIndex
        Me.grid.SelectedIndex = -1
        loadrateplans(ctrlAutoComplete1.GetFilter)
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
    End Sub


    Private Sub dgRatePlans_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grid.ItemCommand
        Cerror = 0
        Me.lblError.Visible = False
        Me.lblErrorSource.Visible = False

        If e.CommandName = "Select" Then
            Me.ctrrateplan1.ClearData()
            Me.ctrrateplan1.edicion = True
            ctrrateplan1.IdRatePlan = grid.DataKeys(e.Item.ItemIndex)
            If e.Item.Cells(dgcolumns.principalSegmentRac).Text.ToUpper = "TRUE" Then
                ctrrateplan1.loadRatePlan(grid.DataKeys(e.Item.ItemIndex), True)
            Else
                ctrrateplan1.loadRatePlan(grid.DataKeys(e.Item.ItemIndex), False)
            End If
            Me.btnSave.Enabled = True
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
            MostrarCmdNew(False)
        ElseIf e.CommandName = "Delete" Then
            Cerror = 0
            lblError.Visible = False
            ''''Dim drPlan As DataRow ''''''''
            ' Dim dsRatePlan As RatePlanData
            'Dim room As RoomsHotelData
            'Dim fares As FaresData
            'Dim drRoom As DataRow
            'Dim drFare As DataRow
            '''''''''''''''''''''''''''''''''''
            'With New RoomFacade
            '    room = .getRooms(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
            '    'getAllRooms = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL)
            'End With
            '''''''''''' codigo para borrar las tarifas ''''''''''''''''''''''''''''''''''''''
            'With New FaresSystem
            '    For Each drRoom In room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows
            '        fares = .GetFaresByHotelRoomType(drRoom(RoomsHotelData.FLD_ID_ROOM_HOTEL))

            '        For Each drFare In fares.Tables(FaresData.FARES_TABLE).Rows
            '            If Not drFare.IsNull(FaresData.IDRATEPLAN_FIELD) AndAlso drFare(FaresData.IDRATEPLAN_FIELD) = grid.DataKeys(e.Item.ItemIndex) Then
            '                .DeleteFares(drFare(FaresData.PKIDFARES_FIELD))
            '            End If
            '        Next

            '    Next
            'End With
            'Dim lockrpr As LockRatesPlanRoomData
            'Dim drLrpr As DataRow
            'Dim strerr As String = ""

            'With New LockRatesPlanRoomAccess

            '    lockrpr = .GetList(Me.cInfoActual.Hotel, Date.MinValue, Date.MaxValue, strerr)
            '    If Not lockrpr Is Nothing AndAlso Not lockrpr.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms) Is Nothing Then
            '        For Each drLrpr In lockrpr.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows
            '            If drLrpr(LockRatesPlanRoomData.FIELD_IDRATEPLAN) = grid.DataKeys(e.Item.ItemIndex) Then
            '                drLrpr.Delete() 'Fares(drFare(fares.PKIDFARES_FIELD))
            '            End If
            '        Next

            '        .UpdateLocks(lockrpr)
            '    End If

            'End With



            With New RatePlanFacade
                If .LogicDeleteRatePlan(Me.cInfoActual.Hotel, grid.DataKeys(e.Item.ItemIndex)) Then
                    'ctrrateplan1.eliminarPortales(Me.cInfoActual.Hotel, grid.DataKeys(e.Item.ItemIndex)) ' no se k ondas cone esto...

                    Me.guardalog("/Pages/RatesPlans.aspx", PaginaBase.acciones.Eliminar, "Eliminó el rateplan con el id " & Me.grid.Items(e.Item.ItemIndex).Cells(dgcolumns.idrateplan).Text & " y el codigo de tarifa " & Me.grid.Items(e.Item.ItemIndex).Cells(dgcolumns.codigotarifa).Text)
                    If grid.CurrentPageIndex > 0 And grid.Items.Count = 1 Then
                        grid.CurrentPageIndex = ((grid.CurrentPageIndex * grid.PageSize) \ grid.PageSize) - 1
                    End If
                    'Eliminamos la oferta
                    '.DelRateRatePlanDeal(grid.DataKeys(e.Item.ItemIndex), Me.cInfoActual.Hotel)
                    ctrrateplan1.clearConfDealData()
                    loadrateplans(ctrlAutoComplete1.GetFilter)
                    Me.grid.SelectedIndex = -1
                    Me.ctrrateplan1.ClearData()
                    lblError.Visible = False
                    lblErrorSource.Visible = False
                    MostrarCmdNew(True)
                Else
                    Cerror = 6
                    lblError.Visible = True
                End If
            End With
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
        ElseIf e.CommandName = "Active" Then
            With New RatePlanFacade
                If .LogicActiveRatePlan(Me.cInfoActual.Hotel, grid.DataKeys(e.Item.ItemIndex)) Then
                    ctrrateplan1.clearConfDealData()
                    loadrateplans(ctrlAutoComplete1.GetFilter)
                    Me.grid.SelectedIndex = -1
                    Me.ctrrateplan1.ClearData()
                    lblError.Visible = False
                    lblErrorSource.Visible = False
                    MostrarCmdNew(True)
                End If
            End With
        End If
    End Sub

    Private Function isLinked(ByVal idRatePlan As String) As Boolean
        Dim links As New LinkRatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New LinkRatePlanFacade
            links = .getList(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
        End With

        Dim dr() As DataRow = links.Tables(0).Select("SourceRatePlan = '" + idRatePlan + "'")

        If dr.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
   

    Private Function GetSegmento(ByVal Cod As String) As String
        Dim dv As DataView
        dv = dsegmentos.Tables("Segmento").DefaultView
        dv.RowFilter = "Code ='" & Cod & "'"
        If dv.Count > 0 Then
            Return dv(0)("Desc")
        Else
            Return Cod
        End If

    End Function

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.ctrrateplan1.edicion = False
        lblError.Visible = False
        lblErrorSource.Visible = False
        Me.ctrrateplan1.ClearData()
        Me.ctrrateplan1.clearConfDealData()
        Me.grid.SelectedIndex = -1
        Cerror = 0
        MostrarCmdNew(True)
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
    End Sub
    'Private Sub btnNew2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
    '    Me.ctrrateplan1.edicion = False
    '    lblError.Visible = False
    '    lblErrorSource.Visible = False
    '    Me.ctrrateplan1.ClearData()
    '    Me.ctrrateplan1.clearConfDealData()
    '    Me.grid.SelectedIndex = -1
    '    Cerror = 0
    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
    'End Sub
    
    Private Sub dgRatePlans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            If e.Item.Cells(dgcolumns.orden).Text = "0" Or e.Item.Cells(dgcolumns.orden).Text = "100000" Then
                e.Item.Cells(dgcolumns.orden).Text = "---"
            End If

            e.Item.Cells(dgcolumns.segment).Text = GetSegmento(e.Item.Cells(dgcolumns.segment).Text)
            Dim LK As HyperLink
            Dim LK2 As LinkButton


            LK2 = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkedit")
            LK2.Text = PortalCulture.GetString("00093")



            If e.Item.Cells(dgcolumns.deleted).Text.ToUpper() = "TRUE" Then 'DESACTIVAR
                e.Item.Cells(dgcolumns.eliminar).Text = ""
                e.Item.Cells(dgcolumns.orden).Text = ""

                LK2 = e.Item.Cells(dgcolumns.activar).FindControl("lnkActivar2")
                LK = e.Item.Cells(dgcolumns.activar).FindControl("lnkActivar")
                LK.Text = PortalCulture.GetString("01520")
                'LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), _
                '"" & ", " & PortalCulture.GetString("01544"))
                LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), PortalCulture.GetString("01544"))
                If ddlDeletedFilter.SelectedValue = "-1" Then
                    e.Item.Style("background-color") = "#FEE"
                    'e.Item.Style("border") = "solid 1px #F44"
                End If
            Else 'ACTIVAR
                If Not isLinked(e.Item.Cells(dgcolumns.code).Text) Then
                    e.Item.Cells(dgcolumns.activar).Text = ""
                    LK2 = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkEliminar2")
                    LK = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkEliminar")
                    LK.Text = PortalCulture.GetString("01521")
                    'LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), _
                    'PortalCulture.GetString("00613") & ", " & PortalCulture.GetString("00464"))

                    LK.NavigateUrl = CtlMensajes3.getShow(LK2.ClientID, PortalCulture.GetString("00047"), PortalCulture.GetString("01551"))

                    If e.Item.Cells(dgcolumns.principalSegmentRac).Text.ToUpper = "TRUE" Then
                        LK.Enabled = False
                    End If
                Else
                    e.Item.Cells(dgcolumns.eliminar).Text = PortalCulture.GetString("01588")
                End If
            End If

        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.name).Text = PortalCulture.GetString("00073")
            e.Item.Cells(dgcolumns.segment).Text = PortalCulture.GetString("00003")
            e.Item.Cells(dgcolumns.code).Text = PortalCulture.GetString("00001")
            e.Item.Cells(dgcolumns.orden).Text = PortalCulture.GetString("00920")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(dgcolumns.eliminar).Text = CType(grid.DataSource, DataView).Count & " " & PortalCulture.GetString("00047")
        End If
    End Sub

    Private Sub dgRatePlans_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If grid.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If grid.CurrentPageIndex < grid.PageCount - 1 Then
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
    Private Sub btnSave2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave2.Click
        Call btnSave_Click(sender, e)
    End Sub

    Private Sub ctrlAutoComplete1_onSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        grid.CurrentPageIndex = 0
        loadrateplans(descripcion)
    End Sub

    Protected Sub ddlDeletedFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDeletedFilter.SelectedIndexChanged
        ctrrateplan1.clearConfDealData()
        loadrateplans(ctrlAutoComplete1.GetFilter)
        Me.grid.SelectedIndex = -1
        Me.ctrrateplan1.ClearData()
        lblError.Visible = False
        lblErrorSource.Visible = False
        MostrarCmdNew(True)
    End Sub
End Class
