Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports APIServices.Helpers.Rate

Partial Class ctrRatePlanLink
    Inherits System.Web.UI.UserControl

    Public Property Editar() As Boolean
        Get
            Return ViewState("Edit")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Edit") = Value
            If ViewState("Edit") = True Then
                txtTarget.Visible = True
                Me.ddlRatePlanTarget.Visible = False
            Else
                txtTarget.Visible = False
                Me.ddlRatePlanTarget.Visible = True
            End If

        End Set
    End Property
    Public Property m_iHotelId() As Integer
        Get
            Return ViewState("me_hotel")
        End Get
        Set(ByVal Value As Integer)
            ViewState("me_hotel") = Value
        End Set
    End Property

    Public Property Target() As String
        Get
            Return If(ViewState("_Target") Is Nothing, "", ViewState("_Target"))
        End Get
        Set(ByVal Value As String)
            ViewState("_Target") = Value
        End Set
    End Property

    Public Property TargetNombre() As String
        Get
            Return If(ViewState("_TargetNombre") Is Nothing, "", ViewState("_TargetNombre"))
        End Get
        Set(ByVal Value As String)
            ViewState("_TargetNombre") = Value
        End Set
    End Property

    Public Property Targetcode() As String
        Get
            Return ViewState("_TargetC")
        End Get
        Set(ByVal Value As String)
            ViewState("_TargetC") = Value
        End Set
    End Property
    Public Property Sourcecode() As String
        Get
            Return ViewState("_SourceC")
        End Get
        Set(ByVal Value As String)
            ViewState("_SourceC") = Value
        End Set
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

    'Public Property SourceRateName() As String
    '    Get
    '        Return viewstate("_SRN")

    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_SRN") = Value
    '    End Set
    'End Property

    'Public Property targetRateName() As String
    '    Get
    '        Return viewstate("_SRNT")

    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_SRNT") = Value
    '    End Set
    'End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            Me.txtDivVisible.Text = "1"
        End If
        hplhide.NavigateUrl = "javascript:Ocultar('0','" & hplShow.ClientID & "','" & hplhide.ClientID & "','" & divDatos.ClientID & "','" & Me.txtRatio.ClientID & "','" & Me.txtoffset.ClientID & "','" & Me.divDatos.ClientID & "','" & divratio.ClientID & "','" & divoffset.ClientID & "','" & Me.lbl_Offset.ClientID & "','" & Me.lbl_Ratio.ClientID & "','" & Me.txtDivVisible.ClientID & "');"
        hplShow.NavigateUrl = "javascript:Ocultar('1','" & hplShow.ClientID & "','" & hplhide.ClientID & "','" & divDatos.ClientID & "','" & Me.txtRatio.ClientID & "','" & Me.txtoffset.ClientID & "','" & Me.divDatos.ClientID & "','" & divratio.ClientID & "','" & divoffset.ClientID & "','" & Me.lbl_Offset.ClientID & "','" & Me.lbl_Ratio.ClientID & "','" & Me.txtDivVisible.ClientID & "');"

        Me.txtoffset.Attributes.Add("onChange", "javascript:FillPrices('" & Me.divDatos.ClientID & "','Offset'" & ",'" & txtoffset.ClientID & "')")
        Me.txtRatio.Attributes.Add("onChange", "javascript:FillPrices('" & Me.divDatos.ClientID & "','Ratio'" & ",'" & txtRatio.ClientID & "')")
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadculture()
        If Me.txtDivVisible.Text = "1" Then
            'hplShow.Style.Add("display", "block")
            divratio.Style.Add("display", "block")
            divoffset.Style.Add("display", "block")
            lbl_Offset.Style.Add("display", "block")
            lbl_Ratio.Style.Add("display", "block")
            'hplhide.Style.Add("display", "none")
            divDatos.Style.Add("display", "none")
        Else

            'hplShow.Style.Add("display", "none")
            divratio.Style.Add("display", "none")
            divoffset.Style.Add("display", "none")
            lbl_Offset.Style.Add("display", "none")
            lbl_Ratio.Style.Add("display", "none")
            'hplhide.Style.Add("display", "block")
            divDatos.Style.Add("display", "block")
        End If
        'Me.ddlRatePlanSource.Attributes.Add("onChange", "javascript:showRatePlan2('" & Me.ddlRatePlanSource.ClientID & "','" & Me.SourceRateName & "','" & lblSRatePlan.ClientID & "')")
        'Me.ddlRatePlanTarget.Attributes.Add("onChange", "javascript:showRatePlan2('" & Me.ddlRatePlanTarget.ClientID & "','" & Me.targetRateName & "','" & lblTRateplan.ClientID & "')")
    End Sub
    Public Sub SourceDataSet(ByVal Value As DataSet)
        Me.ddlRatePlanSource.DataTextField = "texto" 'RatePlanData.FIELD_CODIGOTARIFA
        Me.ddlRatePlanSource.DataValueField = RatePlanData.FIELD_IDRATEPLAN
        Me.ddlRatePlanSource.DataSource = Value
        Me.ddlRatePlanSource.DataBind()

        'For j As Integer = 0 To Value.Tables(Value.RATEPLAN_TABLE).Rows.Count - 1
        '    Me.SourceRateName &= "//" & Value.Tables(Value.RATEPLAN_TABLE).Rows(j).Item(Value.FIELD_NAME).ToString
        '    lblSRatePlan.Text = "" & Value.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0)(Value.FIELD_NAME)
        'Next

        If Me.ddlRatePlanSource.Items.Count > 0 Then
            Me.ddlRatePlanSource.SelectedIndex = 0
        End If
    End Sub
    Public Sub TargetDataSet(ByVal Value As DataSet)

        If Not Value Is Nothing AndAlso Value.Tables.Count > 0 AndAlso Value.Tables(0).Rows.Count > 0 Then
            Me.ddlRatePlanTarget.DataTextField = "texto" 'RatePlanData.FIELD_CODIGOTARIFA
            Me.ddlRatePlanTarget.DataValueField = RatePlanData.FIELD_IDRATEPLAN
            Me.ddlRatePlanTarget.DataSource = Value
            Me.ddlRatePlanTarget.DataBind()
        End If

        'For j As Integer = 0 To Value.Tables(Value.RATEPLAN_TABLE).Rows.Count - 1
        '    Me.targetRateName &= "//" & Value.Tables(Value.RATEPLAN_TABLE).Rows(j).Item(Value.FIELD_NAME).ToString
        '    lblTRateplan.Text = "" & Value.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0)(Value.FIELD_NAME)
        'Next

        If Me.ddlRatePlanTarget.Items.Count > 0 Then
            Me.ddlRatePlanTarget.SelectedIndex = 0
        End If

    End Sub


    Public Function samelink() As Boolean
        If Me.ddlRatePlanSource.SelectedValue = Me.ddlRatePlanTarget.SelectedValue Then
            Me.lblNoLink.Visible = True
            Return True
        Else
            Me.lblNoLink.Visible = False
            Return False
        End If
    End Function

    Private Sub loadculture()

        Me.lbl_Offset.Text = PortalCulture.GetString("00037", True)
        Me.lbl_Ratio.Text = PortalCulture.GetString("00036", True)
        Me.lblEExtraChildRate.Text = PortalCulture.GetString("00041", True)
        Me.lblEExtraAdultRate.Text = PortalCulture.GetString("00040", True)
        Me.lblETwoPersonRAte.Text = PortalCulture.GetString("00039", True)
        Me.lblEOnePersonRate.Text = PortalCulture.GetString("00038", True)
        Me.lblOffset.Text = PortalCulture.GetString("00037")
        Me.lbOffset.Text = PortalCulture.GetString("00037")
        Me.lblRatio.Text = PortalCulture.GetString("00036")

        Me.lblERatePlanSource.Text = PortalCulture.GetString("00482", True)
        Me.lblERatePlanTarget.Text = PortalCulture.GetString("00483", True)

        lblOthersOcupation.Text = PortalCulture.GetString("00044", True)

        RVOnePersonOffset.ErrorMessage = PortalCulture.GetString("00045")
        RVTwoPersonOffset.ErrorMessage = PortalCulture.GetString("00045")
        RVExtraAdultOffset.ErrorMessage = PortalCulture.GetString("00045")
        RVExtraChildOffset.ErrorMessage = PortalCulture.GetString("00045")
        RVOthersOffset.ErrorMessage = PortalCulture.GetString("00045")
        RVOffset.ErrorMessage = PortalCulture.GetString("00045")
        Me.lblNoLink.Text = PortalCulture.GetString("00046")
        hplShow.Text = PortalCulture.GetString("00319")
        hplhide.Text = PortalCulture.GetString("00318")
        lblSoldOut.Text = PortalCulture.GetString("00320", True)
        lbllinkdata.Text = PortalCulture.GetString("00418")
        rfvSoldPercent.Text = PortalCulture.GetString("00071")
    End Sub
    Public Sub cleardata()
        txtOnePersonRatio.Text = ""
        txtTwoPersonRatio.Text = ""
        txtExtraAdultRatio.Text = ""
        txtExtraChildRatio.Text = ""
        txtOthersRatio.Text = ""
        txtOnePersonOffset.Text = ""
        txtTwoPersonOffset.Text = ""
        txtExtraAdultOffset.Text = ""
        txtExtraChildOffset.Text = ""
        txtOthersOffset.Text = ""
        txtRatio.Text = ""
        txtoffset.Text = ""
        txtSoldOut.Text = ""
        Try
            ddlRatePlanTarget.SelectedIndex = 0
        Catch ex As Exception
        End Try
        Try
            ddlRatePlanSource.SelectedIndex = 0
        Catch ex As Exception
        End Try
        Me.lblNoLink.Visible = False
        Me.Editar = False
        Me.txtDivVisible.Text = "1"
    End Sub

    Private Function GetidMoneda(ByVal IdRatePlan As String, ByRef isNetRate As Boolean) As Integer
        With New RatePlanFacade
            Dim dsRatePlan As RatePlanData

            Dim ds As New DataSet
            dsRatePlan = .GetDataRatePlan(IdRatePlan, m_iHotelId)
            If Not dsRatePlan Is Nothing Then
                If dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows.Count > 0 Then
                    With dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows(0)
                        isNetRate = If(Not .IsNull(dsRatePlan.FIELD_IDCONTRATO), True, False)

                        If Not .IsNull(dsRatePlan.FIELD_IDMONEDA) Then
                            Return .Item(dsRatePlan.FIELD_IDMONEDA)
                        End If
                    End With
                End If

            End If

        End With
        Return -1
    End Function

    Function getDataXML() As String
        Dim ds As New LinkRatePlanData
        With New LinkRatePlanFacade
            ds = .getById(Me.m_iHotelId, Target)
        End With
        Return Util.Utility.GetXml(ds.TABLE_LINKRATEPLAN, "UpdateRatePlanLink", ds)
    End Function

    Public Function SaveLinks(ByRef isNetRateDif As Boolean) As Boolean
        Dim isNetRateSrc As Boolean = False
        Dim isNetRateTrg As Boolean = False
        Dim idMonedaSource = GetidMoneda(Me.ddlRatePlanSource.SelectedValue, isNetRateSrc)
        Dim idMonedaTarget = 0

        If Me.Editar Then
            idMonedaTarget = GetidMoneda(Me.Target, isNetRateTrg)
        Else
            idMonedaTarget = GetidMoneda(Me.ddlRatePlanTarget.SelectedValue, isNetRateTrg)
        End If


        Dim sData As String = ""
        Dim sDataPrev As String = ""

        isNetRateDif = False
        If isNetRateSrc Or isNetRateTrg Then
            If (isNetRateSrc <> isNetRateTrg) Then
                isNetRateDif = True
                Return False
            End If
        End If

        If idMonedaSource <> idMonedaTarget Then
            Return False
        Else

            sDataPrev = getDataXML()

            Dim ds As New LinkRatePlanData
            Dim dr As DataRow
            dr = ds.Tables(ds.TABLE_LINKRATEPLAN).NewRow
            With dr
                .Item(ds.FIELD_SourceRatePlan) = Me.ddlRatePlanSource.SelectedValue
                If Not Editar Then
                    .Item(ds.FIELD_TargetRatePlan) = Me.ddlRatePlanTarget.SelectedValue
                Else
                    .Item(ds.FIELD_TargetRatePlan) = Target
                End If

                .Item(ds.FIELD_IdHotel) = m_iHotelId
                If Me.txtExtraAdultOffset.Text.Trim <> "" Then
                    .Item(ds.FIELD_ExtraAdultOffset) = CDbl(Val(Me.txtExtraAdultOffset.Text))
                End If
                If Me.txtExtraAdultRatio.Text <> "" Then
                    .Item(ds.FIELD_ExtraAdultRatio) = CDbl(Val(Me.txtExtraAdultRatio.Text))
                End If
                If Me.txtExtraChildOffset.Text.Trim <> "" Then
                    .Item(ds.FIELD_ExtraChildOffset) = CDbl(Val(Me.txtExtraChildOffset.Text))
                End If
                If Me.txtExtraChildRatio.Text.Trim <> "" Then
                    .Item(ds.FIELD_ExtraChildRatio) = CDbl(Val(Me.txtExtraChildRatio.Text))
                End If
                If Me.txtOnePersonOffset.Text.Trim <> "" Then
                    .Item(ds.FIELD_OnePersonOffset) = CDbl(Val(Me.txtOnePersonOffset.Text))
                End If
                If Me.txtOnePersonRatio.Text.Trim <> "" Then
                    .Item(ds.FIELD_OnePersonRatio) = CDbl(Val(Me.txtOnePersonRatio.Text))
                End If
                If Me.txtOthersOffset.Text.Trim <> "" Then
                    .Item(ds.FIELD_OtherOccupationOffset) = CDbl(Val(Me.txtOthersOffset.Text))
                End If
                If Me.txtOthersRatio.Text.Trim <> "" Then
                    .Item(ds.FIELD_OtherOccupationRatio) = CDbl(Val(Me.txtOthersRatio.Text))
                End If
                If Me.txtTwoPersonOffset.Text.Trim <> "" Then
                    .Item(ds.FIELD_TwoPersonOffset) = CDbl(Val(Me.txtTwoPersonOffset.Text))
                End If
                If Me.txtTwoPersonRatio.Text.Trim <> "" Then
                    .Item(ds.FIELD_TwoPersonRatio) = CDbl(Val(Me.txtTwoPersonRatio.Text))
                End If
                .Item(ds.FIELD_SoldOutPerc) = CDbl(Val(Me.txtSoldOut.Text))
            End With
            ds.Tables(ds.TABLE_LINKRATEPLAN).Rows.Add(dr)
            If Editar = False Then
                With New LinkRatePlanFacade
                    If .Insert(ds) Then
                        sData = ds.GetXml
                        sDataPrev = ""
                        CType(Me.Page, PaginaBase).guardalog("/Pages/RatePlansLinks.aspx", PaginaBase.acciones.Crear, "Se Creo el linkeo de " & Me.ddlRatePlanTarget.SelectedItem.Text & " con " & Me.ddlRatePlanSource.SelectedItem.Text, "", sDataPrev, sData)
                        Return True
                    End If
                    Return False
                End With
            Else
                ds.AcceptChanges()
                ds.Tables(ds.TABLE_LINKRATEPLAN).Rows(0).Item(ds.FIELD_TargetRatePlan) = Target 'Me.txtTarget.Text.ToUpper
                With New LinkRatePlanFacade
                    If .Update(ds) Then
                        sData = Util.Utility.GetXml(ds.TABLE_LINKRATEPLAN, "UpdateRatePlanLink", ds)
                        CType(Me.Page, PaginaBase).guardalog("/Pages/RatePlansLinks.aspx", PaginaBase.acciones.Modificar, "Se Modificó el linkeo de " & Me.Targetcode & " con " & Me.Sourcecode & " su nuevo source es (o sigue siendo) " & Me.ddlRatePlanSource.SelectedItem.Text, "", sDataPrev, sData)
                        Return True
                    End If
                    Return False
                End With
            End If

        End If
    End Function

    Public Function RatePlanTargetHasRates() As Boolean
        Return RateHelper.RatePlanHasRates(Me.m_iHotelId, Me.ddlRatePlanTarget.SelectedValue)
    End Function

    Public Sub loadlink(ByVal Target As String)
        Dim ds As New LinkRatePlanData
        With New LinkRatePlanFacade
            ds = .getById(Me.m_iHotelId, Target)
        End With
        If ds.Tables(ds.TABLE_LINKRATEPLAN).Rows.Count > 0 Then

            With ds.Tables(ds.TABLE_LINKRATEPLAN).Rows(0)
                Try
                    Me.ddlRatePlanSource.SelectedValue = .Item(ds.FIELD_SourceRatePlan)
                Catch ex As Exception
                End Try
                txtTarget.Text = TargetNombre '.Item("RateCodeTarget")

                Me.txtExtraAdultOffset.Text = .Item(ds.FIELD_ExtraAdultOffset).ToString
                Me.txtExtraAdultRatio.Text = .Item(ds.FIELD_ExtraAdultRatio).ToString
                Me.txtExtraChildOffset.Text = .Item(ds.FIELD_ExtraChildOffset).ToString
                Me.txtExtraChildRatio.Text = .Item(ds.FIELD_ExtraChildRatio).ToString
                Me.txtOnePersonOffset.Text = .Item(ds.FIELD_OnePersonOffset).ToString
                Me.txtOnePersonRatio.Text = .Item(ds.FIELD_OnePersonRatio).ToString
                Me.txtOthersOffset.Text = .Item(ds.FIELD_OtherOccupationOffset).ToString
                Me.txtOthersRatio.Text = .Item(ds.FIELD_OtherOccupationRatio).ToString
                Me.txtTwoPersonOffset.Text = .Item(ds.FIELD_TwoPersonOffset).ToString
                Me.txtTwoPersonRatio.Text = .Item(ds.FIELD_TwoPersonRatio).ToString
                Me.txtSoldOut.Text = CDbl(Val(.Item(ds.FIELD_SoldOutPerc).ToString))
                If CDbl(Val(.Item(ds.FIELD_ExtraAdultOffset).ToString)) = CDbl(Val(.Item(ds.FIELD_ExtraChildOffset).ToString)) AndAlso _
                    CDbl(Val(.Item(ds.FIELD_OnePersonOffset).ToString)) = CDbl(Val(.Item(ds.FIELD_ExtraChildOffset).ToString)) AndAlso _
                    CDbl(Val(.Item(ds.FIELD_OtherOccupationOffset).ToString)) = CDbl(Val(.Item(ds.FIELD_ExtraChildOffset).ToString)) AndAlso _
                    CDbl(Val(.Item(ds.FIELD_TwoPersonOffset).ToString)) = CDbl(Val(.Item(ds.FIELD_ExtraChildOffset).ToString)) AndAlso _
                    CDbl(Val(.Item(ds.FIELD_ExtraAdultRatio).ToString)) = CDbl(Val(.Item(ds.FIELD_ExtraChildRatio).ToString)) AndAlso _
                    CDbl(Val(.Item(ds.FIELD_OnePersonRatio).ToString)) = CDbl(Val(.Item(ds.FIELD_ExtraChildRatio).ToString)) AndAlso _
                    CDbl(Val(.Item(ds.FIELD_OtherOccupationRatio).ToString)) = CDbl(Val(.Item(ds.FIELD_ExtraChildRatio).ToString)) AndAlso _
                    CDbl(Val(.Item(ds.FIELD_TwoPersonRatio).ToString)) = CDbl(Val(.Item(ds.FIELD_ExtraChildRatio).ToString)) Then
                    txtRatio.Text = .Item(ds.FIELD_ExtraAdultRatio).ToString
                    txtoffset.Text = .Item(ds.FIELD_TwoPersonOffset).ToString
                    Me.txtDivVisible.Text = 1
                Else
                    Me.txtDivVisible.Text = 0
                End If
            End With
        End If
    End Sub

End Class
