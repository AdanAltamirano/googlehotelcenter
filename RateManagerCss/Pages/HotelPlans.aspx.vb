Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Partial Class HotelPlans
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
    Private Const KEY_HOTELID As String = "HotelId"
    Protected CtrlHotelPlans1 As ctrlHotelPlans

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(Me.pages.Home)

        'CtrlHeader1.Panel = ctrlHeader.Usuario.uHotelAdministrador
        If Request.Browser.Browser.IndexOf("IE") > 0 Then
            Page.ClientTarget = "UpLevel"
        End If
        If Not IsPostBack Then
            Me.lblError.Visible = False
            CtrlHotelPlans1.m_iHotelId = GetHotelId()
            FillList()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Me.lblError.Visible = False
        If CtrlHotelPlans1.AddPlan() Then
            CtrlHotelPlans1.NewPlan()
            FillList()
            Me.dtgPlans.SelectedIndex = -1
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Me.dtgPlans.SelectedIndex = -1
        CtrlHotelPlans1.NewPlan()
        Me.lblError.Visible = False
    End Sub

    Private Function GetHotelId() As Integer
        Return MyBase.cInfoActual.Hotel
    End Function

    Private Sub FillList()
        Dim datPlans As HotelPlansData
        With New HotelPlansSystem
            datPlans = .GetPlansByHotelId(GetHotelId())
        End With
        If Not datPlans Is Nothing Then
            With dtgPlans
                If .CurrentPageIndex > .Items.Count \ .PageSize Then
                    .CurrentPageIndex = (.Items.Count - 1) \ .PageSize
                End If
                .DataSource = datPlans.HotelPlansTable
                CType(.Columns(0), BoundColumn).DataField = HotelPlansData.TableFields.PLANNAME
                CType(.Columns(1), BoundColumn).DataField = HotelPlansData.TableFields.PKID_HOTELPLAN
                .DataBind()
                If .PageCount > 1 Then
                    .PagerStyle.Visible = True
                Else
                    .PagerStyle.Visible = False
                End If
            End With
        End If
    End Sub

    Private Sub dtgPlans_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgPlans.ItemCommand
        Dim iPlanId As Integer
        Try
            iPlanId = CInt(e.Item.Cells(1).Text)
        Catch ex As Exception
            iPlanId = 0
        End Try
        If e.CommandName = "Select" Then
            Me.CtrlHotelPlans1.LoadPlan(iPlanId)
            Me.dtgPlans.SelectedIndex = e.Item.ItemIndex
        End If
    End Sub

    Private Sub dtgPlans_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPlans.PageIndexChanged
        Me.dtgPlans.CurrentPageIndex = e.NewPageIndex
        Me.dtgPlans.SelectedIndex = -1
        FillList()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Me.CtrlHotelPlans1.DeletePlan() Then
            Me.CtrlHotelPlans1.Clear()
            Me.dtgPlans.SelectedIndex = -1
            Me.CtrlHotelPlans1.NewPlan()
            FillList()
            Me.lblError.Visible = False
        Else
            Me.lblError.Visible = True
        End If

    End Sub
    Private Sub loadResources()
        lbltitle.Text = PortalCulture.GetString("00290")

        btnNew.Text = PortalCulture.GetString("00102")

        btnDelete.Text = PortalCulture.GetString("00103")
        btnSave.Text = PortalCulture.GetString("00008")

        dtgPlans.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        dtgPlans.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"

        CType(dtgPlans.Columns(0), BoundColumn).HeaderText = PortalCulture.GetString("00291")

        For Each item As DataGridItem In dtgPlans.Items
            Dim ibtnEdit As ImageButton = item.FindControl("ibtnEdit")
            If Not ibtnEdit Is Nothing Then
                ibtnEdit.ToolTip = PortalCulture.GetString("00093")
            End If
        Next
        Me.lblError.Text = PortalCulture.GetString("00296")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
        If Me.dtgPlans.SelectedIndex <> -1 Then
            Me.btnDelete.Enabled = True
        Else
            Me.btnDelete.Enabled = False
        End If
    End Sub

End Class
