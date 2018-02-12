Option Explicit On 
Option Strict On
Imports System.Text
Partial Class _Date
    Inherits UserControlBase

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

    Private _MaxYear As Integer
    Private _MinYear As Integer
    Public Enum Months As Byte
        january = 1
        february = 2
        march = 3
        april = 4
        may = 5
        june = 6
        july = 7
        august = 8
        september = 9
        october = 10
        november = 11
        december = 12
    End Enum
    Public Property maxYear() As Integer
        Get
            If _MaxYear = 0 Then
                _MaxYear = Now.Year
            End If
            Return _MaxYear
        End Get
        Set(ByVal Value As Integer)
            If (Value > Year(Date.MaxValue) OrElse Value < Year(Date.MinValue)) Then
                _MaxYear = Now.Year
            Else
                _MaxYear = Value
            End If
        End Set
    End Property
    Public Property minYear() As Integer
        Get
            If _MinYear = 0 Then
                _MinYear = Now.Year
            End If

            Return _MinYear
        End Get
        Set(ByVal Value As Integer)
            If (Value > Year(Date.MaxValue) OrElse Value < Year(Date.MinValue)) Then
                _MinYear = Now.Year
            Else
                _MinYear = Value
            End If
        End Set
    End Property
    Public Property selectedYear() As Integer
        Get
            If Me.lstYear.Items.Count = 0 Then Me.loadYears(Me.lstYear)
            Return Integer.Parse(Me.lstYear.SelectedValue)
        End Get
        Set(ByVal Value As Integer)
            If (Value > Year(Date.MaxValue) OrElse Value < Year(Date.MinValue)) Then

            Else
                If Me.lstYear.Items.Count = 0 Then Me.loadYears(Me.lstYear)
                Me.lstYear.SelectedValue = Value.ToString
            End If
        End Set
    End Property
    Public Property selectedMonth() As Months
        Get
            If Me.lstMonth.Items.Count < 12 Then Me.loadMonths(Me.lstMonth)
            Return CType(Me.lstMonth.SelectedValue, Months)
        End Get
        Set(ByVal Value As Months)
            If Me.lstMonth.Items.Count < 12 Then Me.loadMonths(Me.lstMonth)
            Me.lstMonth.SelectedValue = CType(Value, Byte).ToString
        End Set
    End Property
    Public Property typeDate() As Months
        Get
            If Me.lstMonth.Items.Count < 12 Then Me.loadMonths(Me.lstMonth)
            Return CType(Me.lstMonth.SelectedValue, Months)
        End Get
        Set(ByVal Value As Months)
            If Me.lstMonth.Items.Count < 12 Then Me.loadMonths(Me.lstMonth)
            Me.lstMonth.SelectedValue = CType(Value, Byte).ToString
        End Set
    End Property
    Public Property selectedDay() As Byte
        Get
            If Me.lstDay.Items.Count = 0 Then Me.loadDays(Me.lstDay)
            Return Byte.Parse(Me.lstDay.SelectedValue)
        End Get
        Set(ByVal Value As Byte)
            If Me.lstDay.Items.Count = 0 Then Me.loadDays(Me.lstDay)
            Me.lstDay.SelectedValue = Value.ToString
        End Set
    End Property
    Public Property selectedDate() As DateTime
        Get
            Return getDate()
        End Get
        Set(ByVal value As DateTime)
            setDate(value)
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            If Me.lstYear.Items.Count = 0 Then Me.loadYears(Me.lstYear)
            If Me.lstMonth.Items.Count = 0 Then Me.loadMonths(Me.lstMonth)
            If Me.lstDay.Items.Count = 0 Then Me.loadDays(Me.lstDay)
            Me.lstYear.Attributes.Add("onChange", "javascript:renderDay('" & Me.lstDay.ClientID & "', '" & Me.lstMonth.ClientID & "', '" & Me.lstYear.ClientID & "')")
            Me.lstMonth.Attributes.Add("onChange", "javascript:renderDay('" & Me.lstDay.ClientID & "', '" & Me.lstMonth.ClientID & "', '" & Me.lstYear.ClientID & "')")
        End If
    End Sub

    Private Sub loadDays(ByVal lst As System.Web.UI.WebControls.DropDownList)
        Dim day As System.Web.UI.WebControls.ListItem
        Dim i As Integer
        If lst.Items.Count > 0 Then lst.Items.Clear()
        For i = 1 To 31
            day = New System.Web.UI.WebControls.ListItem
            day.Text = i.ToString
            day.Value = i.ToString
            lst.Items.Add(day)
        Next
        lst.SelectedIndex = 0
    End Sub
    'Cargamos la lista con el año
    Private Sub loadYears(ByVal lst As System.Web.UI.WebControls.DropDownList)
        Dim year As System.Web.UI.WebControls.ListItem
        Dim i As Integer
        If lst.Items.Count > 0 Then lst.Items.Clear()
        For i = minYear To maxYear
            year = New System.Web.UI.WebControls.ListItem
            year.Text = i.ToString
            year.Value = i.ToString
            lst.Items.Add(year)
        Next
        lst.SelectedIndex = 0
    End Sub

    'Cargamos una lista con el mes
    Private Sub loadMonths(ByVal lst As System.Web.UI.WebControls.DropDownList)
        Dim month As System.Web.UI.WebControls.ListItem
		Dim selectMonth As Integer
		If lst.Items.Count = 12 Then
			selectMonth = CType(lst.SelectedValue, Integer)
		Else
			selectMonth = Now.Month
		End If
		lst.Items.Clear()
		Dim i As Byte
		For i = 1 To 12
			month = New System.Web.UI.WebControls.ListItem
			month.Text = New Date(Now.Year, i, 1).ToString("MMMM", PortalCulture.GetCulture)
			month.Value = i.ToString
			lst.Items.Add(month)
		Next
		lst.SelectedValue = selectMonth.ToString
    End Sub

    'Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
    '    'writer.Write("<script src=""" &  "/ListDate/listDate.js""></script>")
    '    'MyBase.Render(writer)
    '    'writer.Write("<script>" & _
    '    '"try{" & _
    '    '"renderDay('" & Me.lstDay.ClientID & "', '" & Me.lstMonth.ClientID & "', '" & Me.lstYear.ClientID & "')" & _
    '    '"}catch(ex) {}" & _
    '    '"</script>")


    'End Sub

    Private Function getDate() As DateTime
        Dim day As Integer
        Dim month As Integer
        Dim year As Integer
        Dim selectDate As DateTime
        Try
            day = Integer.Parse(Me.lstDay.SelectedValue)
            month = Integer.Parse(Me.lstMonth.SelectedValue)
            year = Integer.Parse(Me.lstYear.SelectedValue)
            selectDate = New Date(year, month, day)
        Catch ex As Exception
            selectDate = Now
        End Try
        getDate = selectDate
    End Function

    Private Function setDate(ByVal _date As DateTime) As Boolean
        Dim selectDate As DateTime
        Try
            Me.selectedDay = CType(_date.Day, Byte)
            Me.selectedMonth = CType(_date.Month, Months)
            Me.selectedYear = CType(_date.Year, Integer)
            Return True
        Catch ex As Exception
            selectDate = Now
            Return False
        End Try
    End Function

    Private Sub ScriptRegister()
        Dim script As StringBuilder = New StringBuilder()
        Dim scriptUrl As String

        scriptUrl = GeRequestApplicationPath("/ListDate/listDate.js")
        script.Append(String.Format("<script type=""text/javascript"" src='{0}'></script>", scriptUrl))
        script.Append("<SCRIPT type=""text/javascript""> ")
        script.Append("try{")
        script.Append(String.Format("renderDay('{0}','{1}','{2}');", Me.lstDay.ClientID, Me.lstMonth.ClientID, Me.lstYear.ClientID))
        script.Append("}catch(ex) {}")
        script.Append("</SCRIPT>")

        Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, script.ToString())
    End Sub

	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'Para que carge el idioma
        Me.loadMonths(Me.lstMonth)
        ScriptRegister()
	End Sub
End Class

