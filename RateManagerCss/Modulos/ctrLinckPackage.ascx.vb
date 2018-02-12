Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Data.SqlClient
Partial Class ctrLinckPackage
    Inherits UserControlBase

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


    Public Property idReservacion() As String
        Get
            Return ViewState("IdReservacion")
        End Get
        Set(ByVal Value As String)
            ViewState("IdReservacion") = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        lnkCarDetail.Attributes.Add("onclick", "ShowReservation('" + GeRequestApplicationPath(String.Concat("/HotelAdministrator/Pages/DisplayCarReservation.aspx?id=", Me.Request.QueryString("qs"))) + "');return false;")

        If Me.IsPostBack = False Then
            CheckReservation()
        End If
    End Sub
    Private Sub CheckReservation()
        Try
            Dim displaymessaje As Boolean = False
            Dim dat As New DataSet
            dat = GetCheckPackages(Me.idReservacion)
            If dat.Tables(0).Rows.Count > 0 Then

                'Autos
                Try
                    If Not dat.Tables(0).Rows(0)("CarReservationId") Is Nothing Then
                        Dim CarReservationId As String = "0"
                        CarReservationId = CStr(dat.Tables(0).Rows(0)("CarReservationId"))
                        Me.lnkCarDetail.HRef = "../HotelAdministrator/pages/DisplayCarReservation.aspx?id=" + CarReservationId
                        displaymessaje = True
                    Else
                        Me.lnkCarDetail.Visible = False
                    End If
                Catch ex As Exception

                End Try
                

                'Vuelos
                Try
                    Me.lnkFlDetail.Visible = False
                    If Not dat.Tables(0).Rows(0)("FlightReservationId") Is Nothing Then
                        Dim FlightReservationId As String = "0"
                        FlightReservationId = CStr(dat.Tables(0).Rows(0)("FlightReservationId"))
                        Me.lnkFlDetail.NavigateUrl = "DisplayFlightReservation.aspx?id=" + FlightReservationId
                        displaymessaje = True
                        Me.lnkFlDetail.Visible = False
                    Else
                        Me.lnkFlDetail.Visible = False
                    End If
                Catch ex As Exception

                End Try
                

                'Actividades
                Try
                    Me.lnkActDetail.Visible = False
                    If Not dat.Tables(0).Rows(0)("ActivityReservationId") Is Nothing Then
                        Dim ActivityReservationId As String = "0"
                        ActivityReservationId = CStr(dat.Tables(0).Rows(0)("ActivityReservationId"))
                        Me.lnkActDetail.NavigateUrl = "DisplayActivitieReservation.aspx?id=" + ActivityReservationId
                        displaymessaje = True
                        Me.lnkActDetail.Visible = False
                    Else
                        Me.lnkActDetail.Visible = False
                    End If
                Catch ex As Exception

                End Try
                

            End If

            Me.MenssajePackages.Visible = displaymessaje

            If displaymessaje = False Then
                lnkCarDetail.Visible = False
                lnkActDetail.Visible = False
                lnkFlDetail.Visible = False
            End If

        Catch ex As Exception
            MenssajePackages.Visible = False
        End Try
    End Sub


    Private Function GetCheckPackages(ByVal idReservacion As String) As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spCheckReservvationPackgages", conection)
        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@IdReservacion", idReservacion))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        idiomas()
    End Sub

    Private Sub idiomas()
        MenssajePackages.InnerText = PortalCulture.GetString("01055") 'This reservation is part of a package
        lnkCarDetail.InnerText = PortalCulture.GetString("01056") 'Reservation Car Detail
        lnkActDetail.Text = PortalCulture.GetString("01057") 'Reservation Activity Detail
        lnkFlDetail.Text = PortalCulture.GetString("01058") 'Reservation Flight Detail
    End Sub
End Class
