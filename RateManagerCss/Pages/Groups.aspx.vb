Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Portal.Catalogos.Facade
Imports Portal.Catalogos.Common.Data
Imports System.Collections.Generic
Imports System.Configuration.ConfigurationManager
Imports ReferencesSystem
Imports System.Data.SqlClient
Imports Portal.General.Common.Data
Imports Portal.Common
Imports Portal.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade

Partial Public Class Groups
    Inherits PaginaBase
    ' Inherits System.Web.UI.Page

    Enum dgColumns
        IdLockForGroup
        Nombre
        IdRatePlan
        CutoffDate
        StartDate
        EndDate
        Fechas
        Select_
        Delete
        IdHotel
        dgRooms
        RatePlanName
        AddRoom
        EdadAdolecente
        PlusTax
        Moneda
    End Enum

    Enum dgRColumns
        IdLockForGroup_Rooms
        IdLockForGroup
        Quantity
        idTipoHabitacion_Hotel
        Habitacion
        CodigoHabitacion
        NombreHabitacion
        SelectCommand
        Delete
    End Enum
    Enum dgConColumns
        IdConvenio
        Codigo
        Nombre
        Descripcion
        Seleccionar
        Edit
        Delete
    End Enum
    Public ReadOnly Property GetSelectedIDConvenio() As Integer
        Get
            If dgConvenio.SelectedIndex = -1 Then
                Return -1
            Else
                Return dgConvenio.DataKeys(dgConvenio.SelectedIndex)
            End If
        End Get
    End Property
    Public Property SelectRoomIDX() As String
        Get
            Return CType(ViewState("_SelectRoomIDX"), String)
        End Get
        Set(ByVal value As String)
            ViewState("_SelectRoomIDX") = value
        End Set
    End Property

    Public Property LockEditing() As Boolean
        Get
            Return CType(ViewState("_LockEditing"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            ViewState("_LockEditing") = value
        End Set
    End Property

    Public Property idLockForGroup() As Integer
        Get
            Return CType(ViewState("_idLockForGroup"), Integer)
        End Get
        Set(ByVal value As Integer)
            ViewState("_idLockForGroup") = value
        End Set
    End Property

    Public Property SelectLockForGroupIDX() As String
        Get
            Return CType(ViewState("_SelectLockForGroupIDX"), String)
        End Get
        Set(ByVal value As String)
            ViewState("_SelectLockForGroupIDX") = value
        End Set
    End Property
    Public Property Currency() As String
        Get
            Return CType(ViewState("_Currency"), String)
        End Get
        Set(ByVal value As String)
            ViewState("_Currency") = value
        End Set
    End Property
    Public ReadOnly Property idCorporate() As Integer
        Get
            If IsSupervisor Then
                if idSelectedCorpororate.Value="" then
                    Return -1
                Else
                    Return CInt(idSelectedCorpororate.Value)
                End if

            ElseIf IdCorporativoUserChain > -1 Then
                Return CInt(IdCorporativoUserChain)
            Else
                Return -1
            End If
        End Get
    End Property

    Protected ReadOnly Property RateModeView() As Integer
        Get
            Dim value As Integer = 0

            If (Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Adult", GetFareFor("Adult", False), False) _
                    OrElse _
                    Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Children", GetFareFor("Child", False), False) _
                    OrElse _
                    Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Teen", GetFareFor("Teen", False), False) _
                OrElse _
                      (Me.CtrlPlanFaresExc2.FieldException <> "NNNNNNN" _
                        AndAlso (Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Adult", GetFareFor("Adult", False), False) _
                        OrElse _
                        Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Children", GetFareFor("Child", False), False) _
                        OrElse _
                        Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Teen", GetFareFor("Teen", False), False) _
                        ))) Then
                value = 1
            End If

            Return value
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Me.txtChildFare.Attributes.Add("onChange", "javascript:FillPrices('" & CtrlPlanFares2.iddgChild & "','txtChildrenFare'" & ",'" & Me.txtChildFare.ClientID & "')")
            Me.txtAdultFare.Attributes.Add("onChange", "javascript:FillPrices('" & CtrlPlanFares2.iddgAdult & "','txtAdultFare'" & ",'" & Me.txtAdultFare.ClientID & "')")
            Me.txtTeenFare.Attributes.Add("onChange", "javascript:FillPrices('" & CtrlPlanFares2.iddgTeen & "','txtTeenFare'" & ",'" & Me.txtTeenFare.ClientID & "')")

            LoadCorporates()
            LoadRatesSelect()
        End If
        lblError.Visible = False
        lblErrorFare.Visible = False
        rngValCutoffDate.MinimumValue = Today.AddDays(1).Date
        rngValEndDate.MinimumValue = Today.AddDays(1).Date
        rngValStartDate.MinimumValue = Today.AddDays(1).Date

        rngValCutoffDate.MaximumValue = DateTime.MaxValue.Date
        rngValEndDate.MaximumValue = DateTime.MaxValue.Date
        rngValStartDate.MaximumValue = DateTime.MaxValue.Date
    End Sub

    Private Sub LoadCorporates()
        Dim ds As DataSet
        With New HotelSistema
            ds = .GetCorporativosHoteles(0)
        End With
        If IsSupervisor Then
            dgCorporates.Columns(1).FooterText = ds.Tables(0).Rows.Count & " " & PortalCulture.GetString("01219")
            dgCorporates.DataSource = ds
            dgCorporates.DataBind()
            dgCorporates.SelectedIndex = -1
            trCorporates.Visible = True
            btnSelectCorporate.Visible = True
        Else
            'Dim corporates As DataRow() = ds.Tables(0).Select("idCorporativo='" + Me.idCorporate.ToString() + "'")
            'If corporates.Length > 0 Then
            '    Me.idSelectedCorpororate.Value = corporates(0)("idCorporativo")
            '    Me.idSelectedCorpororateName.Value = corporates(0)("NombreCorp")
            'Else

            '    MyBase.redirectTo(PaginaBase.pages.Home)

            'End If
            'trCorporates.Visible = False
            'btnSelectCorporate.Visible = False
            ' ResetForm()
        End If
        divBlock.Visible = False
    End Sub

    Private Sub ResetForm()
        LoadGroup_Grid(idCorporate,ctrlAutoComplete1.GetFilter)
        'btnDeleteGroup.Visible = False
        'btnEditGroup.Visible = False
        'LoadHotels(idCorporate)
        'ClientScript.RegisterStartupScript(Me.GetType(), "Inicializa Ciudad", "SetInitialLocation('MX', '', '', '');", True)
        ''ddlCountries_SelectedIndexChanged(New Object, New EventArgs)
        txtCode.Text = String.Empty
        txtDescription.Text = String.Empty
        txtName.Text = String.Empty
        lblGroupSubtitle.Text = String.Empty
        'hdnGeneralRestrictions.Value = String.Empty
        'hdnEspecificRestrictions.Value = String.Empty
        'hdnUsedEspecificRestrictions.Value = String.Empty
        'hdnIdAgreement.Value = 0
        tabEditGroup.Visible = False
        divBlock.Visible = False

        txtCode.Enabled = True
    End Sub

    Protected Sub dgCorporates_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCorporates.ItemCommand
        If e.CommandName = "Select" Then
            Dim values() As String = e.CommandArgument.ToString().Split("|".ToCharArray)
            If values.Length >= 2 Then
                idSelectedCorpororate.Value = values(0)
                idSelectedCorpororateName.Value = values(1)
                trCorporates.Visible = False
                lblTitleGroupData.Text = values(1)
                getHotels(Integer.Parse(values(0)))
                loadrateplans("")
                ResetForm()
                divMainGroup.Visible = True
            End If

        End If
    End Sub

    Private Sub LoadGroup_Grid(ByVal idCorporate As Integer,ByVal filter As String)
        Dim ds As DataSet = (New RatePlanFacade).GetGroupsByIdCorporate(idCorporate)
        Dim dv As DataView

        If Not Me.dsEmpty(ds) Then
            dv = ds.Tables(0).DefaultView
            dv.RowFilter = filter

            dgConvenio.DataSource = dv
            dgConvenio.DataKeyField = "IdConvenio"
            dgConvenio.DataBind()
        End If

        'ddlConvenios.DataSource = ds
        'ddlConvenios.DataTextField = "ConvenioDescription"
        'ddlConvenios.DataValueField = "IdConvenio"
        'ddlConvenios.DataBind()
        'ddlConvenios.Items.Insert(0, New ListItem(PortalCulture.GetString("01566", False), -1))
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveGroup.Click

        Dim dsConven As DataSet
        Dim idConvenio
        Dim adapter As New RatePlanAccess()
        Dim isEdit As Boolean
        If Not GetSelectedIDConvenio = -1 Then
            idConvenio = GetSelectedIDConvenio
            isEdit = True
        Else
            isEdit = False
        End If

        If adapter.SaveGroup(isEdit, idConvenio, Integer.Parse(idSelectedCorpororate.Value), txtName.Text.Trim(), txtCode.Text.Trim(), txtDescription.Text.Trim()) Then
            ResetForm()
            lblErrorGroup.Visible = False
        Else
            lblErrorGroup.Visible = True
        End If

    End Sub
    'Protected Sub ddlConvenios_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlConvenios.SelectedIndexChanged
    '    hidGridLocked.Value = String.Empty
    '    If Not GetSelectedIDConvenio = -1 Then
    '        LoadGroup(GetSelectedIDConvenio)
    '        tabEditGroup.Visible = False
    '        divBlock.Visible = True
    '        'hdnIdAgreement.Value = ddlAgreementWorking.SelectedValue
    '        'txtNoAgreement.Enabled = False
    '        ResetFormLock(False)
    '        LoadLocks(Integer.Parse(GetSelectedIDConvenio))
    '        'btnEditGroup.Visible = True
    '        'btnDeleteGroup.Visible = True
    '        btnCancelGroup.Visible = True
    '        txtCode.Enabled = False
    '    Else
    '        ResetForm()
    '        btnCancelGroup.Visible = False
    '        txtCode.Enabled = True

    '    End If
    'End Sub

    Private Sub getHotels(ByVal idCorp As Integer)
        With New Hoteles
            ddlHotels.DataSource = .LoadHotelsByIdCorp(idCorp)
        End With

        ddlHotels.DataTextField = "nombre"
        ddlHotels.DataValueField = "idHotel"
        ddlHotels.DataBind()
    End Sub
    Private Sub LoadGroup(ByVal idCorporativo As Integer)
        'Dim dsConvenio As DataSet = (New RatePlanAccess).GetAgreement(idCorporativo, PortalCulture.GetIDCulture)

        If Not GetSelectedIDConvenio = -1 Then
            txtName.Text = dgConvenio.SelectedItem.Cells(dgConColumns.Nombre).Text
            txtCode.Text = dgConvenio.SelectedItem.Cells(dgConColumns.Codigo).Text
            txtDescription.Text = IIf(dgConvenio.SelectedItem.Cells(dgConColumns.Descripcion).Text = "&nbsp;", "", dgConvenio.SelectedItem.Cells(dgConColumns.Descripcion).Text)
            lblGroupSubtitle.Text = "<table width='100%'><tr><td>" & PortalCulture.GetString("00001") & ": " & txtCode.Text & _
                                    "</td><td> " & PortalCulture.GetString("00073") & ": " & txtName.Text & _
                                    "</td></tr><tr><td colspan='2'>" & PortalCulture.GetString("M000152") & ": " & txtDescription.Text & "</td></tr></table>"
        End If
    End Sub


    Protected Sub btnSelectCorporate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelectCorporate.Click
        ResetForm()
        LoadCorporates()
        divMainGroup.Visible = False
    End Sub

    Private Sub Groups_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        tabLock.Visible = Not GetSelectedIDConvenio = -1

        lblTitleGroups.Text = PortalCulture.GetString("01557")
        lblCode.Text = PortalCulture.GetString("M000432")
        lblTitleGroupData.Text = PortalCulture.GetString("01556")
        lblName.Text = PortalCulture.GetString("00073")
        lblDescripcion.Text = PortalCulture.GetString("M000152")
        btnSaveGroup.Text = PortalCulture.GetString("M000357")
        'btnDeleteGroup.Text = PortalCulture.GetString("00103")
        btnSelectCorporate.Text = PortalCulture.GetString("01218")
        ddlShowRates.SelectedIndex = RateModeView
        ddlShowRates.Visible = False
        lblBlocks.Text = PortalCulture.GetString("01560")
        lblGroup.Text = PortalCulture.GetString("01557")
        'btnEditGroup.Text = PortalCulture.GetString("00093")
        btnNewLock.Text = PortalCulture.GetString("00102")
        btnNewGroup.Text = PortalCulture.GetString("00102")
        btnCancelGroup.Text = PortalCulture.GetString("A00143")
        lblTitleGeneralInformation.Text = PortalCulture.GetString("01216")
        lblHotel.Text = PortalCulture.GetString("M000026")
        lblCutoffDate.Text = PortalCulture.GetString("01561")
        lblStartDate.Text = PortalCulture.GetString("00108")
        lblEndDate.Text = PortalCulture.GetString("M000111")
        btnCancelBlock.Text = PortalCulture.GetString("M000143")
        btnAddLock.Text = PortalCulture.GetString("M000357")
        lblRoomType.Text = PortalCulture.GetString("00729")
        lblQuantity.Text = PortalCulture.GetString("M000125")
        lblPrecio.Text = PortalCulture.GetString("00087", True)
        Me.LblChildPrice.Text = PortalCulture.GetString("00088", True)
        lblExtraAdolescente.Text = PortalCulture.GetString("01311", True)
        lblAdolescentePrice.Text = PortalCulture.GetString("01282", True)
        Me.lblExtraAdult.Text = PortalCulture.GetString("M000226", True)
        Me.lblExtraChildPrice.Text = PortalCulture.GetString("M000227", True)
        btnSaveFare.Text = PortalCulture.GetString("M000357")
        btnCancelFare.Text = PortalCulture.GetString("M000384")
        lblErrorGroup.Text = PortalCulture.GetString("015632")
        lblErrorLock.Text = PortalCulture.GetString("01564")

        rngValCutoffDate.ErrorMessage = PortalCulture.GetString("00110")
        rngValStartDate.ErrorMessage = PortalCulture.GetString("00110")
        rngValEndDate.ErrorMessage = PortalCulture.GetString("00110")
        cvalDates.ErrorMessage = PortalCulture.GetString("01569")

        rngValQuantity.ErrorMessage = PortalCulture.GetString("01568")
        ReValAdult.ErrorMessage = PortalCulture.GetString("00083")
        ReValChild.ErrorMessage = PortalCulture.GetString("00083")
        ReValAdoslecenteFare.ErrorMessage = PortalCulture.GetString("00083")
        ReValAdultExtraPrice.ErrorMessage = PortalCulture.GetString("00083")
        ReValChildExtraPrice.ErrorMessage = PortalCulture.GetString("00083")
        ReValTeenExtraPrice.ErrorMessage = PortalCulture.GetString("00083")

        Dim script As String = String.Empty

        'Dim scriptMsg As String = CtlMensajes3.getShow(btnDeleteGroup2.ClientID, "Grupos", "¿Seguro que quiere eliminar este grupo?")
        'btnDeleteGroup.Attributes.Add("onClick", scriptMsg & "; $(window.parent.document).find('html, body').animate({ scrollTop: 0 }, 'slow');return false;")


    End Sub


    Protected Sub btnAddLock_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddLock.Click
        Dim datetime1, datetime2, datetimecutoff As DateTime
        Dim ValidDates As Boolean = False

        ValidDates = ValidarLocks(datetime1, datetime2, datetimecutoff)

        With New RatePlanFacade
            If LockEditing Then
                If .UpdateLockForGroup(idLockForGroup, datetime1, datetime2, ddlHotels.SelectedValue, datetimecutoff, ddlRatePlan.SelectedValue) Then
                    LoadLocks(GetSelectedIDConvenio)
                    ResetFormLock(False)
                    lblErrorLock.Visible = False
                    LockEditing = False
                Else
                    lblErrorLock.Visible = True
                End If
            Else
                If ValidDates Then
                    If .InsertLockForGroup(datetime1, datetime2, GetSelectedIDConvenio, ddlHotels.SelectedValue, datetimecutoff, ddlRatePlan.SelectedValue) Then
                        LoadLocks(GetSelectedIDConvenio)
                        ResetFormLock(False)
                        lblErrorLock.Visible = False
                    Else
                        lblErrorLock.Visible = True
                    End If
                End If
            End If
        End With
    End Sub

    Private Function AddRatePlan(ByVal idrateplan As String) As Boolean

        Dim idDicDesc As String = "0"
        Dim idDicShortDesc As String = "0"
        Dim idDicProm As String = "0"


        Dim dsRate As New RatePlanData
        Dim rRate As DataRow

        With New AplicacionPortalFacade
            If (Not .InsertAllAplicacionPortalForCorp(Integer.Parse(idSelectedCorpororate.Value), Integer.Parse(ddlHotels.SelectedValue), idrateplan)) Then Return False
        End With


        rRate = dsRate.Tables(dsRate.RATEPLAN_TABLE).NewRow()
        With rRate
            .Item(dsRate.FIELD_IDRATEPLAN) = idrateplan
            .Item(dsRate.FIELD_CODIGOTARIFA) = idrateplan
            .Item(dsRate.FIELD_DESCRIPTION) = "Tarifa Grupo"
            .Item(dsRate.FIELD_IDHOTEL) = ddlHotels.SelectedValue
            .Item(dsRate.FIELD_SEGMENT) = "O"
            .Item(dsRate.FIELD_IDDICDESC) = "0"
            .Item(dsRate.FIELD_SegmentRacPrinc) = 0
            .Item(dsRate.FIELD_NAME) = "Tarifa Grupo"
            .Item(dsRate.FIELD_IDDICSHORTDESC) = "0"
            .Item(dsRate.FIELD_GDS) = False
            .Item(dsRate.FIELD_GDSAPPLY) = "NNNN"
            .Item(dsRate.FIELD_PORTAL) = True
            .Item(dsRate.FIELD_UNIPANTALLA) = False
            .Item(dsRate.FIELD_ADS) = False
        End With

        dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows.Add(rRate)

        With New RatePlanFacade
            If .InsertRatePlan(dsRate, idDicDesc, idDicShortDesc, idDicProm) Then
                AddDictionary(idDicDesc, "Tarifa de Grupo", "Tarifa de Grupo")
                AddDictionary(idDicShortDesc, "Tarifa de Grupo", "Tarifa de Grupo")
                Return True
            Else
                Return False
            End If
        End With
    End Function

    Private Function AddDictionary(ByVal idIndice As String, ByVal TextESP As String, ByVal TextING As String) As Boolean
        Dim rowparent As DataRow
        Dim dsDictionary As New DictionaryData
        Dim rowDictionary As DataRow
        With dsDictionary.IndexTable
            rowparent = .NewRow()
            ' add row to index table
            .Rows.Add(rowparent)
            ' assign dictionary id setting row status to modified 
            rowparent(DictionaryData.IndexTablefields.Indice) = idIndice
            ' set the  index table to unmodified state
            .AcceptChanges()
        End With

        rowDictionary = dsDictionary.Tables(DictionaryData.TablaDiccionario).NewRow()
        rowDictionary(DictionaryData.IdIdioma_FIELD) = "1"
        rowDictionary(DictionaryData.Indice_FIELD) = idIndice
        rowDictionary(DictionaryData.Texto_FIELD) = TextESP
        rowDictionary.SetParentRow(rowparent)
        dsDictionary.Tables(DictionaryData.TablaDiccionario).Rows.Add(rowDictionary)

        rowDictionary = dsDictionary.Tables(DictionaryData.TablaDiccionario).NewRow()
        rowDictionary.Item(DictionaryData.IdIdioma_FIELD) = "2"
        rowDictionary(DictionaryData.Indice_FIELD) = idIndice
        rowDictionary.Item(DictionaryData.Texto_FIELD) = TextING
        rowDictionary.SetParentRow(rowparent)
        dsDictionary.Tables(DictionaryData.TablaDiccionario).Rows.Add(rowDictionary)

        With New SystemDictionary(AppSettings("HotelConnection"))
            If .InsertNewDictionary(dsDictionary) Then
                Return True
            End If
        End With
    End Function
    Private Function ValidarLocks(ByRef d1 As DateTime, ByRef d2 As DateTime, ByRef dc As DateTime) As Boolean
        Dim dr() As DataRow
        If Not DateTime.TryParse(txtStartDate.Text, d1) Or Not DateTime.TryParse(txtEndDate.Text, d2) Or Not DateTime.TryParse(txtCutoffDate.Text, dc) Then
            lblError.Text = PortalCulture.GetString("00634")
            lblError.Visible = True
            Return False
        End If

        If Today >= d1 Or Today >= d2 Or Today >= dc Or d1 >= d2 Or dc > d2 Then
            lblError.Text = PortalCulture.GetString("00634")
            lblError.Visible = True
            Return False
        End If

        Dim ds As DataSet
        With New RatePlanFacade
            ds = .GetLockForGroup(GetSelectedIDConvenio)
        End With

        dr = ds.Tables(0).Select("idHotel=" & ddlHotels.SelectedValue & " and (('" & d1.ToString("MM/dd/yyyy") & "' <= EndDate and '" & d1.ToString("MM/dd/yyyy") & "'>= StartDate) " & _
                                 "or (StartDate <= '" & d2.ToString("MM/dd/yyyy") & "' and  StartDate>='" & d1.ToString("MM/dd/yyyy") & "'  ) )")

        If dr.Length = 0 Then
            Return True
        Else
            lblError.Text = PortalCulture.GetString("00514")
            lblError.Visible = True
        End If

    End Function

    Private Sub LoadLocks(ByVal IdConvenio As Integer)
        '(New RatePlanFacade).GetLockForGroup(idConvenio)
        Dim ds As DataSet
        With New RatePlanFacade
            ds = .GetLockForGroup(IdConvenio)
        End With
        dgLocksForGroup.DataSource = ds
        dgLocksForGroup.DataKeyField = "IdLockForGroup"
        dgLocksForGroup.DataBind()
    End Sub

    Protected Sub dgLocksForGroup_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgLocksForGroup.ItemCommand
        hidGridLocked.Value = String.Empty
        If e.CommandName = "Delete" Then
            With New RatePlanFacade
                If .DeleteLockForGroup(dgLocksForGroup.DataKeys(e.Item.ItemIndex)) Then
                    LoadLocks(GetSelectedIDConvenio)
                    ResetFormLock(False)
                End If
            End With
        ElseIf e.CommandName = "Select" Then
            tabEditLock.Visible = True
            btnAddLock.Text = PortalCulture.GetString("M000357")
            lblLockSubtitle.Text = "- " & PortalCulture.GetString("01250") & " [" & dgLocksForGroup.DataKeys(e.Item.ItemIndex) & "]"
            ddlHotels.SelectedValue = e.Item.Cells(dgColumns.IdHotel).Text
            txtCutoffDate.Text = e.Item.Cells(dgColumns.CutoffDate).Text
            txtStartDate.Text = e.Item.Cells(dgColumns.StartDate).Text
            txtEndDate.Text = e.Item.Cells(dgColumns.EndDate).Text
            LockEditing = True
            idLockForGroup = CInt(e.Item.Cells(dgColumns.IdLockForGroup).Text)
        ElseIf e.CommandName = "AddRoom" Then
            dgLocksForGroup.SelectedIndex = e.Item.ItemIndex
            SelectRoomIDX = -1

            If LoadRooms(e.Item.Cells(dgColumns.IdHotel).Text, e.Item.Cells(dgColumns.IdRatePlan).Text) Then
                ddlRoomType.Enabled = True

                'Dim juniors As Integer
                'If Integer.TryParse(e.Item.Cells(dgColumns.EdadAdolecente).Text, juniors) AndAlso juniors > 0 Then
                '    CtrlPlanFares2.isConfigAdolescente = 1
                '    CtrlPlanFaresExc2.isConfigAdolescente = 1
                '    lblExtraAdolescente.Visible = True
                '    txtExtraTeenPrice.Visible = True
                '    txtTeenFare.Visible = True
                '    lblAdolescentePrice.Visible = True
                '    Me.reqExtraTeenPrice.Visible = True
                '    Me.reqTeenFare.Visible = True
                'Else

                '    CtrlPlanFares2.isConfigAdolescente = 0
                '    CtrlPlanFaresExc2.isConfigAdolescente = 0
                '    lblExtraAdolescente.Visible = False
                '    txtExtraTeenPrice.Visible = False
                '    txtTeenFare.Visible = False
                '    lblAdolescentePrice.Visible = False

                '    Me.reqExtraTeenPrice.Visible = False
                '    Me.reqTeenFare.Visible = False


                'End If
                'Room_RefillRestrictions(0)

                '       Limpiar txtsssss
                txtQuantity.Text = String.Empty
                Me.txtExtraAdultPrice.Text = String.Empty
                Me.txtExtraChildPrice.Text = String.Empty
                Me.txtExtraTeenPrice.Text = String.Empty
                txtAdultFare.Text = String.Empty
                txtChildFare.Text = String.Empty
                txtTeenFare.Text = String.Empty

                hidGridLocked.Value = e.Item.Cells(dgColumns.dgRooms).FindControl("dgRooms").ClientID
                Currency = e.Item.Cells(dgColumns.Moneda).Text
                ClientScript.RegisterClientScriptBlock(GetType(Group), "currencys", "$(document).ready(function() { $('span.currency').html('" & Currency & "');});", True)
                Dim strIncTax As String = " " & PortalCulture.GetString("00610") & " "

                If e.Item.Cells(dgColumns.PlusTax).Text = "True" Then
                    strIncTax = " " & PortalCulture.GetString("00611") & " "
                End If
                lblMonTar.Text = strIncTax
            End If
        End If
    End Sub

    Private Function LoadRooms(ByVal idhotel As Integer, ByVal idRatePlan As String) As Boolean
        ddlRoomType.Items.Clear()
        Dim dsRooms As New RoomsHotelData
        'Dim Rooms As New Portal.Hotel.Common.Data.RoomsHotelData

        With New RoomFacade
            dsRooms = .GetRoomsByIdRatePlan(idhotel, idRatePlan)
        End With
        'With New Portal.Hotel.Facade.RoomFacade
        '    Rooms = .getRooms(idhotel, PortalCulture.GetIDCulture)
        'End With
        With dsRooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL)
            If .Rows.Count > 0 Then
                .Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ",1,25)")
                .Columns.Add("valor", System.Type.GetType("System.String"), RoomsHotelData.FLD_ID_ROOM_HOTEL)

                ddlRoomType.DataTextField = "texto" 'Rooms.FLD_ROOM_CODE
                ddlRoomType.DataValueField = "valor"
                ddlRoomType.DataSource = dsRooms
                ddlRoomType.DataBind()

                LoadRoomsQuantity(ddlRoomType.SelectedValue)
                Return True
            Else
                lblError.Visible = True
                lblError.Text = PortalCulture.GetString("01567")
            End If
        End With

        Return False

    End Function

    Private Sub LoadRoomsQuantity(ByVal idRoom As Integer)
        Dim Stock As Integer
        Dim strError As String = String.Empty
        Dim data As RoomsInventoryData
        Dim IniDate As Date
        Dim EndDate As Date
        ddlQuantity.Items.Clear()

        IniDate = CType(dgLocksForGroup.Items(dgLocksForGroup.SelectedIndex).Cells(dgColumns.StartDate).Text, Date)
        EndDate = CType(dgLocksForGroup.Items(dgLocksForGroup.SelectedIndex).Cells(dgColumns.EndDate).Text, Date)

        With New RoomsInventoryFacade
            data = .getInventoryByDate_Data(idRoom, IniDate, EndDate)
        End With

        Stock = data.Tables(0).Compute("Min(Disponibilidad)", "")

        If Stock > 0 Then
            For quantity As Integer = 1 To Stock
                ddlQuantity.Items.Add(New ListItem(quantity.ToString(), quantity))
            Next
        Else
            lblError.Visible = True
            lblError.Text = PortalCulture.GetString("01567") & " : " & strError
        End If
    End Sub

    Private Sub dgLocksForGroup_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgLocksForGroup.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then

            CType(e.Item.Cells(dgColumns.AddRoom).FindControl("lnkAddRoom"), LinkButton).Text = PortalCulture.GetString("01562")

            Dim LK As HyperLink
            Dim LK2 As LinkButton


            LK2 = e.Item.Cells(dgColumns.Delete).FindControl("lnkEliminar2")
            LK = e.Item.Cells(dgColumns.Delete).FindControl("lnkEliminar")
            LK.Text = PortalCulture.GetString("00103")
            'LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), _
            'PortalCulture.GetString("00613") & ", " & PortalCulture.GetString("00464"))

            LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("01557"), PortalCulture.GetString("01558"))
            Dim dg As DataGrid = CType(e.Item.Cells(dgColumns.dgRooms).FindControl("dgRooms"), DataGrid)
            dg.DataSource = (New RatePlanFacade).GetLockForGroup_RoomsList(dgLocksForGroup.DataKeys(e.Item.ItemIndex))
            dg.DataKeyField = "IdLockForGroup_Rooms"
            dg.DataBind()
        End If

        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgColumns.Nombre).Text = PortalCulture.GetString("00073")
            e.Item.Cells(dgColumns.Fechas).Text = PortalCulture.GetString("01561") & "<br>" & PortalCulture.GetString("M000192") & "<br>" & PortalCulture.GetString("M000193")
            e.Item.Cells(dgColumns.dgRooms).Text = PortalCulture.GetString("A00041")
            'e.Item.Cells(dgcolumns.orden).Text = PortalCulture.GetString("00920")
        ElseIf e.Item.ItemType = ListItemType.Footer Then

        End If
    End Sub

    Private Sub ResetFormLock(ByVal showEdit As Boolean)
        txtCutoffDate.Text = String.Empty
        txtStartDate.Text = String.Empty
        txtEndDate.Text = String.Empty
        lblLockSubtitle.Text = String.Empty
        dgLocksForGroup.SelectedIndex = -1
        tabEditLock.Visible = showEdit
    End Sub

    Public Sub btnNewLock_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        btnAddLock.Text = "Agregar"
        ResetFormLock(True)
        hidGridLocked.Value = String.Empty
    End Sub

    Private Sub btnCancelBlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelBlock.Click
        ResetFormLock(False)
    End Sub

    Public Sub btnT_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetFormLock(False)
    End Sub
    Sub LoadRatesSelect()
        ddlShowRates.Items.Clear()
        ddlShowRates.Items.Add(PortalCulture.GetString("01368"))
        ddlShowRates.Items.Add(PortalCulture.GetString("01369"))
    End Sub
    Public Function GetFareRestrictions() As FaresRestrictionsData
        'CtrlPlanFaresExc2.FieldException = "NNNNNNN"
        If CtrlPlanFares2.m_iFareId = 0 Then
            'CtrlPlanFaresExc2.FieldException = CtrRateAplication1.Exceptions
            CtrlPlanFaresExc2.FieldException = "NNNNNNN"
        End If
        Return Me.CtrlPlanFares2.GetFareRestrictions()

    End Function



    Protected Sub dgRooms_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        hidGridLocked.Value = String.Empty
        If e.CommandName = "Delete" Then
            With New RatePlanFacade
                If .DeleteLockForGroup_Rooms(e.Item.Cells(dgRColumns.IdLockForGroup_Rooms).Text) Then
                    LoadLocks(GetSelectedIDConvenio)
                End If

            End With

        ElseIf e.CommandName = "Select" Then
            Dim i As Integer
            For i = 0 To dgLocksForGroup.DataKeys.Count
                If dgLocksForGroup.DataKeys(i) = e.Item.Cells(dgRColumns.IdLockForGroup).Text Then
                    SelectLockForGroupIDX = i
                    dgLocksForGroup.SelectedIndex = i
                    Exit For
                End If
            Next

            Dim dgrooms As DataGrid = CType(source, DataGrid)
            dgrooms.SelectedIndex = e.Item.ItemIndex

            SelectRoomIDX = e.Item.ItemIndex
            If LoadRooms(dgLocksForGroup.Items(SelectLockForGroupIDX).Cells(dgColumns.IdHotel).Text, dgLocksForGroup.Items(SelectLockForGroupIDX).Cells(dgColumns.IdRatePlan).Text) Then

                LoadRoomsQuantity(CType(ddlRoomType.SelectedValue, Integer))
                ddlRoomType.SelectedValue = e.Item.Cells(dgRColumns.idTipoHabitacion_Hotel).Text

                'For Each item As ListItem In ddlRoomType.Items
                '    If item.Value.Split("|")(0) = e.Item.Cells(dgRColumns.idTipoHabitacion_Hotel).Text Then
                '        item.Selected = True
                '    End If
                'Next
                'LoadFare(e.Item.Cells(dgRColumns.idTarifa).Text)


                Dim juniors As Integer
                If Integer.TryParse(e.Item.Cells(dgColumns.EdadAdolecente).Text, juniors) AndAlso juniors > 0 Then
                    CtrlPlanFaresExc2.isConfigAdolescente = 1
                    CtrlPlanFares2.isConfigAdolescente = 1
                    lblExtraAdolescente.Visible = True
                    txtExtraTeenPrice.Visible = True
                    txtTeenFare.Visible = True
                    lblAdolescentePrice.Visible = True
                    Me.reqExtraTeenPrice.Visible = True
                    Me.reqTeenFare.Visible = True
                Else
                    CtrlPlanFaresExc2.isConfigAdolescente = 0
                    CtrlPlanFares2.isConfigAdolescente = 0
                    lblExtraAdolescente.Visible = False
                    txtExtraTeenPrice.Visible = False
                    txtTeenFare.Visible = False
                    lblAdolescentePrice.Visible = False
                    Me.reqExtraTeenPrice.Visible = False
                    Me.reqTeenFare.Visible = False
                End If
                'Room_RefillRestrictions(e.Item.Cells(dgRColumns.idTarifa).Text)

                'txtQuantity.Text = e.Item.Cells(dgRColumns.Quantity).Text
                ddlQuantity.SelectedValue = CType(e.Item.Cells(dgRColumns.Quantity).Text, Integer)

                hidGridLocked.Value = dgrooms.ClientID
                ddlRoomType.Enabled = False
                Currency = dgLocksForGroup.Items(SelectLockForGroupIDX).Cells(dgColumns.Moneda).Text
                ClientScript.RegisterStartupScript(GetType(Group), "currencys", "$(document).ready(function() { $('span.currency').html('" & Currency & "');});", True)
                Dim strIncTax As String = " " & PortalCulture.GetString("00610") & " "

                'If e.Item.Cells(dgColumns.PlusTax).Text = "True" Then
                '    strIncTax = " " & PortalCulture.GetString("00611") & " "
                'End If
                lblMonTar.Text = strIncTax
            End If
        End If
    End Sub

    Public Sub dgRooms_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then

            Dim LK As HyperLink
            Dim LK2 As LinkButton

            LK2 = e.Item.Cells(dgRColumns.Delete).FindControl("lnkEliminar2")
            LK = e.Item.Cells(dgRColumns.Delete).FindControl("lnkEliminar")
            LK.Text = PortalCulture.GetString("00103")

            LK.NavigateUrl = CtlMensajes2.getShow(LK2.ClientID, PortalCulture.GetString("01557"), PortalCulture.GetString("01559"))

            CType(e.Item.Cells(dgRColumns.SelectCommand).FindControl("lnkEdit"), LinkButton).Text = PortalCulture.GetString("00093")

        End If

        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgRColumns.Quantity).Text = PortalCulture.GetString("M000125")
            e.Item.Cells(dgRColumns.Habitacion).Text = PortalCulture.GetString("00141")

        ElseIf e.Item.ItemType = ListItemType.Footer Then

        End If
    End Sub
    Public Function UpdateFare(ByVal idRoom As Integer, ByRef idFare As Integer, ByVal f1 As Date, ByVal f2 As Date, ByVal ch As String, ByVal rp As String, ByVal f1last As String, ByVal f2last As String, ByVal sroom As String, ByVal publish As Boolean, ByVal isOcupacion As Boolean, ByVal FareAdultMin As Double, ByVal FareChildMin As Double, ByVal FareJuniorMin As Double, ByRef scorreo As String _
                               , ByVal idrateplan As String, ByVal exep As String, ByVal idhotel As Integer) As Boolean
        Dim RatePlanRow As RowRatePlan
        Dim flag As Boolean = False

        Dim sdato As String
        Dim sdatodespues As String

        'SetDefaultValues()
        Dim ds As RatePlanData
        With New RatePlanFacade
            ds = .GetDataRatePlan(idrateplan, idhotel)
        End With
        If ds.Tables(ds.RATEPLAN_TABLE).Rows.Count > 0 Then
            With ds.Tables(ds.RATEPLAN_TABLE).Rows(0)
                Try
                    RatePlanRow = New RowRatePlan
                    'Me.RatePlanRow.DESCRIPTION = .Item(ds.FIELD_DESCRIPTION).ToString
                    'Me.RatePlanRow.IDDICDESC = CInt(Val(.Item(ds.FIELD_IDDICDESC)))
                    RatePlanRow.IDRATEPLAN = .Item(ds.FIELD_IDRATEPLAN)
                    RatePlanRow.SEGMENT = .Item(ds.FIELD_SEGMENT)
                    RatePlanRow.RATECODE = .Item(ds.FIELD_CODIGOTARIFA)
                Catch ex As Exception
                End Try
            End With
        End If

        If isOcupacion Then
            txtAdultFare.Text = FareAdultMin
            txtChildFare.Text = FareChildMin
            txtTeenFare.Text = FareJuniorMin
        End If

        If SelectRoomIDX = -1 Then
            If SaveNewFare(idRoom, idFare, f1, f2, String.Format("{0} {1}", sroom, rp), sdato, exep, idrateplan) Then
                scorreo = (New Util.Utility).GeneraCorreoXslt(String.Empty, sdato)
                'CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Crear, "Se creó la tarifa de la habitación " & ch.Substring(0, ch.IndexOf("--")) & " de la fecha " & f1 & " a la fecha " & f2 & " con el rateplan " & Me.RatePlanRow.RATECODE, String.empty, string.empty, sdato)
                flag = True
            End If
        Else
            Dim dsFaresUp As New FaresData
            Dim dsBefore As New DataSet

            'If UpdateFare(idRoom, f1, f2, dsBefore, dsFaresUp, idrateplan, exep, idFare, publish) Then

            '    Dim sreference As String = String.Empty

            '    'AddRatePlan(dsBefore, "Descr_rateplan", String.Format("{0} {1}", sroom, rp))
            '    sdato = Util.Utility.GetXml(dsFaresUp.FARES_TABLE, "UpdateRate", dsBefore)

            '    AddRatePlan(dsFaresUp, "Descr_rateplan", String.Format("{0} {1}", sroom, rp))
            '    sdatodespues = Util.Utility.GetXml(dsFaresUp.FARES_TABLE, "UpdateRate", dsFaresUp)

            '    scorreo = (New Util.Utility).GeneraCorreoXslt(sdato, sdatodespues)
            '    'Dim snota As String = Nota(ch, f1last, f2last, rp, f1, f2, sreference, RatePlanRow)

            '    'CType(Me.Page, PaginaBase).guardalog("/Pages/Groups.aspx", PaginaBase.acciones.Modificar, snota, sreference, sdato, sdatodespues)
            '    Dim usuario As String = String.Empty
            '    flag = True

            'End If
            'idFare = 0
        End If
        'If Me.txtPromoDescription.HasChanges Then CType(Me.Page, PaginaBase).NotifyContentModification("Tarifa de la habitación " & ch & ", y plan tarifario " & Me.RatePlanRow.RATECODE, "Tarifa De Habitacíón")

        Return flag
    End Function

    Private Sub btnSaveFare_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveFare.Click
        Dim FareJuniorMin, FareChildMin, FareAdultMin As Double

        Dim sCorreo As String = String.Empty
        Dim Exceptions, idrateplan As String


        Dim diPadre As DataGridItem
        Dim f1, f2 As Date

        Dim chLast As String, rpLast As String = String.Empty, f1Last As String = String.Empty, f2Last As String = String.Empty
        Dim dgRooms As DataGrid = CType(dgLocksForGroup.Items(dgLocksForGroup.SelectedIndex).Cells(dgColumns.dgRooms).FindControl("dgRooms"), DataGrid)

        Dim idHotel, idroom, quantity As Integer
        Dim idTarifa, nombreHabitacion As String
        Dim Exists As Boolean = False

        'If Not Integer.TryParse(txtQuantity.Text, quantity) Then Return
        quantity = ddlQuantity.SelectedValue


        'If Not CtrlPlanFaresExc2.IsValidData Or Not CtrlPlanFares2.IsValidData Then
        '    lblErrorFare.Visible = True
        '    lblErrorFare.Text = PortalCulture.GetString("00083")
        '    Return
        'End If

        'Me.CtrlPlanFaresExc2.createFieldException()
        'Exceptions = Me.CtrlPlanFaresExc2.FieldException()

        'f1 = CDate(dgLocksForGroup.Items(SelectLockForGroupIDX).Cells(dgColumns.StartDate).Text)
        'f2 = CDate(dgLocksForGroup.Items(SelectLockForGroupIDX).Cells(dgColumns.EndDate).Text)

        'If (SelectRoomIDX >= 0) Then
        'chLast = dgRooms.Items(SelectRoomIDX).Cells(dgRColumns.CodigoHabitacion).Text
        'rpLast = dgLocksForGroup.Items(SelectRoomIDX).Cells(dgColumns.IdRatePlan).Text
        'f1Last = dgRooms.Items(SelectRoomIDX).Cells(dgRColumns.FechaInicia).Text
        'f2Last = dgRooms.Items(SelectRoomIDX).Cells(dgRColumns.FechaFinaliza).Text
        'rpLast = dgLocksForGroup.Items(SelectLockForGroupIDX).Cells(dgColumns.RatePlanName).Text
        'idTarifa = dgRooms.Items(SelectRoomIDX).Cells(dgRColumns.idTarifa).Text
        'End If

        Dim roomStrs() As String = ddlRoomType.Text.Split("--")
        idroom = ddlRoomType.SelectedValue 'roomStrs(0)
        'nombreHabitacion = roomStrs(1)
        idrateplan = dgLocksForGroup.Items(SelectLockForGroupIDX).Cells(dgColumns.IdRatePlan).Text
        idHotel = dgLocksForGroup.Items(SelectLockForGroupIDX).Cells(dgColumns.IdHotel).Text


        CtrlPlanFares2.TarifaMinima(FareAdultMin, FareChildMin, FareJuniorMin)

        lblErrorFare.Text = PortalCulture.GetString("01565")

        Dim bPorOcupacion As Integer = RateModeView
        'If UpdateFare(idroom, idTarifa, f1, f2, chLast, rpLast, f1Last, f2Last, nombreHabitacion, False, bPorOcupacion, FareAdultMin, FareChildMin, FareJuniorMin, sCorreo, idrateplan, Exceptions, idHotel) = True Then
        '    CtrlPlanFares2.m_iFareId = idTarifa
        '    CtrlPlanFares2.Save(rpLast, nombreHabitacion, sCorreo, Me.CtrlPlanFaresExc2.getRatesExceptions())

        For i As Integer = 0 To dgRooms.Items.Count - 1
            If ddlRoomType.SelectedValue.ToString() = dgRooms.Items(i).Cells(dgRColumns.idTipoHabitacion_Hotel).Text Then
                Exists = True
            End If
        Next

        With New RatePlanFacade
            If SelectRoomIDX = -1 Then
                If Not Exists Then
                    lblErrorFare.Visible = Not .InsertLockForGroup_Rooms(dgLocksForGroup.DataKeys(dgLocksForGroup.SelectedIndex), quantity, ddlRoomType.SelectedValue)
                Else
                    lblErrorFare.Visible = True
                    lblErrorFare.Text = "Éste grupo ya cuenta con un bloqueo con la habitación seleccionada"
                End If
            Else
                lblErrorFare.Visible = Not .UpdateLockForGroup_Rooms(dgRooms.Items(SelectRoomIDX).Cells(dgRColumns.IdLockForGroup_Rooms).Text, quantity)
            End If

            If Not lblErrorFare.Visible Then
                LoadLocks(GetSelectedIDConvenio)
                dgLocksForGroup.SelectedIndex = -1
                hidGridLocked.Value = String.Empty
            End If
            
        End With

        'Me.CtrlPlanFares2.m_iRoomId = idroom
        'Me.CtrlPlanFares2.m_iFareId = 0
        'Else
        'lblErrorFare.Visible = True
        'End If
    End Sub

    Private Function UpdateFare(ByVal idroom As Integer, ByVal f1 As Date, ByVal f2 As Date, ByRef dsBefore As DataSet, ByRef dsFareUp As FaresData, ByVal idrateplan As String _
                                , ByVal exep As String, ByRef fareid As Integer, Optional ByVal publish As Boolean = True) As Boolean
        Dim datFares As New FaresData
        Dim ExistCode As New FaresData
        Dim fareRow As DataRow
        Dim bResult As Boolean
        'Me.lblDateError.Visible = False

        dsBefore = (New FaresSystem).GetFareByFareId(fareid)
        With datFares
            fareRow = .Tables(.FARES_TABLE).NewRow()
            Try
                ' try to fill fare row data
                fareRow(.PKIDFARES_FIELD) = fareid
                fareRow(.ENDDATE_FIELD) = Format(f2, "yyyy/MM/dd")
                fareRow(.EXTRAADULTPRICE_FIELD) = ConvDouble(Me.txtExtraAdultPrice.Text)
                fareRow(.EXTRACHILDPRICE_FIELD) = ConvDouble(Me.txtExtraChildPrice.Text)
                fareRow(.EXTRATEENPRICE_FIELD) = ConvDouble(Me.txtExtraTeenPrice.Text)

                fareRow(.RATEENPRICE_FIELD) = 0
                fareRow(.NINIORATE) = 0
                fareRow(.PRICE_FIELD) = 0

                Double.TryParse(Me.txtTeenFare.Text, fareRow(.RATEENPRICE_FIELD))
                Double.TryParse(Me.txtChildFare.Text, fareRow(.NINIORATE))
                Double.TryParse(Me.txtAdultFare.Text, fareRow(.PRICE_FIELD))

                fareRow(FaresData.EXCEPTION_FIELD) = exep
                fareRow(.HOTELROOMTYPEID_FIELD) = idroom
                fareRow(.STARTDATE_FIELD) = Format(CDate(f1), "yyyy/MM/dd")
                'fareRow(.WAITLISTAVAILABLE_FIELD) = Me.chkWaitListAvailable.Checked


                fareRow(FaresData.NOARRIVOS_FIELD) = "NNNNNNN"
                fareRow(FaresData.RULESDEFAULT) = True

                fareRow(FaresData.IDRATEPLAN_FIELD) = idrateplan
                'Promociones, Ventanas, Ocupacion





                .Tables(.FARES_TABLE).Rows.Add(fareRow)
                ' set row state to modified
                fareRow.AcceptChanges()
                fareRow(.PKIDFARES_FIELD) = fareRow(.PKIDFARES_FIELD)
                With New FaresSystem
                    Try
                        bResult = .ActualizaFares(datFares)
                        dsFareUp = datFares
                        dsFareUp.AcceptChanges()
                    Catch ex As OverflowException
                        'Me.lblDateError.Visible = True
                        Return False
                    End Try
                End With
                If bResult = True Then
                    fareid = .Tables(.FARES_TABLE).Rows(0)(.PKIDFARES_FIELD)
                End If
            Catch ex As Exception
                Return False
            End Try
        End With
        Return True
    End Function

    Function ConvDouble(ByVal valor As String) As Double
        Return If(String.IsNullOrEmpty(valor), 0, Double.Parse(valor))
    End Function

    Sub Addrateplan(ByVal ds As DataSet, ByVal field As String, ByVal valor As String)
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            ds.Tables(0).Columns.Add(field)
            ds.Tables(0).Rows(0)(field) = valor
        End If
    End Sub

    Function Nota(ByVal rooom As String, ByVal f1last As String, ByVal f2last As String, ByVal rp As String, ByVal f1 As String, ByVal f2 As String, ByRef sreference As String, ByVal raterow As RowRatePlan) As String
        Dim msg As String = "Se modificó la tarifa de la habitación " & rooom & " de la fecha " & f1last & " a la fecha " & f2last & " con el rateplan " & rp & " su nueva fecha es (o sigue siendo) del " & f1 & " al " & f2 & " el rateplan es (o sigue siendo) " & raterow.RATECODE
        Dim drhotel As DataRow = CType(Me.Page, PaginaBase).HotelInfo
        Dim idioma As String

        sreference = "Cambio de tarifa"
        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))
        If idioma = "en-US" Then
            sreference = "Update Rate"
            msg = "It changed the room rate " & rooom & " from " & f1last & " to " & f2last & " with rateplan " & rp & " the new date is (or remains) of " & f1 & " to " & f2 & " , with rateplan " & raterow.RATECODE
        End If
        Return msg
    End Function

    Public Function SaveNewFare(ByVal idRoom As Integer, ByRef idtar As Integer, ByVal f1 As Date, ByVal f2 As Date, ByVal descr As String, ByRef sdato As String, ByVal EXEP As String, ByVal IDRATEPLAN As String) As Boolean
        Dim datFare As New FaresData
        Dim ExistCode As New FaresData
        Dim rowFare As DataRow
        Dim adultos, ninios As Integer
        'Dim sdatodespues As String
        ' Dim dv As DataView




        'buscar el codigo que le pertenece
        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getRoomByID(idRoom)
        End With
        If room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows.Count = 0 Then Return False
        adultos = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL)(0)(RoomsHotelData.FLD_NUMBER_MAXADULTS)
        ninios = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL)(0)(RoomsHotelData.FLD_NUMBER_MAXCHILDREN)
        Try
            With datFare.Tables(FaresData.FARES_TABLE)
                rowFare = .NewRow()
                rowFare(FaresData.ENDDATE_FIELD) = f2
                rowFare(FaresData.EXTRAADULTPRICE_FIELD) = ConvDouble(txtExtraAdultPrice.Text)
                rowFare(FaresData.EXTRACHILDPRICE_FIELD) = ConvDouble(txtExtraChildPrice.Text)
                rowFare(FaresData.EXTRATEENPRICE_FIELD) = ConvDouble(txtExtraTeenPrice.Text)

                rowFare(FaresData.NINIORATE) = 0
                rowFare(FaresData.RATEENPRICE_FIELD) = 0
                rowFare(FaresData.PRICE_FIELD) = 0

                Double.TryParse(Me.txtChildFare.Text, rowFare(FaresData.NINIORATE))
                Double.TryParse(txtTeenFare.Text, rowFare(FaresData.RATEENPRICE_FIELD))
                Double.TryParse(Me.txtAdultFare.Text, rowFare(FaresData.PRICE_FIELD))

                rowFare(FaresData.STARTDATE_FIELD) = f1
                rowFare(FaresData.EXCEPTION_FIELD) = EXEP

                rowFare(FaresData.RULESDEFAULT) = False
                rowFare(FaresData.NOARRIVOS_FIELD) = "NNNNNNN"
                rowFare(FaresData.HOTELROOMTYPEID_FIELD) = idRoom
                rowFare(FaresData.RATETYPE_FIELD) = "O"
                rowFare(FaresData.IDRATEPLAN_FIELD) = IDRATEPLAN
                rowFare(FaresData.RATECODE_FIELD) = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ROOM_CODE) & IDRATEPLAN

                'rowFare(FaresData.IDDICCDESCPROM_FIELD) = Me.txtPromoDescription.Insert()


                .Rows.Add(rowFare)
            End With

            With New FaresSystem
                Try
                    If .InsertFares(datFare) Then
                        idtar = datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.PKIDFARES_FIELD)
                        AddRatePlan(datFare, "Descr_rateplan", descr)
                        sdato = Util.Utility.GetXml(FaresData.FARES_TABLE, "UpdateRate", datFare)
                        'esta condición es para cuando se autollenaran las tarifasrestricciones
                        If adultos > 0 Then
                            If Not SaveFaresRestrictions(idtar, adultos, ninios) Then Return False
                        End If
                    End If
                Catch ex As Exception
                    '    Me.lblDateError.Visible = True

                    'Catch ex As OverflowException
                    'Me.lblDateError.Visible = True
                    Return False
                End Try
            End With
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Private Function SaveFaresRestrictions(ByVal idTarifa As Integer, ByVal adultos As Integer, ByVal ninos As Integer) As Boolean
        Dim datRestrictions As New FaresRestrictionsData
        For idxAdults As Integer = 1 To adultos
            For idxChild As Integer = 0 To ninos
                'Combinaciond de adultos - niños
                Dim newRow As DataRow = datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).NewRow()
                With newRow
                    .Item(FaresRestrictionsData.ADULTFARE_FIELD) = 0
                    .Item(FaresRestrictionsData.CHILDFARE_FIELD) = 0
                    Double.TryParse(Me.txtAdultFare.Text, .Item(FaresRestrictionsData.ADULTFARE_FIELD))
                    Double.TryParse(Me.txtChildFare.Text, .Item(FaresRestrictionsData.CHILDFARE_FIELD))

                    .Item(FaresRestrictionsData.ADULTNUMBER_FIELD) = idxAdults
                    .Item(FaresRestrictionsData.CHILDNUMBER_FIELD) = idxChild
                    .Item(FaresRestrictionsData.IDFARE_FIELD) = idTarifa
                    .Item(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = 0
                End With
                datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
            Next
        Next

        With New FaresRestrictionSystem
            SaveFaresRestrictions = .InsertFaresRestrictions(datRestrictions)
        End With
    End Function

    Private Sub Room_RefillRestrictions(ByVal fareid As Integer)
        If (SelectRoomIDX >= 0) Then
            Me.CtrlPlanFares2.m_iFareId = fareid
        Else
            Me.CtrlPlanFares2.m_iFareId = 0
        End If
        Me.CtrlPlanFares2.m_iRoomId = ddlRoomType.SelectedValue.Split("|")(0)

        Me.CtrlPlanFares2.ReFill()
        Me.CtrlPlanFaresExc2.ReFill()

        Dim enableExt As Boolean = Integer.Parse(ddlRoomType.SelectedValue.Split("|")(2)) > 0
        txtExtraAdultPrice.Enabled = enableExt
        reqExtraAdultPrice.Enabled = enableExt
        txtExtraChildPrice.Enabled = enableExt
        reqExtraChildPrice.Enabled = enableExt
        txtExtraTeenPrice.Enabled = enableExt
        reqExtraTeenPrice.Enabled = enableExt

        If Not enableExt Then
            txtExtraAdultPrice.Text = String.Empty
            txtExtraChildPrice.Text = String.Empty
            txtExtraTeenPrice.Text = String.Empty
        End If
    End Sub

    Public Sub LoadFare(ByVal iFareId As Integer)

        Dim datFares As DataSet
        Dim rowFare As DataRow

        'm_iFareId = iFareId
        ' get fare information
        With New FaresSystem
            datFares = .GetFareByFareId(iFareId)
        End With

        If Not datFares Is Nothing AndAlso datFares.Tables(FaresData.FARES_TABLE).Rows.Count > 0 Then

            rowFare = datFares.Tables(FaresData.FARES_TABLE).Rows(0)

            Me.txtAdultFare.Text = CDbl(rowFare(FaresData.PRICE_FIELD))
            Me.txtChildFare.Text = CDbl(Val(rowFare(FaresData.NINIORATE).ToString))

            If txtExtraAdultPrice.Enabled Then
                Me.txtExtraAdultPrice.Text = CDbl(Val(rowFare(FaresData.EXTRAADULTPRICE_FIELD)))
            End If
            If txtExtraChildPrice.Enabled Then
                Me.txtExtraChildPrice.Text = CDbl(Val(rowFare(FaresData.EXTRACHILDPRICE_FIELD)))
            End If
            If txtExtraTeenPrice.Enabled Then
                txtExtraTeenPrice.Text = CDbl(IIf(rowFare(FaresData.EXTRATEENPRICE_FIELD) Is DBNull.Value, _
                                                  0, rowFare(FaresData.EXTRATEENPRICE_FIELD)))
            End If

            txtTeenFare.Text = CDbl(IIf(rowFare(FaresData.RATEENPRICE_FIELD) Is DBNull.Value, _
                                             0, rowFare(FaresData.RATEENPRICE_FIELD)))

            CtrlPlanFaresExc2.FieldException = rowFare(FaresData.EXCEPTION_FIELD)
        End If
    End Sub

    Public Function GetFareFor(ByVal target As String, ByVal isNetRate As Boolean) As Double
        Dim input As TextBox = Me.FindControl("txt" + target + "Fare" + If(isNetRate, "NR", String.Empty))
        Dim value As Double = 0
        If input IsNot Nothing Then Double.TryParse(input.Text, value)
        Return value
    End Function

    Private Sub btnCancelFare_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelFare.Click
        ResetFormLock(False)
        hidGridLocked.Value = String.Empty
        If Not SelectLockForGroupIDX = -1 Then
            Dim oDG As Object = dgLocksForGroup.Items(SelectLockForGroupIDX).FindControl("dgRooms")
            If Not oDG Is Nothing Then
                CType(oDG, DataGrid).SelectedIndex = -1
            End If
        End If
    End Sub

    'Protected Sub btnDeleteGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteGroup2.Click
    '    With New RatePlanFacade
    '        If .LogicDeleteGroupsById(GetSelectedIDConvenio) Then ResetForm()
    '    End With
    'End Sub

    'Protected Sub btnEditGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditGroup.Click
    '    'txtCode.Text=lblCode.Text
    '    'txtDescription.Text=lblDescripcion.Text
    '    'txtName.Text=lblName.Text
    '    tabEditGroup.Visible = True
    'End Sub

    Private Sub btnCancelGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelGroup.Click
        tabEditGroup.Visible = False
    End Sub


    Private Sub dgConvenio_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgConvenio.ItemCommand
        hidGridLocked.Value = String.Empty
        If e.CommandName = "Delete" Then
            With New RatePlanFacade
                If .LogicDeleteGroupsById(dgConvenio.DataKeys(e.Item.ItemIndex)) Then ResetForm()
            End With
        ElseIf e.CommandName = "Edit" Then
            tabEditGroup.Visible = True
            dgConvenio.SelectedIndex = e.Item.ItemIndex
            LoadGroup(GetSelectedIDConvenio)
            btnCancelGroup.Visible = True
            txtCode.Enabled = False
        ElseIf e.CommandName = "Select" Then
            dgConvenio.SelectedIndex = e.Item.ItemIndex
            LoadGroup(GetSelectedIDConvenio)

            tabEditGroup.Visible = False
            divBlock.Visible = True
            'hdnIdAgreement.Value = ddlAgreementWorking.SelectedValue
            'txtNoAgreement.Enabled = False
            ResetFormLock(False)
            LoadLocks(GetSelectedIDConvenio)
            'btnEditGroup.Visible = True
            'btnDeleteGroup.Visible = True
        End If

    End Sub

    Private Sub btnNewGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewGroup.Click
        ResetForm()
        btnCancelGroup.Visible = False
        txtCode.Enabled = True

        tabEditGroup.Visible = True
        dgConvenio.SelectedIndex = -1

    End Sub

    Private Sub dgConvenio_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgConvenio.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then

            Dim LK As HyperLink
            Dim LK2 As LinkButton

            LK2 = e.Item.Cells(dgConColumns.Delete).FindControl("lnkEliminar2")
            LK = e.Item.Cells(dgConColumns.Delete).FindControl("lnkEliminar")
            LK.Text = PortalCulture.GetString("00103")
            'LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), _
            'PortalCulture.GetString("00613") & ", " & PortalCulture.GetString("00464"))

            LK.NavigateUrl = CtlMensajes3.getShow(LK2.ClientID, PortalCulture.GetString("01557"), PortalCulture.GetString("01570"))

            CType(e.Item.Cells(dgConColumns.Edit).FindControl("lnkEdit"), LinkButton).Text = PortalCulture.GetString("00093")
        End If

        If e.Item.ItemType = ListItemType.Header Then
            'e.Item.Cells(dgColumns.dgRooms).Text = PortalCulture.GetString("A00041")
            '        e.Item.Cells(dgcolumns.orden).Text = PortalCulture.GetString("00920")
        ElseIf e.Item.ItemType = ListItemType.Footer Then

        End If
    End Sub

    Private Sub dgConvenio_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgConvenio.PageIndexChanged
        dgConvenio.CurrentPageIndex = e.NewPageIndex
        dgConvenio.SelectedIndex = -1
        LoadGroup_Grid(idCorporate, ctrlAutoComplete1.GetFilter)
    End Sub

    Private Sub dgLocksForGroup_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgLocksForGroup.PageIndexChanged
        dgLocksForGroup.CurrentPageIndex = e.NewPageIndex
        dgLocksForGroup.SelectedIndex = -1
        LoadLocks(GetSelectedIDConvenio)
    End Sub

    Private Sub ctrlAutoComplete1_onSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        dgConvenio.CurrentPageIndex = 0
        'LoadGroup_Grid(idCorporate, ctrlAutoComplete1.GetFilter)
        ResetForm()
        dgConvenio.SelectedIndex = -1
    End Sub

    Private Sub dgCorporates_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgCorporates.PageIndexChanged
        dgCorporates.CurrentPageIndex = e.NewPageIndex
        LoadCorporates()
    End Sub

    Private Sub loadrateplans(ByVal sFiltro As String)
        Dim ds As RatePlanData
        Dim idAsoc As Integer = GetIdAsociation()
        Dim dv As DataView

        With New RatePlanFacade
            If MyBase.IsSupervisor Or MyBase.IsUsuarioHotelAssociation Then
                'Mostramos todos los rateplans incluidos los de tarifas netas.
                ds = .GetRatePlanByIdHotel(ddlHotels.SelectedValue, PortalCulture.GetIDCulture, 1, 1, idAsociacion:=idAsoc)
            Else
                'Mostramos solamente los ratesplans que no sean de tarifas netas.
                ds = .GetRatePlanByIdHotel(ddlHotels.SelectedValue, PortalCulture.GetIDCulture, 1, 0, idAsociacion:=idAsoc)
            End If
        End With

        For Each row As DataRow In ds.Tables(0).Rows
            row.Item("Name") = row.Item("idRatePlan") & " -- " & row.Item("Name")
        Next

        dv = ds.Tables(0).DefaultView
        dv.RowFilter = "Segment = 'O'"
        ddlRatePlan.DataSource = dv
        ddlRatePlan.DataTextField = "Name"
        ddlRatePlan.DataValueField = "idRatePlan"
        ddlRatePlan.DataBind()
    End Sub

    Private Sub ddlHotels_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlHotels.SelectedIndexChanged
        loadrateplans("")
    End Sub
    Private Sub ddlRoomType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlRoomType.SelectedIndexChanged
        LoadRoomsQuantity(CType(ddlRoomType.SelectedValue, Integer))
    End Sub
End Class
