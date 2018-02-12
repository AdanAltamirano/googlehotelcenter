Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade

Partial Class CtrlPlanFaresExcPackage
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    '    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '       'Put user code to initialize the page here
    '  End Sub

    Public Property FieldException() As String
        Get
            Return ViewState("Excep")
        End Get
        Set(ByVal Value As String)
            ViewState("Excep") = Value
        End Set
    End Property
    Public Property combinaciones() As Integer
        Get
            Return ViewState("CombPers")
        End Get
        Set(ByVal Value As Integer)
            ViewState("CombPers") = Value
        End Set
    End Property

    Enum RestrictionsDtgCols As Integer
        AdultNumber = 0
        AdultFare
        ChildNumber
        ChildFare
        TotalFare
        RestrictionId
        Tools
    End Enum

    Public Sub createFieldException()
        Dim cad As String = ""
        For I As Integer = 1 To 7
            Dim CK As CheckBox
            CK = FindControl("chk" & I.ToString)
            If CK.Checked = False Then
                cad &= "N"
            Else
                cad &= "Y"
            End If
        Next
        FieldException = cad
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        'If Not IsPostBack Then
        '    FillDataGrid()
        'End If

    End Sub

    'Public Sub ReFill()
    '    FillDataGrid()
    'End Sub

#Region "FILL DE LOS NUEVOS DATAGRIDS"
    'Private Sub FillDatas()
    '    Dim datRestrictions As FaresRestrictionsData
    '    Dim dtAdults As DataTable, dtChildren As DataTable
    '    Dim ad As Byte, ch As Byte
    '    Dim drnew As DataRow
    '    Dim strch As String = ""
    '    dtAdults = New DataTable("Adults")
    '    dtChildren = New DataTable("Children")
    '    dtAdults.Columns.Add("Adults")
    '    dtAdults.Columns.Add("Price")
    '    dtAdults.Columns.Add(datRestrictions.PKIDRESTRICTION_FIELD)

    '    dtChildren.Columns.Add("Children")
    '    dtChildren.Columns.Add("Price")
    '    dtChildren.Columns.Add(datRestrictions.PKIDRESTRICTION_FIELD)

    '    'datRestrictions = CType(Me.Page, FaresCatalogue).GetFareRestrictions
    '    datRestrictions = CType(Me.Page, Package).GetFareRestrictions

    '    If Not datRestrictions Is Nothing Then
    '        For Each dr As DataRow In datRestrictions.Tables(datRestrictions.FARESRESTRICTION_TABLE).Rows
    '            If dr(datRestrictions.ADULTNUMBER_FIELD) <> ad Then
    '                ad = dr(datRestrictions.ADULTNUMBER_FIELD)
    '                drnew = dtAdults.NewRow
    '                drnew("Adults") = ad
    '                drnew("Price") = dr(datRestrictions.EXCADULTFARE_FIELD)
    '                drnew(datRestrictions.PKIDRESTRICTION_FIELD) = dr(datRestrictions.PKIDRESTRICTION_FIELD)
    '                dtAdults.Rows.Add(drnew)
    '            End If
    '            If dr(datRestrictions.CHILDNUMBER_FIELD) > 0 AndAlso strch.IndexOf("," & dr(datRestrictions.CHILDNUMBER_FIELD) & ",") = -1 Then
    '                ch = dr(datRestrictions.CHILDNUMBER_FIELD)
    '                strch &= "," & ch & ","
    '                drnew = dtChildren.NewRow
    '                drnew("Children") = ch
    '                drnew("Price") = dr(datRestrictions.EXCNINIOFARE_FIELD)
    '                drnew(datRestrictions.PKIDRESTRICTION_FIELD) = dr(datRestrictions.PKIDRESTRICTION_FIELD)
    '                dtChildren.Rows.Add(drnew)
    '            End If
    '        Next
    '        dgChild.DataSource = dtChildren
    '        dgAdult.DataSource = dtAdults
    '        dgChild.DataBind()
    '        dgAdult.DataBind()
    '    End If

    'End Sub
#End Region

    'Private Sub FillDataGrid()
    '    FillDatas()
    '    Dim datRestrictions As FaresRestrictionsData

    '    datRestrictions = CType(Me.Page, FaresCatalogue).GetFareRestrictions
    '    datRestrictions = CType(Me.Page, Package).GetFareRestrictions

    '    If Not datRestrictions Is Nothing Then
    '        dtgRestrictions.DataSource = datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE)
    '    End If
    '    dtgRestrictions.DataBind()
    '    If Not datRestrictions Is Nothing Then
    '        If Not datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE) Is Nothing AndAlso datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Count > 0 Then
    '            Dim cad As String = Me.FieldException

    '            For I As Integer = 1 To 7
    '                Dim CK As CheckBox
    '                CK = FindControl("chk" & I.ToString)
    '                CK.Checked = False
    '                If cad.Length >= 7 Then
    '                    If cad.Substring(I - 1, 1) = "Y" Then
    '                        CK.Checked = True
    '                    End If
    '                End If
    '            Next

    '        End If
    '        combinaciones = datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Count
    '    Else
    '        combinaciones = 0
    '    End If

        'End Sub
        'Public Function IsValidData() As Boolean
        '    createFieldException()
        '    If FieldException() <> "NNNNNNN" Then
        '        Dim txtAdultFare As TextBox

        '        For Each item As DataGridItem In dgAdult.Items
        '            txtAdultFare = item.Cells(1).FindControl("txtAdultFare")
        '            If CDbl(txtAdultFare.Text) < 1 Then
        '                Return False
        '            End If
        '        Next
        '    End If
        '    Return True
        'End Function

        'Public Function getRatesExceptions() As DataTable
        '    Dim txtAdultFare As TextBox
        '    Dim txtChildFare As TextBox
        '    Dim cvAd As System.Web.UI.WebControls.CustomValidator
        '    Dim cvCh As System.Web.UI.WebControls.CustomValidator
        '    Dim tabla As New DataTable
        '    tabla.Columns.Add("TarifaAdultoExc", GetType(System.Double))
        '    tabla.Columns.Add("TarifaNinioExc", GetType(System.Double))
        '    Dim dr As DataRow

        '    'For i As Integer = 0 To combinaciones - 1
        '    '    dr = tabla.NewRow
        '    '    txtAdultFare = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("txtAdultFare")
        '    '    'cvAd = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("cvErrAdults")
        '    '    txtChildFare = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("txtChildrenFare")
        '    '    'cvCh = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("cvErrChilds")
        '    '    dr("TarifaAdultoExc") = CInt(txtAdultFare.Text)
        '    '    dr("TarifaNinioExc") = CInt(txtChildFare.Text)
        '    '    tabla.Rows.Add(dr)
        '    'Next
        '    For Each item As DataGridItem In dgAdult.Items
        '        dr = tabla.NewRow
        '        txtAdultFare = item.Cells(1).FindControl("txtAdultFare")
        '        dr("TarifaAdultoExc") = CDbl(txtAdultFare.Text)
        '        dr("TarifaNinioExc") = 0
        '        tabla.Rows.Add(dr)
        '        For Each item2 As DataGridItem In dgChild.Items
        '            txtChildFare = item2.Cells(1).FindControl("txtChildrenFare")
        '            dr = tabla.NewRow
        '            If txtAdultFare.Text <> "" Then
        '                dr("TarifaAdultoExc") = CDbl(txtAdultFare.Text)
        '            Else
        '                dr("TarifaAdultoExc") = 0
        '            End If

        '            If txtAdultFare.Text <> "" Then
        '                dr("TarifaNinioExc") = CDbl(txtChildFare.Text)
        '            Else
        '                dr("TarifaNinioExc") = 0
        '            End If

        '            tabla.Rows.Add(dr)

        '        Next
        '    Next
        '    Return tabla
        'End Function

        ''Private Sub dtgRestrictions_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        ''    If e.Item.ItemType = ListItemType.Header Then
        ''        e.Item.Cells(RestrictionsDtgCols.AdultNumber).Text = PortalCulture.GetString("00060")
        ''        e.Item.Cells(RestrictionsDtgCols.ChildNumber).Text = PortalCulture.GetString("00061")
        ''        e.Item.Cells(RestrictionsDtgCols.AdultFare).Text = PortalCulture.GetString("00134")
        ''        e.Item.Cells(RestrictionsDtgCols.ChildFare).Text = PortalCulture.GetString("00135")
        ''        e.Item.Cells(RestrictionsDtgCols.TotalFare).Text = PortalCulture.GetString("00136")
        ''    End If

        ''    Dim txtAdultFare As TextBox
        ''    Dim txtChildFare As TextBox
        ''    Dim lblAdult As Label
        ''    Dim lblChild As Label
        ''    Dim valAdt As RegularExpressionValidator
        ''    Dim valChd As RegularExpressionValidator

        ''    txtAdultFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtAdultFare")
        ''    txtChildFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtChildrenFare")

        ''    lblAdult = e.Item.Cells(RestrictionsDtgCols.AdultNumber).FindControl("lblAdults")
        ''    lblChild = e.Item.Cells(RestrictionsDtgCols.ChildNumber).FindControl("lblChildren")



        ''    valAdt = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("valAdultExtraPrice")
        ''    valChd = e.Item.Cells(RestrictionsDtgCols.ChildFare).FindControl("valChildrenExtraPrice")

        ''    If Not valAdt Is Nothing Then valAdt.Text = PortalCulture.GetString("00240")
        ''    If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00240")

        ''    If Not txtAdultFare Is Nothing AndAlso lblAdult.Text <= 0 Then
        ''        txtAdultFare.Text = 0
        ''        txtAdultFare.Enabled = False
        ''        txtAdultFare.Visible = False

        ''    End If
        ''    If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
        ''        txtChildFare.Text = 0
        ''        txtChildFare.Enabled = False
        ''        txtChildFare.Visible = False
        ''        If lblChild.Text <= 0 Then
        ''            lblChild.Text = ""
        ''        End If
        ''    End If

        ''    'Mostramos la tarifa total 
        ''    If Not txtAdultFare Is Nothing And Not txtChildFare Is Nothing Then
        ''        Dim lblTotalFare As Label
        ''        lblTotalFare = e.Item.Cells(RestrictionsDtgCols.TotalFare).FindControl("lblTotal")
        ''        If Not lblTotalFare Is Nothing Then
        ''            txtAdultFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
        ''            txtChildFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
        ''            lblTotalFare.Text = CDec(CDec(txtAdultFare.Text) + CDec(txtChildFare.Text)).ToString("C")
        ''        End If
        ''    End If

        ''    Dim chkActive As CheckBox
        ''    chkActive = e.Item.Cells(RestrictionsDtgCols.Tools).FindControl("chkActive")
        ''    Dim dgi As DataGridItem
        ''    If Not chkActive Is Nothing Then
        ''        dgi = CType(chkActive.Parent.Parent, DataGridItem)
        ''        If e.Item.Cells(RestrictionsDtgCols.RestrictionId).Text = "0" Then
        ''            chkActive.Checked = False
        ''            If dgi.ItemType = ListItemType.AlternatingItem Then
        ''                dgi.CssClass = "dgAlternate"
        ''                dgi.Attributes.Add("itemType", "AlternatingItem")
        ''            Else
        ''                dgi.CssClass = "dgitem"
        ''                dgi.Attributes.Add("itemType", "Item")
        ''            End If
        ''        Else
        ''            chkActive.Checked = True
        ''            dgi = CType(chkActive.Parent.Parent, DataGridItem)
        ''            dgi.CssClass = "dgSelected"
        ''        End If
        ''    End If
        ''End Sub

        'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        '    loadResources()
        'End Sub

        'Private Sub loadResources()
        '    Me.lblLunes.Text = PortalCulture.GetString("00300")
        '    Me.lblMartes.Text = PortalCulture.GetString("00301")
        '    Me.lblMiercoles.Text = PortalCulture.GetString("00302")
        '    Me.lblJueves.Text = PortalCulture.GetString("00303")
        '    Me.lblViernes.Text = PortalCulture.GetString("00304")
        '    Me.lblSabado.Text = PortalCulture.GetString("00305")
        '    Me.lblDomingo.Text = PortalCulture.GetString("00306")
        'End Sub

        'Private Sub dgAdult_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAdult.ItemDataBound
        '    If e.Item.ItemType = ListItemType.Header Then
        '        e.Item.Cells(0).Text = PortalCulture.GetString("00060")
        '        'e.Item.Cells(RestrictionsDtgCols.ChildNumber).Text = PortalCulture.GetString("00061")
        '        e.Item.Cells(1).Text = PortalCulture.GetString("00134")
        '        'e.Item.Cells(RestrictionsDtgCols.ChildFare).Text = PortalCulture.GetString("00135")
        '        'e.Item.Cells(RestrictionsDtgCols.TotalFare).Text = PortalCulture.GetString("00136")
        '    End If
        '    Dim txtAdultFare As TextBox
        '    'Dim txtChildFare As TextBox
        '    Dim lblAdult As Label
        '    'Dim lblChild As Label
        '    Dim valAdt As RegularExpressionValidator
        '    'Dim valChd As RegularExpressionValidator
        '    txtAdultFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtAdultFare")
        '    'txtChildFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtChildrenFare")
        '    lblAdult = e.Item.Cells(RestrictionsDtgCols.AdultNumber).FindControl("lblAdults")
        '    'lblChild = e.Item.Cells(RestrictionsDtgCols.ChildNumber).FindControl("lblChildren")
        '    valAdt = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("valAdultExtraPrice")
        '    'valChd = e.Item.Cells(RestrictionsDtgCols.ChildFare).FindControl("valChildrenExtraPrice")
        '    If Not valAdt Is Nothing Then valAdt.Text = PortalCulture.GetString("00240")
        '    'If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00240")
        '    If Not txtAdultFare Is Nothing AndAlso lblAdult.Text <= 0 Then
        '        txtAdultFare.Text = 0
        '        txtAdultFare.Enabled = False
        '        txtAdultFare.Visible = False
        '    End If
        '    'If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
        '    '    txtChildFare.Text = 0
        '    '    txtChildFare.Enabled = False
        '    '    txtChildFare.Visible = False
        '    '    If lblChild.Text <= 0 Then
        '    '        lblChild.Text = ""
        '    '    End If
        '    'End If
        '    'Mostramos la tarifa total 
        '    'If Not txtAdultFare Is Nothing Then 'And Not txtChildFare Is Nothing Then
        '    'Dim lblTotalFare As Label
        '    'lblTotalFare = e.Item.Cells(RestrictionsDtgCols.TotalFare).FindControl("lblTotal")
        '    'If Not lblTotalFare Is Nothing Then
        '    'txtAdultFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
        '    'txtChildFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
        '    'lblTotalFare.Text = CDec(CDec(txtAdultFare.Text) + CDec(txtChildFare.Text)).ToString("C")
        '    'End If
        '    'End If'
        '    Dim chkActive As CheckBox
        '    chkActive = e.Item.Cells(3).FindControl("chkActive")
        '    Dim dgi As DataGridItem
        '    If Not chkActive Is Nothing Then
        '        dgi = CType(chkActive.Parent.Parent, DataGridItem)
        '        If e.Item.Cells(2).Text = "0" Then
        '            chkActive.Checked = False
        '            If dgi.ItemType = ListItemType.AlternatingItem Then
        '                dgi.CssClass = "dgAlternate"
        '                dgi.Attributes.Add("itemType", "AlternatingItem")
        '            Else
        '                dgi.CssClass = "Dgitem"
        '                dgi.Attributes.Add("itemType", "Item")
        '            End If
        '        Else
        '            chkActive.Checked = True
        '            dgi = CType(chkActive.Parent.Parent, DataGridItem)
        '            dgi.CssClass = "dgSelected"
        '        End If
        '    End If
        'End Sub

        'Private Sub dgChild_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgChild.ItemDataBound
        '    If e.Item.ItemType = ListItemType.Header Then
        '        'e.Item.Cells(RestrictionsDtgCols.AdultNumber).Text = PortalCulture.GetString("00060")
        '        e.Item.Cells(0).Text = PortalCulture.GetString("00061")
        '        'e.Item.Cells(RestrictionsDtgCols.AdultFare).Text = PortalCulture.GetString("00134")
        '        e.Item.Cells(1).Text = PortalCulture.GetString("00135")
        '        'e.Item.Cells(RestrictionsDtgCols.TotalFare).Text = PortalCulture.GetString("00136")
        '    End If

        '    'Dim txtAdultFare As TextBox
        '    Dim txtChildFare As TextBox
        '    'Dim lblAdult As Label
        '    Dim lblChild As Label
        '    'Dim valAdt As RegularExpressionValidator
        '    Dim valChd As RegularExpressionValidator
        '    'txtAdultFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtAdultFare")
        '    txtChildFare = e.Item.Cells(1).FindControl("txtChildrenFare")
        '    'lblAdult = e.Item.Cells(RestrictionsDtgCols.AdultNumber).FindControl("lblAdults")
        '    lblChild = e.Item.Cells(0).FindControl("lblChildren")
        '    'valAdt = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("valAdultExtraPrice")
        '    valChd = e.Item.Cells(1).FindControl("valChildrenExtraPrice")
        '    'If Not valAdt Is Nothing Then valAdt.Text = PortalCulture.GetString("00240")
        '    If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00240")
        '    'If Not txtAdultFare Is Nothing AndAlso lblAdult.Text <= 0 Then
        '    '    txtAdultFare.Text = 0
        '    '    txtAdultFare.Enabled = False
        '    '    txtAdultFare.Visible = False
        '    'End If
        '    If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
        '        txtChildFare.Text = 0
        '        txtChildFare.Enabled = False
        '        txtChildFare.Visible = False
        '        If lblChild.Text <= 0 Then
        '            lblChild.Text = ""
        '        End If
        '    End If
        '    'Mostramos la tarifa total 
        '    'If Not txtAdultFare Is Nothing And Not txtChildFare Is Nothing Then
        '    '    Dim lblTotalFare As Label
        '    '    lblTotalFare = e.Item.Cells(RestrictionsDtgCols.TotalFare).FindControl("lblTotal")
        '    '    If Not lblTotalFare Is Nothing Then
        '    '        txtAdultFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
        '    '        txtChildFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
        '    '        lblTotalFare.Text = CDec(CDec(txtAdultFare.Text) + CDec(txtChildFare.Text)).ToString("C")
        '    '    End If
        '    'End If
        '    Dim chkActive As CheckBox
        '    chkActive = e.Item.Cells(3).FindControl("chkActive")
        '    Dim dgi As DataGridItem
        '    If Not chkActive Is Nothing Then
        '        dgi = CType(chkActive.Parent.Parent, DataGridItem)
        '        If e.Item.Cells(2).Text = "0" Then
        '            chkActive.Checked = False
        '            If dgi.ItemType = ListItemType.AlternatingItem Then
        '                dgi.CssClass = "dgAlternate"
        '                dgi.Attributes.Add("itemType", "AlternatingItem")
        '            Else
        '                dgi.CssClass = "Dgitem"
        '                dgi.Attributes.Add("itemType", "Item")
        '            End If
        '        Else
        '            chkActive.Checked = True
        '            dgi = CType(chkActive.Parent.Parent, DataGridItem)
        '            dgi.CssClass = "dgSelected"
        '        End If
        '    End If
        'End Sub

End Class




