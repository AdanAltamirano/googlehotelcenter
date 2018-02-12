Public Partial Class MonedasHotel
    Inherits PaginaBase

    Enum Columns
        idMoneda
        idHotel
        Codigo
        Nombre
        tipoCambio
        tmpTipoCambio
        link
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMessage.Text = ""
        If Not Me.IsPostBack Then
            CargaMonedas()
        End If
    End Sub

    Private Sub CargaMonedas()
        Dim dsMonedas As Portal.General.Common.Data.MonedaDatosHotel
        dsMonedas = (New Portal.General.Facade.MonedaHotel).GetMonedaList(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture())
        dgMonedas.DataSource = dsMonedas
        dgMonedas.DataBind()
        If Me.dsEmpty(dsMonedas) Then
            Me.btnSave.Enabled = False
        End If
    End Sub

    Private Sub MonedasHotel_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        lblTitulo.Text = PortalCulture.GetString("M0BT0000030", False)
        btnSave.Text = PortalCulture.GetString("M000060")
    End Sub

    Private Sub dgMonedas_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgMonedas.ItemCommand
        Dim dsdatos As New Portal.General.Common.Data.MonedaDatosHotel
        Dim dsmonedas As New Portal.General.Facade.MonedaHotel
        Dim nuevoTC As TextBox = e.Item.FindControl("txtTipoCambio")
        Dim Moneda As String
        Dim hr As Boolean

        nuevoTC = e.Item.FindControl("txtTipoCambio")
        Moneda = e.CommandArgument.ToString

        If Not nuevoTC Is Nothing Then

            Try                
                hr = dsmonedas.UpdateMoneda(e.Item.Cells(Columns.idMoneda).Text, cInfoActual.Hotel, _
                                               nuevoTC.Text, dsdatos)
                lblMessage.Text = String.Format(PortalCulture.GetString("01397", False), Moneda, nuevoTC.Text)
                'lblMessage.Attributes.Add("display", "block")
                Me.lblMessage.Style.Add("display", "")
                'lblMessage.Visible = True
            Catch ex As Exception
                lblMessage.Text = String.Format(PortalCulture.GetString("01398", False), Moneda)
                nuevoTC.Text = e.Item.Cells(Columns.tipoCambio).Text
            End Try
        End If
    End Sub

    Private Sub dgMonedas_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgMonedas.ItemDataBound
        Dim nuevoTC As TextBox

        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(Columns.Codigo).Text = PortalCulture.GetString("00001", False)
            e.Item.Cells(Columns.Nombre).Text = PortalCulture.GetString("00073", False)
        End If
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            nuevoTC = e.Item.FindControl("txtTipoCambio")
            nuevoTC.Attributes.Add("onclick", "FireClearMsg();")
        End If

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Dim ds As New Portal.General.Common.Data.MonedaDatosHotel
        Dim row As DataRow
        Dim tmpText As TextBox
        Dim tc As Double
        Dim id As Integer
        Dim hr As Boolean

        If dgMonedas.Items.Count = 0 Then Return

        For i As Integer = 0 To dgMonedas.Items.Count - 1
            Int32.TryParse(dgMonedas.Items(i).Cells(Columns.idMoneda).Text, id)
            tmpText = dgMonedas.Items(i).FindControl("txtTipoCambio")
            If Not tmpText Is Nothing Then
                Double.TryParse(tmpText.Text, tc)
                row = ds.Tables(Portal.General.Common.Data.MonedaDatosHotel.MONEDA_TABLE).NewRow
                row(Portal.General.Common.Data.MonedaDatosHotel.FIELD_idHotel) = cInfoActual.Hotel
                row(Portal.General.Common.Data.MonedaDatosHotel.FIELD_TipoCambio) = tc
                ds.Tables(Portal.General.Common.Data.MonedaDatosHotel.MONEDA_TABLE).Rows.Add(row)
                row.AcceptChanges()
                row(Portal.General.Common.Data.MonedaDatosHotel.FIELD_idMoneda) = id

            End If
        Next
        hr = (New Portal.General.Facade.MonedaHotel).UpdateMoneda(ds)
        If hr Then
            'lblMessage.Text = String.Format(PortalCulture.GetString("01397", False), Moneda, nuevoTC.Text)
            'lblMessage.Attributes.Add("display", "block")

        End If
    End Sub

End Class