Public Class ExportExcellReportNBC
    Inherits System.Web.UI.Page

    Public Enum Columns As Integer
        Dia
        Booking
        CallCenter
        Expedia
        PriceTravel
        RoomCloud
        WebSite
    End Enum

    Public Function ColumunsName(ByVal col As Columns) As String
        Select Case col
            Case Columns.Dia
                Return "Dia"
            Case Columns.Booking
                Return "Booking.com"
            Case Columns.CallCenter
                Return "Call Center"
            Case Columns.Expedia
                Return "Expedia"
            Case Columns.PriceTravel
                Return "Price Travel"
            Case Columns.RoomCloud
                Return "Room Cloud"
            Case Columns.WebSite
                Return "Web Site"
        End Select
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                ' Formatear la fecha a fecha corta y hora

                Dim hotelName As String = Session("HotelReportNBC").ToString()
                Dim generatedAt As String = DateTime.Now.ToString("dd/MMM/yyyy hh-mm-ss")

                Response.Clear()
                Response.Buffer = True
                Response.ClearHeaders()
                Response.CacheControl = "no-cache"
                Response.AddHeader("Pragma", "no-cache")
                Response.AddHeader("content-disposition", "attachment;filename=" + Session("titleReportNBC") + "_" + generatedAt + ".xls")
                Response.Charset = ""
                Response.ContentEncoding = System.Text.Encoding.Default

                lblHotel.Text = hotelName
                lblGenerated.Text = PortalCulture.GetString("reportText_Generated") + generatedAt

                Response.ContentType = "application/ms-excel"
                Dim stringWrite As New System.IO.StringWriter
                Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)
                Dim dt As New DataTable
                dt.Merge(CType(Session("dvReportNBC"), DataSet).Tables(0))

                tblTitle.RenderControl(htmlWrite)
                tblHeaders.RenderControl(htmlWrite)

                With Grid
                    .DataSource = dt
                    .DataBind()
                    .RenderControl(htmlWrite)
                End With
                Response.Write(stringWrite.ToString)
            End If
        Catch ex As Exception
            Response.Clear()
        Finally
            Response.End()
        End Try

    End Sub

End Class