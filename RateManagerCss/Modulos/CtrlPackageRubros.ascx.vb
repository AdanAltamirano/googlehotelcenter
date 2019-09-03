Imports SearchEngine.CarRents.Common
Imports SearchEngine.CarRents.Facade
Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports SearchEngine.Activitys.Facade
Imports SearchEngine.Activitys.Comun
Partial Class CtrlPackageRubros
    Inherits System.Web.UI.UserControl
    Public Property CityId() As Integer
        Get
            Return viewstate("EmpresaCity")
        End Get
        Set(ByVal Value As Integer)
            viewstate("EmpresaCity") = Value
        End Set
    End Property
    Private Enum Rubros
        Portal = 0
        Hotel = 10
        Autos = 11
        Actividades = 27
        Vuelos = 23
        Paquetes = 9
    End Enum


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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'TODO VA AL CONTROL RUBROS

        If Not Me.IsPostBack Then
            Dim Corporativo As String = ""
            Dim IdCorporativo As Integer = 0
            If CType(Me.Page, PaginaBase).isUserChain Then
                Dim ds As DataSet
                With New HotelSistema
                    ds = .GetCorporativos(CType(Me.Page, PaginaBase).UserIdentityName)
                End With
                If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                    Corporativo = ds.Tables(0).Rows(0)("NombreCorp")
                    IdCorporativo = ds.Tables(0).Rows(0)("idCorporativo")
                End If
            End If

            LoadAerolineas()
            LoadFranquicias(Corporativo)
            loadActivities(IdCorporativo)
            ClearData()
        End If
    End Sub
    Private Sub ClearData()
        Me.chkActivity.Checked = False
        Me.txtActProm.Text = ""
        For Each item As ListItem In Me.lstActividades.Items
            item.Selected = False
        Next

        Me.chkCar.Checked = False
        Me.txtCarProm.Text = ""
        Me.txtSipp.Text = ""
        Me.chkFlight.Checked = False
        Me.ddlAerolinea.SelectedIndex = 0
        Me.ddlFranquicia.SelectedIndex = 0

    End Sub
    Public Sub LoadData(ByVal idPaquete As Integer)
        ClearData()
        Dim ds As New PaquetesArmadosData
        With New PaquetesArmadosFacade
            ds = .PaqueteArmadoGet(idPaquete)
        End With
        If Not ds Is Nothing AndAlso ds.Tables(PaquetesArmadosData.tablePaquetesArmados).Rows.Count > 0 Then
            Dim drs() As DataRow
            drs = ds.Tables(PaquetesArmadosData.tablePaquetesArmados).Select(PaquetesArmadosData.field_Rubro & "=" & Rubros.Actividades)
            If drs.Length > 0 Then
                For Each dr As DataRow In drs
                    If Not dr.IsNull(PaquetesArmadosData.field_IdRate) Then
                        Dim item As ListItem = Me.lstActividades.Items.FindByValue(dr(PaquetesArmadosData.field_IdRate))
                        If Not item Is Nothing Then
                            item.Selected = True
                        End If
                    End If
                Next
                Me.txtActProm.Text = "" & drs(0)(PaquetesArmadosData.field_CodigoPromocion)
                Me.chkActivity.Checked = True
            End If
            drs = ds.Tables(PaquetesArmadosData.tablePaquetesArmados).Select(PaquetesArmadosData.field_Rubro & "=" & Rubros.Autos)
            If drs.Length > 0 Then
                Me.chkCar.Checked = True
                txtCarProm.Text = "" & drs(0)(PaquetesArmadosData.field_CodigoPromocion)
                txtSipp.Text = "" & drs(0)(PaquetesArmadosData.field_IdRate)
                Try
                    Me.ddlFranquicia.SelectedValue = drs(0)(PaquetesArmadosData.field_IdPropiedad)
                Catch ex As Exception
                    ddlFranquicia.SelectedIndex = 0
                End Try
            End If
            drs = ds.Tables(PaquetesArmadosData.tablePaquetesArmados).Select(PaquetesArmadosData.field_Rubro & "=" & Rubros.Vuelos)
            If drs.Length > 0 Then
                chkFlight.Checked = True
                Try
                    Me.ddlAerolinea.SelectedValue = drs(0)(PaquetesArmadosData.field_IdPropiedad)
                Catch ex As Exception
                    ddlAerolinea.SelectedIndex = 0
                End Try
            End If
        End If
    End Sub

    Public Sub AddScripts(ByVal ddl As String, ByVal lst As String)
        imgRubroClose.Attributes.Add("onclick", "javascript:ShowDetails('dvPaqueteRubro', 'none');ShowDetails('" & ddl & "', '');ShowDetails('" & lst & "', '');")
    End Sub

    Private Sub loadActivities(ByVal IdCorporativo As Integer)
        'Dim ds As bookingActivityCommon
        'With New BookingActivityFacade
        'ds = .SearchActivitiesByCityId(CityId, IdCorporativo)
        'End With

        'ds.Tables(bookingActivityCommon.Tabla_Booking).Columns.Add("Texto", GetType(String), "nombre + ' - ' + Empresa ")



        'lstActividades.DataSource = ds
        'lstActividades.DataTextField = "Texto"
        'lstActividades.DataValueField = "idactividad"
        'lstActividades.DataBind()
    End Sub

    Private Sub LoadFranquicias(ByVal NombreCorp As String)
        Dim Franq As New bookingAutosComun
        With New BookingAutosFacade

            If CType(Me.Page, PaginaBase).isUserChain Then
                If NombreCorp <> "" Then
                    ' Franq = .SearchAutosFranquiciasCorporativoList(NombreCorp)
                End If
                Me.ddlFranquicia.DataSource = Franq
                Me.ddlFranquicia.DataValueField = "Franquicia"
                Me.ddlFranquicia.DataTextField = "Descripcion"
            Else
                'Franq = .SearchAutosFranquiciasList()
                Me.ddlFranquicia.DataSource = Franq
                Me.ddlFranquicia.DataValueField = "Codigo"
                Me.ddlFranquicia.DataTextField = "Descripcion"
            End If
        End With


        Me.ddlFranquicia.DataBind()
        Dim item As New ListItem
        item.Value = ""
        item.Text = PortalCulture.GetString("M000640")

        ddlFranquicia.Items.Insert(0, item)
    End Sub

    Private Sub LoadAerolineas()
        Try
            Dim ds As New DataSet
            ds.ReadXml(Server.MapPath(Request.ApplicationPath & "/Data/Airlines.xml"))
            Dim dv As DataView
            dv = ds.Tables("Airline").DefaultView
            dv.Sort = "Airline_Text asc"
            ddlAerolinea.DataSource = dv
            ddlAerolinea.DataTextField = "Airline_Text"
            ddlAerolinea.DataValueField = "Code"
            ddlAerolinea.DataBind()
            Dim item As New ListItem
            item.Value = ""
            item.Text = PortalCulture.GetString("M000640")

            ddlAerolinea.Items.Insert(0, item)
        Catch ex As Exception

        End Try
     
    End Sub
    Public Function GetLinkUrl() As String
        Dim dspaqueteArmado As New PaquetesArmadosData
        'Dim dr As DataRow
        Dim query As String = ""
        Dim PackageId As String = ""

        If Me.chkActivity.Checked Then
            For Each item As ListItem In Me.lstActividades.Items
                PackageId = "A"
                If item.Selected Then
                    If query = "" Then
                        query &= "&ActivityID=" & item.Value()
                    Else
                        query &= "," & item.Value()
                    End If
                End If
            Next
        End If
        If PackageId = "A" AndAlso Me.txtActProm.Text.Trim <> "" Then
            query &= "&ActPromotion=" & Me.txtActProm.Text.Trim
        End If

        If Me.chkCar.Checked AndAlso Me.ddlFranquicia.SelectedIndex > 0 AndAlso Me.txtSipp.Text <> "" Then
            PackageId &= "C"
            query &= "&Franquicia=" & Me.ddlFranquicia.SelectedValue.Trim & "&SippCode=" & Me.txtSipp.Text
            query &= "&CarPromotion=" & txtCarProm.Text.Trim
        End If

        If Me.chkFlight.Checked AndAlso Me.ddlAerolinea.SelectedIndex > 0 Then

        End If
        If PackageId <> "" Then
            query &= "&PckgId=" & PackageId & "H"
        End If
        Return query




    End Function

    Public Function InsertPaqueteArmado(ByVal idpaquete As Integer) As Boolean
        Dim dspaqueteArmado As New PaquetesArmadosData
        Dim dr As DataRow
        If Me.chkActivity.Checked Then
            For Each item As ListItem In Me.lstActividades.Items
                If item.Selected Then
                    dr = dspaqueteArmado.Tables(PaquetesArmadosData.tablePaquetesArmados).NewRow
                    SetPaqueteArmadoRow(dr, Me.txtActProm.Text.Trim, idpaquete, "", item.Value, Rubros.Actividades)
                    dspaqueteArmado.Tables(PaquetesArmadosData.tablePaquetesArmados).Rows.Add(dr)
                End If
            Next
        End If
        If Me.chkCar.Checked AndAlso Me.ddlFranquicia.SelectedIndex > 0 Then
            dr = dspaqueteArmado.Tables(PaquetesArmadosData.tablePaquetesArmados).NewRow
            SetPaqueteArmadoRow(dr, Me.txtCarProm.Text.Trim, idpaquete, Me.ddlFranquicia.SelectedValue, Me.txtSipp.Text, Rubros.Autos)
            dspaqueteArmado.Tables(PaquetesArmadosData.tablePaquetesArmados).Rows.Add(dr)
        End If
        If Me.chkFlight.Checked AndAlso Me.ddlAerolinea.SelectedIndex > 0 Then
            dr = dspaqueteArmado.Tables(PaquetesArmadosData.tablePaquetesArmados).NewRow
            SetPaqueteArmadoRow(dr, "", idpaquete, Me.ddlAerolinea.SelectedValue, "", Rubros.Vuelos)
            dspaqueteArmado.Tables(PaquetesArmadosData.tablePaquetesArmados).Rows.Add(dr)
        End If

        Dim dspaqueteArmadoDelete As New PaquetesArmadosData
        dr = dspaqueteArmadoDelete.Tables(PaquetesArmadosData.tablePaquetesArmados).NewRow
        SetPaqueteArmadoRow(dr, "", idpaquete, "", "", 0)
        dspaqueteArmadoDelete.Tables(PaquetesArmadosData.tablePaquetesArmados).Rows.Add(dr)
        dr.AcceptChanges()
        dr.Delete()
        If dspaqueteArmado.Tables(PaquetesArmadosData.tablePaquetesArmados).Rows.Count > 0 Then
            With New PaquetesArmadosFacade
                Dim sqlcon As System.Data.SqlClient.SqlConnection
                Dim trans As System.Data.SqlClient.SqlTransaction
                trans = .BeginTransaction(sqlcon)

                If .PaqueteArmadoDeleteByIdPaquete(dspaqueteArmadoDelete, sqlcon, trans) AndAlso .PaqueteArmadoInsert(dspaqueteArmado, sqlcon, trans) Then
                    .commitTransaction(sqlcon, trans)
                Else
                    .RollBackTransaction(sqlcon, trans)
                End If

            End With

        End If
    End Function
    Private Sub SetPaqueteArmadoRow(ByRef dr As DataRow, ByVal codProm As String, ByVal IdPaq As Integer, ByVal IdProp As String, ByVal IdRate As String, ByVal Rubro As Integer)
        Dim dspaqueteArmado As New PaquetesArmadosData
        dr(PaquetesArmadosData.field_CodigoPromocion) = codProm
        dr(PaquetesArmadosData.field_IdPaquete) = IdPaq
        dr(PaquetesArmadosData.field_IdPropiedad) = IdProp
        dr(PaquetesArmadosData.field_IdRate) = IdRate
        dr(PaquetesArmadosData.field_Rubro) = Rubro
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.chkActivity.Text = PortalCulture.GetString("01039")
        Me.chkFlight.Text = PortalCulture.GetString("01038")
        Me.chkCar.Text = PortalCulture.GetString("01040")
        Me.imgRubroClose.Value = PortalCulture.GetString("01041")
        Me.lblActProm.InnerHtml = PortalCulture.GetString("01042", True)
        Me.lblCarProm.InnerHtml = PortalCulture.GetString("01042", True)
    End Sub
End Class
