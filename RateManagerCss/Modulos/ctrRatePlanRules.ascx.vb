Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade
Partial Class ctrRatePlanRules

    Inherits System.Web.UI.UserControl
    Private Enum cancelpolicy
        useDefault
        bydays
        byhour
        specifichour
    End Enum

    Public Property edicion() As Boolean
        Get
            Return ViewState("Edicion")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Edicion") = Value
        End Set
    End Property
    Public Property IdRule() As Integer
        Get
            Return ViewState("_idrule")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_idrule") = Value
        End Set
    End Property

    Public Property m_iHotelId() As Integer
        Get
            Return ViewState("me_hotel")
        End Get
        Set(ByVal Value As Integer)
            VIEWSTATE("me_hotel") = Value
        End Set
    End Property

    Private Property idDiccPCReview() As Integer
        Get
            Return ViewState("idDiccPCReview")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idDiccPCReview") = Value
        End Set
    End Property

    Private Property idDiccPCFull() As Integer
        Get
            Return ViewState("idDiccPCFull")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idDiccPCFull") = Value
        End Set
    End Property

    Private Property idDiccPoliticaGarantia() As Integer
        Get
            Return ViewState("idDiccPoliticaGarantia")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idDiccPoliticaGarantia") = Value
        End Set
    End Property

    Private Property idDiccPoliticaCreditCard() As Integer
        Get
            Return ViewState("idDiccPoliticaCreditCard")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idDiccPoliticaCreditCard") = Value
        End Set
    End Property



#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents RfvCancel As System.Web.UI.WebControls.RequiredFieldValidator
    Protected txtCancelPolitiesReview As CtrlIdioma
    Protected txtCancelPolitiesFull As CtrlIdioma
    Protected txtGuaranteePolicy As CtrlIdioma
    Protected txtPolicyCreditCard As CtrlIdioma

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
        Me.txtCancelPolitiesReview.RequiredText = Not Me.chkRateRules.Checked
        Me.txtCancelPolitiesFull.RequiredText = Not Me.chkRateRules.Checked
        Me.txtGuaranteePolicy.RequiredText = Not Me.chkRateRules.Checked
        Me.txtPolicyCreditCard.RequiredText = Not Me.chkRateRules.Checked

        If Not IsPostBack Then
            ddlCancelationPolicy.Items.Clear()
            ddlCancelationPolicy.Items.Insert(cancelpolicy.useDefault, PortalCulture.GetString("00796"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.bydays, PortalCulture.GetString("00020"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.byhour, PortalCulture.GetString("00021"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.specifichour, PortalCulture.GetString("00381"))
            fillGuar()
            Me.txtCancellationPolicy.Style.Add("display", "none")
            Me.ddlHour.Style.Add("display", "none")
            Me.lblSep.Style.Add("display", "none")
            Me.lblAux.Style.Add("display", "none")
            lblEDaysHour.Style.Add("display", "none")
            Me.ddlMinutes.Style.Add("display", "none")
            Me.lblAux.Text = PortalCulture.GetString("00413")
        End If
        'ddlCancelationPolicy.Attributes.Add("onchange", "javascript:LoadMsg('" & Me.ddlCancelationPolicy.ClientID _
        '& "','" & lblAux.ClientID & "','" & lblEDaysHour.ClientID & "','" & PortalCulture.GetString("00410") _
        '& "','" & PortalCulture.GetString("00409") & "','" & PortalCulture.GetString("00411") _
        '& "','" & PortalCulture.GetString("00412") & "','" & PortalCulture.GetString("00413") & "','" & Me.ddlHour.ClientID _
        '& "','" & Me.ddlMinutes.ClientID & "','" & Me.lblSep.ClientID & "','" & Me.txtCancellationPolicy.ClientID & "')")

        txtCancelPolitiesReview.IsMultiline = False
        'txtCancelPolitiesReview.RequiredText = False
        txtCancelPolitiesReview.MaxLength = 52
        txtCancelPolitiesReview.Width = 440
        'txtCancelPolitiesFull.RequiredText = False
        txtCancelPolitiesFull.Width = 440
    End Sub
    Public Function validaHours() As Boolean

        Dim valido As Boolean = True
        If Me.ddlCancelationPolicy.SelectedIndex <> cancelpolicy.specifichour Then
            If txtCancellationPolicy.Text.Length > 3 Then
                valido = False
            End If
            lblErrorHours.Text = "0-999"
        End If
        lblErrorHours.Visible = Not valido
        Return valido
    End Function
    Private Sub loadResources()
       
        lblEDescripcion.Text = PortalCulture.GetString("00002", True)
        lblERequired.Text = PortalCulture.GetString("00017", True)
        lblERateRules.Text = PortalCulture.GetString("00018", True)
        lbltitlepolity.Text = PortalCulture.GetString("00019")
        lblEDaysHour.Text = PortalCulture.GetString("00022")
        chkRateRules.Text = PortalCulture.GetString("00023")
        lblEVerReq.Text = PortalCulture.GetString("00026", True)
        ddlGuarDep.Items(0).Text = PortalCulture.GetString("00027")
        ddlGuarDep.Items(0).Value = "N"
        ddlGuarDep.Items(1).Text = PortalCulture.GetString("00028")
        ddlGuarDep.Items(1).Value = "G"
        ddlGuarDep.Items(2).Text = PortalCulture.GetString("00029")
        ddlGuarDep.Items(2).Value = "D"
        ddlVerReq.Items(0).Text = PortalCulture.GetString("00030")
        ddlVerReq.Items(0).Value = True
        ddlVerReq.Items(1).Text = PortalCulture.GetString("00031")
        ddlVerReq.Items(1).Value = False
        lblAdvanceddays.Text = PortalCulture.GetString("00119", True)
        lblNoArrivals.Text = PortalCulture.GetString("00120", True)
        lblMaxNights.Text = PortalCulture.GetString("00118", True)
        lblMinNights.Text = PortalCulture.GetString("00117", True)
        Me.lblLunes.Text = PortalCulture.GetString("00300")
        Me.lblMartes.Text = PortalCulture.GetString("00301")
        Me.lblMiercoles.Text = PortalCulture.GetString("00302")
        Me.lblJueves.Text = PortalCulture.GetString("00303")
        Me.lblViernes.Text = PortalCulture.GetString("00304")
        Me.lblSabado.Text = PortalCulture.GetString("00305")
        Me.lbldomingo.Text = PortalCulture.GetString("00306")
        ddlCancelationPolicy.Items(cancelpolicy.useDefault).Text = PortalCulture.GetString("M000640")
        ddlCancelationPolicy.Items(cancelpolicy.bydays).Text = PortalCulture.GetString("00020")
        ddlCancelationPolicy.Items(cancelpolicy.byhour).Text = PortalCulture.GetString("00021")
        ddlCancelationPolicy.Items(cancelpolicy.specifichour).Text = PortalCulture.GetString("00381")
        If ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays Then
            Me.lblAux.Text = PortalCulture.GetString("00413")
            lblEDaysHour.Text = PortalCulture.GetString("00410")
        ElseIf ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour Then
            Me.lblAux.Text = PortalCulture.GetString("00413")
            lblEDaysHour.Text = PortalCulture.GetString("00409")
        Else
            Me.lblAux.Text = PortalCulture.GetString("00412")
            lblEDaysHour.Text = PortalCulture.GetString("00411")
        End If
        Me.lblCancel.Text = PortalCulture.GetString("00413", True)

        lblCancelPolitiesFull.Text = PortalCulture.GetString("M000536", True)
        lblCancelPolitiesReview.Text = PortalCulture.GetString("M000527", True)

        lblTtitleGuarantee.Text = PortalCulture.GetString("M000528", False)
        lblGuaranteePolicy.Text = PortalCulture.GetString("M000528", False)
        lblPolicyCreditCard.Text = PortalCulture.GetString("M000529", False)
    End Sub

    'función que llena la lista de la garantía ó depósito
    Private Sub fillGuar()
        ddlGuarDep.Items.Clear()
        ddlGuarDep.Items.Insert(0, "N")
        ddlGuarDep.Items.Insert(1, "G")
        ddlGuarDep.Items.Insert(2, "D")
        ddlGuarDep.Items(0).Text = "Ninguna"
        ddlGuarDep.Items(0).Value = "N"
        ddlGuarDep.Items(1).Text = "Garantía"
        ddlGuarDep.Items(1).Value = "G"
        ddlGuarDep.Items(2).Text = "Depósito"
        ddlGuarDep.Items(2).Value = "D"
        ddlVerReq.Items.Clear()
        ddlVerReq.Items.Insert(0, "Y")
        ddlVerReq.Items.Insert(0, "N")
        ddlVerReq.Items(0).Text = "Yes"
        ddlVerReq.Items(0).Value = True
        ddlVerReq.Items(1).Text = "No"
        ddlVerReq.Items(1).Value = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
    End Sub

    Public Sub clearData()
        edicion = False
        txtCancellationPolicy.Text = ""
        chkRateRules.Checked = False
        ddlCancelationPolicy.SelectedIndex = cancelpolicy.useDefault
        Me.txtCancellationPolicy.Style.Add("display", "none")
        Me.ddlHour.Style.Add("display", "none")
        Me.lblSep.Style.Add("display", "none")
        Me.ddlMinutes.Style.Add("display", "none")
        Me.lblAux.Style.Add("display", "none")
        lblEDaysHour.Style.Add("display", "none")
        ddlGuarDep.SelectedIndex = 0
        txtDescription.Text = ""
        txtAdvancedDays.Text = ""
        txtMaxAdvancedDays.Text = ""
        txtMaxNights.Text = ""
        txtMinNights.Text = ""
        lblAux.Text = ""
        Me.varCancelationTime.Value = ""

        Dim ck As CheckBox
        For i As Integer = 1 To 7
            ck = Me.FindControl("Chk" & i)
            ck.Checked = False
        Next
        lblErrorHours.Visible = False

        idDiccPCReview = 0
        idDiccPCFull = 0
        txtCancelPolitiesReview.Limpia()
        txtCancelPolitiesFull.Limpia()
        txtGuaranteePolicy.Limpia()
        txtPolicyCreditCard.Limpia()
        idDiccPoliticaGarantia = 0
        idDiccPoliticaCreditCard = 0

    End Sub

    Function getDataXML() As String
        Dim ds As New RatesPlanRulesData
        With New RatesPlanRulesFacade
            ds = .getById(IdRule)
        End With
        Return Util.Utility.GetXml(ds.TABLE_RATEPLANRULES, "UpdateRatesPlanRules", ds)
    End Function

    Public Function SaveRules(ByVal publish As Boolean) As Boolean
        Dim ds As New RatesPlanRulesData
        Dim dr As DataRow
        Dim sData As String = ""
        Dim sDataPrev As String = ""

        sDataPrev = getDataXML()
        dr = ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).NewRow

        With dr
            .Item(RatesPlanRulesData.FIELD_CancelDays) = System.DBNull.Value
            .Item(RatesPlanRulesData.FIELD_CancelHours) = System.DBNull.Value
            .Item(RatesPlanRulesData.FIELD_CancelSpecificHour) = System.DBNull.Value
            If chkNonCancelable.Checked Then
                .Item(RatesPlanRulesData.FIELD_CancelDays) = 0
            Else
                If ddlCancelationPolicy.SelectedIndex <> cancelpolicy.useDefault Then
                    If Me.ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays AndAlso txtCancellationPolicy.Text <> "" Then
                        .Item(RatesPlanRulesData.FIELD_CancelDays) = CInt(Val(Me.txtCancellationPolicy.Text))
                    ElseIf Me.ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour AndAlso txtCancellationPolicy.Text <> "" Then
                        .Item(RatesPlanRulesData.FIELD_CancelHours) = CInt(Val(txtCancellationPolicy.Text))
                    Else
                        .Item(RatesPlanRulesData.FIELD_CancelSpecificHour) = Me.ddlHour.SelectedValue.ToString & Me.ddlMinutes.SelectedValue.ToString  'Me.txtCancellationPolicy.Text
                    End If
                End If
            End If

            .Item(RatesPlanRulesData.FIELD_DepNight) = System.DBNull.Value
            .Item(RatesPlanRulesData.FIELD_DepPerc) = System.DBNull.Value
            .Item(RatesPlanRulesData.FIELD_IDHOTEL) = m_iHotelId
            .Item(RatesPlanRulesData.FIELD_GuarDep) = ddlGuarDep.SelectedValue
            .Item(RatesPlanRulesData.FIELD_IDRULE) = 0
            .Item(RatesPlanRulesData.FIELD_RateRulesDef) = chkRateRules.Checked
            .Item(RatesPlanRulesData.FIELD_ReqVerif) = ddlVerReq.SelectedValue
            .Item(RatesPlanRulesData.FIELD_DESCRIPTION) = txtDescription.Text
            If txtAdvancedDays.Text <> "" Then
                .Item(RatesPlanRulesData.FIELD_ADVBOOKING) = CInt(Val(txtAdvancedDays.Text))
            End If
            If txtMaxAdvancedDays.Text <> "" Then
                .Item(RatesPlanRulesData.FIELD_MAXADVBOOKING) = CInt(Val(txtMaxAdvancedDays.Text))
            End If
            If txtMaxNights.Text <> "" Then
                .Item(RatesPlanRulesData.FIELD_MAXDIAS) = CInt(Val(txtMaxNights.Text))
            End If
            If txtMinNights.Text <> "" Then
                .Item(RatesPlanRulesData.FIELD_MINDIAS) = CInt(Val(txtMinNights.Text))
            End If
            If GetArrivosField() <> "NNNNNNN" Then
                .Item(RatesPlanRulesData.FIELD_NOARRIVOS) = GetArrivosField()
            End If

            Dim CancelPolities As String
            CancelPolities = txtCancelPolitiesReview.textodefault.PadRight(52)
            If txtCancelPolitiesFull.textodefault.Length > 184 Then
                .Item(RatesPlanRulesData.FIELD_PoliticaCancelacion) = CancelPolities & txtCancelPolitiesFull.textodefault.Substring(0, 184)
            Else
                .Item(RatesPlanRulesData.FIELD_PoliticaCancelacion) = CancelPolities & txtCancelPolitiesFull.textodefault
            End If

            If idDiccPCReview = 0 Then
                If validaTexto(txtCancelPolitiesReview.GetES, txtCancelPolitiesReview.GetEN) Then
                    .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview) = txtCancelPolitiesReview.Insert()
                End If
            Else
                .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview) = idDiccPCReview
            End If

            If idDiccPCFull = 0 Then
                If validaTexto(txtCancelPolitiesFull.GetES, txtCancelPolitiesFull.GetEN) Then
                    .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull) = txtCancelPolitiesFull.Insert()
                End If
            Else
                .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull) = idDiccPCFull
            End If


            Dim GuaranteePolicy As String
            GuaranteePolicy = txtGuaranteePolicy.textodefault
            If txtGuaranteePolicy.textodefault.Length > 250 Then
                .Item(RatesPlanRulesData.FIELD_PoliticaGarantia) = GuaranteePolicy.Substring(0, 250)
            Else
                .Item(RatesPlanRulesData.FIELD_PoliticaGarantia) = GuaranteePolicy
            End If

            If idDiccPoliticaGarantia = 0 Then
                If validaTexto(txtGuaranteePolicy.GetES, txtGuaranteePolicy.GetEN) Then
                    .Item(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia) = txtGuaranteePolicy.Insert()
                End If
            Else
                .Item(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia) = idDiccPoliticaGarantia
            End If

            Dim PoliticaCreditCard As String
            PoliticaCreditCard = txtPolicyCreditCard.textodefault
            If txtPolicyCreditCard.textodefault.Length > 250 Then
                .Item(RatesPlanRulesData.FIELD_PoliticaCreditCard) = PoliticaCreditCard.Substring(0, 250)
            Else
                .Item(RatesPlanRulesData.FIELD_PoliticaCreditCard) = PoliticaCreditCard
            End If

            If idDiccPoliticaCreditCard = 0 Then
                If validaTexto(txtPolicyCreditCard.GetES, txtPolicyCreditCard.GetEN) Then
                    .Item(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard) = txtPolicyCreditCard.Insert()
                End If
            Else
                .Item(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard) = idDiccPoliticaCreditCard
            End If


        End With
        ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).Rows.Add(dr)
        Dim sw As Boolean
        If edicion = False Then
            With New RatesPlanRulesFacade
                sw = .Insert(ds)
                If sw Then
                    sDataPrev = ""
                    sData = Util.Utility.GetXml(ds.TABLE_RATEPLANRULES, "UpdateRatesPlanRules", ds)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/RatePlansRules.aspx", PaginaBase.acciones.Crear, "Se creó la regla " & txtDescription.Text, "", sDataPrev, sData)

                    If publish Then
                        CType(Me.Page, PaginaBase).guardalog("/Pages/RatePlansRules.aspx", PaginaBase.acciones.Publicar, "Se modificó la regla " & txtDescription.Text, "", sDataPrev, sData)
                    End If

                    If Me.txtCancelPolitiesReview.HasChanges OrElse Me.txtCancelPolitiesFull.HasChanges OrElse Me.txtGuaranteePolicy.HasChanges OrElse Me.txtPolicyCreditCard.HasChanges Then
                        CType(Me.Page, PaginaBase).NotifyContentModification("Regla de plan tarifario " & txtDescription.Text, "Reglas De Plan Tarifario")
                    End If

                    If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview) Then
                        txtCancelPolitiesReview.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview), publish)
                    End If

                    If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull) Then
                        txtCancelPolitiesFull.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull), publish)
                    End If

                    If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia) Then
                        txtGuaranteePolicy.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia), publish)
                    End If

                    If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard) Then
                        txtPolicyCreditCard.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard), publish)
                    End If

                End If

                Return sw
            End With
        Else
            ds.AcceptChanges()
            ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).Rows(0).Item(RatesPlanRulesData.FIELD_IDRULE) = Me.IdRule

            With New RatesPlanRulesFacade
                sw = .Update(ds)
                If sw Then
                    sData = Util.Utility.GetXml(ds.TABLE_RATEPLANRULES, "UpdateRatesPlanRules", ds)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/RatePlansRules.aspx", If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar), "Se modificó la regla " & txtDescription.Text, "", sDataPrev, sData)

                    If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview) Then
                        'txtCancelPolitiesReview.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview), validaTexto(False, txtCancelPolitiesReview, dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview)), validaTexto(True, txtCancelPolitiesReview, dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview)), publish)
                        txtCancelPolitiesReview.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview), publish)
                    End If

                    If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull) Then
                        'txtCancelPolitiesFull.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull), validaTexto(False, txtCancelPolitiesFull, dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull)), validaTexto(True, txtCancelPolitiesFull, dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull)), publish)
                        txtCancelPolitiesFull.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull), publish)
                    End If

                    If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia) Then
                        'txtGuaranteePolicy.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia), validaTexto(False, txtGuaranteePolicy, dr.Item(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia)), validaTexto(True, txtGuaranteePolicy, dr.Item(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia)), publish)
                        txtGuaranteePolicy.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia), publish)
                    End If

                    If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard) Then
                        'txtPolicyCreditCard.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard), validaTexto(False, txtPolicyCreditCard, dr.Item(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard)), validaTexto(True, txtPolicyCreditCard, dr.Item(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard)), publish)
                        txtPolicyCreditCard.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard), publish)
                    End If

                    If Me.txtCancelPolitiesReview.HasChanges OrElse Me.txtCancelPolitiesFull.HasChanges OrElse Me.txtGuaranteePolicy.HasChanges OrElse Me.txtPolicyCreditCard.HasChanges Then
                        CType(Me.Page, PaginaBase).NotifyContentModification("Regla de plan tarifario " & txtDescription.Text, "Reglas De Plan Tarifario")
                    End If

                End If

                Return sw
            End With
        End If
    End Function

    Public Sub loadRule(ByVal rule As Integer)
        clearData()
        Dim ds As New RatesPlanRulesData
        With New RatesPlanRulesFacade
            ds = .getById(rule)
        End With
        Me.txtCancellationPolicy.Style.Add("display", "none")
        Me.ddlHour.Style.Add("display", "none")
        Me.lblSep.Style.Add("display", "none")
        Me.ddlMinutes.Style.Add("display", "none")

        Me.lblAux.Style.Add("display", "none")
        lblEDaysHour.Style.Add("display", "none")

        If ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).Rows.Count > 0 Then
            Me.IdRule = rule
            txtCancellationPolicy.Text = ""
            Dim timeObject As String = "{type:"
            With ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).Rows(0)


                If Not .IsNull(RatesPlanRulesData.FIELD_CancelDays) Then
                    Me.txtCancellationPolicy.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_CancelDays)))
                    If CInt(Val(.Item(RatesPlanRulesData.FIELD_CancelDays))) = 0 Then
                        chkNonCancelable.Checked = True
                    End If
                    ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays

                    timeObject += Me.ddlCancelationPolicy.SelectedIndex.ToString() + ", value:'" + Me.txtCancellationPolicy.Text + "'}"

                    Me.lblAux.Text = ""
                    lblEDaysHour.Text = PortalCulture.GetString("00410")
                    Me.txtCancellationPolicy.Style.Add("display", "")
                    Me.lblAux.Style.Add("display", "")
                    lblEDaysHour.Style.Add("display", "")
                ElseIf Not .IsNull(RatesPlanRulesData.FIELD_CancelHours) Then
                    txtCancellationPolicy.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_CancelHours).ToString))
                    ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour

                    timeObject += Me.ddlCancelationPolicy.SelectedIndex.ToString() + ", value:'" + Me.txtCancellationPolicy.Text + "'}"

                    Me.lblAux.Text = ""
                    lblEDaysHour.Text = PortalCulture.GetString("00409")
                    Me.txtCancellationPolicy.Style.Add("display", "")
                    Me.lblAux.Style.Add("display", "")
                    lblEDaysHour.Style.Add("display", "")
                ElseIf Not .IsNull(RatesPlanRulesData.FIELD_CancelSpecificHour) Then
                    ddlCancelationPolicy.SelectedIndex = cancelpolicy.specifichour
                    Me.lblAux.Text = PortalCulture.GetString("00412")
                    lblEDaysHour.Text = PortalCulture.GetString("00411")
                    Me.txtCancellationPolicy.Style.Add("display", "none")
                    Me.ddlHour.Style.Add("display", "")
                    Me.ddlMinutes.Style.Add("display", "")
                    Me.lblSep.Style.Add("display", "")
                    Me.lblAux.Style.Add("display", "")
                    lblEDaysHour.Style.Add("display", "")
                    Try
                        Dim hora As String = .Item(RatesPlanRulesData.FIELD_CancelSpecificHour).ToString
                        Me.ddlHour.SelectedValue = hora.Substring(0, 2)
                        If CInt(hora.Substring(2, 2)) < 15 Then
                            Me.ddlMinutes.SelectedValue = "00"
                        ElseIf CInt(hora.Substring(2, 2)) < 30 Then
                            Me.ddlMinutes.SelectedValue = "15"
                        ElseIf CInt(hora.Substring(2, 2)) < 45 Then
                            Me.ddlMinutes.SelectedValue = "30"
                        Else
                            Me.ddlMinutes.SelectedValue = "45"
                        End If
                    Catch ex As Exception
                        Me.ddlHour.SelectedValue = "01"
                        Me.ddlMinutes.SelectedValue = "00"
                    End Try
                    timeObject += Me.ddlCancelationPolicy.SelectedIndex.ToString() + ", value:'" + Me.ddlHour.SelectedValue + ":" + Me.ddlMinutes.SelectedValue + "'}"
                Else
                    timeObject += "0,value:''}"
                End If

                Me.varCancelationTime.Value = timeObject

                '****************************Politicas de garantia******************************
                Dim PoliticaGarantia As String
                If Not .IsNull(RatesPlanRulesData.FIELD_PoliticaGarantia) Then
                    PoliticaGarantia = .Item(RatesPlanRulesData.FIELD_PoliticaGarantia).ToString
                Else
                    PoliticaGarantia = ""
                End If
                txtGuaranteePolicy.textodefault = PoliticaGarantia

                If Not .IsNull(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia) Then
                    txtGuaranteePolicy.CargaDatos(.Item(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia))
                    'If txtGuaranteePolicy.GetEN <> Nothing Then
                    '    txtGuaranteePolicy.SetEN(txtGuaranteePolicy.GetEN.Trim)
                    'End If

                    'If txtGuaranteePolicy.GetES <> Nothing Then
                    '    txtGuaranteePolicy.setES(txtGuaranteePolicy.GetES.Trim)
                    'End If

                    idDiccPoliticaGarantia = .Item(RatesPlanRulesData.FIELD_idDiccPoliticaGarantia)
                Else
                    idDiccPoliticaGarantia = 0
                End If

                'Politica de garantia de la tarjeta de credito
                Dim PoliticaCreditCard As String
                If Not .IsNull(RatesPlanRulesData.FIELD_PoliticaCreditCard) Then
                    PoliticaCreditCard = .Item(RatesPlanRulesData.FIELD_PoliticaCreditCard).ToString
                Else
                    PoliticaCreditCard = ""
                End If
                txtPolicyCreditCard.textodefault = PoliticaCreditCard

                If Not .IsNull(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard) Then
                    txtPolicyCreditCard.CargaDatos(.Item(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard))
                    'If txtPolicyCreditCard.GetEN <> Nothing Then
                    '    txtPolicyCreditCard.SetEN(txtPolicyCreditCard.GetEN.Trim)
                    'End If

                    'If txtPolicyCreditCard.GetES <> Nothing Then
                    '    txtPolicyCreditCard.setES(txtPolicyCreditCard.GetES.Trim)
                    'End If

                    idDiccPoliticaCreditCard = .Item(RatesPlanRulesData.FIELD_idDiccPoliticaCreditCard)
                Else
                    idDiccPoliticaCreditCard = 0
                End If

                '*******************************************************************************


                Dim cancelpolities As String
                If Not .IsNull(RatesPlanRulesData.FIELD_PoliticaCancelacion) Then
                    cancelpolities = .Item(RatesPlanRulesData.FIELD_PoliticaCancelacion).ToString
                Else
                    cancelpolities = ""
                End If

                txtCancelPolitiesReview.textodefault = cancelpolities

                If Not .IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview) Then
                    txtCancelPolitiesReview.CargaDatos(.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview))
                    'If txtCancelPolitiesReview.GetEN <> Nothing Then
                    '    txtCancelPolitiesReview.SetEN(txtCancelPolitiesReview.GetEN.Trim)
                    'End If

                    'If txtCancelPolitiesReview.GetES <> Nothing Then
                    '    txtCancelPolitiesReview.setES(txtCancelPolitiesReview.GetES.Trim)
                    'End If

                    idDiccPCReview = .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview)
                Else
                    idDiccPCReview = 0
                End If

                If Not .IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull) Then
                    txtCancelPolitiesFull.CargaDatos(.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull))

                    'If txtCancelPolitiesFull.GetEN <> Nothing Then
                    '    txtCancelPolitiesFull.SetEN(txtCancelPolitiesFull.GetEN.Trim)
                    'End If

                    'If txtCancelPolitiesFull.GetES <> Nothing Then
                    '    txtCancelPolitiesFull.setES(txtCancelPolitiesFull.GetES.Trim)
                    'End If

                    idDiccPCFull = .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull)
                Else
                    idDiccPCFull = 0
                End If

                'If cancelpolities.Length > 52 Then
                '    txtCancelPolitiesReview.SetEN((cancelpolities.Substring(0, 52)).Trim)
                '    txtCancelPolitiesFull.SetEN((cancelpolities.Substring(52)).Trim)
                'Else
                '    txtCancelPolitiesReview.SetEN(cancelpolities.Trim)
                'End If

                Try
                    ddlGuarDep.SelectedValue = .Item(RatesPlanRulesData.FIELD_GuarDep)
                Catch ex As Exception
                    ddlGuarDep.SelectedValue = "N"
                End Try
                Me.txtDescription.Text = .Item(RatesPlanRulesData.FIELD_DESCRIPTION)
                If Not .IsNull(RatesPlanRulesData.FIELD_RateRulesDef) Then
                    chkRateRules.Checked = .Item(RatesPlanRulesData.FIELD_RateRulesDef)
                    Me.txtPolicyCreditCard.RequiredText = Not chkRateRules.Checked
                    Me.txtGuaranteePolicy.RequiredText = Not chkRateRules.Checked
                    Me.txtCancelPolitiesFull.RequiredText = Not chkRateRules.Checked
                    Me.txtCancelPolitiesReview.RequiredText = Not chkRateRules.Checked
                End If

                Try
                    ddlVerReq.SelectedValue = .Item(RatesPlanRulesData.FIELD_ReqVerif)
                Catch ex As Exception
                    ddlVerReq.SelectedValue = False
                End Try
                txtMinNights.Text = ""
                txtMaxNights.Text = ""
                txtAdvancedDays.Text = ""
                txtMaxAdvancedDays.Text = ""
                If Not .IsNull(RatesPlanRulesData.FIELD_MINDIAS) Then
                    txtMinNights.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_MINDIAS).ToString))
                End If
                If Not .IsNull(RatesPlanRulesData.FIELD_MAXDIAS) Then
                    txtMaxNights.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_MAXDIAS).ToString))
                End If

                If Not .IsNull(RatesPlanRulesData.FIELD_ADVBOOKING) Then
                    txtAdvancedDays.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_ADVBOOKING).ToString))
                End If
                If Not .IsNull(RatesPlanRulesData.FIELD_MAXADVBOOKING) Then
                    txtMaxAdvancedDays.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_MAXADVBOOKING).ToString))
                End If
                getNoArrrivalsField(.Item(RatesPlanRulesData.FIELD_NOARRIVOS).ToString)
            End With
        End If
    End Sub

    Private Sub getNoArrrivalsField(ByVal Field As String)
        Dim ck As CheckBox
        If Field <> "" Then
            For i As Integer = 1 To 7
                ck = FindControl("Chk" & i)
                ck.Checked = False
                Try
                    If Field.Substring(i - 1, 1) = "Y" Then
                        ck.Checked = True
                    End If
                Catch ex As Exception
                End Try
            Next
        End If
    End Sub

    Private Function GetArrivosField() As String
        Dim NoArr As String = ""
        Dim ck As CheckBox
        Dim cheked As String
        For i As Integer = 1 To 7
            ck = Me.FindControl("Chk" & i)
            cheked = "Y"
            If Not ck.Checked Then cheked = "N"
            NoArr &= cheked
        Next
        Return NoArr
    End Function

    Public Function validaTexto(ByVal bIdioma As Boolean, ByVal txtControlIdioma As CtrlIdioma, ByVal idDicc As Integer) As String
        Dim texto As String
        Dim auxControlIdioma As CtrlIdioma = New CtrlIdioma
        Dim auxTxtIng As String = ""
        Dim auxTxtEsp As String = ""

        auxControlIdioma.CargaDatosAuxiliares(idDicc, auxTxtIng, auxTxtEsp)

        If bIdioma = True Then 'valida Idioma Español
            If txtControlIdioma.GetES = Nothing Then
                texto = " "
            Else
                If txtControlIdioma.GetES.Trim = "" Then
                    texto = " "
                Else
                    texto = txtControlIdioma.GetES
                End If
            End If

            If texto = " " Then
                If auxTxtEsp = Nothing Then
                    texto = Nothing
                End If
            End If
        Else 'valida Idioma Ingles
            If txtControlIdioma.GetEN = Nothing Then
                texto = " "
            Else
                If txtControlIdioma.GetEN.Trim = "" Then
                    texto = " "
                Else
                    texto = txtControlIdioma.GetEN
                End If
            End If

            If texto = " " Then
                If auxTxtIng = Nothing Then
                    texto = Nothing
                End If
            End If
        End If

        Return texto
    End Function

    Public Function validaTexto(ByVal txtEsp As String, ByVal txtIng As String) As Boolean
        Dim value As Boolean = False

        If txtEsp <> Nothing Then
            If txtEsp.Trim <> "" Then
                value = True
            End If
        End If

        If value = False Then
            If txtIng <> Nothing Then
                If txtIng.Trim <> "" Then
                    value = True
                End If
            End If
        End If

        Return value
    End Function

End Class
