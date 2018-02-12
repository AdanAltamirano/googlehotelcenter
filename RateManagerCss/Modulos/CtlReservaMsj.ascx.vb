Partial Class CtlReservaMsj
    Inherits ucBase

    Public Function GetPrice(ByVal room As Integer) As String
        Return CType(Me.FindControl("txtPriceRoom" + room.ToString), TextBox).Text()
    End Function
    Public Sub SetPrice(ByVal room As Integer, ByVal value As String)
        CType(Me.FindControl("txtPriceRoom" + room.ToString), TextBox).Text = value
    End Sub
    Public Function GetTax(ByVal room As Integer) As String
        Return CType(Me.FindControl("txtTaxRoom" + room.ToString), TextBox).Text()
    End Function
    
    Public WriteOnly Property SetRooms() As Double
        Set(ByVal Value As Double)
            iTotRooms.Value = Value
        End Set
    End Property
    Public WriteOnly Property SetTotal() As Double
        Set(ByVal Value As Double)
            lblTotal.InnerHtml = Value
        End Set
    End Property
    Public WriteOnly Property SetMoneda() As String
        Set(ByVal Value As String)
            lblmoneda.InnerHtml = Value
            For i As Integer = 1 To 9
                CType(Me.FindControl("lblMonedaRoom" & i.ToString), HtmlGenericControl).InnerHtml = Value
            Next
        End Set
    End Property


    Public WriteOnly Property SetTax() As String
        Set(ByVal Value As String)
            Me.txtTax.Value = Value
            If Value > 0 Then
                Me.lblImpuesto.Text = PortalCulture.GetString("00640", True) & Value & "%"
            Else
                Me.lblImpuesto.Text = PortalCulture.GetString("00640", True) & PortalCulture.GetString("00433")
            End If

        End Set
    End Property




#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtPrice As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxRoom1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxRoom2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxRoom3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxRoom4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxRoom5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxRoom6 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxRoom7 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxRoom8 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxRoom9 As System.Web.UI.WebControls.TextBox

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
        Me.txtPriceRoom1.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & iTotRooms.ClientID & "','" & Me.ClientID & "','" & lblTotal.ClientID & "','" & PortalCulture.GetString("M000240") & "','" & txtPriceRoom1.ClientID & "');")
        Me.txtPriceRoom2.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & iTotRooms.ClientID & "','" & Me.ClientID & "','" & lblTotal.ClientID & "','" & PortalCulture.GetString("M000240") & "','" & txtPriceRoom1.ClientID & "');")
        Me.txtPriceRoom3.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & iTotRooms.ClientID & "','" & Me.ClientID & "','" & lblTotal.ClientID & "','" & PortalCulture.GetString("M000240") & "','" & txtPriceRoom1.ClientID & "');")
        Me.txtPriceRoom4.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & iTotRooms.ClientID & "','" & Me.ClientID & "','" & lblTotal.ClientID & "','" & PortalCulture.GetString("M000240") & "','" & txtPriceRoom1.ClientID & "');")
        Me.txtPriceRoom5.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & iTotRooms.ClientID & "','" & Me.ClientID & "','" & lblTotal.ClientID & "','" & PortalCulture.GetString("M000240") & "','" & txtPriceRoom1.ClientID & "');")
        Me.txtPriceRoom6.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & iTotRooms.ClientID & "','" & Me.ClientID & "','" & lblTotal.ClientID & "','" & PortalCulture.GetString("M000240") & "','" & txtPriceRoom1.ClientID & "');")
        Me.txtPriceRoom7.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & iTotRooms.ClientID & "','" & Me.ClientID & "','" & lblTotal.ClientID & "','" & PortalCulture.GetString("M000240") & "','" & txtPriceRoom1.ClientID & "');")
        Me.txtPriceRoom8.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & iTotRooms.ClientID & "','" & Me.ClientID & "','" & lblTotal.ClientID & "','" & PortalCulture.GetString("M000240") & "','" & txtPriceRoom1.ClientID & "');")
        Me.txtPriceRoom9.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & iTotRooms.ClientID & "','" & Me.ClientID & "','" & lblTotal.ClientID & "','" & PortalCulture.GetString("M000240") & "','" & txtPriceRoom1.ClientID & "');")
        Me.btnOk.Attributes.Add("onclick", "javascript:ok('TblPnl','PnlBox','" & iTotRooms.ClientID & "','" & Me.ClientID & "','" & PortalCulture.GetString("00651") & "');")
    End Sub
    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        Response.Write("<script>var ClientID='" & Me.ClientID & "' </script>")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

    End Sub

    Private ReadOnly Property getcancel() As String
        Get
            Return "cancel('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "')"
        End Get
    End Property

    Public Function getShow(ByVal obj As String, ByVal title As String, ByVal question As String) As String
        lblTitle.Text = title
        lblPrompt.Text = question
        btnCancel.Value = "No"
        btnOk.Value = PortalCulture.GetString("00030")
        btnCancel.Attributes.Add("onclick", getcancel)
        Dim hideCmbs As New System.Text.StringBuilder
        Return "<script>show('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "','" & obj & "');</script>"

    End Function
End Class
