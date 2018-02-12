Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Xml
Imports Portal.General.Facade
Imports WSHotelDataAccess

Partial Class LogHotelNoAvailable
    Inherits PaginaBase

    Enum dgcolumns
        Fecha
        checkin
        checkout
    End Enum

    Private Property checkin() As Date
        Get
            Return viewstate("checkin")
        End Get
        Set(ByVal Value As Date)
            viewstate("checkin") = Value
        End Set
    End Property

    Private Property checkout() As Date
        Get
            Return viewstate("checkout")
        End Get
        Set(ByVal Value As Date)
            viewstate("checkout") = Value
        End Set
    End Property

    Private Property NombreHotel() As String
        Get
            Return viewstate("NombreHotel")
        End Get
        Set(ByVal Value As String)
            viewstate("NombreHotel") = Value
        End Set
    End Property

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TD1 As System.Web.UI.HtmlControls.HtmlTableCell

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

        If Not Me.IsHotelSelected Then
            Me.redirectTo(PaginaBase.pages.SearchHotel)
        End If

        If Not IsPostBack Then
            dgDetalle.Visible = False
            TDDetalle.Visible = False
            Me.txtInicio.Text = Now.Date.ToString("MM/dd/yyyy")
            Me.txtFinal.Text = Now.Date.ToString("MM/dd/yyyy")
            'ddlHoteles.DataTextField = "NombreEmpresa"
            'ddlHoteles.DataValueField = "ID"
            ' Dim data As DataSet, dv As DataView
            'If Me.IsSupervisor Then
            'data = getCompanys()
            'dv = data.Tables(0).DefaultView
            'dv.RowFilter = "ID is not null and ID<>0"
            'Me.ddlHoteles.DataSource = dv
            'Me.ddlHoteles.DataBind()
            'Me.ddlHoteles.Items.Insert(0, "All")
            'Me.ddlHoteles.Items(0).Value = 0
            'Else
            'Dim reader As SqlDataReader
            'With New HotelSistema
            '    reader = .GetHotelsCompanyByUserId(Usuario)
            'End With
            'While reader.Read
            '    Dim item As New ListItem(reader.Item("Nombre"), reader.Item("idHotel"))
            '    ddlHoteles.Items.Add(item)
            'End While
        End If
        'End If
        CargaRecursos()
        btnSearch.OnClientClick = "return FireUpdateStatus();"
    End Sub

    Private Function LeerlogNoAvail(ByVal f1 As DateTime, ByVal f2 As DateTime, ByVal nombre As String) As DataSet
        Dim ds As DataSet
        'With New clsDAHOC
        '    'Return .Gf1, f2, idhotel)
        '    If idhotel = 0 Then
        '        ds = .GetAvailNoFound(f1, f2)
        '    Else
        '        ds = .GetAvailNoFound(idhotel, f1, f2)
        '    End If
        'End With
        With New clsHotelNotAvailable
            ds = .getResumenNoAvailablity(f1, f2, nombre)
        End With
        Return ds
    End Function

    Private Sub loaddatos()
        'Dim name As String
        Dim TblHoteles As String = "Hoteles"
        Dim datos As DataSet

        dgDetalle.Visible = False
        TDDetalle.Visible = False

        Me.checkin = CDate(txtInicio.Text)
        Me.checkout = CDate(txtFinal.Text)
        Me.NombreHotel = txtNombreHotel.Text

        datos = LeerlogNoAvail(Me.checkin, Me.checkout, txtNombreHotel.Text)
        'If Me.ddlHoteles.SelectedItem.Value = 0 Then
        '    Dim dr As DataRow
        '    dr = datos.Tables(0).NewRow()
        '    dr("idhotel") = 0
        '    dr("nombre") = PortalCulture.GetString("00172")
        '    datos.Tables(0).Rows.Add(dr)
        'End If
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)

        dgSolicitudes.DataKeyField = "idhotel"
        dgSolicitudes.DataSource = datos
        dgSolicitudes.DataBind()

        'Me.dlHoteles.DataKeyField = "idhotel"
        'Me.dlHoteles.DataMember = datos.Tables("hoteles").TableName
        'Me.dlHoteles.DataSource = datos
        'Me.dlHoteles.DataBind()
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        loaddatos()
    End Sub

    'Private Function getCompanys() As DataSet
    '    Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
    '    Dim command As New SqlCommand("spCompanySearchCompanys", conection)
    '    With command
    '        .CommandType = CommandType.StoredProcedure
    '        .Parameters.Add(New SqlParameter("@idRubro", AppSettings("idRubro")))
    '    End With
    '    Dim adapter As New SqlDataAdapter(command)
    '    Dim dRes As New DataSet
    '    adapter.Fill(dRes)
    '    Return dRes
    'End Function
    Private Sub CargaRecursos()
        Me.lblInicio.Text = PortalCulture.GetString("00108", True)
        Me.lblFinal.Text = PortalCulture.GetString("00109", True)
        btnSearch.Text = PortalCulture.GetString("M0BT0000115")
        lblTitle.Text = PortalCulture.GetString("01466")

        dgSolicitudes.Columns(0).HeaderText = PortalCulture.GetString("00668") '"Nombre Hotel"
        dgSolicitudes.Columns(1).HeaderText = PortalCulture.GetString("00669")
        CType(dgSolicitudes.Columns(2), ButtonColumn).Text = PortalCulture.GetString("00376")


        dgDetalle.Columns(0).HeaderText = PortalCulture.GetString("M000120")
        dgDetalle.Columns(1).HeaderText = PortalCulture.GetString("M000122")
        dgDetalle.Columns(2).HeaderText = PortalCulture.GetString("M000123")
        dgDetalle.Columns(3).HeaderText = PortalCulture.GetString("00670")

    End Sub

    'Private Sub dglog_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)

    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

    '        Dim hpl As HyperLink
    '        hpl = e.Item.FindControl("hplPagina")
    '        e.Item.Cells(dgcolumns.Fecha).Text = CDate(e.Item.Cells(dgcolumns.Fecha).Text).ToString("MMM/dd/yyyy")
    '        e.Item.Cells(dgcolumns.checkin).Text = CDate(e.Item.Cells(dgcolumns.checkin).Text).ToString("MMM/dd/yyyy")
    '        e.Item.Cells(dgcolumns.checkout).Text = CDate(e.Item.Cells(dgcolumns.checkout).Text).ToString("MMM/dd/yyyy")

    '    ElseIf e.Item.ItemType = ListItemType.Header Then
    '        'e.Item.Cells(dgcolumns.Usuario).Text = PortalCulture.GetString("M000027")
    '        'e.Item.Cells(dgcolumns.Accion).Text = PortalCulture.GetString("00425")
    '        e.Item.Cells(dgcolumns.Fecha).Text = PortalCulture.GetString("M000120")
    '        'e.Item.Cells(dgcolumns.Hora).Text = PortalCulture.GetString("00426")
    '        'e.Item.Cells(dgcolumns.Nota).Text = PortalCulture.GetString("00427")
    '    End If
    'End Sub

    'Private Sub dlHoteles_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
    '    Dim dv As DataView
    '    Dim dg As DataGrid
    '    Dim htls As String()
    '    Dim tshow As HtmlTable
    '    Dim ds As DataSet
    '    ds = CType(dlHoteles.DataSource, DataSet)
    '    If ds.Tables.Count > 1 Then
    '        dv = ds.Tables(1).DefaultView
    '        dv.RowFilter = "isnull(idhotel,0)" & "=" & dlHoteles.DataKeys(e.Item.ItemIndex)
    '        dg = e.Item.FindControl("dgLog")
    '        If Not dg Is Nothing Then
    '            dg.Visible = False
    '            AddHandler dg.ItemDataBound, AddressOf dglog_ItemDataBound
    '            tshow = e.Item.FindControl("tshow")
    '            If Not tshow Is Nothing Then
    '                tshow.Rows(0).Cells(0).Attributes.Add("onclick", "showHotel('" & dg.ClientID & "','" & tshow.Rows(0).Cells(0).ClientID & "');")
    '            End If
    '            dg.DataSource = dv
    '            dg.DataBind()
    '            If dv.Count > 0 Then
    '                dg.Visible = True
    '                e.Item.Visible = False
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub dgSolicitudes_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSolicitudes.ItemCommand
        If e.CommandName = "Detalle" Then
            CargaDetalle(dgSolicitudes.DataKeys(e.Item.ItemIndex), Me.checkin, Me.checkout)
            lblTitleDetalle.Text = e.Item.Cells(0).Text & " - " & PortalCulture.GetString("00671") '"Detalle De Solicitudes No Disponible"
        End If
    End Sub
    Private Sub CargaDetalle(ByVal idhotel As Integer, ByVal f1 As Date, ByVal f2 As Date)
        Dim ds As DataSet
        dgDetalle.Visible = True
        TDDetalle.Visible = True
        With New clsHotelNotAvailable
            ds = .getDetalleNoAvailablity(f1, f2, idhotel)
        End With
        dgDetalle.DataSource = ds
        dgDetalle.DataBind()
    End Sub
End Class


Public Class clsHotelNotAvailable
    Public Function getResumenNoAvailablity(ByVal f1 As Date, ByVal f2 As Date, ByVal namehotel As String) As DataSet
        Dim ds As New DataSet
        Dim da As SqlDataAdapter = New SqlDataAdapter("", New SqlConnection(AppSettings("HotelConnectionString")))
        Try

            da.SelectCommand.CommandText = "spHotelNotAvailableGetResumen"
            da.SelectCommand.CommandType = CommandType.StoredProcedure
            da.SelectCommand.Parameters.AddWithValue("@f1", f1)
            da.SelectCommand.Parameters.AddWithValue("@f2", f2)
            da.SelectCommand.Parameters.AddWithValue("@nameHotel", namehotel)
            da.Fill(ds)

        Catch ex As Exception

        End Try
        Return ds
    End Function


    Public Function getDetalleNoAvailablity(ByVal f1 As Date, ByVal f2 As Date, ByVal idhotel As Integer) As DataSet
        Dim ds As New DataSet
        Dim da As SqlDataAdapter = New SqlDataAdapter("", New SqlConnection(AppSettings("HotelConnectionString")))
        Try

            da.SelectCommand.CommandText = "spHotelNotAvailableGetDetalle"
            da.SelectCommand.CommandType = CommandType.StoredProcedure
            da.SelectCommand.Parameters.AddWithValue("@f1", f1)
            da.SelectCommand.Parameters.AddWithValue("@f2", f2)
            da.SelectCommand.Parameters.AddWithValue("@idHotel", idhotel)
            da.Fill(ds)

        Catch ex As Exception

        End Try
        Return ds
    End Function


End Class