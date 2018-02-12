Partial Class TarjetasHotel
    Inherits paginabase

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

    Private _Tarjetas As String

    Private Property IdHotel() As Integer
        Get
            Return Viewstate("IdHotel")
        End Get
        Set(ByVal Value As Integer)
            Viewstate("IdHotel") = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Me.cInfoActual.Hotel = 0 Then Me.redirectTo(PaginaBase.pages.SearchHotel)
        If Not IsPostBack Then
            IdHotel = cInfoActual.Hotel
            Dim dTarjetas As New Portal.Hotel.Common.Data.TarjetasHotelData
            dTarjetas = (New Portal.Hotel.Facade.TarjetasHotelFacade).GetTarjetasByHotelId(IdHotel)
            For Each dr As DataRow In dTarjetas.TablaTarjetasHotel.Rows
                _Tarjetas &= dr.Item(Portal.Hotel.Common.Data.TarjetasHotelData.TableFields.FLD_Code) & ","
            Next
            CargaTarjetas()
        End If
    End Sub

    Private Sub CargaTarjetas()
        Dim ds As New DataSet
        ds.ReadXml(Server.MapPath(Request.ApplicationPath) & "\Tarjetas.xml")

        dgTarjetas.DataSource = ds.Tables("Tarjetas")
        dgTarjetas.DataBind()
    End Sub

    Private Sub dgTarjetas_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgTarjetas.ItemDataBound
        Dim cb As New CheckBox
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            cb = e.Item.Cells(0).FindControl("cbTarjeta")
            If InStr(_Tarjetas, e.Item.Cells(1).Text) Then
                cb.Checked = True
            End If
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (New Portal.Hotel.Facade.TarjetasHotelFacade).DeleteTarjetasByHotelId(IdHotel) Then
            For Each j As DataGridItem In dgTarjetas.Items
                If j.ItemType = ListItemType.AlternatingItem Or j.ItemType = ListItemType.Item Then
                    Dim cb As CheckBox = j.Cells(0).FindControl("cbTarjeta")
                    If cb.Checked Then
                        With New Portal.Hotel.Facade.TarjetasHotelFacade
                            Dim cTar As New Portal.Hotel.Common.Data.TarjetasHotelData
                            Dim dr As DataRow = cTar.Tables(Portal.Hotel.Common.Data.TarjetasHotelData.Tabla_TarjetasHotel).NewRow
                            dr.Item(Portal.Hotel.Common.Data.TarjetasHotelData.TableFields.ID_HOTEL) = IdHotel
                            dr.Item(Portal.Hotel.Common.Data.TarjetasHotelData.TableFields.FLD_Code) = j.Cells(1).Text
                            cTar.Tables(Portal.Hotel.Common.Data.TarjetasHotelData.Tabla_TarjetasHotel).Rows.Add(dr)
                            .InsertTarjeta(cTar)
                        End With
                    End If
                End If
            Next
        Else
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitulo.Text = PortalCulture.GetString("M000603")
        btnSave.Text = PortalCulture.GetString("M000060")
    End Sub

    Private Sub dgTarjetas_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgTarjetas.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(1).Text = PortalCulture.GetString("M000432")
            e.Item.Cells(2).Text = PortalCulture.GetString("M000152")
        End If
    End Sub
End Class
