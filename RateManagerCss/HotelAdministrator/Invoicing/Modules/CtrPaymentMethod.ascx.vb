Option Explicit On


Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.Hotels.DataAccess

Partial Public Class CtrPaymentMethod
    Inherits System.Web.UI.UserControl


    Dim _company As Integer = -1

    Public Property Company() As Integer
        Get
            Return _company
        End Get
        Set(ByVal Value As Integer)
            _company = Value
        End Set
    End Property
    Public Property IsEdit() As Boolean
        Get
            Return ViewState("CtrPaymentMethod_isEdit")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("CtrPaymentMethod_isEdit") = Value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then LoadPaymentMethods()
    End Sub
    Private Sub LoadPaymentMethods()
        Dim dt As PaymentMethodDataSet.PaymentMethod_GetAllDataTable


        dt = New Oz.UniBilling.Hotels.Business.PaymentMethod().GetAll()

        With ddlMethod
            .DataSource = dt
            If (PortalCulture.GetIDCulture() = 1) Then
                .DataTextField = "MethodESP"
            Else
                .DataTextField = "MethodING"
            End If
            .DataValueField = "PaymentMethodID"
            .DataBind()
        End With

    End Sub

    Public Function SavePaymentMethod() As Integer
        Dim dt As New PaymentMethodDataSet.CompanyPaymentMethodDataTable
        Dim drow As PaymentMethodDataSet.CompanyPaymentMethodRow

        'Validaciones
        If ddlMethod.SelectedValue = "1" And txtOther.Text.Trim() = "" Then
            Return -1
        End If

        If txtAccountNumber.Text.Trim() <> "" And txtAccountNumber.Text.Length < 4 Then
            Return -2
        End If






        If IsEdit Then
            dt = New Oz.UniBilling.Hotels.Business.PaymentMethod().SelectByID_CompanyPaymentMethod(Integer.Parse(hidCompanyPaymentMethodID.Value))
            drow = dt(0)
        Else
            drow = dt.NewCompanyPaymentMethodRow()
        End If

        drow.PaymentMethodID = ddlMethod.SelectedValue
        drow.Default = chkDefault.Checked
        drow.CompanyID = _company
        If (txtAccountNumber.Text.Trim().Length = 0 Or txtAccountNumber.Text.Trim().Length > 3) Then
            drow.AccountNumber = txtAccountNumber.Text.Trim()
        End If

        If (ddlMethod.SelectedValue = "1") Then
            drow.Method = txtOther.Text
        End If

        If IsEdit Then
            IsEdit = False
        Else
            dt.AddCompanyPaymentMethodRow(drow)
        End If


        Dim rules As New Oz.UniBilling.Hotels.Business.PaymentMethod()
        If rules.Update(dt) Then
            'lblMsg.Text = "Metodo de pago guardado"
            ResetForm()
            Return 1
        Else
            'lblMsg.Text = "No se pudo guardar el metodo de pago"
            Return -3
        End If

    End Function

    Public Function LoadPaymentMethod(ByVal id As Integer, ByVal isEdit As Boolean) As Integer
        Me.IsEdit = isEdit

        Dim dt As PaymentMethodDataSet.CompanyPaymentMethodDataTable


        dt = New Oz.UniBilling.Hotels.Business.PaymentMethod().SelectByID_CompanyPaymentMethod(id)
        If dt.Rows.Count > 0 Then
            ddlMethod.SelectedValue = dt(0).PaymentMethodID

            If ddlMethod.SelectedValue = "1" Then
                txtOther.Enabled = True
            Else
                txtOther.Enabled = False
                txtOther.Text = ""
            End If

            If ddlMethod.SelectedValue = "2" Then
                txtAccountNumber.Text = ""
                txtAccountNumber.Enabled = False
            Else
                txtAccountNumber.Enabled = True
            End If

            If Not dt(0).IsAccountNumberNull Then txtAccountNumber.Text = dt(0).AccountNumber
            If Not dt(0).IsMethodNull Then txtOther.Text = dt(0).Method
            chkDefault.Checked = dt(0).Default
            hidCompanyPaymentMethodID.Value = dt(0).CompanyPaymentMethodID

            Return 1
        Else
            Return 0
        End If

    End Function

    Public Function DeletPaymentMethod(ByVal id As Integer) As Integer

        Dim dt As PaymentMethodDataSet.CompanyPaymentMethodDataTable


        dt = New Oz.UniBilling.Hotels.Business.PaymentMethod().SelectByID_CompanyPaymentMethod(id)
        If dt.Rows.Count > 0 Then
            dt(0).Delete()
            Dim rules As New Oz.UniBilling.Hotels.Business.PaymentMethod()
            If rules.Update(dt) Then
                'lblMsg.Text = "Metodo de pago eliminado"
                ResetForm()
                Return 1
            Else
                'lblMsg.Text = "No se pudo eliminar el metodo de pago"
                Return -4
            End If

        End If

    End Function
    Public Function ResetForm()
        ddlMethod.SelectedIndex = 0
        txtAccountNumber.Text = ""
        txtAccountNumber.Enabled = True
        txtOther.Text = ""
        txtOther.Enabled = True
        chkDefault.Checked = False
        IsEdit = False
    End Function

    Private Sub LoadResources()
        lblMethod.Text = PortalCulture.GetString("01531")
        lblOther.Text = PortalCulture.GetString("01532")
        lblDefaul.Text = PortalCulture.GetString("01534")
        lblAccountNumber.Text = PortalCulture.GetString("01533")

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        LoadResources()
    End Sub
End Class