Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports System.Configuration

Partial Class ctrlHotelPlans
    Inherits System.Web.UI.UserControl

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

    Private TEXT_NEW_PLAN As String = "Nuevo Plan"
    Private TEXT_DESCRIPTION_TITLE As String = "Recursos de Descripción"

    Private Const KEY_HOTELID = "HotelId"
    
    Private Const DATABASE_HOTEL_CONNECTION_STRING = "HotelConnectionString"
	Protected mlDescriptionPlan As CtrlIdioma

#Region "Propiedades"
    Public Property m_iHotelPlanId() As Integer
        Get
            Return ViewState("HotelPlanId")
        End Get
        Set(ByVal Value As Integer)
            ViewState("HotelPlanId") = Value
        End Set
    End Property
    Public Property m_iHotelId() As Integer
        Get
            Return ViewState(KEY_HOTELID)
        End Get
        Set(ByVal Value As Integer)
            ViewState(KEY_HOTELID) = Value
        End Set
    End Property
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            ' set to 0 all properties
            Me.m_iHotelPlanId = 0
            ClearControls()
            FillDataList()
        End If
		'''todo
		mlDescriptionPlan.IsHTML = True
		mlDescriptionPlan.Height = 128
		mlDescriptionPlan.IsMultiline = True

        TEXT_NEW_PLAN = PortalCulture.GetString("00293") '"Nuevo Plan"
        TEXT_DESCRIPTION_TITLE = PortalCulture.GetString("00295") '"Recursos de Descripción"
    End Sub

	Private Sub ClearControls()
		mlDescriptionPlan.Limpia()		
		Me.lstPlanType.SelectedValue = Nothing
        Me.lblEditTitle.Text = ""

	End Sub

    Public Sub Clear()

        ClearControls()
        Me.lblEditTitle.Text = TEXT_NEW_PLAN
        Me.m_iHotelPlanId = 0
    End Sub

    Public Sub LoadPlan(ByVal iPlanId As Integer)
        Dim datHotelPlan As HotelPlansData
        Dim rowPlan As DataRow

        ' get plan information by id
        With New HotelPlansSystem
            datHotelPlan = .GetPlanById(iPlanId)
        End With

        ' get plan strings
        If Not datHotelPlan Is Nothing AndAlso datHotelPlan.HotelPlansTable.Rows.Count > 0 Then
            rowPlan = datHotelPlan.HotelPlansTable.Rows(0)
            Me.m_iHotelPlanId = rowPlan(HotelPlansData.TableFields.PKID_HOTELPLAN)
            ' name string                                    

            ' load language information
            mlDescriptionPlan.Limpia()
            mlDescriptionPlan.CargaDatos(rowPlan(HotelPlansData.TableFields.ID_DESCRIPTION))
            mlDescriptionPlan.textodefault = rowPlan(HotelPlansData.TableFields.DESCRIPTION)

            Me.lblEditTitle.Text = PortalCulture.GetString("00294")
            Try
                Me.lstPlanType.SelectedValue = rowPlan(HotelPlansData.TableFields.ID_PLAN)
            Catch ex As Exception
                Me.lstPlanType.SelectedValue = Nothing
            End Try
        End If
        Me.lstPlanType.Enabled = False
    End Sub

    ' Add a new plan name to database using the current plan Id
    Public Function AddPlan() As Boolean
        If Not Page.IsValid Then
            Return False
        End If
        ' if no current planid create new plan
        If Me.m_iHotelPlanId = 0 Then
            Return SaveNewPlan()
        Else
            Return UpdatePlan()
        End If

    End Function

    ' create new plan with the specified language
    Private Function SaveNewPlan() As Boolean
        Dim datPlan As New HotelPlansData
        Dim planRow As DataRow
        Dim bResult As Boolean


        'If Not Me.txtDescription.Text.Length = 0 Then
        ' set new plan data
        If Not lstPlanType.SelectedValue Is Nothing Then
            With datPlan

                planRow = .HotelPlansTable.NewRow()
                planRow(.TableFields.DESCRIPTION) = mlDescriptionPlan.textodefault
                planRow(.TableFields.ID_PLAN) = lstPlanType.SelectedValue
                planRow(.TableFields.ID_HOTEL) = m_iHotelId

                .HotelPlansTable.Rows.Add(planRow)

                ' save new plan     
                With New HotelPlansSystem
                    bResult = .InsertPlans(datPlan)
                End With
                ' set current plan id
                If bResult = True Then
                    mlDescriptionPlan.Update(.HotelPlansTable.Rows(0)(.TableFields.ID_DESCRIPTION))
                    Me.m_iHotelPlanId = .HotelPlansTable.Rows(0)(.TableFields.PKID_HOTELPLAN)
                End If
            End With
        End If
        'End If
        Return bResult
    End Function

    ' update current plan
    Private Function UpdatePlan() As Boolean
        Dim datPlan As New HotelPlansData
        Dim planRow As DataRow
        Dim bResult As Boolean

        With New HotelPlansSystem
            datPlan = .GetPlanById(Me.m_iHotelPlanId)
        End With
        If datPlan.HotelPlansTable.Rows.Count > 0 Then
            ' set plan data
            With datPlan
                datPlan.HotelPlansTable.Rows(0)(.TableFields.DESCRIPTION) = mlDescriptionPlan.textodefault
                datPlan.HotelPlansTable.Rows(0)(.TableFields.ID_PLAN) = Me.lstPlanType.SelectedValue
                ' save plan changes
                With New HotelPlansSystem
                    bResult = .UpdatePlans(datPlan)
                End With
                ' set current plan id
                If bResult = True Then
                    mlDescriptionPlan.Update(.HotelPlansTable.Rows(0)(.TableFields.ID_DESCRIPTION))
                    Me.m_iHotelPlanId = .HotelPlansTable.Rows(0)(.TableFields.PKID_HOTELPLAN)
                End If
            End With
        End If
        Return bResult
    End Function

    Public Sub NewPlan()
        ClearControls()
        Me.lstPlanType.Enabled = True
        Me.lblEditTitle.Text = PortalCulture.GetString("00293")    '  "Nuevo Plan"
        Me.m_iHotelPlanId = 0

    End Sub

    Public Function DeletePlan() As Boolean
        Dim bResult As Boolean
        If m_iHotelPlanId <> 0 Then
            With New HotelPlansSystem
                bResult = .DeletePlan(m_iHotelPlanId)
            End With
            If bResult = True Then
                m_iHotelPlanId = 0
                Clear()
            End If
        End If
        Return bResult
    End Function

    Private Sub FillDataList()
        Dim datPlans As PlansData
        With New PlansSystem
            datPlans = .GetAllPlans(PortalCulture.GetIDCulture())
        End With

        If Not datPlans Is Nothing Then
            With lstPlanType
                .DataSource = datPlans.PlansTable
                .DataTextField = datPlans.TableFields.DEFAULTNAME
                .DataValueField = datPlans.TableFields.PKIDPLANS
                .DataBind()
            End With
        End If
    End Sub

    Private Sub loadResources()
        lblPlanType.Text = PortalCulture.GetString("00290", True)
        lblPlanName.Text = PortalCulture.GetString("00292", True)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
    End Sub

End Class
