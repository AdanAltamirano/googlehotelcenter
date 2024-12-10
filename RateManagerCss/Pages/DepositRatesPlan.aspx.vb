
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Partial Class DepositRatesPlan
    Inherits PaginaBase
    Private Property iddepositratesplan() As Integer
        Get
            Return viewstate("depositratesplan")
        End Get
        Set(ByVal Value As Integer)
            viewstate("depositratesplan") = Value
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

    Protected Property HasData() As Boolean
        Get
            HasData = False
            If Me.ViewState("HasData") IsNot Nothing Then HasData = Me.ViewState("HasData")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("HasData") = value
        End Set
    End Property

    Private Property IDPrepago() As Integer
        Get
            Return ViewState("IDPrepago")
        End Get
        Set(ByVal Value As Integer)
            ViewState("IDPrepago") = Value
        End Set
    End Property


    Private Property RateCode() As String
        Get
            Return ViewState("RateCode")
        End Get
        Set(ByVal Value As String)
            ViewState("RateCode") = Value
        End Set
    End Property
    Private Property ChanelSource() As String
        Get
            Return ViewState("ChanelSource")
        End Get
        Set(ByVal Value As String)
            ViewState("ChanelSource") = Value
        End Set
    End Property


    Private Enum dgcolumns
        ratecode
        Name
        idAgenciasRate
        rategds
        rateportal
        rateUnip
        rateAds
        Channel
    End Enum
#Region " Web Form Designer Generated Code "


    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtDescripcion As CtrlIdiomaRFCk

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Sub MostrarCmdNew(ByVal show As Boolean)
        'cmdNew.Style.Add("display", IIf(show, "block", "none"))
        cmdNew.Style.Add("display", "none")
        divContenedor.Style.Add("display", IIf(show, "none", "block"))
    End Sub


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Me.txtDescripcion.Height = "80"
        txtDescripcion.IsHTML = True
        If Not IsPostBack Then
            rbPrepago.Visible = False
            loadrateplans("")
            ClearData()
            divPrepago.Style.Add("display", "none")            
            MostrarCmdNew(True)
            'btnNew.Visible = True
        End If
        rbPrepago.Attributes.Add("onclick", String.Format("FireShowPrepay('{0}','{1}', true);", rbPrepago.ClientID, divPrepago.ClientID))
        RdbOneNigth.Attributes.Add("onclick", String.Format("FireShowPrepay('{0}','{1}', false);", rbPrepago.ClientID, divPrepago.ClientID))
        RdbNone.Attributes.Add("onclick", String.Format("FireShowPrepay('{0}','{1}', false);", rbPrepago.ClientID, divPrepago.ClientID))
        rdbAlltotal.Attributes.Add("onclick", String.Format("FireShowPrepay('{0}','{1}', false);", rbPrepago.ClientID, divPrepago.ClientID))
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))

        Dim idAsoc As Integer = Me.GetIdAsociation
        If (Me.IsSupervisor Or isUserChain Or (Me.IsUsuarioHotelAssociation And idAsoc = Me.IdAsociation)) Then
        Else
            lblTarget.Style.Add("display", "none")
            RdbUV.Style.Add("display", "none")
            RdbHotel.Style.Add("display", "none")
        End If

    End Sub

    Private Function RatePlanFilter(ByVal segmentType As String, ByVal RatesPlan As Portal.General.Common.Data.RatePlanData) As Portal.General.Common.Data.RatePlanData
        'Elimina los planes tarifarios que contengan el tipo de segmento especificado
        Dim dv2 As DataView
        For Each r As DataRow In RatesPlan.Tables("RatePlans").Rows()
            dv2 = RatesPlan.Tables("RatePlans").DefaultView
            dv2.RowFilter = "Segment" & "=" & "'" & segmentType & "'"
            If dv2.Count > 0 AndAlso r("Segment").ToString() = segmentType Then
                r.Delete()
            End If
        Next
        RatesPlan.AcceptChanges()
        Return RatesPlan
    End Function

    Private Sub loadrateplans(ByVal sfiltro As String)
        Dim ds As RatePlanData
        Dim dv As DataView
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            Dim bNetRatesPlan As Byte = IIf(MyBase.IsSupervisor Or MyBase.IsUsuarioNetRates, 1, 0)
            ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, incluirNetRatesPlan:=bNetRatesPlan, idAsociacion:=idAsoc, DeleteFilter:=1)
        End With

        If MyBase.IdCorporativoUserChain = 4 AndAlso (MyBase.IsHotel Or MyBase.IsUsuarioHotel) Then
            'ds = RatePlanFilter("C", ds)
        End If

        With dgdeposit
            dv = ds.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView
            dv.RowFilter = sfiltro
            .DataKeyField = RatePlanData.FIELD_IDRATEPLAN
            .DataSource = dv 'ds
            .DataBind()
        End With
    End Sub

    Private Sub dgRatePlans_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgdeposit.PageIndexChanged
        Me.dgdeposit.CurrentPageIndex = e.NewPageIndex
        Me.dgdeposit.SelectedIndex = -1
        loadrateplans(ctrlAutoComplete1.GetFilter)
        'btnNew.Visible = True        
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadCulture()
    End Sub
    Private Sub loadCulture()

        cmdNew.Value = PortalCulture.GetString("00102")
        Me.RdbNone.Text = PortalCulture.GetString("00717")
        Me.RdbOneNigth.Text = PortalCulture.GetString("00715")
        Me.rdbAlltotal.Text = PortalCulture.GetString("00716")
        Me.rbPrepago.Text = PortalCulture.GetString("01266")
        Me.lblDeposittitle.Text = PortalCulture.GetString("00718", True)
        Me.lblDescripcion.Text = PortalCulture.GetString("00786", True)
        Me.lblTitle.Text = PortalCulture.GetString("00787")
        Me.btnNuevo.Text = PortalCulture.GetString("00009")
        Me.btnSave.Text = PortalCulture.GetString("M000060")
        Me.lblTarget.Text = PortalCulture.GetString("00790", True)
        Me.rvPorcentaje.Text = PortalCulture.GetString("01261")
    End Sub

    Private Sub dgRatePlans_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgdeposit.ItemCommand
        If e.CommandName = "EditGDS" Or e.CommandName = "EditPOR" Or e.CommandName = "EditUNI" Or e.CommandName = "EditADS" Then
            ClearData()

            Dim ds As DepositsRates
            Dim idAgencia As Integer

            rbPrepago.Visible = False
            RateCode = e.Item.Cells(dgcolumns.ratecode).Text
            With New DepositRatesFacade
                Select Case e.CommandName
                    Case "EditGDS"
                        ChanelSource = "GDS"
                    Case "EditPOR"
                        ChanelSource = "POR"
                        'Integer.TryParse(e.Item.Cells(dgcolumns.idAgenciasRate).Text, idAgencia)
                        rbPrepago.Visible = True 'If(idAgencia > 0, True, False)
                        txtPrepago.Text = ""
                    Case "EditUNI"
                        ChanelSource = "UNI"
                    Case "EditADS"
                        ChanelSource = "ADS"
                End Select
                ds = .GetDepositData(e.Item.Cells(dgcolumns.ratecode).Text, Me.cInfoActual.Hotel, ChanelSource)
            End With
            lblInfoRatePlan.Text = String.Format(PortalCulture.GetString("00788"), e.Item.Cells(dgcolumns.ratecode).Text & " - " & e.Item.Cells(dgcolumns.Name).Text, ChanelSource)
            If Not ds Is Nothing AndAlso ds.Tables(DepositsRates.TABLEDepositRates).Rows.Count > 0 Then
                loaddata(ds)
            Else
                divPrepago.Style.Add("display", "none")
            End If
            MostrarCmdNew(False)

            'btnNew.Visible = False
        End If
    End Sub

    Private Sub ClearData()
        Me.HasData = False
        txtDescripcion.Limpia()
        Me.RdbNone.Checked = True
        Me.RdbOneNigth.Checked = False
        Me.rdbAlltotal.Checked = False
        Me.rbPrepago.Checked = False
        Me.RdbUV.Checked = True
        Me.RdbHotel.Checked = False
        iddepositratesplan = 0
        idDicc = 0
        ChanelSource = ""
        RateCode = ""
        IDPrepago = 0
        txtPrepago.Text = ""
        lblInfoRatePlan.Text = PortalCulture.GetString("00789")
        divPrepago.Style.Add("display", "none")
    End Sub

    Private Sub btnNuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
        ClearData()
        MostrarCmdNew(True)
        'btnNew.Visible = True
    End Sub
    'Private Sub btnNuevo2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
    '    ClearData()
    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
    '    btnNew.Visible = False
    'End Sub
    Private Sub loaddata(ByVal Ds As DepositsRates)
        Dim importe As Double
        With Ds.Tables(DepositsRates.TABLEDepositRates).Rows(0)
            ChanelSource = .Item(DepositsRates.FIELD_Source)
            RateCode = .Item(DepositsRates.FIELD_ratecode)
            If Not .IsNull(DepositsRates.FIELD_IdDiccDepositInfo) Then
                idDicc = .Item(DepositsRates.FIELD_IdDiccDepositInfo)
            Else
                idDicc = 0
            End If
            txtDescripcion.CargaDatos(idDicc)
            If Not .IsNull(DepositsRates.FIELD_DepositTarget) Then
                If .Item(DepositsRates.FIELD_DepositTarget).ToString.ToUpper.Trim = "HTL" Then
                    Me.RdbHotel.Checked = True
                    Me.RdbUV.Checked = False
                End If
            End If
            If Not .IsNull(DepositsRates.FIELD_DepositType) Then
                divPrepago.Style.Add("display", "none")
                Select Case CType(.Item(DepositsRates.FIELD_DepositType), Integer)
                    Case 1
                        Me.RdbOneNigth.Checked = True
                    Case 2
                        Me.rdbAlltotal.Checked = True
                    Case 3
                        rbPrepago.Checked = True
                        divPrepago.Style.Add("display", "")
                        If Not .IsNull(DepositsRates.FIELD_PrepaymentPerc) Then
                            Double.TryParse(.Item(DepositsRates.FIELD_PrepaymentPerc), importe)
                            Me.txtPrepago.Text = If(importe > 0, importe.ToString, "")
                        End If
                End Select
            End If

            Me.HasData = True

            Me.iddepositratesplan = .Item(DepositsRates.FIELD_IdDepositRatesPlan)
        End With
    End Sub

    Function getDataXML() As String
        Dim ds As New DepositsRates
        With New DepositRatesFacade
            ds = .GetDepositData(RateCode, Me.cInfoActual.Hotel, ChanelSource)
        End With
        Return Util.Utility.GetXml(DepositsRates.TABLEDepositRates, "UpdateDeposit", ds)
    End Function

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPublish.Click
        If Me.RateCode <> "" AndAlso Me.ChanelSource <> "" Then
            Dim publish As Boolean = (CType(sender, Button).ID = Me.btnPublish.ID)
            Dim DS As New DepositsRates
            Dim dr As DataRow
            Dim sData As String = ""
            Dim sDataPrev As String = ""
            Dim dporc As Double

            dr = DS.Tables(DepositsRates.TABLEDepositRates).NewRow
            dr.Item(DepositsRates.FIELD_IdDepositRatesPlan) = iddepositratesplan
            dr.Item(DepositsRates.FIELD_IdHotel) = Me.cInfoActual.Hotel
            dr.Item(DepositsRates.FIELD_ratecode) = RateCode
            dr.Item(DepositsRates.FIELD_Source) = Me.ChanelSource
            dr.Item(DepositsRates.FIELD_DepositInfo) = Me.txtDescripcion.textodefault
            dr.Item(DepositsRates.FIELD_IdDiccDepositInfo) = Me.txtDescripcion.IdIndice
            If Me.rdbAlltotal.Checked Then
                dr.Item(DepositsRates.FIELD_DepositType) = 2
            ElseIf Me.RdbOneNigth.Checked Then
                dr.Item(DepositsRates.FIELD_DepositType) = 1
            ElseIf Me.rbPrepago.Checked Then
                dr.Item(DepositsRates.FIELD_DepositType) = 3
                If Not String.IsNullOrEmpty(txtPrepago.Text) Then
                    Double.TryParse(txtPrepago.Text, dporc)
                    If dporc >= 0 And dporc <= 100 Then _
                        dr.Item(DepositsRates.FIELD_PrepaymentPerc) = dporc
                End If
            End If

            If Me.RdbHotel.Checked Then
                dr.Item(DepositsRates.FIELD_DepositTarget) = "HTL"
            Else
                dr.Item(DepositsRates.FIELD_DepositTarget) = "ZT"
            End If
            dr.Item(DepositsRates.FIELD_IdDepositRatesPlan) = iddepositratesplan
            DS.Tables(DepositsRates.TABLEDepositRates).Rows.Add(dr)
            If Me.iddepositratesplan > 0 Then
                If Me.idDicc <> 0 Then
                    Me.txtDescripcion.Update(Me.idDicc, publish)
                Else
                    idDicc = Me.txtDescripcion.Insert()
                End If
                sDataPrev = getDataXML()

                DS.Tables(DepositsRates.TABLEDepositRates).AcceptChanges()
                DS.Tables(DepositsRates.TABLEDepositRates).Rows(0).Item(DepositsRates.FIELD_IdHotel) = DS.Tables(DepositsRates.TABLEDepositRates).Rows(0).Item(DepositsRates.FIELD_IdHotel)
                If idDicc <> 0 Then
                    DS.Tables(DepositsRates.TABLEDepositRates).Rows(0).Item(DepositsRates.FIELD_IdDiccDepositInfo) = idDicc
                End If
                With New DepositRatesFacade
                    If .UpdateDepositData(DS) Then
                        sData = Util.Utility.GetXml(DepositsRates.TABLEDepositRates, "UpdateDeposit", DS)
                        Me.guardalog("/Pages/DepositRatesPlan.aspx", If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar), "Se Modificó el deposito tarifa del id " & iddepositratesplan & " y el codigo de tarifa " & RateCode, "", sDataPrev, sData)
                        If Me.txtDescripcion.HasChanges Then Me.NotifyContentModification("Deposito de tarifa con el codigo " & RateCode, "Depositos De Planes Tarifarios")
                        ClearData()                        
                        MostrarCmdNew(True)
                        'btnNew.Visible = True
                    Else
                        'btnNew.Visible = False
                    End If
                End With
            Else
                With New DepositRatesFacade
                    If .InsertDepositData(DS, Me.idDicc) Then
                        sData = Util.Utility.GetXml(DepositsRates.TABLEDepositRates, "UpdateDeposit", DS)
                        Me.txtDescripcion.Update(DS.Tables(DepositsRates.TABLEDepositRates).Rows(0)(DepositsRates.FIELD_IdDiccDepositInfo), publish)
                        Me.guardalog("/Pages/DepositRatesPlan.aspx", PaginaBase.acciones.Crear, "Se creo el deposito tarifa del id " & iddepositratesplan & " y el codigo de tarifa " & RateCode, "", sDataPrev, sData)
                        If publish Then
                            Me.guardalog("/Pages/DepositRatesPlan.aspx", PaginaBase.acciones.Publicar, "Se Modificó el deposito tarifa del id " & iddepositratesplan & " y el codigo de tarifa " & RateCode, "", sDataPrev, sData)
                        End If
                        If Me.txtDescripcion.HasChanges Then
                            Me.NotifyContentModification("Deposito de tarifa con el codigo " & RateCode, "Depositos De Planes Tarifarios")
                        End If
                        ClearData()                        
                        MostrarCmdNew(True)
                        'btnNew.Visible = True
                    Else

                        'btnNew.Visible = False
                    End If
                End With
            End If
        Else

            'btnNew.Visible = False
        End If


    End Sub

    Private Sub dgRatePlans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgdeposit.ItemDataBound
        If e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim img As LinkButton
            If Not CType(e.Item.Cells(dgcolumns.rategds).Text, Boolean) Then
                img = e.Item.FindControl("lnkGDS")
                img.Visible = False
            End If
            If Not CType(e.Item.Cells(dgcolumns.rateportal).Text, Boolean) Then
                img = e.Item.FindControl("lnkPOR")
                img.Visible = False
            End If
            If Not CType(e.Item.Cells(dgcolumns.rateUnip).Text, Boolean) Then
                img = e.Item.FindControl("lnkUNI")
                img.Visible = False
            End If
            If Not CType(e.Item.Cells(dgcolumns.rateAds).Text, Boolean) Then
                img = e.Item.FindControl("lnkADS")
                img.Visible = False
            End If


        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Name).Text = PortalCulture.GetString("00073")
            e.Item.Cells(dgcolumns.ratecode).Text = PortalCulture.GetString("00001")
            e.Item.Cells(dgcolumns.Channel).Text = PortalCulture.GetString("00791")
        End If
    End Sub

    Private Sub dgRatePlans_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgdeposit.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Name).Text = PortalCulture.GetString("00073")
            e.Item.Cells(dgcolumns.ratecode).Text = PortalCulture.GetString("00001")
            e.Item.Cells(dgcolumns.Channel).Text = PortalCulture.GetString("00791")
        End If
    End Sub

    Private Sub ctrlAutoComplete1_OnSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        dgdeposit.CurrentPageIndex = 0
        loadrateplans(descripcion)
    End Sub

End Class
