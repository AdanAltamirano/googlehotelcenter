Imports Portal.Common
Imports Portal.Facade
Imports System.Configuration.ConfigurationManager
Partial Class CtrlIdioma
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
    Private _ShowErrorText As Boolean = True
    'Private _RequiredText As Boolean = True
    Public Property RequiredText() As Boolean
        Get
            Return Me.rfvDefaultText.Enabled
            'Return _RequiredText
        End Get
        Set(ByVal Value As Boolean)
            '_RequiredText = Value
            Me.rfvDefaultText.Enabled = Value
            Me.rfvDefaultText2.Enabled = Value
        End Set
    End Property

    Public Overrides ReadOnly Property ClientID() As String
        Get
            Return Me.tblGen.ClientID
        End Get
    End Property

    Public Property ShowErrorText() As Boolean
        Get
            Return _ShowErrorText
        End Get
        Set(ByVal Value As Boolean)
            _ShowErrorText = Value
        End Set
    End Property
    Public ReadOnly Property ReturnNameTxtEn() As String
        Get
            Return Me.txtenglish.ClientID
        End Get
    End Property
    Public ReadOnly Property ReturnNameTxtEs() As String
        Get
            Return Me.txtspanish.ClientID
        End Get
    End Property
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
            Dim val As Integer = 0
            Integer.TryParse(ViewState("IdIndice"), val)
            Return val
        End Get
        Set(ByVal Value As Integer)
            Dim val As Integer = 0
            Integer.TryParse(ViewState("IdIndice"), val)
            If val <> Value Then
                ViewState("IdIndice") = Value
                If Value > 0 Then
                    CargaDatos(Value)
                End If
            End If
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

    '***27/08/05**********funcion que deshabilita el control del idioma
    Public Property deshabilita() As Boolean
        Get

        End Get
        Set(ByVal Value As Boolean)
            Me.txtenglish.Enabled = Value
            Me.txtspanish.Enabled = Value
        End Set
    End Property

    'T********************************

    Private Function textDefault() As String
        'TODO POR MIENTRAS
        Select Case AppSettings("DefaultLanguage")
            Case "en-US"
                Return Me.txtenglish.Text.Trim()
            Case "es-MX"
                Return Me.txtspanish.Text.Trim()
        End Select
        Return ""
    End Function

    Public Property Height() As Integer
        Get
            Return viewstate("_Height")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_Height") = Value
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return viewstate("_Width")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_Width") = Value
        End Set
    End Property


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.DivSelect.Attributes("onClick") = " SelectTab('" & DivSelect.ClientID & "','" & DivUpload.ClientID & "','" & Me.tblenglish.ClientID & "','" & Me.tblspanish.ClientID & "'); SelectIdioma('" & IdiomaSelected.ClientID & "',0); "
        Me.DivUpload.Attributes("onClick") = " SelectTab('" & DivUpload.ClientID & "','" & DivSelect.ClientID & "','" & Me.tblspanish.ClientID & "','" & Me.tblenglish.ClientID & "'); SelectIdioma('" & IdiomaSelected.ClientID & "',1); "
        '        Me.IsHTML = True
        'If Me.RequiredText = False Then
        'rfvDefaultText.ControlToValidate = txtAux.ID
        'rfvDefaultText2.ControlToValidate = txtAux.ID
        'Else
        'Select Case AppSettings("DefaultLanguage")
        '    Case "en-US"
        '        rfvDefaultText.ControlToValidate = txtenglish.ID
        '    Case "es-MX"
        '        rfvDefaultText.ControlToValidate = txtspanish.ID
        'End Select
        rfvDefaultText.ControlToValidate = txtenglish.ID
        rfvDefaultText2.ControlToValidate = txtspanish.ID
        'End If
    End Sub

    Private Sub Cargarow(ByRef cd As DictionaryData, ByVal texto As String, ByVal idioma As Integer, ByVal pRow As DataRow)
        Dim dr As DataRow
        dr = cd.Tables(DictionaryData.TablaDiccionario).NewRow
        dr(DictionaryData.IdIdioma_FIELD) = idioma
        dr(DictionaryData.Indice_FIELD) = Me.IdIndice
        dr(DictionaryData.Texto_FIELD) = texto
        dr.SetParentRow(pRow)
        cd.Tables(DictionaryData.TablaDiccionario).Rows.Add(dr)
    End Sub
    Public Function SetEN(ByVal value As String) As String
        Me.txtenglish.Text = value
    End Function
    Public Function setES(ByVal value As String) As String
        Me.txtspanish.Text = value
    End Function

    Public Function GetEN(Optional ByVal value As String = "") As String
        If Me.txtenglish.Text <> "" Then
            Return Me.txtenglish.Text.Trim()
        Else
            Me.txtenglish.Text = value
        End If
    End Function

    Public Function GetES(Optional ByVal value As String = "") As String
        If Me.txtspanish.Text <> "" Then
            Return Me.txtspanish.Text
        Else
            Me.txtspanish.Text = value
        End If
    End Function

    Public Function Insert(Optional ByVal idind As Integer = 0) As Integer
        Dim datDictionary As New DictionaryData
        Dim rowParent As DataRow
        Dim bInserted As Boolean
        ' create the new index parent row
        With datDictionary.IndexTable
            rowParent = .NewRow()
            ' add row to index table
            rowParent(DictionaryData.IndexTablefields.Indice) = idind
            .Rows.Add(rowParent)

            ' set the  index table to unmodified state
            'datDictionary.IndexTable.AcceptChanges()
        End With


        If Me.txtenglish.Text.Trim <> "" Then
            Me.Cargarow(datDictionary, Me.txtenglish.Text, idiomas.English, rowParent)
        End If
        If Me.txtspanish.Text.Trim <> "" Then
            Me.Cargarow(datDictionary, Me.txtspanish.Text, idiomas.Spanish, rowParent)
        End If

        With New SystemDictionary(AppSettings("HotelConnection"))
            bInserted = .InsertNewDictionary(datDictionary)
            If bInserted Then
                Return datDictionary.Tables(DictionaryData.TablaIndice).Rows(0).Item(DictionaryData.IdDiccionario_FIELD)
            End If
        End With
        Return 0
    End Function

    Public ReadOnly Property HasChanges() As Boolean
        Get
            Dim flag As Boolean = False
            Boolean.TryParse(Me.ViewState(Me.ID + "_HasChange"), flag)
            Return flag
        End Get
    End Property

    Public Function Update(ByVal idind As Integer, Optional ByVal publish As Boolean = True) As Integer
        Dim datDictionary As New DictionaryData
        Dim rowParent As DataRow
        Dim bInserted As Boolean
        ' create the new index parent row
        With datDictionary.IndexTable
            rowParent = .NewRow()
            ' add row to index table
            .Rows.Add(rowParent)
            ' assign dictionary id setting row status to modified 
            rowParent(DictionaryData.IndexTablefields.Indice) = idind ' If(Me.IdIndice = 0, idind, Me.IdIndice)
            ' set the  index table to unmodified state
            datDictionary.IndexTable.AcceptChanges()
        End With

        If Not Me.RequiredText OrElse Me.txtenglish.Text.Trim <> "" Then
            Me.Cargarow(datDictionary, Me.txtenglish.Text, idiomas.English, rowParent)
        End If
        If Not Me.RequiredText OrElse Me.txtspanish.Text.Trim <> "" Then
            Me.Cargarow(datDictionary, Me.txtspanish.Text, idiomas.Spanish, rowParent)
        End If

        Dim has As Boolean = False
        With New SystemDictionary(AppSettings("HotelConnection"))
            bInserted = .InsertDictionary(datDictionary, publish, has)
        End With
        Me.ViewState(Me.ID + "_Published") = publish
        Me.ViewState(Me.ID + "_HasChange") = has
        Return idind
    End Function

    Public Function Update(ByVal txtEng As String, ByVal txtEsp As String, Optional ByVal idind As Integer = 0, Optional ByVal publish As Boolean = True) As Integer
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

        Me.Cargarow(datDictionary, txtEng, idiomas.English, rowParent)
        Me.Cargarow(datDictionary, txtEsp, idiomas.Spanish, rowParent)


        Dim has As Boolean = False
        With New SystemDictionary(AppSettings("HotelConnection"))

            bInserted = .InsertDictionary(datDictionary, publish, has)
        End With
        Me.ViewState(Me.ID + "_Published") = publish
        Me.ViewState(Me.ID + "_HasChange") = has

        Return idind
    End Function

    Public Function Update(ByVal idind As Integer, ByVal txtEng As String, ByVal txtEsp As String, Optional ByVal publish As Boolean = True) As Boolean
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

        If txtEng <> Nothing Then
            Me.Cargarow(datDictionary, txtEng, idiomas.English, rowParent)
        End If

        If txtEsp <> Nothing Then
            Me.Cargarow(datDictionary, txtEsp, idiomas.Spanish, rowParent)
        End If

        Dim has As Boolean = False
        With New SystemDictionary(AppSettings("HotelConnection"))
            bInserted = .InsertDictionary(datDictionary, publish, has)
        End With
        Me.ViewState(Me.ID + "_Published") = publish
        Me.ViewState(Me.ID + "_HasChange") = has

        Return bInserted
    End Function

    Public Sub CargaDatos(ByVal idindice As Integer)
        Dim cd As DictionaryData
        Dim dr As DataRow
        'Me.Limpia()
        Me.txtenglish.Text = ""
        Me.txtspanish.Text = ""
        Dim pub As Boolean = True
        With New SystemDictionary(AppSettings("HotelConnection"))
            cd = .GetDictionaryByID(idindice)
        End With
        If Not cd Is Nothing AndAlso cd.Tables(DictionaryData.TablaDiccionario).Rows.Count > 0 Then
            For Each dr In cd.Tables(DictionaryData.TablaDiccionario).Rows
                Select Case CType(dr(DictionaryData.IdIdioma_FIELD), idiomas)
                    Case idiomas.Spanish
                        txtspanish.Text = dr(DictionaryData.Texto_FIELD)
                    Case idiomas.English
                        txtenglish.Text = dr(DictionaryData.Texto_FIELD)
                End Select
                If pub Then pub = dr(DictionaryData.Published_FIELD)
            Next
        End If
        Me.ViewState("IdIndice") = idindice
        Me.ViewState(Me.ID + "_Published") = pub
    End Sub



    Public Sub CargaDatosAuxiliares(ByVal idindice As Integer, ByRef txtenglish As String, ByRef txtspanish As String)
        Dim cd As DictionaryData
        Dim dr As DataRow
        With New SystemDictionary(AppSettings("HotelConnection"))
            cd = .GetDictionaryByID(idindice)
        End With
        If Not cd Is Nothing AndAlso cd.Tables(DictionaryData.TablaDiccionario).Rows.Count > 0 Then
            For Each dr In cd.Tables(DictionaryData.TablaDiccionario).Rows
                Select Case CType(dr(DictionaryData.IdIdioma_FIELD), idiomas)
                    Case idiomas.Spanish
                        txtspanish = dr(DictionaryData.Texto_FIELD)
                    Case idiomas.English
                        txtspanish = dr(DictionaryData.Texto_FIELD)
                End Select
            Next
            Me.ViewState("IdIndice") = idindice
            'Me.IdIndice = idindice
        End If
        'Me.CargaDatos(idindice)
    End Sub

    Public Sub Limpia()
        Me.IdIndice = 0
        Me.txtenglish.Text = ""
        Me.txtspanish.Text = ""
    End Sub

    Public ReadOnly Property Published() As Boolean
        Get
            Published = True
            If Me.ViewState(Me.ID + "_Published") IsNot Nothing AndAlso Me.ViewState(Me.ID + "_Published").ToString().Trim.Length > 0 Then Published = Me.ViewState(Me.ID + "_Published")
        End Get
    End Property

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        Dim sScript As New System.Text.StringBuilder
        If Not Page.ClientScript.IsStartupScriptRegistered("Allow2Tabs") Then
            'If Not Page.IsStartupScriptRegistered("Allow2Tabs") Then
            sScript.Append("<SCRIPT language=""javascript"">" & vbCrLf)
            sScript.Append("function HideMe(id){" & vbCrLf)
            sScript.Append("	document.getElementById(id).style.display='none';}" & vbCrLf)
            sScript.Append(" function ShowMe(id) {" & vbCrLf)
            sScript.Append("	document.getElementById(id).style.display='';}" & vbCrLf)
            sScript.Append(" function SelectTab(from, to,sel,upl){" & vbCrLf)
            sScript.Append("	document.getElementById(from).className='TabSelected'" & vbCrLf)
            sScript.Append("    document.getElementById(to).className='Tab';" & vbCrLf)
            sScript.Append("    document.getElementById(upl).style.display='none';" & vbCrLf)
            sScript.Append("    document.getElementById(sel).style.display='';" & vbCrLf)
            sScript.Append("    try { document.getElementById(sel).focus(); } catch(e) {}" & vbCrLf)
            sScript.Append("    };" & vbCrLf)
            sScript.Append("</SCRIPT>" & vbCrLf)
            Response.Write(sScript.ToString)
            ''Page.RegisterStartupScript("Allow2Tabs", "<SCRIPT></SCRIPT>")
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Allow2Tabs", "<SCRIPT></SCRIPT>")
        End If

        Select Case AppSettings("DefaultLanguage")
            Case "en-US"
                Response.Write("<script> SelectTab('" & DivSelect.ClientID & "','" & DivUpload.ClientID & "','" & Me.tblenglish.ClientID & "','" & Me.tblspanish.ClientID & "'); SelectIdioma('" & IdiomaSelected.ClientID & "',0); </script>")
            Case "es-MX"
                Response.Write("<script> SelectTab('" & DivUpload.ClientID & "','" & DivSelect.ClientID & "','" & Me.tblspanish.ClientID & "','" & Me.tblenglish.ClientID & "'); SelectIdioma('" & IdiomaSelected.ClientID & "',1); </script>")
        End Select
    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)

        'txtspanish.Width = IIf(Me.Width = 0, Unit.Percentage(100), Unit.Pixel(Me.Width))
        'txtenglish.Width = txtspanish.Width

        'txtspanish.Height = IIf(Me.Height = 0, Unit.Percentage(100), Unit.Pixel(Me.Height))
        'txtenglish.Height = txtspanish.Height

        If Me.Height <> 0 Then
            txtspanish.Height = Unit.Pixel(Me.Height)
            txtenglish.Height = txtspanish.Height
        End If

        tblspanish.Height = IIf(Me.Height = 0, "100%", Me.Height)
        'tblspanish.Width = IIf(Me.Width = 0, "100%", Me.Width)

        tblenglish.Height = tblspanish.Height
        'tblenglish.Width = tblspanish.Width

        tblGen.Width = IIf(Me.Width = 0, "100%", Me.Width)

        'Select Case AppSettings("DefaultLanguage")
        '    Case "en-US"
        '        rfvDefaultText.Text = PortalCulture.GetString("00128")
        '        rfvDefaultText.ErrorMessage = PortalCulture.GetString("00128")
        '    Case "es-MX"
        '        rfvDefaultText.Text = PortalCulture.GetString("00129")
        '        rfvDefaultText.ErrorMessage = PortalCulture.GetString("00129")
        'End Select

        rfvDefaultText.Text = PortalCulture.GetString("00128")
        rfvDefaultText.ErrorMessage = PortalCulture.GetString("00128")
        rfvDefaultText2.Text = PortalCulture.GetString("00129")
        rfvDefaultText2.ErrorMessage = PortalCulture.GetString("00129")
        If Not (ShowErrorText) Then
            rfvDefaultText.Text = "*"
            rfvDefaultText2.Text = "*"
        End If
    End Sub


    Public Property MaxLength() As Integer
        Get
            Return CBool(Me.txtenglish.MaxLength)
        End Get
        Set(ByVal Value As Integer)
            Me.txtenglish.MaxLength = Value
            Me.txtspanish.MaxLength = Value
        End Set
    End Property


    Public ReadOnly Property HasValue() As Boolean
        Get
            Return (Me.IdIndice > 0 AndAlso (Me.txtenglish.Text.Trim().Length > 0 OrElse Me.txtspanish.Text.Length > 0))
        End Get
    End Property

    Public Overloads Function ToString(ByVal lang As Integer) As String
        Dim result As String = Me.txtspanish.Text.Trim()
        If lang = 2 Then result = Me.txtenglish.Text.Trim()
        Return result
    End Function

End Class