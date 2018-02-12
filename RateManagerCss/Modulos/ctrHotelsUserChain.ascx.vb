Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.Hotel.DataAccess
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade

Partial Class ctrHotelsUserChain
    Inherits System.Web.UI.UserControl


#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnCargar As System.Web.UI.WebControls.Button

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

#Region "Declarations"
    Public Event onSelected(ByVal CompanyID As Integer)
#End Region

#Region "Propertys"
    Public ReadOnly Property SelectedCompanyID() As Integer
        Get
            Dim result As Integer = -1
            If ddlHoteles.SelectedItem.Value <> "-1" Then
                Try
                    result = (CInt(ddlHoteles.SelectedItem.Value))
                Catch
                    result = -1
                End Try
            End If
            Return result
        End Get
    End Property

    Public ReadOnly Property SelectedCompanyName() As String
        Get
            Dim result As String = ""
            If ddlHoteles.SelectedItem.Value <> "-1" Then
                Try
                    result = ddlHoteles.SelectedItem.Text
                Catch
                    result = ""
                End Try
            End If
            Return result
        End Get
    End Property

    Public Property CurrentSelectedCompanyID() As Integer
        Get
            Dim result As Integer = -1
            Try
                result = CType(Session(String.Format("{0}CurrentSelectedCompanyID", Me.ID)), Integer)
                If result = 0 Then result = (-1)
            Catch
                result = -1
            End Try
            Return result
        End Get
        Set(ByVal Value As Integer)
            Session(String.Format("{0}CurrentSelectedCompanyID", Me.ID)) = Value
        End Set
    End Property


    Private ReadOnly Property MyPageBase() As PaginaBase
        Get
            Try
                Return CType(Me.Page, PaginaBase)
            Catch
            End Try
            Return Nothing
        End Get
    End Property
#End Region

#Region "Pages"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Not IsPostBack Then
            LoadResources()

        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub LoadResources()
        lblEmpresas.Text = PortalCulture.GetString("00880", True)
    End Sub

    Public Sub LoadHotels()
        Dim ds As DataSet
        With (New Hoteles)
            ds = .LoadHotelsByUser(Session("idUsuario"))
        End With
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count Then
            ddlHoteles.DataSource = ds.Tables(0).DefaultView
            ddlHoteles.DataTextField = "nombre"
            ddlHoteles.DataValueField = "idEmpresa"
            ddlHoteles.DataBind()
        End If
        LoadCompanyCorporate()
        ddlHoteles.Items.Insert(0, New ListItem(PortalCulture.GetString("M000272", False), "-1"))

        If CurrentSelectedCompanyID <> -1 Then
            ddlHoteles.SelectedIndex = ddlHoteles.Items.IndexOf(ddlHoteles.Items.FindByValue(CurrentSelectedCompanyID))
        ElseIf MyPageBase.IsHotelSelected Then
            ddlHoteles.SelectedIndex = ddlHoteles.Items.IndexOf(ddlHoteles.Items.FindByValue(MyPageBase.cInfoActual.Empresa))
        Else
            ddlHoteles.SelectedIndex = 0
        End If
        SetCompany()

    End Sub

    Private Sub LoadCompanyCorporate()
        Dim ds As New DataSet
        With (New Hoteles)
            ds = .LoadCompanyCorporateByCorporateID(MyPageBase.IdCorporativoUserChain)
        End With
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            Dim dr As DataRow = ds.Tables(0).Rows(0)

            ddlHoteles.Items.Insert(0, New ListItem(CType(dr("Nombre"), String), CType(dr("idEmpresa"), String)))
        End If




    End Sub


    Public Sub SetCompany()
        CurrentSelectedCompanyID = SelectedCompanyID
        lblCurrentHotel.Text = ""
        If SelectedCompanyName <> String.Empty Then lblCurrentHotel.Text = String.Format(">> {0} <<", SelectedCompanyName)
    End Sub

#End Region

#Region "Events"

    Private Sub ddlHoteles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHoteles.SelectedIndexChanged
        SetCompany()
        RaiseEvent onSelected(CurrentSelectedCompanyID)
    End Sub

#End Region


 
End Class
