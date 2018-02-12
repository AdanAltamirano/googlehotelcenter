Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Xml
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página

        Dim reader As SqlDataReader

        If Not Me.IsHotelSelected Then

            Me.redirectTo(PaginaBase.pages.SearchHotel)
        End If
        If Not IsPostBack Then
            Me.txtInicio.Text = Now.Date.ToString("MM/dd/yyyy")
            Me.txtFinal.Text = Now.Date.ToString("MM/dd/yyyy")
            ddlHoteles.DataTextField = "NombreEmpresa"
            ddlHoteles.DataValueField = "ID"
            Dim data As DataSet, dv As DataView
            If Me.IsSupervisor Then
                data = getCompanys()
                dv = data.Tables(0).DefaultView
                dv.RowFilter = "ID is not null and ID<>0"
                Me.ddlHoteles.DataSource = dv
                Me.ddlHoteles.DataBind()
                Me.ddlHoteles.Items.Insert(0, "All")
                Me.ddlHoteles.Items(0).Value = 0
            Else
                With New HotelSistema
                    If Not MyBase.IsUsuarioHotelAssociation Then
                        reader = .GetHotelsCompanyByUserId(Usuario)
                    Else
                        reader = .GetHotelsCompanyByUserIdAssociation(Usuario)
                    End If
                End With
                While reader.Read
                    Dim item As New ListItem(reader.Item("Nombre"), reader.Item("idHotel"))
                    ddlHoteles.Items.Add(item)
                End While
                Me.ddlHoteles.Items.Insert(0, "All")
                Me.ddlHoteles.Items(0).Value = 0
            End If

            If ddlHoteles.Items.Count > 1 Then
                ddlHoteles.SelectedIndex = ddlHoteles.Items.IndexOf(ddlHoteles.Items.FindByValue(cInfoActual.Hotel))
            End If

        End If
        btnSearch.OnClientClick = "return FireUpdateStatus();"
    End Sub


    Public Shared Function ToDate(ByVal sDate As String) As Date
        Dim d1 As String

        Try
            d1 = System.DateTime.Parse(sDate, _
                                       New System.Globalization.CultureInfo("en-US", True), _
                                       System.Globalization. _
                                       DateTimeStyles.NoCurrentDateDefault)
        Catch ex As Exception
            Try
                d1 = System.DateTime.Parse(sDate, _
                                        New System.Globalization.CultureInfo("es-MX", True), _
                                        System.Globalization. _
                                        DateTimeStyles.NoCurrentDateDefault)

            Catch ex1 As Exception
                Return sDate
            End Try
        End Try
        Return d1
    End Function

    Private Sub loaddatos()
        Dim name As String
        Dim TblHoteles As String = "Hoteles"
        Dim datos As LogData
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)

        datos = Leerlog(ToDate(txtInicio.Text), ToDate(txtFinal.Text), Me.ddlHoteles.SelectedItem.Value)
        If Me.ddlHoteles.SelectedItem.Value = 0 Then
            Dim dr As DataRow
            dr = datos.Tables("hoteles").NewRow()
            dr("idhotel") = 0
            dr("nombre") = PortalCulture.GetString("00172")
            datos.Tables("hoteles").Rows.Add(dr)
        End If
        If (datos.Tables.Count = 2) Then
            Me.dlHoteles.DataKeyField = "idhotel"
            Me.dlHoteles.DataMember = datos.Tables("hoteles").TableName
            Me.dlHoteles.DataSource = datos
            Me.dlHoteles.DataBind()
        End If
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        loaddatos()
    End Sub
    Private Function getCompanys() As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spCompanySearchCompanys", conection)
        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idRubro", AppSettings("idRubro")))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lbltitle.text = PortalCulture.GetString("01467")
        Me.lblInicio.Text = PortalCulture.GetString("00108", True)
        Me.lblFinal.Text = PortalCulture.GetString("00109", True)
        btnSearch.Text = PortalCulture.GetString("M0BT0000115")
    End Sub

    Private Sub RedirectDetails(ByVal id As String)
        Me.redirectTo(PaginaBase.pages.DetailLog, String.Format("?id={0}", id))
    End Sub

    Private Sub dglog_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        If e.CommandName = "DetalleLog" Then
            Dim idlog As Integer
            idlog = Integer.Parse(e.Item.Cells(dgcolumns.idLog).Text)
            RedirectDetails(idlog)
        End If
    End Sub

    Private Sub dglog_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Item.Cells(dgcolumns.Accion).Text = acciones.Crear Then
                e.Item.Cells(dgcolumns.Accion).Text = "Creación"
            ElseIf e.Item.Cells(dgcolumns.Accion).Text = acciones.Eliminar Then
                e.Item.Cells(dgcolumns.Accion).Text = "Eliminación"
            ElseIf e.Item.Cells(dgcolumns.Accion).Text = acciones.Modificar Then
                e.Item.Cells(dgcolumns.Accion).Text = "Modificación"
            ElseIf e.Item.Cells(dgcolumns.Accion).Text = acciones.LogIn Then
                e.Item.Cells(dgcolumns.Accion).Text = "LogIn"
            End If
            Dim hpl As HyperLink
            hpl = e.Item.FindControl("hplPagina")
            e.Item.Cells(dgcolumns.Fecha).Text = CDate(e.Item.Cells(dgcolumns.Fecha).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(dgcolumns.Hora).Text = CDate(e.Item.Cells(dgcolumns.Hora).Text).ToString("T", New System.Globalization.CultureInfo("de-DE"))
            If Not hpl Is Nothing Then
                hpl.NavigateUrl = GeRequestApplicationPath(e.Item.Cells(dgcolumns.Pagina).Text) ' "/" & e.Item.Cells(dgcolumns.Pagina).Text
                hpl.Text = PortalCulture.GetString("00393")
            End If

            Dim hplDetail As HyperLink
            Dim idlog As Integer
            Dim bshowlink As Byte

            Byte.TryParse(e.Item.Cells(dgcolumns.showlink).Text, bshowlink)
            idlog = Integer.Parse(e.Item.Cells(dgcolumns.idLog).Text)
            hplDetail = e.Item.FindControl("lnkDetalle")
            hplDetail.NavigateUrl = GeRequestApplicationPath("/Portal/Pages/LogDetalle.aspx") & "?id=" & idlog
            hplDetail.Text = PortalCulture.GetString("00903")
            hplDetail.Visible = (bshowlink = 1)

        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Usuario).Text = PortalCulture.GetString("M000027")
            e.Item.Cells(dgcolumns.Accion).Text = PortalCulture.GetString("00425")
            e.Item.Cells(dgcolumns.Fecha).Text = PortalCulture.GetString("M000120")
            e.Item.Cells(dgcolumns.Hora).Text = PortalCulture.GetString("00426")
            e.Item.Cells(dgcolumns.Nota).Text = PortalCulture.GetString("00427")
            e.Item.Cells(dgcolumns.detalle).Text = PortalCulture.GetString("00903")
        End If
    End Sub
    Private Sub dlHoteles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlHoteles.ItemDataBound
        Dim dv As DataView
        Dim dg As DataGrid
        Dim htls As String()
        Dim tshow As HtmlTable
        Dim ds As DataSet
        ds = CType(dlHoteles.DataSource, DataSet)
        If ds.Tables.Count > 1 Then
            dv = ds.Tables("Log").DefaultView
            dv.RowFilter = "isnull(Hotel,0)" & "=" & dlHoteles.DataKeys(e.Item.ItemIndex)
            dg = e.Item.FindControl("dgLog")
            If Not dg Is Nothing Then
                dg.Visible = False
                AddHandler dg.ItemDataBound, AddressOf dglog_ItemDataBound
                tshow = e.Item.FindControl("tshow")
                If Not tshow Is Nothing Then
                    tshow.Rows(0).Cells(0).Attributes.Add("onclick", "showHotel('" & dg.ClientID & "','" & tshow.Rows(0).Cells(0).ClientID & "');")
                End If
                dg.DataSource = dv
                dg.DataBind()
                If dv.Count > 0 Then
                    dg.Visible = True
                    e.Item.Visible = False
                End If
            End If
        End If
    End Sub


End Class
