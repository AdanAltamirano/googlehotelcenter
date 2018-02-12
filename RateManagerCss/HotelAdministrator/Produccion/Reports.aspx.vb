Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Configuration.ConfigurationManager
Imports System.IO

Partial Class Reports
    Inherits System.Web.UI.Page

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
    Dim DBConnection As OleDbConnection
    Dim DBCommand As OleDbCommand
    Dim ConnectionString As String = AppSettings("HotelConnectionString")
    Dim Reports As DataSet

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Reports = New DataSet
        Reports.ReadXml(Server.MapPath("Reports.xml"))
        If Not Page.IsPostBack Then
            
            ddlSelecReport.DataSource = Reports
            ddlSelecReport.DataTextField = "Description"
            ddlSelecReport.DataValueField = "QueryID"
            ddlSelecReport.DataBind()
            ddlSelecReport.Items.Insert(0, New ListItem("Seleccione un reporte", -1))
            'ViewState("Reports") = Reports
        Else
            'Reports = ViewState("Reports")
        End If
        lbltitle.Text = PortalCulture.GetString("00259")
        lblSelectReport.Text = PortalCulture.GetString("01464")
        ddlSelecReport.Items(0).Text = PortalCulture.GetString("01464")
        btnExpReport.OnClientClick = "return FireUpdateStatus();"
    End Sub

    Private Sub btnGenReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '-- Bind the recordset to a control    
        If ddlSelecReport.SelectedValue <> -1 Then
            GridReport.DataSource = GetReport()
            GridReport.DataBind()
        End If
    End Sub
    Private Function GetReport() As DataSet
        'exec the query
        Dim data As New DataSet
        Dim dsCommand As New SqlDataAdapter
        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.Text
                    .CommandText = GetSqlStringReport()
                    .Connection = New SqlConnection(ConnectionString)
                End With
                .Fill(data)
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()
            End Try
        End With
        Return data
    End Function
    Private Function GetSqlStringReport() As String
        'Search query
        Dim Row As DataRow
        Dim col As DataColumn
        Dim SqlString As String = ""
        If ddlSelecReport.SelectedValue = -1 Then
            Return ""
        End If
        For Each Row In Reports.Tables("Report").Rows
            Dim Found As Boolean
            For Each col In Reports.Tables("Report").Columns
                If col.ColumnName = "QueryID" And Not Row(col) Is System.DBNull.Value AndAlso Row(col) = ddlSelecReport.SelectedValue Then
                    Found = True
                Else
                    If Found And col.ColumnName = "Query" Then
                        SqlString = Row(col)
                        Exit For
                    End If
                End If
            Next
            If Found Then
                Exit For
            End If
        Next
        Call ReplaceParameters(SqlString)
        Return SqlString
    End Function
    Private Sub btnExpReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExpReport.Click
        Dim dG As New DataGrid
        If ddlSelecReport.SelectedValue <> -1 Then
            dG.AllowPaging = False
            dG.AllowSorting = False
            dG.DataSource = GetReport()
            dG.DataBind()

            Response.Clear()
            Response.Buffer = True
            Response.ClearHeaders()
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("content-disposition", "attachment;filename=Reports.xls")
            Response.Charset = ""
            Response.ContentEncoding = System.Text.Encoding.Unicode
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
            Response.ContentType = "application/ms-excel"

            Me.EnableViewState = False
            Dim oStringWriter As StringWriter = New System.IO.StringWriter
            Dim oHtmlTextWriter As HtmlTextWriter = New System.Web.UI.HtmlTextWriter(oStringWriter)
            dG.RenderControl(oHtmlTextWriter)
            Response.Write(oStringWriter.ToString())
            Response.End()
        Else
            Return
        End If
    End Sub
    Private Sub ddlSelecReport_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSelecReport.SelectedIndexChanged
        If ddlSelecReport.SelectedValue <> -1 Then
            lblReportDesc.Text = ""
            Call LoadParameters()
            btnExpReport.Visible = True
        Else
            lblReportDesc.Text = ""
        End If
    End Sub
    Private Sub CheckParameters(ByRef NoParameters)
        NoParameters = 0
        Dim Row As DataRow
        Dim col As DataColumn
        For Each Row In Reports.Tables("Report").Rows
            Dim Found As Boolean
            For Each col In Reports.Tables("Report").Columns
                If col.ColumnName = "QueryID" And Not Row(col) Is System.DBNull.Value AndAlso Row(col) = ddlSelecReport.SelectedValue Then
                    Found = True
                ElseIf Found And col.ColumnName = "DescriptionExt" And Not Row(col) Is System.DBNull.Value Then
                    If Row(col) <> "" Then lblReportDesc.Text = "<b>Descripcion: </b>" & Row(col)
                ElseIf Found And col.ColumnName Like "NoParameters" Then
                    If Row(col) Is System.DBNull.Value Then
                        NoParameters = 0
                    Else
                        NoParameters = Row(col)
                    End If
                    Exit For
                End If
            Next
            If Found Then
                Exit For
            End If
        Next
    End Sub
    Private Sub LoadParameters()
        Dim NoParameters As Integer
        Dim NoParam As Integer
        NoParam = 1
        Call CheckParameters(NoParameters)
        If NoParameters <> 0 Then
            Dim Row As DataRow
            Dim col As DataColumn
            For Each Row In Reports.Tables("Report").Rows
                Dim Found As Boolean = False
                For Each col In Reports.Tables("Report").Columns
                    If col.ColumnName = "QueryID" And Not Row(col) Is System.DBNull.Value AndAlso Row(col) = ddlSelecReport.SelectedValue Then
                        Found = True
                    End If
                    If Found And col.ColumnName = ("Parameter" & CStr(NoParam)) Then
                        If Not Row(col) Is System.DBNull.Value Then
                            Select Case NoParam
                                Case 1
                                    lblParam1.Text = Row(col)
                                    lblParam1.Visible = True
                                    txtBParam1.Visible = True
                                Case 2
                                    lblParam2.Text = Row(col)
                                    lblParam2.Visible = True
                                    txtBParam2.Visible = True
                                Case 3
                                    lblParam3.Text = Row(col)
                                    lblParam3.Visible = True
                                    txtBParam3.Visible = True
                                Case 4
                                    lblParam4.Text = Row(col)
                                    lblParam4.Visible = True
                                    txtBParam4.Visible = True
                                Case 5
                                    lblParam5.Text = Row(col)
                                    lblParam5.Visible = True
                                    txtBParam5.Visible = True
                                Case 6
                                    lblParam6.Text = Row(col)
                                    lblParam6.Visible = True
                                    txtBParam6.Visible = True
                            End Select
                            NoParam += 1
                            lblDataReport.Visible = True
                        End If
                    End If
                Next
            Next
        End If
    End Sub
    Private Sub ReplaceParameters(ByRef SqlString)
        Dim NoParameters As Integer
        Dim NoParam As Integer
        NoParam = 1
        Call CheckParameters(NoParameters)
        If NoParameters <> 0 Then
            Dim Row As DataRow
            Dim col As DataColumn
            For Each Row In Reports.Tables("Report").Rows
                Dim Found As Boolean
                For Each col In Reports.Tables("Report").Columns
                    If col.ColumnName = "QueryID" And Not Row(col) Is System.DBNull.Value AndAlso Row(col) = ddlSelecReport.SelectedValue Then
                        Found = True
                    ElseIf Found And col.ColumnName = ("Parameter" & CStr(NoParam)) Then
                        If Not Row(col) Is System.DBNull.Value Then
                            Select Case NoParam
                                Case 1
                                    SqlString = Replace(SqlString, "@@@param1", Trim(txtBParam1.Text))
                                Case 2
                                    SqlString = Replace(SqlString, "@@@param2", Trim(txtBParam2.Text))
                                Case 3
                                    SqlString = Replace(SqlString, "@@@param3", Trim(txtBParam3.Text))
                                Case 4
                                    SqlString = Replace(SqlString, "@@@param4", Trim(txtBParam4.Text))
                                Case 5
                                    SqlString = Replace(SqlString, "@@@param5", Trim(txtBParam5.Text))
                                Case 6
                                    SqlString = Replace(SqlString, "@@@param6", Trim(txtBParam6.Text))
                            End Select
                            NoParam += 1
                        End If
                    End If
                Next
                If Found Then
                    Exit For
                End If
            Next
        End If
    End Sub
End Class
