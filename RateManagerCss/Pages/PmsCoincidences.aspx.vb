Imports Portal.Hotel.DataAccess
Imports Portal.General.DataAccess
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Microsoft.Office.Interop
Imports System.Net.Http
Imports System.IO
Imports ClosedXML.Excel

Partial Class PmsCoincidences
    Inherits PaginaBase

    Dim dsPMS As DataSet
    Dim fileRoute As String = ""
    Dim fileName As String = ""
    ' fileRoute & Session("PMSCodeId") & "_" & TipoArchivo & "_" & fileName

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Function SendExcelToConflux(type As String)
        ' URL de la API a la que deseas llamar
        Dim excelFile As String = fileRoute & Session("PMSCodeId") & "_" & type & "_" & fileName
        Dim confluxUrl As String = AppSettings("Default_Conflux_URL") & type

        ' Configurar el contenido de la solicitud con el archivo Excel
        Dim content As New MultipartFormDataContent()

        ' Guardar el archivo Excel en una secuencia de bytes
        Dim excelBytes As Byte() = Nothing
        Dim ms As New MemoryStream()

        Using workBook As New XLWorkbook(excelFile)
            workBook.SaveAs(ms)
            excelBytes = ms.ToArray()
        End Using

        ' Agregar el contenido del archivo Excel a la solicitud
        Dim excelContent As New ByteArrayContent(excelBytes)
        excelContent.Headers.ContentType = New System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        content.Add(excelContent, "archivo", excelFile)

        ' Crear una instancia de HttpClient
        Using httpClient As New HttpClient()
            ' Realizar la llamada a la API utilizando una solicitud POST y enviar el archivo Excel
            Dim response As HttpResponseMessage = httpClient.PostAsync(confluxUrl, content).Result

            ' Verificar si la llamada fue exitosa (código de estado 200)
            If response.IsSuccessStatusCode Then
                ' Leer el contenido de la respuesta como una cadena
                Dim responseData As String = response.Content.ReadAsStringAsync().Result

                ' Aquí puedes procesar la respuesta según tus necesidades
                'log("Respuesta de la API: " & responseData)
                PaginaBase.WriteLog(responseData, "Log_PMSRatesAndRooms_ExportExcel")
            Else
                ' La llamada no fue exitosa
                'MessageBox.Show(")
                PaginaBase.WriteLog("Error al llamar a la API. Código de estado: " & response.StatusCode.ToString() & " / Error MSG: " & response.ReasonPhrase.ToString(), "Log_PMSRatesAndRooms_ExportExcel_Error")
            End If
        End Using
    End Function

    Private Property hotelId() As Integer
        Get
            Return ViewState("hotelId")
        End Get
        Set(ByVal Value As Integer)
            ViewState("hotelId") = Value
        End Set
    End Property

    Dim hotels As DataTable
    Dim corporatives As DataTable = New DataTable
    Dim reportRequestTime As String = ""

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then
            Try
                btnExcel.Visible = If(Session("PMSRatePlans") IsNot Nothing AndAlso Session("PMSRatePlans").Tables(0).Rows.Count > 0 AndAlso Session("PMSRooms") IsNot Nothing AndAlso Session("PMSRooms").Tables(0).Rows.Count > 0, True, False)
            Catch ex As Exception
                btnExcel.Visible = False
            End Try

            If Not Session("idCorporativoUserChain") Is Nothing Then 'AndAlso Session("idCorporativoUserChain") <> "-1" Then

                'With (New Hoteles)
                '    ds = .LoadHotelsByIdCorp(Session("idCorporativoUserChain"))
                'End With

                With (New Hoteles)

                    Try
                        hotels = .LoadHotelsByUser(Session("idCorporativoUserChain")).Tables(0).Select("idcorporativo is not null").CopyToDataTable
                    Catch ex As Exception
                        hotels = Nothing
                    End Try

                    If hotels IsNot Nothing AndAlso hotels.Rows.Count > 0 Then

                        Session("dsHotelsList") = hotels

                        corporatives.Columns.Add("idCorporativo")
                        corporatives.Columns.Add("NombreCorp")
                        corporatives.PrimaryKey = New DataColumn() {corporatives.Columns("idCorporativo")}
                        Dim corpRow As DataRow
                        For Each r As DataRow In hotels.Rows

                            If corporatives.Rows.Find(r("idcorporativo")) Is Nothing Then
                                corpRow = corporatives.NewRow
                                corpRow.Item("idCorporativo") = r("idcorporativo")
                                corpRow.Item("NombreCorp") = r("NombreCorp")
                                corporatives.Rows.Add(corpRow)
                            End If
                        Next

                        corporatives.DefaultView.Sort = "NombreCorp asc"

                        ddlCorporatives.DataSource = corporatives
                        ddlCorporatives.DataTextField = "NombreCorp"
                        ddlCorporatives.DataValueField = "idCorporativo"
                        Dim item As ListItem = New ListItem
                        item.Text = PortalCulture.GetString("M000272", False)
                        item.Value = -1
                        ddlCorporatives.DataBind()
                        ddlCorporatives.Items.Insert(0, item)

                    End If

                    'ds.Clear()
                    'ds = New DataSet
                    'ds.Tables.Add(hotelsByCorporate)
                End With

            End If
        Else
            btnExcel.Visible = False
            If Session("dsHotelsList") IsNot Nothing Then
                hotels = New DataTable
                hotels.Merge(CType(Session("dsHotelsList"), DataTable))
            End If
        End If
    End Sub

    Protected Sub ddlCorporatives_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCorporatives.SelectedIndexChanged
        ddlHoteles.Items.Clear()
        If Integer.Parse(ddlCorporatives.SelectedValue) > 0 Then
            If Not hotels Is Nothing AndAlso hotels.Rows.Count Then
                Dim hotelsByCorporate As DataTable = hotels.Select("idCorporativo = " & ddlCorporatives.SelectedValue.ToString).CopyToDataTable
                ddlHoteles.DataSource = hotelsByCorporate
                ddlHoteles.DataTextField = "Nombre"
                ddlHoteles.DataValueField = "idHotel"
                ddlHoteles.DataBind()
            End If
            lblHotelSelected.Style.Add("display", "none")
            lblHotelSelected.Text = ""
        End If
        Dim item As ListItem = New ListItem
        item.Text = PortalCulture.GetString("M000272", False)
        item.Value = -1
        ddlHoteles.Items.Insert(0, item)
    End Sub

    Private Function getHotelPMSRoomsAndRatePlans(ByVal idHotel As Integer, Optional ByVal language As Integer = 1) As DataSet
        ' [spGeRatesAndRoomsPMS] 2135 , 1
        Dim conection As New SqlConnection(AppSettings("HotelConnectionString"))
        Dim command As New SqlCommand("spGeRatesAndRoomsPMS", conection)

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idHotel", idHotel)) ' Se usa como comodin en el sp
            .Parameters.Add(New SqlParameter("@language", language)) ' Movimiento 300 : Busqueda por idEmpresa o IdHotel
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)

        If dRes.Tables(0).Rows.Count > 0 Then
            btnSendToConflux.Visible = True
        End If

        Return dRes
    End Function

    Public Function CreatePMSDataDoc(ByVal subtitle As String, ByVal table As DataTable)

        Dim excellApp As New Excel.Application()

        Dim workBook As Excel.Workbook = excellApp.Workbooks.Add()
        Dim workSheet As Excel.Worksheet = CType(workBook.Sheets(1), Excel.Worksheet)

        ' Encabezados
        Dim tColumns As Integer = table.Columns.Count
        For x As Integer = 0 To tColumns - 1
            workSheet.Cells(1, x + 1) = table.Columns(x).ColumnName
        Next

        If subtitle.Equals("RatePlans") Then
            Dim rangeH As Excel.Range = workSheet.Range("H1").EntireColumn
            Dim rangeI As Excel.Range = workSheet.Range("I1").EntireColumn
            Dim rangeL As Excel.Range = workSheet.Range("L1").EntireColumn

            ' Establece formato "Texto" para columnas de hora (Ej. 20:00)
            rangeH.NumberFormat = "@"
            rangeI.NumberFormat = "@"
            rangeL.NumberFormat = "@"

        End If

        ' Renglones
        Dim tRows As Integer = table.Rows.Count
        For x As Integer = 0 To tRows - 1
            For z As Integer = 0 To tColumns - 1
                workSheet.Cells(x + 2, z + 1) = table.Rows(x).ItemArray(z)
            Next
        Next

        fileRoute = Server.MapPath("~" & AppSettings("Default_Conflux_Docs_Folder") & "/") '"C:\testFolder\"
        fileName = Session("PMSHotelName") & "_" & Date.Now.ToString("dd-MM-yyyy") & "_" & reportRequestTime & ".xlsx"

        workBook.SaveAs(fileRoute & Session("PMSCodeId") & "_" & subtitle & "_" & fileName)

        excellApp.Quit()
        ReleaseComObject(excellApp)

    End Function

    Private Function labelsState(ByVal state As Boolean)
        lbl_PMS_Data.Visible = state
        lblRatesPlan.Visible = state
        lblRooms.Visible = state
        lbl_Conflux_Data_RP.Visible = state
        lbl_Conflux_Data_RT.Visible = state
    End Function

    Private Sub btnSendToConflux_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendToConflux.Click

        If String.IsNullOrWhiteSpace(fileRoute) Or String.IsNullOrWhiteSpace(fileName) Then
            dsPMS = getHotelPMSRoomsAndRatePlans(Me.hotelId, 1)
            reportRequestTime = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Millisecond.ToString
            CreatePMSDataDoc("RatePlans", dsPMS.Tables(0))
            CreatePMSDataDoc("Rooms", dsPMS.Tables(1))
        End If

        SendExcelToConflux("RatePlans")
        SendExcelToConflux("Rooms")
    End Sub

    Private Sub btnCargar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargar.Click
        lblMsgActualizacion.Visible = False
        If ddlHoteles.SelectedValue <> -1 Then
            'Buscamos los ratesPlan del hotel
            Me.hotelId = ddlHoteles.SelectedValue
            lblHotelSelected.Text = ddlHoteles.SelectedItem.Text

            Session("PMSHotelName") = ddlHoteles.SelectedItem.Text
            Session("PMSCodeId") = hotels.Select("idHotel = " & Me.hotelId)(0).Item("idEmpresa")

            dsPMS = getHotelPMSRoomsAndRatePlans(Me.hotelId, 1)

            If Not IsNothing(dsPMS) Then

                If dsPMS.Tables(0).Rows.Count > 0 Then
                    dgCFRP.DataSource = dsPMS.Tables(0)
                    dgCFRP.DataBind()
                End If

                If dsPMS.Tables(1).Rows.Count > 0 Then
                    dgCFRT.DataSource = dsPMS.Tables(1)
                    dgCFRT.DataBind()
                End If

            End If

            lblHotelSelected.Style.Add("display", "block")
            Dim ds As DataSet
            With New RatePlanAccess
                ds = .LoadRatesPlanPMS(Me.hotelId, PortalCulture.GetIDCulture, 1, 1)
            End With

            If ds IsNot Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then

                Dim rpTable As New DataTable
                rpTable.Columns.Add("Código")
                rpTable.Columns.Add("Nombre")

                Dim rpRow As DataRow

                For Each row As DataRow In ds.Tables(0).Rows
                    rpRow = rpTable.NewRow
                    rpRow("Código") = row("idRatePlan")
                    rpRow("Nombre") = row("Nombre")
                    rpTable.Rows.Add(rpRow)
                Next

                Session("PMSRatePlans") = rpTable

            Else
                Session("PMSRatePlans") = Nothing
            End If

            dgRatesPlan.DataSource = ds
            dgRatesPlan.DataKeyField = "idRatePlan"
            dgRatesPlan.DataBind()

            Dim dsRooms As DataSet
            With New RoomAccess
                dsRooms = .LoadRoomsPMS(Me.hotelId, PortalCulture.GetIDCulture)
            End With

            If dsRooms IsNot Nothing AndAlso dsRooms.Tables(0).Rows.Count > 0 Then

                Dim roomsTable As New DataTable
                roomsTable.Columns.Add("Código")
                roomsTable.Columns.Add("Nombre")

                Dim roomRow As DataRow

                For Each row As DataRow In dsRooms.Tables(0).Rows
                    roomRow = roomsTable.NewRow
                    roomRow("Código") = row("CodigoHabitacion")
                    roomRow("Nombre") = row("Nombre")
                    roomsTable.Rows.Add(roomRow)
                Next

                Session("PMSRooms") = roomsTable
            Else
                Session("PMSRooms") = Nothing
            End If

            dgRooms.DataSource = dsRooms
            dgRooms.DataBind()

            'renglonDatos.Style.Add("display", "block")
            'renglonBotones.Style.Add("display", "block")
            Dim ban As Boolean = False

            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                dgRatesPlan.Visible = True
                msgRatesPlan.Visible = False
                ban = True
            Else
                dgRatesPlan.Visible = False
                msgRatesPlan.Visible = True
            End If
            If Not dsRooms Is Nothing AndAlso dsRooms.Tables(0).Rows.Count > 0 Then
                dgRooms.Visible = True
                msgRooms.Visible = False
                ban = True
            Else
                dgRooms.Visible = False
                msgRooms.Visible = True
            End If
            If Not ban Then
                'renglonBotones.Style.Add("display", "none")
            End If
        Else
            'renglonDatos.Style.Add("display", "none")
            'renglonBotones.Style.Add("display", "none")
            lblHotelSelected.Text = ""
            lblHotelSelected.Style.Add("display", "none")
            Me.hotelId = -1
        End If

        btnExcel.Visible = If(Session("PMSRatePlans") IsNot Nothing AndAlso Session("PMSRatePlans").Rows.Count > 0 AndAlso Session("PMSRooms") IsNot Nothing AndAlso Session("PMSRooms").Rows.Count > 0, True, False)
        labelsState(btnExcel.Visible)
        btnExcel.DataBind()

    End Sub

    ' Función para liberar recursos de la Interfaz de Excel
    Private Sub ReleaseComObject(obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
        Catch ex As Exception
            ' Manejar la excepción si es necesario
        Finally
            obj = Nothing
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Call guardar()
    End Sub
    Private Sub guardar()
        If Me.hotelId <> -1 Then
            'Actualizamos los idratesplan del PMS
            For Each row As DataGridItem In dgRatesPlan.Items
                Dim txtidRatePlanPMS As TextBox
                txtidRatePlanPMS = row.FindControl("txtidRatePlanPMS")
                If Not txtidRatePlanPMS Is Nothing Then
                    Dim idRatePlan As String = row.Cells(0).Text
                    With New RatePlanAccess
                        .UpdRatesPlanPMS(Me.hotelId, idRatePlan, txtidRatePlanPMS.Text.Trim)
                    End With
                End If
            Next
            'Actualizamos los roomsCodes del PMS
            For Each row As DataGridItem In dgRooms.Items
                Dim txtCodigoHabitacionPMS As TextBox
                txtCodigoHabitacionPMS = row.FindControl("txtCodigoHabitacionPMS")
                If Not txtCodigoHabitacionPMS Is Nothing Then
                    Dim CodigoHabitacion As String = row.Cells(0).Text
                    With New RoomAccess
                        .UpdRoomsPMS(Me.hotelId, CodigoHabitacion, txtCodigoHabitacionPMS.Text.Trim)
                    End With
                End If
            Next
            'cancelar()
            lblMsgActualizacion.Visible = True
        End If
    End Sub

    Private Sub dgRatesPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRatesPlan.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            Dim txtidRatePlanPMS As TextBox
            txtidRatePlanPMS = e.Item.Cells(3).FindControl("txtidRatePlanPMS")
            If Not txtidRatePlanPMS Is Nothing Then
                txtidRatePlanPMS.Text = IIf(e.Item.Cells(2).Text = "&nbsp;", "", e.Item.Cells(2).Text)
            End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = "Codigo"
            e.Item.Cells(1).Text = "Nombre"
            e.Item.Cells(3).Text = "Hotel"
        End If
    End Sub

    Private Sub dgRooms_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRooms.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            Dim txtCodigoHabitacionPMS As TextBox
            txtCodigoHabitacionPMS = e.Item.Cells(3).FindControl("txtCodigoHabitacionPMS")
            If Not txtCodigoHabitacionPMS Is Nothing Then
                txtCodigoHabitacionPMS.Text = IIf(e.Item.Cells(2).Text = "&nbsp;", "", e.Item.Cells(2).Text)
            End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = "Codigo"
            e.Item.Cells(1).Text = "Nombre"
            e.Item.Cells(3).Text = "Hotel"
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        cancelar()
    End Sub
    Private Sub cancelar()
        'renglonDatos.Style.Add("display", "none")
        'renglonBotones.Style.Add("display", "none")
        ddlHoteles.SelectedIndex = 0
        Me.hotelId = -1
        lblHotelSelected.Text = ""
        lblHotelSelected.Style.Add("display", "none")
        lblMsgActualizacion.Visible = False
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        btnCargar.Text = PortalCulture.GetString("00149", False)
        lblRatesPlan.Text = PortalCulture.GetString("00047", False)
        lblRooms.Text = PortalCulture.GetString("00048", False)
        msgRatesPlan.Text = PortalCulture.GetString("01149", False)
        msgRooms.Text = PortalCulture.GetString("01150", False)
        lblMsgActualizacion.Text = PortalCulture.GetString("01151", False)
        btnAceptar.Text = PortalCulture.GetString("M000060", False)
        btnCancel.Text = PortalCulture.GetString("M000143", False)
        lblTitle.Text = PortalCulture.GetString("01152", False)
    End Sub
End Class
