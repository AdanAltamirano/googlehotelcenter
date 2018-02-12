Imports Portal.General.Common.Data
Imports Portal.General.Facade

Partial Public Class PrepagoRatesPlan
    Inherits PaginaBase
    Protected dtTipoPrepago As New DataTable
    Private msError As String = ""

    Private Enum dgcolumns
        iddepositratesplan
        prepagoTipo
        PrepaymentPerc
        idAgenciasRate
        idrateplan
        CodigoTarifa
        Name
        chkTemptipo0
        chkTemptipo1
        chkTemptipo2
        chkTemptipo3        
    End Enum

    Private Sub ShowError()
        Me.lblError.Visible = (msError.Trim.Length > 0)
        Me.lblError.Text = msError
    End Sub

    Private Property prepagoRatesplan() As Integer
        Get
            Return ViewState("_prepagoRatesplan")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_prepagoRatesplan") = Value
        End Set
    End Property

    Function CreateListCheck() As DataTable
        Dim dt As New DataTable
        Dim columna As DataColumn = New DataColumn
        Dim keys(0) As DataColumn
        Dim row As DataRow

        columna.DataType = System.Type.GetType("System.Int32")
        columna.ColumnName = "idTipo"

        dt.Columns.Add(columna)
        dt.Columns.Add("Nombre")
        keys(0) = columna
        dt.PrimaryKey = keys

        row = dt.NewRow
        row("idtipo") = "0"
        row("Nombre") = "Una Noche"
        dt.Rows.Add(row)

        row = dt.NewRow
        row("idtipo") = "1"
        row("Nombre") = "Total Estancia"
        dt.Rows.Add(row)

        row = dt.NewRow
        row("idtipo") = "2"
        row("Nombre") = "50% Estancia"
        dt.Rows.Add(row)
        Return dt
    End Function

    Private Sub LoadRatesPlan(ByVal sFiltro As String)
        Dim ds As DataSet
        Dim dv As DataView
        Dim idAsoc As Integer = Me.GetIdAsociation

        dtTipoPrepago = CreateListCheck()
        ds = (New RatePlanFacade).GetRatesPlanPrepagoByHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)

        With dgratesplan
            dv = ds.Tables(0).DefaultView
            dv.RowFilter = sFiltro
            .DataKeyField = RatePlanData.FIELD_IDRATEPLAN
            .DataSource = dv '//ds
            .DataBind()
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count = 0 Then
                Me.btnSave.Enabled = False
            End If
        End With
    End Sub

    Private Sub loadCulture()
        Me.lblTitle.Text = PortalCulture.GetString("01227")
        'Me.btnNuevo.Text = PortalCulture.GetString("00159")
        Me.btnSave.Text = PortalCulture.GetString("M000060")
        Me.lblAyuda.Text = String.Format("{0}<br/>{1}", PortalCulture.GetString("01231"), PortalCulture.GetString("01261"))
    End Sub

    Function getDataXML(ByVal sId As String) As String
        Dim ds As New DataSet
        Dim dv As DataView
        With New LinkRatePlanFacade
            ds = (New RatePlanFacade).GetRatesPlanPrepagoByHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With
        Return Util.Utility.GetXml("table", "UpdatePrepagos", ds)
    End Function


    Private Function Save() As Boolean
        Dim ds As New clsCommonPrepago
        Dim dr As DataRow
        Dim chkTmp As CheckBox
        Dim tmpChk0 As CheckBox
        Dim tmpChk1 As CheckBox
        Dim tmpChk2 As CheckBox
        Dim tmpChk3 As CheckBox
        Dim txtPorc As System.Web.UI.HtmlControls.HtmlInputText
        Dim scoma As String
        Dim sId As String = ""
        Dim sData As String = ""
        Dim sDataPrev As String = ""
        Dim hotel As String
        Dim dporc As Double
        Dim id As Integer
        Dim tipo As Integer
        Dim hr As Boolean = False

        Hotel = Me.cInfoActual.HotelName

        For i As Integer = 0 To dgratesplan.Items.Count - 1
            'chkTmp = dgRatePlans.Items(i).FindControl("chkPrepago")
            id = CType(dgratesplan.Items(i).Cells(dgcolumns.iddepositratesplan).Text, Integer)
            tmpChk0 = dgratesplan.Items(i).FindControl("chkPrepago0")
            tmpChk1 = dgratesplan.Items(i).FindControl("chkPrepago1")
            tmpChk2 = dgratesplan.Items(i).FindControl("chkPrepago2")
            tmpChk3 = dgratesplan.Items(i).FindControl("chkPrepago3")
            txtPorc = dgratesplan.Items(i).FindControl("txtPorcentaje")

            If tmpChk0.Checked Then
                tipo = 0
            ElseIf tmpChk1.Checked Then
                tipo = 1
            ElseIf tmpChk2.Checked Then
                tipo = 2
            ElseIf tmpChk3.Checked Then
                tipo = 3
            Else
                tipo = -1
            End If
            hr = True
            sId &= scoma & id
            scoma = ""
            dr = ds.Tables(clsCommonPrepago.TABLE_PREPAGO).NewRow
            dr.Item(clsCommonPrepago.FIELD_IdPrepagoRatesPlan) = id
            dr.Item(clsCommonPrepago.FIELD_IdHotel) = Me.cInfoActual.Hotel
            dr.Item(clsCommonPrepago.FIELD_ratecode) = dgratesplan.Items(i).Cells(dgcolumns.idrateplan).Text
            dr.Item(clsCommonPrepago.FIELD_Source) = "POR"
            dr.Item(clsCommonPrepago.FIELD_PrepagoTipo) = tipo
            Double.TryParse(txtPorc.Value, dporc)
            If dporc >= 0 And dporc <= 100 And tipo = 3 Then _
            dr.Item(clsCommonPrepago.FIELD_PrepaymentPerc) = dporc

            If id > 0 And (tipo >= 0) Then
                ds.Tables(clsCommonPrepago.TABLE_PREPAGO).Rows.Add(dr)
                dr.AcceptChanges()
                dr.Item(clsCommonPrepago.FIELD_IdPrepagoRatesPlan) = id
            End If
            If id > 0 And (tipo = -1) Then
                ds.Tables(clsCommonPrepago.TABLE_PREPAGO).Rows.Add(dr)
                dr.AcceptChanges()
                dr.Delete()
            End If
            If id = 0 And tipo >= 0 Then
                ds.Tables(clsCommonPrepago.TABLE_PREPAGO).Rows.Add(dr)
            End If

        Next
        If hr Then
            sDataPrev = getDataXML(sId)
            hr = (New clsFacadePrepago).Update(ds)
            If hr Then
                dgratesplan.SelectedIndex = -1
                LoadRatesPlan(ctrlAutoComplete1.GetFilter)
                sData = Util.Utility.GetXml(ds.TABLE_PREPAGO, "UpdatePrepagoPlan", ds)
                Me.guardalog("/Pages/PrepagoRatesPlan.aspx", PaginaBase.acciones.Modificar, String.Format("Se Modifico prepago para los rateplan del hotel {0}", hotel), "", sDataPrev, sData)
            Else
                Me.msError = PortalCulture.GetString("00844")
            End If
        End If

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If (Not (Me.isUserChain AndAlso IdCorporativoUserChain > 0) And Not Me.IsSupervisor) Then
            MyBase.redirectTo(PaginaBase.pages.Home)
        End If
        If Not IsPostBack Then
            LoadRatesPlan("")
        End If

    End Sub

    Private Sub dgRatePlans_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgratesplan.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Name).Text = PortalCulture.GetString("00073")
            e.Item.Cells(dgcolumns.idrateplan).Text = PortalCulture.GetString("00001")
            e.Item.Cells(dgcolumns.chkTemptipo0).Text = PortalCulture.GetString("01228")
            e.Item.Cells(dgcolumns.chkTemptipo1).Text = PortalCulture.GetString("01229")
            e.Item.Cells(dgcolumns.chkTemptipo2).Text = PortalCulture.GetString("01230")
            e.Item.Cells(dgcolumns.chkTemptipo3).Text = PortalCulture.GetString("01262")
        End If
    End Sub

    Private Sub dgRatePlans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgratesplan.ItemDataBound
        Dim tmpChk0 As CheckBox
        Dim tmpChk1 As CheckBox
        Dim tmpChk2 As CheckBox
        Dim tmpChk3 As CheckBox
        Dim txtPorc As System.Web.UI.HtmlControls.HtmlInputText
        Dim dPorc As Double
        Dim tipo As Integer
        Dim idAgenciasRate As Integer

        If e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            tmpChk0 = e.Item.FindControl("chkPrepago0")
            tmpChk1 = e.Item.FindControl("chkPrepago1")
            tmpChk2 = e.Item.FindControl("chkPrepago2")
            tmpChk3 = e.Item.FindControl("chkPrepago3")
            txtPorc = e.Item.FindControl("txtPorcentaje")

            tipo = CType(e.Item.Cells(dgcolumns.prepagoTipo).Text, Integer)
            Select Case tipo
                Case 0
                    tmpChk0.Checked = True
                Case 1
                    tmpChk1.Checked = True
                Case 2
                    tmpChk2.Checked = True
                Case 3
                    tmpChk3.Checked = True
            End Select

            Double.TryParse(e.Item.Cells(dgcolumns.PrepaymentPerc).Text, dPorc)
            Integer.TryParse(e.Item.Cells(dgcolumns.idAgenciasRate).Text, idAgenciasRate)
            If dPorc >= 0 Then
                txtPorc.Value = dPorc
                txtPorc.Style.Add("display", "block")
            End If
            If idAgenciasRate = 0 Then
                'tmpChk3.Visible = False
                'txtPorc.Visible = False
            End If

            tmpChk0.Attributes.Add("onclick", String.Format("javascript:FireChek('{0}','{1}','{2}','{3}','{4}');", _
                                                            tmpChk0.ClientID, tmpChk1.ClientID, tmpChk2.ClientID, _
                                                            tmpChk3.ClientID, txtPorc.ClientID))
            tmpChk1.Attributes.Add("onclick", String.Format("javascript:FireChek('{0}','{1}','{2}','{3}','{4}');", _
                                                            tmpChk1.ClientID, tmpChk0.ClientID, tmpChk2.ClientID, _
                                                            tmpChk3.ClientID, txtPorc.ClientID))
            tmpChk2.Attributes.Add("onclick", String.Format("javascript:FireChek('{0}','{1}','{2}','{3}','{4}');", _
                                                            tmpChk2.ClientID, tmpChk0.ClientID, tmpChk1.ClientID, _
                                                            tmpChk3.ClientID, txtPorc.ClientID))
            tmpChk3.Attributes.Add("onclick", String.Format("javascript:FireChek('{0}','{1}','{2}','{3}','{4}', true);", _
                                                            tmpChk3.ClientID, tmpChk0.ClientID, tmpChk1.ClientID, _
                                                            tmpChk2.ClientID, txtPorc.ClientID))

        End If
    End Sub

    Private Sub dgRatePlans_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgratesplan.PageIndexChanged
        Me.dgratesplan.CurrentPageIndex = e.NewPageIndex
        Me.dgratesplan.SelectedIndex = -1
        LoadRatesPlan(ctrlAutoComplete1.GetFilter)
    End Sub

    Private Sub PrepagoRatesPlan_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        loadCulture()
        ShowError()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Save()
    End Sub

    Private Sub ctrlAutoComplete1_OnSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        dgratesplan.CurrentPageIndex = 0
        LoadRatesPlan(descripcion)
    End Sub
End Class