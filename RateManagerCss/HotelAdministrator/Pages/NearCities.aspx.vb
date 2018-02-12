Imports Portal.Catalogos.Common
Imports Portal.Catalogos.Common.Data
Imports Portal.Catalogos.Facade
Partial Class NearCities
    Inherits PaginaBase
    Private Property IdCityAdd() As Integer
        Get
            Return viewstate("_AddIDCity")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_AddIDCity") = Value
        End Set
    End Property
    Private Property CityNameAdd() As String
        Get
            Return viewstate("_AddNameCity")
        End Get
        Set(ByVal Value As String)
            viewstate("_AddNameCity") = Value
        End Set
    End Property
    Private Property StateNameAdd() As String
        Get
            Return viewstate("_StateNameAdd")
        End Get
        Set(ByVal Value As String)
            viewstate("_StateNameAdd") = Value
        End Set
    End Property
    Enum Columns As Integer
        Incluir
        Ciudad
        Estado
        Distancia
        idCiudad
        IncluirValue
        DistanciaValue
        idEmpresa
    End Enum

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents hplSearch As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblShowTab As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

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
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then Me.redirectTo(PaginaBase.pages.SearchHotel)
        If Not IsPostBack Then
            btnSearchMoreCities.Attributes.Add("onclick", "javascript:show('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "')")
        End If
        Me.DefaultButton(Me.txtCityName, Me.lnkSearch)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadCulture()
        DataBindGrid()
    End Sub
    Private Sub DataBindGrid()
        Me.grid.DataSource = LoadNearCities()
        Me.grid.DataBind()
    End Sub
    Private Sub loadCulture()
        btnSearchMoreCities.Value = PortalCulture.GetString("00547")
        Me.lblTitle.Text = PortalCulture.GetString("00548")
        lblAddNearCity.InnerHtml = PortalCulture.GetString("00549")
        lblInfo.Text = PortalCulture.GetString("00546")
        Me.grid.Columns(Columns.Incluir).HeaderText = PortalCulture.GetString("M000101")
        Me.grid.Columns(Columns.Ciudad).HeaderText = PortalCulture.GetString("M000254")
        Me.grid.Columns(Columns.Estado).HeaderText = PortalCulture.GetString("00252")
        Me.grid.Columns(Columns.Distancia).HeaderText = PortalCulture.GetString("M000103")
        Me.btnSave.Text = PortalCulture.GetString("M000106")
        lblCityName.InnerHtml = PortalCulture.GetString("00550", True)
        lnkSearch.Text = PortalCulture.GetString("00547")
        Me.btnCancel.Text = PortalCulture.GetString("M000143")
        Me.dgCities.Columns(0).HeaderText = PortalCulture.GetString("00254")
        Me.dgCities.Columns(1).HeaderText = PortalCulture.GetString("00252")

    End Sub
    Private Function LoadNearCities() As DataTable
        Dim nearcities As DataSet
        With New clsFacadeNearCities
            nearcities = .loadNearCities(MyBase.cInfoActual.Empresa)
        End With

        If IdCityAdd <> 0 AndAlso Not Me.CityNameAdd Is Nothing Then
            If nearcities.Tables(clsCommonNearCities.Table_EmpresasCiudades).Select("idciudad=" & IdCityAdd).Length = 0 Then
                Dim dr As DataRow
                dr = nearcities.Tables(clsCommonNearCities.Table_EmpresasCiudades).NewRow()
                dr(clsCommonNearCities.Field_idCiudad) = IdCityAdd
                dr(clsCommonNearCities.Field_NameCity) = CityNameAdd
                dr(clsCommonNearCities.Field_StateName) = Me.StateNameAdd
                dr("Activo") = 0
                dr("Distancia") = 0
                dr(clsCommonNearCities.Field_IDEmpresa) = 0
                nearcities.Tables(clsCommonNearCities.Table_EmpresasCiudades).Rows.Add(dr)
            End If
        End If
        Return nearcities.Tables(clsCommonNearCities.Table_EmpresasCiudades)
    End Function

    Private Sub lnkSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSearch.Click
        Dim Script As String
        If Me.txtCityName.Text.Trim.Length < 3 Then Exit Sub
        'Dim er As String
        Dim ds As clsCommonNearCities
        With New clsFacadeNearCities
            ds = .loadNearCitiesByName(Me.txtCityName.Text)
        End With
        dgCities.DataSource = ds
        dgCities.DataBind()
        'Page.RegisterStartupScript("", "<script>show('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "');</script>")

        Script = String.Format("<script>show('{0}','{1}');</script>", PnlBox.ClientID, TblPnl.ClientID)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Script)
    End Sub

    Private Sub grid_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grid.ItemCommand

    End Sub

    Private Sub dgCities_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCities.ItemCommand
        IdCityAdd = e.Item.Cells(3).Text
        CitynameAdd = e.Item.Cells(0).Text
        Me.StateNameAdd = e.Item.Cells(1).Text
        txtCityName.Text = ""
        dgCities.DataSource = Nothing
        dgCities.DataBind()
    End Sub
    Private Function validateFields() As Boolean
        Dim Val As IValidator
        'Ejecutamos la validacion de los validators
        For Each Val In Page.Validators
            Val.Validate()
        Next
        Return Me.Page.IsValid
    End Function

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not validateFields() Then Exit Sub

        'Construimos el dataset a partir de el datagrid.
        Dim i As Integer

        Dim ds As New clsCommonNearCities
        Dim tb As DataTable = ds.Tables(clsCommonNearCities.Table_EmpresasCiudades)

        Dim checkInclude As System.Web.UI.WebControls.CheckBox
        Dim txtDistance As System.Web.UI.WebControls.TextBox

        'Se llena el dataset ds con los datos del grid
        For i = 0 To grid.Items.Count - 1

            checkInclude = grid.Items(i).Cells(Columns.Incluir).FindControl("chkInclude")
            txtDistance = grid.Items(i).Cells(Columns.Distancia).FindControl("txtDistance")


            If Not IsNumeric(txtDistance.Text) Then txtDistance.Text = 0


            Dim row As DataRow = tb.NewRow()
            With row
                .Item(clsCommonNearCities.Field_IDEmpresa) = MyBase.cInfoActual.Empresa
                .Item(clsCommonNearCities.Field_idCiudad) = grid.Items(i).Cells(Columns.idCiudad).Text
                .Item(clsCommonNearCities.Field_Distancia) = txtDistance.Text
            End With
            tb.Rows.Add(row)
            'Se aceptan cambios para establecer los rows en unchange
            row.AcceptChanges()

            If checkInclude.Checked = True Then
                'Actualizamos se cambia el estado del row para actualizar
                'El procedimiento almacenado checa si existe un row con idEmpresa e idAeropuerto igual 
                'Si existe lo actualiza si no lo inserta
                row.Item(clsCommonNearCities.Field_IDEmpresa) = MyBase.cInfoActual.Empresa
            Else
                'Eliminamos se cambia el estado del row para eliminar
                If CInt(grid.Items(i).Cells(Columns.idEmpresa).Text) <> 0 Then
                    row.Delete()
                End If
            End If

        Next
        'ds.Tables.Add(tb)
        With New clsFacadeNearCities
            If .SaveNearCities(ds) Then
                Me.grid.DataSource = .loadNearCities(MyBase.cInfoActual.Empresa)
            End If
        End With
        Me.grid.DataBind()
        Me.IdCityAdd = 0
        Me.CityNameAdd = ""

        Me.dgCities.DataSource = Nothing
        Me.dgCities.DataBind()
        Me.txtCityName.Text = ""

    End Sub

    Private Sub grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
            'Mostramos los aeropuertos activos
            Dim checkInclude As System.Web.UI.WebControls.CheckBox
            checkInclude = e.Item.Cells(Columns.Incluir).FindControl("chkInclude")
            If Not checkInclude Is Nothing Then
                checkInclude.Checked = CType(e.Item.Cells(Columns.IncluirValue).Text, Boolean)
                checkInclude.Attributes.Add("onclick", "javascript:HighlightRow2('" & checkInclude.ClientID & "','" & e.Item.ItemType.ToString & "')")
                Dim dgi As DataGridItem
                dgi = CType(checkInclude.Parent.Parent, DataGridItem)
                If checkInclude.Checked = True Then
                    dgi.CssClass = "dgSelected"
                Else
                    If dgi.ItemType = ListItemType.AlternatingItem Then
                        dgi.CssClass = "dgAlternate"
                        dgi.Attributes.Add("itemType", "AlternatingItem")
                        e.Item.Attributes.Add("Typeitem", "AlternatingItem")
                    Else
                        dgi.CssClass = "dgItem"
                        dgi.Attributes.Add("itemType", "Item")
                        e.Item.Attributes.Add("Typeitem", "Item")
                    End If
                End If
            End If
            'Ponemos el valor de la distancia
            Dim txtDistance As System.Web.UI.WebControls.TextBox
            txtDistance = e.Item.Cells(Columns.Distancia).FindControl("txtDistance")
            If Not txtDistance Is Nothing Then
                txtDistance.Text = e.Item.Cells(Columns.DistanciaValue).Text
            End If


            'Ponemos el valor del validador
            Dim lblerr As System.Web.UI.WebControls.RegularExpressionValidator
            lblerr = e.Item.Cells(Columns.Distancia).FindControl("valChildrenExtraPrice")
            If Not lblerr Is Nothing Then lblerr.Text = PortalCulture.GetString("M000105")
        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(Columns.Incluir).Text = PortalCulture.GetString("M000101")
            e.Item.Cells(Columns.Ciudad).Text = PortalCulture.GetString("M000254")
            e.Item.Cells(Columns.Distancia).Text = PortalCulture.GetString("M000103")

        End If
    End Sub

    Private Sub dgCities_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCities.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lnk As LinkButton
            lnk = e.Item.FindControl("lnkAddCity")
            If Not lnk Is Nothing Then
                lnk.Text = PortalCulture.GetString("M000515")
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("00254")
            e.Item.Cells(1).Text = PortalCulture.GetString("00252")
        End If

    End Sub
End Class
'create spCiudadesSearchByName
'create spNearCitiesGetByEmpresa
'create spNearCitiesUpdate
