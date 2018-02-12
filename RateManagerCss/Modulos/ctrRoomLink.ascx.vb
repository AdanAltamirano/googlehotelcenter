Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Partial Class ctrRoomLink
    Inherits System.Web.UI.UserControl

    'Public Property SourceName() As String
    '    Get
    '        Return viewstate("_SN")
    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_SN") = Value
    '    End Set
    'End Property
    'Public Property TargetName() As String
    '    Get
    '        Return viewstate("_TN")
    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_TN") = Value
    '    End Set
    'End Property
    Public Property Editar() As Boolean
        Get
            Return viewstate("Edit")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("Edit") = Value
            If viewstate("Edit") = True Then
                txtTarget.Visible = True
                Me.ddlRoomTarget.Visible = False
            Else
                txtTarget.Visible = False
                Me.ddlRoomTarget.Visible = True
            End If
        End Set
    End Property
    Private Property TargetEdit() As Integer
        Get
            Return viewstate("_TargetEdit")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_TargetEdit") = Value
        End Set
    End Property

    Public WriteOnly Property SourceDataSet() As RoomsHotelData
        Set(ByVal Value As RoomsHotelData)
            Me.ddlRoomSource.DataTextField = "texto" 'RoomsHotelData.FLD_ROOM_CODE
            Me.ddlRoomSource.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
            Me.ddlRoomSource.DataSource = Value
            Me.ddlRoomSource.DataBind()
            If Me.ddlRoomSource.Items.Count > 0 Then
                Me.ddlRoomSource.SelectedIndex = 0
                ' Me.lblRoomSource.Text = Value.Tables(Value.TBL_ROOM_HOTEL).Rows(0).Item(Value.FLD_NOMBRE).ToString
            End If
            'For i As Integer = 0 To Value.Tables(Value.TBL_ROOM_HOTEL).Rows.Count - 1
            '    Me.SourceName &= "//" & Value.Tables(Value.TBL_ROOM_HOTEL).Rows(i).Item(Value.FLD_NOMBRE).ToString
            'Next
        End Set
    End Property
    Public WriteOnly Property TargetDataSet() As RoomsHotelData
        Set(ByVal Value As RoomsHotelData)
            Me.ddlRoomTarget.Items.Clear()
            Me.ddlRoomTarget.ClearSelection()
            If (Not Value Is Nothing) AndAlso Value.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows.Count > 0 Then

                Me.ddlRoomTarget.DataTextField = "texto" 'RoomsHotelData.FLD_ROOM_CODE
                Me.ddlRoomTarget.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
                Me.ddlRoomTarget.DataSource = Value.Tables(RoomsHotelData.TBL_ROOM_HOTEL)
                Me.ddlRoomTarget.DataBind()
                If Me.ddlRoomTarget.Items.Count > 0 Then
                    Me.ddlRoomTarget.SelectedIndex = 0
                    '  Me.lblRoomTarget.Text = Value.Tables(Value.TBL_ROOM_HOTEL).Rows(0).Item(Value.FLD_NOMBRE).ToString
                End If
            End If
            'For i As Integer = 0 To Value.Tables(Value.TBL_ROOM_HOTEL).Rows.Count - 1
            '    Me.TargetName &= "//" & Value.Tables(Value.TBL_ROOM_HOTEL).Rows(i).Item(Value.FLD_NOMBRE).ToString
            'Next
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
    Public ReadOnly Property samelink() As Boolean
        Get
            If Me.ddlRoomSource.SelectedValue = Me.ddlRoomTarget.SelectedValue Then
                Me.lblNoLink.Visible = True
                Return True
            Else
                Me.lblNoLink.Visible = False
                Return False
            End If
        End Get
    End Property
    Public Property Targetcode() As String
        Get
            Return ViewState("_TargetC")
        End Get
        Set(ByVal Value As String)
            VIEWSTATE("_TargetC") = Value
        End Set
    End Property
    Public Property Sourcecode() As String
        Get
            Return ViewState("_SourceC")
        End Get
        Set(ByVal Value As String)
            VIEWSTATE("_SourceC") = Value
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página        
        If Not IsPostBack Then
            Me.txtDivVisible.Text = "1"
        End If
        'Me.ddlRoomSource.Attributes.Add("onChange", "javascript:GName('" & Me.ddlRoomSource.ClientID & "','" & Me.lblRoomSource.ClientID & "','" & SourceName & "')")
        'Me.ddlRoomTarget.Attributes.Add("onChange", "javascript:GName('" & Me.ddlRoomTarget.ClientID & "','" & Me.lblRoomTarget.ClientID & "','" & TargetName & "')")
        hplhide.NavigateUrl = "javascript:Ocultar('0','" & hplShow.ClientID & "','" & hplhide.ClientID & "','" & divDatos.ClientID & "','" & Me.txtRatio.ClientID & "','" & Me.txtoffset.ClientID & "','" & Me.divDatos.ClientID & "','" & divratio.ClientID & "','" & divoffset.ClientID & "','" & Me.lbl_Offset.ClientID & "','" & Me.lbl_Ratio.ClientID & "','" & Me.txtDivVisible.ClientID & "');"
        hplShow.NavigateUrl = "javascript:Ocultar('1','" & hplShow.ClientID & "','" & hplhide.ClientID & "','" & divDatos.ClientID & "','" & Me.txtRatio.ClientID & "','" & Me.txtoffset.ClientID & "','" & Me.divDatos.ClientID & "','" & divratio.ClientID & "','" & divoffset.ClientID & "','" & Me.lbl_Offset.ClientID & "','" & Me.lbl_Ratio.ClientID & "','" & Me.txtDivVisible.ClientID & "');"
        Me.txtoffset.Attributes.Add("onChange", "javascript:FillPrices('" & Me.divDatos.ClientID & "','Offset'" & ",'" & txtoffset.ClientID & "')")
        Me.txtRatio.Attributes.Add("onChange", "javascript:FillPrices('" & Me.divDatos.ClientID & "','Ratio'" & ",'" & txtRatio.ClientID & "')")

    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadculture()
        If Me.txtDivVisible.Text = "1" Then
            hplShow.Style.Add("display", "block")
            divratio.Style.Add("display", "block")
            divoffset.Style.Add("display", "block")
            lbl_Offset.Style.Add("display", "block")
            lbl_Ratio.Style.Add("display", "block")
            hplhide.Style.Add("display", "none")
            divDatos.Style.Add("display", "none")
        Else
            hplShow.Style.Add("display", "none")
            divratio.Style.Add("display", "none")
            divoffset.Style.Add("display", "none")
            lbl_Offset.Style.Add("display", "none")
            lbl_Ratio.Style.Add("display", "none")
            hplhide.Style.Add("display", "block")
            divDatos.Style.Add("display", "block")
        End If
    End Sub

    Private Sub loadculture()
        Me.lblEExtraChildRate.Text = PortalCulture.GetString("00041", True)
        Me.lblEExtraAdultRate.Text = PortalCulture.GetString("00040", True)
        Me.lblETwoPersonRAte.Text = PortalCulture.GetString("00039", True)
        Me.lblEOnePersonRate.Text = PortalCulture.GetString("00038", True)
        Me.lblOffset.Text = PortalCulture.GetString("00037", True)
        Me.lblRatio.Text = PortalCulture.GetString("00036", True)
        Me.lblOffset2.Text = PortalCulture.GetString("00037", True)
        Me.lblRatio2.Text = PortalCulture.GetString("00036", True)
        lbl_Ratio.Text = PortalCulture.GetString("00036", True)
        Me.lbl_Offset.Text = PortalCulture.GetString("00037", True)
        Me.lblERoomSource.Text = PortalCulture.GetString("00124", True)
        Me.lblERoomTarget.Text = PortalCulture.GetString("00123", True)
        Me.lblTitle.Text = PortalCulture.GetString("00032")
        lblOthersOcupation.Text = PortalCulture.GetString("00044", True)
        If Editar = True Then
            Me.lblTitle.Text = PortalCulture.GetString("00033")
        End If
        RVOnePersonOffset.ErrorMessage = PortalCulture.GetString("00045")
        RVTwoPersonOffset.ErrorMessage = PortalCulture.GetString("00045")
        RVExtraAdultOffset.ErrorMessage = PortalCulture.GetString("00045")
        RVExtraChildOffset.ErrorMessage = PortalCulture.GetString("00045")
        RVOthersOffset.ErrorMessage = PortalCulture.GetString("00045")
        RVOffset.ErrorMessage = PortalCulture.GetString("00045")
        Me.lblNoLink.Text = PortalCulture.GetString("00046")
        lbllinkdata.Text = PortalCulture.GetString("00418")
        hplShow.Text = PortalCulture.GetString("00319")
        hplhide.Text = PortalCulture.GetString("00318")
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
        txtoffset.Text = ""
        txtRatio.Text = ""
        Try
            ddlRoomTarget.SelectedIndex = 0
        Catch ex As Exception
        End Try
        Try
            ddlRoomSource.SelectedIndex = 0
        Catch ex As Exception
        End Try
        Me.lblNoLink.Visible = False
        Me.Editar = False
        Me.txtDivVisible.Text = "1"
    End Sub

    Function getDataXML(ByVal id As Integer) As String
        Dim ds As New LinkRoomTypeData
        With New LinkRoomsFacade
            ds = .getById(id, PortalCulture.GetIDCulture)
        End With
        Return Util.Utility.GetXml(ds.TABLE_LINKROOM, "UpdateRoomLink", ds)
    End Function

    Public Function SaveLinks() As Boolean

        Dim ds As New LinkRoomTypeData
        Dim dr As DataRow
        Dim sData As String = ""
        Dim sDataPrev As String = ""

        dr = ds.Tables(ds.TABLE_LINKROOM).NewRow
        With dr
            .Item(ds.FIELD_SourceRoom) = Me.ddlRoomSource.SelectedValue
            If Editar = False Then
                .Item(ds.FIELD_TargetRoom) = Me.ddlRoomTarget.SelectedValue
            Else
                .Item(ds.FIELD_TargetRoom) = TargetEdit
            End If

            If Me.txtExtraAdultOffset.Text.Trim <> "" Then
                .Item(ds.FIELD_ExtraAdultOffset) = CDbl(Val(Me.txtExtraAdultOffset.Text))
            End If
            If Me.txtExtraAdultRatio.Text.Trim <> "" Then
                .Item(ds.FIELD_ExtraAdultRatio) = CDbl(Val(Me.txtExtraAdultRatio.Text))
            End If
            If Me.txtExtraChildOffset.Text.Trim <> "" Then
                .Item(ds.FIELD_ExtraChildOffset) = CDbl(Val(Me.txtExtraChildOffset.Text))
            End If
            If Me.txtExtraChildRatio.Text.Trim <> "" Then
                .Item(ds.FIELD_ExtraChildRatio) = CDbl(Val(Me.txtExtraChildRatio.Text))
            End If
            If Me.txtOnePersonOffset.Text <> "" Then
                .Item(ds.FIELD_OnePersonOffset) = CDbl(Val(Me.txtOnePersonOffset.Text))
            End If
            If Me.txtOnePersonRatio.Text <> "" Then
                .Item(ds.FIELD_OnePersonRatio) = CDbl(Val(Me.txtOnePersonRatio.Text))
            End If
            If Me.txtOthersOffset.Text <> "" Then
                .Item(ds.FIELD_OtherOccupationOffset) = CDbl(Val(Me.txtOthersOffset.Text))
            End If
            If Me.txtOthersRatio.Text <> "" Then
                .Item(ds.FIELD_OtherOccupationRatio) = CDbl(Val(Me.txtOthersRatio.Text))
            End If
            If Me.txtTwoPersonOffset.Text <> "" Then
                .Item(ds.FIELD_TwoPersonOffset) = CDbl(Val(Me.txtTwoPersonOffset.Text))
            End If
            If Me.txtTwoPersonRatio.Text <> "" Then
                .Item(ds.FIELD_TwoPersonRatio) = CDbl(Val(Me.txtTwoPersonRatio.Text))
            End If
        End With
        ds.Tables(ds.TABLE_LINKROOM).Rows.Add(dr)
        If Editar = False Then
            With New LinkRoomsFacade
                If .Insert(ds) Then
                    sData = Util.Utility.GetXml(ds.TABLE_LINKROOM, "UpdateRoomLink", ds)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/RoomsLinks.aspx", PaginaBase.acciones.Crear, "Se Creo el linkeo de " & Me.ddlRoomTarget.SelectedItem.Text & " con " & Me.ddlRoomSource.SelectedItem.Text, "", sDataPrev, sData)
                    Return True
                End If
                Return False
            End With
        Else
            sDataPrev = getDataXML(TargetEdit)

            ds.AcceptChanges()
            ds.Tables(ds.TABLE_LINKROOM).Rows(0).Item(ds.FIELD_TargetRoom) = TargetEdit
            With New LinkRoomsFacade
                If .Update(ds) Then
                    sData = Util.Utility.GetXml(ds.TABLE_LINKROOM, "UpdateRoomLink", ds)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/RoomsLinks.aspx", PaginaBase.acciones.Modificar, "Se Modificó el linkeo de " & Me.Targetcode & " con " & Me.Sourcecode & " su nuevo source es (o sigue siendo) " & Me.ddlRoomSource.SelectedItem.Text, "", sDataPrev, sData)
                    Return True
                End If
                Return False
            End With
        End If
    End Function

    Public Sub loadlink(ByVal Target As Integer)

        Dim ds As New LinkRoomTypeData
        With New LinkRoomsFacade
            ds = .getById(Target, PortalCulture.GetIDCulture)
        End With
        If ds.Tables(ds.TABLE_LINKROOM).Rows.Count > 0 Then

            With ds.Tables(ds.TABLE_LINKROOM).Rows(0)
                'para tomar el nombre de cuarto ke se linkeó
                TargetEdit = Target
                Try
                    '  Me.ddlRoomTarget.SelectedValue = .Item(ds.FIELD_TargetRoom)
                    txtTarget.Text = .Item(RoomsHotelData.FLD_ROOM_CODE) & "--" & .Item(RoomsHotelData.FLD_NOMBRE) 'Me.ddlRoomSource.SelectedItem.Text
                    '  Me.lblRoomTarget.Text = .Item(RoomsHotelData.FLD_NOMBRE)
                Catch ex As Exception
                End Try

                Try
                    Me.ddlRoomSource.SelectedValue = .Item(ds.FIELD_SourceRoom)
                Catch ex As Exception
                End Try

                Try
                    Me.ddlRoomTarget.SelectedValue = .Item(ds.FIELD_TargetRoom)
                Catch ex As Exception
                End Try

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
