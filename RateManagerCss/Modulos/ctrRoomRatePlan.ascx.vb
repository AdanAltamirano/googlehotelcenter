Partial Class ctrRoomRatePlan
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ddlRoomType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlRatePlanType As System.Web.UI.WebControls.DropDownList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public WriteOnly Property Editable() As Boolean
        Set(ByVal Value As Boolean)
            lblDay.Enabled = Value
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
            Me.lblDay.ForeColor = Value
        End Set
    End Property
    Public Property Day() As Integer
        Get
            Return CInt(viewstate("_midia"))
        End Get
        Set(ByVal Value As Integer)
            viewstate("_midia") = Value
            Me.lblDay.Text = Value
            Dim selectDate As Date
            With CType(Me.Page, SegmentRoomsAvailability)
                Try
                    selectDate = New Date(.SelectedYear, CInt(.SelectedMes + 1), Me.Day)
                Catch ex As Exception
                End Try
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.txtRooms.Attributes.Add("onchange", "javascript:SaveAvaCalendar ('" & txtRooms.ClientID & "','" & lblDay.Text & "')")
        ''''Me.lnkRooms.NavigateUrl = "javascript:ShowTxtCalendario('" & txtRooms.ClientID & "','" & Me.lnkRooms.ClientID & "')"
        'Me.lnkRooms.NavigateUrl = "javascript:ShowTxtCalendario('" & txtRooms.ClientID & "','" & Me.lnkRooms.ClientID & "','" & lblDay.Text & "')"
    End Sub

    Public Function Show(ByVal valor As Boolean)
        lblDay.Visible = valor
        Me.lblAvailability.Visible = valor
        Me.lblAvailabilityByRoom.Visible = valor
        Me.lnkRooms.Style.Add("display", "none")
        If valor = True Then
            Me.lnkRooms.Style.Add("display", "")
        End If
        txtRooms.Style.Add("display", "none")
    End Function

    Public Sub showDay(ByVal valor As Boolean)
        lblDay.Visible = valor
    End Sub

    Public Sub loaddatos(ByVal Sold As Integer, ByVal Ava As Integer, ByVal tooltipTarifa As String, Optional ByVal rackrate As Double = 0)
        lblAvailability.Text = Sold & " /"
        lblAvailability.ToolTip = tooltipTarifa

        Me.txtRooms.Text = Ava
        Me.txtRooms.ToolTip = tooltipTarifa

        Me.lnkRooms.Text = Ava
        Me.lnkRooms.ToolTip = tooltipTarifa

        lblAvailabilityByRoom.Text = ""
    End Sub


    Public Sub loaddatos(ByVal Sold As Integer, ByVal Ava As Integer, ByVal AvailRoom As Integer, ByVal tooltipTarifa As String, ByVal rackrate As Double)
        lblAvailability.Text = String.Format("{0} /", Sold)
        lblAvailability.ToolTip = tooltipTarifa

        Me.txtRooms.Text = Ava
        Me.txtRooms.ToolTip = tooltipTarifa

        Me.lblAvailabilityByRoom.Text = String.Format("/ {0}", AvailRoom)
        Me.lnkRooms.Text = Ava
        Me.lnkRooms.ToolTip = tooltipTarifa
    End Sub


    Public Sub loaddatos(ByVal Text As String, ByVal tooltipTarifa As String)
        lblAvailability.Text = Text
        lblAvailability.ToolTip = tooltipTarifa
        lblAvailabilityByRoom.Text = ""
    End Sub

    Private Sub lnkRooms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Me.txtRooms.Visible = True
        Me.lnkRooms.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.txtRooms.Attributes.Add("onchange", "javascript:SaveAvaCalendar ('" & txtRooms.ClientID & "','" & lblDay.Text & "')")
        Me.lnkRooms.NavigateUrl = "javascript:ShowTxtCalendario('" & txtRooms.ClientID & "','" & Me.lnkRooms.ClientID & "','" & lblDay.Text & "')"
    End Sub
End Class
