Public Partial Class HotelDineroMail
    Inherits PaginaBase
    Private msError As String = ""

    Private Sub LoadResources()
        lblTitle.Text = PortalCulture.GetString("01276")
        btnSave.Text = PortalCulture.GetString("A00153")
        cmdEliminar.Text = PortalCulture.GetString("00103")
    End Sub

    Private Sub ShowError()
        Me.lblError.Visible = (msError.Trim.Length > 0)
        Me.lblError.Text = msError
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Dim hr As Boolean
            hr = CtrRatePlanDineroMail1.LoadDineroMailHotel(MyBase.cInfoActual.Hotel)
            cmdEliminar.Enabled = hr
        End If
        Me.cmdEliminar.OnClientClick = String.Format("return confirm('{0}');", PortalCulture.GetString("01273"))
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Dim hr As Boolean = CtrRatePlanDineroMail1.SaveHotelDineroMail(MyBase.cInfoActual.Hotel)
        If hr Then
            hr = CtrRatePlanDineroMail1.LoadDineroMailHotel(MyBase.cInfoActual.Hotel)
            cmdEliminar.Enabled = hr
        Else
            Me.msError = PortalCulture.GetString("00844")
        End If
    End Sub

    Protected Sub cmdEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEliminar.Click
        Dim hr As Boolean = CtrRatePlanDineroMail1.DeleteHotelPaymentMode()
        If hr Then
            CtrRatePlanDineroMail1.ClearCtlrs()
            cmdEliminar.Enabled = False
        Else
            Me.msError = PortalCulture.GetString("01275")
        End If
    End Sub

    Private Sub HotelDineroMail_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        LoadResources()
        ShowError()
    End Sub

End Class