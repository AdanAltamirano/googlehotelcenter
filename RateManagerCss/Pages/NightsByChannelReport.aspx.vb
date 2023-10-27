Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Public Class NightsByChannelReport
    Inherits PaginaBase

    Dim currentReport As DataSet
    Dim currentReportName As String = ""
    Dim yearSelected As String = Date.Today.Year.ToString()
    Dim monthSelected As String = "01"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim currentYear As Integer = Date.Now.Year
        Dim yearsLimit As Integer = currentYear - 5

        ddlYear.Items.Clear()

        ddlYear.Items.Add(New ListItem(currentYear, currentYear, True))

        For year As Integer = currentYear - 1 To yearsLimit Step year - 1
            ddlYear.Items.Add(New ListItem(year, year))
        Next

        btnExcel.Visible = If(Session("dvReportNBC") Is Nothing Or currentReport Is Nothing, False, True)
        btnExcel.DataBind()

    End Sub

    Private Sub ctrlAutoCompleteHotels_onSendFilter(ByVal id As String, ByVal hotelName As String) Handles ctrlAutoCompleteHotels.OnSendFilter
        If Not String.IsNullOrWhiteSpace(hotelName) Then

            ' a partir de donde termina la cadena de like, mas el tamaño de la cadena menos el residuo constante
            Dim selected() As String = hotelName.Substring(21, hotelName.Length - 24).Split("-")
            hotelName = selected(1)
            Session("HotelReportNBC") = hotelName
            currentReport = getHotelRBNReport(selected(0))
            If currentReport IsNot Nothing AndAlso currentReport.Tables.Count > 0 AndAlso currentReport.Tables(0).Rows.Count > 0 Then
                ' Se habilita el boton de descarga

                lblError.Visible = False
                ' Centrado del contenido en DataGrid y DataBinding
                nightsByChannel.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                nightsByChannel.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                nightsByChannel.AlternatingItemStyle.HorizontalAlign = HorizontalAlign.Center
                nightsByChannel.DataSource = currentReport
                Session("dvReportNBC") = currentReport
                btnExcel.Visible = True
                btnExcel.DataBind()
                nightsByChannel.DataBind()
            Else
                lblError.Text = PortalCulture.GetString("reportNoResults")
                lblError.Visible = True
                nightsByChannel.DataSource = Nothing
                nightsByChannel.DataBind()
                btnExcel.Visible = False
                btnExcel.DataBind()
            End If

        End If

    End Sub

    Public Function getHotelRBNReport(ByVal idEmpresa As String) As DataSet
        ' [spHotelNightsByChannelAndMonth] 15268,'2023/10/01'
        Session("dvReportNBC") = Nothing
        currentReport = Nothing
        Dim conection As New SqlConnection(AppSettings("HotelConnectionString"))
        Dim command As New SqlCommand("spHotelNightsByChannelAndMonth", conection)
        yearSelected = ddlYear.SelectedItem.Value
        monthSelected = ddlMonth.SelectedItem.Value
        Dim dateToSearch As Date = yearSelected & "/" & monthSelected & "/01"
        currentReportName = "Report_NightsByChannel_" & idEmpresa & "_" & yearSelected & "-" & monthSelected
        Session("titleReportNBC") = currentReportName

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@IdEmpresa", idEmpresa))
            .Parameters.Add(New SqlParameter("@Month", dateToSearch.ToString("yyyy/MM/dd")))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)

        If dRes IsNot Nothing AndAlso dRes.Tables.Count > 0 AndAlso dRes.Tables(0).Rows.Count > 0 Then
            dRes.Tables(0).Columns.Add("TxD", GetType(Integer)) ' Total por dia
            Dim colmns As Integer = dRes.Tables(0).Columns.Count - 1 ' Total de columnas
            Dim dr As DataRow
            Dim txs(colmns - 1) As Integer

            For Each dr In dRes.Tables(0).Rows
                Dim txd As Integer = 0
                For x As Integer = 1 To colmns - 1 ' total de columnas menos "TxD"
                    If dr(x) Is Nothing Or IsDBNull(dr(x)) Then
                        dr(x) = 0
                    Else
                        txd += dr(x)
                    End If
                    txs(x - 1) += dr(x) ' Aqui suma de la primera ota a la ultima
                Next
                dr("TxD") = txd
                txs(colmns - 1) += txd ' Aqui suma lo resultante de txd
            Next

            dRes.Tables(0).Rows.Add(dRes.Tables(0).NewRow) ' Total por source
            dRes.Tables(0).Rows(dRes.Tables(0).Rows.Count - 1)(0) = PortalCulture.GetString("tt_totalBySource") ' TxS
            dRes.Tables(0).Columns("day").ColumnName = PortalCulture.GetString("commonString_Day")
            dRes.Tables(0).Columns("TxD").ColumnName = PortalCulture.GetString("tt_totalByDay")

            For x As Integer = 0 To colmns - 1 ' total de columnas menos "TxD"
                dRes.Tables(0).Rows(dRes.Tables(0).Rows.Count - 1)(x + 1) = txs(x)
            Next

            dRes.AcceptChanges()
        End If

        Return dRes
    End Function

End Class