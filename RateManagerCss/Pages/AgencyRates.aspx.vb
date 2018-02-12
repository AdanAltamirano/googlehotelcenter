Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade


Partial Class AgencyRates
    Inherits PaginaBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents lblDescripcionError As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub
#End Region

    Protected WithEvents ctrlAgencyRates1 As ctrlAgencyRates
    Protected WithEvents CtlMensajes1 As ctlMensajes

    Enum dgcolumns
        code
        name
        edit
        eliminar
        idrateplan
        orden
    End Enum

#Region "Metodos"
    Private Sub loadAgencyRates(ByVal sfiltro As String)
        Try
            Dim ds As AgencyRatesData
            Dim dv As DataView
            Dim idAsoc As Integer = Me.GetIdAsociation

            With New AgencyRatesFacade
                ds = .GetAgencyRatesByIdHotel(MyBase.cInfoActual.Hotel, idAsoc, idIdioma:=PortalCulture.GetIDCulture)
            End With

            'Dim dv As DataView

            With dgRates
                dv = ds.Tables(AgencyRatesData.AgencyRates_TABLE).DefaultView
                dv.RowFilter = sfiltro
                .DataKeyField = AgencyRatesData.FIELD_rateCode
                .DataSource = dv
                .DataBind()
            End With
        Catch ex As Exception
            Dim mensaje As String = ex.Message
        End Try
    End Sub


#End Region

#Region "Propiedades"
    Private Property Cerror() As Integer
        Get
            Return viewstate("_cerror")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_cerror") = Value
        End Set
    End Property
#End Region

#Region "Eventos"

    Sub MostrarCmdNew(ByVal show As Boolean)
        cmdNew.Style.Add("display", IIf(show, "", "none"))
        divContenedor.Style.Add("display", IIf(show, "none", ""))
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

        ctrlAgencyRates1.idCompany = MyBase.cInfoActual.Empresa

        If Not IsPostBack Then
            Cerror = 0
            ctrlAgencyRates1.m_iHotelId = Me.cInfoActual.Hotel
            loadAgencyRates("")

            Me.ctrlAgencyRates1.edicion = False
            lblErrorSource.Visible = False
            MostrarCmdNew(True)
        End If

        Me.ResizefrmPrincipal()
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdNew.Value = PortalCulture.GetString("00102")
        If Me.ctrlAgencyRates1.edicion = True Then
            lblctrtitulo.Text = PortalCulture.GetString("00593")
        Else
            lblctrtitulo.Text = PortalCulture.GetString("00592")
        End If

        Me.lblTitle.Text = PortalCulture.GetString("00757")
        Me.btnSave.Text = PortalCulture.GetString("00751")
        Me.btnNuevo.Text = PortalCulture.GetString("00009")

        dgRates.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00758")
        dgRates.PagerStyle.NextPageText = PortalCulture.GetString("00759") & " >>"
        dgRates.Columns(dgcolumns.name).HeaderText = PortalCulture.GetString("00760")
        dgRates.Columns(dgcolumns.code).HeaderText = PortalCulture.GetString("00761")
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPublish.Click
        If Not Page.IsValid Then Return
        Dim publish As Boolean = (CType(sender, Button).ID = Me.btnPublish.ID)
        lblErrorSource.Text = "*"
        lblError.Visible = False
        lblErrorSource.Visible = False

        If ctrlAgencyRates1.isSourceSelected() Then
            Cerror = ctrlAgencyRates1.SavePlan(publish)

            If Cerror <> 0 Then
                lblErrorSource.Visible = True
                lblErrorSource.Text = ctrlAgencyRates1.descripcionError
            Else
                dgRates.SelectedIndex = -1
                loadAgencyRates(ctrlAutoComplete1.GetFilter)
                MostrarCmdNew(True)
            End If
        Else
            lblErrorSource.Visible = True
            lblErrorSource.Text = ctrlAgencyRates1.descripcionError

        End If
    End Sub

    Private Sub dgAgencyRates_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRates.ItemCommand
        Try
            Cerror = 0
            Me.lblError.Visible = False
            Me.lblErrorSource.Visible = False
            If e.CommandName = "Select" Then
                Me.ctrlAgencyRates1.ClearData()
                Me.ctrlAgencyRates1.edicion = True
                ctrlAgencyRates1.IdRatePlan = e.Item.Cells(dgcolumns.idrateplan).Text
                ctrlAgencyRates1.idRateCode = dgRates.DataKeys(e.Item.ItemIndex)

                'If e.Item.Cells(dgcolumns.orden).Text <> "&nbsp;" Then
                '    ctrlAgencyRates1.m_orden = e.Item.Cells(dgcolumns.orden).Text
                'Else
                '    ctrlAgencyRates1.m_orden = 0
                'End If

                Session("idRP") = ctrlAgencyRates1.IdRatePlan
                ctrlAgencyRates1.loadRatePlan(ctrlAgencyRates1.IdRatePlan, False)

                Me.btnSave.Enabled = True

                MostrarCmdNew(False)
            ElseIf e.CommandName = "Delete" Then
                Cerror = 0
                lblError.Visible = False
                'Dim dsRatePlan As RatePlanData

                With New RatePlanFacade
                    If .DeleteRatePlan(Me.cInfoActual.Hotel, e.Item.Cells(dgcolumns.idrateplan).Text) Then
                        Me.guardalog("/Pages/AgencyRates.aspx", PaginaBase.acciones.Eliminar, "Eliminó el rateplan con el Codigo Tarifa  " & Me.dgRates.Items(e.Item.ItemIndex).Cells(dgcolumns.code).Text & " y el idHotel " & Me.cInfoActual.Hotel)
                        If dgRates.CurrentPageIndex > 0 And dgRates.Items.Count = 1 Then
                            dgRates.CurrentPageIndex = ((dgRates.CurrentPageIndex * dgRates.PageSize) \ dgRates.PageSize) - 1
                        End If

                        Dim dsAR As AgencyRatesData
                        With New AgencyRatesFacade
                            dsAR = .GetAgencyRatesByRateCode(dgRates.DataKeys(e.Item.ItemIndex))

                            If Not (dsAR Is Nothing) Then
                                With New AgencyRatesAvailFacade
                                    .DeleteAgencyRatesAvail(dsAR.Tables(AgencyRatesData.AgencyRates_TABLE).Rows(0).ItemArray("0"))
                                    ctrlAgencyRates1.eliminarPortales(Me.cInfoActual.Hotel, e.Item.Cells(dgcolumns.idrateplan).Text)
                                End With
                            End If

                            .DeleteAgencyRates(Me.cInfoActual.Hotel, dgRates.DataKeys(e.Item.ItemIndex))
                        End With

                        loadAgencyRates(ctrlAutoComplete1.GetFilter)
                        Me.ctrlAgencyRates1.ClearData()
                        Me.dgRates.SelectedIndex = -1
                        MostrarCmdNew(True)
                    Else
                        Cerror = 6
                        lblError.Visible = True
                    End If
                End With

            End If
        Catch ex As Exception
            Dim mensaje As String = ex.Message
        End Try
    End Sub

    Private Sub dgAgencyRates_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRates.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dgRates.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00758")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgRates.CurrentPageIndex < dgRates.PageCount - 1 Then
                Dim _next As New System.Web.UI.WebControls.LinkButton
                _next.CommandArgument = "Next"
                _next.CommandName = "Page"
                _next.Text = PortalCulture.GetString("00759") & "&nbsp;>"
                _next.CausesValidation = False

                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, _next)
            End If
        End If
    End Sub

    Private Sub dgAgencyRates_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRates.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            Dim LK As HyperLink
            Dim LK2 As LinkButton

            LK = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkEliminar")
            LK.Text = PortalCulture.GetString("00762")
            LK2 = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkedit")
            LK2.Text = PortalCulture.GetString("00763")
            LK2 = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkEliminar2")
            LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), PortalCulture.GetString("00765"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.name).Text = PortalCulture.GetString("00760")
            e.Item.Cells(dgcolumns.code).Text = PortalCulture.GetString("00761")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(dgcolumns.eliminar).Text = CType(dgRates.DataSource, DataView).Count & " " & PortalCulture.GetString("00766")
        End If
    End Sub

    Private Sub dgAgencyRates_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRates.PageIndexChanged
        Me.dgRates.CurrentPageIndex = e.NewPageIndex
        Me.dgRates.SelectedIndex = -1
        loadAgencyRates(ctrlAutoComplete1.GetFilter)

    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
        lblErrorSource.Text = "*"
        lblError.Visible = False
        lblErrorSource.Visible = False
        Me.ctrlAgencyRates1.ClearData()
        Me.dgRates.SelectedIndex = -1

        MostrarCmdNew(True)
    End Sub
    'Private Sub btnNuevo2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
    '    lblErrorSource.Text = "*"
    '    lblError.Visible = False
    '    lblErrorSource.Visible = False
    '    Me.ctrlAgencyRates1.ClearData()
    '    Me.dgRates.SelectedIndex = -1
    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
    '    btnNew.Visible = False
    'End Sub
#End Region

    Private Sub ctrlAutoComplete1_OnSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        dgRates.CurrentPageIndex = 0
        loadAgencyRates(descripcion)
    End Sub

End Class
