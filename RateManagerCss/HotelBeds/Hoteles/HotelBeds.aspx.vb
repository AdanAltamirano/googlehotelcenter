Public Partial Class HotelBeds1
    Inherits PaginaBase

    Public Enum columns
        HotelCode
        HBName
        HBCity
        HBCountry
        Relacionarlos
        idHotel
        UVName
        UVCity
        UVCountry
        Modificar
        RelacionHB
        Edicion
        idHotelFix
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            'Call Load_UVHotels()
            'Call Load_HBHotels

        End If

    End Sub

    Private Sub Load_HBHotels()
        Dim tab_HBList As New HotelBedsDS.HotelsDataTable()
        Dim strError As String = String.Empty
        If New HotelBedsRules().Search_HBHotelsByKey(txtSearchBeds.Text, tab_HBList, strError) Then
            gvBeds.DataSource = tab_HBList
            gvBeds.DataBind()
        Else
            lblError.Text = strError
            lblError.Visible = True
        End If
    End Sub


    Private Sub Load_UVHotels()
        Dim tab_CitiesList As New HotelBedsDS.HotelsDataTable()
        If New HotelBedsRules().Search_UVHotelsByKey(txtSearch.Text, tab_CitiesList) Then
            gridListHotels.DataSource = tab_CitiesList
            gridListHotels.DataBind()
        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Dim tab_CitiesList As New HotelBedsDS.HotelsDataTable()

        If New HotelBedsRules().Search_UVHotelsByKey(txtSearch.Text.Trim(), tab_CitiesList) Then
            gridListHotels.DataSource = tab_CitiesList
            gridListHotels.DataBind()
        End If
    End Sub
    Protected Sub btnBuscarBeds_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscarBeds.Click
        Dim strError As String = String.Empty
        Dim tab_HBList As New HotelBedsDS.HotelsDataTable()

        If New HotelBedsRules().Search_HBHotelsByKey(txtSearchBeds.Text, tab_HBList, strError) Then
            gvBeds.DataSource = tab_HBList
            gvBeds.DataBind()
        Else
            lblError.Text = strError
            lblError.Visible = True
        End If
    End Sub

    Protected Sub gridListHotels_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridListHotels.RowCommand
        If e.CommandName = "Rel" Then
            Dim idhotel As Integer
            If Integer.TryParse(gridListHotels.Rows(Convert.ToInt32(e.CommandArgument)).Cells(5).Text, idhotel) Then
                If New HotelBedsRules().Insert_HotelFix(idhotel, gridListHotels.Rows(Convert.ToInt32(e.CommandArgument)).Cells(0).Text) Then
                    If txtSearch.Text = String.Empty Then
                        Dim tab_CitiesList As New HotelBedsDS.HotelsDataTable()
                        If New HotelBedsRules().Select_UVHotelsSuggested(tab_CitiesList) Then
                            gridListHotels.DataSource = tab_CitiesList
                            gridListHotels.DataBind()
                        End If
                    Else
                        Dim tab_CitiesList As New HotelBedsDS.HotelsDataTable()
                        If New HotelBedsRules().Search_UVHotelsByKey(txtSearch.Text.Trim(), tab_CitiesList) Then
                            gridListHotels.DataSource = tab_CitiesList
                            gridListHotels.DataBind()
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub gridListHotels_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridListHotels.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            If rowView("HotelCode").ToString() <> "" Then
                For Each cell As TableCell In e.Row.Cells
                    cell.BackColor = Drawing.Color.LightBlue
                Next
            End If
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(columns.UVName).Text = PortalCulture.GetString("00073")
            e.Row.Cells(columns.UVCity).Text = PortalCulture.GetString("00254")
            e.Row.Cells(columns.UVCountry).Text = PortalCulture.GetString("00251")
            e.Row.Cells(columns.RelacionHB).Text = PortalCulture.GetString("01613")
            e.Row.Cells(columns.idHotel).Text = PortalCulture.GetString("01594")
            e.Row.Cells(columns.Edicion).Text = PortalCulture.GetString("00065")
        End If

        'If ((e.Row.RowState And DataControlRowState.Edit) > 0) Then
        '    'Dim txtRelacion As TextBox = CType(e.Row.FindControl("txtRelacion"), TextBox)
        '    'txtRelacion.Text = "Código1"
        'End If

    End Sub

    Protected Sub gridListHotels_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        gridListHotels.EditIndex = e.NewEditIndex
        Call Load_UVHotels()
    End Sub

    Protected Sub gridListHotels_RowCancelingEdit()
        'Reset the edit index.
        gridListHotels.EditIndex = -1
        'Bind data to the GridView control.
        Load_UVHotels()
    End Sub

    Protected Sub gridListHotels_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim txtRelacion As TextBox = gridListHotels.Rows(e.RowIndex).FindControl("txtRelacion")
        Dim idhotel As Integer
        If Integer.TryParse(gridListHotels.Rows(e.RowIndex).Cells(columns.idHotel).Text, idhotel) Then
            If New HotelBedsRules().Insert_HotelFix(idhotel, txtRelacion.Text) Then
                txtSearch.Text = gridListHotels.Rows(e.RowIndex).Cells(columns.UVName).Text
                gridListHotels.EditIndex = -1
                Load_UVHotels()
            End If
        End If
    End Sub

    Protected Sub gridListHotels_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim idhotel As Integer
        If Integer.TryParse(gridListHotels.Rows(e.RowIndex).Cells(columns.idHotel).Text, idhotel) Then
            If New HotelBedsRules().Delete_HotelFix(idhotel) Then
                txtSearch.Text = ""
                gridListHotels.EditIndex = -1
                Load_UVHotels()
            End If
        End If
    End Sub

    Private Sub gridListHotels_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gridListHotels.PageIndexChanging
        Try
            gridListHotels.PageIndex = e.NewPageIndex
            Dim tab_CitiesList As New HotelBedsDS.HotelsDataTable()
            If New HotelBedsRules().Search_UVHotelsByKey(txtSearch.Text.Trim(), tab_CitiesList) Then
                gridListHotels.DataSource = tab_CitiesList
                gridListHotels.DataBind()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub gvBeds_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvBeds.PageIndexChanging
        Try
            gvBeds.PageIndex = e.NewPageIndex
            Dim tab_HBList As New HotelBedsDS.HotelsDataTable()
            Dim strError As String = String.Empty
            If New HotelBedsRules().Search_HBHotelsByKey(txtSearchBeds.Text, tab_HBList, strError) Then
                gvBeds.DataSource = tab_HBList
                gvBeds.DataBind()
            Else
                lblError.Text = strError
                lblError.Visible = True
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub gvBeds_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBeds.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            If rowView("idHotel").ToString() <> "" Then
                For Each cell As TableCell In e.Row.Cells
                    cell.BackColor = Drawing.Color.LightBlue
                Next
            End If
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(columns.HotelCode).Text = PortalCulture.GetString("01612")
            e.Row.Cells(columns.HBName).Text = PortalCulture.GetString("00073")
            e.Row.Cells(columns.HBCity).Text = PortalCulture.GetString("00254")
            e.Row.Cells(columns.HBCountry).Text = PortalCulture.GetString("00251")
            e.Row.Cells(columns.idHotel).Text = PortalCulture.GetString("01594")
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblAgregados.Text = PortalCulture.GetString("01614")
        lblSeachBeds.Text = PortalCulture.GetString("M0BT0000220")
        lblSearch.Text = PortalCulture.GetString("M0BT0000220")
        lblTitleADO.Text = PortalCulture.GetString("01595")
        lblTitleHB.Text = PortalCulture.GetString("01597")
        btnBuscar.Text = PortalCulture.GetString("M000637")
        btnBuscarBeds.Text = PortalCulture.GetString("M000637")

    End Sub

End Class