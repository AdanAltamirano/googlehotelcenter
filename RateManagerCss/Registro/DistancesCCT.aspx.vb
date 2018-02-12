Imports Portal.Facade
Imports Portal.Common
Imports System.Configuration.ConfigurationManager
Imports Contenido.presentacion
Partial Public Class DistancesCCT
    Inherits PaginaBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(pages.Home)
        If Not IsPostBack Then
            
            CtrlDistanciasCCT1.IdEmpresa = Me.cInfoActual.Empresa
            CtrlDistanciasCCT1.ModeView = Opciones.ViewMode.Edit
            CtrlDistanciasCCT1.IdIdioma = PortalCulture.GetIDCulture
            CtrlDistanciasCCT1.idDelHotel = Me.cInfoActual.Hotel
        End If
           
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lbltitle.Text = PortalCulture.GetString("A00752")
    End Sub
End Class