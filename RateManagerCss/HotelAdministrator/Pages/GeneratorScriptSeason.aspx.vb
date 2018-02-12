Imports XCrypt
Imports System.Text
Imports System.Configuration.ConfigurationManager
Imports System.Web.Security
Imports Portal.Hotel.Facade
Imports Portal.General.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Common
Imports System.Data.SqlClient
Imports System.Xml


Partial Class GeneratorScriptSeason
    Inherits PaginaBase

#Region " Web Form Designer Generated Code "

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If MyBase.IsSupervisor OrElse MyBase.IsUnibilling OrElse MyBase.IsContent Then
            If MyBase.cInfoActual.Hotel = 0 Then
                MyBase.redirectTo(PaginaBase.pages.SearchHotel)
            Else
                cmbHoteles.Visible = False
                ShowScript(MyBase.cInfoActual.Hotel, MyBase.cInfoActual.HotelName)
            End If
        Else
            If Not IsPostBack Then
                tbScripts.Visible = False
                LoadCmbHotels()
            End If
        End If
    End Sub



    Private Sub LoadCmbHotels()
        cmbHoteles.DataSource = Nothing
        cmbHoteles.Items.Clear()

        Dim ds As DataSet
        If Not MyBase.IsUsuarioHotel Then
            With New HotelSistema
                ds = .GetHotelsCompanyByUser(Usuario)
            End With
        ElseIf MyBase.IsUsuarioHotel Then
            With New HotelSistema
                ds = .GetHotelsCompanyByUserHotelId(Usuario)
            End With
        End If

        If Not ds Is Nothing Then
            cmbHoteles.DataSource = ds.Tables(0)
            cmbHoteles.DataTextField = "Nombre"
            cmbHoteles.DataValueField = "idHotel"
            cmbHoteles.DataBind()
            cmbHoteles.Items.Insert(0, PortalCulture.GetString("M000272"))

        End If
    End Sub

    Private Sub ShowScript(ByVal idHotel As Integer, ByVal hotelName As String)
        Try


            Dim xe As XCryptEngine = New XCryptEngine
            Dim ds As DataSet = GetRatePlanSeasonByIdHotel(idHotel)
            Dim encryptedQueryEs As String = ascii2hex(xe.Encrypt(String.Format("{0}$es", idHotel)))
            Dim encryptedQueryEn As String = ascii2hex(xe.Encrypt(String.Format("{0}$en", idHotel)))
            Dim dr As DataRow


            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
                txtEspañol.Text += String.Format("<!-- Español[{0}]  {1} -->" + vbCrLf, idHotel, hotelName)
                txtIngles.Text += String.Format("<!-- English[{0}]  {1} -->" + vbCrLf, idHotel, hotelName)
                txtEspañol.Text += "<!-- Tarifas Disponible para el hotel -->"
                txtIngles.Text += "<!-- Available Rates for the Hotel -->"
                For Each dr In ds.Tables(0).Rows
                    txtEspañol.Text += String.Format(vbCrLf + "<div class=""showRateBlock"" id=""{0}""></div>", dr("RatePlan"))
                    txtIngles.Text += String.Format(vbCrLf + "<div class=""showRateBlock"" id=""{0}""></div>", dr("RatePlan"))
                Next
            End If

            xe.InitializeEngine(XCryptEngine.AlgorithmType.TripleDES)
            xe.Key = "Crs-OnePage"
            encryptedQueryEs = ascii2hex(xe.Encrypt(String.Format("{0}$es", idHotel)))
            encryptedQueryEn = ascii2hex(xe.Encrypt(String.Format("{0}$en", idHotel)))


            txtEspañol.Text += String.Format(vbCrLf + vbCrLf + "<script type=""text/javascript"" src=""https://crs.univisit.com/wshoteluv2/pages/GetPriceRateByRoom.aspx?code={0}""></script>", encryptedQueryEs)
            txtIngles.Text += String.Format(vbCrLf + vbCrLf + "<script type=""text/javascript"" src=""https://crs.univisit.com/wshoteluv2/pages/GetPriceRateByRoom.aspx?code={0}""></script>", encryptedQueryEn)
            tbScripts.Visible = True
        Catch ex As Exception
        End Try
    End Sub

    '[spGetRatePlanSeasonByIdHotel]

    Private Function ascii2hex(ByVal ascii As String) As String
        Dim hex As New StringBuilder
        Try
            For Each c As Char In ascii
                hex.Append(String.Format("{0:X2}", Asc(c)))
            Next
        Catch ex As Exception
        End Try
        Return hex.ToString
    End Function

    Private Function hex2ascii(ByVal hex As String) As String
        Dim ascii As New StringBuilder
        Try
            For i As Integer = 0 To hex.Length - 1 Step 2
                'viktor ascii.Append(Chr(Integer.Parse(hex.Substring(i, 2).ToUpper, Globalization.NumberStyles.HexNumber)))
            Next
        Catch ex As Exception
        End Try
        Return ascii.ToString
    End Function

    Private Function GetRatePlanSeasonByIdHotel(ByVal idHotel As Integer) As DataSet
        Try
            Dim ds As DataSet = New DataSet
            Dim ConnectionString As String = AppSettings("HotelConnectionString")
            Dim adapter As SqlDataAdapter = New SqlDataAdapter
            Dim cnn As SqlConnection = New SqlConnection(ConnectionString)
            Dim cmd As SqlCommand = New SqlCommand("spGetRatePlanSeasonByIdHotel", cnn)

            cmd.Parameters.Add("@idHotel", idHotel)
            cmd.CommandType = CommandType.StoredProcedure
            adapter.SelectCommand = cmd
            adapter.Fill(ds)

            Return ds
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    Private Sub cmbHoteles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbHoteles.SelectedIndexChanged
        txtEspañol.Text = ""
        txtIngles.Text = ""
        tbScripts.Visible = False
        If cmbHoteles.SelectedIndex > 0 Then
            ShowScript(cmbHoteles.SelectedValue, cmbHoteles.SelectedItem.Text)
        End If

    End Sub
    Private Sub LoadResources()
        lblTitulo.Text = PortalCulture.GetString("00854")
        lblIngles.Text = PortalCulture.GetString("M0BT0000081", True)
        lblEspañol.Text = PortalCulture.GetString("M0BT0000080", True)
        btnCopiarEspaniol.Value = PortalCulture.GetString("00487")
        btnCopiarIngles.Value = PortalCulture.GetString("00487")



    End Sub
    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        btnCopiarEspaniol.Attributes("onclick") = String.Format("javascript:copia_portapapeles('{0}','{1}')", txtEspañol.ClientID, PortalCulture.GetString("00855"))
        btnCopiarIngles.Attributes("onclick") = String.Format("javascript:copia_portapapeles('{0}','{1}')", txtIngles.ClientID, PortalCulture.GetString("00855"))
        LoadResources()
    End Sub
End Class


