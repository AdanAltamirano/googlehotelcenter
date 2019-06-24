Imports Portal.General.Facade
Imports Portal.General.Common
Imports Portal.General.Common.Data

Imports Portal.TaskManager.Facade
Imports Portal.General.Rules

Partial Class CompanyRegister
    Inherits PaginaBase

    Private Property Edit() As Boolean
        Get
            Return viewstate("_EditEmpresa")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("_EditEmpresa") = Value
        End Set
    End Property
    Private Property idempresa() As Integer
        Get
            Return viewstate("_IdEmpresa")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_IdEmpresa") = Value
        End Set
    End Property
    Private Property idhotel() As Integer
        Get
            Return viewstate("_IdHotel")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_IdHotel") = Value
        End Set
    End Property
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMoneda As System.Web.UI.WebControls.Label
    Protected WithEvents cmbMonedas As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblCheckin As System.Web.UI.WebControls.Label
    Protected WithEvents cmbCheckinHora As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbCheckinMin As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblCheckout As System.Web.UI.WebControls.Label
    Protected WithEvents cmbCheckoutHora As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbCheckoutMin As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblMaxNumCuartos As System.Web.UI.WebControls.Label
    Protected WithEvents txtMaxNumCuartos As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RangeValidator2 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents lblEdadMaximaNiño As System.Web.UI.WebControls.Label
    Protected WithEvents txtEdadMaximaNino As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator6 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RangeValidator7 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents lblEdadPagoExtra As System.Web.UI.WebControls.Label
    Protected WithEvents txtEdadPagoExtra As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator7 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RangeValidator8 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents lblImpuesto As System.Web.UI.WebControls.Label
    Protected WithEvents txtImpuesto As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator8 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RangeValidator9 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents lblCommision As System.Web.UI.WebControls.Label
    Protected WithEvents txtCommision As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator12 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RangeValidator13 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents lblUnder As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblBegin As System.Web.UI.WebControls.Label
    Protected WithEvents CustomValidator1 As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents cmdEnviar As System.Web.UI.WebControls.Button
    Protected WithEvents Form2 As System.Web.UI.HtmlControls.HtmlForm

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


    Protected WithEvents EmpresaModulo1 As EmpresaModulo

    Private status As Byte

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            'EmpresaModulo1.companyType = 0
            EmpresaModulo1.ididioma = PortalCulture.GetIDCulture()
            Edit = False
            If Not Request.QueryString("idempresa") Is Nothing AndAlso Not Request.QueryString("sGuid") Is Nothing AndAlso Not Request.QueryString("idhotel") Is Nothing Then
                idempresa = Request.QueryString("idempresa")
                idhotel = Request.QueryString("idhotel")
                If EmpresaModulo1.LoadData(idempresa, Request.QueryString("sGuid"), idhotel, status) Then
                    Edit = True
                End If
            End If
        End If
        If status = 2 Then
            Me.cmbPublish.Visible = False
            Me.cmbDesactivar.Visible = True
        End If
    End Sub

    Private Sub cmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAceptar.Click
        If Page.IsValid Then
            EmpresaModulo1.Visible = True
            cmdCancelar.Visible = False
            EmpresaModulo1.showError(False)
            If Edit = False Then
                If EmpresaModulo1.Add(0) Then
                    BtnNuevo_Click(Nothing, Nothing)
                Else
                    EmpresaModulo1.showError(True)
                End If
            Else
                If Not EmpresaModulo1.Update(idempresa, idhotel) Then
                    EmpresaModulo1.showError(True)
                Else
                    BtnNuevo_Click(Nothing, Nothing)
                End If
            End If

        End If
    End Sub

    Private Sub cmdSaveHouse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveHouse.Click
        If Page.IsValid Then
            EmpresaModulo1.Visible = True
            cmdCancelar.Visible = False
            EmpresaModulo1.showError(False)
            If Edit = False Then
                If EmpresaModulo1.Add(0) Then
                    BtnNuevo_Click(Nothing, Nothing)
                Else
                    EmpresaModulo1.showError(True)
                End If
            Else
                If Not EmpresaModulo1.Update(idempresa, idhotel) Then
                    EmpresaModulo1.showError(True)
                Else
                    BtnNuevo_Click(Nothing, Nothing)
                End If
            End If

        End If
    End Sub

    Private Sub cmdCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelar.Click
        EmpresaModulo1.Visible = True
        cmdCancelar.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitulo.Text = PortalCulture.GetString("00823")
        Me.cmdCancelar.Text = PortalCulture.GetString("A00143", False) 'Cancelar
        BtnNuevo.Text = PortalCulture.GetString("00102", False) 'Nuevo
        cmdAceptar.Text = PortalCulture.GetString("A00153", False) 'Guardar
        Me.cmbPublish.Text = PortalCulture.GetString("00845")

        'If EmpresaModulo1.companyType = 0 Then
        '    cmbPublish.Visible = True
        '    cmdAceptar.Visible = True
        '    cmdSaveHouse.Visible = False
        'Else
        '    cmbPublish.Visible = False
        '    cmdAceptar.Visible = False
        '    cmdSaveHouse.Visible = True
        'End If
        If status = 2 Then
            Me.cmbPublish.Visible = False
        End If
        Me.cmbDesactivar.Text = PortalCulture.GetString("01521")
    End Sub

    Private Sub BtnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnNuevo.Click
        Response.Redirect("CompanyRegister.aspx", True)
    End Sub

    Private Sub cmdEnviar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEnviar.Click
        If Page.IsValid Then

        Else
            CustomValidator1.IsValid = False

        End If

    End Sub

    Private Sub cmbPublish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPublish.Click
        If Page.IsValid Then

            EmpresaModulo1.Visible = True
            cmdCancelar.Visible = False
            EmpresaModulo1.showError(False)
            If Edit = False Then
                Dim Newidempresa As Integer = 0
                If EmpresaModulo1.Add(Newidempresa) Then
                    With New EmpresaSistema
                        If Not .CompanyCompleted(Newidempresa) Then
                            EmpresaModulo1.showError(True)
                        Else
                            With New RequestSystem
                                Dim x As Integer = .CompanyRegistrationApproved(Newidempresa)
                            End With
                        End If
                    End With
                    BtnNuevo_Click(Nothing, Nothing)
                Else
                    EmpresaModulo1.showError(True)
                End If
            Else
                If EmpresaModulo1.Update(idempresa, idhotel) Then
                    With New EmpresaSistema
                        If Not .CompanyCompleted(idempresa) Then
                            EmpresaModulo1.showError(True)
                        Else
                            With New RequestSystem
                                Dim x As Integer = .CompanyRegistrationApproved(idempresa)
                            End With
                            BtnNuevo_Click(Nothing, Nothing)
                        End If
                    End With

                Else
                    EmpresaModulo1.showError(True)
                End If

            End If
        End If
    End Sub
    Private Sub cmbDesactivar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDesactivar.Click
        If EmpresaModulo1.Desactiva_Empresa(idempresa) Then
            BtnNuevo_Click(Nothing, Nothing)
        End If
    End Sub
End Class
