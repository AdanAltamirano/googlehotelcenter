Option Explicit On

Imports Oz.UniBilling.Hotels.DataAccess
Imports Oz.UniBilling.Hotels.Business

Partial Public Class PaymentMethod
    Inherits PaginaBase
    Dim errorInt As Integer
    Enum dgcolumns
        idCompanyMethod
        Method
        Default_
        AccountNumber
        Editar
        Eliminar
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        CtrlPaymentMethod1.Company = MyBase.cInfoActual.Empresa





    End Sub
    Private Sub LoadPaymentMethods()
        Dim dt As PaymentMethodDataSet.CompanyPaymentMethod_GeByCompanyIDDataTable
        Dim dv As DataView

        dt = New Oz.UniBilling.Hotels.Business.PaymentMethod().SelectByCompanyID(MyBase.cInfoActual.Empresa, PortalCulture.GetIDCulture())

        With grid
            .DataSource = dt
            .DataBind()
        End With

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        errorInt = CtrlPaymentMethod1.SavePaymentMethod()
        cmdNew.Style("display") = "block"
    End Sub

    Protected Sub grid_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grid.ItemCommand
        If e.CommandName = "Edit" Then
            cmdNew.Style("display") = "none"
            errorInt = CtrlPaymentMethod1.LoadPaymentMethod(grid.DataKeys(e.Item.ItemIndex), True)
        ElseIf e.CommandName = "Delete" Then
            errorInt = CtrlPaymentMethod1.DeletPaymentMethod(grid.DataKeys(e.Item.ItemIndex))
            cmdNew.Style("display") = "block"
        End If


    End Sub

    Private Sub PaymentMethod_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        LoadPaymentMethods()
        LoadResources()
    End Sub

    Private Sub grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            
            Dim LK As HyperLink
            Dim LK2 As LinkButton

            LK2 = e.Item.Cells(dgcolumns.Editar).FindControl("lnkEdit")
            If Not LK2 Is Nothing Then LK2.Text = PortalCulture.GetString("00093")

            LK = e.Item.Cells(dgcolumns.Eliminar).FindControl("lnkDelete")
            LK2 = e.Item.Cells(dgcolumns.Eliminar).FindControl("lnkDelete2")


            If Not LK Is Nothing Then
                LK.Text = PortalCulture.GetString("00103")
                LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("01549"), PortalCulture.GetString("01550"))
            End If




        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Method).Text = PortalCulture.GetString("01535")
            e.Item.Cells(dgcolumns.Default_).Text = PortalCulture.GetString("01534")
            e.Item.Cells(dgcolumns.AccountNumber).Text = PortalCulture.GetString("01533")
            e.Item.Cells(dgcolumns.Editar).Text = PortalCulture.GetString("00065")
            e.Item.Cells(dgcolumns.Eliminar).Text = PortalCulture.GetString("00103")
        End If
    End Sub

    Private Sub LoadResources()
        cmdNew.Value = PortalCulture.GetString("00102")
        Me.btncancel.Text = PortalCulture.GetString("00009")
        Me.btnSave.Text = PortalCulture.GetString("00008")
        grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        grid.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"

        Me.lblTitle.Text = PortalCulture.GetString("01530")

        If errorInt = -4 Then
            lblError.Text = PortalCulture.GetString("01275")
            lblError.Visible = True
        ElseIf errorInt < 0 Then
            lblError.Text = PortalCulture.GetString("01536")
            lblError.Visible = True
        Else
            lblError.Visible = False
        End If

        If CtrlPaymentMethod1.IsEdit Then
            lblMsg.Text = PortalCulture.GetString("01548")
        Else
            lblMsg.Text = PortalCulture.GetString("01547")
        End If

    End Sub
End Class