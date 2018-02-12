Public Class ucBase
	Inherits System.Web.UI.UserControl


    Public Const SESSION_INFO As String = "KEY_INFO"

	Private _EditMode As Boolean = False
    Private _ID As Long = Nothing
    Private _ID_Str As String


#Region "Properties"

    '"Modos de Los Controles. "

    'Este es el ID que se usara en caso de que se este editando.. 
    Public Property ID_EditMode() As Long
        Get
            'si no la encuentras en la clase restaurala del viewstate 
            If Not IsNothing(Viewstate("ID_Edit")) Then _ID = Viewstate("ID_Edit")
            Return _ID
        End Get
        Set(ByVal Value As Long)
            viewstate("ID_Edit") = Value
            _ID = Value
        End Set
    End Property

    Public Property ID_EditMode_Str() As String
        Get
            'si no la encuentras en la clase restaurala del viewstate 
            If Not IsNothing(Viewstate("ID_Edit_Str")) Then _ID_Str = Viewstate("ID_Edit_Str")
            Return _ID_Str
        End Get
        Set(ByVal Value As String)
            viewstate("ID_Edit_Str") = Value
            _ID_Str = Value
        End Set
    End Property

    'Con esto se determina que se esta haciendo.. 
    Public Property EditMode() As Boolean
        Get
            'si no la encuentras en la clase restaurala del viewstate 
            If Not IsNothing(Viewstate("EditMode")) Then _EditMode = Viewstate("EditMode")
            Return _EditMode
        End Get
        Set(ByVal Value As Boolean)
            Viewstate("EditMode") = Value
            _EditMode = Value
        End Set
    End Property

    Public Property MustInitValues() As Boolean
        Get
            'si no la encuentras en la clase restaurala del viewstate 
            If Not IsNothing(Viewstate("MustInitValues")) Then
                Viewstate("MustInitValues") = True
            End If
            Return Viewstate("MustInitValues")
        End Get
        Set(ByVal Value As Boolean)
            Viewstate("MustInitValues") = Value
        End Set
    End Property
#End Region

#Region " Funciones en Java Script para Focus y Default Button "

    Public Sub DefaultButton(ByRef objTextControl As TextBox, ByRef objDefaultButton As Object)
        objTextControl.Attributes.Add("onkeydown", "fnTrapKD(document.all." & objDefaultButton.ClientID & ")")
    End Sub

    Public Sub SetFocusToControl(ByVal obj As Object)
        Dim strScript As String
        strScript = "<SCRIPT language=""javascript"">" & vbCrLf
        strScript = strScript & "fnFocus('" & obj.ClientID & "');" & vbCrLf
        strScript = strScript & "</SCRIPT>" & vbCrLf
        Session("strFocus") = strScript
    End Sub

#End Region

    'Metodos por control

    'Private Sub InitValues()

    'End Sub

    'Public Function Update() As Boolean

    'End Function

    'Public Function Delete() As Boolean

    'End Function

    'Public Overridable Function Cancel() As Boolean

    'End Function

    'Public Function LoadItem(ByVal ID As Integer) As Boolean

    'End Function

    'Protected Sub SetEditMode(ByVal ID As Integer)

    'End Sub

    'Protected Sub ShowError()

    'End Sub


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Response.Expires = 0
    End Sub

    Function GetIdAsociation()
        Dim IdAsociation As Integer = -1
        Try
            If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
            IdAsociation = If(IdAsociation = 0, -1, IdAsociation)
        Catch ex As Exception
        End Try
        Return IdAsociation
    End Function

End Class





