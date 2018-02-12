Partial Class ExportExcel
    Inherits System.Web.UI.Page

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Public Enum Columns As Integer
        Cliente
        Address
        TravelerCity
        TravelerState
        TravelerZip
        TravelerPhoneHome
        Email
        Itinerario
        Llegada
        TipoHab
        IATA
        Agency
        earlyOut
        Noshow
        Fee
    End Enum

    Public Function ColumunsName(ByVal col As Columns) As String
        Select Case col
            Case Columns.Itinerario
                Return PortalCulture.GetString("M000119")
            Case Columns.Cliente
                Return PortalCulture.GetString("00721")
            Case Columns.Llegada
                Return PortalCulture.GetString("M000122")
            Case Columns.Address
                Return PortalCulture.GetString("00722")
            Case Columns.TravelerCity
                Return PortalCulture.GetString("00723")
            Case Columns.TravelerZip
                Return PortalCulture.GetString("00731")
            Case Columns.TravelerPhoneHome
                Return PortalCulture.GetString("00725")
            Case Columns.Email
                Return PortalCulture.GetString("00726")
            Case Columns.IATA
                Return PortalCulture.GetString("00730")
            Case Columns.TipoHab
                Return PortalCulture.GetString("M000124")
            Case Columns.Agency
                Return PortalCulture.GetString("01507")
            Case Columns.earlyOut
                Return PortalCulture.GetString("01586")
            Case Columns.Noshow
                Return "No Show"
            Case Columns.Fee
                Return PortalCulture.GetString("M0BT0000322").Substring(0, 8)
        End Select
    End Function


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Try
            If Not IsPostBack Then
                Me.Grid.Columns(Columns.Itinerario).HeaderText = ColumunsName(Columns.Itinerario)
                Me.Grid.Columns(Columns.Cliente).HeaderText = ColumunsName(Columns.Cliente)
                Me.Grid.Columns(Columns.Llegada).HeaderText = ColumunsName(Columns.Llegada)
                Me.Grid.Columns(Columns.TipoHab).HeaderText = ColumunsName(Columns.TipoHab)

                Me.Grid.Columns(Columns.Address).HeaderText = ColumunsName(Columns.Address)
                Me.Grid.Columns(Columns.TravelerCity).HeaderText = ColumunsName(Columns.TravelerCity)
                Me.Grid.Columns(Columns.TravelerState).HeaderText = ColumunsName(Columns.TravelerState)
                Me.Grid.Columns(Columns.TravelerZip).HeaderText = ColumunsName(Columns.TravelerZip)
                Me.Grid.Columns(Columns.TravelerPhoneHome).HeaderText = ColumunsName(Columns.TravelerPhoneHome)
                Me.Grid.Columns(Columns.Email).HeaderText = ColumunsName(Columns.Email)
                Me.Grid.Columns(Columns.IATA).HeaderText = ColumunsName(Columns.IATA)
                Me.Grid.Columns(Columns.Agency).HeaderText = ColumunsName(Columns.Agency)
                Me.Grid.Columns(Columns.earlyOut).HeaderText = ColumunsName(Columns.earlyOut)
                Me.Grid.Columns(Columns.Noshow).HeaderText = ColumunsName(Columns.Noshow)
                Me.Grid.Columns(Columns.Fee).HeaderText = ColumunsName(Columns.Fee)

                If ConfigurationManager.AppSettings("idSegmento") <> 4 Then
                    Me.Grid.Columns(Columns.earlyOut).Visible = False
                    Me.Grid.Columns(Columns.Noshow).Visible = False
                End If

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

                If Not dt.Columns.Contains("noshow") Then
                    dt.Columns.Add("noshow", Type.GetType("System.Boolean"))
                    dt.Columns.Add("earlyOut", Type.GetType("System.Boolean"))
                End If

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
            e.Item.Cells(Columns.Llegada).Text = CDate(e.Item.Cells(Columns.Llegada).Text).ToString("MMM/dd/yyyy")

        End If
    End Sub



End Class
