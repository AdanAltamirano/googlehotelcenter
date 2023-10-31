Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Xml

Public Class NightsByChannelReport
    Inherits PaginaBase

    Dim currentReport As DataSet
    Dim currentReportName As String = ""
    Dim yearSelected As String = Date.Today.Year.ToString()
    Dim monthSelected As String = "01"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim currentYear As Integer = Date.Now.Year
            Dim yearsLimit As Integer = currentYear - 5

            ddlYear.Items.Clear()

            ddlYear.Items.Add(New ListItem(currentYear, currentYear, True))

            For year As Integer = currentYear - 1 To yearsLimit Step year - 1
                ddlYear.Items.Add(New ListItem(year, year))
            Next

        End If

        btnExcel.Visible = If(Session("dvReportNBC") Is Nothing Or currentReport Is Nothing, False, True)
        btnExcel.DataBind()

    End Sub

    Private Sub ExcecuteReport()
        Dim searchInput As HtmlInputText = ctrlAutoCompleteHotels.Controls(0)

        If Not String.IsNullOrWhiteSpace(searchInput.Value) AndAlso searchInput.Value.Length > 0 Then

            Dim searchString As String = searchInput.Value

            Try
                If searchString.Contains("-") Then
                    Dim selected() As String = searchString.Split("-")
                    searchString = selected(1)
                    Dim idEmpresa As String = selected(0)

                    If Integer.Parse(idEmpresa) > 0 Then ' Si el idEmpresa es un valor entero valido
                        If searchString.Length > 0 AndAlso idEmpresa.Length > 0 Then
                            currentReport = getHotelRBNReport(idEmpresa)
                        End If

                        If currentReport IsNot Nothing AndAlso currentReport.Tables.Count > 0 AndAlso currentReport.Tables(0).Rows.Count > 0 Then
                            Session("HotelReportNBC") = searchString ' Guarda el nombre del hotel
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
                            showError(1)
                        End If
                    End If

                Else
                    showError(2)
                End If
            Catch ex As Exception
                showError() ' Busqueda no valida
            End Try
        Else
            showError(2)
        End If
    End Sub

    Private Sub ctrlAutoCompleteHotels_onSendFilter(ByVal id As String, ByVal hotelName As String) Handles ctrlAutoCompleteHotels.OnSendFilter
        ExcecuteReport()
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click

        ExcecuteReport()
    End Sub

    Private Function showError(Optional ByVal ErrorCode As Integer = 0)
        Dim ErrorString As String = PortalCulture.GetString("msgInvalidSearch")
        lblError.CssClass = "Validators" ' Texto en rojo
        'lblError.CssClass = "" ' Texto default
        Select Case ErrorCode
            Case 1
                ErrorString = PortalCulture.GetString("reportNoResults")
            Case 2
                ErrorString = PortalCulture.GetString("msgSelectOneHotel")

        End Select
        currentReport = Nothing
        Session("dvReportNBC") = Nothing

        lblError.Text = ErrorString
        lblError.Visible = True
        nightsByChannel.DataSource = Nothing
        nightsByChannel.DataBind()
        btnExcel.Visible = False
        btnExcel.DataBind()
    End Function



    Public Function getHotelRBNReport(ByVal idEmpresa As String) As DataSet
        ' [spHotelNightsByChannelAndMonth] 15268,'2023/10/01'
        Session("dvReportNBC") = Nothing
        currentReport = Nothing
        Session("HotelReportNBC") = ""
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