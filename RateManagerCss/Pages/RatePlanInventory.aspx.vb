Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.Hotel.Facade

Partial Class RatePlanInventory
    Inherits PaginaBase

    Private Property RatePlan() As String
        Get
            Return viewstate("Rp")
        End Get
        Set(ByVal Value As String)
            viewstate("Rp") = Value
        End Set
    End Property
    Dim links As New LinkRatePlanData
    Dim ds As RatePlanData
    Enum dgdependentscolumns
        TargetRatePlan
        RateCodeTarget
        targetname
        Avail
        SoldOutPerc
        Percent
        SourceRatePlan
        SourceRatePlanCode
        texto
    End Enum
    


#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnload As System.Web.UI.WebControls.Button
    Protected WithEvents dgDependents As System.Web.UI.WebControls.DataGrid
    Protected WithEvents hplIndDepRoom As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtIndepRooms As System.Web.UI.WebControls.TextBox

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region
    Private valor As String

    Sub LeeInventario()
        Dim data As RoomsInventoryData
        Dim dt As DateTime
        Dim dt2 As DateTime
        Dim dr As System.Data.DataRow()

        inputMaxInventario.Value = 1000
        dt = New Date(Now.Year, Now.Month, 1)
        dt2 = New Date(Now.Year, Now.Month, Now.DaysInMonth(Now.Year, Now.Month))
        With New RoomsInventoryFacade
            data = .getAllRoomsInventoryByDate_Data(Me.cInfoActual.Hotel, dt, dt2)
            If Not dsEmpty(data) Then
                dr = data.Tables(0).Select("habitaciones >=0", "Habitaciones desc")
                If dr.Length > 0 Then
                    inputMaxInventario.Value = dr(0)(RoomsInventoryData.FLD_NUMBER_ROOMS) 'data.Tables(0).Rows(0)(RoomsInventoryData.FLD_NUMBER_ROOMS)
                End If
            End If
        End With
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        LeeInventario()
        If Not IsPostBack Then
            loaddatos()
        End If


      
    End Sub

    Private Sub loaddatos()
        Dim idAsoc As Integer = Me.GetIdAsociation


        With New LinkRatePlanFacade
            links = .getList(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With
        links.Tables(LinkRatePlanData.TABLE_LINKRATEPLAN).Columns.Add("texto", GetType(System.String), "substring(" & LinkRatePlanData.FIELD_TargetRatePlan & "+' - '+" & "targetname,1,25)")

        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, IncluirPaquetesSegmentoK:=1, incluirNetRatesPlan:=1, idAsociacion:=IdAsoc)
        End With
        

        ''eliminar los ratesplan que ya tienen links
        Dim dv As DataView
        For Each r As DataRow In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            dv = links.Tables(LinkRatePlanData.TABLE_LINKRATEPLAN).DefaultView
            dv.RowFilter = LinkRatePlanData.FIELD_TargetRatePlan & "='" & r(RatePlanData.FIELD_IDRATEPLAN) & "'"
            If dv.Count > 0 Then
                r.Delete()
            End If
        Next
        ds.Tables(RatePlanData.RATEPLAN_TABLE).AcceptChanges()
        ds.Tables(RatePlanData.RATEPLAN_TABLE).Columns.Add("texto", GetType(System.String), "substring(" & RatePlanData.FIELD_CODIGOTARIFA & "+ ' - ' +" & RatePlanData.FIELD_NAME & ",1,25)")
        dgIndependent.DataKeyField = "idRatePlan"
        dgIndependent.DataSource = ds
        dgIndependent.DataBind()
        txtRatesPlans.Text = ""
        For Each r As DataRow In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            txtRatesPlans.Text &= "/" & r("QtyRooms").ToString
        Next

    End Sub

    Private Sub dgDependent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim lnk As HyperLink, txt As TextBox
            lnk = e.Item.FindControl("lnkDepRooms")
            txt = e.Item.FindControl("txtDepRooms")

            txt.Style.Add("display", "none")
            lnk.Style.Add("display", "block")
            Dim dv As DataView
            dv = ds.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView
            dv.RowFilter = RatePlanData.FIELD_IDRATEPLAN & "='" & e.Item.Cells(dgdependentscolumns.SourceRatePlan).Text & "'"

            If dv.Count > 0 Then
                e.Item.Cells(dgdependentscolumns.Avail).Text = dv(0)("QtyRooms").ToString
            End If

            If e.Item.Cells(dgdependentscolumns.SoldOutPerc).Text <> "&nbsp;" Then
                Dim solout As Double
                Double.TryParse(e.Item.Cells(dgdependentscolumns.SoldOutPerc).Text, solout)
                txt.Text = IIf(solout > 0, e.Item.Cells(dgdependentscolumns.SoldOutPerc).Text, "")
                lnk.Text = e.Item.Cells(dgdependentscolumns.SoldOutPerc).Text
                If solout = 0 Then
                    txt.Style.Add("display", "block")
                    lnk.Style.Add("display", "none")
                End If
                Try
                    e.Item.Cells(dgdependentscolumns.Avail).Text = CDbl(lnk.Text) * CDbl(e.Item.Cells(dgdependentscolumns.Avail).Text) \ 100
                Catch ex As Exception
                    e.Item.Cells(dgdependentscolumns.Avail).Text = "-"
                End Try
            Else
                lnk.Text = "##"
            End If
            Dim lbl As New Label
            lbl.Text = e.Item.Cells(dgdependentscolumns.texto).Text 'e.Item.Cells(dgdependentscolumns.RateCodeTarget).Text
            'If e.Item.Cells(dgdependentscolumns.targetname).Text <> "&nbsp;" Then
            '    lbl.ToolTip = e.Item.Cells(dgdependentscolumns.targetname).Text
            'End If
            e.Item.Cells(dgdependentscolumns.RateCodeTarget).Controls.Add(lbl)
            lnk.NavigateUrl = "javascript:ShowTxt('" & txt.ClientID & "','" & lnk.ClientID & "')"
            'aqui hay ke mandarle los datos para el log.
            txt.Attributes.Add("onchange", "javascript:SaveDepAva('" & e.Item.Cells(dgdependentscolumns.TargetRatePlan).Text & "','" & e.Item.Cells(dgdependentscolumns.SourceRatePlan).Text & "','" & txt.ClientID & "','" & e.Item.Cells(dgdependentscolumns.RateCodeTarget).Text & "','" & e.Item.Cells(dgdependentscolumns.SourceRatePlanCode).Text & "')")
        End If

    End Sub

    Private Sub dgIndependent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dgIndependent.ItemDataBound
        Dim dv As DataView
        Dim dg As DataGrid
        'Dim htls As String()
        'Dim tshow As HtmlTable
        If e.Item.ItemType = ListItemType.Header Then
            Dim ddl As DropDownList = e.Item.FindControl("ddlIndependents")
            ddl.DataValueField = "idRatePlan"
            ddl.DataTextField = "texto"
            ddl.DataSource = ds
            ddl.DataBind()
            ddl.SelectedIndex = 0
            'Dim lbl As Label
            'lbl = e.Item.FindControl("lblRPNameIndependent")
            'Dim strName As String = ""
            'For Each drRP As DataRow In ds.Tables(ds.RATEPLAN_TABLE).Rows
            '    If drRP.IsNull(ds.FIELD_NAME) Then
            '        strName &= "--" & "//"
            '    Else
            '        strName &= drRP(ds.FIELD_NAME).ToString & "//"
            '    End If
            'Next
            'If strName.Trim <> "" Then
            '    lbl.Text = strName.Split("//")(0)
            'End If


            Dim txt As TextBox

            txt = e.Item.FindControl("txtIndRooms")



            dv = ds.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView
            dv.RowFilter = RatePlanData.FIELD_IDRATEPLAN & "='" & ddl.SelectedValue & "'"

            If dv.Count > 0 Then
                txt.Text = dv(0)("QtyRooms").ToString
            End If

            ddl.Attributes.Add("onchange", "showLinks('" & ddl.ClientID & "','','" & txt.ClientID & "');")


            btnCancel.Attributes.Add("onclick", "showLinks('" & ddl.ClientID & "','','" & txt.ClientID & "');")


            'aqui hay ke mandarle los datos para el log.
            txt.Attributes.Add("onchange", "javascript:SaveIndAva('" & ddl.ClientID & "','" & txt.ClientID & "')")
        End If

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            If links.Tables.Count > 0 Then
                dv = links.Tables(LinkRatePlanData.TABLE_LINKRATEPLAN).DefaultView
                dv.RowFilter = LinkRatePlanData.FIELD_SourceRatePlan & "='" & dgIndependent.DataKeys(e.Item.ItemIndex) & "'"
                dg = e.Item.FindControl("dgDependent")
                If Not dg Is Nothing Then
                    AddHandler dg.ItemDataBound, AddressOf dgDependent_ItemDataBound
                    dg.DataSource = dv
                    dg.DataBind()

                    If e.Item.ItemIndex = 0 Then
                        dg.Style.Add("display", "block")
                    Else
                        dg.Style.Add("display", "none")
                    End If
                End If
            End If
        End If
    End Sub
    Public Function Show(ByVal e As Object)
        If e Is System.DBNull.Value Then
            Return "##"
        Else
            Return e
        End If

    End Function

    Function getDataXML() As String
        Dim ds As New LinkRatePlanData
        With New LinkRatePlanFacade
            ds = .getList(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With
        Util.Utility.GetXml(ds.TABLE_LINKRATEPLAN, "UpdateLinkRate", ds)

    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then

            Dim ds As LinkRatePlanData, row As DataRow
            ds = New LinkRatePlanData
            Dim SwD As Boolean = False
            Dim SwI As Boolean = False
            Dim notaDependent As String = "Se cambió el porcentaje de habitaciones de venta con los siguientes datos: "
            Dim sData As String = ""
            Dim sDataPrev As String = ""
            Dim imaxInventario As Integer
            Dim hr As Boolean = False

            Integer.TryParse(inputMaxInventario.Value, imaxInventario)

            sDataPrev = getDataXML()
            For Each st As String In txtCambiaDep.Text.Split(",")
                If st.Trim <> "" Then
                    If String.IsNullOrEmpty(st.Split("/")(1)) Or IsNumeric(st.Split("/")(2)) AndAlso st.Split("/")(2).ToString.IndexOf(".") < 0 AndAlso st.Split("/")(2).ToString.IndexOf("-") < 0 Then
                        'si hay un registro agregado se eliminará por que esta cadena fue la última
                        For Each row In ds.Tables(LinkRatePlanData.TABLE_LINKRATEPLAN).Rows
                            If row(LinkRatePlanData.FIELD_TargetRatePlan) = st.Split("/")(0) AndAlso row(LinkRatePlanData.FIELD_SourceRatePlan) = st.Split("/")(1) Then
                                row.Delete()
                                Exit For
                            End If
                        Next
                        'ahora si, hay que agregarlo
                        row = ds.Tables(LinkRatePlanData.TABLE_LINKRATEPLAN).NewRow
                        row(LinkRatePlanData.FIELD_TargetRatePlan) = st.Split("/")(0)
                        row(LinkRatePlanData.FIELD_SourceRatePlan) = st.Split("/")(1)
                        row(LinkRatePlanData.FIELD_SoldOutPerc) = st.Split("/")(2)
                        row(LinkRatePlanData.FIELD_IdHotel) = Me.cInfoActual.Hotel
                        ds.Tables(LinkRatePlanData.TABLE_LINKRATEPLAN).Rows.Add(row)
                        notaDependent &= st.Split("/")(2) & "% del rateplan " & st.Split("/")(4) & " para el rateplan " & st.Split("/")(3) & ". "
                        SwD = True
                    End If
                End If
            Next

            Dim dsRp As RatePlanData
            dsRp = New RatePlanData
            Dim notaIndependent As String = "Se cambió la definicion de disponibilidad de habitaciones de venta con los siguientes datos: "
            For Each st As String In txtCambiaInd.Text.Split(",")
                If st.Trim <> "" Then
                    If String.IsNullOrEmpty(st.Split("/")(1)) Or IsNumeric(st.Split("/")(1)) AndAlso st.Split("/")(1).ToString.IndexOf(".") < 0 AndAlso st.Split("/")(1).ToString.IndexOf("-") < 0 Then
                        'si hay un registro agregado se eliminará por que esta cadena fue la última
                        For Each row In dsRp.Tables(RatePlanData.RATEPLAN_TABLE).Rows
                            If row(RatePlanData.FIELD_IDRATEPLAN) = st.Split("/")(0) Then
                                row.Delete()
                                Exit For
                            End If
                        Next
                        'ahora si, hay que agregarlo
                        row = dsRp.Tables(RatePlanData.RATEPLAN_TABLE).NewRow
                        row(RatePlanData.FIELD_IDRATEPLAN) = st.Split("/")(0)
                        row(RatePlanData.FIELD_QTYROOMS) = IIf(String.IsNullOrEmpty(st.Split("/")(1)), DBNull.Value, st.Split("/")(1))
                        row(RatePlanData.FIELD_IDHOTEL) = Me.cInfoActual.Hotel
                        dsRp.Tables(RatePlanData.RATEPLAN_TABLE).Rows.Add(row)
                        notaIndependent &= st.Split("/")(1) & " habitaciones para el rateplan " & st.Split("/")(2) & ". "
                        SwI = True
                        If Not row(RatePlanData.FIELD_QTYROOMS) Is DBNull.Value AndAlso row(RatePlanData.FIELD_QTYROOMS) > imaxInventario Then
                            hr = True
                        End If
                    End If
                End If
            Next

          
            With New LinkRatePlanFacade
                If .UpdateSoldOut(ds, dsRp) Then
                    sData = Util.Utility.GetXml(ds.TABLE_LINKRATEPLAN, "UpdateLinkRate", ds)
                    If SwI Then
                        Me.guardalog("/Pages/RatePlanInventory.aspx", PaginaBase.acciones.Modificar, notaIndependent, "", sDataPrev, sData)
                    End If
                    If SwD Then
                        Me.guardalog("/Pages/RatePlanInventory.aspx", PaginaBase.acciones.Modificar, notaDependent, "", sDataPrev, sData)
                    End If
                End If
            End With
            loaddatos()
        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.clstitle.Text = PortalCulture.GetString("00326")
        lblPerc.Text = PortalCulture.GetString("00452")
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.SoldOutInventory, Me.btnSave, "M")
        lblRateCode.Text = PortalCulture.GetString("00480")
        lblAvail.Text = PortalCulture.GetString("00481")
        btnCancel.Text = PortalCulture.GetString("00009")
        btnSave.Text = PortalCulture.GetString("00008")

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect(Request.RawUrl)
    End Sub

    Private Sub dgIndependent_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dgIndependent.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim ctl As RangeValidator = e.Item.FindControl("RValIventory")
            ctl.ErrorMessage = PortalCulture.GetString("00612")
            Dim imax As Integer

            Integer.TryParse(inputMaxInventario.Value, imax)
            ''ctl = e.Item.FindControl("rvInventario")
            ctl.MaximumValue = If(imax = 0, 1000, imax)
            ctl.MinimumValue = 0
            ctl.ErrorMessage = String.Format(PortalCulture.GetString("01451"), inputMaxInventario.Value)
        End If
    End Sub
End Class
