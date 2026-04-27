Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Portal.General.Facade

Partial Class LogReport
    Inherits PaginaBase

    Enum dgcolumns
        Usuario
        hplPagina
        Pagina
        Accion
        Fecha
        Hora
        Nota
        detalle
        idLog
        showlink
    End Enum

    ' Filter values captured before DataBind so ItemDataBound can read them
    Private _accionFilter  As String  = "-1"
    Private _usuarioFilter As String  = ""
    Private _totalRecords  As Integer = 0

#Region "Page events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Me.IsHotelSelected Then
            Me.redirectTo(PaginaBase.pages.SearchHotel)
        End If

        If Not IsPostBack Then
            Me.txtInicio.Text = Now.Date.ToString("MM/dd/yyyy")
            Me.txtFinal.Text  = Now.Date.ToString("MM/dd/yyyy")

            LoadHotelDropdown()
            LoadAccionDropdown()
        End If

        btnSearch.OnClientClick = "return FireUpdateStatus();"
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lbltitle.Text  = PortalCulture.GetString("01467")
        lblInicio.Text = PortalCulture.GetString("00108", True)
        lblFinal.Text  = PortalCulture.GetString("00109", True)
        btnSearch.Text = PortalCulture.GetString("M0BT0000115")
        lblAccion.Text = "Acción:"
        lblUsuario.Text = "Usuario:"
    End Sub

#End Region

#Region "Data loading"

    Private Sub LoadHotelDropdown()
        ddlHoteles.DataTextField  = "NombreEmpresa"
        ddlHoteles.DataValueField = "ID"

        If Me.IsSupervisor Then
            Dim data As DataSet  = GetCompanies()
            Dim dv   As DataView = data.Tables(0).DefaultView
            dv.RowFilter = "ID is not null and ID<>0"
            Me.ddlHoteles.DataSource = dv
            Me.ddlHoteles.DataBind()
        Else
            Dim reader As SqlDataReader
            With New HotelSistema
                If Not MyBase.IsUsuarioHotelAssociation Then
                    reader = .GetHotelsCompanyByUserId(Usuario)
                Else
                    reader = .GetHotelsCompanyByUserIdAssociation(Usuario)
                End If
            End With
            While reader.Read
                ddlHoteles.Items.Add(New ListItem(reader.Item("Nombre"), reader.Item("idHotel")))
            End While
        End If

        Me.ddlHoteles.Items.Insert(0, New ListItem("All", "0"))

        If ddlHoteles.Items.Count > 1 Then
            ddlHoteles.SelectedIndex = ddlHoteles.Items.IndexOf(ddlHoteles.Items.FindByValue(cInfoActual.Hotel))
        End If
    End Sub

    Private Sub LoadAccionDropdown()
        ddlAccion.Items.Clear()
        ddlAccion.Items.Add(New ListItem("Todas",        "-1"))
        ddlAccion.Items.Add(New ListItem("Creación",      "0"))
        ddlAccion.Items.Add(New ListItem("Modificación",  "1"))
        ddlAccion.Items.Add(New ListItem("Eliminación",   "2"))
        ddlAccion.Items.Add(New ListItem("LogIn",         "3"))
        ddlAccion.Items.Add(New ListItem("Envío tarifa",  "8"))
    End Sub

    Private Sub loaddatos()
        ' Capture filters before DataBind triggers ItemDataBound
        _accionFilter  = ddlAccion.SelectedValue
        _usuarioFilter = txtUsuario.Text.Trim()
        _totalRecords  = 0

        Dim ci As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)

        Dim datos As LogData = Leerlog(ToDate(txtInicio.Text), ToDate(txtFinal.Text), Me.ddlHoteles.SelectedItem.Value)

        If Me.ddlHoteles.SelectedItem.Value = 0 Then
            Dim dr As DataRow = datos.Tables("hoteles").NewRow()
            dr("idhotel") = 0
            dr("nombre")  = PortalCulture.GetString("00172")
            datos.Tables("hoteles").Rows.Add(dr)
        End If

        If datos.Tables.Count = 2 Then
            Me.dlHoteles.DataKeyField = "idhotel"
            Me.dlHoteles.DataMember  = datos.Tables("hoteles").TableName
            Me.dlHoteles.DataSource  = datos
            Me.dlHoteles.DataBind()
        End If

        System.Threading.Thread.CurrentThread.CurrentCulture = ci

        If _totalRecords > 0 Then
            lblTotal.Text = String.Format("{0} registro(s) encontrado(s)", _totalRecords)
        Else
            lblTotal.Text = "No se encontraron registros para los filtros seleccionados."
        End If
    End Sub

    Private Function GetCompanies() As DataSet
        Dim conn    As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spCompanySearchCompanys", conn)
        command.CommandType = CommandType.StoredProcedure
        command.Parameters.Add(New SqlParameter("@idRubro", AppSettings("idRubro")))
        Dim adapter As New SqlDataAdapter(command)
        Dim ds      As New DataSet
        adapter.Fill(ds)
        Return ds
    End Function

#End Region

#Region "Button events"

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        loaddatos()
    End Sub

#End Region

#Region "DataList / DataGrid binding"

    Private Sub dlHoteles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlHoteles.ItemDataBound
        Dim ds As DataSet = TryCast(dlHoteles.DataSource, DataSet)
        If ds Is Nothing OrElse ds.Tables.Count < 2 Then Return

        Dim dv As DataView = ds.Tables("Log").DefaultView

        ' Build RowFilter: hotel + optional action + optional user
        Dim filterParts As New System.Collections.Generic.List(Of String)
        filterParts.Add("isnull(Hotel,0)=" & dlHoteles.DataKeys(e.Item.ItemIndex))

        If _accionFilter <> "-1" Then
            filterParts.Add("Accion=" & _accionFilter)
        End If
        If Not String.IsNullOrEmpty(_usuarioFilter) Then
            Dim safeUser As String = _usuarioFilter.Replace("'", "''")
            filterParts.Add("Usuario LIKE '%" & safeUser & "%'")
        End If

        dv.RowFilter = String.Join(" AND ", filterParts.ToArray())

        Dim dg    As DataGrid  = e.Item.FindControl("dgLog")
        Dim tshow As Web.UI.HtmlControls.HtmlTable = e.Item.FindControl("tshow")

        If dg Is Nothing Then Return

        If dv.Count = 0 Then
            ' Hide hotel section entirely when no matching records
            e.Item.Visible = False
            Return
        End If

        ' Wire up the expand/collapse click on the hotel header
        If Not tshow Is Nothing Then
            tshow.Rows(0).Cells(0).Attributes.Add("onclick",
                "showHotel('" & dg.ClientID & "','" & tshow.Rows(0).Cells(0).ClientID & "');")
        End If

        AddHandler dg.ItemDataBound, AddressOf dglog_ItemDataBound
        dg.DataSource = dv
        dg.DataBind()
        dg.Visible = True
        _totalRecords += dv.Count
    End Sub

    Private Sub dglog_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            ' ── Translate numeric action code to colored badge ──
            Dim accionVal As Integer
            If Integer.TryParse(e.Item.Cells(dgcolumns.Accion).Text, accionVal) Then
                Select Case accionVal
                    Case 0 : e.Item.Cells(dgcolumns.Accion).Text = "<span class='badge badge-create'>Creación</span>"
                    Case 1 : e.Item.Cells(dgcolumns.Accion).Text = "<span class='badge badge-edit'>Modificación</span>"
                    Case 2 : e.Item.Cells(dgcolumns.Accion).Text = "<span class='badge badge-delete'>Eliminación</span>"
                    Case 3 : e.Item.Cells(dgcolumns.Accion).Text = "<span class='badge badge-login'>LogIn</span>"
                    Case 8 : e.Item.Cells(dgcolumns.Accion).Text = "<span class='badge badge-send'>Envío tarifa</span>"
                    Case Else : e.Item.Cells(dgcolumns.Accion).Text = "<span class='badge badge-other'>Acción " & accionVal & "</span>"
                End Select
            End If

            ' ── Format date and time columns ──
            e.Item.Cells(dgcolumns.Fecha).Text = CDate(e.Item.Cells(dgcolumns.Fecha).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(dgcolumns.Hora).Text = CDate(e.Item.Cells(dgcolumns.Hora).Text).ToString("T", New System.Globalization.CultureInfo("de-DE"))

            ' ── Page hyperlink ──
            Dim hpl As HyperLink = e.Item.FindControl("hplPagina")
            If Not hpl Is Nothing Then
                hpl.NavigateUrl = GeRequestApplicationPath(e.Item.Cells(dgcolumns.Pagina).Text)
                hpl.Text = PortalCulture.GetString("00393")
            End If

            ' ── Detail hyperlink (only shown when log has data) ──
            Dim bshowlink As Byte
            Byte.TryParse(e.Item.Cells(dgcolumns.showlink).Text, bshowlink)
            Dim idlog As Integer = Integer.Parse(e.Item.Cells(dgcolumns.idLog).Text)
            Dim hplDetail As HyperLink = e.Item.FindControl("lnkDetalle")
            If Not hplDetail Is Nothing Then
                hplDetail.NavigateUrl = GeRequestApplicationPath("/Portal/Pages/LogDetalle.aspx") & "?id=" & idlog
                hplDetail.Text = PortalCulture.GetString("00903")
                hplDetail.CssClass = "detail-link"
                hplDetail.Visible = (bshowlink = 1)
            End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Usuario).Text = PortalCulture.GetString("M000027")
            e.Item.Cells(dgcolumns.Accion).Text  = PortalCulture.GetString("00425")
            e.Item.Cells(dgcolumns.Fecha).Text   = PortalCulture.GetString("M000120")
            e.Item.Cells(dgcolumns.Hora).Text    = PortalCulture.GetString("00426")
            e.Item.Cells(dgcolumns.Nota).Text    = PortalCulture.GetString("00427")
            e.Item.Cells(dgcolumns.detalle).Text = PortalCulture.GetString("00903")
        End If
    End Sub

#End Region

#Region "Helpers"

    Public Shared Function ToDate(ByVal sDate As String) As Date
        Try
            Return System.DateTime.Parse(sDate,
                New System.Globalization.CultureInfo("en-US", True),
                System.Globalization.DateTimeStyles.NoCurrentDateDefault)
        Catch
            Try
                Return System.DateTime.Parse(sDate,
                    New System.Globalization.CultureInfo("es-MX", True),
                    System.Globalization.DateTimeStyles.NoCurrentDateDefault)
            Catch
                Return CDate(sDate)
            End Try
        End Try
    End Function

#End Region

End Class
