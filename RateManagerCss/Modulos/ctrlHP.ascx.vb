Partial Class ctrlHP
    Inherits UserControlBase
    Public WriteOnly Property Editable() As Boolean
        Set(ByVal Value As Boolean)
            lnkDay.Enabled = Value
        End Set
    End Property
    Public WriteOnly Property ShowLink() As Boolean
        Set(ByVal Value As Boolean)
            lnkRooms.Visible = Value
        End Set
    End Property

    Public WriteOnly Property status() As System.Drawing.Color
        Set(ByVal Value As System.Drawing.Color)
            Me.txtRooms.ForeColor = Value
            Me.lblAvailability.ForeColor = Value
            Me.lnkRooms.ForeColor = Value
            lblRackRate.ForeColor = Value
            lblMenor.ForeColor = Value
            Me.lnkDay.ForeColor = Value
            'lbllowestcode.ForeColor = Value
        End Set
    End Property
    Public Property Day() As Integer
        Get
            Return CInt(viewstate("_midia"))
        End Get
        Set(ByVal Value As Integer)
            viewstate("_midia") = Value
            Me.lnkDay.Text = Value
            Dim selectDate As Date
            With CType(Me.Page, HomePage)
                Try
                    selectDate = New Date(.SelectedYear, CInt(.SelectedMes + 1), Me.Day)
                Catch ex As Exception
                    lnkDay.NavigateUrl = GeRequestApplicationPath("/Pages/SegmentRoomsAvailability.aspx")
                End Try
                lnkDay.NavigateUrl = GeRequestApplicationPath(String.Concat("/Pages/SegmentRoomsAvailability.aspx?Date=", selectDate))
            End With
        End Set
    End Property

    Public Property rooms() As String
        Get
            Return Me.txtRooms.Text
        End Get
        Set(ByVal Value As String)
            Me.txtRooms.Text = Value
        End Set
    End Property

    Public Property change() As String
        Get
            Return Me.txtCambia.Text
        End Get
        Set(ByVal Value As String)
            Me.txtCambia.Text = Value
        End Set
    End Property


#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lnkDay3 As System.Web.UI.WebControls.LinkButton
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
        Me.txtRooms.Attributes.Add("onchange", "javascript:CambiaTxt('" & txtCambia.ClientID & "')")
        Me.lnkRooms.NavigateUrl = "javascript:ShowTxt('" & txtRooms.ClientID & "','" & Me.lnkRooms.ClientID & "')"

    End Sub

    Public Function Show(ByVal valor As Boolean)

        lnkDay.Visible = valor
        Me.lblRackRate.Visible = valor
        If ConfigurationManager.AppSettings("idSegmento") = 4 Then
            Me.lblMenor.Visible = False
        Else
            Me.lblMenor.Visible = valor
        End If
        Me.lblAvailability.Visible = valor
        'lbllowestcode.Visible = valor
        Me.lnkRooms.Style.Add("display", "none")
        If valor = True Then
            Me.lnkRooms.Style.Add("display", "")
        End If
        txtRooms.Style.Add("display", "none")
    End Function

    Public Sub loaddatos(ByVal Sold As String, ByVal Ava As Integer, Optional ByVal rackrate As Double = 0)
        lblAvailability.Text = Sold & " /"
        Me.txtRooms.Text = Ava
        Me.lnkRooms.Text = Ava
        Me.lblRackRate.Text = FCurrency(rackrate, 2)
    End Sub
    Public Sub loaddatos(ByVal Text As String)
        lblAvailability.Text = Text
    End Sub
    Public Sub loadRate(ByVal value As String, ByVal value2 As String)
        lblMenor.Text = value
        lblMenor.ToolTip = value2
        '        lbllowestcode.Text = value2
    End Sub
    Public Sub loadRackRate(ByVal value As String)
        lblRackRate.Text = value
    End Sub


    Private Sub lnkRooms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Me.txtRooms.Visible = True
        Me.lnkRooms.Visible = False
    End Sub
End Class
