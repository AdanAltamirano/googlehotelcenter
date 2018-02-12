Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class HotelBeds_ModifyDestination
    Inherits PaginaBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack And Request.QueryString("IdCiudad") IsNot Nothing Then
            Dim idCiudad As Integer

            If Integer.TryParse(Request.QueryString("IdCiudad"), idCiudad) Then
                Dim tab_City As New HotelBedsDS.CityDataTable
                If New HotelBedsRules().Select_City_ByID(idCiudad, tab_City) And tab_City.Rows.Count > 0 Then
                    Dim res As String = "IDCiudad" & tab_City(0).IdCiudad.ToString() & "<br />"
                    res &= "Nombre: " & tab_City(0).Nombre & " <br/> "
                    res &= "Codigo: " & tab_City(0).codigo & " <br/> "
                    res &= "Estado: " & tab_City(0).Estado & " <br/> "
                    res &= "Pais: " & tab_City(0).Pais

                    If tab_City(0).IsSkippedNull() Then
                        res &= "<br /> [Ciudad Omitida]"
                        btnAddToSkipped.Text = "Restaurar ciudad"
                    End If

                    litCity.Text = res

                    lblTopCiudad.Text = "Modificando ciudad" & tab_City(0).Nombre & " - Listado de destinos hotelbeds relacionados."
                End If
                GridRel_Search(idCiudad)
                GridRes_Suggested(idCiudad)
            End If
        End If
    End Sub


    Private Sub GridRel_Search(ByVal idCiudad As Integer)
        Dim tab_DestinationList As New HotelBedsDS.DestinationsDataTable
        If New HotelBedsRules().Search_Destinations_ByIdCiudad(idCiudad, tab_DestinationList) Then
            gridDestRel.DataSource = tab_DestinationList
            gridDestRel.DataBind()
            If tab_DestinationList.Rows.Count = 0 Then
                lblMsgRel.Text = "No se encontraron destinos relacionados"
            Else
                lblMsgRel.Text = ""
            End If
        End If
    End Sub

    Private Sub GridRes_Search(ByVal search As String)
        Dim tab_DestinationList As New HotelBedsDS.DestinationsDataTable
        If New HotelBedsRules().Search_Destinations_ByKey(search, tab_DestinationList) Then
            gridDestRes.DataSource = tab_DestinationList
            gridDestRes.DataBind()

            lblMSG_Result.Text = tab_DestinationList.Rows.Count.ToString() & "Destination HotelBeds encontradas"
        End If
    End Sub

    Private Sub GridRes_Suggested(ByVal idCiudad As Integer)
        Dim tab_DestinationList As New HotelBedsDS.DestinationsDataTable
        If New HotelBedsRules().Search_Destinations_Suggested(idCiudad, tab_DestinationList) Then
            gridDestRes.DataSource = tab_DestinationList
            gridDestRes.DataBind()

            lblMSG_Result.Text = "Destinos HotelBeds sugeridos"
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddBottom.Click, btnAddTop.Click
        Dim idciudad As Integer
        Dim added As Boolean = False


        If Integer.TryParse(Request.QueryString("IdCiudad"), idciudad) Then
            For Each gvrow As GridViewRow In gridDestRes.Rows

                If TypeOf gvrow.Cells(8).Controls(1) Is CheckBox Then
                    If CType(gvrow.Cells(8).Controls(1), CheckBox).Checked Then
                        If New HotelBedsRules().Insert_DestinationsFix(gvrow.Cells(0).Text, Integer.Parse(gvrow.Cells(3).Text), idciudad) Then
                            added = True
                        End If
                    End If
                End If
            Next
        End If
        If added Then
            If txtSearch.Text = String.Empty Then
                GridRes_Suggested(idciudad)
            Else
                GridRes_Search(txtSearch.Text.Trim())
            End If
            GridRel_Search(idciudad)
        End If

    End Sub

    Protected Sub btnDel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDel.Click
        Dim idCiudad As Integer
        Dim deleted As Boolean
        If Integer.TryParse(Request.QueryString("idCiudad"), idCiudad) Then
            For Each gvRow As GridViewRow In gridDestRel.Rows

                If TypeOf gvRow.Cells(8).Controls(1) Is CheckBox Then
                    If CType(gvRow.Cells(8).Controls(1), CheckBox).Checked Then
                        If New HotelBedsRules().Update_RemoveIdCiudad(Integer.Parse(gvRow.Cells(0).Text)) Then
                            deleted = True
                        End If
                    End If
                End If
            Next
        End If

        If deleted Then
            If txtSearch.Text = String.Empty Then
                GridRes_Suggested(idCiudad)
            Else
                GridRes_Search(txtSearch.Text.Trim())
            End If
            GridRel_Search(idCiudad)
        End If
    End Sub


    Protected Sub gridDestRes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridDestRes.RowCommand

        If e.CommandName = "Add" And Request.QueryString("IdCiudad") IsNot Nothing Then
            Dim idciudad As Integer
            If Integer.TryParse(Request.QueryString("IdCiudad"), idciudad) Then
                Dim gvrow As GridViewRow = gridDestRes.Rows(Convert.ToInt32(e.CommandArgument))
                If New HotelBedsRules().Insert_DestinationsFix(gvrow.Cells(0).Text, Integer.Parse(gvrow.Cells(3).Text), idciudad) Then
                    If txtSearch.Text = String.Empty Then
                        GridRes_Suggested(idciudad)
                    Else
                        GridRes_Search(txtSearch.Text.Trim())
                    End If
                    GridRel_Search(idciudad)
                End If
            End If
        End If

    End Sub

    Protected Sub gridDestRes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridDestRes.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow And Request.QueryString("IdCiudad") IsNot Nothing Then
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            If rowView("IdCiudad").ToString() <> "" Then
                e.Row.Controls(7).Visible = False
                e.Row.Controls(8).Visible = False
            End If
        End If
    End Sub

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As Control = gridDestRes.HeaderRow.FindControl("chkSelectAll")
        Dim idCiudad As Integer
        If Integer.TryParse(Request.QueryString("IdCiudad"), idCiudad) Then

            If TypeOf chk Is CheckBox Then
                For Each gvrow As GridViewRow In gridDestRes.Rows
                    Dim ctrl As Control = gvrow.Cells(8).Controls(1)

                    If TypeOf ctrl Is CheckBox Then
                        CType(ctrl, CheckBox).Checked = CType(chk, CheckBox).Checked
                    End If
                Next
            End If

        End If
    End Sub

    Protected Sub chkSelectA_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As Control = gridDestRel.HeaderRow.FindControl("chkSelectA")
        If TypeOf chk Is CheckBox Then
            For Each gvrow As GridViewRow In gridDestRel.Rows
                Dim ctrl As Control = gvrow.Cells(8).Controls(1)
                If TypeOf ctrl Is CheckBox Then
                    CType(ctrl, CheckBox).Checked = CType(chk, CheckBox).Checked
                End If
            Next
        End If
    End Sub

    Protected Sub btnAddSkipped_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddToSkipped.Click
        Dim idCiudad As Integer

        If Integer.TryParse(Request.QueryString("IdCiudad"), idCiudad) Then
            If btnAddToSkipped.Text = "Restaurar Ciudad" Then

                If New HotelBedsRules().Delete_DestinationSkipped(idCiudad) Then
                    ClientScript.RegisterStartupScript([GetType], "owo", "alert('La ciudad actual se ha quitado de la lista de ciudades omitidas');", True)
                    litCity.Text = litCity.Text.Substring(0, litCity.Text.Length - 21)
                    btnAddToSkipped.Text = "Omitir Ciudad"
                Else
                    ClientScript.RegisterStartupScript([GetType], "owo", "alert('La ciudad no se pudo quitar de la lista de ciudades omitidas');", True)
                End If
            Else

                If New HotelBedsRules().Insert_DestinationSkipped(idCiudad) Then
                    ClientScript.RegisterStartupScript([GetType], "owo", "alert('La ciudad actual se ha enviado a la lista de ciudades omitidas');", True)
                    litCity.Text += "<br/>[Ciudad omitida]"
                    btnAddToSkipped.Text = "Restaurar ciudad"
                Else
                    ClientScript.RegisterStartupScript([GetType], "owo", "alert('La ciudad no se pudo enviar a la lista de ciudades omitidas');", True)
                End If


            End If
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        GridRes_Search(txtSearch.Text.Trim())
    End Sub

    Protected Sub gridDestRel_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridDestRel.RowCommand

        If e.CommandName = "Add" And Request.QueryString("IdCiudad") IsNot Nothing Then
            Dim idciudad As Integer
            If Integer.TryParse(Request.QueryString("IdCiudad"), idciudad) Then
                Dim gvrow As GridViewRow = gridDestRes.Rows(Convert.ToInt32(e.CommandArgument))
                If New HotelBedsRules().Insert_DestinationsFix(gvrow.Cells(0).Text, Integer.Parse(gvrow.Cells(3).Text), idciudad) Then
                    If txtSearch.Text = String.Empty Then
                        GridRes_Suggested(idciudad)
                    Else
                        GridRes_Search(txtSearch.Text.Trim())
                    End If
                    GridRel_Search(idciudad)
                End If
            End If
        End If

    End Sub
End Class