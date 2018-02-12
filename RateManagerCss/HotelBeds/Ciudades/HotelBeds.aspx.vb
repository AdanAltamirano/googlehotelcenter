Partial Public Class HotelBeds
    Inherits PaginaBase

    Enum RelcColumns
        idRelacion
        idCiudad
        Nombre
        Codigo
        ZoneCode
        Edicion
    End Enum
    Enum UVColumns
        idCiudad
        Nombre
        codigo
        NumRel
        Seleccionar
    End Enum

    Enum HBColumns
        DestinationCode
        CountryCode
        DestinationName
        ZoneCode
        ZoneName
        idCiudad
        codigo
        Seleccionar
    End Enum

    Private Property Adding() As Boolean
        Get
            Return ViewState("_Adding")
        End Get
        Set(ByVal value As Boolean)
            ViewState("_Adding") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Adding = False
        End If

    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Load_CitiesUV()
        gvRelacion.EditIndex = -1
    End Sub

    Protected Sub btnSearchDestination_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchDestination.Click
        Load_Destinations()
        gvRelacion.EditIndex = -1
    End Sub

    Protected Sub Load_RelatedCities()
        Dim tab_RelatedCitiesList As New HotelBedsDS.RelatedListDataTable()
        If New HotelBedsRules().Search_DestinationsFixByKey(txtSearchRelated.Text.Trim(), tab_RelatedCitiesList) Then
            gvRelacion.DataSource = tab_RelatedCitiesList
            gvRelacion.DataBind()
        End If
    End Sub

    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        gvRelacion_RowCancelingEdit()
        Adding = True
        If Not txtSearch.Text = String.Empty Or gridList.Rows.Count > 0 Then
            Load_CitiesUV()
        End If
        If Not txtSearchDestination.Text = String.Empty Or gvDestinosHB.Rows.Count > 0 Then
            Load_Destinations()
        End If
        btnNuevo.Visible = False
    End Sub

    Protected Sub btnSearchRelated_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchRelated.Click
        Load_RelatedCities()
        gvRelacion.EditIndex = -1
    End Sub

    Private Sub Load_CitiesUV()
        Dim tab_CitiesList As New HotelBedsDS.CitiesListDataTable()
        If New HotelBedsRules().Search_Cities_ByKey(txtSearch.Text.Trim(), tab_CitiesList) Then
            gridList.DataSource = tab_CitiesList
            gridList.DataBind()
        End If
    End Sub

    Private Sub Load_Destinations()
        Dim tab_DestinationsDataTable As New HotelBedsDS.DestinationsDataTable()
        If New HotelBedsRules().Search_Destinations_ByKey(txtSearchDestination.Text, tab_DestinationsDataTable) Then
            gvDestinosHB.DataSource = tab_DestinationsDataTable
            gvDestinosHB.DataBind()
        End If
    End Sub

    Protected Sub gridList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridList.RowDataBound
        'Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            If CType(rowView("NumRel").ToString(), Integer) > 0 Then
                For Each cell As TableCell In e.Row.Cells
                    cell.BackColor = Drawing.Color.LightBlue
                Next
            End If
            Dim btnSeleccionar As LinkButton
            btnSeleccionar = e.Row.FindControl("btnSeleccionar")
            If Adding Then
                btnSeleccionar.Visible = True
            Else
                btnSeleccionar.Visible = False
            End If
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(UVColumns.idCiudad).Text = PortalCulture.GetString("01604")
            e.Row.Cells(UVColumns.Nombre).Text = PortalCulture.GetString("00073")
            e.Row.Cells(UVColumns.codigo).Text = PortalCulture.GetString("M000432")
            e.Row.Cells(UVColumns.NumRel).Text = PortalCulture.GetString("01605")
        End If
    End Sub

    Private Sub gridList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridList.SelectedIndexChanged
        Dim index As Integer
        index = gridList.SelectedIndex
        Load_CitiesUV()
        gridList.SelectedIndex = index
        For Each cell As TableCell In gridList.SelectedRow.Cells
            cell.BackColor = Drawing.Color.LightGreen
        Next
    End Sub

    Private Sub gvDestinosHB_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDestinosHB.SelectedIndexChanged
        Dim index As Integer
        index = gvDestinosHB.SelectedIndex
        Load_Destinations()
        gvDestinosHB.SelectedIndex = index
        For Each cell As TableCell In gvDestinosHB.SelectedRow.Cells
            cell.BackColor = Drawing.Color.LightGreen
        Next
    End Sub

    Protected Sub gvDestinosHB_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDestinosHB.RowDataBound
        'Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            If rowView("idCiudad").ToString() <> "" Then
                For Each cell As TableCell In e.Row.Cells
                    cell.BackColor = Drawing.Color.LightBlue
                Next
            End If
            Dim btnSeleccionar As LinkButton
            btnSeleccionar = e.Row.FindControl("btnSeleccionar")
            If Adding Then
                btnSeleccionar.Visible = True
            Else
                btnSeleccionar.Visible = False
            End If
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(HBColumns.DestinationCode).Text = PortalCulture.GetString("01606")
            e.Row.Cells(HBColumns.CountryCode).Text = PortalCulture.GetString("01608")
            e.Row.Cells(HBColumns.DestinationName).Text = PortalCulture.GetString("01609")
            e.Row.Cells(HBColumns.ZoneCode).Text = PortalCulture.GetString("01607")
            e.Row.Cells(HBColumns.idCiudad).Text = PortalCulture.GetString("01604")
            e.Row.Cells(HBColumns.codigo).Text = PortalCulture.GetString("M000432")
        End If
    End Sub

    Protected Sub gvRelacion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRelacion.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(RelcColumns.ZoneCode).Text = PortalCulture.GetString("01607")
            e.Row.Cells(RelcColumns.idRelacion).Text = PortalCulture.GetString("01611")
            e.Row.Cells(RelcColumns.Edicion).Text = PortalCulture.GetString("00763")
            e.Row.Cells(RelcColumns.Nombre).Text = PortalCulture.GetString("00073")
            e.Row.Cells(RelcColumns.idCiudad).Text = PortalCulture.GetString("01604")
            e.Row.Cells(RelcColumns.Codigo).Text = PortalCulture.GetString("M000432")
        End If
    End Sub

    Protected Sub gvRelacion_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        gvRelacion.EditIndex = e.NewEditIndex
        Call Load_RelatedCities()
    End Sub

    Protected Sub gvRelacion_RowCancelingEdit()
        'Reset the edit index.
        gvRelacion.EditIndex = -1
        'Bind data to the GridView control.
        Load_RelatedCities()
    End Sub

    Protected Sub btnAddRelation_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddRelation.Click
        If gridList.SelectedIndex >= 0 AndAlso gvDestinosHB.SelectedIndex >= 0 Then

            If Not Add_Relation() Then
                lblAddError.Visible = True
                lblAddError.Text = PortalCulture.GetString("01610")
            End If
        Else
            lblAddError.Visible = True
            lblAddError.Text = PortalCulture.GetString("01615")
        End If
    End Sub

    Private Function Add_Relation() As Boolean
        Dim idCiudad As Integer
        Dim destinationCode As String
        Dim zoneCode As Integer



        If Not Integer.TryParse(gridList.SelectedRow.Cells.Item(UVColumns.idCiudad).Text, idCiudad) Then
            Return False
        End If

        destinationCode = gvDestinosHB.Rows(gvDestinosHB.SelectedIndex).Cells.Item(HBColumns.DestinationCode).Text

        If Not Integer.TryParse(gvDestinosHB.SelectedRow.Cells.Item(HBColumns.ZoneCode).Text, zoneCode) Then
            Return False
        End If


        If New HotelBedsRules().Insert_DestinationsFix(destinationCode, zoneCode, idCiudad) Then
            txtSearchRelated.Text = gridList.SelectedRow.Cells.Item(UVColumns.Nombre).Text
            Adding = False
            Load_RelatedCities()
            gridList.SelectedIndex = -1
            gvDestinosHB.SelectedIndex = -1
            Load_CitiesUV()
            Load_Destinations()
            btnNuevo.Visible = True

            Return True
        Else

            Return False

        End If
    End Function

    Protected Sub btnCancelRelation_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelRelation.Click
        Adding = False
        If Not txtSearch.Text = String.Empty Or gridList.Rows.Count > 0 Then
            Load_CitiesUV()
        End If
        If Not txtSearchDestination.Text = String.Empty Or gridList.Rows.Count > 0 Then
            Load_Destinations()
        End If
        gvDestinosHB.SelectedIndex = -1
        gridList.SelectedIndex = -1
        btnNuevo.Visible = True
    End Sub

    Protected Sub gvRelacion_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim txtCodigo As TextBox = gvRelacion.Rows(e.RowIndex).FindControl("txtCodigo")
        Dim txtZoneCode As TextBox = gvRelacion.Rows(e.RowIndex).FindControl("txtZoneCode")
        Dim zoneCode As Integer
        Dim idCiudad As Integer
        Integer.TryParse(gvRelacion.Rows(e.RowIndex).Cells(RelcColumns.idCiudad).Text, idCiudad)
        Dim idRelacion As Integer


        If Integer.TryParse(gvRelacion.Rows(e.RowIndex).Cells(RelcColumns.idRelacion).Text, idRelacion) AndAlso Integer.TryParse(txtZoneCode.Text, zoneCode) Then
            If New HotelBedsRules().Insert_DestinationsFix(txtCodigo.Text, CType(txtZoneCode.Text, Integer), idCiudad, idRelacion) Then
                txtSearchRelated.Text = gvRelacion.Rows(e.RowIndex).Cells(RelcColumns.Nombre).Text
                gvRelacion.EditIndex = -1
                Load_RelatedCities()
            End If
        Else
            lblAddError.Visible = True
            lblAddError.Text = PortalCulture.GetString("01610")
        End If
    End Sub

    Protected Sub gvRelacion_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim idRelacion As Integer

        If Integer.TryParse(gvRelacion.Rows(e.RowIndex).Cells(RelcColumns.idRelacion).Text, idRelacion) Then
            If New HotelBedsRules().Update_RemoveIdCiudad(idRelacion) Then
                gvRelacion.EditIndex = -1
                If Not txtSearch.Text = String.Empty Then
                    Load_CitiesUV()
                End If
                If Not txtSearchDestination.Text = String.Empty Then
                    Load_Destinations()
                End If
                If Not txtSearchRelated.Text = String.Empty Then
                    Load_RelatedCities()
                End If
            End If
        End If
    End Sub
    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        lblSearch.Text = PortalCulture.GetString("01601")
        lblSearchDestination.Text = PortalCulture.GetString("01602")
        lblSearchRelated.Text = PortalCulture.GetString("01603")
        
        lblTitleDestinations.Text = PortalCulture.GetString("01599")
        lblTitleRelated.Text = PortalCulture.GetString("01600")
        lblTitleUVCities.Text = PortalCulture.GetString("01598")

        btnAddRelation.Text = PortalCulture.GetString("M0BT0000188")
        btnBuscar.Text = PortalCulture.GetString("M000637")
        btnCancelRelation.Text = PortalCulture.GetString("00413")
        btnSearchDestination.Text = PortalCulture.GetString("M000637")
        btnSearchRelated.Text = PortalCulture.GetString("M000637")
        btnNuevo.Text = PortalCulture.GetString("00102")

        If Adding Then
            divAddRelation.Visible = True
        Else
            divAddRelation.Visible = False
        End If
    End Sub

    Private Sub gridList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gridList.PageIndexChanging
        gridList.PageIndex = e.NewPageIndex
        Load_CitiesUV()
    End Sub

    Private Sub gvRelacion_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRelacion.PageIndexChanging
        gvRelacion.PageIndex = e.NewPageIndex
        Load_RelatedCities()
    End Sub

    Private Sub gvDestinosHB_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDestinosHB.PageIndexChanging
        gvDestinosHB.PageIndex = e.NewPageIndex
        Load_Destinations()
    End Sub

    Private Sub txtSearch_KeyPress(ByVal KeyAscii As Integer)

        If txtSearch.Text Is Nothing Then Return
        If txtSearch.Text.Length = 6 And KeyAscii = 13 Then
            Load_CitiesUV()
        End If

    End Sub

End Class