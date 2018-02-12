Imports Portal.Common
Imports Portal.Facade
Imports System.Configuration.ConfigurationManager

Partial Public Class CtrlIdiomaPortalFCK
    Inherits System.Web.UI.UserControl

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

    Public Property IdIdioma() As Integer
        Get
            Return Me.CtrlIdiomaFCk1.IdIdioma
        End Get
        Set(ByVal Value As Integer)
            Me.CtrlIdiomaFCk1.IdIdioma = Value
        End Set
    End Property

    Public Function SoloIdiomaDefault() As Boolean
        Return Me.CtrlIdiomaFCk1.SoloIdiomaDefault1        
    End Function

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

    Public ReadOnly Property Published() As Boolean
        Get
            Return Me.ViewState(Me.ID + "_Published")
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


    Public Function textodefaultTEXT(ByVal len As Integer) As String
        'TODO POR MIENTRAS
        Dim valor As Boolean
        Dim src As String

        Select Case AppSettings("DefaultLanguage")
            Case "en-US"
                src = CtrlIdiomaFCk1.textoInglesTEXT.Trim()
                If src.Length > len Then src = src.Substring(0, len)
                Return src
            Case "es-MX"
                src = CtrlIdiomaFCk1.textoEspañolTEXT.Trim()
                If src.Length > len Then src = src.Substring(0, len)
                Return src
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

    Public Property textoIngles() As String
        Get
            Return CtrlIdiomaFCk1.textoIngles
        End Get
        Set(ByVal Value As String)
            CtrlIdiomaFCk1.textoIngles = Value
        End Set
    End Property

    Public Property textoEspañol() As String
        Get
            Return CtrlIdiomaFCk1.textoEspañol
        End Get
        Set(ByVal Value As String)
            CtrlIdiomaFCk1.textoEspañol = Value
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
            Return ViewState("IsHTML")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsHTML") = Value
            CtrlIdiomaFCk1.isHtml = Value
        End Set
    End Property

    Public Property IdIndice() As Integer
        Get
            Return ViewState("IdIndice")
        End Get
        Set(ByVal Value As Integer)
            ViewState("IdIndice") = Value
            '//VIKTOR If Value <> 0 Then CargaDatos(Value, 0)
        End Set
    End Property

    Public Property IdPortal() As Integer
        Get
            Return ViewState("IdPortal")
        End Get
        Set(ByVal Value As Integer)
            ViewState("IdPortal") = Value
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
            Return ViewState("_Height")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_Height") = Value
            CtrlIdiomaFCk1.Height = Value
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return ViewState("_Width")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_Width") = Value
        End Set
    End Property

    Private Sub Cargarow(ByRef cd As DictionaryDataPortal, ByVal texto As String, ByVal contenido As String, ByVal idioma As Integer, ByVal pRow As DataRow)
        Dim dr As DataRow

        dr = cd.Tables(DictionaryDataPortal.TablaDiccionario).NewRow
        dr(DictionaryDataPortal.FIELD_IdIdioma) = idioma
        dr(DictionaryDataPortal.Indice_FIELD) = Me.IdIndice
        dr(DictionaryDataPortal.FIELD_Texto) = texto
        contenido = contenido.Trim
        dr(DictionaryDataPortal.FIELD_TextoContenido) = If(contenido.Length > 3999, contenido.Substring(0, 3999), contenido)
        dr(DictionaryDataPortal.FIELD_IdPortal) = IdPortal
        dr.SetParentRow(pRow)
        cd.Tables(DictionaryDataPortal.TablaDiccionario).Rows.Add(dr)
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

    Public Function Insert(Optional ByVal idind As Integer = 0, Optional ByVal publish As Boolean = True) As Integer
        Dim datDictionary As New DictionaryDataPortal
        Dim rowParent As DataRow

        Dim bInserted As Boolean
        ' create the new index parent row
        With datDictionary.IndexTable
            rowParent = .NewRow()
            ' add row to index table
            rowParent(DictionaryDataPortal.IndexTablefields.Indice) = idind
            .Rows.Add(rowParent)
            ' set the  index table to unmodified state
            'datDictionary.IndexTable.AcceptChanges()
        End With

        If CtrlIdiomaFCk1.textoIngles.Trim <> "" Then
            Me.Cargarow(datDictionary, CtrlIdiomaFCk1.textoIngles, CtrlIdiomaFCk1.textoInglesTEXT, idiomas.English, rowParent)
        End If
        If CtrlIdiomaFCk1.textoEspañol.Trim <> "" Then
            Me.Cargarow(datDictionary, CtrlIdiomaFCk1.textoEspañol, CtrlIdiomaFCk1.textoEspañolTEXT, idiomas.Spanish, rowParent)
        End If

        With New SystemDictionaryPortal()
            bInserted = .InsertNewDictionary(datDictionary, publish)
            If bInserted Then
                Return datDictionary.Tables(DictionaryDataPortal.TablaIndice).Rows(0).Item(DictionaryDataPortal.FIELD_IdDiccionario)
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
        Dim datDictionary As New DictionaryDataPortal
        Dim rowParent As DataRow
        Dim bInserted As Boolean
        ' create the new index parent row
        With datDictionary.IndexTable
            rowParent = .NewRow()
            ' add row to index table
            .Rows.Add(rowParent)
            ' assign dictionary id setting row status to modified 
            rowParent(DictionaryDataPortal.IndexTablefields.Indice) = idind
            ' set the  index table to unmodified state
            datDictionary.IndexTable.AcceptChanges()
        End With
        Me.IdIndice = idind

        If CtrlIdiomaFCk1.textoInglesTEXT.Trim <> "" Then
            Me.Cargarow(datDictionary, CtrlIdiomaFCk1.textoIngles, CtrlIdiomaFCk1.textoInglesTEXT, idiomas.English, rowParent)
        End If
        If CtrlIdiomaFCk1.textoEspañolTEXT.Trim <> "" Then
            Me.Cargarow(datDictionary, CtrlIdiomaFCk1.textoEspañol, CtrlIdiomaFCk1.textoEspañolTEXT, idiomas.Spanish, rowParent)
        End If

        Dim has As Boolean = False
        With New SystemDictionaryPortal()
            bInserted = .InsertDictionary(datDictionary, publish, has)
        End With
        Me.ViewState(Me.ID + "_HasChange") = has
        Return idind
    End Function

    Public Function Update(ByVal txtEng As String, ByVal txtEsp As String, ByVal txtEngTEXT As String, ByVal txtEspTEXT As String, Optional ByVal idind As Integer = 0, Optional ByVal publish As Boolean = True) As Integer
        Dim datDictionary As New DictionaryDataPortal
        Dim rowParent As DataRow
        Dim bInserted As Boolean

        ' create the new index parent row
        With datDictionary.IndexTable
            rowParent = .NewRow()
            ' add row to index table
            .Rows.Add(rowParent)
            ' assign dictionary id setting row status to modified 
            rowParent(DictionaryDataPortal.IndexTablefields.Indice) = idind
            ' set the  index table to unmodified state
            datDictionary.IndexTable.AcceptChanges()
        End With

        Me.Cargarow(datDictionary, txtEng, txtEngTEXT, idiomas.English, rowParent)
        Me.Cargarow(datDictionary, txtEsp, txtEspTEXT, idiomas.Spanish, rowParent)

        Dim has As Boolean = False
        With New SystemDictionaryPortal()
            bInserted = .InsertDictionary(datDictionary, publish, has)
        End With
        Me.ViewState(Me.ID + "_HasChange") = has
        Return idind

    End Function


    Public Function Update(ByVal idind As Integer, ByVal txtEng As String, ByVal txtEsp As String, ByVal txtEngTEXT As String, ByVal txtEspTEXT As String, Optional ByVal publish As Boolean = True) As Boolean
        Dim datDictionary As New DictionaryDataPortal
        Dim rowParent As DataRow
        Dim bInserted As Boolean
        ' create the new index parent row
        With datDictionary.IndexTable
            rowParent = .NewRow()
            ' add row to index table
            .Rows.Add(rowParent)
            ' assign dictionary id setting row status to modified 
            rowParent(DictionaryDataPortal.IndexTablefields.Indice) = idind
            ' set the  index table to unmodified state
            datDictionary.IndexTable.AcceptChanges()
        End With

        If txtEng <> Nothing Then
            Me.Cargarow(datDictionary, txtEng, txtEngTEXT, idiomas.English, rowParent)
        End If

        If txtEsp <> Nothing Then
            Me.Cargarow(datDictionary, txtEsp, txtEspTEXT, idiomas.Spanish, rowParent)
        End If

        Dim has As Boolean = False
        With New SystemDictionaryPortal()
            bInserted = .InsertDictionary(datDictionary, publish, has)
        End With
        Me.ViewState(Me.ID + "_HasChange") = has

        Return bInserted
    End Function

    Public Sub Delete(ByVal idPortal As Integer, ByVal idDicc As Integer)
        Dim sError As String = ""
        With New SystemDictionaryPortal()
            .DeleteDictionaryPortal(idPortal, idDicc, sError)
        End With
    End Sub

    Public Sub CargaDatos(ByVal idindice As Integer)
        Dim cd As DictionaryDataPortal
        Dim dr As DataRow

        CtrlIdiomaFCk1.textoEspañol = ""
        CtrlIdiomaFCk1.textoIngles = ""

        With New SystemDictionaryPortal()
            cd = .GetDictionaryByID(idindice, IdPortal)
        End With
        If Not cd Is Nothing AndAlso cd.Tables(DictionaryDataPortal.TablaDiccionario).Rows.Count > 0 Then
            For Each dr In cd.Tables(DictionaryDataPortal.TablaDiccionario).Rows
                Select Case CType(dr(DictionaryDataPortal.FIELD_IdIdioma), idiomas)
                    Case idiomas.Spanish
                        CtrlIdiomaFCk1.textoEspañol = dr(DictionaryDataPortal.FIELD_Texto)
                    Case idiomas.English
                        CtrlIdiomaFCk1.textoIngles = dr(DictionaryDataPortal.FIELD_Texto)
                End Select
            Next
            Me.ViewState(Me.ID + "_Published") = cd.Tables(DictionaryDataPortal.TablaDiccionario).Rows(0)(DictionaryDataPortal.Published_FIELD)
        End If
    End Sub

    Public Sub CargaDatosAuxiliares(ByVal idindice As Integer, ByVal idportal As Integer, ByRef txtenglish As String, ByRef txtspanish As String)
        Dim cd As DictionaryDataPortal
        Dim dr As DataRow
        With New SystemDictionaryPortal()
            cd = .GetDictionaryByID(idindice, idportal)
        End With
        If Not cd Is Nothing AndAlso cd.Tables(DictionaryDataPortal.TablaDiccionario).Rows.Count > 0 Then
            For Each dr In cd.Tables(DictionaryDataPortal.TablaDiccionario).Rows
                Select Case CType(dr(DictionaryDataPortal.FIELD_IdIdioma), idiomas)
                    Case idiomas.Spanish
                        txtspanish = dr(DictionaryDataPortal.FIELD_Texto)
                    Case idiomas.English
                        txtenglish = dr(DictionaryDataPortal.FIELD_Texto)
                End Select
            Next
            Me.ViewState(Me.ID + "_Published") = cd.Tables(DictionaryDataPortal.TablaDiccionario).Rows(0)(DictionaryDataPortal.Published_FIELD)
        End If
    End Sub

    Public Sub Limpia()
        Me.IdIndice = 0
        CtrlIdiomaFCk1.textoIngles = ""
        CtrlIdiomaFCk1.textoEspañol = ""
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
    End Sub


End Class