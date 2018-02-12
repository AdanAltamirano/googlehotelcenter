Public Partial Class ExcelPackages
    Inherits System.Web.UI.Page

    Public Enum Columns As Integer
        packageid
        Itinerary
        TravelerName
        FechaReservacion
        checkin
        checkOut
        Actividad
        Autobus
        Status
        Fee
    End Enum

    Public Function ColumunsName(ByVal col As Columns) As String
        Select Case col
            Case Columns.packageid
                Return "ID"
            Case Columns.Itinerary
                Return PortalCulture.GetString("M000119")
            Case Columns.FechaReservacion
                Return PortalCulture.GetString("M000120")
            Case Columns.TravelerName
                Return PortalCulture.GetString("M000121")
            Case Columns.checkin
                Return PortalCulture.GetString("M000122")
            Case Columns.checkOut
                Return PortalCulture.GetString("M000123")
            Case Columns.Status
                Return PortalCulture.GetString("M000562")
            Case Columns.Actividad
                Return PortalCulture.GetString("01039")
            Case Columns.Autobus
                Return "Autobus"
            Case Columns.Fee
                Return PortalCulture.GetString("M0BT0000322").Substring(0, 8)
        End Select
    End Function


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Try
            If Not IsPostBack Then
                Me.Grid.Columns(Columns.Itinerary).HeaderText = ColumunsName(Columns.Itinerary)
                Me.Grid.Columns(Columns.packageid).HeaderText = ColumunsName(Columns.packageid)
                Me.Grid.Columns(Columns.TravelerName).HeaderText = ColumunsName(Columns.TravelerName)
                Me.Grid.Columns(Columns.FechaReservacion).HeaderText = ColumunsName(Columns.FechaReservacion)
                Me.Grid.Columns(Columns.checkin).HeaderText = ColumunsName(Columns.checkin)
                Me.Grid.Columns(Columns.checkOut).HeaderText = ColumunsName(Columns.checkOut)
                Me.Grid.Columns(Columns.Actividad).HeaderText = ColumunsName(Columns.Actividad)
                Me.Grid.Columns(Columns.Autobus).HeaderText = ColumunsName(Columns.Autobus)
                Me.Grid.Columns(Columns.Status).HeaderText = ColumunsName(Columns.Status)
                Me.Grid.Columns(Columns.Fee).HeaderText = ColumunsName(Columns.Fee)

                Response.Clear()
                Response.Buffer = True
                Response.ClearHeaders()
                Response.CacheControl = "no-cache"
                Response.AddHeader("Pragma", "no-cache")
                Response.AddHeader("content-disposition", "attachment;filename=ReservationsReport_" + DateTime.Now.ToShortDateString().ToString() + ".xls")
                Response.Charset = ""
                Response.ContentEncoding = System.Text.Encoding.Default

                Response.ContentType = "application/ms-excel"
                Dim stringWrite As New System.IO.StringWriter
                Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)
                Dim dt As New DataTable
                dt.Merge(CType(Session("dvRes"), DataView).Table())
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

    Private Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Cells(Columns.FechaReservacion).Text = CDate(e.Item.Cells(Columns.FechaReservacion).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(Columns.checkin).Text = CDate(e.Item.Cells(Columns.checkin).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(Columns.checkOut).Text = CDate(e.Item.Cells(Columns.checkOut).Text).ToString("MMM/dd/yyyy")

            Dim status() As String = {"undefined", PortalCulture.GetString("M000331"), PortalCulture.GetString("M000592"), PortalCulture.GetString("M000333"), PortalCulture.GetString("00719")}
            e.Item.Cells(Columns.Status).Text = status(CType(e.Item.Cells(Columns.Status).Text, Integer))
            If Not e.Item.Cells(Columns.Actividad).Text = "0" Then
                e.Item.Cells(Columns.Actividad).Text = PortalCulture.GetString("00051")
            Else
                e.Item.Cells(Columns.Actividad).Text = "--"
            End If
            If Not e.Item.Cells(Columns.Autobus).Text = "0" Then
                e.Item.Cells(Columns.Autobus).Text = PortalCulture.GetString("00051")
            Else
                e.Item.Cells(Columns.Autobus).Text = "--"
            End If
        End If
    End Sub
End Class