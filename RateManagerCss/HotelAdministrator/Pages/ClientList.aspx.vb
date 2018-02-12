Imports Portal.Hotel.Facade

Partial Public Class ClientList
    Inherits PaginaBase

    Protected Enum Columns
        Id = 0
        Email = 1
        Nombre = 2
        Telefono = 3
        Domicilio = 4
        CodigoPostal = 5
        Ciudad = 6
        Compañia = 7
        Promotions = 8
    End Enum

    Protected ReadOnly Property CurrentUser() As Integer
        Get
            Return If(Me.IsSupervisor, 0, Me.UserIdentityName)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            'If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

            If Not MyBase.IsSupervisor AndAlso Not MyBase.isUserChain Then
                MyBase.redirectTo(PaginaBase.pages.IsDefaulter)
            Else
                Me.CargaPortalesDe(Me.CurrentUser)
            End If


            Me.txtDesde.maxYear = Today.Year
            Me.txtDesde.selectedDate = Today.Subtract(New TimeSpan(30, 0, 0, 0))
            Me.txtHasta.maxYear = Today.Year
            Me.txtHasta.selectedDate = Today
            Me.chkFechas.Checked = True

        End If
    End Sub

    Private Sub CargaPortalesDe(ByVal idUser As Integer)
        Dim controller As New PortalsFacade()
        Me.lstPortales.DataTextField = "Nombre"
        Me.lstPortales.DataValueField = "IdPortal"
        Me.lstPortales.DataSource = controller.GetByUser(idUser)
        Me.lstPortales.DataBind()
        Me.lstPortales.Items.Insert(0, New ListItem(PortalCulture.GetString("M000640"), "-1"))
        Me.lstPortales.Items.Insert(1, New ListItem(PortalCulture.GetString("00172"), "0"))
    End Sub

    Protected Function GetLabel(ByVal key As String) As String
        Return PortalCulture.GetString(key)
    End Function


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.lstClientes.Columns(Columns.Ciudad).HeaderText = PortalCulture.GetString("00254")
        Me.lstClientes.Columns(Columns.CodigoPostal).HeaderText = PortalCulture.GetString("00731")
        Me.lstClientes.Columns(Columns.Domicilio).HeaderText = PortalCulture.GetString("M0BT0000163")
        Me.lstClientes.Columns(Columns.Email).HeaderText = PortalCulture.GetString("00163")
        Me.lstClientes.Columns(Columns.Compañia).HeaderText = PortalCulture.GetString("01187")
        Me.lstClientes.Columns(Columns.Nombre).HeaderText = PortalCulture.GetString("00073")
        Me.lstClientes.Columns(Columns.Promotions).HeaderText = PortalCulture.GetString("01188")
        Me.lstClientes.Columns(Columns.Telefono).HeaderText = PortalCulture.GetString("00164")

        Me.btnBuscar.Text = PortalCulture.GetString("M000637")
        Me.btnFiltrar.Text = PortalCulture.GetString("M000637")

        Dim sFiltro As String
        sFiltro = PortalCulture.GetString("01186") & ","
        sFiltro &= If(lstPortales.SelectedIndex = 0, "", String.Format(PortalCulture.GetString("01385"), lstPortales.SelectedItem.Text))
        sFiltro &= String.Format(PortalCulture.GetString("01374"), txtDesde.selectedDate.ToString("dd/MM/yyyy"), txtHasta.selectedDate.ToString("dd/MM/yyyy"))
        sFiltro &= If(String.IsNullOrEmpty(txtNombre.Text), "", String.Format(PortalCulture.GetString("01377"), txtNombre.Text))
        sFiltro &= If(String.IsNullOrEmpty(txtEmail.Text), "", String.Format(PortalCulture.GetString(1386), txtEmail.Text))
        Me.lblFiltro.Text = sFiltro.Replace(",,", "").Replace(", ,", "")
        If Not Me.IsPostBack Then
            lstPortales.SelectedIndex = 1
            TraeDatos(Me, Nothing)
        End If

    End Sub

    Protected Sub TraeDatos(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click, btnFiltrar.Click
        Me.lstClientes.CurrentPageIndex = 0
        Me.TraeDatos()
    End Sub

    Private Sub TraeDatos()
        If Me.lstPortales.SelectedValue <> "-1" Then

            Dim controller As New CustomerFacade()

            Me.lstClientes.DataSource = controller.GetByPortal(Me.lstPortales.SelectedValue, If(Me.lstPortales.SelectedValue = 0, Me.CurrentUser, 0), If(Me.chkFechas.Checked, Me.txtDesde.selectedDate, New Date(2000, 1, 1)), If(Me.chkFechas.Checked, Me.txtHasta.selectedDate, Today), Me.txtNombre.Text.Trim(), Me.txtEmail.Text.Trim(), String.Empty)
            Me.lstClientes.DataBind()

        End If
    End Sub

    Private Sub lstClientes_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles lstClientes.PageIndexChanged
        Me.lstClientes.CurrentPageIndex = e.NewPageIndex
        Me.TraeDatos()
    End Sub

End Class