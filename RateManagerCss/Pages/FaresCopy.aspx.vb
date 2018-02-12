Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Partial Class FaresCopy
    Inherits PaginaBase
    'Private Property ratesplans() As String
    '    Get
    '        Return viewstate("_ratesplans")
    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_ratesplans") = Value
    '    End Set
    'End Property
    Enum dgcolumns
        Copy
        room
        nameroom
        StartDate
        EndDate
        Rate2A0C
        NewStartDate
        NewEndDate
        IdTarifa
    End Enum

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents imgCalendar As System.Web.UI.WebControls.Image
    Protected WithEvents txtFecha1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFecha2 As System.Web.UI.WebControls.TextBox

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
        'Al copiar una tarifa se copiará automaticamente sus links
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        'Probar con hotel ke no tenga rateplan
        If Not IsPostBack Then
            loadAllRatesplans()
            CargaFechas()
            GetData()
            '   ddlRatePlan.Attributes.Add("onChange", "javascript:showRatePlanName('" & ddlRatePlan.ClientID & "','" & ratesplans & "','" & lblRatePlanName.ClientID & "')")
            lblAllSave.Visible = False
        End If
        chkAll.Attributes.Add("onclick", "javascript:CheckAll('" & chkAll.ClientID & "','" & Me.dgRates.ClientID & "')")
    End Sub
    Public Function GetIDtxtInicio(ByVal o As Integer) As String
        Dim txt As TextBox
        txt = dgRates.Items(o).FindControl("txtinicio")
        If Not txt Is Nothing Then Return txt.ClientID
        Return "txtFecha"
    End Function

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

    Private Function loadAllRatesplans() As RatePlanData
        Dim ds As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(Me.cInfoActual.Hotel, idAsociacion:=idAsoc, DeleteFilter:=1)
        End With

        If MyBase.IdCorporativoUserChain = 4 AndAlso (MyBase.IsHotel Or MyBase.IsUsuarioHotel) Then
            ds = RatePlanFilter("C", ds)
        End If

        Dim links As New LinkRatePlanData
        With New LinkRatePlanFacade
            links = .getList(Me.cInfoActual.Hotel)
        End With
        ds.Tables(ds.RATEPLAN_TABLE).Columns.Add("test", GetType(System.String), "substring(" & ds.FIELD_CODIGOTARIFA & "+' - '+" & ds.FIELD_NAME & ",1,25)")
        ''eliminar los ratesplan que ya tienen links
        Dim dv As DataView
        For Each r As DataRow In ds.Tables(ds.RATEPLAN_TABLE).Rows
            dv = links.Tables(links.TABLE_LINKRATEPLAN).DefaultView
            dv.RowFilter = links.FIELD_TargetRatePlan & "='" & r(ds.FIELD_IDRATEPLAN) & "'"
            If dv.Count > 0 Or r(ds.FIELD_SEGMENT) = "K" Then
                r.Delete()
            End If
        Next
        ds.Tables(ds.RATEPLAN_TABLE).AcceptChanges()
        ddlRatePlan.DataTextField = "test" 'ds.FIELD_CODIGOTARIFA
        ddlRatePlan.DataValueField = ds.FIELD_IDRATEPLAN
        ddlRatePlan.DataSource = ds
        ddlRatePlan.DataBind()
        'RatesPlans = ""
        'For i As Integer = 0 To ds.Tables(ds.RATEPLAN_TABLE).Rows.Count - 1
        '    If ds.Tables(ds.RATEPLAN_TABLE).Rows(i).IsNull(ds.FIELD_NAME) Then
        '        RatesPlans &= "--" & "//"
        '    Else
        '        RatesPlans &= ds.Tables(ds.RATEPLAN_TABLE).Rows(i).Item(ds.FIELD_NAME).ToString & "//"
        '    End If
        'Next

        Return ds
    End Function

    Private Sub btnLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Me.lblErrorOverlapped.Visible = False
        Me.lblAllSave.Visible = False
        GetData()
    End Sub

    Private Sub GetData()
        If ddlRatePlan.Items.Count > 0 AndAlso ddlRatePlan.SelectedItem.Value.Trim <> "" Then
            lblNoRates.Visible = False
            Me.dgRates.Visible = True
            Dim ds As FaresData
            Dim st As Date = New Date(Me.ddlYear1.SelectedValue, Me.ddlMonth1.SelectedIndex + 1, 1)
            Dim ed As Date = New Date(Me.ddlYear2.SelectedValue, Me.ddlMonth2.SelectedIndex + 1, Date.DaysInMonth(Me.ddlYear2.SelectedValue, Me.ddlMonth2.SelectedIndex + 1))
            With New FaresSystem
                ds = .GetTarifasByRatePlan(Me.cInfoActual.Hotel, Me.ddlRatePlan.SelectedItem.Value.Trim, st, ed)
            End With
            Dim ci As System.Globalization.CultureInfo
            ci = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
            Me.dgRates.DataSource = ds
            Me.dgRates.DataBind()
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
            If dgRates.Items.Count = 0 Then
                lblNoRates.Visible = True
                Me.dgRates.Visible = False
            End If
        End If

    End Sub
    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadculture()
        'lblRatePlanName.Text = "-"
        'If ratesplans <> "" Then
        '    lblRatePlanName.Text = ratesplans.Split("//")((ddlRatePlan.SelectedIndex) * 2)
        'End If

    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Me.lblErrorOverlapped.Visible = False
        lblAllSave.Visible = False
        Dim ds As New FaresData
        Dim dr As DataRow
        Dim chk As CheckBox
        Dim txt1 As TextBox
        Dim txt2 As TextBox
        Dim sData As String = ""
        Dim sMsg As String = ""

        For Each item As DataGridItem In Me.dgRates.Items
            chk = item.FindControl("chkChange")
            If chk.Checked Then
                txt1 = item.FindControl("txtinicio")
                txt2 = item.FindControl("txtfin")
                'validar fechas f2>f1, cambiar sp y capas
                Try
                    If CDate(txt2.Text) >= CDate(txt1.Text) Then
                        dr = ds.Tables(ds.FARES_TABLE).NewRow
                        dr(ds.PKIDFARES_FIELD) = CInt(item.Cells(dgcolumns.IdTarifa).Text)
                        dr(ds.STARTDATE_FIELD) = CDate(txt1.Text)
                        dr(ds.ENDDATE_FIELD) = CDate(txt2.Text)
                        ds.Tables(ds.FARES_TABLE).Rows.Add(dr)
                    End If
                Catch ex As Exception
                    'error en las fechas
                End Try
            End If
        Next
        If ds.Tables(ds.FARES_TABLE).Rows.Count > 0 Then
            With New FaresSystem
                If .CopyFares(ds) <> 0 Then
                    Me.lblErrorOverlapped.Visible = True
                Else
                    sData = Util.Utility.GetXml(ds.FARES_TABLE, "UpdateCopyRate", ds)
                    sMsg = String.Format("Copia de tarifas con Id {0}, para la fecha de {1} a {2}", dr(ds.PKIDFARES_FIELD), txt1.Text, txt2.Text)
                    Me.guardalog("/Pages/PrepagoRatesPlan.aspx", PaginaBase.acciones.Crear, sMsg, "", "", sData)
                    lblAllSave.Visible = True
                End If
            End With
            Me.GetData()
        End If
    End Sub

    Private Sub dgRates_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRates.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Cells(dgcolumns.StartDate).Text = CDate(e.Item.Cells(dgcolumns.StartDate).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(dgcolumns.EndDate).Text = CDate(e.Item.Cells(dgcolumns.EndDate).Text).ToString("MMM/dd/yyyy")

            If e.Item.Cells(dgcolumns.Rate2A0C).Text <> "&nbsp;" Then
                e.Item.Cells(dgcolumns.Rate2A0C).Text = FCurrency(e.Item.Cells(dgcolumns.Rate2A0C).Text, 2)
            Else
                e.Item.Cells(dgcolumns.Rate2A0C).Text = "NA"
            End If
            Dim img As HtmlImage
            img = e.Item.FindControl("imgCalInicio")
            Dim txt1 As TextBox = e.Item.FindControl("txtinicio")
            txt1.Text = CDate(e.Item.Cells(dgcolumns.StartDate).Text).AddYears(1).ToString("MM/dd/yyyy")
            Dim txt2 As TextBox = e.Item.FindControl("txtfin")
            txt2.Text = CDate(e.Item.Cells(dgcolumns.EndDate).Text).AddYears(1).ToString("MM/dd/yyyy")
            If Not img Is Nothing Then
                img.Attributes.Add("onclick", "if(self.gfPop)gfPop.fPopCalendar(" & txt2.ClientID & "," & txt1.ClientID & ");return false;")
            End If
            img = e.Item.FindControl("imgCalFin")
            img.Attributes.Add("onclick", "if(self.gfPop)gfPop.fPopCalendar1(" & txt2.ClientID & ");return false;")
            Dim lbl As New Label
            lbl.Text = e.Item.Cells(dgcolumns.room).Text
            lbl.ToolTip = e.Item.Cells(dgcolumns.nameroom).Text
            e.Item.Cells(dgcolumns.room).Controls.Add(lbl)
        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.room).Text = PortalCulture.GetString("00170")
            e.Item.Cells(dgcolumns.StartDate).Text = PortalCulture.GetString("00276")
            e.Item.Cells(dgcolumns.EndDate).Text = PortalCulture.GetString("00277")
            e.Item.Cells(dgcolumns.Copy).Text = PortalCulture.GetString("00487")
            e.Item.Cells(dgcolumns.Rate2A0C).Text = PortalCulture.GetString("00489")
            e.Item.Cells(dgcolumns.NewStartDate).Text = PortalCulture.GetString("00491")
            e.Item.Cells(dgcolumns.NewEndDate).Text = PortalCulture.GetString("00492")
        End If

    End Sub
    Private Sub CargaFechas()
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        ddlMonth1.Items.Clear()
        ddlMonth2.Items.Clear()
        For i As Integer = 1 To 12
            ddlMonth1.Items.Add(MonthName(i, True))
            ddlMonth2.Items.Add(MonthName(i, True))
        Next

        ddlYear1.Items.Clear()
        ddlYear2.Items.Clear()
        For i As Integer = Now.Year - 4 To Now.Year + 1
            ddlYear1.Items.Add(New ListItem(i, i))
            ddlYear2.Items.Add(New ListItem(i, i))
        Next

        ddlMonth1.SelectedIndex = Now.Month - 1
        ddlMonth2.SelectedIndex = Now.Month - 1
        ddlYear1.SelectedValue = Now.Year - 1
        ddlYear2.SelectedValue = Now.Year
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub

    Private Sub loadculture()
        lblNoRates.Text = PortalCulture.GetString("01463")
        lblAllSave.Text = PortalCulture.GetString("00515")
        Me.lblRatePlan.Text = PortalCulture.GetString("00016", True) & " "
        Me.lblDesde.Text = PortalCulture.GetString("00108", True) & " "
        Me.lblHasta.Text = PortalCulture.GetString("00109", True) & " "
        Me.btnLoad.Text = PortalCulture.GetString("00149")
        Me.chkAll.Text = PortalCulture.GetString("00484")
        Me.btnSave.Text = PortalCulture.GetString("A00153")
        Me.lblErrorOverlapped.Text = PortalCulture.GetString("00486")
        Me.dgRates.Columns(dgcolumns.room).HeaderText = PortalCulture.GetString("00170")
        Me.dgRates.Columns(dgcolumns.StartDate).HeaderText = PortalCulture.GetString("00276")
        Me.dgRates.Columns(dgcolumns.EndDate).HeaderText = PortalCulture.GetString("00277")
        Me.dgRates.Columns(dgcolumns.Copy).HeaderText = PortalCulture.GetString("00487")
        Me.dgRates.Columns(dgcolumns.Rate2A0C).HeaderText = PortalCulture.GetString("00489")
        Me.dgRates.Columns(dgcolumns.NewStartDate).HeaderText = PortalCulture.GetString("00491")
        Me.dgRates.Columns(dgcolumns.NewEndDate).HeaderText = PortalCulture.GetString("00492")
        Me.lblTitle.Text = PortalCulture.GetString("00488")
        Me.lblSelect.Text = PortalCulture.GetString("00490")

    End Sub





End Class
'Sabado 20 enero 2007.
'procedimiento creado spTarifasGetByIdRatePlan, spTarifasCopy
