Imports Portal.Catalogos.Common.Data
Imports Portal.Catalogos.Facade
Imports System.Runtime.Serialization
Partial Class NearAirports

    Inherits PaginaBase
    Private Property drAirportAdd() As RowAirportsNear
        Get
            Return viewstate("_AddAirports")
        End Get
        Set(ByVal Value As RowAirportsNear)
            viewstate("_AddAirports") = Value
        End Set
    End Property

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnSearch As System.Web.UI.WebControls.Button
    Protected WithEvents tblSearchData As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents hplSearch As System.Web.UI.WebControls.HyperLink

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region
    Enum Columns As Integer
        Incluir
        Ciudad
        Aeropuerto
        Distancia
        Tiempo
        idAropuerto
        idMunucipo
        idCiudad
        IncluirValue
        DistanciaValue
        TiempoValue
        PK_idEmpresa
        PK_idAropuerto
    End Enum
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then Me.redirectTo(PaginaBase.pages.SearchHotel)
        If Not IsPostBack Then
            Me.btnSearchMoreAirports.Attributes.Add("onclick", "javascript:show('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "')")
        End If
        Me.DefaultButton(Me.txtCityName, Me.lnkSearch)
        'Me.ResizefrmPrincipal()
    End Sub
    Private Sub DataBindGrid()
        Me.grid.DataSource = LoadNearAiports()
        Me.grid.DataBind()
    End Sub
    Private Function LoadNearAiports() As clsCommonAirportsNear
        Dim Airports As clsCommonAirportsNear
        With New clsFacadeAirportsNear
            Airports = .loadAirportsNear(MyBase.cInfoActual.Empresa)
        End With
        If Not Me.drAirportAdd Is Nothing Then
            If Airports.Tables(0).Select("idAeropuerto=" & drAirportAdd.FldidAeropuerto).Length = 0 Then
                Dim dr As DataRow
                dr = Airports.Tables(0).NewRow()
                dr("idAeropuerto") = Me.drAirportAdd.FldidAeropuerto
                dr("Ciudad") = Me.drAirportAdd.FldCiudad
                dr("Aeropuerto") = Me.drAirportAdd.FldAeropuerto
                dr("idMunicipio") = 0
                dr("idCiudad") = Me.drAirportAdd.FldidCiudad
                dr("Activo") = 0
                dr("Distancia") = 0
                dr("Tiempo") = 0
                dr("PK_idEmpresa") = MyBase.cInfoActual.Empresa
                dr("PK_idAeropuerto") = Me.drAirportAdd.FldidAeropuerto
                Airports.Tables(0).Rows.Add(dr)
            End If
        End If
        Return Airports
    End Function

    Private Function ColumunsName(ByVal col As Columns) As String
        Select Case col
            Case Columns.Incluir
                Return PortalCulture.GetString("M000101")
            Case Columns.Ciudad
                Return PortalCulture.GetString("M000254")
            Case Columns.Aeropuerto
                Return PortalCulture.GetString("M000109")
            Case Columns.Distancia
                Return PortalCulture.GetString("M000103")
            Case Columns.Tiempo
                Return PortalCulture.GetString("M000104")
        End Select
        Return ""
    End Function


    Private Sub grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
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

            'Ponemos el valor del tiempo
            Dim txtTime As System.Web.UI.WebControls.TextBox
            txtTime = e.Item.Cells(Columns.Tiempo).FindControl("txtTime")
            If Not txtTime Is Nothing Then
                txtTime.Text = e.Item.Cells(Columns.TiempoValue).Text
            End If

            'Ponemos el valor del validador
            Dim lblerr As System.Web.UI.WebControls.RegularExpressionValidator
            lblerr = e.Item.Cells(Columns.Distancia).FindControl("valChildrenExtraPrice")
            If Not lblerr Is Nothing Then lblerr.Text = PortalCulture.GetString("M000105")

            Dim lblerr2 As System.Web.UI.WebControls.RegularExpressionValidator
            lblerr2 = e.Item.Cells(Columns.Tiempo).FindControl("RegularExpressionValidator1")
            If Not lblerr2 Is Nothing Then lblerr2.Text = PortalCulture.GetString("M000105")

        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not validateFields() Then Exit Sub

        'Construimos el dataset a partir de el datagrid.
        Dim i As Integer

        Dim ds As New clsCommonAirportsNear
        Dim tb As DataTable = ds.Tables(clsCommonAirportsNear.Table_EmpresasAeropuertos)

        Dim checkInclude As System.Web.UI.WebControls.CheckBox
        Dim txtDistance As System.Web.UI.WebControls.TextBox
        Dim txtTime As System.Web.UI.WebControls.TextBox
        'Se llena el dataset ds con los datos del grid
        For i = 0 To grid.Items.Count - 1

            checkInclude = grid.Items(i).Cells(Columns.Incluir).FindControl("chkInclude")
            txtDistance = grid.Items(i).Cells(Columns.Distancia).FindControl("txtDistance")
            txtTime = grid.Items(i).Cells(Columns.Tiempo).FindControl("txtTime")

            If Not IsNumeric(txtDistance.Text) Then txtDistance.Text = 0
            If Not IsNumeric(txtTime.Text) Then txtTime.Text = 0

            Dim row As DataRow = tb.NewRow()
            With row
                .Item(clsCommonAirportsNear.Field_IDEmpresa) = MyBase.cInfoActual.Empresa
                .Item(clsCommonAirportsNear.Field_idAeropuerto) = grid.Items(i).Cells(Columns.idAropuerto).Text
                .Item(clsCommonAirportsNear.Field_Distancia) = txtDistance.Text
                .Item(clsCommonAirportsNear.Field_Tiempo) = txtTime.Text
            End With
            tb.Rows.Add(row)
            'Se aceptan cambios para establecer los rows en unchange
            row.AcceptChanges()

            If checkInclude.Checked = True Then
                'Actualizamos se cambia el estado del row para actualizar
                'El procedimiento almacenado checa si existe un row con idEmpresa e idAeropuerto igual 
                'Si existe lo actualiza si no lo inserta
                row.Item(clsCommonAirportsNear.Field_IDEmpresa) = MyBase.cInfoActual.Empresa
            Else
                'Eliminamos se cambia el estado del row para eliminar
                If CInt(grid.Items(i).Cells(Columns.PK_idAropuerto).Text) <> 0 Then
                    row.Delete()
                End If
            End If

        Next
        'ds.Tables.Add(tb)
        With New clsFacadeAirportsNear
            If .SaveAirportsNear(ds) Then
                Me.grid.DataSource = .loadAirportsNear(MyBase.cInfoActual.Empresa)
            End If
        End With
        Me.grid.DataBind()
        Me.drAirportAdd = Nothing
        Me.dgAirports.DataSource = Nothing
        Me.dgAirports.DataBind()
        Me.txtCityName.Text = ""
    End Sub
    Private Function validateFields() As Boolean
        Dim Val As IValidator
        'Ejecutamos la validacion de los validators
        For Each Val In Page.Validators
            Val.Validate()
        Next
        Return Me.Page.IsValid
    End Function

    Private Sub loadResources()
        Me.lblTitle.Text = PortalCulture.GetString("M000107")

        Me.btnSave.Text = PortalCulture.GetString("M000106")
        Me.lblInfo.Text = PortalCulture.GetString("M000108")
        grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        grid.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        Me.grid.Columns(Columns.Incluir).HeaderText = ColumunsName(Columns.Incluir)
        Me.grid.Columns(Columns.Ciudad).HeaderText = ColumunsName(Columns.Ciudad)
        Me.grid.Columns(Columns.Aeropuerto).HeaderText = ColumunsName(Columns.Aeropuerto)
        Me.grid.Columns(Columns.Distancia).HeaderText = ColumunsName(Columns.Distancia)
        Me.grid.Columns(Columns.Tiempo).HeaderText = ColumunsName(Columns.Tiempo)

        Me.dgAirports.Columns(1).HeaderText = PortalCulture.GetString("00254")
        Me.dgAirports.Columns(2).HeaderText = PortalCulture.GetString("M000109")

        Me.lnkSearch.Text = PortalCulture.GetString("00543")
        lblAddairportNear.InnerText = PortalCulture.GetString("00544")
        Me.btnSearchMoreAirports.Value = PortalCulture.GetString("00543")
        btnCancel.Text = PortalCulture.GetString("M000143")
        Me.lblSearhNewAirport.InnerText = PortalCulture.GetString("00545", True)
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
        DataBindGrid()
    End Sub

    

    'spaeropuertoSearchbyIata
    'spaeropuertoSearchByName
    'spAirport_CompanyGetListByIdCompany



    Private Sub dgAirports_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAirports.ItemCommand
        ' ds = LoadNearAiports()
        drAirportAdd = New RowAirportsNear
        drAirportAdd.FldidAeropuerto = e.Item.Cells(0).Text
        drAirportAdd.FldCiudad = e.Item.Cells(1).Text
        drAirportAdd.FldAeropuerto = e.Item.Cells(2).Text
        drAirportAdd.FldidCiudad = e.Item.Cells(5).Text
        drAirportAdd.FldidAeropuerto = e.Item.Cells(0).Text
       
    End Sub


    Private Sub lnkSearch_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSearch.Click
        If Me.txtCityName.Text.Trim.Length < 3 Then Exit Sub
        'Dim er As String
        Dim ds As clsCommonAeropuertos
        If Me.txtCityName.Text.Trim.Length = 3 Then
            With New clsFacadeAeropuertos
                ds = .SearchAirportByIATA(Me.txtCityName.Text.Trim, PortalCulture.GetIDCulture)
            End With
        Else
            With New clsFacadeAeropuertos
                ds = .SearchAirports(Me.txtCityName.Text.Trim, PortalCulture.GetIDCulture)
            End With
        End If
        dgAirports.DataSource = ds
        dgAirports.DataBind()
        'Page.RegisterStartupScript("", "<script>show('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "');</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, "<script>show('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "');</script>")
    End Sub

    Private Sub dgAirports_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAirports.ItemDataBound
        Dim lnk As LinkButton
        lnk = e.Item.FindControl("lnkAddAirport")
        If Not lnk Is Nothing Then
            lnk.Text = PortalCulture.GetString("M000515")
        End If

    End Sub
End Class
<Serializable()> Public Class RowAirportsNear
    Implements ISerializable
    Public FldidAeropuerto As Integer
    Public FldCiudad As String
    Public FldAeropuerto As String
    Public FldidCiudad As Integer
    Public Sub New()

    End Sub


    Public Sub New(ByVal Airport As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        With Airport
            FldidAeropuerto = .GetValue("idAeropuerto", GetType(Integer))
            FldCiudad = .GetValue("Ciudad", GetType(String))
            FldAeropuerto = .GetValue("Aeropuerto", GetType(String))
            FldidCiudad = .GetValue("idCiudad", GetType(Integer))
        End With
    End Sub

    Public Sub GetObjectData(ByVal Airport As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        With Airport
            .AddValue("idAeropuerto", FldidAeropuerto, GetType(Integer))
            .AddValue("Ciudad", FldCiudad, GetType(String))
            .AddValue("Aeropuerto", FldAeropuerto, GetType(String))
            .AddValue("idCiudad", FldidCiudad, GetType(Integer))

        End With
    End Sub
End Class