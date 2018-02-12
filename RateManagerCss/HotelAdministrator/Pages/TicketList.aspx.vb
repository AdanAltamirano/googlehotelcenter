Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data

Partial Public Class TicketList
    Inherits PaginaBase

    Protected Function GetLabel(ByVal key As String) As String
        Return PortalCulture.GetString(key)
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not Me.IsPostBack Then
            Me.DataBind()
        End If
    End Sub

    Public Overrides Sub DataBind()
        Me.lstTickets.CurrentPageIndex = 0
        Me.lstTickets.DataSource = Me.GetData()
        Me.lstTickets.DataBind()
    End Sub

    Private Function GetData() As List(Of TicketData)
        Dim controller As New TicketFacade()

        Dim id As Integer = 0
        Integer.TryParse(Me.txtReferencia.Text, id)
        Dim status As Integer = 0
        If Me.chkInProcess.Checked Then status += 1
        If Me.chkClosed.Checked Then status += 2

        Return controller.GetList(Me.cInfoActual.Hotel, Me.txtSubject.Text, id, status)
    End Function

    Private Sub lstTickets_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles lstTickets.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = Me.GetLabel("01235")
            e.Item.Cells(1).Text = Me.GetLabel("M000120")
            e.Item.Cells(2).Text = Me.GetLabel("01286")
            e.Item.Cells(3).Text = Me.GetLabel("01296")
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Me.DataBind()
    End Sub

    Private Sub lstTickets_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles lstTickets.PageIndexChanged

        Dim data As List(Of TicketData) = Me.GetData()
        If data IsNot Nothing Then

            Dim pages As Integer = data.Count / Me.lstTickets.PageSize
            If data.Count Mod Me.lstTickets.PageSize > 0 Then pages += 1
            If e.NewPageIndex > (pages - 1) Then
                Me.lstTickets.CurrentPageIndex = pages - 1
            Else
                Me.lstTickets.CurrentPageIndex = e.NewPageIndex
            End If

            Me.lstTickets.DataSource = Me.GetData()
            Me.lstTickets.DataBind()

        End If

    End Sub

End Class