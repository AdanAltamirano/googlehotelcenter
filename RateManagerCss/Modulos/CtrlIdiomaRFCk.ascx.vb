Imports Portal.Common
Imports Portal.Facade
Imports System.Configuration.ConfigurationManager

Partial Class CtrlIdiomaRFCk
    Inherits System.Web.UI.UserControl
    Protected WithEvents CtrlIdiomaFCk1 As CtrlIdiomaFCk

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

    Enum idiomas
        Spanish = 1
        English = 2
    End Enum

    Private _ShowErrorText As Boolean = True
    Private _RequiredText As Boolean = True

    Public Property RequiredText() As Boolean
        Get
            Return _RequiredText
        End Get
        Set(ByVal Value As Boolean)
            _RequiredText = Value
        End Set
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
            Return Me.CtrlIdiomaFCk1.ReturnNameTxtEn
        End Get
    End Property

    Public ReadOnly Property ReturnNameTxtEs() As String
        Get
            Return Me.CtrlIdiomaFCk1.ReturnNameTxtEs
        End Get
    End Property

    Private Function textDefault() As String
        'TODO POR MIENTRAS
        Select Case AppSettings("DefaultLanguage")
            Case "en-US"
                Return CtrlIdiomaFCk1.textoIngles.Trim()
            Case "es-MX"
                Return CtrlIdiomaFCk1.textoEspañol.Trim()
        End Select
        Return ""
    End Function

    Public Property textodefault() As String
        Get
            Return textDefault()
        End Get
        Set(ByVal Value As String)
            Select Case AppSettings("DefaultLanguage")
                Case "en-US"
                    If CtrlIdiomaFCk1.textoIngles.Trim = "" Then
                        CtrlIdiomaFCk1.textoIngles = Value
                    End If
                Case "es-MX"
                    If CtrlIdiomaFCk1.textoEspañol.Trim = "" Then
                        CtrlIdiomaFCk1.textoEspañol = Value
                    End If
            End Select
        End Set
    End Property

    Public Property IsMultiline() As Boolean
        Get
            Return CtrlIdiomaFCk1.IsMultiline
        End Get
        Set(ByVal Value As Boolean)
            CtrlIdiomaFCk1.IsMultiline = Value
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
            Return viewstate("IdIndice")
        End Get
        Set(ByVal Value As Integer)
            viewstate("IdIndice") = Value
            If Value <> 0 Then CargaDatos(Value)
        End Set
    End Property

    '***27/08/05**********funcion que deshabilita el control del idioma
    Public Property deshabilita() As Boolean
        Get

        End Get
        Set(ByVal Value As Boolean)
        End Set
    End Property

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
        CtrlIdiomaFCk1.textoIngles = value
        Return ""
    End Function

    Public Function setES(ByVal value As String) As String
        CtrlIdiomaFCk1.textoEspañol = value
        Return ""
    End Function

    Public Function GetEN(Optional ByVal value As String = "") As String
        If CtrlIdiomaFCk1.textoIngles.Trim <> "" Then
            Return (CtrlIdiomaFCk1.textoIngles)
        Else
            CtrlIdiomaFCk1.textoIngles = value
        End If
        Return ""
    End Function

    Public Function GetES(Optional ByVal value As String = "") As String
        If CtrlIdiomaFCk1.textoEspañol.Trim <> "" Then
            Return (CtrlIdiomaFCk1.textoEspañol)
        Else
            CtrlIdiomaFCk1.textoEspañol = value
        End If
        Return ""
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

        If CtrlIdiomaFCk1.textoIngles.Trim <> "" Then
            Me.Cargarow(datDictionary, CtrlIdiomaFCk1.textoIngles, idiomas.English, rowParent)
        End If
        If CtrlIdiomaFCk1.textoEspañol.Trim <> "" Then
            Me.Cargarow(datDictionary, CtrlIdiomaFCk1.textoEspañol, idiomas.Spanish, rowParent)
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

    Public Function Update(Optional ByVal idind As Integer = 0, Optional ByVal publish As Boolean = True) As Integer
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

        If CtrlIdiomaFCk1.textoIngles.Trim <> "" Then
            Me.Cargarow(datDictionary, CtrlIdiomaFCk1.textoIngles, idiomas.English, rowParent)
        End If
        If CtrlIdiomaFCk1.textoEspañol.Trim <> "" Then
            Me.Cargarow(datDictionary, CtrlIdiomaFCk1.textoEspañol, idiomas.Spanish, rowParent)
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
        Dim pub As Boolean = True

        CtrlIdiomaFCk1.textoEspañol = ""
        CtrlIdiomaFCk1.textoIngles = ""

        With New SystemDictionary(AppSettings("HotelConnection"))
            cd = .GetDictionaryByID(idindice)
        End With
        If Not cd Is Nothing AndAlso cd.Tables(DictionaryData.TablaDiccionario).Rows.Count > 0 Then
            For Each dr In cd.Tables(DictionaryData.TablaDiccionario).Rows
                Select Case CType(dr(DictionaryData.IdIdioma_FIELD), idiomas)
                    Case idiomas.Spanish
                        CtrlIdiomaFCk1.textoEspañol = dr(DictionaryData.Texto_FIELD)
                    Case idiomas.English
                        CtrlIdiomaFCk1.textoIngles = dr(DictionaryData.Texto_FIELD)
                End Select
                If pub Then pub = dr(DictionaryData.Published_FIELD)
            Next
            Me.ViewState(Me.ID + "_Published") = pub
        End If
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
                        txtenglish = dr(DictionaryData.Texto_FIELD)
                End Select
            Next
        End If
    End Sub

    Public Sub Limpia()
        Me.IdIndice = 0
        CtrlIdiomaFCk1.textoIngles = ""
        CtrlIdiomaFCk1.textoEspañol = ""
    End Sub

    Public ReadOnly Property Published() As Boolean
        Get
            Published = True
            If Me.ViewState(Me.ID + "_Published") IsNot Nothing AndAlso Me.ViewState(Me.ID + "_Published").ToString().Trim.Length > 0 Then Published = Me.ViewState(Me.ID + "_Published")
        End Get
    End Property

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)

        'txtspanish.Width = IIf(Me.Width = 0, Unit.Percentage(100), Unit.Pixel(Me.Width))
        'txtenglish.Width = txtspanish.Width

        'txtspanish.Height = IIf(Me.Height = 0, Unit.Percentage(100), Unit.Pixel(Me.Height))
        'txtenglish.Height = txtspanish.Height

        'If Me.Height <> 0 Then
        '    txtspanish.Height = Unit.Pixel(Me.Height)
        '    txtenglish.Height = txtspanish.Height
        'End If

        'tblspanish.Height = IIf(Me.Height = 0, "100%", Me.Height)
        ''tblspanish.Width = IIf(Me.Width = 0, "100%", Me.Width)

        'tblenglish.Height = tblspanish.Height
        ''tblenglish.Width = tblspanish.Width

        'tblGen.Width = IIf(Me.Width = 0, "100%", Me.Width)

        'Select Case AppSettings("DefaultLanguage")
        '    Case "en-US"
        '        rfvDefaultText.Text = PortalCulture.GetString("00128")
        '        rfvDefaultText.ErrorMessage = PortalCulture.GetString("00128")
        '    Case "es-MX"
        '        rfvDefaultText.Text = PortalCulture.GetString("00129")
        '        rfvDefaultText.ErrorMessage = PortalCulture.GetString("00129")
        'End Select
        'If Not (ShowErrorText) Then rfvDefaultText.Text = "*"
    End Sub


    'Public Property MaxLength() As Integer
    '    'Get
    '    '    Return CBool(Me.txtenglish.MaxLength)
    '    'End Get
    '    'Set(ByVal Value As Integer)
    '    '    Me.txtenglish.MaxLength = Value
    '    '    Me.txtspanish.MaxLength = Value
    '    'End Set
    'End Property


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Put user code to initialize the page here
    End Sub

End Class
