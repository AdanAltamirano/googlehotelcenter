Imports System.Data.SqlClient

Partial Class rsReport
    Inherits PaginaBase

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

    Private Property Filtro() As String
        Get
            Return ViewState("Filtro")
        End Get
        Set(ByVal Value As String)
            ViewState("Filtro") = Value
        End Set
    End Property

#Region "Calendario"

    Private Function htmlWriteCalendar() As Byte
        'Page.RegisterStartupScript("frameCalendar", "<iframe width=""174"" height=""189"" name=""gToday:normal:agenda.js"" id=""gToday:normal:agenda.js"" src=""" & GeRequestApplicationPath("/Calendar/es/ipopeng.htm") & """ scrolling=""no"" frameborder=""0"" style=""Z-INDEX:999; LEFT:-500px; VISIBILITY:visible; POSITION:absolute; TOP:-500px""></iframe>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "frameCalendar", "<iframe width=""174"" height=""189"" name=""gToday:normal:agenda.js"" id=""gToday:normal:agenda.js"" src=""" & GeRequestApplicationPath("/Calendar/es/ipopeng.htm") & """ scrolling=""no"" frameborder=""0"" style=""Z-INDEX:999; LEFT:-500px; VISIBILITY:visible; POSITION:absolute; TOP:-500px""></iframe>")
        Return 0
    End Function

    Private Function htmlSetCalendar(ByVal elementToRender As Literal, ByVal elementClientIdToGetSetDate As String) As Byte
        Dim htmlCal As String = "<a href=""javascript:void(0)"" onclick=""if(self.gfPop)gfPop.fPopCalendar(" & elementClientIdToGetSetDate & ");return false;"" HIDEFOCUS><img class=""PopcalTrigger"" align=""absMiddle"" src=""" & GeRequestApplicationPath("/Calendar/calbtn.gif") & """ border=""0"" alt=""""></a>"
        elementToRender.Text = htmlCal
        Return 0
    End Function

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            txtInicio.Value = Now.ToString("MM/dd/yyyy")
            txtFin.Value = Now.ToString("MM/dd/yyyy")
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        htmlWriteCalendar()
        ddlStatus.Enabled = cbStatus.Checked
        ddlGds.Enabled = cbGDS.Checked
        htmlSetCalendar(ltInicio, "txtInicio")
        htmlSetCalendar(ltFin, "txtFin")
        txtInicio.Attributes.Add("onclick", "if(self.gfPop)gfPop.fPopCalendar(txtInicio);return false;")
        txtFin.Attributes.Add("onclick", "if(self.gfPop)gfPop.fPopCalendar(txtFin);return false;")
        If cbFechas.Checked Then
            txtInicio.Disabled = False
            txtFin.Disabled = False
        Else
            txtInicio.Disabled = True
            txtFin.Disabled = True
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        dgLogs.CurrentPageIndex = 0
        dgLogs.SelectedIndex = -1
        Enlazar()
    End Sub

    Private Sub Enlazar()
        Dim pdsLogs As New PagedDataSource
        Dim dt As New DataTable("logs")
        dt = CargarLogs()
        Filtro = ""
        'filtro x status ...
        If cbStatus.Checked Then
            If ddlStatus.SelectedValue = 1 Then
                Filtro = "MSG = ''"
            ElseIf ddlStatus.SelectedValue = 2 Then
                Filtro = "MSG <> ''"
            End If
        End If
        'filtro x fechas ...
        If cbFechas.Checked Then
            If CDate(txtFin.Value) < CDate(txtInicio.Value) Then txtFin.Value = txtInicio.Value
            If Filtro = "" Then
                Filtro = "DS >= '" & CDate(txtInicio.Value).ToString("dd/MMM/yyyy").ToUpper & "' AND DS <= '" & CDate(txtFin.Value).ToString("dd/MMM/yyyy").ToUpper & "'"
            Else
                Filtro &= " AND DS >= '" & CDate(txtInicio.Value).ToString("dd/MMM/yyyy").ToUpper & "' AND DS <= '" & CDate(txtFin.Value).ToString("dd/MMM/yyyy").ToUpper & "'"
            End If
        End If
        'filtro x GDS ...
        If cbGDS.Checked Then
            If ddlGds.SelectedValue <> "XX" Then
                If Filtro = "" Then
                    Filtro = "GDS = '" & ddlGds.SelectedValue & "'"
                Else
                    Filtro &= " AND GDS = '" & ddlGds.SelectedValue & "'"
                End If
            End If
        End If
        'aqui se aplica el filtro generado ...
        dt.DefaultView.RowFilter = Filtro
        dgLogs.DataSource = dt.DefaultView
        dgLogs.DataBind()
    End Sub

    Private Function CargarLogs() As DataTable
        Dim cn As SqlConnection = New SqlConnection(ConexionSQL)
        Dim cmd As SqlCommand
        Dim da As SqlDataAdapter
        Dim dt As New DataTable("Logs")
        Dim sQry As String = "Select IdLog, PN, Convert(Datetime, DS) DS, Convert(Datetime, TS) TS, " & _
            "Convert(Datetime, [IN]) [IN], Convert(Datetime, OT) OT, NA, NC, RSP, TX, MSG, GDS, AGY " & _
            "From wzSeamlessLog Order By DS, TS"

        Try
            cn.Open()
            cmd = New SqlCommand(sQry, cn)
            cmd.CommandType = CommandType.Text
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)
        Catch ex As Exception
            dt = New DataTable("Logs")
        Finally
            CargarLogs = dt
            cn.Close()
        End Try
    End Function

    Private Sub dgLogs_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgLogs.PageIndexChanged
        dgLogs.CurrentPageIndex = e.NewPageIndex
        Enlazar()
    End Sub

End Class
