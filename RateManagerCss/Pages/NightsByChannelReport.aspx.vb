Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Public Class NightsByChannelReport
    Inherits PaginaBase

    Dim currentReport As DataSet
    Dim currentReportName As String = ""
    Dim HotelName As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim currentYear As Integer = Date.Now.Year
        Dim yearsLimit As Integer = currentYear - 5
        ddlYear.Items.Clear()
        ddlMonth.Items.Clear()

        ddlYear.Items.Add(New ListItem(currentYear, currentYear, True))

        For year As Integer = currentYear - 1 To yearsLimit Step year - 1
            ddlYear.Items.Add(New ListItem(year, year))
        Next

        ddlMonth.Items.Add(New ListItem("Enero", "01"))
        ddlMonth.Items.Add(New ListItem("Febrero", "02"))
        ddlMonth.Items.Add(New ListItem("Marzo", "03"))
        ddlMonth.Items.Add(New ListItem("Abril", "04"))
        ddlMonth.Items.Add(New ListItem("Mayo", "05"))
        ddlMonth.Items.Add(New ListItem("Junio", "06"))
        ddlMonth.Items.Add(New ListItem("Julio", "07"))
        ddlMonth.Items.Add(New ListItem("Agosto", "08"))
        ddlMonth.Items.Add(New ListItem("Septiembre", "09"))
        ddlMonth.Items.Add(New ListItem("Octubre", "10"))
        ddlMonth.Items.Add(New ListItem("Noviembre", "11"))
        ddlMonth.Items.Add(New ListItem("Diciembre", "12"))

    End Sub

    Private Sub ctrlAutoCompleteHotels_onSendFilter(ByVal id As String, ByVal hotelName As String) Handles ctrlAutoCompleteHotels.OnSendFilter
        If Not String.IsNullOrWhiteSpace(hotelName) Then

            ' a partir de donde termina la cadena de like, mas el tamaño de la cadena menos el residuo constante
            Dim selected() As String = hotelName.Substring(21, hotelName.Length - 24).Split("-")
            hotelName = selected(1)
            currentReport = getHotelRBNReport(selected(0))
            If currentReport IsNot Nothing Then
                ' Se habilita el boton de descarga
                btnExcel.Enabled = True

                ' Centrado del contenido en DataGrid y DataBinding
                nightsByChannel.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                nightsByChannel.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                nightsByChannel.AlternatingItemStyle.HorizontalAlign = HorizontalAlign.Center
                nightsByChannel.DataSource = currentReport
                nightsByChannel.DataBind()
            End If
        End If

    End Sub

    Public Function getHotelRBNReport(ByVal idEmpresa As String) As DataSet
        ' [spHotelNightsByChannelAndMonth] 15268,'2023/10/01'

        Dim conection As New SqlConnection(AppSettings("HotelConnectionString"))
        Dim command As New SqlCommand("spHotelNightsByChannelAndMonth", conection)
        Dim dateToSearch As Date = ddlYear.SelectedValue & "/" & ddlMonth.SelectedValue & "/01"
        currentReportName = "Report_NightsByChannel_" & idEmpresa & "_" & ddlYear.SelectedValue & "-" & ddlMonth.SelectedValue

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@IdEmpresa", idEmpresa))
            .Parameters.Add(New SqlParameter("@Month", Date.Today.ToString("yyyy/MM/dd")))
            '.Parameters.Add(New SqlParameter("@Month", dateToSearch.ToString("yyyy/MM/dd")))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)

        Return dRes
    End Function

End Class