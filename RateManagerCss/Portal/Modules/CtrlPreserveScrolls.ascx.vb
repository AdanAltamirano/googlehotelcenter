Imports System.Text
Partial Class CtrlPreserveScrolls
    Inherits System.Web.UI.UserControl

    Public Enum TypeControl
        DIV = 0
        THEWINDOW = 1
    End Enum

    Public Property NamesControls() As ArrayList
        Get
            Return ViewState("NamesControls")
        End Get
        Set(ByVal Value As ArrayList)
            ViewState("NamesControls") = Value
        End Set
    End Property

    Public Property ModesNamesControls() As ArrayList
        Get
            Return ViewState("ModesNamesControls")
        End Get
        Set(ByVal Value As ArrayList)
            ViewState("ModesNamesControls") = Value
        End Set
    End Property

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            'Put user code to initialize the page here
            If Not NamesControls Is Nothing Then
                Dim dt As New DataTable("Scrolls")
                Dim dra As DataRow
                With dt.Columns
                    .Add(New DataColumn("StaticPostBackScrollVerticalPosition", GetType(System.String)))
                    .Add(New DataColumn("StaticPostBackScrollHorizontalPosition", GetType(System.String)))
                End With
                For i As Integer = 0 To Me.NamesControls.Count - 1
                    dra = dt.NewRow()
                    With dra
                        .Item("StaticPostBackScrollVerticalPosition") = 0
                        .Item("StaticPostBackScrollHorizontalPosition") = 0
                    End With
                    dt.Rows.Add(dra)
                Next
                With Me.DG
                    .DataSource = dt
                    .DataBind()
                End With
                Me.DG.Style.Item("display") = "none"
            End If
        End If
    End Sub

    Public Sub Add(ByVal YourTypeControl As TypeControl, Optional ByVal NameControl As String = "")
        If NamesControls Is Nothing Then
            NamesControls = New ArrayList
            ModesNamesControls = New ArrayList
        End If
        If NameControl = "" Then
            If YourTypeControl = TypeControl.THEWINDOW Then
                If ModesNamesControls.IndexOf(YourTypeControl.ToString) >= 0 Then
                    Response.Write("<b>" & YourTypeControl.ToString & " is already added.</b><br>")
                    Exit Sub
                Else
                    NamesControls.Add(YourTypeControl.ToString)
                    ModesNamesControls.Add(YourTypeControl.ToString)
                    Exit Sub
                End If
            Else
                Response.Write("Solo se puede agregar un parametro de tipo WINDOW y no DIV")
                Exit Sub
            End If
        Else
            If NamesControls.IndexOf(NameControl) >= 0 Then
                Response.Write("<b>" & NameControl.ToString & " is already added.</b><br>")
                Exit Sub
            Else
                NamesControls.Add(NameControl)
                ModesNamesControls.Add(YourTypeControl.ToString)
                Exit Sub
            End If
        End If
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Not Page.IsClientScriptBlockRegistered("scriptScrolls") Then
            If DG.Items.Count > 0 Then
                Dim s As New StringBuilder
                s.Append("<script>" & ControlChars.CrLf)
                s.Append("               var max = " & Me.NamesControls.Count & ";" & ControlChars.CrLf)
                s.Append("               var controlNames = new Array(" & Me.NamesControls.Count & ")" & ControlChars.CrLf)
                s.Append("               var  ModesNamesControls = new Array(" & Me.ModesNamesControls.Count & ")" & ControlChars.CrLf)
                s.Append("               var  NamesInVerticalPosition = new Array(" & Me.NamesControls.Count & ")" & ControlChars.CrLf)
                s.Append("               var  NamesInHorizontalPosition = new Array(" & Me.NamesControls.Count & ")" & ControlChars.CrLf)
                For i As Integer = 0 To DG.Items.Count - 1
                    With DG.Items(i)
                        Dim VerticalPosition As TextBox
                        Dim Horizontal As TextBox
                        VerticalPosition = .Cells(0).FindControl("txtStaticPostBackScrollVerticalPosition")
                        Horizontal = .Cells(1).FindControl("txtStaticPostBackScrollHorizontalPosition")
                        If Not VerticalPosition Is Nothing Then
                            s.Append("               controlNames[" & i & "] = '" & Me.NamesControls.Item(i) & "'; " & ControlChars.CrLf)
                            s.Append("               NamesInVerticalPosition[" & i & "] = '" & VerticalPosition.ClientID & "';" & ControlChars.CrLf)
                            s.Append("               NamesInHorizontalPosition[" & i & "] = '" & Horizontal.ClientID & "';" & ControlChars.CrLf)
                            s.Append("               ModesNamesControls[" & i & "] = '" & ModesNamesControls.Item(i) & "';" & ControlChars.CrLf)
                        End If
                    End With
                Next
                s.Append("       </script>" & ControlChars.CrLf)
                Page.RegisterClientScriptBlock("scriptScrolls", s.ToString())
            End If
        End If
    End Sub
End Class
