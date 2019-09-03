Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Data.SqlClient

Partial Class ctrlSearchCompany
    Inherits System.Web.UI.UserControl


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
    Public ReadOnly Property getTxtName() As TextBox
        Get
            Return Me.txtName
        End Get
    End Property

    Public Property Rubro() As Integer
        Get
            If viewstate.Item("KEY_IDRUBRO") Is Nothing Then
                viewstate.Item("KEY_IDRUBRO") = 0
            End If
            Return viewstate.Item("KEY_IDRUBRO")
        End Get
        Set(ByVal Value As Integer)
            viewstate.Item("KEY_IDRUBRO") = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not Me.IsPostBack Then
            fillDropDownsList()
        End If

        If Rubro <> 0 Then
            Me.lblRubro.Visible = False
            Me.cmbRubro.SelectedValue = Rubro
            Me.cmbRubro.Visible = False
        End If
    End Sub

    Private Sub subClearCmb(ByRef cmb As DropDownList)
        cmb.Items.Clear()
        cmb.Items.Add(New ListItem("-- Todos --", -1))
    End Sub

    Private Sub fillDropDownsList()
        Dim i As Integer
        subClearCmb(cmbRubro)
        subClearCmb(cmbEstado)
        subClearCmb(cmbMunicipio)
        subClearCmb(cmbCiudad)

        Dim dsRubros As DataSet = Me.loadRubrosCompany()

        If Not dsRubros Is Nothing And dsRubros.Tables.Count <> 0 Then
            For i = 0 To dsRubros.Tables(0).Rows.Count - 1
                With dsRubros.Tables(0).Rows(i)
                    Me.cmbRubro.Items.Add(New ListItem(.Item("NombreRubro"), .Item("idRubro")))
                End With
            Next
        End If

        Dim dsEstados As DataSet = Me.loadEstadosByidPais("MX")
        If Not dsEstados Is Nothing And dsEstados.Tables.Count <> 0 Then
            For i = 0 To dsEstados.Tables(0).Rows.Count - 1
                With dsEstados.Tables(0).Rows(i)
                    Me.cmbEstado.Items.Add(New ListItem(.Item("Nombre"), .Item("idEstado")))
                End With
            Next
        End If

    End Sub

    Private Function loadRubrosCompany() As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spCompanyRubroActual", conection)
        With command
            .CommandType = CommandType.StoredProcedure
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        Try
            adapter.Fill(dRes)
        Catch ex As Exception
            Dim smes = ex.ToString
        End Try

        Return dRes
    End Function

    Private Function loadEstadosByidPais(ByVal idPais As String) As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spLugar_EstadosGetByIdPais", conection)
        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idPais", idPais))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Private Function loadMunicipiosByIdEstado(ByVal idEstado As Integer) As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spLugar_MunicipiosGetByIdEstado", conection)
        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idEstado", idEstado))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Private Function loadCiudadesByIdMunicipio(ByVal idMunicipio As Integer) As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spLugar_CiudadesGetByIdMunicipio", conection)
        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idMunicipio", idMunicipio))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Private Sub cmbEstado_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEstado.SelectedIndexChanged
        subClearCmb(cmbMunicipio)
        Me.lblMunicipio.Visible = False
        Me.cmbMunicipio.Visible = False
        Me.cmbMunicipio.SelectedIndex = 0


        Me.lblCiudad.Visible = False
        Me.cmbCiudad.Visible = False
        Me.cmbCiudad.SelectedIndex = 0

        If Me.cmbEstado.SelectedIndex > 0 Then
            cmbMunicipio.Visible = True
            Me.lblMunicipio.Visible = True
            Dim i As Integer
            Dim dsMunicipios As DataSet = Me.loadMunicipiosByIdEstado(Me.cmbEstado.SelectedValue)
            If Not dsMunicipios Is Nothing And dsMunicipios.Tables.Count <> 0 Then
                For i = 0 To dsMunicipios.Tables(0).Rows.Count - 1
                    With dsMunicipios.Tables(0).Rows(i)
                        Me.cmbMunicipio.Items.Add(New ListItem(.Item("Nombre"), .Item("idMunicipio")))
                    End With
                Next
            End If
        End If
    End Sub

    Private Sub cmbMunicipio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMunicipio.SelectedIndexChanged
        subClearCmb(cmbCiudad)

        Me.lblCiudad.Visible = False
        Me.cmbCiudad.Visible = False
        Me.cmbCiudad.SelectedIndex = 0

        If Me.cmbMunicipio.SelectedIndex > 0 Then
            Me.lblCiudad.Visible = True
            Me.cmbCiudad.Visible = True

            Dim i As Integer
            Dim dsCiudades As DataSet = Me.loadCiudadesByIdMunicipio(Me.cmbMunicipio.SelectedValue)
            If Not dsCiudades Is Nothing And dsCiudades.Tables.Count <> 0 Then
                For i = 0 To dsCiudades.Tables(0).Rows.Count - 1
                    With dsCiudades.Tables(0).Rows(i)
                        Me.cmbCiudad.Items.Add(New ListItem(.Item("Nombre"), .Item("idCiudad")))
                    End With
                Next
            End If
        End If
    End Sub

    Private Function getCompanys(ByVal status As String, ByVal isAsoc As Boolean) As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        'Dim command As New SqlCommand("spCompanySearchCompanys", conection)
        Dim spname As String = IIf(isAsoc, "spCompanySearchCompanysAssociation", "spCompanySearchCompanys")
        Dim command As New SqlCommand(spname, conection)
        Dim idUsuario = CType(Me.Page, PaginaBase).Usuario
        Dim idAsociacionHotel As Integer = CType(Me.Page, PaginaBase).GetIdAsociation

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idRubro", Me.cmbRubro.SelectedValue))
            .Parameters.Add(New SqlParameter("@Nombre", Me.txtName.Text.Trim.Replace("'", "")))
            .Parameters.Add(New SqlParameter("@idEstado", Me.cmbEstado.SelectedValue))
            .Parameters.Add(New SqlParameter("@idMunicipio", Me.cmbMunicipio.SelectedValue))
            .Parameters.Add(New SqlParameter("@idCiudad", Me.cmbCiudad.SelectedValue))
            .Parameters.Add(New SqlParameter("@status", status))
            'If CType(Me.Page, PaginaBase).isUserChain Or CType(Me.Page, PaginaBase).IsUsuarioHotelAssociation Then
            If Not CType(Me.Page, PaginaBase).IsSupervisor And Not CType(Me.Page, PaginaBase).IsUsuarioCallCenter Then
                .Parameters.Add(New SqlParameter("@idUsuario", idUsuario))
            End If
            If idAsociacionHotel <> -1 Then
                .Parameters.Add(New SqlParameter("@idAsociacionHotel", idAsociacionHotel))
            End If

        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Public Function HotelData(ByVal idHotel As Integer) As DataSet
        Dim conection As New SqlConnection(AppSettings("HotelConnection"))
        'Dim command As New SqlCommand("spCompanySearchCompanys", conection)
        Dim spname As String = "spHotelGetData"
        Dim command As New SqlCommand(spname, conection)
        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idHotel", idHotel))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Public Function searchCompanys(ByRef data As DataSet, Optional ByVal status As String = "") As Boolean
        data = Me.getCompanys(status, CType(Me.Page, PaginaBase).IsUsuarioHotelAssociation)
        Return True
    End Function

    Private Sub lnkAvancedSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkAvancedSearch.Click
        Me.PanelAvancedSearch.Visible = Not Me.PanelAvancedSearch.Visible
        Me.cmbEstado.SelectedIndex = 0
        Me.cmbEstado_SelectedIndexChanged(Me.cmbEstado, Nothing)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CargaRecursos()
    End Sub

    Private Sub CargaRecursos()
        Label1.Text = PortalCulture.GetString("00249", True)
        lnkAvancedSearch.Text = PortalCulture.GetString("00241", False)
        lblRubro.Text = PortalCulture.GetString("00250", True)
        Label4.Text = PortalCulture.GetString("00251", True)
        Label7.Text = PortalCulture.GetString("00252", True)
        lblMunicipio.Text = PortalCulture.GetString("00253", True)
        lblCiudad.Text = PortalCulture.GetString("00254", True)
        Label2.Text = PortalCulture.GetString("00255", True)
    End Sub

End Class



