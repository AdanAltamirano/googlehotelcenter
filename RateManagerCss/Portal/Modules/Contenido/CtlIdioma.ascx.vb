Imports Portal.Common
Imports Portal.Facade
Imports System.Configuration.ConfigurationManager
Partial Class CtlIdioma
    Inherits UserControlBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lnkSpanish As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkEnglish As System.Web.UI.WebControls.HyperLink
    Protected WithEvents td2 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td3 As System.Web.UI.HtmlControls.HtmlTableCell

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

	Enum idiomas
		Spanish = 1
		English = 2
	End Enum
    Private _Height As Integer = 0
    Private _Width As Integer = 0
    Public Property IsHTML() As Boolean
        Get
            Return viewstate("IsHTML")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("IsHTML") = Value
        End Set
    End Property
    Public Property IdIndice() As Integer
        Get
            Return viewstate("IdIndice")
        End Get
        Set(ByVal Value As Integer)
            viewstate("IdIndice") = Value
            If Value <> 0 Then CargaDatos(Value)
        End Set
    End Property

    Public Property textodefault() As String
        Get
            Return textDefault()
        End Get
        Set(ByVal Value As String)
            Select Case AppSettings("DefaultLanguage")
                Case "en-US"
                    If Me.txtenglish.Text.Trim() = "" Then
                        Me.txtenglish.Text = Value
                    End If
                Case "es-MX"
                    If Me.txtspanish.Text.Trim() = "" Then
                        Me.txtspanish.Text = Value
                    End If
            End Select
        End Set
    End Property

    Public Property textoIngles() As String
        Get
            Return Me.txtenglish.Text
        End Get
        Set(ByVal Value As String)
            Me.txtenglish.Text = Value
        End Set
    End Property
    Public Property textoInglesUpdate() As Boolean
        Get
            Return viewstate("ING_UPD")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("ING_UPD") = Value
        End Set
    End Property


    Public Property textoEspañol() As String
        Get
            Return Me.txtspanish.Text
        End Get
        Set(ByVal Value As String)
            Me.txtspanish.Text = Value
        End Set
    End Property

    Public Property textoEspañolUpdate() As Boolean
        Get
            Return viewstate("ESP_UPD")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("ESP_UPD") = Value
        End Set
    End Property

    Private Function textDefault() As String
        'TODO POR MIENTRAS
        Select Case AppSettings("DefaultLanguage")
            Case "en-US"
                Return Me.txtenglish.Text.Trim()
            Case "es-MX"
                Return Me.txtspanish.Text.Trim()
        End Select
    End Function

    Public Property Height() As Integer
        Get
            Return _Height
        End Get
        Set(ByVal Value As Integer)
            Me._Height = Value
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return _Width
        End Get
        Set(ByVal Value As Integer)
            _Width = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.DivSelect.Attributes("onClick") = " SelectTab('" & DivSelect.ClientID & "','" & DivUpload.ClientID & "','" & Me.tblenglish.ClientID & "','" & Me.tblspanish.ClientID & "'); SelectIdioma('" & IdiomaSelected.ClientID & "',0); "
        Me.DivUpload.Attributes("onClick") = " SelectTab('" & DivUpload.ClientID & "','" & DivSelect.ClientID & "','" & Me.tblspanish.ClientID & "','" & Me.tblenglish.ClientID & "'); SelectIdioma('" & IdiomaSelected.ClientID & "',1); "
        '        Me.IsHTML = True


    End Sub

    Public Property IsMultiline() As Boolean
        Get
            Return CBool(Me.txtenglish.TextMode)
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                Me.txtenglish.TextMode = TextBoxMode.MultiLine
                Me.txtspanish.TextMode = TextBoxMode.MultiLine
            Else
                Me.txtenglish.TextMode = TextBoxMode.SingleLine
                Me.txtspanish.TextMode = TextBoxMode.SingleLine
            End If
        End Set
    End Property

    Private Sub Cargarow(ByRef cd As DictionaryData, ByVal texto As String, ByVal idioma As Integer, ByVal pRow As DataRow)
        Dim dr As DataRow
        dr = cd.Tables(cd.TablaDiccionario).NewRow
        dr(cd.IdIdioma_FIELD) = idioma
        dr(cd.Indice_FIELD) = Me.IdIndice
        dr(cd.Texto_FIELD) = texto
        dr.SetParentRow(pRow)
        cd.Tables(cd.TablaDiccionario).Rows.Add(dr)
    End Sub

    Public Function Update(Optional ByVal idind As Integer = 0) As Integer
        Dim datDictionary As New DictionaryData
        Dim rowParent As DataRow
        Dim bInserted As Boolean

        ' create the new index parent row
        With datDictionary.IndexTable
            rowParent = .NewRow()
            ' add row to index table
            .Rows.Add(rowParent)
            ' assign dictionary id setting row status to modified 
            rowParent(DictionaryData.IndexTablefields.Indice) = idind
            ' set the  index table to unmodified state
            datDictionary.IndexTable.AcceptChanges()
        End With

        If Me.txtenglish.Text.Trim <> "" Then
            Me.Cargarow(datDictionary, Me.txtenglish.Text, idiomas.English, rowParent)
        End If
        If Me.txtspanish.Text.Trim <> "" Then
            Me.Cargarow(datDictionary, Me.txtspanish.Text, idiomas.Spanish, rowParent)
        End If

        With New SystemDictionary(AppSettings("PortalConnection"))
            bInserted = .InsertDictionary(datDictionary)
        End With

        '' if we could not insert the row, try tu update it 
        'If bInserted = False Then
        '	' change row status to modified
        '	datDictionary.AcceptChanges()
        '	rowDict(DictionaryData.DictionaryTableFields.IdIdioma) = rowDict(DictionaryData.DictionaryTableFields.IdIdioma)
        '	' update dataset
        '	With New SystemDictionary(m_strConnectionString)
        '		.UpdateDictionary(datDictionary, True)
        '	End With
        'End If

        Return idind
    End Function

    Public Sub CargaDatos(ByVal idindice As Integer)
        Dim cd As DictionaryData
        Dim dr As DataRow
        'Me.Limpia()
        Me.txtenglish.Text = ""
        Me.txtspanish.Text = ""
        With New SystemDictionary(AppSettings("PortalConnection"))
            cd = .GetDictionaryByID(idindice)
        End With
        If Not cd Is Nothing AndAlso cd.Tables(cd.TablaDiccionario).Rows.Count > 0 Then
            For Each dr In cd.Tables(cd.TablaDiccionario).Rows
                Select Case CType(dr(cd.IdIdioma_FIELD), idiomas)
                    Case idiomas.Spanish
                        txtspanish.Text = dr(cd.Texto_FIELD)
                    Case idiomas.English
                        txtenglish.Text = dr(cd.Texto_FIELD)
                End Select
            Next
        End If
    End Sub

    Public Sub Limpia()
        Me.IdIndice = 0
        Me.txtenglish.Text = ""
        Me.txtspanish.Text = ""
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        Dim sScript As New System.Text.StringBuilder
        If Not Page.IsStartupScriptRegistered("Allow2Tabs") Then
            sScript.Append("<SCRIPT language=""javascript"">" & vbCrLf)
            sScript.Append("function HideMe(id){" & vbCrLf)
            sScript.Append("	document.getElementById(id).style.display='none';}" & vbCrLf)
            sScript.Append(" function ShowMe(id) {" & vbCrLf)
            sScript.Append("	document.getElementById(id).style.display='block';}" & vbCrLf)
            sScript.Append(" function SelectTab(from, to,sel,upl){" & vbCrLf)
            sScript.Append("	document.getElementById(from).className='TabSelected'" & vbCrLf)
            sScript.Append("    document.getElementById(to).className='Tab';" & vbCrLf)
            sScript.Append("    document.getElementById(upl).style.display='none';" & vbCrLf)
            sScript.Append("    document.getElementById(sel).style.display='block';" & vbCrLf)
            sScript.Append("    document.getElementById(sel).focus();" & vbCrLf)
            sScript.Append("    };" & vbCrLf)
            sScript.Append("</SCRIPT>" & vbCrLf)
            Response.Write(sScript.ToString)
            Page.RegisterStartupScript("Allow2Tabs", "<SCRIPT></SCRIPT>")
        End If
        If Request.Form(IdiomaSelected.Name) = 1 Then  'vuelve a mostrar los panels. 
            Response.Write("<script lang=javascript> SelectTab('" & DivUpload.ClientID & "','" & DivSelect.ClientID & "','" & tblspanish.ClientID & "','" & tblenglish.ClientID & "'); </script>")
        End If
    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        txtspanish.Height = IIf(Me.Height = 0, Unit.Percentage(90), Unit.Pixel(Me.Height))
        txtspanish.Width = IIf(Me.Width = 0, Unit.Percentage(100), Unit.Pixel(Me.Width))
        txtenglish.Height = txtspanish.Height
        txtenglish.Width = txtspanish.Width

        tblspanish.Height = IIf(Me.Height = 0, "100%", Me.Height)
        tblspanish.Width = IIf(Me.Width = 0, "100%", Me.Width)
        tblenglish.Height = tblspanish.Height
        tblenglish.Width = tblspanish.Width
    End Sub
End Class