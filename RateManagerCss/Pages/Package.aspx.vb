Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade

Partial Class Package
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

    Enum dgcolumns
        code
        name
        edit
        eliminar
        idrateplan
        idpaquete
    End Enum

    Private Property Cerror() As Integer
        Get
            Return viewstate("_cerror")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_cerror") = Value
        End Set
    End Property

    Protected WithEvents CtrlPackage1 As ctrlPackage
    Protected WithEvents CtlMensajes1 As ctlMensajes

    Sub MostrarCmdNew(ByVal show As Boolean)
        cmdNew.Style.Add("display", IIf(show, "block", "none"))
        divContenedor.Style.Add("display", IIf(show, "none", "block"))
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

        CtrlPackage1.idCompany = MyBase.cInfoActual.Empresa

        If Not IsPostBack Then
            Cerror = 0
            'CtrlPackage1.m_iHotelId = Me.cInfoActual.Hotel
            loadpackages("")
            Me.CtrlPackage1.edicion = False
            lblErrorSource.Visible = False
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
            MostrarCmdNew(True)
        End If
        Me.ResizefrmPrincipal()
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))
    End Sub

    Private Sub loadpackages(ByVal sFilter As String)
        Dim ds As PackageData
        Dim dv As DataView
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New PackageFacade
            ds = .GetPackageByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
        End With

        With dgPackage
            dv = ds.Tables(0).DefaultView
            dv.RowFilter = sFilter
            'dsegmentos se utilizará en el databound
            .DataKeyField = ds.FIELD_RateCode
            .DataSource = dv
            .DataBind()
        End With
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPublish.Click
        If Not Page.IsValid Then
            divContenedor.Style.Add("display", "block")
            Return
        End If

        Dim publish As Boolean = (CType(sender, Button).ID = Me.btnPublish.ID)

        'RaiseEvent cmdValida.Click
        lblError.Visible = False
        lblErrorSource.Visible = False
        If CtrlPackage1.isSourceSelected() Then
            Cerror = CtrlPackage1.SavePlan(publish)
            If Cerror <> 0 Then
                If Cerror = -3 Then
                    lblErrorSource.Text = CtrlPackage1.descripcionError 'PortalCulture.GetString("01012")
                    lblErrorSource.Visible = True
                Else
                    lblError.Visible = True
                End If
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)                
            Else
                dgPackage.SelectedIndex = -1
                loadpackages(ctrlAutoComplete1.GetFilter)
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)

                'If CtrlPackage1.paqueteNuevo <> "" Then
                '    Me.CtrlPackage1.ClearData()
                '    Me.CtrlPackage1.edicion = True
                '    CtrlPackage1.IdRatePlan = CtrlPackage1.paqueteNuevo
                '    CtrlPackage1.idRateCode = CtrlPackage1.paqueteNuevo
                '    CtrlPackage1.loadRatePlan(CtrlPackage1.paqueteNuevo, False)
                '    Me.btnSave.Enabled = True
                'End If
                MostrarCmdNew(True)
            End If
        Else
            lblErrorSource.Text = PortalCulture.GetString("00659")
            lblErrorSource.Visible = True
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If CtrlPackage1.edicion = False Then
            CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlan, btnSave, "A")
        End If
        Dim LK As HyperLink
        Dim lk2 As LinkButton
        For Each i As DataGridItem In Me.dgPackage.Items
            If i.ItemType = ListItemType.AlternatingItem Or i.ItemType = ListItemType.Item Then
                LK = i.Cells(Me.dgcolumns.eliminar).FindControl("lnkEliminar")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlan, LK, "D")
                lk2 = i.Cells(Me.dgcolumns.code).FindControl("lnkedit")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlan, lk2, "M")
            End If
        Next
        loadResources()
    End Sub

    Private Sub loadResources()
        cmdNew.Value = PortalCulture.GetString("00102")
        If Me.CtrlPackage1.edicion = True Then
            lblctrtitulo.Text = PortalCulture.GetString("00593")
        Else
            lblctrtitulo.Text = PortalCulture.GetString("00592")
        End If
        Me.btncancel.Text = PortalCulture.GetString("00009")
        Me.btnSave.Text = PortalCulture.GetString("00008")
        dgPackage.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        dgPackage.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        Me.lblTitle.Text = PortalCulture.GetString("00658")
        dgPackage.Columns(dgcolumns.name).HeaderText = PortalCulture.GetString("00073")
        dgPackage.Columns(dgcolumns.code).HeaderText = PortalCulture.GetString("00001")
        ''''Me.lblErrorSource.Text = PortalCulture.GetString("00659")
        Select Case Cerror
            Case 1
                lblError.Text = PortalCulture.GetString("00308")
            Case 2
                lblError.Text = PortalCulture.GetString("00127")
            Case 3
                lblError.Text = PortalCulture.GetString("00309")
            Case 4
                lblError.Text = "ya hay un Paquete con ese Código y segmento"
            Case 5
                lblError.Text = PortalCulture.GetString("00344")
            Case 6
                lblError.Text = PortalCulture.GetString("00466")
        End Select
    End Sub

    Private Sub dgpackage_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPackage.PageIndexChanged
        Me.dgPackage.CurrentPageIndex = e.NewPageIndex
        Me.dgPackage.SelectedIndex = -1
        loadpackages(ctrlAutoComplete1.GetFilter)
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
    End Sub

    Private Sub dgpackage_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPackage.ItemCommand
        Cerror = 0
        Me.lblError.Visible = False
        Me.lblErrorSource.Visible = False
        If e.CommandName = "Select" Then
            Me.CtrlPackage1.ClearData()
            Me.CtrlPackage1.edicion = True
            Me.CtrlPackage1.IdPaquete = e.Item.Cells(dgcolumns.idpaquete).Text
            CtrlPackage1.IdRatePlan = e.Item.Cells(dgcolumns.idrateplan).Text 'dgPackage.DataKeys(e.Item.ItemIndex)
            CtrlPackage1.idRateCode = dgPackage.DataKeys(e.Item.ItemIndex)
            'If e.Item.Cells(Me.dgcolumns.principalSegmentRac).Text.ToUpper = "TRUE" Then
            '    CtrlPackage1.loadRatePlan(dgPackage.DataKeys(e.Item.ItemIndex), True)
            'Else
            ' CtrlPackage1.paqueteNuevo = ""
            CtrlPackage1.loadRatePlan(e.Item.Cells(dgcolumns.idrateplan).Text, False)
            'End If
            Me.btnSave.Enabled = True
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
            MostrarCmdNew(False)
        ElseIf e.CommandName = "Delete" Then
            Cerror = 0
            lblError.Visible = False
            Dim dsRatePlan As RatePlanData
            With New RatePlanFacade
                If .DeleteRatePlan(Me.cInfoActual.Hotel, e.Item.Cells(dgcolumns.idrateplan).Text) Then
                    Me.guardalog("/Pages/RatesPlans.aspx", PaginaBase.acciones.Eliminar, "Eliminó el rateplan con el id " & Me.dgPackage.Items(e.Item.ItemIndex).Cells(dgcolumns.code).Text & " y el codigo de tarifa " & Me.dgPackage.Items(e.Item.ItemIndex).Cells(dgcolumns.code).Text)

                    If dgPackage.CurrentPageIndex > 0 And dgPackage.Items.Count = 1 Then
                        dgPackage.CurrentPageIndex = ((dgPackage.CurrentPageIndex * dgPackage.PageSize) \ dgPackage.PageSize) - 1
                    End If
                    With New PackageFacade
                        .DeletePackage(Me.cInfoActual.Hotel, dgPackage.DataKeys(e.Item.ItemIndex))
                        CtrlPackage1.eliminarPortales(Me.cInfoActual.Hotel, e.Item.Cells(dgcolumns.idrateplan).Text)
                    End With
                    With New FaresSystem
                        .DeleteTarifasByRatePlan(Me.cInfoActual.Hotel, e.Item.Cells(dgcolumns.idrateplan).Text)
                    End With
                    'FALTA BORRAR TARIFAS.

                    loadpackages(ctrlAutoComplete1.GetFilter)

                    Me.dgPackage.SelectedIndex = -1
                    Me.CtrlPackage1.ClearData()
                    MostrarCmdNew(True)
                Else
                    Cerror = 6
                    lblError.Visible = True
                End If
            End With
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
            'CtlMensajes1.Show(PortalCulture.GetString("00047"), PortalCulture.GetString("00464"), ctlMensajes.Tipos.Prompt)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.CtrlPackage1.edicion = False
        lblError.Visible = False
        Me.CtrlPackage1.ClearData()
        Me.dgPackage.SelectedIndex = -1
        Cerror = 0
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
        MostrarCmdNew(True)
    End Sub
    'Private Sub btnNew2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
    '    Me.CtrlPackage1.edicion = False
    '    lblError.Visible = False
    '    Me.CtrlPackage1.ClearData()
    '    Me.dgPackage.SelectedIndex = -1
    '    Cerror = 0
    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
    '    btnNew.Visible = False
    'End Sub
    Private Sub dgpackage_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPackage.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then

            Dim LK As HyperLink
            Dim LK2 As LinkButton
            LK = e.Item.Cells(Me.dgcolumns.eliminar).FindControl("lnkEliminar")

            LK.Text = PortalCulture.GetString("00103")
            LK2 = e.Item.Cells(Me.dgcolumns.eliminar).FindControl("lnkedit")
            LK2.Text = PortalCulture.GetString("00093")
            LK2 = e.Item.Cells(Me.dgcolumns.eliminar).FindControl("lnkEliminar2")
            LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), PortalCulture.GetString("00464"))
            'If e.Item.Cells(Me.dgcolumns.principalSegmentRac).Text.ToUpper = "TRUE" Then
            '    LK.Enabled = False
            'End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.name).Text = PortalCulture.GetString("00073")
            e.Item.Cells(dgcolumns.code).Text = PortalCulture.GetString("00001")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(dgcolumns.eliminar).Text = CType(dgPackage.DataSource, DataView).Count & " " & PortalCulture.GetString("00658")
        End If
    End Sub

    Private Sub dgpackage_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPackage.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dgPackage.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgPackage.CurrentPageIndex < dgPackage.PageCount - 1 Then
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


    'Public Function GetFareRestrictions() As FaresRestrictionsData
    '    Return CtrlPackage1.GetFareRestrictions()
    '    'Return Me.CtrlPlanFares2.GetFareRestrictions()
    'End Function

    'Public Function valorIdDgAdult()
    '    Return CtrlPackage1.IdDgAdult()
    'End Function

    'Public Function valorIdDgChild()
    '    Return CtrlPackage1.IdDgChild()
    'End Function

    'Public Sub loadDatos()
    '    CtrlPackage1.loadDatos()
    'End Sub

    Private Sub ctrlAutoComplete1_OnSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        dgPackage.CurrentPageIndex = 0
        loadpackages(descripcion)
    End Sub

End Class
