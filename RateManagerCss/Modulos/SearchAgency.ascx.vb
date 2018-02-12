Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient

Partial Class SearchAgency
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
    Private Const spGetAgencies As String = "spGetAgencies"
    Public Event FinalizarBusqueda()
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then
            lblNotFound.Text = PortalCulture.GetString("03249")            
        End If
        Call EstablecerVisibilidadListAgencias(True, False)
    End Sub

    Private Sub uxSearchLink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSearchLink.Click
        'Codigo para buscar Agencias.
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Try
            If txtFiltro.Text.Trim <> "" Then
                Dim cCom As DataSet
                'ddlBoxAgencies.Items.Clear()
                ddlBoxAgencies.Items.Clear()
                cCom = New DataSet
                cmCom = New SqlCommand(spGetAgencies, New SqlConnection(AppSettings("AgencyConnectionString")))
                With cmCom
                    .CommandType = CommandType.StoredProcedure
                End With
                daCom.SelectCommand = cmCom
                daCom.Fill(cCom)

                'If rdbNombre.Checked Then
                cCom.Tables(0).DefaultView.RowFilter = String.Format("Agencia Like '%{0}%'", txtFiltro.Text.Trim)
                'ElseIf rdbIATA.Checked Then
                '    cCom.Tables(0).DefaultView.RowFilter = String.Format("IATA = '{0}'", txtFiltro.Text.Trim)
                'End If

            Dim dt As New DataView
            dt = cCom.Tables(0).DefaultView
            If dt.Count > 0 Then
                'ddlBoxAgencies.DataTextField = "Agencia"
                'ddlBoxAgencies.DataValueField = "IdAgencia"
                dt.Sort = "Agencia ASC"
                'ddlBoxAgencies.DataSource = dt
                'ddlBoxAgencies.DataBind()
                ddlBoxAgencies.DataTextField = "Agencia"
                ddlBoxAgencies.DataValueField = "IdAgencia"
                ddlBoxAgencies.DataSource = dt
                ddlBoxAgencies.DataBind()

                EstablecerVisibilidadListAgencias(True, False)
            Else
                lblNotFound.Text = PortalCulture.GetString("03249")
                EstablecerVisibilidadListAgencias(True, True)
            End If
            Else
                lblNotFound.Text = PortalCulture.GetString("03249")
                ddlBoxAgencies.Items.Clear()
                EstablecerVisibilidadListAgencias(True, True)
            End If
            RaiseEvent FinalizarBusqueda()
        Catch ex As Exception
            Dim strError As String
            strError = ex.ToString
        End Try

    End Sub
    Public Sub EstablecerVisibilidadListAgencias(ByVal valor1 As Boolean, ByVal valor2 As Boolean)
        If valor1 = True Then
            If ddlBoxAgencies.Items.Count > 0 Then
                ddlBoxAgencies.Style("display") = ""
            Else
                ddlBoxAgencies.Style("display") = "none"
            End If
            uxSearchLink.Style("display") = ""
            txtFiltro.Style("display") = ""
        Else
            uxSearchLink.Style("display") = "none"
            txtFiltro.Style("display") = "none"
        End If
        If valor2 = True Then
            lblNotFound.Style("display") = ""
        Else
            lblNotFound.Style("display") = "none"
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        uxSearchLink.Text = PortalCulture.GetString("M0BT0000115")
        lblNotFound.Text = PortalCulture.GetString("00815")

        'ckbAgencias.Text = PortalCulture.GetString("03251", False)
        'ckbAgencias.Attributes("onclick") = String.Format("javascript:onCheckBoxClickOcultarMostrar('{0}','{1}', '{2}', '{3}', '{4}', '{5}');", ckbAgencias.ClientID, txtFiltro.ClientID, uxSearchLink.ClientID, ddlBoxAgencies.ClientID, lblNotFound.ClientID, renglonOpt.ClientID)
    End Sub

    Public Sub EstablecerMensaje(ByVal msg As String)
        EstablecerVisibilidadListAgencias(True, True)
        lblNotFound.Text = msg
    End Sub
    Public Function AgenciaSeleccionada() As String

        If (ddlBoxAgencies.Items.Count > 0 AndAlso ddlBoxAgencies.SelectedIndex <> -1 AndAlso ddlBoxAgencies.SelectedItem.Text <> "") Then
            'EstablecerVisibilidadListAgencias(True, False)
            Return ddlBoxAgencies.SelectedItem.Text
        Else
            'lblNotFound.Text = PortalCulture.GetString("03252", True)
            'EstablecerVisibilidadListAgencias(True, True)
            Return ""
        End If
    End Function
    Public Function IdAgenciaSeleccionada() As Integer
        If (ddlBoxAgencies.Items.Count > 0 AndAlso ddlBoxAgencies.SelectedIndex <> -1 AndAlso ddlBoxAgencies.SelectedItem.Text <> "") Then
            Return ddlBoxAgencies.SelectedItem.Value
        Else
            'lblNotFound.Text = PortalCulture.GetString("03252", True)
            'EstablecerVisibilidadListAgencias(True, True)
            Return -1
        End If
    End Function
    Public Sub AgregarAgencia(ByVal agencia As String, ByVal idAgencia As Integer)
        Dim lstItem As New ListItem
        ddlBoxAgencies.Items.Clear()
        lstItem.Text = agencia
        lstItem.Value = idAgencia
        ddlBoxAgencies.Items.Add(lstItem)
        ddlBoxAgencies.SelectedIndex = ddlBoxAgencies.Items.Count - 1
    End Sub
    Public Function NumberOfAgenciesFounded() As Integer
        Return ddlBoxAgencies.Items.Count
    End Function
    Public Sub Limpiar()
        ddlBoxAgencies.Items.Clear()
        txtFiltro.Text = ""
        EstablecerVisibilidadListAgencias(True, False)
    End Sub
End Class
