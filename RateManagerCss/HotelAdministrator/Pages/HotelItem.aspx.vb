
Imports Portal.General.Common.Data
Imports Portal.General.Facade

Partial Public Class HotelItem
    Inherits PaginaBase

    Dim errorInt As Integer
    Enum dgcolumns
        idHotelItem
        Name
        Price
        Edit
        Delet
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        ctrlHotelItem1.idHotel = MyBase.cInfoActual.Hotel
        ctrlHotelItem1.idCompany = MyBase.cInfoActual.Empresa
        Dim dsHotel As HotelDatos
        Dim dsEtiq As MonedaDatos

        With New HotelSistema
            dsHotel = .GetHotelById(MyBase.cInfoActual.Hotel)
        End With
        With dsHotel
            If dsHotel.Tables(.HOTEL_TABLE).Rows.Count > 0 Then
                ctrlHotelItem1.IDMoneda = .Tables(.HOTEL_TABLE)(0)(.FIELD_IDMONEDA)
                ctrlHotelItem1.Moneda = .Tables(.HOTEL_TABLE)(0)("Codigo")
            Else
                ctrlHotelItem1.Visible = False
            End If
        End With

        If Not IsPostBack Then
            If Request.QueryString("idTypeHotelItem") IsNot Nothing Then
                cmdNew.Style("display") = "none"
                ctrlHotelItem1.LoadHotelItem(CType(Request.QueryString("idTypeHotelItem"), Integer), True)
            End If
        End If

    End Sub

    Private Sub LoadHotelItems()
        'Dim dt As PaymentMethodDataSet.CompanyPaymentMethod_GeByCompanyIDDataTable
        Dim ds As HotelItemData

        ds = New Portal.General.Facade.HotelItemFacade().GetHotelItemByIDHotel(MyBase.cInfoActual.Hotel)

        With grid
            .DataSource = ds.Tables(ds.HOTELITEM_TABLE)
            .DataBind()
        End With

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        errorInt = ctrlHotelItem1.SaveHotelItem(MyBase.cInfoActual.Hotel)
        cmdNew.Style("display") = "block"
    End Sub

    Protected Sub grid_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grid.ItemCommand

        If e.CommandName = "Edit" Then
            cmdNew.Style("display") = "none"
            errorInt = ctrlHotelItem1.LoadHotelItem(grid.DataKeys(e.Item.ItemIndex), True)
        ElseIf e.CommandName = "Delete" Then
            errorInt = ctrlHotelItem1.DeletHotelItem(grid.DataKeys(e.Item.ItemIndex))
            cmdNew.Style("display") = "block"
        End If
    End Sub

    Private Sub PaymentMethod_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        LoadHotelItems()
        LoadResources()
    End Sub

    Private Sub grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then

            Dim LK As HyperLink
            Dim LK2 As LinkButton

            LK2 = e.Item.Cells(dgcolumns.Edit).FindControl("lnkEdit")
            If Not LK2 Is Nothing Then LK2.Text = PortalCulture.GetString("00093")

            LK = e.Item.Cells(dgcolumns.Delet).FindControl("lnkDelete")
            LK2 = e.Item.Cells(dgcolumns.Delet).FindControl("lnkDelete2")


            If Not LK Is Nothing Then
                LK.Text = PortalCulture.GetString("00103")
                LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("01576"), PortalCulture.GetString("01577"))
            End If




        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Name).Text = PortalCulture.GetString("00073")
            e.Item.Cells(dgcolumns.Price).Text = PortalCulture.GetString("M000648")

        End If
    End Sub

    Private Sub LoadResources()
        cmdNew.Value = PortalCulture.GetString("00102")
        Me.btncancel.Text = PortalCulture.GetString("00009")
        Me.btnSave.Text = PortalCulture.GetString("00008")
        grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        grid.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"

        Me.lblTitle.Text = PortalCulture.GetString("01576")

        If errorInt = -4 Then
            lblError.Text = PortalCulture.GetString("01275")
            lblError.Visible = True
        ElseIf errorInt < 0 Then
            lblError.Text = PortalCulture.GetString("01575")
            lblError.Visible = True
        Else
            lblError.Visible = False
        End If

        If ctrlHotelItem1.IsEdit Then
            lblMsg.Text = PortalCulture.GetString("01573")
        Else
            lblMsg.Text = PortalCulture.GetString("01574")
        End If

    End Sub

End Class