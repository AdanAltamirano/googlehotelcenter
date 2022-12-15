Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Partial Class CtrlPlanFaresExcNR
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

    Public Property FieldException() As String
        Get
            Return ViewState("Excep")
        End Get
        Set(ByVal Value As String)
            ViewState("Excep") = Value
        End Set
    End Property
    Public Property combinaciones() As Integer
        Get
            Return ViewState("CombPers")
        End Get
        Set(ByVal Value As Integer)
            ViewState("CombPers") = Value
        End Set
    End Property

    Public Property m_TextBoxPorcMin() As String
        Get
            Return viewstate("TextBoxPorcMinClientId")
        End Get
        Set(ByVal Value As String)
            viewstate("TextBoxPorcMinClientId") = Value
        End Set
    End Property

    Public Property m_TextBoxPorcMax() As String
        Get
            Return viewstate("TextBoxPorcMaxClientId")
        End Get
        Set(ByVal Value As String)
            viewstate("TextBoxPorcMaxClientId") = Value
        End Set
    End Property

    Public Property PlusTaxProperty() As Boolean
        Get
            Return ViewState("PlusTaxProperty")
        End Get
        Set(value As Boolean)
            ViewState("PlusTaxProperty") = value
        End Set
    End Property

    Public Property EcotasaProperty() As Double
        Get
            Return ViewState("EcotasaProperty")
        End Get
        Set(value As Double)
            ViewState("EcotasaProperty") = value
        End Set
    End Property



    Enum RestrictionsDtgCols As Integer
        AdultNumber = 0
        AdultFare
        ChildNumber
        ChildFare
        TotalFare
        RestrictionId
        Tools
    End Enum

    Public Sub createFieldException()
        Dim cad As String = ""
        For I As Integer = 1 To 7
            Dim CK As CheckBox
            CK = FindControl("chk" & I.ToString)
            If CK.Checked = False Then
                cad &= "N"
            Else
                cad &= "Y"
            End If
        Next
        FieldException = cad
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            FillDataGrid()
        End If

    End Sub

    Public Sub ReFill()
        FillDataGrid()
    End Sub

#Region "FILL DE LOS NUEVOS DATAGRIDS"
    Private Sub FillDatas()
        Dim datRestrictions As FaresRestrictionsData
        Dim dtAdults As DataTable, dtChildren As DataTable
        Dim dtTeen As DataTable
        Dim ad As Byte, ch As Byte
        Dim drnew As DataRow
        Dim strch As String = ""

        dtAdults = New DataTable("Adults")
        dtChildren = New DataTable("Children")
        dtTeen = New DataTable("Teen")

        dtAdults.Columns.Add("Adults")
        dtAdults.Columns.Add("Price")
        dtAdults.Columns.Add("PriceNR")
        dtAdults.Columns.Add(FaresRestrictionsData.PKIDRESTRICTION_FIELD)

        dtChildren.Columns.Add("Children")
        dtChildren.Columns.Add("Price")
        dtChildren.Columns.Add("PriceNR")
        dtChildren.Columns.Add(FaresRestrictionsData.PKIDRESTRICTION_FIELD)

        dtTeen.Columns.Add("teen")
        dtTeen.Columns.Add("Price")
        dtTeen.Columns.Add("PriceNR")
        dtTeen.Columns.Add(FaresRestrictionsData.PKIDRESTRICTION_FIELD)

        datRestrictions = CType(Me.Page, FaresCatalogueNR).GetFareRestrictions
        If Not datRestrictions Is Nothing Then
            For Each dr As DataRow In datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows
                If dr(FaresRestrictionsData.ADULTNUMBER_FIELD) <> ad Then
                    ad = dr(FaresRestrictionsData.ADULTNUMBER_FIELD)
                    drnew = dtAdults.NewRow
                    drnew("Adults") = ad
                    drnew("Price") = dr(FaresRestrictionsData.EXCADULTFARE_FIELD)
                    drnew("PriceNR") = IIf(dr(FaresRestrictionsData.EXCADULTFARENR_FIELD) Is DBNull.Value, 0, dr(FaresRestrictionsData.EXCADULTFARENR_FIELD))
                    drnew(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = dr(FaresRestrictionsData.PKIDRESTRICTION_FIELD)
                    dtAdults.Rows.Add(drnew)
                End If
                If dr(FaresRestrictionsData.CHILDNUMBER_FIELD) > 0 AndAlso strch.IndexOf("," & dr(FaresRestrictionsData.CHILDNUMBER_FIELD) & ",") = -1 Then
                    ch = dr(FaresRestrictionsData.CHILDNUMBER_FIELD)
                    strch &= "," & ch & ","
                    drnew = dtChildren.NewRow
                    drnew("Children") = ch
                    drnew("Price") = dr(FaresRestrictionsData.EXCNINIOFARE_FIELD)
                    drnew("PriceNR") = IIf(dr(FaresRestrictionsData.EXCNINIOFARENR_FIELD) Is DBNull.Value, 0, dr(FaresRestrictionsData.EXCNINIOFARENR_FIELD))
                    drnew(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = dr(FaresRestrictionsData.PKIDRESTRICTION_FIELD)
                    dtChildren.Rows.Add(drnew)

                    drnew = dtTeen.NewRow
                    drnew("Teen") = ch
                    drnew("Price") = dr(FaresRestrictionsData.EXTEENFARE_FIELD)
                    drnew("PriceNR") = IIf(dr(FaresRestrictionsData.EXTEENFARENR_FIELD) Is DBNull.Value, 0, dr(FaresRestrictionsData.EXTEENFARENR_FIELD))
                    drnew(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = dr(FaresRestrictionsData.PKIDRESTRICTION_FIELD)
                    dtTeen.Rows.Add(drnew)

                End If
            Next

            dgChild.DataSource = dtChildren
            dgAdult.DataSource = dtAdults

            dgChild.DataBind()
            dgAdult.DataBind()

            If CType(Me.Page, PaginaBase).isConfigAdolescente Then
                dgTeen.DataSource = dtTeen
                dgTeen.DataBind()
            End If

        End If

    End Sub
#End Region

    Private Sub FillDataGrid()
        FillDatas()
        Dim datRestrictions As FaresRestrictionsData
        datRestrictions = CType(Me.Page, FaresCatalogueNR).GetFareRestrictions
        'If Not datRestrictions Is Nothing Then
        '    dtgRestrictions.DataSource = datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE)
        'End If
        'dtgRestrictions.DataBind()
        If Not datRestrictions Is Nothing Then
            If Not datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE) Is Nothing AndAlso datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Count > 0 Then
                Dim cad As String = Me.FieldException

                For I As Integer = 1 To 7
                    Dim CK As CheckBox
                    CK = FindControl("chk" & I.ToString)
                    CK.Checked = False
                    If cad.Length >= 7 Then
                        If cad.Substring(I - 1, 1) = "Y" Then
                            CK.Checked = True
                        End If
                    End If
                Next

            End If
            combinaciones = datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Count
        Else
            combinaciones = 0
        End If

    End Sub
    Public Function IsValidData() As Boolean
        IsValidData = True
        createFieldException()
        If FieldException() <> "NNNNNNN" Then

            IsValidData = True

            Dim txtNetRate As TextBox
            Dim txtRate As TextBox

            Dim netRate As Double = 0
            Dim rate As Double = 0
            Dim minPercent As Integer = 0
            Dim maxPercent As Integer = 0
            Dim isSupervisor As Boolean = CType(Me.Page, PaginaBase).IsSupervisor

            Integer.TryParse(Me.varMinPercent.Value, minPercent)
            Integer.TryParse(Me.varMaxPercent.Value, maxPercent)

            For Each item As DataGridItem In dgAdult.Items
                txtNetRate = item.Cells(2).FindControl("txtAdultFareNR")
                txtRate = item.Cells(1).FindControl("txtAdultFare")

                If Not Me.isUsuarioMixto Then
                    Double.TryParse(CType(item.Cells(1).FindControl("varRate"), HtmlInputHidden).Value, rate)
                Else
                    Double.TryParse(txtRate.Text, rate)
                End If
                'Double.TryParse(txtNetRate.Text, netRate)
                Double.TryParse(txtNetRate.Text, netRate)

                'If netRate > 0 AndAlso (Not isSupervisor OrElse rate > 0) Then
                If Not isSupervisor AndAlso (netRate > 0) Then
                    'If Not isSupervisor AndAlso rate = 0 Then
                    If (rate = 0 OrElse netRate / (1 - (minPercent / 100)) > rate OrElse netRate / (1 - (maxPercent / 100)) < rate OrElse netRate = 0) Then
                        rate = (netRate / (1 - (maxPercent / 100)))
                        txtRate.Text = rate.ToString()
                    ElseIf Not isSupervisor Then
                        txtRate.Text = rate
                    End If
                ElseIf Me.isUsuarioMixto AndAlso rate > 0 Then
                    txtNetRate.Text = (rate * (1 - (maxPercent / 100))).ToString()
                Else
                    IsValidData = False
                    If isSupervisor Then
                        Double.TryParse(txtRate.Text, rate)
                        If rate > 0 AndAlso netRate > 0 Then
                            IsValidData = True
                        End If
                    End If
                End If

                'IsValidData = (IsValidData AndAlso (isSupervisor OrElse ((netRate * (1 + (minPercent / 100))) <= rate)))
                ' IsValidData = IsValidData AndAlso (isSupervisor OrElse ( (netRate * (1 + (minPercent / 100)) < rate OrElse netRate * (1 + (maxPercent / 100)) > rate))

                If Not IsValidData Then Exit Function

            Next

            If Not isSupervisor Then

                For Each item As DataGridItem In Me.dgChild.Items
                    txtNetRate = item.Cells(2).FindControl("txtChildrenFareNR")
                    txtRate = item.Cells(1).FindControl("txtChildrenFare")

                    'Double.TryParse(txtRate.Text, rate)

                    Double.TryParse(txtNetRate.Text, netRate)
                    If Not Me.isUsuarioMixto Then
                        Double.TryParse(CType(item.Cells(1).FindControl("varRate"), HtmlInputHidden).Value, rate)
                        'If netRate > 0 AndAlso rate = 0 Then
                        If rate = 0 OrElse netRate / (1 - (minPercent / 100)) > rate OrElse netRate / (1 - (maxPercent / 100)) < rate OrElse netRate = 0 Then
                            rate = (netRate / (1 - (maxPercent / 100)))
                            txtRate.Text = rate.ToString()
                        Else
                            txtRate.Text = rate
                        End If

                        ' IsValidData = ((netRate * (1 + (minPercent / 100))) <= rate)
                        ' IsValidData = netRate * (1 + (minPercent / 100)) < rate OrElse netRate * (1 + (maxPercent / 100)) > rate

                        'If Not IsValidData Then Exit Function
                    Else
                        Double.TryParse(txtRate.Text, rate)
                        txtNetRate.Text = (rate * (1 - (maxPercent / 100))).ToString()
                    End If

                Next

                For Each item As DataGridItem In Me.dgTeen.Items
                    txtNetRate = item.Cells(2).FindControl("txtTeenFareNR")
                    txtRate = item.Cells(1).FindControl("txtTeenFare")

                    'Double.TryParse(txtRate.Text, rate)

                    Double.TryParse(txtNetRate.Text, netRate)
                    If Not Me.isUsuarioMixto Then
                        Double.TryParse(CType(item.Cells(1).FindControl("varRate"), HtmlInputHidden).Value, rate)
                        'If netRate > 0 AndAlso rate = 0 Then
                        If rate = 0 OrElse netRate / (1 - (minPercent / 100)) > rate OrElse netRate / (1 - (maxPercent / 100)) < rate OrElse netRate = 0 Then
                            rate = (netRate / (1 - (maxPercent / 100)))
                            txtRate.Text = rate.ToString()
                        Else
                            txtRate.Text = rate
                        End If

                        'IsValidData = ((netRate * (1 + (minPercent / 100))) <= rate)
                        ' IsValidData = netRate * (1 + (minPercent / 100)) < rate OrElse netRate * (1 + (maxPercent / 100)) > rate

                        ' If Not IsValidData Then Exit Function
                    Else
                        Double.TryParse(txtRate.Text, rate)
                        txtNetRate.Text = (rate * (1 - (maxPercent / 100))).ToString()
                    End If
                    

                Next

            End If

        End If

    End Function


    Protected ReadOnly Property IsSupervisor() As Boolean
        Get
            Return CType(Me.Page, PaginaBase).IsSupervisor
        End Get
    End Property

    Protected ReadOnly Property isUsuarioMixto() As Boolean
        Get
            Return CType(Me.Page, PaginaBase).IsUsuarioMixto
        End Get
    End Property


    Public Function getRatesExceptions() As DataTable
        Dim txtAdultFare As TextBox
        Dim txtAdultFareNR As TextBox
        Dim txtChildFare As TextBox
        Dim txtChildFareNR As TextBox
        Dim txtTeenFare As TextBox
        Dim txtTeenFareNR As TextBox

        'Dim cvAd As System.Web.UI.WebControls.CustomValidator
        'Dim cvCh As System.Web.UI.WebControls.CustomValidator
        Dim tabla As New DataTable
        Dim pos As Integer

        tabla.Columns.Add("TarifaAdultoExc", GetType(System.Double))
        tabla.Columns.Add("TarifaNinioExc", GetType(System.Double))
        tabla.Columns.Add("TarifaAdultoExcNR", GetType(System.Double))
        tabla.Columns.Add("TarifaNinioExcNR", GetType(System.Double))
        tabla.Columns.Add("TarifaAdolescenteExc", GetType(System.Double))
        tabla.Columns.Add("TarifaAdolescenteExcNR", GetType(System.Double))

        Dim dr As DataRow

        'For i As Integer = 0 To combinaciones - 1
        '    dr = tabla.NewRow
        '    txtAdultFare = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("txtAdultFare")
        '    'cvAd = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("cvErrAdults")
        '    txtChildFare = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("txtChildrenFare")
        '    'cvCh = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("cvErrChilds")
        '    dr("TarifaAdultoExc") = CInt(txtAdultFare.Text)
        '    dr("TarifaNinioExc") = CInt(txtChildFare.Text)
        '    tabla.Rows.Add(dr)
        'Next

        Dim value As Double = 0
        Dim dPrecio As Double

        For Each item As DataGridItem In dgAdult.Items
            dr = tabla.NewRow


            txtAdultFareNR = item.Cells(1).FindControl("txtAdultFareNR")
            Double.TryParse(txtAdultFareNR.Text, value)
            dr("TarifaAdultoExcNR") = value
            dr("TarifaNinioExcNR") = 0
            dr("TarifaAdolescenteExcNR") = 0

            txtAdultFare = item.Cells(2).FindControl("txtAdultFare")
            Double.TryParse(txtAdultFare.Text, value)
            dr("TarifaAdultoExc") = value
            dr("TarifaNinioExc") = 0
            dr("TarifaAdolescenteExc") = 0


            tabla.Rows.Add(dr)
            pos = 0
            For Each item2 As DataGridItem In dgChild.Items
                txtChildFareNR = item2.Cells(1).FindControl("txtChildrenFareNR")
                txtChildFare = item2.Cells(2).FindControl("txtChildrenFare")
                dr = tabla.NewRow

                Double.TryParse(txtAdultFare.Text, dPrecio)
                dr("TarifaAdultoExc") = dPrecio
                Double.TryParse(txtChildFare.Text, dPrecio)
                dr("TarifaNinioExc") = dPrecio
                Double.TryParse(txtAdultFareNR.Text, dPrecio)
                dr("TarifaAdultoExcNR") = dPrecio
                Double.TryParse(txtChildFareNR.Text, dPrecio)
                dr("TarifaNinioExcNR") = dPrecio


                'If txtAdultFare.Text <> "" Then
                '    dr("TarifaAdultoExc") = CDbl(txtAdultFare.Text)
                'Else
                '    dr("TarifaAdultoExc") = 0
                'End If

                'If txtChildFare.Text <> "" Then
                '    dr("TarifaNinioExc") = CDbl(txtChildFare.Text)
                'Else
                '    dr("TarifaNinioExc") = 0
                'End If

                'If txtAdultFareNR.Text <> "" Then
                '    dr("TarifaAdultoExcNR") = CDbl(txtAdultFareNR.Text)
                'Else
                '    dr("TarifaAdultoExcNR") = 0
                'End If

                'If txtChildFareNR.Text <> "" Then
                '    dr("TarifaNinioExcNR") = CDbl(txtChildFareNR.Text)
                'Else
                '    dr("TarifaNinioExcNR") = 0
                'End If

                dr("TarifaAdolescenteExc") = 0
                dr("TarifaAdolescenteExcNR") = 0
                If dgTeen.Items.Count > 0 Then
                    txtTeenFare = dgTeen.Items(pos).Cells(1).FindControl("txtTeenFare")
                    txtTeenFareNR = dgTeen.Items(pos).Cells(1).FindControl("txtTeenFareNR")
                    If Not txtTeenFare Is Nothing Then
                        Double.TryParse(txtTeenFare.Text, value)
                        dr("TarifaAdolescenteExc") = value 'If(Not String.IsNullOrEmpty(txtTeenFare.Text), value, 0)
                    End If
                    If Not txtTeenFare Is Nothing Then
                        Double.TryParse(txtTeenFareNR.Text, value)
                        dr("TarifaAdolescenteExcNR") = value 'If(Not String.IsNullOrEmpty(txtTeenFareNR.Text), value, 0)
                    End If

                    pos += 1
                End If
                tabla.Rows.Add(dr)
            Next
        Next
        Return tabla
    End Function

    'Private Sub dtgRestrictions_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
    '    If e.Item.ItemType = ListItemType.Header Then
    '        e.Item.Cells(RestrictionsDtgCols.AdultNumber).Text = PortalCulture.GetString("00060")
    '        e.Item.Cells(RestrictionsDtgCols.ChildNumber).Text = PortalCulture.GetString("00061")
    '        e.Item.Cells(RestrictionsDtgCols.AdultFare).Text = PortalCulture.GetString("00134")
    '        e.Item.Cells(RestrictionsDtgCols.ChildFare).Text = PortalCulture.GetString("00135")
    '        e.Item.Cells(RestrictionsDtgCols.TotalFare).Text = PortalCulture.GetString("00136")
    '    End If

    '    Dim txtAdultFare As TextBox
    '    Dim txtChildFare As TextBox
    '    Dim lblAdult As Label
    '    Dim lblChild As Label
    '    Dim valAdt As RegularExpressionValidator
    '    Dim valChd As RegularExpressionValidator

    '    txtAdultFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtAdultFare")
    '    txtChildFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtChildrenFare")

    '    lblAdult = e.Item.Cells(RestrictionsDtgCols.AdultNumber).FindControl("lblAdults")
    '    lblChild = e.Item.Cells(RestrictionsDtgCols.ChildNumber).FindControl("lblChildren")



    '    valAdt = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("valAdultExtraPrice")
    '    valChd = e.Item.Cells(RestrictionsDtgCols.ChildFare).FindControl("valChildrenExtraPrice")

    '    If Not valAdt Is Nothing Then valAdt.Text = PortalCulture.GetString("00240")
    '    If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00240")

    '    If Not txtAdultFare Is Nothing AndAlso lblAdult.Text <= 0 Then
    '        txtAdultFare.Text = 0
    '        txtAdultFare.Enabled = False
    '        txtAdultFare.Visible = False

    '    End If
    '    If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
    '        txtChildFare.Text = 0
    '        txtChildFare.Enabled = False
    '        txtChildFare.Visible = False
    '        If lblChild.Text <= 0 Then
    '            lblChild.Text = ""
    '        End If
    '    End If

    '    'Mostramos la tarifa total 
    '    If Not txtAdultFare Is Nothing And Not txtChildFare Is Nothing Then
    '        Dim lblTotalFare As Label
    '        lblTotalFare = e.Item.Cells(RestrictionsDtgCols.TotalFare).FindControl("lblTotal")
    '        If Not lblTotalFare Is Nothing Then
    '            txtAdultFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
    '            txtChildFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
    '            lblTotalFare.Text = CDec(CDec(txtAdultFare.Text) + CDec(txtChildFare.Text)).ToString("C")
    '        End If
    '    End If

    '    Dim chkActive As CheckBox
    '    chkActive = e.Item.Cells(RestrictionsDtgCols.Tools).FindControl("chkActive")
    '    Dim dgi As DataGridItem
    '    If Not chkActive Is Nothing Then
    '        dgi = CType(chkActive.Parent.Parent, DataGridItem)
    '        If e.Item.Cells(RestrictionsDtgCols.RestrictionId).Text = "0" Then
    '            chkActive.Checked = False
    '            If dgi.ItemType = ListItemType.AlternatingItem Then
    '                dgi.CssClass = "dgAlternate"
    '                dgi.Attributes.Add("itemType", "AlternatingItem")
    '            Else
    '                dgi.CssClass = "dgitem"
    '                dgi.Attributes.Add("itemType", "Item")
    '            End If
    '        Else
    '            chkActive.Checked = True
    '            dgi = CType(chkActive.Parent.Parent, DataGridItem)
    '            dgi.CssClass = "dgSelected"
    '        End If
    '    End If
    'End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
    End Sub

    Private Sub loadResources()
        Me.lblLunes.Text = PortalCulture.GetString("00300")
        Me.lblMartes.Text = PortalCulture.GetString("00301")
        Me.lblMiercoles.Text = PortalCulture.GetString("00302")
        Me.lblJueves.Text = PortalCulture.GetString("00303")
        Me.lblViernes.Text = PortalCulture.GetString("00304")
        Me.lblSabado.Text = PortalCulture.GetString("00305")
        Me.lblDomingo.Text = PortalCulture.GetString("00306")
    End Sub

    Private Sub dgAdult_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAdult.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("00060")
            e.Item.Cells(1).Text = PortalCulture.GetString("01125")
            e.Item.Cells(2).Text = PortalCulture.GetString("01124")
        End If


        Dim lblAdult As Label
        lblAdult = e.Item.Cells(0).FindControl("lblAdults")


        Dim lblAdultValMax As Label
        Dim lblAdultValMin As Label
        lblAdultValMax = e.Item.Cells(5).FindControl("lblAdultValMax")
        lblAdultValMin = e.Item.Cells(5).FindControl("lblAdultValMin")

        If Not lblAdultValMax Is Nothing Then
            lblAdultValMax.Style.Add("display", "none")
            lblAdultValMax.Text = PortalCulture.GetString("01135")
        End If
        If Not lblAdultValMin Is Nothing Then
            lblAdultValMin.Style.Add("display", "none")
            lblAdultValMin.Text = PortalCulture.GetString("01136")
        End If

        Dim txtAdultFareNR As TextBox
        'Dim lblAdultNR As Label
        Dim valAdtNR As RegularExpressionValidator
        txtAdultFareNR = e.Item.Cells(1).FindControl("txtAdultFareNR")
        valAdtNR = e.Item.Cells(1).FindControl("valAdultExtraPriceNR")
        If Not valAdtNR Is Nothing Then valAdtNR.Text = PortalCulture.GetString("00240")
        If Not txtAdultFareNR Is Nothing AndAlso lblAdult.Text <= 0 Then
            txtAdultFareNR.Text = 0
            txtAdultFareNR.Enabled = False
            txtAdultFareNR.Visible = False
        End If


        Dim txtAdultFare As TextBox
        Dim valAdt As RegularExpressionValidator
        txtAdultFare = e.Item.Cells(2).FindControl("txtAdultFare")
        valAdt = e.Item.Cells(2).FindControl("valAdultExtraPrice")
        If Not valAdt Is Nothing Then valAdt.Text = PortalCulture.GetString("00240")
        If Not txtAdultFare Is Nothing AndAlso lblAdult.Text <= 0 Then
            txtAdultFare.Text = 0
            txtAdultFare.Enabled = False
            txtAdultFare.Visible = False
        End If

        If Not txtAdultFareNR Is Nothing AndAlso Not txtAdultFare Is Nothing Then
            txtAdultFareNR.Attributes.Add("onChange", "javascript:CheckValContract('" & m_TextBoxPorcMin & "','" & m_TextBoxPorcMax & "','" & txtAdultFareNR.ClientID & "','" & txtAdultFare.ClientID & "','" & lblAdultValMax.ClientID & "','" & lblAdultValMin.ClientID & "' ,'" & PlusTaxProperty & "', '" & EcotasaProperty & "')")
            'txtAdultFare.Attributes.Add("onChange", "javascript:CheckValContract('" & m_TextBoxPorcMin & "','" & m_TextBoxPorcMax & "','" & txtAdultFareNR.ClientID & "','" & txtAdultFare.ClientID & "','" & lblAdultValMax.ClientID & "','" & lblAdultValMin.ClientID & "')")
        End If

        Dim chkActive As CheckBox
        chkActive = e.Item.Cells(4).FindControl("chkActive")
        Dim dgi As DataGridItem
        If Not chkActive Is Nothing Then
            dgi = CType(chkActive.Parent.Parent, DataGridItem)
            If e.Item.Cells(3).Text = "0" Then
                chkActive.Checked = False
                If dgi.ItemType = ListItemType.AlternatingItem Then
                    dgi.CssClass = "dgAlternate"
                    dgi.Attributes.Add("itemType", "AlternatingItem")
                Else
                    dgi.CssClass = "Dgitem"
                    dgi.Attributes.Add("itemType", "Item")
                End If
            Else
                chkActive.Checked = True
                dgi = CType(chkActive.Parent.Parent, DataGridItem)
                dgi.CssClass = "dgSelected"
            End If
        End If
    End Sub

    Private Sub dgChild_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgChild.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("00061")
            e.Item.Cells(1).Text = PortalCulture.GetString("01125")
            e.Item.Cells(2).Text = PortalCulture.GetString("01124")
        End If

        Dim lblChild As Label
        lblChild = e.Item.Cells(0).FindControl("lblChildren")

        Dim lblChildValMax As Label
        Dim lblChildValMin As Label
        lblChildValMax = e.Item.Cells(5).FindControl("lblChildValMax")
        lblChildValMin = e.Item.Cells(5).FindControl("lblChildValMin")
        If Not lblChildValMax Is Nothing Then
            lblChildValMax.Style.Add("display", "none")
            lblChildValMax.Text = PortalCulture.GetString("01135")
        End If
        If Not lblChildValMin Is Nothing Then
            lblChildValMin.Style.Add("display", "none")
            lblChildValMin.Text = PortalCulture.GetString("01136")
        End If

        Dim txtChildFareNR As TextBox
        Dim valChdNR As RegularExpressionValidator
        txtChildFareNR = e.Item.Cells(1).FindControl("txtChildrenFareNR")
        valChdNR = e.Item.Cells(1).FindControl("valChildrenExtraPriceNR")
        If Not valChdNR Is Nothing Then valChdNR.Text = PortalCulture.GetString("00240")
        If Not txtChildFareNR Is Nothing AndAlso lblChild.Text <= 0 Then
            txtChildFareNR.Text = 0
            txtChildFareNR.Enabled = False
            txtChildFareNR.Visible = False
            If lblChild.Text <= 0 Then
                lblChild.Text = ""
            End If
        End If

        Dim txtChildFare As TextBox
        Dim valChd As RegularExpressionValidator
        txtChildFare = e.Item.Cells(2).FindControl("txtChildrenFare")
        valChd = e.Item.Cells(2).FindControl("valChildrenExtraPrice")
        If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00240")
        If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
            txtChildFare.Text = 0
            txtChildFare.Enabled = False
            txtChildFare.Visible = False
            If lblChild.Text <= 0 Then
                lblChild.Text = ""
            End If
        End If

        'Validacion del porcentaje maximo y minimo de ganancia UV
        If Not txtChildFareNR Is Nothing AndAlso Not txtChildFare Is Nothing Then
            txtChildFareNR.Attributes.Add("onChange", "javascript:CheckValContract('" & m_TextBoxPorcMin & "','" & m_TextBoxPorcMax & "','" & txtChildFareNR.ClientID & "','" & txtChildFare.ClientID & "','" & lblChildValMax.ClientID & "','" & lblChildValMin.ClientID & "','" & PlusTaxProperty & "', '" & EcotasaProperty & "')")
            'txtChildFare.Attributes.Add("onChange", "javascript:CheckValContract('" & m_TextBoxPorcMin & "','" & m_TextBoxPorcMax & "','" & txtChildFareNR.ClientID & "','" & txtChildFare.ClientID & "','" & lblChildValMax.ClientID & "','" & lblChildValMin.ClientID & "')")
        End If

        Dim chkActive As CheckBox
        chkActive = e.Item.Cells(4).FindControl("chkActive")
        Dim dgi As DataGridItem
        If Not chkActive Is Nothing Then
            dgi = CType(chkActive.Parent.Parent, DataGridItem)
            If e.Item.Cells(3).Text = "0" Then
                chkActive.Checked = False
                If dgi.ItemType = ListItemType.AlternatingItem Then
                    dgi.CssClass = "dgAlternate"
                    dgi.Attributes.Add("itemType", "AlternatingItem")
                Else
                    dgi.CssClass = "Dgitem"
                    dgi.Attributes.Add("itemType", "Item")
                End If
            Else
                chkActive.Checked = True
                dgi = CType(chkActive.Parent.Parent, DataGridItem)
                dgi.CssClass = "dgSelected"
            End If
        End If
    End Sub

    Private Sub dgTeen_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgTeen.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("01282")
            e.Item.Cells(1).Text = PortalCulture.GetString("01125")
            e.Item.Cells(2).Text = PortalCulture.GetString("01124")
        End If

        Dim lblChild As Label
        lblChild = e.Item.Cells(0).FindControl("lblChildren")

        Dim lblChildValMax As Label
        Dim lblChildValMin As Label
        lblChildValMax = e.Item.Cells(5).FindControl("lblTeenValMax")
        lblChildValMin = e.Item.Cells(5).FindControl("lblTeenValMin")
        If Not lblChildValMax Is Nothing Then
            lblChildValMax.Style.Add("display", "none")
            lblChildValMax.Text = PortalCulture.GetString("01135")
        End If
        If Not lblChildValMin Is Nothing Then
            lblChildValMin.Style.Add("display", "none")
            lblChildValMin.Text = PortalCulture.GetString("01136")
        End If

        Dim txtChildFareNR As TextBox
        Dim valChdNR As RegularExpressionValidator
        txtChildFareNR = e.Item.Cells(1).FindControl("txtTeenFareNR")
        valChdNR = e.Item.Cells(1).FindControl("valTeenExtraPriceNR")
        If Not valChdNR Is Nothing Then valChdNR.Text = PortalCulture.GetString("00240")
        If Not txtChildFareNR Is Nothing AndAlso lblChild.Text <= 0 Then
            txtChildFareNR.Text = 0
            txtChildFareNR.Enabled = False
            txtChildFareNR.Visible = False
            If lblChild.Text <= 0 Then
                lblChild.Text = ""
            End If
        End If

        Dim txtChildFare As TextBox
        Dim valChd As RegularExpressionValidator
        txtChildFare = e.Item.Cells(2).FindControl("txtTeenFare")
        valChd = e.Item.Cells(2).FindControl("valTeenExtraPrice")
        If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00240")
        If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
            txtChildFare.Text = 0
            txtChildFare.Enabled = False
            txtChildFare.Visible = False
            If lblChild.Text <= 0 Then
                lblChild.Text = ""
            End If
        End If

        If Not txtChildFareNR Is Nothing AndAlso Not txtChildFare Is Nothing Then
            txtChildFareNR.Attributes.Add("onChange", "javascript:CheckValContract('" & m_TextBoxPorcMin & "','" & m_TextBoxPorcMax & "','" & txtChildFareNR.ClientID & "','" & txtChildFare.ClientID & "','" & lblChildValMax.ClientID & "','" & lblChildValMin.ClientID & "','" & PlusTaxProperty & "', '" & EcotasaProperty & "')")
            'txtChildFare.Attributes.Add("onChange", "javascript:CheckValContract('" & m_TextBoxPorcMin & "','" & m_TextBoxPorcMax & "','" & txtChildFareNR.ClientID & "','" & txtChildFare.ClientID & "','" & lblChildValMax.ClientID & "','" & lblChildValMin.ClientID & "')")
        End If

        Dim chkActive As CheckBox
        chkActive = e.Item.Cells(4).FindControl("chkActive")
        Dim dgi As DataGridItem
        If Not chkActive Is Nothing Then
            dgi = CType(chkActive.Parent.Parent, DataGridItem)
            If e.Item.Cells(3).Text = "0" Then
                chkActive.Checked = False
                If dgi.ItemType = ListItemType.AlternatingItem Then
                    dgi.CssClass = "dgAlternate"
                    dgi.Attributes.Add("itemType", "AlternatingItem")
                Else
                    dgi.CssClass = "Dgitem"
                    dgi.Attributes.Add("itemType", "Item")
                End If
            Else
                chkActive.Checked = True
                dgi = CType(chkActive.Parent.Parent, DataGridItem)
                dgi.CssClass = "dgSelected"
            End If
        End If
    End Sub

    Public ReadOnly Property MinStorageControlId() As String
        Get
            Return Me.varMinPercent.ClientID
        End Get
    End Property

    Public ReadOnly Property MaxStorageControlId() As String
        Get
            Return Me.varMaxPercent.ClientID
        End Get
    End Property


    Private Sub grid_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgAdult.PreRender, dgChild.PreRender, dgTeen.PreRender

        CType(sender, DataGrid).Columns(2).Visible = (Me.IsSupervisor Or Me.isUsuarioMixto)
        CType(sender, DataGrid).Columns(1).Visible = Not Me.isUsuarioMixto

    End Sub
    Public Function IsFareValuesEqualTo(ByVal target As String, ByVal value As Double, ByVal isNetRate As Boolean) As Boolean

        Dim flag As Boolean = True
        Dim input As TextBox
        Dim grid As DataGrid = Me.FindControl("dg" + If(target.ToLower() = "children", "Child", target))
        Dim wild As Integer = 0

        If grid IsNot Nothing Then
            For Each row As DataGridItem In grid.Items
                If row.ItemType = ListItemType.AlternatingItem OrElse row.ItemType = ListItemType.Item Then
                    input = row.FindControl("txt" + target + "Fare" + If(isNetRate, "NR", ""))
                    If input IsNot Nothing Then
                        Double.TryParse(input.Text, wild)
                        flag = (wild = value)
                    End If
                End If
                If Not flag Then Exit For
            Next
        End If
        Return flag

    End Function


End Class
