Imports Portal.General.Common.Data
Imports Portal.General.Facade
Partial Class TravelAgentCommision
    Inherits PaginaBase

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
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            loadData()
        End If
        Me.txtMonto.Attributes.Add("onchange", "javascript:SelectRadio('" & Me.RbdAmount.ClientID & "','" & Me.RdbPercet.ClientID & "','" & Me.txtMonto.ClientID & "')")
        Me.txtPorciento.Attributes.Add("onchange", "javascript:SelectRadio('" & Me.RdbPercet.ClientID & "','" & Me.RbdAmount.ClientID & "','" & Me.txtPorciento.ClientID & "')")
    End Sub

    Private Sub loadData()
        Dim ds As HotelDatos
        Me.RbdAmount.Checked = True
        Me.RdbPercet.Checked = False
        'lblmonedahotel
        With New HotelSistema
            ds = .AgencyCommisionConfigurationGet(Me.cInfoActual.Hotel)
        End With
        If Not ds.Tables(HotelDatos.HOTEL_TABLE) Is Nothing AndAlso ds.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 Then
            With ds.Tables(HotelDatos.HOTEL_TABLE).Rows(0)
                lblmonedahotel.Text = ds.Tables(HotelDatos.HOTEL_TABLE).Rows(0)("codigo")
                If Not .IsNull(HotelDatos.fld_CommisionAmount) Then
                    Me.RbdAmount.Checked = True
                    Me.RdbPercet.Checked = False
                    Me.txtMonto.Text = .Item(HotelDatos.fld_CommisionAmount)
                ElseIf Not .IsNull(HotelDatos.fld_CommisionPercentOfReservation) Then
                    Me.txtPorciento.Text = .Item(HotelDatos.fld_CommisionPercentOfReservation)
                    Me.RdbPercet.Checked = True
                    Me.RbdAmount.Checked = True
                End If
            End With
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        update()
    End Sub
    Private Sub update()
        Dim ds As New HotelDatos
        Dim dr As DataRow
        dr = ds.Tables(HotelDatos.HOTEL_TABLE).NewRow
        dr.Item(HotelDatos.FIELD_PKID) = Me.cInfoActual.Hotel
        If Me.RdbPercet.Checked Then
            dr.Item(HotelDatos.fld_CommisionPercentOfReservation) = txtPorciento.Text
        ElseIf Me.RbdAmount.Checked Then
            dr.Item(HotelDatos.fld_CommisionAmount) = txtMonto.Text
        End If
        ds.Tables(HotelDatos.HOTEL_TABLE).Rows.Add(dr)
        ds.Tables(HotelDatos.HOTEL_TABLE).AcceptChanges()
        ds.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_PKID) = ds.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_PKID)
        With New HotelSistema
            .AgencyCommisionConfigurationUpdate(ds)
        End With
    End Sub

    Private Sub loadCulture()
        lblTitle.InnerHtml = PortalCulture.GetString("00652")
        RbdAmount.Text = PortalCulture.GetString("00653")
        RdbPercet.Text = PortalCulture.GetString("00654")
        lblPorciento.Text = PortalCulture.GetString("00655")
        lblperReservation.Text = PortalCulture.GetString("00656")
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadCulture()
    End Sub
End Class

