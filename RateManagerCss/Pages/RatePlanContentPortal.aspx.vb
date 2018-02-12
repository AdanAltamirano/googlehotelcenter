Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade

Partial Public Class RatePlanContentPortal
    Inherits PaginaBase

    Enum dgcolumns
        idrateplan
        orden
        code
        name
        segment
        edit
        principalSegmentRac
        codigotarifa
    End Enum

    Sub loadResources()
        lblTitle.Text = PortalCulture.GetString("01265")
        lblContPortal.Text = PortalCulture.GetString("01251")

        dgRateplan.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        dgRateplan.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        dgRateplan.Columns(dgcolumns.name).HeaderText = PortalCulture.GetString("00073")
        dgRateplan.Columns(dgcolumns.code).HeaderText = PortalCulture.GetString("00001")
        dgRateplan.Columns(dgcolumns.segment).HeaderText = PortalCulture.GetString("00003")

    End Sub

    Private Sub loadrateplans()
        Dim ds As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            If MyBase.IsSupervisor Then
                'Mostramos todos los rateplans incluidos los de tarifas netas.
                ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, 1, idAsociacion:=IdAsoc,DeleteFilter:=1)
            Else
                'Mostramos solamente los ratesplans que no sean de tarifas netas.
                ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, 0, idAsociacion:=idAsoc, DeleteFilter:=1)
            End If

        End With

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

        With dgRateplan
            'dsegmentos se utilizará en el databound            
            .DataKeyField = RatePlanData.FIELD_IDRATEPLAN
            .DataSource = ds
            .DataBind()
        End With

       
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            loadrateplans()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
        End If
        'Me.ResizefrmPrincipal()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.CtrRatePlanContain1.Clear()
        dgRateplan.SelectedIndex = -1
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
    End Sub

    Function LoadRatePlan(ByVal id As String)
        Dim dsRatePlan As RatePlanData
        Dim dato As String = String.Empty
        Dim idDicc As Integer
        Dim isvalid As Boolean = False
        Dim idHotel As Integer
        Dim hr As Boolean = False

        idHotel = CType(Me.Page, PaginaBase).cInfoActual.Hotel
        With New RatePlanFacade
            dsRatePlan = .GetDataRatePlan(ID, idHotel)
        End With
        If dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows.Count > 0 Then
            With dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows(0)
                If Not .IsNull(dsRatePlan.FIELD_IDDICDESC) Then
                    idDicc = .Item(dsRatePlan.FIELD_IDDICDESC)
                Else
                    idDicc = 0
                End If
                hr = CtrRatePlanContain1.LoadContainPortal(idHotel, idDicc, .Item(dsRatePlan.FIELD_IDRATEPLAN), .Item(dsRatePlan.FIELD_CODIGOTARIFA), isvalid)
            End With

            If Not hr Then
                lblError.Visible = True
                lblError.Text = PortalCulture.GetString("01264")
            End If
        End If
    End Function

    Private Sub dgRatePlans_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRateplan.ItemCommand
        Dim sData As String = ""
        Dim sDataPrev As String = ""

        Me.lblError.Visible = False
        Me.lblErrorSource.Visible = False
        If e.CommandName = "Select" Then
            'Me.ctrrateplan1.ClearData()
            'Me.ctrrateplan1.edicion = True
            'ctrrateplan1.IdRatePlan = dgRatePlans.DataKeys(e.Item.ItemIndex)
            If e.Item.Cells(dgcolumns.principalSegmentRac).Text.ToUpper = "TRUE" Then

                '   ctrrateplan1.loadRatePlan(dgRatePlans.DataKeys(e.Item.ItemIndex), True)
            Else
                '  ctrrateplan1.loadRatePlan(dgRatePlans.DataKeys(e.Item.ItemIndex), False)
            End If
            LoadRatePlan(dgRateplan.DataKeys(e.Item.ItemIndex))
            Me.btnSave.Enabled = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
        ElseIf e.CommandName = "Delete" Then
            lblError.Visible = False
            ''''Dim drPlan As DataRow ''''''''
            ' Dim dsRatePlan As RatePlanData
            Dim room As RoomsHotelData
            Dim fares As FaresData
            Dim drRoom As DataRow
            Dim drFare As DataRow
            ''''''''''''''''''''''''''''''''''
            With New RoomFacade
                room = .getRooms(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
                'getAllRooms = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL)
            End With
            sDataPrev = room.GetXml
            ''''''''''' codigo para borrar las tarifas ''''''''''''''''''''''''''''''''''''''
            With New FaresSystem
                For Each drRoom In room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows
                    fares = .GetFaresByHotelRoomType(drRoom(RoomsHotelData.FLD_ID_ROOM_HOTEL))

                    For Each drFare In fares.Tables(FaresData.FARES_TABLE).Rows
                        If Not drFare.IsNull(FaresData.IDRATEPLAN_FIELD) AndAlso drFare(FaresData.IDRATEPLAN_FIELD) = dgRateplan.DataKeys(e.Item.ItemIndex) Then
                            .DeleteFares(drFare(FaresData.PKIDFARES_FIELD))
                        End If
                    Next

                Next
            End With
            Dim lockrpr As LockRatesPlanRoomData
            Dim drLrpr As DataRow
            Dim strerr As String = ""

            With New LockRatesPlanRoomAccess

                lockrpr = .GetList(Me.cInfoActual.Hotel, Date.MinValue, Date.MaxValue, strerr)
                If Not lockrpr Is Nothing AndAlso Not lockrpr.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms) Is Nothing Then
                    For Each drLrpr In lockrpr.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows
                        If drLrpr(LockRatesPlanRoomData.FIELD_IDRATEPLAN) = dgRateplan.DataKeys(e.Item.ItemIndex) Then
                            drLrpr.Delete() 'Fares(drFare(fares.PKIDFARES_FIELD))
                        End If
                    Next

                    .UpdateLocks(lockrpr)
                End If

            End With

            'Dim dsLPlan As LinkRatePlanData
            'With New LinkRatePlanAccess
            '    dsLPlan = .GetList(Me.cInfoActual.Hotel)
            '    For Each drPlan In dsLPlan.Tables(dsLPlan.TABLE_LINKRATEPLAN).Rows
            '        .Deletelink(Me.cInfoActual.Hotel, drPlan(dsLPlan.FIELD_TargetRatePlan), dgRatePlans.DataKeys(e.Item.ItemIndex))
            '    Next
            'End With

            With New RatePlanFacade
                If .DeleteRatePlan(Me.cInfoActual.Hotel, dgRateplan.DataKeys(e.Item.ItemIndex)) Then
                    '''ctrrateplan1.eliminarPortales(Me.cInfoActual.Hotel, dgRatePlans.DataKeys(e.Item.ItemIndex))

                    Me.guardalog("/Pages/RatesPlans.aspx", PaginaBase.acciones.Eliminar, "Eliminó el rateplan con el id " & Me.dgRateplan.Items(e.Item.ItemIndex).Cells(dgcolumns.idrateplan).Text & " y el codigo de tarifa " & Me.dgRateplan.Items(e.Item.ItemIndex).Cells(dgcolumns.codigotarifa).Text, "", sDataPrev, sData)
                    If dgRateplan.CurrentPageIndex > 0 And dgRateplan.Items.Count = 1 Then
                        dgRateplan.CurrentPageIndex = ((dgRateplan.CurrentPageIndex * dgRateplan.PageSize) \ dgRateplan.PageSize) - 1
                    End If
                    'Eliminamos la oferta
                    .DelRateRatePlanDeal(dgRateplan.DataKeys(e.Item.ItemIndex), Me.cInfoActual.Hotel)
                    '''ctrrateplan1.clearConfDealData()
                    loadrateplans()
                    Me.dgRateplan.SelectedIndex = -1
                    ''' Me.ctrrateplan1.ClearData()

                Else
                    lblError.Visible = True
                End If
            End With
            'CtlMensajes1.Show(PortalCulture.GetString("00047"), PortalCulture.GetString("00464"), ctlMensajes.Tipos.Prompt)
        End If
    End Sub

    Private Sub dgRatePlans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRateplan.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            If e.Item.Cells(dgcolumns.orden).Text = "0" Or e.Item.Cells(dgcolumns.orden).Text = "100000" Then
                e.Item.Cells(dgcolumns.orden).Text = "---"
            End If

            '' e.Item.Cells(dgcolumns.segment).Text = GetSegmento(e.Item.Cells(dgcolumns.segment).Text)
            Dim LK2 As LinkButton

            LK2 = e.Item.Cells(dgcolumns.edit).FindControl("lnkedit")
            LK2.Text = PortalCulture.GetString("00093")
            LK2 = e.Item.Cells(dgcolumns.edit).FindControl("lnkEliminar2")
            ''' LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), _
            ''' PortalCulture.GetString("00613") & ", " & PortalCulture.GetString("00464"))
            If e.Item.Cells(dgcolumns.principalSegmentRac).Text.ToUpper = "TRUE" Then
            End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.name).Text = PortalCulture.GetString("00073")
            e.Item.Cells(dgcolumns.segment).Text = PortalCulture.GetString("00003")
            e.Item.Cells(dgcolumns.code).Text = PortalCulture.GetString("00001")
            e.Item.Cells(dgcolumns.orden).Text = PortalCulture.GetString("00920")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(dgcolumns.edit).Text = CType(dgRateplan.DataSource, DataSet).Tables(RatePlanData.RATEPLAN_TABLE).Rows.Count & " " & PortalCulture.GetString("00047")
        End If
    End Sub

    Private Sub dgRatePlans_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRateplan.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dgRateplan.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgRateplan.CurrentPageIndex < dgRateplan.PageCount - 1 Then
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

    Private Sub dgRatePlans_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRateplan.PageIndexChanged
        Me.dgRateplan.CurrentPageIndex = e.NewPageIndex
        Me.dgRateplan.SelectedIndex = -1
        loadrateplans()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
    End Sub

    Private Sub RatePlanContentPortal_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        loadResources()
    End Sub

End Class