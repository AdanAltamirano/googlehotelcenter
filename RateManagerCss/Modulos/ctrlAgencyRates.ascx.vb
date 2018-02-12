Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Partial Class ctrlAgencyRates
    Inherits UserControlBase

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox2 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox3 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox4 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents SearchAgency1 As SearchAgency
    Protected WithEvents ctrPortal1 As ctrPortal

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub
#End Region

    Protected WithEvents txtDescripcion As CtrlIdioma
    Protected WithEvents txtShortDescription As CtrlIdioma
    Const KEY_HOTELID = "HotelId"
    Public descripcionError = String.Empty

#Region "Metodos"

    Protected Property HasData() As Boolean
        Get
            HasData = False
            If Me.ViewState("HasData") IsNot Nothing Then HasData = Me.ViewState("HasData")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("HasData") = value
        End Set
    End Property


    Private Sub loadRules()
        Dim rules As RatesPlanRulesData

        With New RatesPlanRulesFacade
            rules = .getList(Me.m_iHotelId)
        End With

        Me.ddlRules.DataSource = rules.Tables(rules.TABLE_RATEPLANRULES)
        ddlRules.DataTextField = RatesPlanRulesData.FIELD_DESCRIPTION
        ddlRules.DataValueField = RatesPlanRulesData.FIELD_IDRULE
        ddlRules.DataBind()
        ddlRules.Items.Insert(0, PortalCulture.GetString("00027"))
        ddlRules.Items(0).Value = 0
    End Sub

    Public Sub ClearData()
        validarScript()

        Me.HasData = False

        Me.txtRateCode.Text = String.Empty
        Me.txtDescProm.Text = ""
        Me.txtShortDescription.Limpia()
        Me.txtPromoDescription.Limpia()
        txtDescripcion.Limpia()
        edicion = False
        Me.lstRatePlans.SelectedIndex = 0

        If ddlRules.Items.Count <> 0 Then
            ddlRules.SelectedIndex = 0
        End If

        Me.chkGDS.Checked = True
        ' Me.txtPorcGDS.Enabled = True
        Session("trGDS") = True

        Me.chkUnipantalla.Checked = False
        Me.chkADS.Checked = False
        'Me.txtPorcUNI.Enabled = False

        Me.chkPortal.Checked = False
        ' Me.txtPorcPOR.Enabled = False

        Me.chkGDSAmadeus.Checked = False
        Me.chkGDSGalileo.Checked = False
        Me.chkGDSabre.Checked = False
        Me.chkGDSWorldSpan.Checked = False

        Me.txtPorcGDS.Text = "0"
        Me.txtPorcUNI.Text = "0"
        Me.txtPorcPOR.Text = "0"
        Me.txtPorcADS.Text = "0"

        'Me.txtPorcUNI.Text = String.Empty
        'Me.txtPorcPOR.Text = String.Empty

        Me.chkTodasAgencias.Checked = True
        Me.txtAgencia.Text = String.Empty

        listadoDT = Nothing
        dgAgencias.DataSource = Nothing
        dgAgencias.DataBind()

        'Ocultamos los reglones del iata y de agencias
        'renglonBuscarPor.Style("display") = "none"
        'RenglonIATA.Style("display") = "none"
        'RenglonAgencia.Style("display") = "none"

        cargarDatosHotel()
        FillRatePlans()

        ctrPortal1.Limpiar()
    End Sub

    Private Sub FillRatePlans()

        Dim dataPlans As RatePlanData
        Dim dataLinks As LinkRatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            dataPlans = .GetRatePlanByIdHotel(ID:=Me.m_iHotelId, idioma:=PortalCulture.GetIDCulture, incluirNetRatesPlan:=1, IncluirPaquetesSegmentoK:=1, idAsociacion:=idAsoc)
        End With

        If dataPlans IsNot Nothing AndAlso dataPlans.Tables.Contains(dataPlans.RATEPLAN_TABLE) Then

            With dataPlans.Tables(dataPlans.RATEPLAN_TABLE)
                'Establecemos el id del plan como llave primaria
                Dim keysPlan As New List(Of DataColumn)
                keysPlan.Add(.Columns(dataPlans.FIELD_IDRATEPLAN))
                .PrimaryKey = keysPlan.ToArray()

                'Se obtiene un listado de los planes vinculados
                With New LinkRatePlanFacade
                    dataLinks = .getList(Me.m_iHotelId, PortalCulture.GetIDCulture)
                End With

                'Se eliminan los planes vinculados a otros.
                If dataLinks IsNot Nothing AndAlso dataLinks.Tables.Contains(dataLinks.TABLE_LINKRATEPLAN) Then
                    For Each link As DataRow In dataLinks.Tables(dataLinks.TABLE_LINKRATEPLAN).Rows
                        If .Rows.Contains(link(dataLinks.FIELD_TargetRatePlan)) Then .Rows.Find(link(dataLinks.FIELD_TargetRatePlan)).Delete()
                    Next
                    'Se aceptan los cambios, para que se vean reflejadas las bajas
                    .AcceptChanges()
                End If

                'Llenamos el combo
                Me.lstRatePlans.Items.Clear()

                Me.lstRatePlans.Items.Add(PortalCulture.GetString("M000272"))
                For Each plan As DataRow In .Rows
                    If plan(dataPlans.FIELD_SEGMENT).ToString().ToUpper() = "I" Then
                        Dim value As New JObject( _
                            New JProperty("id", plan(dataPlans.FIELD_IDRATEPLAN)), _
                            New JProperty("code", plan(dataPlans.FIELD_CODIGOTARIFA)), _
                            New JProperty("segment", plan(dataPlans.FIELD_SEGMENT)), _
                            New JProperty("name", plan(dataPlans.FIELD_NAME)), _
                            New JProperty("contract", If(plan.IsNull(dataPlans.FIELD_IDCONTRATO), 0, plan(dataPlans.FIELD_IDCONTRATO))) _
                        )
                        Me.lstRatePlans.Items.Add(New ListItem(plan(dataPlans.FIELD_NAME), value.ToString()))
                    End If
                Next

            End With

        End If

    End Sub

    Private Sub CargaContratos()
        Dim Contratos As ContractNetRateData
        ddlContratosNR.Items.Clear()
        With (New ContractNetRateFacade)
            Contratos = .LoadContractsByIdHotel(m_iHotelId)
        End With
        If Not Contratos Is Nothing AndAlso Contratos.Tables(Contratos.CONTRACTNR_TABLE).Rows.Count > 0 Then
            ddlContratosNR.DataSource = Contratos.Tables(Contratos.CONTRACTNR_TABLE)
            ddlContratosNR.DataTextField = Contratos.FIELD_NOMBRE
            ddlContratosNR.DataValueField = Contratos.FIELD_IDCONTRACTO
            ddlContratosNR.DataBind()
        Else
            'No se encontraron contratos
        End If
        ddlContratosNR.Items.Insert(0, PortalCulture.GetString("M000482"))
        ddlContratosNR.Items(0).Value = 0
    End Sub

    Dim dsRatePlan As RatePlanData
    Dim dato As String = String.Empty

    Function getDataXML() As String
        Dim ds As RatePlanData

        With New RatePlanFacade
            ds = .GetDataRatePlan(IdRatePlan, Me.m_iHotelId)
        End With
        Return Util.Utility.GetXml(ds.RATEPLAN_TABLE, "UpdateRatePlanAgency", ds)
    End Function

    Public Function SavePlan(ByVal publish As Boolean) As Integer
        Dim sData As String = ""
        Dim sDataPrev As String = ""

        Try
            If Not IsValidAgencyRates() Then
                validarScript()

                Return -1
            End If

            Dim dsRate As New RatePlanData
            Dim Rp As RatePlanData
            Dim rRate As DataRow
            Dim val As Boolean
            Dim idRate, NuevoIdRate As String
            Dim idHotel As Integer, dvRp As DataView
            Dim agenciaAplicada As String = String.Empty

            sDataPrev = getDataXML()
            rRate = dsRate.Tables(dsRate.RATEPLAN_TABLE).NewRow()

            If chkGDS.Checked = False Then
                agenciaAplicada = "NNNN"
            Else
                If chkGDSAmadeus.Checked = True Then
                    agenciaAplicada = agenciaAplicada + "Y"
                Else
                    agenciaAplicada = agenciaAplicada + "N"
                End If

                If chkGDSGalileo.Checked = True Then
                    agenciaAplicada = agenciaAplicada + "Y"
                Else
                    agenciaAplicada = agenciaAplicada + "N"
                End If

                If chkGDSabre.Checked = True Then
                    agenciaAplicada = agenciaAplicada + "Y"
                Else
                    agenciaAplicada = agenciaAplicada + "N"
                End If

                If chkGDSWorldSpan.Checked = True Then
                    agenciaAplicada = agenciaAplicada + "Y"
                Else
                    agenciaAplicada = agenciaAplicada + "N"
                End If
            End If

            With rRate
                If Me.edicion = False Then
                    .Item(dsRate.FIELD_IDRATEPLAN) = txtRateCode.Text.ToUpper
                Else
                    .Item(dsRate.FIELD_IDRATEPLAN) = Session("idRP")
                End If

                .Item(dsRate.FIELD_DESCRIPTION) = Me.txtDescripcion.textodefault
                .Item(dsRate.FIELD_IDHOTEL) = Me.m_iHotelId
                .Item(dsRate.FIELD_SEGMENT) = "I"

                If ddlRules.SelectedValue <> 0 Then
                    .Item(dsRate.FIELD_IDRULE) = ddlRules.SelectedValue
                Else
                    .Item(dsRate.FIELD_IDRULE) = System.DBNull.Value
                End If

                .Item(dsRate.FIELD_NAME) = Me.txtShortDescription.textodefault
                .Item(dsRate.FIELD_CODIGOTARIFA) = Me.txtRateCode.Text.ToUpper
                .Item(dsRate.FIELD_GDS) = chkGDS.Checked
                .Item(dsRate.FIELD_PORTAL) = chkPortal.Checked
                .Item(dsRate.FIELD_UNIPANTALLA) = chkUnipantalla.Checked
                .Item(dsRate.FIELD_ADS) = chkADS.Checked
                .Item(dsRate.FIELD_GDSAPPLY) = agenciaAplicada
                .Item(dsRate.FIELD_IDDICCPROMODESC) = Me.txtPromoDescription.IdIndice

                If (Me.txtDescProm.Text <> "" AndAlso (chkPortal.Checked Or Me.chkUnipantalla.Checked Or Me.chkGDS.Checked)) Then
                    .Item(dsRate.FIELD_DESCPROMOTION) = CDbl(txtDescProm.Text)
                End If

                If ddlContratosNR.SelectedValue <> 0 Then
                    .Item(dsRate.FIELD_IDCONTRATO) = ddlContratosNR.SelectedValue
                Else
                    .Item(dsRate.FIELD_IDCONTRATO) = System.DBNull.Value
                End If


                'If m_orden > 0 Then
                '    .Item(dsRate.FIELD_ORDEN) = m_orden
                'End If
            End With

            dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows.Add(rRate)

            Dim dsAgencyRates As New AgencyRatesData
            Dim dr As DataRow

            dr = dsAgencyRates.Tables(dsAgencyRates.AgencyRates_TABLE).NewRow
            dr(dsAgencyRates.FIELD_idHotel) = CDbl(Me.m_iHotelId)
            dr(dsAgencyRates.FIELD_rateCode) = txtRateCode.Text.ToUpper
            dr(dsAgencyRates.FIELD_GDSApply) = agenciaAplicada
            dr(dsAgencyRates.FIELD_porcUNI) = txtPorcUNI.Text
            dr(dsAgencyRates.FIELD_porcPOR) = txtPorcPOR.Text
            dr(dsAgencyRates.FIELD_porcGDS) = txtPorcGDS.Text
            dr(dsAgencyRates.FIELD_porcADS) = txtPorcADS.Text

            dsAgencyRates.Tables(dsAgencyRates.AgencyRates_TABLE).Rows.Add(dr)

            If Me.edicion = False Then
                With New RatePlanAccess
                    Dim idPromoDescription As Integer = Me.txtPromoDescription.IdIndice
                    If .InsertRtPlan(dsRate, Me.idDicc, Me.idShortDesc, idPromoDescription) Then
                        ctrPortal1.InsertarPortales(Me.m_iHotelId, 1, txtRateCode.Text, chkPortal.Checked)
                        sData = Util.Utility.GetXml(dsRate.RATEPLAN_TABLE, "UpdateRatePlanAgency", dsRate)
                        CType(Me.Page, PaginaBase).guardalog("/Pages/agencyRates.aspx", PaginaBase.acciones.Crear, "Creó el rateplan para agencia con el id " & rRate(dsRate.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "", sDataPrev, sData)
                        If publish Then
                            CType(Me.Page, PaginaBase).guardalog("/Pages/agencyRates.aspx", PaginaBase.acciones.Publicar, "Modifico la tarifa para agencia del id " & rRate(dsRate.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "", sDataPrev, sData)
                        Else
                            CType(Me.Page, PaginaBase).NotifyContentModification("Plan tarifario para agencia con el codigo " & rRate(RatePlanData.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "Plan Tarifario De Agencia")
                        End If
                        Me.txtDescripcion.Update(dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICDESC), publish)
                        Me.txtShortDescription.Update(dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICSHORTDESC), publish)
                        Me.txtPromoDescription.Update(idPromoDescription, publish)

                        Dim idAgenciasRate As Integer
                        With New AgencyRatesDataAccess
                            .InsertAgencyRates(dsAgencyRates)

                            Dim dsAR As AgencyRatesData
                            With New AgencyRatesDataAccess
                                dsAR = .LoadAgencyRatesBy_idHotel_rateCode(Me.m_iHotelId, txtRateCode.Text)

                                If Not dsAR Is Nothing AndAlso dsAR.Tables(dsAR.AgencyRates_TABLE).Rows.Count > 0 Then
                                    With dsAR.Tables(dsAR.AgencyRates_TABLE).Rows(0)
                                        idAgenciasRate = .Item(dsAR.FIELD_idAgenciasRate)
                                    End With
                                End If
                            End With
                        End With

                        Try
                            If Me.lstRatePlans.SelectedIndex > 0 Then
                                Dim value As JObject = JsonConvert.DeserializeObject(Of JObject)(Me.lstRatePlans.SelectedValue)
                                'If Me.ddlContratosNR.SelectedValue = value("contract").Value(Of Integer)() Then
                                If (Me.ddlContratosNR.SelectedValue > 0) = (value("contract").Value(Of Integer)() > 0) Then
                                    .CloneRates(Me.m_iHotelId, value("id").Value(Of String)(), Me.txtRateCode.Text.ToUpper)
                                    CType(Me.Page, PaginaBase).guardalog("/Pages/agencyRates.aspx", PaginaBase.acciones.Crear, "Clonó las tarifas del plan tarifario " & value("id").Value(Of String)() & " al plan recién creado " & Me.txtRateCode.Text.ToUpper & ".", "", "", "")
                                End If
                            End If
                        Catch ex As Exception
                        End Try

                        SaveAgencyRatesAvail(idAgenciasRate)
                        ClearData()
                        SearchAgency1.Limpiar()
                        Return 0
                    Else
                        validarScript()
                        descripcionError = PortalCulture.GetString("00781")
                    End If
                End With
            Else
                With New RatePlanAccess

                    If Session("idDD") <> 0 Then
                        Me.txtDescripcion.Update(Session("idDD"), publish)
                    Else
                        Session("idDD") = Me.txtDescripcion.Insert()
                    End If

                    If Session("idDSD") <> 0 Then
                        Me.txtShortDescription.Update(Session("idDSD"), publish)
                    Else
                        Session("idDSD") = txtShortDescription.Insert()
                    End If

                    If dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICCPROMODESC) <> 0 Then
                        Me.txtPromoDescription.Update(dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICCPROMODESC), publish)
                    Else
                        dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICCPROMODESC) = Me.txtPromoDescription.Insert()
                    End If

                    dsRate.Tables(dsRate.RATEPLAN_TABLE).AcceptChanges()
                    dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDHOTEL) = dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDHOTEL)
                    dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDDICDESC) = Session("idDD")
                    dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_SegmentRacPrinc) = Session("segmentRacPrinc")
                    dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDDICSHORTDESC) = Session("idDSD")

                    If .UpdateRtPlan(dsRate) Then
                        sData = Util.Utility.GetXml(dsRate.RATEPLAN_TABLE, "UpdateRatePlanAgency", dsRate)
                        CType(Me.Page, PaginaBase).guardalog("/Pages/agencyRates.aspx", If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar), "Modifico la tarifa para agencia del id " & rRate(dsRate.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "", sDataPrev, sData)

                        If Me.txtDescripcion.HasChanges OrElse Me.txtPromoDescription.HasChanges OrElse Me.txtShortDescription.HasChanges Then
                            CType(Me.Page, PaginaBase).NotifyContentModification("Plan tarifario para agencia con el codigo " & rRate(RatePlanData.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "Plan Tarifario De Agencia")
                        End If

                        With New AgencyRatesDataAccess
                            dsAgencyRates.Tables(dsAgencyRates.AgencyRates_TABLE).AcceptChanges()
                            dsAgencyRates.Tables(dsAgencyRates.AgencyRates_TABLE).Rows(0).Item(dsAgencyRates.FIELD_idHotel) = dsAgencyRates.Tables(dsAgencyRates.AgencyRates_TABLE).Rows(0).Item(dsAgencyRates.FIELD_idHotel)

                            dr(dsAgencyRates.FIELD_idAgenciasRate) = Session("idAgenciasRate")
                            .UpdateAgencyRates(dsAgencyRates, Session("idAgenciasRate"))

                            ctrPortal1.ModificarPortales(chkPortal.Checked)
                        End With
                        SaveAgencyRatesAvail(Session("idAgenciasRate"))
                        ClearData()
                        SearchAgency1.Limpiar()
                        Return 0
                    End If
                End With
            End If

        Catch ex As Exception
            Dim mensaje As String = ex.Message
        End Try

        Return 2
    End Function

    Private Sub SaveAgencyRatesAvail(ByVal idAgenciasRate As Integer)
        Dim dsAgencyRatesAvail As New AgencyRatesAvailData
        Dim dr As DataRow

        If Not (listadoDT Is Nothing) Then
            Dim dt As DataTable = listadoDT
            Dim i As Integer

            dr = Nothing
            For i = 0 To dt.Rows.Count - 1
                dr = dsAgencyRatesAvail.Tables(dsAgencyRatesAvail.AgencyRatesAvail_TABLE).NewRow
                dr(dsAgencyRatesAvail.FIELD_idAgenciasRate) = idAgenciasRate.ToString
                If dt.Rows(i).Item("isTourOperador") = "0" Then
                    dr(dsAgencyRatesAvail.FIELD_IATA) = dt.Rows(i).Item("IATA")
                Else
                    dr(dsAgencyRatesAvail.FIELD_idAgencia) = dt.Rows(i).Item("idAgencia")
                End If
                dsAgencyRatesAvail.Tables(dsAgencyRatesAvail.AgencyRatesAvail_TABLE).Rows.Add(dr)
            Next
        End If

        If edicion = False Then
            With New AgencyRatesAvailDataAccess
                .InsertAgencyRatesAvail(dsAgencyRatesAvail)
            End With
        Else
            With New AgencyRatesAvailDataAccess
                .DeleteAgencyRatesAvail(idAgenciasRate)
                .InsertAgencyRatesAvail(dsAgencyRatesAvail)
            End With
        End If
    End Sub

    Private Function IsValidAgencyRates() As Boolean
        Dim comision As Integer

        descripcionError = String.Empty

        If txtRateCode.Text.Trim = String.Empty Then
            descripcionError = PortalCulture.GetString("00769")

            Return False
        End If

        If txtShortDescription.textodefault.Trim = String.Empty Then
            descripcionError = PortalCulture.GetString("00770")

            Return False
        End If

        If txtDescripcion.textodefault.Trim = String.Empty Then
            descripcionError = PortalCulture.GetString("00771")

            Return False
        End If

        If ddlRules.Items.Count < 0 Then
            descripcionError = PortalCulture.GetString("00772")

            Return False
        End If

        If chkGDS.Checked = True Then
            If (chkGDSAmadeus.Checked = False And chkGDSGalileo.Checked = False And chkGDSabre.Checked = False And chkGDSWorldSpan.Checked = False) Then
                descripcionError = PortalCulture.GetString("00773")

                Return False
            End If

            espacios(txtPorcGDS)
            If isNumero(txtPorcGDS.Text) = False Then
                descripcionError = PortalCulture.GetString("00774")

                Return False
            End If
        Else
            txtPorcGDS.Text = "0"
        End If

        If chkUnipantalla.Checked = True Then
            If txtPorcUNI.Text.Trim = String.Empty Then
                descripcionError = PortalCulture.GetString("00775")
                txtPorcUNI.Text = "0"

                Return False
            End If

            espacios(txtPorcUNI)
            If isNumero(txtPorcUNI.Text) = False Then
                descripcionError = PortalCulture.GetString("00776")

                Return False
            End If
        Else
            txtPorcUNI.Text = "0"
        End If

        If chkPortal.Checked = True Then
            If (ctrPortal1.validaSeleccionPortal(Me.chkPortal.Checked)) = False Then
                descripcionError = PortalCulture.GetString("01012")
                Return False
            End If

            If txtPorcPOR.Text.Trim = String.Empty Then
                descripcionError = PortalCulture.GetString("00777")
                txtPorcPOR.Text = "0"

                Return False
            End If

            espacios(txtPorcPOR)
            If isNumero(txtPorcPOR.Text) = False Then
                descripcionError = PortalCulture.GetString("00778")

                Return False
            End If
        Else
            txtPorcPOR.Text = "0"
        End If

        If chkADS.Checked = True Then
            If txtPorcADS.Text.Trim = String.Empty Then
                descripcionError = PortalCulture.GetString("00819")
                txtPorcADS.Text = "0"

                Return False
            End If

            espacios(txtPorcADS)
            If isNumero(txtPorcADS.Text) = False Then
                descripcionError = PortalCulture.GetString("00820")

                Return False
            End If
        Else
            txtPorcADS.Text = "0"
        End If

        If chkTodasAgencias.Checked = False Then
            If listadoDT Is Nothing Then
                descripcionError = PortalCulture.GetString("00779")

                Return False
            End If
        End If

        Return True
    End Function

    Public Sub espacios(ByRef texto As TextBox)
        Try
            Dim cadena As String = texto.Text.Trim
            Dim cadenaSinEspacios As String = String.Empty
            Dim i As Integer

            For i = 0 To cadena.Length - 1
                If cadena.Substring(i, 1) <> " " Then
                    cadenaSinEspacios = cadenaSinEspacios + cadena.Substring(i, 1)
                End If
            Next

            texto.Text = cadenaSinEspacios
        Catch
        End Try
    End Sub

    Public Sub validarScript()
        Me.Page.RegisterStartupScript("FillChkGDS", "<script>" & String.Format("javascript:onCheckBoxesClick('{0}', '{1}', '{2}');", chkGDS.ClientID, trGDSApply.ClientID, trPorcGDS.ClientID) & "</script>")
        Me.Page.RegisterStartupScript("FillChkPortal", "<script>" & String.Format("javascript:onCheckBox('{0}', '{1}');", chkPortal.ClientID, trPorcPortal.ClientID) & "</script>")
        Me.Page.RegisterStartupScript("FillChkUniPantalla", "<script>" & String.Format("javascript:onCheckBox('{0}', '{1}');", chkUnipantalla.ClientID, trPorcUni.ClientID) & "</script>")
        Me.Page.RegisterStartupScript("FillChkADS", "<script>" & String.Format("javascript:onCheckBox('{0}', '{1}');", chkADS.ClientID, trPorcADS.ClientID) & "</script>")

        Me.Page.RegisterStartupScript("FillChkAgencias", "<script>" & String.Format("javascript:onCheckBoxAgencias('{0}', '{1}', '{2}');", chkTodasAgencias.ClientID, TrAgencias.ClientID, txtAgencia.ClientID) & "</script>")

    End Sub
#End Region

#Region "Propiedades"
    Public Shared Function isNumero(ByVal objO As Object) As Boolean
        Try
            Dim n As Double = CType(objO, Double)

            If (n > 0) And (n < 101) Then
                Return True
            Else
                Return False
            End If
        Catch
            Return False
        End Try
    End Function

    Public Property idCompany() As Integer
        Get
            If viewstate.Item("idCompany") Is Nothing Then
                Return 0
            Else
                Return viewstate.Item("idCompany")
            End If
        End Get

        Set(ByVal Value As Integer)
            viewstate.Add("idCompany", Value)
        End Set
    End Property

    Public Property m_iHotelId() As Integer
        Get
            Return ViewState(KEY_HOTELID)
        End Get

        Set(ByVal Value As Integer)
            VIEWSTATE(KEY_HOTELID) = Value
        End Set
    End Property

    Public Property edicion() As Boolean
        Get
            Return ViewState("Edicion")
        End Get

        Set(ByVal Value As Boolean)
            ViewState("Edicion") = Value
           
        End Set
    End Property

    Public ReadOnly Property isSourceSelected() As Boolean
        Get
            If Not Regex.IsMatch(txtRateCode.Text.Trim, "^[A-Z0-9 a-z]*$") Then
                descripcionError = PortalCulture.GetString("01512")
                Return False
            End If

            If (Me.chkGDS.Checked Or Me.chkPortal.Checked Or Me.chkUnipantalla.Checked Or Me.chkADS.Checked) = False Then
                validarScript()
                descripcionError = PortalCulture.GetString("00768")
                Return False
            Else
                Return True
            End If



        End Get
    End Property

    Public Property IdRatePlan() As String
        Get
            Return If(ViewState("_IdRatePlan") Is Nothing, "", ViewState("_IdRatePlan"))
        End Get

        Set(ByVal Value As String)
            viewstate("_IdRatePlan") = Value
        End Set
    End Property

    Private Property idDicc() As Integer
        Get
            Return ViewState("idDicc")
        End Get

        Set(ByVal Value As Integer)
            ViewState("idDicc") = Value
        End Set
    End Property

    Private Property idShortDesc() As Integer
        Get
            Return ViewState("idShortDesc")
        End Get

        Set(ByVal Value As Integer)
            ViewState("idShortDesc") = Value
        End Set
    End Property

    Public Property idRateCode() As String
        Get
            Return viewstate("idRateCode")
        End Get

        Set(ByVal Value As String)
            viewstate("idRateCode") = Value
        End Set
    End Property

    Public Property listadoDT() As DataTable
        Get
            If Not Session("listado") Is Nothing Then
                Return CType(Session("listado"), DataTable)
            End If
            Return Nothing
        End Get

        Set(ByVal Value As DataTable)
            Session("listado") = Value
        End Set
    End Property

    'Public Property m_orden() As Integer
    '    Get
    '        Return viewstate("orden")
    '    End Get

    '    Set(ByVal Value As Integer)
    '        viewstate("orden") = Value
    '    End Set
    'End Property

    Public Sub loadRatePlan(ByVal rateCode As String, ByVal Principal As Boolean)
        Dim dsRatePlan As RatePlanData
        Dim dato As String = String.Empty

        With New RatePlanFacade
            dsRatePlan = .GetDataRatePlan(rateCode, Me.m_iHotelId)
        End With

        If dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows.Count > 0 Then
            Me.HasData = True
            With dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows(0)
                If Not .IsNull(dsRatePlan.FIELD_IDDICDESC) Then
                    idDicc = .Item(dsRatePlan.FIELD_IDDICDESC)
                Else
                    idDicc = 0
                End If

                If Not .IsNull(dsRatePlan.FIELD_IDDICSHORTDESC) Then
                    idShortDesc = .Item(dsRatePlan.FIELD_IDDICSHORTDESC)
                Else
                    idShortDesc = 0
                End If

                If Not .IsNull(dsRatePlan.FIELD_IDDICCPROMODESC) Then
                    Me.txtPromoDescription.CargaDatos(.Item(dsRatePlan.FIELD_IDDICCPROMODESC))
                Else
                    Me.txtPromoDescription.CargaDatos(0)
                End If

                txtShortDescription.CargaDatos(idShortDesc)
                txtShortDescription.textodefault = .Item(dsRatePlan.FIELD_NAME)
                txtDescripcion.CargaDatos(idDicc)
                txtDescripcion.textodefault = .Item(dsRatePlan.FIELD_DESCRIPTION)
                txtRateCode.Text = .Item(dsRatePlan.FIELD_CODIGOTARIFA)

                Try
                    ddlRules.SelectedValue = .Item(dsRatePlan.FIELD_IDRULE)
                Catch ex As Exception
                    ddlRules.SelectedValue = 0
                End Try

                Session("idDD") = .Item(dsRatePlan.FIELD_IDDICDESC)
                Session("segmentRacPrinc") = .Item(dsRatePlan.FIELD_SegmentRacPrinc)
                Session("idDSD") = .Item(dsRatePlan.FIELD_IDDICSHORTDESC)
                txtDescProm.Text = .Item(dsRatePlan.FIELD_DESCPROMOTION).ToString

                'Leemos el campoidContrato
                If Not .IsNull(dsRatePlan.FIELD_IDCONTRATO) Then
                    Me.ddlContratosNR.SelectedValue = .Item(dsRatePlan.FIELD_IDCONTRATO)
                Else
                    Me.ddlContratosNR.SelectedValue = 0
                End If

            End With

            Dim dsAR As AgencyRatesData
            With New AgencyRatesDataAccess
                dsAR = .LoadAgencyRatesBy_idHotel_rateCode(Me.m_iHotelId, txtRateCode.Text.ToUpper)

                If Not dsAR Is Nothing AndAlso dsAR.Tables(dsAR.AgencyRates_TABLE).Rows.Count > 0 Then
                    With dsAR.Tables(dsAR.AgencyRates_TABLE).Rows(0)
                        Session("idAgenciasRate") = .Item(dsAR.FIELD_idAgenciasRate)

                        txtPorcUNI.Text = .Item(dsAR.FIELD_porcUNI)
                        txtPorcPOR.Text = .Item(dsAR.FIELD_porcPOR)
                        txtPorcGDS.Text = .Item(dsAR.FIELD_porcGDS)
                        txtPorcADS.Text = .Item(dsAR.FIELD_porcADS)
                        If txtPorcGDS.Text.Trim <> "0" Then
                            chkGDS.Checked = True
                            txtPorcGDS.Enabled = True

                            dato = .Item(dsAR.FIELD_GDSApply)
                            dato = dato.ToUpper

                            chkGDSAmadeus.Checked = False
                            chkGDSGalileo.Checked = False
                            chkGDSabre.Checked = False
                            chkGDSWorldSpan.Checked = False

                            If dato.Chars(0) = "Y" Then
                                chkGDSAmadeus.Checked = True
                            End If

                            If dato.Chars(1) = "Y" Then
                                chkGDSGalileo.Checked = True
                            End If

                            If dato.Chars(2) = "Y" Then
                                chkGDSabre.Checked = True
                            End If

                            If dato.Chars(3) = "Y" Then
                                chkGDSWorldSpan.Checked = True
                            End If
                        Else
                            chkGDS.Checked = False
                            txtPorcGDS.Enabled = False
                        End If

                        If txtPorcPOR.Text.Trim <> "0" Then
                            chkPortal.Checked = True
                            txtPorcPOR.Enabled = True
                        Else
                            chkPortal.Checked = False
                            'txtPorcPOR.Enabled = False
                        End If

                        If txtPorcUNI.Text.Trim <> "0" Then
                            chkUnipantalla.Checked = True
                            txtPorcUNI.Enabled = True
                        Else
                            chkUnipantalla.Checked = False
                            'txtPorcUNI.Enabled = False
                        End If

                        If txtPorcADS.Text.Trim <> "0" Then
                            chkADS.Checked = True
                            txtPorcADS.Enabled = True
                        Else
                            chkADS.Checked = False
                            'txtPorcADS.Enabled = False 
                        End If

                        dato = .Item(dsAR.FIELD_idAgenciasRate)
                    End With

                    'ctrPortal1.LoadPortales(m_iHotelId, 1, Me.txtRateCode.Text)
                    ctrPortal1.LoadPortales(m_iHotelId, 1, IdRatePlan)
                End If
            End With

            Dim dsARA As AgencyRatesAvailData
            With New AgencyRatesAvailDataAccess
                Dim i As Integer
                Dim dt As DataTable = Nothing
                Dim dr As DataRow

                dsARA = .LoadAgencyRatesAvailByID(dato)

                If Not dsARA Is Nothing AndAlso dsARA.Tables(dsARA.AgencyRatesAvail_TABLE).Rows.Count > 0 Then
                    chkTodasAgencias.Checked = False
                    dt = New DataTable

                    dt.Columns.Add("IATA")
                    dt.Columns.Add("Nombre")
                    dt.Columns.Add("idAgencia")
                    dt.Columns.Add("isTourOperador")


                    For i = 0 To dsARA.Tables(dsARA.AgencyRatesAvail_TABLE).Rows.Count - 1
                        With dsARA.Tables(dsARA.AgencyRatesAvail_TABLE).Rows(i)
                            dr = dt.NewRow()
                            dr("IATA") = .Item(dsARA.FIELD_IATA)
                            dr("Nombre") = .Item("Nombre")
                            dr("idAgencia") = .Item(dsARA.FIELD_idAgencia)
                            If .Item(dsARA.FIELD_idAgencia) = "-1" Then
                                dr("isTourOperador") = "0"
                            Else
                                dr("isTourOperador") = "1"
                            End If
                            dt.Rows.Add(dr)
                        End With
                    Next
                Else
                    chkTodasAgencias.Checked = True
                End If

                listadoDT = dt
                dgAgencias.DataSource = dt
                dgAgencias.DataBind()
            End With

            validarScript()
        End If
    End Sub
#End Region

#Region "Eventos"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Session("trGDS") = True
            ClearData()
            loadRules()
            cargarDatosHotel()
            CargaContratos()
            rdbNombre.Attributes("onclick") = String.Format("javascript:onrdbNombre('{0}', '{1}','{2}');", RenglonIATA.ClientID, RenglonAgencia.ClientID, rdbNombre.ClientID)
            rdbIATA.Attributes("onclick") = String.Format("javascript:onrdbIATA('{0}', '{1}','{2}');", RenglonIATA.ClientID, RenglonAgencia.ClientID, rdbIATA.ClientID)
        
        End If
        txtRateCode.Attributes.Add("onkeypress", "return validarkeyCode(event);")
    End Sub

    Private Sub cargarDatosHotel()
        Dim dsHotel As HotelDatos
        'Dim dsEtiq As MonedaDatos

        With New HotelSistema
            dsHotel = .GetHotelById(m_iHotelId)
        End With

        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                Me.chkGDS.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnGDS) OrElse .Item(dsHotel.FIELD_AvailOnGDS) = 0, False, True)
                Me.chkPortal.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnPortal) OrElse .Item(dsHotel.FIELD_AvailOnPortal) = 0, False, True)
                Me.chkUnipantalla.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnOnePage) OrElse .Item(dsHotel.FIELD_AvailOnOnePage) = 0, False, True)
                Me.chkADS.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnADS) OrElse .Item(dsHotel.FIELD_AvailOnADS) = 0, False, True)
            End With
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        Me.lblRatecode.Text = PortalCulture.GetString("00740", True)
        Me.lblname.Text = PortalCulture.GetString("00741", True)
        Me.lblDescripcion.Text = PortalCulture.GetString("00742", True)
        Me.lblRule.Text = PortalCulture.GetString("00743", True)
        Me.lblRateApply.Text = PortalCulture.GetString("00744", True)
        Me.lblAplicaGDS.Text = PortalCulture.GetString("00746", True)
        Me.lblComision.Text = PortalCulture.GetString("00747", True)
        Me.lblAgencia.Text = PortalCulture.GetString("00748", True)
        Me.btnAgencia.Text = PortalCulture.GetString("00750")
        Me.chkUnipantalla.Text = PortalCulture.GetString("00745")
        Me.lblPorcUni.Text = PortalCulture.GetString("00745")
        Me.chkTodasAgencias.Text = PortalCulture.GetString("00749")
        Me.lblPromotiontitle.Text = PortalCulture.GetString("00560")
        Me.lblPromotion.Text = PortalCulture.GetString("00559", True)
        Me.lblContratos.Text = PortalCulture.GetString("01114", True)
        dgAgencias.Columns(1).HeaderText = PortalCulture.GetString("M0BT0000186")

        chkGDS.Attributes("onclick") = String.Format("javascript:onCheckBoxesClick('{0}', '{1}', '{2}');", chkGDS.ClientID, trGDSApply.ClientID, trPorcGDS.ClientID)
        'chkPortal.Attributes("onclick") = String.Format("javascript:onCheckBox('{0}', '{1}');", chkPortal.ClientID, trPorcPortal.ClientID)
        chkPortal.Attributes("onclick") = String.Format("javascript:showRowPortal('{0}', '{1}', '{2}');", chkPortal.ClientID, trPorcPortal.ClientID, trPortal.ClientID)

        chkUnipantalla.Attributes("onclick") = String.Format("javascript:onCheckBox('{0}', '{1}');", chkUnipantalla.ClientID, trPorcUni.ClientID)
        chkADS.Attributes("onclick") = String.Format("javascript:onCheckBox('{0}', '{1}');", chkADS.ClientID, trPorcADS.ClientID)
        chkTodasAgencias.Attributes("onclick") = String.Format("javascript:onCheckBoxAgencias('{0}', '{1}', '{2}');", chkTodasAgencias.ClientID, TrAgencias.ClientID, txtAgencia.ClientID)

        'renglonBuscarPor.Style("display") = "none"
        'RenglonIATA.Style("display") = "none"
        'RenglonAgencia.Style("display") = "none"

        If chkTodasAgencias.Checked = False Then
            renglonBuscarPor.Style("display") = "block"
            If rdbIATA.Checked = True Then
                RenglonIATA.Style("display") = "block"
                RenglonAgencia.Style("display") = "none"
            Else
                RenglonIATA.Style("display") = "none"
                RenglonAgencia.Style("display") = "block"
            End If
        Else
            renglonBuscarPor.Style("display") = "none"
            RenglonIATA.Style("display") = "none"
            RenglonAgencia.Style("display") = "none"
        End If
        lblBuscarPor.Text = PortalCulture.GetString("M0BT0000113")
        btnAgregarTourOp.Text = PortalCulture.GetString("M0BT0000188")
        rdbNombre.Text = PortalCulture.GetString("M0BT0000186")

        If SearchAgency1.NumberOfAgenciesFounded > 0 Then
            btnAgregarTourOp.Style("display") = "block"
        Else
            btnAgregarTourOp.Style("display") = "none"
        End If

        If CType(Me.Page, PaginaBase).IsSupervisor = False Then
            ddlContratosNR.Visible = False
            lblContratos.Visible = False
        Else
            ddlContratosNR.Visible = True
            lblContratos.Visible = True
        End If

    End Sub

    Private Sub btnAgencia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgencia.Click
        If rdbIATA.Checked Then
            If txtAgencia.Text.Trim <> String.Empty Then
                chkTodasAgencias.Checked = False
                Dim dt As DataTable = listadoDT()
                Dim dr As DataRow
                If dt Is Nothing Then
                    dt = New DataTable
                    dt.Columns.Add("IATA")
                    dt.Columns.Add("Nombre")
                    dt.Columns.Add("idAgencia")
                    dt.Columns.Add("isTourOperador")
                End If
                dr = dt.NewRow()
                dr("IATA") = txtAgencia.Text
                dr("Nombre") = "--"
                dr("idAgencia") = "-1"
                dr("isTourOperador") = "0"
                dt.Rows.Add(dr)
                listadoDT = dt
                dgAgencias.DataSource = dt
                dgAgencias.DataBind()
                txtAgencia.Text = String.Empty
            End If
        End If
    End Sub

    Private Sub dgAgencias_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAgencias.ItemCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim dt As DataTable

                If Not (listadoDT Is Nothing) Then
                    dt = listadoDT
                    dt.Rows(e.Item.ItemIndex).Delete()

                    If dt.Rows.Count = 0 Then
                        dt = Nothing
                    End If

                    listadoDT = dt
                    dgAgencias.DataSource = dt
                    dgAgencias.DataBind()
                End If
            End If
        Catch ex As Exception
            Dim mensaje As String = ex.Message
        End Try
    End Sub

    Private Sub chkGDS_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGDS.CheckedChanged
        If chkGDS.Checked = False Then
            txtPorcGDS.Text = "0"
            txtPorcGDS.Enabled = False
        Else
            txtPorcGDS.Enabled = True
        End If
    End Sub
#End Region

    Private Sub btnAgregarTourOp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarTourOp.Click
        If rdbNombre.Checked Then
            If SearchAgency1.IdAgenciaSeleccionada <> -1 Then
                'Es por nombre de agencia, (Tour Operador)
                Dim dt As DataTable = listadoDT()
                If Not dt Is Nothing Then
                    If dt.Select("idAgencia = '" & SearchAgency1.IdAgenciaSeleccionada() & "'").Length > 0 Then
                        Exit Sub
                    End If
                End If
                Dim dr As DataRow
                If dt Is Nothing Then
                    dt = New DataTable
                    dt.Columns.Add("IATA")
                    dt.Columns.Add("Nombre")
                    dt.Columns.Add("idAgencia")
                    dt.Columns.Add("isTourOperador")
                End If
                dr = dt.NewRow()
                dr("IATA") = "--"
                dr("Nombre") = SearchAgency1.AgenciaSeleccionada()
                dr("idAgencia") = SearchAgency1.IdAgenciaSeleccionada()
                dr("isTourOperador") = "1"
                dt.Rows.Add(dr)
                listadoDT = dt
                dgAgencias.DataSource = dt
                dgAgencias.DataBind()
            End If
        End If
    End Sub

    Public Function eliminarPortales(ByVal IdHotel As Integer, ByVal codigo As String) As Boolean
        ctrPortal1.EliminarPortales(IdHotel, 1, codigo)
    End Function
End Class
