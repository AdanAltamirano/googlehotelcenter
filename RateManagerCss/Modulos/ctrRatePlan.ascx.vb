Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports APIServices.Models
Imports APIServices.Conflux
Imports APIServices.Conflux.Enum
Imports APIServices.Conflux.OTA.Models.Rates
Imports APIServices.Conflux.Models.Rates.Response
Imports APIServices.Conflux.Models.RatePlan.Response
Imports RateManager.Utitlities.Hotel


Partial Class ctrRatePlan
    Inherits UserControlBase

    Dim ds As New DataSet
    Protected WithEvents txtDescripcion As CtrlIdioma
    Protected WithEvents txtShortDescription As CtrlIdioma
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox2 As System.Web.UI.WebControls.TextBox
    Const KEY_HOTELID = "HotelId"
    Protected WithEvents txtOrden As System.Web.UI.WebControls.TextBox
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Public descripcionError = String.Empty
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents hotelPayment As System.Web.UI.WebControls.CheckBox
    Protected WithEvents portalMovil As System.Web.UI.WebControls.CheckBox
    Protected WithEvents onlyCC As System.Web.UI.WebControls.CheckBox
    Public WithEvents msgGHC_Title As System.Web.UI.WebControls.Label

    Protected WithEvents ctrPortal1 As ctrPortal
    '''Protected WithEvents trPortal As System.Web.UI.HtmlControls.HtmlTableRow

    Public Property CodigosTarifas() As String
        Get
            Return viewstate("_SN")
        End Get
        Set(ByVal Value As String)
            viewstate("_SN") = Value
        End Set
    End Property

    Public Property Supervisor() As Boolean
        Get
            Return viewstate("Supervisor")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("Supervisor") = Value
        End Set
    End Property
    Public Property HasAssignamentCountry() As Boolean
        Get
            Return ViewState("ContainAssignament")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("ContainAssignament") = Value
        End Set
    End Property
    Public Property IdRatePlan() As String
        Get
            Return viewstate("_IdRatePlan")
        End Get
        Set(ByVal Value As String)
            viewstate("_IdRatePlan") = Value
        End Set
    End Property
    Public ReadOnly Property isSourceSelected() As Boolean
        Get
            ''''Return Me.chkGDS.Checked Or Me.chkPortal.Checked Or Me.chkUnipantalla.Checked Or Me.chkADS.Checked
            Dim valor As Boolean = True

            If Me.chkGDS.Checked = False And Me.chkPortal.Checked = False And Me.chkUnipantalla.Checked = False And Me.chkADS.Checked = False Then
                descripcionError = PortalCulture.GetString("00324")
                valor = False
            Else
                If Me.chkGDS.Checked = True And Me.chkGDSAmadeus.Checked = False And Me.chkGDSGalileo.Checked = False And Me.chkGDSSabre.Checked = False And Me.chkGDSWorldSpan.Checked = False Then
                    descripcionError = PortalCulture.GetString("00919")
                    valor = False
                End If
            End If


            If Not Regex.IsMatch(txtRateCode.Text.Trim, "^[A-Z0-9 a-z]*$") Then
                descripcionError = PortalCulture.GetString("01512")
                Return False
            End If

            If Not Regex.IsMatch(txtAccessCode.Text.Trim, "^[A-Z0-9 a-z]*$") Then
                descripcionError = PortalCulture.GetString("M000519")
                Return False
            End If


            'If valor = True Then
            '    If chkPortal.Checked = True Then
            '        If (ctrPortal1.validaSeleccionPortal(Me.chkPortal.Checked)) = False Then
            '            descripcionError = PortalCulture.GetString("01012")
            '            valor = False
            '        End If
            '    End If
            'End If

            If valor = True Then
                If chkPortal.Checked = True And (ctrPortal1.validaSeleccionPortal(Me.chkPortal.Checked)) = False Then
                    '                    If (ctrPortal1.validaSeleccionPortal(Me.chkPortal.Checked)) = False Then
                    descripcionError = PortalCulture.GetString("01012")
                    valor = False
                    '               End If
                End If
            End If

            Return valor
        End Get
    End Property
    Public Property edicion() As Boolean
        Get
            Return ViewState("Edicion")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Edicion") = Value

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

    Public Property totalRatePlan() As Integer
        Get
            Return ViewState("totalRatePlan")
        End Get
        Set(ByVal Value As Integer)
            ViewState("totalRatePlan") = Value
        End Set
    End Property
    Public ReadOnly Property tarifaComisionableId() As String
        Get
            Return TextBoxTarifaComisionable.ClientID
        End Get
    End Property

    Public ReadOnly Property ContratosNRId() As String
        Get
            Return ddlContratosNR.ClientID
        End Get
    End Property

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not Me.IsPostBack Then

            'Me.lblmsg.Style.Add("display", "none")
            Me.txtCD.Style.Add("display", "none")
            Me.lblAccessCode.Style.Add("display", "none")
            Me.lblCD.Style.Add("display", "none")
            Me.txtAccessCode.Style.Add("display", "none")
            FillSegments()
            FillRatePlans()

            Dim i As Byte
            Dim j As Byte = ddlSegmentos.Items.Count - 1
            For i = 0 To j
                If ddlSegmentos.Items(i).Value = "I" Then
                    ddlSegmentos.Items.RemoveAt(i)
                    i = j
                End If
            Next

            Me.txtShortDescription.IsMultiline = False
            lblDeposittitle.Visible = False
            RdbNone.Visible = False
            RdbOneNigth.Visible = False
            rdbAlltotal.Visible = False
            '''''cargarOrden()

            cargarDatosHotel()
            CargaContratos()
            tableConfDeal.Style.Add("Display", "none")
            txtInicio.Text = Now.ToString("MM/dd/yyyy")
            txtFinal.Text = Now.AddDays(1).ToString("MM/dd/yyyy")

            Carga_Monedas()
        End If

        'Me.chkPortal.Attributes.Add("onclick", "javascript:showRowPromotion('" & Me.chkUnipantalla.ClientID & "','" & Me.chkPortal.ClientID & "');")
        Me.chkPortal.Attributes.Add("onclick", "javascript:showRowPortal('" & Me.chkUnipantalla.ClientID & "','" & Me.chkPortal.ClientID & "','" & Me.trPorcPortal.ClientID & "');")

        'Me.chkPortal.Attributes.Add("onclick", "javascript:showRowPortal('" & Me.chkUnipantalla.ClientID & "','" & Me.chkPortal.ClientID & "','" & trPortal.ClientID & "');")

        chkUnipantalla.Attributes.Add("onclick", "javascript:showRowPromotion('" & Me.chkPortal.ClientID & "','" & Me.chkUnipantalla.ClientID & "','" & Me.trPorcUni.ClientID & "');")


        chkGDS.Attributes.Add("onclick", "javascript:onCheckBoxesClick('" & Me.chkGDS.ClientID & "','" & trGDSApply.ClientID & "','" & trPorcGDS.ClientID & "');showRowPromotion('" & Me.chkPortal.ClientID & "','" & Me.chkUnipantalla.ClientID & "','" & Me.trPorcUni.ClientID & "');")

        Me.txtDescripcion.RequiredText = False
        Me.txtDescripcion.MaxLength = 80

        Me.Page.RegisterStartupScript("", "<script>MostrarOcultarConf('" & tableConfDeal.ClientID & "','" & CheckBoxDeal.ClientID & "');" & "DesabilitarHabilitarHora('" & CheckBoxDefHora.ClientID & "','" & HoraInicio.ClientID & "','" & MinutoInicio.ClientID & "','" & HoraFin.ClientID & "','" & MinutoFin.ClientID & "');</script>")

        txtRateCode.Attributes.Add("onkeypress", "return validarkeyCode(event);")
        txtAccessCode.Attributes.Add("onkeypress", "return validarkeyCode(event);")
        'ddlContratosNR.Attributes.Add("onChange", String.Format("javascript:FireNoneNetRate('{0}','{1}','{2}');", _
        '                                Me.lblTitleDinero.ClientID, pnlDinero.ClientID, ddlContratosNR.ClientID))

    End Sub

    Private Sub FillRatePlans()

        Dim dataPlans As RatePlanData
        Dim dataLinks As LinkRatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            If CType(Me.Page, PaginaBase).IsUsuarioHomeAgency Then
                dataPlans = .GetRatePlanByConvenio(ID:=Me.m_iHotelId, idioma:=PortalCulture.GetIDCulture, incluirNetRatesPlan:=1, IncluirPaquetesSegmentoK:=1, IdUsuario:=CType(Me.Page, PaginaBase).Usuario, idAsociacion:=idAsoc, DeleteFilter:=1)
            Else
                dataPlans = .GetRatePlanByIdHotel(ID:=Me.m_iHotelId, idioma:=PortalCulture.GetIDCulture, incluirNetRatesPlan:=1, IncluirPaquetesSegmentoK:=1, idAsociacion:=idAsoc, DeleteFilter:=1)

            End If
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
                    If plan(dataPlans.FIELD_SEGMENT).ToString().ToUpper() <> "I" Then
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

    Private Sub cargarDatosHotel()
        Dim dsHotel As HotelDatos
        Dim dsEtiq As MonedaDatos

        With New HotelSistema
            dsHotel = .GetHotelById(m_iHotelId)
        End With

        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                Me.chkGDS.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnGDS) OrElse .Item(dsHotel.FIELD_AvailOnGDS) = 0, False, True)
                Me.chkPortal.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnPortal) OrElse .Item(dsHotel.FIELD_AvailOnPortal) = 0, False, True)
                Me.chkUnipantalla.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnOnePage) OrElse .Item(dsHotel.FIELD_AvailOnOnePage) = 0, False, True)
                Me.chkADS.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnADS) OrElse .Item(dsHotel.FIELD_AvailOnADS) = 0, False, True)

                'Me.tipoPago.Visible = IIf(.IsNull(dsHotel.FIELD_IsHouse) OrElse .Item(dsHotel.FIELD_IsHouse) = "False", False, True)
            End With
        End If
    End Sub

    Public Sub cargarOrden()
        Dim i As Integer

        ddlOrden.Items.Clear()
        ddlOrden.Items.Insert(0, "---")

        For i = 1 To Me.totalRatePlan
            ddlOrden.Items.Add(i.ToString)
        Next

        If Me.edicion = False Then
            ddlOrden.Items.Add(ddlOrden.Items.Count)
            ddlOrden.SelectedIndex = ddlOrden.Items.Count - 1
        End If
    End Sub
    'función que llena la lista con los segmentos
    Private Sub FillSegments()
        ds.ReadXml(Server.MapPath(Request.ApplicationPath & "/Data/Segmentos.xml"))
        ddlSegmentos.DataSource = ds.Tables("Segmento")
        ddlSegmentos.DataTextField = "Desc"
        ddlSegmentos.DataValueField = "Code"
        ddlSegmentos.DataBind()
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
        For i As Integer = 0 To ds.Tables("Segmento").Rows.Count - 1
            Me.CodigosTarifas &= "//" & ds.Tables("Segmento").Rows(i).Item("Head").ToString
        Next
        Me.txtRateCode.Text = ds.Tables("Segmento").Rows(0).Item("Head").ToString
    End Sub

    Private Function ValidateLinkCurrency()
        'Try
        '    cmbMonedas.SelectedIndex = 

        'Catch ex As Exception

        'End Try
        Return True
    End Function

    Function getDataXML() As String
        Dim ds As New RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(Me.m_iHotelId, idAsociacion:=idAsoc)
        End With
        Return Util.Utility.GetXml(ds.RATEPLAN_TABLE, "UpdateRatesPlan", ds)
    End Function

    Function ValidaCaracteres() As Boolean

    End Function

    Public Function SavePlan(ByVal publish As Boolean) As Integer
        Dim strError As String = String.Empty
        If (Me.ddlSegmentos.SelectedValue = "N" And Me.txtAccessCode.Text = "") Then
            Me.Page.RegisterStartupScript("", "<script>ShowDivNegotiates('" & ddlSegmentos.ClientID & "','" & Me.txtRateCode.ClientID & _
        "','" & Me.CodigosTarifas & "','" & lblAccessCode.ClientID & "','" & txtAccessCode.ClientID & "','" & lblCD.ClientID & "','" & txtCD.ClientID & "','0'" & ");</script>")
            Return 5
        End If

        'Solo se va usar para revisar si hubo un cambio de descuento, si hay cambio se enviaria el request a google
        Dim totalPromotionBeforeEdition As String = Nothing

        If Me.edicion Then
            Dim dsRatePlan As RatePlanData
            With New RatePlanFacade
                dsRatePlan = .GetDataRatePlan(Me.txtRateCode.Text.Trim, Me.m_iHotelId)
            End With

            If dsRatePlan IsNot Nothing AndAlso dsRatePlan.Tables.Count > 0 AndAlso dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows.Count > 0 Then
                With dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows(0)
                    totalPromotionBeforeEdition = .Item(dsRatePlan.FIELD_DESCPROMOTION).ToString()
                End With
            End If

            If dsRatePlan IsNot Nothing AndAlso dsRatePlan.Tables.Count > 0 AndAlso dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows.Count > 0 AndAlso dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows(0)("onAgreement") AndAlso dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows(0)(dsRatePlan.FIELD_SEGMENT).ToString() <> Me.ddlSegmentos.SelectedValue Then
                Return 11
            End If

        End If

        Dim dsRate As New RatePlanData 'Para Guardar
        Dim Rp As RatePlanData
        Dim rRate As DataRow
        Dim val As Boolean
        Dim idRate, NuevoIdRate As String
        Dim idHotel As Integer, dvRp As DataView
        Dim GDSAplicado As String = String.Empty
        Dim sData As String = ""
        Dim sDataPrev As String = ""
        Dim idAsoc As Integer = Me.GetIdAsociation


        With New RatePlanFacade
            Rp = .GetRatePlanByIdHotel(Me.m_iHotelId, idAsociacion:=idAsoc)
        End With
        sDataPrev = getDataXML()


        dvRp = Rp.Tables(Rp.RATEPLAN_TABLE).DefaultView
        If Not Me.edicion Then
            dvRp.RowFilter = dsRate.FIELD_CODIGOTARIFA & "='" & Me.txtRateCode.Text.Trim & "' and " & dsRate.FIELD_IDHOTEL & "=" & Me.m_iHotelId
        Else
            dvRp.RowFilter = dsRate.FIELD_CODIGOTARIFA & "='" & Me.txtRateCode.Text.Trim & "' and " & dsRate.FIELD_IDHOTEL & "=" & Me.m_iHotelId & " and " & dsRate.FIELD_IDRATEPLAN & "<>'" & Me.IdRatePlan & "'"
        End If

        If Me.edicion AndAlso Not ValidateLinkCurrency() Then
            Return 4
        End If

        If dvRp.Count > 0 Then
            Me.Page.RegisterStartupScript("", "<script>ShowDivNegotiates('" & ddlSegmentos.ClientID & "','" & Me.txtRateCode.ClientID & _
        "','" & Me.CodigosTarifas & "','" & lblAccessCode.ClientID & "','" & txtAccessCode.ClientID & "','" & lblCD.ClientID & "','" & txtCD.ClientID & "','0'" & ");</script>")
            If dvRp(0)("Deleted").ToString() = "True" Then
                Return 12
            Else
                Return 3
            End If

        End If

        Dim ComGDS As Integer = 0.0
        Dim ComPortal As Integer = 0.0
        Dim ComOnePage As Integer = 0.0
        Dim ComADS As Integer = 0.0

        If chkGDS.Checked Then
            Try
                If txtPorcGDS.Text.Trim = String.Empty Then
                    ComGDS = -1
                Else
                    ComGDS = Integer.Parse(txtPorcGDS.Text)
                End If
            Catch ex As Exception
                Return 7
            End Try
        Else
            ComGDS = -1
        End If

        If chkPortal.Checked Then
            Try
                If txtPorcPOR.Text.Trim = String.Empty Then
                    ComPortal = -1
                Else
                    ComPortal = Integer.Parse(txtPorcPOR.Text)
                End If
            Catch ex As Exception
                Return 8
            End Try
        Else
            ComPortal = -1
        End If

        If chkUnipantalla.Checked Then
            Try
                If txtPorcUNI.Text.Trim = String.Empty Then
                    ComOnePage = -1
                Else
                    ComOnePage = Integer.Parse(txtPorcUNI.Text)
                End If
            Catch ex As Exception
                Return 9
            End Try
        Else
            ComOnePage = -1
        End If

        If chkADS.Checked Then
            Try
                If txtPorcADS.Text.Trim = String.Empty Then
                    ComADS = -1
                Else
                    ComADS = Integer.Parse(txtPorcADS.Text)
                End If
            Catch ex As Exception
                Return 10
            End Try
        Else
            ComADS = -1
        End If

        If chkGDS.Checked = False Then
            GDSAplicado = "NNNN"
        Else
            If chkGDSAmadeus.Checked = True Then
                GDSAplicado = GDSAplicado + "Y"
            Else
                GDSAplicado = GDSAplicado + "N"
            End If


            If chkGDSGalileo.Checked = True Then
                GDSAplicado = GDSAplicado + "Y"
            Else
                GDSAplicado = GDSAplicado + "N"
            End If

            If chkGDSSabre.Checked = True Then
                GDSAplicado = GDSAplicado + "Y"
            Else
                GDSAplicado = GDSAplicado + "N"
            End If

            If chkGDSWorldSpan.Checked = True Then
                GDSAplicado = GDSAplicado + "Y"
            Else
                GDSAplicado = GDSAplicado + "N"
            End If
        End If

        rRate = dsRate.Tables(dsRate.RATEPLAN_TABLE).NewRow()
        With rRate
            If Me.edicion = False Then
                .Item(dsRate.FIELD_IDRATEPLAN) = Me.txtRateCode.Text.ToUpper 'Me.txtCodigo.Text.ToUpper
            Else
                .Item(dsRate.FIELD_IDRATEPLAN) = IdRatePlan
            End If
            .Item(dsRate.FIELD_DESCRIPTION) = Me.txtDescripcion.textodefault

            .Item(dsRate.FIELD_SEGMENT) = Me.ddlSegmentos.SelectedValue
            .Item(dsRate.FIELD_IDHOTEL) = Me.m_iHotelId
            .Item(dsRate.FIELD_IDDICDESC) = Me.txtDescripcion.IdIndice
            .Item(dsRate.FIELD_HOTELPAYMENT) = hotelPayment.Checked
            .Item(dsRate.FIELD_ISMOBILERATE) = portalMovil.Checked
            .Item(dsRate.FIELD_ISCALLCENTERONLY) = onlyCC.Checked
            .Item(dsRate.FIELD_CODIGOTARIFA) = Me.txtRateCode.Text.ToUpper
            .Item(dsRate.FIELD_NAME) = Me.txtShortDescription.textodefault
            .Item(dsRate.FIELD_IDDICSHORTDESC) = Me.txtShortDescription.IdIndice
            .Item(dsRate.FIELD_GDS) = chkGDS.Checked
            .Item(dsRate.FIELD_GDSAPPLY) = GDSAplicado
            .Item(dsRate.FIELD_PORTAL) = chkPortal.Checked
            .Item(dsRate.FIELD_UNIPANTALLA) = chkUnipantalla.Checked
            .Item(dsRate.FIELD_ADS) = chkADS.Checked
            .Item(dsRate.FIELD_COMGDS) = IIf(ComGDS = -1, System.DBNull.Value, ComGDS)
            .Item(dsRate.FIELD_COMPORTAL) = IIf(ComPortal = -1, System.DBNull.Value, ComPortal)
            .Item(dsRate.FIELD_COMONEPAGE) = IIf(ComOnePage = -1, System.DBNull.Value, ComOnePage)
            .Item(dsRate.FIELD_COMADS) = IIf(ComADS = -1, System.DBNull.Value, ComADS)
            .Item(dsRate.WAITLISTAVAILABLE_FIELD) = Me.chkWaitListAvailable.Checked
            .Item(dsRate.FIELD_IDDICCPROMODESC) = Me.txtPromoDescription.IdIndice

            'If tipoPago.Visible Then
            '.Item(dsRate.FIELD_TIPOPAGO) = ddlTipoPago.SelectedIndex
            'End If

            If ddlContratosNR.SelectedValue <> 0 Then
                .Item(dsRate.FIELD_IDCONTRATO) = ddlContratosNR.SelectedValue
            Else
                .Item(dsRate.FIELD_IDCONTRATO) = System.DBNull.Value
            End If

            If (Me.txtDaysFree.Text <> "" AndAlso (chkPortal.Checked Or Me.chkUnipantalla.Checked Or Me.chkGDS.Checked)) Then
                .Item(dsRate.FIELD_DAYSFREE) = CInt(txtDaysFree.Text)
            End If

            If (Me.txtDescProm.Text <> "" AndAlso (chkPortal.Checked Or Me.chkUnipantalla.Checked Or Me.chkGDS.Checked)) Then
                .Item(dsRate.FIELD_DESCPROMOTION) = CDbl(txtDescProm.Text)
            End If

            'If Me.rdbAlltotal.Checked Then
            '    .Item(dsRate.FIELD_DEPOSITTYPE) = 2
            'ElseIf Me.RdbOneNigth.Checked Then
            '    .Item(dsRate.FIELD_DEPOSITTYPE) = 1
            'End If

            If Me.ddlSegmentos.SelectedValue.ToString.ToUpper = "N" Then
                .Item(dsRate.FIELD_ACCESSCODE) = Me.txtAccessCode.Text.Trim()
                .Item(dsRate.FIELD_CD) = Me.txtCD.Text.Trim()
            ElseIf Me.ddlSegmentos.SelectedValue.ToString.ToUpper = "C" Then
                If Me.txtAccessCode.Text.Trim <> "" OrElse Me.txtCD.Text.Trim <> "" Then
                    .Item(dsRate.FIELD_ACCESSCODE) = Me.txtAccessCode.Text.Trim()
                    .Item(dsRate.FIELD_CD) = Me.txtCD.Text.Trim()
                End If
            End If

            .Item(dsRate.FIELD_IDRULE) = System.DBNull.Value

            If ddlOrden.SelectedIndex <> 0 Then
                .Item(dsRate.FIELD_ORDEN) = ddlOrden.SelectedValue
            Else
                .Item(dsRate.FIELD_ORDEN) = System.DBNull.Value
            End If

            If ddlRules.SelectedValue <> 0 Then
                .Item(dsRate.FIELD_IDRULE) = ddlRules.SelectedValue
            End If


            If cmbMonedas.SelectedValue <> -1 Then
                .Item(dsRate.FIELD_IDMONEDA) = cmbMonedas.SelectedValue
            Else
                .Item(dsRate.FIELD_IDMONEDA) = System.DBNull.Value
            End If
            rRate(dsRate.WAITLISTAVAILABLE_FIELD) = Me.chkWaitListAvailable.Checked

        End With
        dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows.Add(rRate)

        If Me.edicion = False Then
            With New RatePlanFacade
                Dim idPromoDescription As Integer = Me.txtPromoDescription.IdIndice
                If .InsertRatePlan(dsRate, Me.idDicc, Me.idShortDesc, idPromoDescription) Then
                    ctrPortal1.InsertarPortales(Me.m_iHotelId, 3, txtRateCode.Text, chkPortal.Checked)
                    sData = Util.Utility.GetXml(dsRate.RATEPLAN_TABLE, "UpdatePlanPlan", dsRate)
                    sDataPrev = ""
                    CType(Me.Page, PaginaBase).guardalog("/Pages/RatesPlans.aspx", PaginaBase.acciones.Crear, "Creó el rateplan con el id " & rRate(dsRate.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "", sDataPrev, sData)
                    If publish Then
                        CType(Me.Page, PaginaBase).guardalog("/Pages/RatesPlans.aspx", PaginaBase.acciones.Publicar, "Modifico el rateplan con el id " & rRate(dsRate.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "", sDataPrev, sData)
                    Else
                        CType(Me.Page, PaginaBase).NotifyContentModification("Plan tarifario con el codigo " & rRate(dsRate.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "Planes Tarifarios")
                    End If
                    Me.txtDescripcion.Update(dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICDESC), publish)
                    Me.txtShortDescription.Update(dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICSHORTDESC), publish)
                    Me.txtPromoDescription.Update(idPromoDescription, publish)

                    Me.totalRatePlan = Me.totalRatePlan + 1

                    If CheckBoxDeal.Checked = True Then
                        If CheckBoxDefHora.Checked = True Then
                            .InsertRatePlanDeal(Me.txtRateCode.Text.ToUpper, Me.m_iHotelId, txtInicio.Text, txtFinal.Text, HoraInicio.SelectedValue & ":" & MinutoInicio.SelectedValue, HoraFin.SelectedValue & ":" & MinutoFin.SelectedValue)
                        Else
                            .InsertRatePlanDeal(Me.txtRateCode.Text.ToUpper, Me.m_iHotelId, txtInicio.Text, txtFinal.Text, "", "")
                        End If
                    End If

                    Try
                        If Me.lstRatePlans.SelectedIndex > 0 Then
                            Dim value As JObject = JsonConvert.DeserializeObject(Of JObject)(Me.lstRatePlans.SelectedValue)
                            'If Me.ddlContratosNR.SelectedValue = value("contract").Value(Of Integer)() Then
                            If (Me.ddlContratosNR.SelectedValue > 0) = (value("contract").Value(Of Integer)() > 0) Then
                                .CloneRates(Me.m_iHotelId, value("id").Value(Of String)(), Me.txtRateCode.Text.ToUpper)
                                CType(Me.Page, PaginaBase).guardalog("/Pages/RatesPlans.aspx", PaginaBase.acciones.Crear, "Clonó las tarifas del plan tarifario " & value("id").Value(Of String)() & " al plan recién creado " & Me.txtRateCode.Text.ToUpper & ".", "", "", "")
                            End If
                        End If
                    Catch ex As Exception
                    End Try

                    Dim AllWorld As Boolean = True
                    For Each country As ListItem In chklCountries.Items
                        If Not country.Selected Then
                            AllWorld = False
                        End If
                    Next

                    If Not AllWorld Then
                        With (New ClsAssingCountryRatePlan)
                            For Each country As ListItem In chklCountries.Items
                                If country.Selected Then
                                    .InsertAssingnment(Me.m_iHotelId, txtRateCode.Text.Trim, country.Value, strError)
                                End If
                            Next
                        End With
                    End If

                    Me.strError.Value = strError

                    'Google Nuevo RatePlan

                    Dim confluxService As New ConfluxService()
                    Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                    Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)

                    Dim pgBase As PaginaBase = New PaginaBase()

                    If isEnabledGoogleRequest Then
                        Dim res As RatePlanResponse = confluxService.InsertRatePlan(info.Hotel, info.Empresa, Me.txtRateCode.Text, Me.txtShortDescription.GetES(), Me.txtDescripcion.GetES(), "ES")
                        pgBase.guardalog("/Pages/RatesPlans.aspx", pgBase.acciones.Sincronizar, "Sincronizar Nuevo RatePlan ctrRatePlan", "", res.RequestXML, res.Response, info.Hotel)
                    End If

                    ClearData()
                    clearConfDealData()

                    Return 0
                End If


            End With
        Else

            If Me.idDicc <> 0 Then
                Me.txtDescripcion.Update(Me.idDicc, publish)
            Else
                idDicc = Me.txtDescripcion.Insert()
            End If

            If Me.idShortDesc <> 0 Then
                Me.txtShortDescription.Update(Me.idShortDesc, publish)
            Else
                idShortDesc = Me.txtShortDescription.Insert()
            End If

            If dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICCPROMODESC) <> 0 Then
                Me.txtPromoDescription.Update(dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICCPROMODESC), publish)
            Else
                dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICCPROMODESC) = Me.txtPromoDescription.Insert()
            End If

            dsRate.Tables(dsRate.RATEPLAN_TABLE).AcceptChanges()
            dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDHOTEL) = dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDHOTEL)
            If idDicc <> 0 Then
                dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDDICDESC) = idDicc
            End If
            If idShortDesc <> 0 Then
                dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDDICSHORTDESC) = idShortDesc
            End If

            With New RatePlanFacade
                If .UpdateRatePlan(dsRate) Then
                    '**** actualizarAgencyRates(GDSAplicado)
                    ctrPortal1.ModificarPortales(chkPortal.Checked)
                    sData = Util.Utility.GetXml(dsRate.RATEPLAN_TABLE, "UpdatePlanPlan", dsRate)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/RatesPlans.aspx", If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar), "Modifico el rateplan con el id " & rRate(dsRate.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "", sDataPrev, sData)
                    If Me.txtDescripcion.HasChanges OrElse Me.txtShortDescription.HasChanges OrElse Me.txtPromoDescription.HasChanges Then
                        CType(Me.Page, PaginaBase).NotifyContentModification("Plan tarifario con el codigo " & rRate(dsRate.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "Planes Tarifarios")
                    End If

                    Dim AllWorld As Boolean = True
                    For Each country As ListItem In chklCountries.Items
                        If Not country.Selected Then
                            AllWorld = False
                        End If
                    Next

                    If HasAssignamentCountry Then
                        With (New ClsAssingCountryRatePlan)
                            .DeleteAsignmentCountries(Me.m_iHotelId, IdRatePlan, strError)
                        End With
                    End If

                    If Not AllWorld Then
                        If strError = "" Then
                            With (New ClsAssingCountryRatePlan)
                                For Each country As ListItem In chklCountries.Items
                                    If country.Selected Then
                                        .InsertAssingnment(Me.m_iHotelId, IdRatePlan, country.Value, strError)
                                    End If
                                Next
                            End With
                        End If
                    End If

                    Me.strError.Value = strError

                    Dim confluxService As New ConfluxService()
                    Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                    Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
                    Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

                    If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                        If totalPromotionBeforeEdition <> "" And txtDescProm.Text = "" Then

                            ExecuteServices(info.Hotel, info.Empresa, IdRatePlan, isEnabledGoogleRequest, isEnabledSendingRatesAPICache)

                        ElseIf totalPromotionBeforeEdition <> "" And txtDescProm.Text <> "" Then
                            If CDbl(totalPromotionBeforeEdition) <> CDbl(txtDescProm.Text) Then
                                ExecuteServices(info.Hotel, info.Empresa, IdRatePlan, isEnabledGoogleRequest, isEnabledSendingRatesAPICache)
                            End If
                        ElseIf totalPromotionBeforeEdition = "" And txtDescProm.Text <> "" Then
                            ExecuteServices(info.Hotel, info.Empresa, IdRatePlan, isEnabledGoogleRequest, isEnabledSendingRatesAPICache)
                        End If
                    End If

                    ClearData()

                    Dim i As Byte
                    Dim j As Byte = ddlSegmentos.Items.Count - 1
                    For i = 0 To j
                        If ddlSegmentos.Items(i).Value = "I" Then
                            ddlSegmentos.Items.RemoveAt(i)
                            i = j
                        End If
                    Next
                    If CheckBoxDeal.Checked = True Then
                        If CheckBoxDefHora.Checked = True Then
                            .UpdateRatePlanDeal(IdRatePlan, Me.m_iHotelId, txtInicio.Text, txtFinal.Text, HoraInicio.SelectedValue & ":" & MinutoInicio.SelectedValue, HoraFin.SelectedValue & ":" & MinutoFin.SelectedValue)
                        Else
                            .UpdateRatePlanDeal(IdRatePlan, Me.m_iHotelId, txtInicio.Text, txtFinal.Text, "", "")
                        End If
                    Else
                        .DelRateRatePlanDeal(IdRatePlan, Me.m_iHotelId)
                    End If

                    clearConfDealData()

                    Return 0
                End If
            End With

        End If

        Me.Page.RegisterStartupScript("", "<script>ShowDivNegotiates('" & ddlSegmentos.ClientID & "','" & Me.txtRateCode.ClientID & _
        "','" & Me.CodigosTarifas & "','" & lblAccessCode.ClientID & "','" & txtAccessCode.ClientID & "','" & lblCD.ClientID & "','" & txtCD.ClientID & "','0'" & ");</script>")

        Return 2

    End Function

    Public Sub clearConfDealData()
        CheckBoxDeal.Checked = False
        CheckBoxDefHora.Checked = False
        HoraInicio.Enabled = False
        MinutoInicio.Enabled = False
        HoraFin.Enabled = False
        MinutoFin.Enabled = False

        HoraInicio.SelectedIndex = 0
        MinutoInicio.SelectedIndex = 0
        HoraFin.SelectedIndex = 0
        MinutoFin.SelectedIndex = 0
        txtInicio.Text = Now.ToString("MM/dd/yyyy")
        txtFinal.Text = Now.AddDays(1).ToString("MM/dd/yyyy")
        'ocultamos la tabla
        tableConfDeal.Style.Add("display", "none")
    End Sub

    'Private Function actualizarAgencyRates(ByVal GDSApply As String)
    '    Dim dsAgencyRates As New AgencyRatesData
    '    Dim dr As DataRow
    '    Dim idAgenciasRate As String = String.Empty

    '    Dim dsAR As AgencyRatesData
    '    With New AgencyRatesDataAccess
    '        dsAR = .LoadAgencyRatesBy_idHotel_rateCode(Me.m_iHotelId, txtRateCode.Text.ToUpper)

    '        If Not dsAR Is Nothing AndAlso dsAR.Tables(dsAR.AgencyRates_TABLE).Rows.Count > 0 Then
    '            With dsAR.Tables(dsAR.AgencyRates_TABLE).Rows(0)
    '                dr = dsAgencyRates.Tables(dsAgencyRates.AgencyRates_TABLE).NewRow
    '                dr(dsAgencyRates.FIELD_idHotel) = CDbl(Me.m_iHotelId)
    '                dr(dsAgencyRates.FIELD_rateCode) = txtRateCode.Text.ToUpper
    '                dr(dsAgencyRates.FIELD_GDSApply) = GDSApply
    '                dr(dsAgencyRates.FIELD_porcUNI) = .Item(dsAR.FIELD_porcUNI)
    '                dr(dsAgencyRates.FIELD_porcPOR) = .Item(dsAR.FIELD_porcPOR)
    '                dr(dsAgencyRates.FIELD_porcGDS) = .Item(dsAR.FIELD_porcGDS)
    '                dr(dsAgencyRates.FIELD_porcADS) = .Item(dsAR.FIELD_porcADS)
    '                dsAgencyRates.Tables(dsAgencyRates.AgencyRates_TABLE).Rows.Add(dr)
    '                idAgenciasRate = .Item(dsAR.FIELD_idAgenciasRate)

    '                With New AgencyRatesDataAccess
    '                    dsAgencyRates.Tables(dsAgencyRates.AgencyRates_TABLE).AcceptChanges()
    '                    dsAgencyRates.Tables(dsAgencyRates.AgencyRates_TABLE).Rows(0).Item(dsAgencyRates.FIELD_idHotel) = dsAgencyRates.Tables(dsAgencyRates.AgencyRates_TABLE).Rows(0).Item(dsAgencyRates.FIELD_idHotel)

    '                    dr(dsAgencyRates.FIELD_idAgenciasRate) = idAgenciasRate
    '                    .UpdateAgencyRates(dsAgencyRates, idAgenciasRate)
    '                End With
    '            End With
    '        End If
    '    End With
    'End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
        If Me.edicion Then
            ddlSegmentos.Attributes.Add("onChange", "javascript:ShowDivNegotiates('" & ddlSegmentos.ClientID & "','" & Me.txtRateCode.ClientID & _
                    "','" & Me.CodigosTarifas & "','" & lblAccessCode.ClientID & "','" & txtAccessCode.ClientID & "','" & lblCD.ClientID & "','" & txtCD.ClientID & "','1'" & ")")
        Else
            ddlSegmentos.Attributes.Add("onChange", "javascript:ShowDivNegotiates('" & ddlSegmentos.ClientID & "','" & Me.txtRateCode.ClientID & _
        "','" & Me.CodigosTarifas & "','" & lblAccessCode.ClientID & "','" & txtAccessCode.ClientID & "','" & lblCD.ClientID & "','" & txtCD.ClientID & "','0'" & ")")
        End If
        Me.Page.RegisterStartupScript("FillChkADS", "<script>" & String.Format("javascript:onCheckBoxADS('{0}', '{1}');", chkADS.ClientID, trPorcADS.ClientID) & "</script>")
        chkADS.Attributes("onclick") = String.Format("javascript:onCheckBoxADS('{0}', '{1}');", chkADS.ClientID, trPorcADS.ClientID)
        lblComision.Text = PortalCulture.GetString("01047", True)
        lblPorcUni.Text = PortalCulture.GetString("00457", False)

        If Me.Supervisor = False Then
            ddlContratosNR.Visible = False
            lblContratos.Visible = False
        Else
            ddlContratosNR.Visible = True
            lblContratos.Visible = True
        End If
        TextBoxTarifaComisionable.Style.Add("display", "none")
        CheckBoxDeal.Attributes.Add("onClick", "javascript:MostrarOcultarConf('" & tableConfDeal.ClientID & "','" & CheckBoxDeal.ClientID & "' );")
        CheckBoxDefHora.Attributes.Add("onClick", "javascript:DesabilitarHabilitarHora('" & CheckBoxDefHora.ClientID & "','" & HoraInicio.ClientID & "','" & MinutoInicio.ClientID & "','" & HoraFin.ClientID & "','" & MinutoFin.ClientID & "');")
        timevalidator.Style.Add("display", "none")
    End Sub

    Private Sub loadResources()
        lblPortal.Text = PortalCulture.GetString("01009")
        lblDescripcion.Text = PortalCulture.GetString("00002", True)
        lblSegmento.Text = PortalCulture.GetString("00003", True)
        lblRatecode.Text = PortalCulture.GetString("00006", True)
        lblAccessCode.Text = PortalCulture.GetString("00007", True)
        Me.lblRule.Text = PortalCulture.GetString("00298", True)
        Me.ddlRules.Items(0).Text = PortalCulture.GetString("00027")
        lblname.Text = PortalCulture.GetString("00073", True)
        'Me.lblmsg.Text = PortalCulture.GetString("00315")
        lblRateApply.Text = PortalCulture.GetString("00323", True)
        chkUnipantalla.Text = PortalCulture.GetString("00457")
        Me.lblFreeNights.Text = PortalCulture.GetString("00557")
        Me.lblGratis.Text = PortalCulture.GetString("00558", True)
        Me.lblPromotion.Text = PortalCulture.GetString("00559", True)
        Me.lblPromotiontitle.Text = PortalCulture.GetString("00560")
        Me.RVNochesgratis.ErrorMessage = PortalCulture.GetString("00561")
        Me.RVPromotion.ErrorMessage = PortalCulture.GetString("00562")
        Me.RdbNone.Text = PortalCulture.GetString("00717")
        Me.RdbOneNigth.Text = PortalCulture.GetString("00715")
        Me.rdbAlltotal.Text = PortalCulture.GetString("00716")
        Me.lblDeposittitle.Text = PortalCulture.GetString("00718", True)
        Me.lblOrden.Text = PortalCulture.GetString("00920", True)
        Me.rvOrden.ErrorMessage = PortalCulture.GetString("00921")
        Me.lblAplicaGDS.Text = PortalCulture.GetString("00746")
        Me.chkWaitListAvailable.Text = PortalCulture.GetString("01350")
        Me.hotelPayment.Text = PortalCulture.GetString("01646")

        Me.lblContratos.Text = PortalCulture.GetString("01114", True)
        Me.lblFrom.Text = PortalCulture.GetString("00108", True)
        Me.lblTo.Text = PortalCulture.GetString("00109", True)
        Me.lblFromHora.Text = PortalCulture.GetString("00108", True)
        Me.lblToHora.Text = PortalCulture.GetString("00109", True)
        Me.CheckBoxDeal.Text = PortalCulture.GetString("01146", False)
        Me.CheckBoxDefHora.Text = PortalCulture.GetString("01147", False)
        Me.timevalidator.Text = PortalCulture.GetString("01148", False)
        Me.lblMoneda.Text = PortalCulture.GetString("M0UT00477", True)
        Me.lblhelp.Text = PortalCulture.GetString("01382")
        Me.lblAsignarPais.Text = PortalCulture.GetString("01645", True)

        'lblTipoPago.Text = PortalCulture.GetString("M0UT02719", True)
        'ddlTipoPago.Items.Clear()
        'ddlTipoPago.Items.Add(PortalCulture.GetString("M0UT02720"))
        'ddlTipoPago.Items.Add(PortalCulture.GetString("01445"))
        ''ddlTipoPago.SelectedIndex = 0
        'ddlTipoPago.DataBind()
    End Sub

    Public Sub loadRatePlan(ByVal id As String, ByVal Principal As Boolean)
        Dim dsRatePlan As RatePlanData
        Dim dato As String = String.Empty
        Dim dsAssignCountry As DataSet = New DataSet
        HasAssignamentCountry = False


        With New RatePlanFacade
            dsRatePlan = .GetDataRatePlan(id, Me.m_iHotelId)
        End With
        If dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows.Count > 0 Then
            Me.HasData = True
            With dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows(0)
                If Not .IsNull(dsRatePlan.FIELD_IDDICDESC) Then
                    idDicc = .Item(dsRatePlan.FIELD_IDDICDESC)
                Else
                    idDicc = 0
                End If

                'Hotel Payment Check
                Trace.Write("HotelPayment antes")
                If Not .IsNull(dsRatePlan.FIELD_HOTELPAYMENT) Then
                    hotelPayment.Checked = .Item(dsRatePlan.FIELD_HOTELPAYMENT)
                Else
                    hotelPayment.Checked = False
                End If

                If Not .IsNull(dsRatePlan.FIELD_ISMOBILERATE) Then
                    portalMovil.Checked = .Item(dsRatePlan.FIELD_ISMOBILERATE)
                Else
                    portalMovil.Checked = False
                End If

                If Not .IsNull(dsRatePlan.FIELD_ISCALLCENTERONLY) Then
                    onlyCC.Checked = .Item(dsRatePlan.FIELD_ISCALLCENTERONLY)
                Else
                    onlyCC.Checked = False
                End If

                Trace.Write("HotelPayment despues")
                If Not .IsNull(dsRatePlan.FIELD_IDDICSHORTDESC) Then
                    idShortDesc = .Item(dsRatePlan.FIELD_IDDICSHORTDESC)
                Else
                    idShortDesc = 0
                End If
                txtDescripcion.CargaDatos(idDicc)

                If Not .IsNull(dsRatePlan.FIELD_IDMONEDA) Then
                    Me.cmbMonedas.SelectedIndex = cmbMonedas.Items.IndexOf(cmbMonedas.Items.FindByValue(.Item(dsRatePlan.FIELD_IDMONEDA)))
                Else
                    Me.cmbMonedas.SelectedIndex = 0
                End If

                If Principal Then
                    Me.ddlSegmentos.Enabled = False
                Else
                    Me.ddlSegmentos.Enabled = Principal OrElse Not (.Item("onAgreement"))
                End If

                Me.txtShortDescription.CargaDatos(idShortDesc)
                Me.txtDescripcion.textodefault = .Item(dsRatePlan.FIELD_DESCRIPTION).ToString
                Me.txtShortDescription.CargaDatos(idShortDesc)
                Me.txtShortDescription.textodefault = .Item(dsRatePlan.FIELD_NAME).ToString
                Me.chkGDS.Checked = False
                Me.chkGDSAmadeus.Checked = False
                Me.chkGDSGalileo.Checked = False
                Me.chkGDSSabre.Checked = False
                Me.chkGDSWorldSpan.Checked = False
                Me.chkPortal.Checked = False
                Me.chkUnipantalla.Checked = False
                Me.chkADS.Checked = False
                If Not .IsNull(dsRatePlan.FIELD_GDS) Then
                    Me.chkGDS.Checked = .Item(dsRatePlan.FIELD_GDS)
                End If
                Me.chkWaitListAvailable.Checked = .Item(dsRatePlan.WAITLISTAVAILABLE_FIELD)

                If Not .IsNull(dsRatePlan.FIELD_GDSAPPLY) Then
                    dato = .Item(dsRatePlan.FIELD_GDSAPPLY)
                    dato = dato.ToUpper

                    If dato.Chars(0) = "Y" Then
                        Me.chkGDSAmadeus.Checked = True
                    End If

                    If dato.Chars(1) = "Y" Then
                        Me.chkGDSGalileo.Checked = True
                    End If

                    If dato.Chars(2) = "Y" Then
                        Me.chkGDSSabre.Checked = True
                    End If

                    If dato.Chars(3) = "Y" Then
                        Me.chkGDSWorldSpan.Checked = True
                    End If
                End If

                If Not .IsNull(dsRatePlan.FIELD_PORTAL) Then
                    Me.chkPortal.Checked = .Item(dsRatePlan.FIELD_PORTAL)
                End If
                If Not .IsNull(dsRatePlan.FIELD_UNIPANTALLA) Then
                    Me.chkUnipantalla.Checked = .Item(dsRatePlan.FIELD_UNIPANTALLA)
                End If
                If Not .IsNull(dsRatePlan.FIELD_ADS) Then
                    Me.chkADS.Checked = .Item(dsRatePlan.FIELD_ADS)
                End If
                'si todos son null por default ponemos el de gds
                If .IsNull(dsRatePlan.FIELD_GDS) AndAlso .IsNull(dsRatePlan.FIELD_PORTAL) AndAlso .IsNull(dsRatePlan.FIELD_UNIPANTALLA) Then
                    Me.chkGDS.Checked = True
                End If
                If Not .IsNull(dsRatePlan.FIELD_SEGMENT) Then
                    If .Item(dsRatePlan.FIELD_SEGMENT).ToString = "I" Then
                        FillSegments()
                    End If
                    Me.ddlSegmentos.SelectedValue = .Item(dsRatePlan.FIELD_SEGMENT).ToString
                Else
                    Me.ddlSegmentos.SelectedValue = "R"
                End If
                'If Principal Then
                '    Me.ddlSegmentos.Enabled = False
                'Else
                '    Me.ddlSegmentos.Enabled = True
                'End If

                'Leemos el campoidContrato
                If Not .IsNull(dsRatePlan.FIELD_IDCONTRATO) Then
                    Me.ddlContratosNR.SelectedValue = .Item(dsRatePlan.FIELD_IDCONTRATO)
                Else
                    Me.ddlContratosNR.SelectedValue = 0
                End If

                Me.txtAccessCode.Text = .Item(dsRatePlan.FIELD_ACCESSCODE).ToString
                Me.txtCD.Text = .Item(dsRatePlan.FIELD_CD).ToString
                Me.txtRateCode.Enabled = False
                Me.txtRateCode.Text = .Item(dsRatePlan.FIELD_CODIGOTARIFA).ToString
                Me.txtDaysFree.Text = .Item(dsRatePlan.FIELD_DAYSFREE).ToString
                Me.txtDescProm.Text = .Item(dsRatePlan.FIELD_DESCPROMOTION).ToString

                If Not .IsNull(dsRatePlan.FIELD_IDDICCPROMODESC) Then
                    Me.txtPromoDescription.CargaDatos(.Item(dsRatePlan.FIELD_IDDICCPROMODESC))
                Else
                    Me.txtPromoDescription.CargaDatos(0)
                End If

                Me.txtPorcGDS.Text = IIf(.Item(dsRatePlan.FIELD_COMGDS).ToString = String.Empty, "", .Item(dsRatePlan.FIELD_COMGDS).ToString)
                Me.txtPorcPOR.Text = IIf(.Item(dsRatePlan.FIELD_COMPORTAL).ToString = String.Empty, "", .Item(dsRatePlan.FIELD_COMPORTAL).ToString)
                Me.txtPorcUNI.Text = IIf(.Item(dsRatePlan.FIELD_COMONEPAGE).ToString = String.Empty, "", .Item(dsRatePlan.FIELD_COMONEPAGE).ToString)
                Me.txtPorcADS.Text = IIf(.Item(dsRatePlan.FIELD_COMADS).ToString = String.Empty, "", .Item(dsRatePlan.FIELD_COMADS).ToString)

                'Me.RdbOneNigth.Checked = False
                'Me.rdbAlltotal.Checked = False
                'Me.RdbNone.Checked = True
                'If Not .IsNull(dsRatePlan.FIELD_DEPOSITTYPE) Then
                '    Select Case .Item(dsRatePlan.FIELD_DEPOSITTYPE)
                '        Case 1
                '            Me.RdbOneNigth.Checked = True
                '        Case 2
                '            Me.rdbAlltotal.Checked = True
                '    End Select
                'End If

                If Me.totalRatePlan < ddlOrden.Items.Count Then
                    ddlOrden.Items.Remove(ddlOrden.Items.Count - 1)
                End If

                If Not .IsNull(dsRatePlan.FIELD_ORDEN) AndAlso .Item(dsRatePlan.FIELD_ORDEN) > 0 Then
                    If .Item(dsRatePlan.FIELD_ORDEN) > ddlOrden.Items.Count - 1 Then
                        ddlOrden.Items.Add(CType(.Item(dsRatePlan.FIELD_ORDEN), String))
                    End If

                    ddlOrden.SelectedValue = .Item(dsRatePlan.FIELD_ORDEN)
                Else
                    ddlOrden.SelectedIndex = 0
                End If

                Try
                    ddlRules.SelectedValue = .Item(dsRatePlan.FIELD_IDRULE)
                Catch ex As Exception
                    ddlRules.SelectedValue = 0
                End Try

                Dim rateCom As Integer
                If dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Columns.IndexOf("TarifaComisionable") <> -1 Then
                    TextBoxTarifaComisionable.Text = .Item("TarifaComisionable")
                Else
                    TextBoxTarifaComisionable.Text = 0
                End If
                Me.chkWaitListAvailable.Checked = .Item(dsRatePlan.WAITLISTAVAILABLE_FIELD)
            End With

            ctrPortal1.LoadPortales(m_iHotelId, 3, IdRatePlan)

            'Leemos la configuracion de hora si es que tiene
            With New RatePlanFacade
                Dim ds As New DataSet
                ds = .GetRateRatePlanDeal(IdRatePlan, m_iHotelId)
                If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                    'hotelPayment.Checked = IIf(ds.Tables(0).Rows(0)("hotelPayment") Is DBNull.Value, False, True)
                    CheckBoxDeal.Checked = True
                    'tableConfDeal.Visible = True
                    txtInicio.Text = CType(ds.Tables(0).Rows(0)("fechaInicio"), String).Trim
                    txtFinal.Text = CType(ds.Tables(0).Rows(0)("fechaFin"), String).Trim

                    If ds.Tables(0).Rows(0)("horaInicio") Is DBNull.Value Then
                        CheckBoxDefHora.Checked = False
                        HoraInicio.Enabled = False
                        MinutoInicio.Enabled = False
                        HoraFin.Enabled = False
                        MinutoFin.Enabled = False
                    Else
                        CheckBoxDefHora.Checked = True
                        HoraInicio.Enabled = True
                        MinutoInicio.Enabled = True
                        HoraFin.Enabled = True
                        MinutoFin.Enabled = True
                        'Leemos la configuracion de la hora
                        HoraInicio.SelectedValue = CType(ds.Tables(0).Rows(0)("HoraInicio"), String).Substring(0, 2)
                        MinutoInicio.SelectedValue = CType(ds.Tables(0).Rows(0)("HoraInicio"), String).Substring(3, 2)
                        HoraFin.SelectedValue = CType(ds.Tables(0).Rows(0)("HoraFin"), String).Substring(0, 2)
                        MinutoFin.SelectedValue = CType(ds.Tables(0).Rows(0)("HoraFin"), String).Substring(3, 2)
                    End If
                Else
                    clearConfDealData()
                End If
            End With
            Dim strError As String = String.Empty
            dsAssignCountry = (New ClsAssingCountryRatePlan).GetAssignamentCountries(Me.m_iHotelId, IdRatePlan, strError)
            If dsAssignCountry.Tables.Count > 0 AndAlso dsAssignCountry.Tables(0).Rows.Count > 0 Then
                HasAssignamentCountry = True
                For Each Row As DataRow In dsAssignCountry.Tables(0).Rows
                    For Each country As ListItem In chklCountries.Items
                        If country.Value = Row.Item("idPais").ToString.Trim Then
                            country.Selected = True
                        End If
                    Next
                Next
            Else
                For Each country As ListItem In chklCountries.Items
                    country.Selected = True
                Next
            End If
        End If

        Me.Page.RegisterStartupScript("", "<script>ShowDivNegotiates('" & ddlSegmentos.ClientID & "','" & Me.txtRateCode.ClientID & _
         "','" & Me.CodigosTarifas & "','" & lblAccessCode.ClientID & "','" & txtAccessCode.ClientID & "','" & lblCD.ClientID & "','" & txtCD.ClientID & "','1'" & ");MostrarOcultarConf('" & tableConfDeal.ClientID & "','" & CheckBoxDeal.ClientID & "');</script>")

    End Sub

    Protected Property HasData() As Boolean
        Get
            HasData = False
            If Me.ViewState("HasData") IsNot Nothing Then HasData = Me.ViewState("HasData")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("HasData") = value
        End Set
    End Property

    Public Sub ClearData()
        Me.hotelPayment.Checked = False
        Me.portalMovil.Checked = False
        Me.onlyCC.Checked = False
        Me.HasData = False
        txtDescripcion.Limpia()
        Me.txtPromoDescription.Limpia()
        edicion = False
        Me.ddlSegmentos.SelectedIndex = 0
        Me.txtAccessCode.Text = ""
        Me.txtCD.Text = ""
        Me.txtRateCode.Enabled = True
        Me.txtRateCode.Text = Me.CodigosTarifas.Split("//")(2)
        Me.chkGDS.Checked = False
        Me.ddlSegmentos.Enabled = True
        Me.txtShortDescription.Limpia()
        Me.chkPortal.Checked = False
        Me.chkUnipantalla.Checked = False
        'Me.chkUnipantalla.Checked = False
        Me.chkADS.Checked = False
        txtDaysFree.Text = ""
        Me.txtDescProm.Text = ""
        Me.RdbNone.Checked = True
        Me.RdbOneNigth.Checked = False
        Me.rdbAlltotal.Checked = False
        Me.chkGDSAmadeus.Checked = False
        Me.chkGDSGalileo.Checked = False
        Me.chkGDSSabre.Checked = False
        Me.chkGDSWorldSpan.Checked = False
        Me.txtPorcGDS.Text = ""
        Me.txtPorcPOR.Text = ""
        Me.txtPorcUNI.Text = ""
        Me.txtPorcADS.Text = ""
        Me.chkWaitListAvailable.Checked = True
        cargarOrden()
        cargarDatosHotel()
        CargaContratos()
        Carga_Monedas()
        FillRatePlans()
        Me.lstRatePlans.SelectedIndex = 0
        For Each item As ListItem In chklCountries.Items
            item.Selected = False
        Next

        ctrPortal1.Limpiar()
        TextBoxTarifaComisionable.Text = 0
    End Sub

    'Public Function eliminarPortales(ByVal IdHotel As Integer, ByVal opcion As Integer, ByVal codigo As String) As Boolean
    '    ctrPortal1.EliminarPortales(IdHotel, opcion, codigo)
    'End Function

    Public Function eliminarPortales(ByVal IdHotel As Integer, ByVal codigo As String) As Boolean
        ctrPortal1.EliminarPortales(IdHotel, 3, codigo)
    End Function

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



    Public Sub SetMsgTarifasCom1(ByVal script As String)
        ddlContratosNR.Attributes.Add("onChange", script)
    End Sub

    Public Function validaHora() As String
        Dim script As String
        Dim par As String
        'se puso en una variable porque se metio otra funcion de validacion.
        script = "validaHora('" & CheckBoxDeal.ClientID & "','" & CheckBoxDefHora.ClientID & "','" & HoraInicio.ClientID & "','" & MinutoInicio.ClientID & "','" & HoraFin.ClientID & "','" & MinutoFin.ClientID & "','" & timevalidator.ClientID & "');"

        Return script
    End Function


    Private Sub Carga_Monedas()
        cmbMonedas.DataSource = (New MonedaSistema).GetMonedaListIdName
        cmbMonedas.DataTextField = "Nombre"
        cmbMonedas.DataValueField = "idMoneda"
        cmbMonedas.DataBind()
        cmbMonedas.Items.Insert(0, New ListItem("", "-1"))
    End Sub

    Private Sub SendRatesToService(ByVal ratesForRequest As RatesMessages, ByVal ratesForRequestPromotion As RatesMessages, ByVal endpoint As String, ByVal endpointDelete As String, ByVal hotelId As String, ByVal service As String)

        Dim pgBase As PaginaBase = New PaginaBase()

        Dim note As String = String.Format("Sincronizar ctrRatePlan {0}", service)
        Dim noteDelete As String = String.Format("Eliminar ctrRatePlan {0}", service)

        Dim noteException As String = String.Format("Sincronizar exception ctrRatePlan {0}", service)
        Dim noteDeleteException As String = String.Format("Eliminar exception ctrRatePlan {0}", service)

        Dim res As Tuple(Of RateResponse, RateResponse) = HotelUtilitie.ConfluxServiceHelper.UpdateRate(ratesForRequest, endpoint, endpointDelete)

        pgBase.guardalog("/Pages/RatesPlans.aspx", pgBase.acciones.Sincronizar, note, "", res.Item1.RequestXML, res.Item1.Xml, hotelId)

        If res.Item2 IsNot Nothing Then
            pgBase.guardalog("/Pages/RatesPlans.aspx", pgBase.acciones.Eliminar, noteDelete, "", res.Item2.RequestXML, res.Item2.Xml, hotelId)
        End If

        Dim resPromotion As Tuple(Of RateResponse, RateResponse) = HotelUtilitie.ConfluxServiceHelper.UpdateRate(ratesForRequestPromotion, endpoint, endpointDelete)
        pgBase.guardalog("/Pages/RatesPlans.aspx", pgBase.acciones.Sincronizar, noteException, "", resPromotion.Item1.RequestXML, resPromotion.Item1.Xml, hotelId)

        If resPromotion.Item2 IsNot Nothing Then
            pgBase.guardalog("/Pages/RatesPlans.aspx", pgBase.acciones.Eliminar, noteDeleteException, "", resPromotion.Item2.RequestXML, resPromotion.Item2.Xml, hotelId)
        End If

    End Sub

    Private Sub ExecuteServices(ByVal hotelId As Integer, ByVal companyId As Integer, ByVal idRatePlan As String, ByVal isEnabledGoogleRequest As Boolean, ByVal isEnabledSendingRatesAPICache As Boolean)
        Dim ratesForRequest As RatesMessages = HotelUtilitie.ConfluxServiceHelper.GetRateMessages(hotelId, idRatePlan, companyId, TypeRateEnum.RoomRate)
        Dim ratesForRequestPromotion As RatesMessages = HotelUtilitie.ConfluxServiceHelper.GetRateMessages(hotelId, idRatePlan, companyId, TypeRateEnum.RoomRatePromotion)

        If isEnabledGoogleRequest Then
            SendRatesToService(ratesForRequest, ratesForRequestPromotion, HotelUtilitie.ENDPOINT, HotelUtilitie.ENDPOINTDELETE, hotelId, "Conflux")
        End If

        If isEnabledSendingRatesAPICache Then
            SendRatesToService(ratesForRequest, ratesForRequestPromotion, HotelUtilitie.ENDPOINTAPI, HotelUtilitie.ENDPOINTAPIDELETE, hotelId, "APICache")
        End If
    End Sub


End Class
