Imports Portal.Common
Imports Portal.Facade
Imports System.Configuration
Partial Class ctrlLanguageDictionary
    Inherits System.Web.UI.UserControl
    Const KEY_IDDICTIONARY = "iDictionaryId"
    Const KEY_STRTITLE = "strTitle"
    Const KEY_LANGUAGEID = "iLanguageId"
    Const KEY_CONNECTIONSTRING = "DictionaryConnectionString"
    Public Property m_strTitle() As String
        Get
            Return ViewState(KEY_STRTITLE)
        End Get
        Set(ByVal Value As String)
            ViewState(KEY_STRTITLE) = Value
            Me.lblTitle.Text = Value
        End Set

    End Property

    Public WriteOnly Property m_visibleTitle() As Boolean
        Set(ByVal Value As Boolean)
            Me.lblTitle.Visible = Value
        End Set
    End Property

    Private Property m_idDictionary() As Integer
        Get
            Return ViewState(KEY_IDDICTIONARY)
        End Get
        Set(ByVal Value As Integer)
            ViewState(KEY_IDDICTIONARY) = Value
        End Set
    End Property
    Private Property m_iLanguageId() As Integer
        Get
            Return ViewState(KEY_LANGUAGEID)
        End Get
        Set(ByVal Value As Integer)
            ViewState(KEY_LANGUAGEID) = Value
        End Set
    End Property
    Private Property m_strConnectionString() As String
        Get
            Return viewstate(KEY_CONNECTIONSTRING)
        End Get
        Set(ByVal Value As String)
            viewstate(KEY_CONNECTIONSTRING) = Value
        End Set
    End Property

#Region " Código generado por el Diseñador de Web Forms "


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

    Enum DatagridCols As Integer
        LanguageName = 0
        Text
        EditLink
        idDictionary
        idLanguage
    End Enum

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

   
    Private Sub FillDataGrid()
        Dim datDictionary As DictionaryData
        With New SystemDictionary(m_strConnectionString)
            datDictionary = .GetDictionaryStrings(m_idDictionary)
        End With

        If Not datDictionary Is Nothing AndAlso Not datDictionary.DictionaryTable Is Nothing Then
            BindDatagridFields()
            Me.dtgStrings.DataSource = datDictionary.DictionaryTable
        End If
        Me.dtgStrings.DataBind()
    End Sub

   
    Private Sub BindDatagridFields()
        Dim boundColumn As boundColumn
        With Me.dtgStrings.Columns
            With CType(.Item(DatagridCols.idDictionary), boundColumn)
                .DataField = DictionaryData.DictionaryTableFields.IdDiccionario
            End With
            With CType(.Item(DatagridCols.idLanguage), boundColumn)
                .DataField = DictionaryData.DictionaryTableFields.IdIdioma
            End With

            With CType(.Item(DatagridCols.LanguageName), boundColumn)
                .DataField = DictionaryData.DictionaryTableFields.Idioma
            End With
        End With
    End Sub

    Public Sub LoadDictionary(ByVal iDictionaryIndex As Integer, ByVal strConnectionString As String)
        m_idDictionary = iDictionaryIndex
        m_strConnectionString = strConnectionString
        FillDataGrid()
    End Sub
   
    Private Sub dtgStrings_EditCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgStrings.EditCommand
        Dim iLanguageId As Integer
        Me.dtgStrings.EditItemIndex = e.Item.ItemIndex
        FillDataGrid()
    End Sub

    Private Sub dtgStrings_UpdateCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgStrings.UpdateCommand
        ' save language information
        Dim txtString As TextBox

        Dim datDictionary As New DictionaryData
        Dim rowDict As DataRow
        Dim rowParent As DataRow
        Dim bInserted As Boolean

        ' create the new index parent row
        With datDictionary.IndexTable
            rowParent = .NewRow()
            ' add row to index table
            .Rows.Add(rowParent)
            ' assign dictionary id setting row status to modified 
            rowParent(DictionaryData.IndexTablefields.Indice) = m_idDictionary
            ' set the  index table to unmodified state
            datDictionary.IndexTable.AcceptChanges()
        End With

        ' create row and assign information
        rowDict = datDictionary.DictionaryTable.NewRow()
        rowDict(DictionaryData.DictionaryTableFields.IdDiccionario) = m_idDictionary


        rowDict(DictionaryData.DictionaryTableFields.IdIdioma) = CInt(e.Item.Cells(Me.DatagridCols.idLanguage).Text)

        txtString = CType(e.Item.Cells(Me.DatagridCols.Text).FindControl("txtString"), TextBox)
        rowDict(DictionaryData.DictionaryTableFields.Texto) = txtString.Text

        rowDict.SetParentRow(rowParent)

        '''''' try to insert dictionary data'''''
        ' set row language to selected language

        ' add row to dataset
        datDictionary.DictionaryTable.Rows.Add(rowDict)
        ' inset row into dtabase
        With New SystemDictionary(m_strConnectionString)
            bInserted = .InsertDictionary(datDictionary)
        End With
        ' if we could not insert the row, try tu update it 
        If bInserted = False Then
            ' change row status to modified
            rowDict.AcceptChanges()
            rowDict(DictionaryData.DictionaryTableFields.IdIdioma) = rowDict(DictionaryData.DictionaryTableFields.IdIdioma)
            ' update dataset
            With New SystemDictionary(m_strConnectionString)
                .UpdateDictionary(datDictionary, True)
            End With
        End If
        Me.dtgStrings.EditItemIndex = -1
        FillDataGrid()
    End Sub

    Private Sub dtgStrings_CancelCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgStrings.CancelCommand
        Me.dtgStrings.EditItemIndex = -1
        FillDataGrid()
    End Sub

    Private Sub dtgStrings_ItemDataBound1(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgStrings.ItemDataBound
        Dim lab As Label
        Dim Lb1, Lb2, Lb3 As LinkButton
        lab = e.Item.Cells(DatagridCols.Text).FindControl("Label1")
        If Not lab Is Nothing Then
            If lab.Text.Length > 20 Then
                lab.Text = lab.Text.Substring(0, 17) & "..."
            End If
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Lb1 = e.Item.Cells(DatagridCols.EditLink).FindControl("LinkButton1")
            If Not Lb1 Is Nothing Then
                Lb1.Text = PortalCulture.GetString("00093")
            End If
        End If
        If e.Item.ItemType = ListItemType.EditItem Then
            Lb1 = e.Item.Cells(DatagridCols.EditLink).FindControl("LinkButton3")
            Lb1 = e.Item.Cells(DatagridCols.EditLink).FindControl("LinkButton2")
            If Not Lb3 Is Nothing Then
                Lb3.Text = PortalCulture.GetString("00094")
            End If
            If Not Lb2 Is Nothing Then
                Lb2.Text = PortalCulture.GetString("00095")
            End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            dtgStrings.Columns(DatagridCols.LanguageName).HeaderText = PortalCulture.GetString("00092")
            dtgStrings.Columns(DatagridCols.Text).HeaderText = PortalCulture.GetString("00096")
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("00092")
    End Sub

End Class
