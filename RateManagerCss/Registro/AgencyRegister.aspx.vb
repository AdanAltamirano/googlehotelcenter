Imports Portal.General.Facade
Imports Portal.General.Common
Imports Portal.General.Common.Data

Imports Portal.TaskManager.Facade
Imports Portal.General.Rules
Imports System.Configuration.ConfigurationManager


Partial Public Class AgencyRegister
    Inherits PaginaBase

    Enum dgcolumns
        idempresa
        idagencia
        nombre
        sel_nombre
        ciudad
        pais
        contacto
        editar
        eliminar
    End Enum

    Private status As Byte
    Public idSegmento As String = AppSettings("IdSegmento")

    Private Property Edit() As Boolean
        Get
            Return ViewState("_EditEmpresa")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("_EditEmpresa") = Value
        End Set
    End Property

    Private Property idempresa() As Integer
        Get
            Return ViewState("_IdEmpresa")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_IdEmpresa") = Value
        End Set
    End Property

    Private Property idhotel() As Integer
        Get
            Return ViewState("_IdHotel")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_IdHotel") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Introducir aquí el código de usuario para inicializar la página
        If String.IsNullOrEmpty(AppSettings("IdSegmento")) Then MyBase.redirectTo(pages.Home)

        If Not IsPostBack Then
            LoadAgencies("")
            AgenciasModulo1.ididioma = PortalCulture.GetIDCulture()
            Edit = False
            If Not Request.QueryString("idempresa") Is Nothing AndAlso Not Request.QueryString("sGuid") Is Nothing AndAlso Not Request.QueryString("idhotel") Is Nothing Then
                idempresa = Request.QueryString("idempresa")
                idhotel = Request.QueryString("idhotel")

            End If
        End If
        'If status = 2 Then
        '    Me.cmbPublish.Visible = False
        'End If

        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))
    End Sub

    Private Sub AgencyRegister_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        lblTitulo.Text = PortalCulture.GetString("01526")
        Me.cmdCancelar.Text = PortalCulture.GetString("A00143", False) 'Cancelar
        BtnNuevo.Text = PortalCulture.GetString("00102", False) 'Nuevo
        cmdAceptar.Text = PortalCulture.GetString("A00153", False) 'Guardar
        cmdNew.Value = PortalCulture.GetString("00102", False) 'Nuevo
        'Me.cmbPublish.Text = PortalCulture.GetString("A00153")
        grid.Columns(dgcolumns.sel_nombre).HeaderText = PortalCulture.GetString("00249")
        grid.Columns(dgcolumns.ciudad).HeaderText = PortalCulture.GetString("00254")
        grid.Columns(dgcolumns.pais).HeaderText = PortalCulture.GetString("00251")
        grid.Columns(dgcolumns.contacto).HeaderText = PortalCulture.GetString("00257")
    End Sub

    Protected Sub BtnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnNuevo.Click
        'Response.Redirect("AgencyRegister.aspx", True)
        AgenciasModulo1.Clear()
        AgenciasModulo1.Editing=False
    End Sub

    Protected Sub cmdCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancelar.Click
        AgenciasModulo1.Visible = True
        cmdCancelar.Visible = False
        AgenciasModulo1.Clear()
        AgenciasModulo1.Editing=False
    End Sub

    

    Protected Sub cmdAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAceptar.Click
        If Page.IsValid Then
            Dim status As Integer
            AgenciasModulo1.Visible = True
            cmdCancelar.Visible = False
            AgenciasModulo1.showError(False)
            If AgenciasModulo1.Editing = False Then
                If AgenciasModulo1.Add(0, status) Then
                    AgenciasModulo1.Clear()
                    LoadAgencies(ctrlAutoComplete1.GetFilter)
                    MostrarCmdNew(True)
                Else
                    AgenciasModulo1.showError(True, status)
                    MostrarCmdNew(False)
                End If
            Else
                If Not AgenciasModulo1.Update(idempresa) Then
                    AgenciasModulo1.showError(True)
                    MostrarCmdNew(False)
                Else
                    lblError.Visible = False
                    MostrarCmdNew(True)
                    LoadAgencies(ctrlAutoComplete1.GetFilter)
                End If
            End If

        End If
    End Sub

    'Protected Sub cmbPublish_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmbPublish.Click
    '    If Page.IsValid Then

    '        AgenciasModulo1.Visible = True
    '        cmdCancelar.Visible = False
    '        AgenciasModulo1.showError(False)
    '        If Edit = False Then
    '            Dim Newidempresa As Integer = 0
    '            If AgenciasModulo1.Add(Newidempresa) Then
    '                With New EmpresaSistema
    '                    If Not .CompanyCompleted(Newidempresa) Then
    '                        AgenciasModulo1.showError(True)
    '                    Else
    '                        With New RequestSystem
    '                            Dim x As Integer = .CompanyRegistrationApproved(Newidempresa)
    '                        End With
    '                        AgenciasModulo1.Clear()
    '                        AgenciasModulo1.showExito(PortalCulture.GetString("01527"))
    '                    End If

    '                End With


    '            Else
    '                AgenciasModulo1.showError(True)
    '            End If
    '        Else

    '        End If
    '    End If
    'End Sub

    Sub LoadAgencies(ByVal sFiltro As String)
        Dim ds As Portal.General.Common.Data.EmpresaDatos
        Dim em As New EmpresaSistema
        Dim idSegmento As Integer
        Dim dv As DataView
        If AppSettings("IdSegmento") Is Nothing Then
            Trace.Write("no tiene segmento")
        Else
            Trace.Write("no tiene segmento: " & AppSettings("IdSegmento"))

        End If
        Trace.Write("1")
        Integer.TryParse(AppSettings("IdSegmento"), idSegmento)
        Trace.Write("2")
        ds = em.GetCompanyAgencyByIdSegment(idSegmento)
        Trace.Write("3")
        If Not Me.dsEmpty(ds) Then
            Trace.Write("4")
            dv = ds.Tables(0).DefaultView
            Trace.Write("5 " & sFiltro)
            dv.RowFilter = sFiltro
            Trace.Write("6")
            grid.DataSource = dv
            Trace.Write("7")
            grid.DataKeyField = EmpresaDatos.FIELD_idEmpresa
            Trace.Write("8")
            grid.DataBind()
            Trace.Write("9")
        End If
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancel.Click
        Me.AgenciasModulo1.Editing = False
        'Me.ctrrateplan1.edicion = False
        lblError.Visible = False
        lblErrorSource.Visible = False
        Me.AgenciasModulo1.Clear()
        'Me.ctrrateplan1.ClearData()
        'Me.ctrrateplan1.clearConfDealData()
        Me.grid.SelectedIndex = -1
        'Cerror = 0
        MostrarCmdNew(True)
    End Sub
    Sub MostrarCmdNew(ByVal show As Boolean)
        cmdNew.Style("display") = IIf(show, "block", "none")
        divContenedor.Style("display") = IIf(show, "none", "block")
    End Sub

    Protected Sub grid_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grid.ItemCommand
        'Cerror = 0
        Me.lblError.Visible = False
        Me.lblErrorSource.Visible = False

        If e.CommandName = "Select" Then
            Me.AgenciasModulo1.Clear()
            'Me.ctrrateplan1.ClearData()
            Me.AgenciasModulo1.Editing = True
            'Me.ctrrateplan1.edicion = True
            'Me.AgenciasModulo1.ID = grid.DataKeys(e.Item.ItemIndex)
            'ctrrateplan1.IdRatePlan = grid.DataKeys(e.Item.ItemIndex)
            Me.AgenciasModulo1.loadCompany(grid.DataKeys(e.Item.ItemIndex))
            idempresa = grid.DataKeys(e.Item.ItemIndex)
            Me.cmdAceptar.Enabled = True
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
            MostrarCmdNew(False)
        ElseIf e.CommandName = "Delete" Then
            'Cerror = 0


            If New Portal.General.Facade.EmpresaSistema().CompanyLogicDelete(grid.DataKeys(e.Item.ItemIndex)) Then
                'ctrrateplan1.eliminarPortales(Me.cInfoActual.Hotel, grid.DataKeys(e.Item.ItemIndex)) ' no se k ondas cone esto...

                Me.guardalog("/Pages/AgencyRegister.aspx", PaginaBase.acciones.Eliminar, "Eliminó la agencia con el id " & Me.grid.Items(e.Item.ItemIndex).Cells(dgcolumns.idempresa).Text)
                If grid.CurrentPageIndex > 0 And grid.Items.Count = 1 Then
                    grid.CurrentPageIndex = ((grid.CurrentPageIndex * grid.PageSize) \ grid.PageSize) - 1
                End If
                'Eliminamos la oferta
                '.DelRateRatePlanDeal(grid.DataKeys(e.Item.ItemIndex), Me.cInfoActual.Hotel)
                AgenciasModulo1.Clear()
                LoadAgencies(ctrlAutoComplete1.GetFilter)
                Me.grid.SelectedIndex = -1
                lblError.Visible = False
                lblErrorSource.Visible = False
                MostrarCmdNew(True)
            Else
                lblError.Visible = True
            End If


            'With New EmpresaSistema

            '    'If .LogicDeleteRatePlan(Me.cInfoActual.Hotel, grid.DataKeys(e.Item.ItemIndex)) Then
            '    '    'ctrrateplan1.eliminarPortales(Me.cInfoActual.Hotel, grid.DataKeys(e.Item.ItemIndex)) ' no se k ondas cone esto...

            '    '    Me.guardalog("/Pages/RatesPlans.aspx", PaginaBase.acciones.Eliminar, "Eliminó el rateplan con el id " & Me.grid.Items(e.Item.ItemIndex).Cells(dgcolumns.idrateplan).Text & " y el codigo de tarifa " & Me.grid.Items(e.Item.ItemIndex).Cells(dgcolumns.codigotarifa).Text)
            '    '    If grid.CurrentPageIndex > 0 And grid.Items.Count = 1 Then
            '    '        grid.CurrentPageIndex = ((grid.CurrentPageIndex * grid.PageSize) \ grid.PageSize) - 1
            '    '    End If
            '    '    'Eliminamos la oferta
            '    '    '.DelRateRatePlanDeal(grid.DataKeys(e.Item.ItemIndex), Me.cInfoActual.Hotel)
            '    '    ctrrateplan1.clearConfDealData()
            '    '    loadrateplans(ctrlAutoComplete1.GetFilter)
            '    '    Me.grid.SelectedIndex = -1
            '    '    Me.ctrrateplan1.ClearData()
            '    '    lblError.Visible = False
            '    '    lblErrorSource.Visible = False
            '    '    MostrarCmdNew(True)
            '    Else
            '    'Cerror = 6
            '    lblError.Visible = True
            '    End If
            'End With
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
        End If
    End Sub

    Private Sub ctrlAutoComplete1_onSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        grid.CurrentPageIndex = 0
        LoadAgencies(descripcion)
    End Sub

    Private Sub grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then

            Dim LK As HyperLink
            Dim LK2 As LinkButton


            LK2 = e.Item.Cells(dgcolumns.editar).FindControl("lnkedit")
            LK2.Text = PortalCulture.GetString("00093")



            'e.Item.Cells(dgcolumns.eliminar).Text = ""
            LK2 = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkEliminar2")
            LK = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkEliminar")
            LK.Text = PortalCulture.GetString("00103")
            'LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), _
            'PortalCulture.GetString("00613") & ", " & PortalCulture.GetString("00464"))

            LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), PortalCulture.GetString("01555"))


        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.sel_nombre).Text = PortalCulture.GetString("00249")
            e.Item.Cells(dgcolumns.ciudad).Text = PortalCulture.GetString("00254")
            e.Item.Cells(dgcolumns.pais).Text = PortalCulture.GetString("00251")
            e.Item.Cells(dgcolumns.contacto).Text = PortalCulture.GetString("00257")
            'ElseIf e.Item.ItemType = ListItemType.Footer Then
            '    e.Item.Cells(dgcolumns.eliminar).Text = CType(grid.DataSource, DataView).Count & " " & PortalCulture.GetString("00047")
        End If
    End Sub

    Protected Sub grid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grid.PageIndexChanged
           Me.grid.CurrentPageIndex = e.NewPageIndex
        Me.grid.SelectedIndex = -1
        LoadAgencies(ctrlAutoComplete1.GetFilter)
    End Sub
End Class