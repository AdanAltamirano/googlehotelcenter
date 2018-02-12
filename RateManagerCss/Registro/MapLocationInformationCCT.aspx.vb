Imports Portal.Facade
Imports Portal.Common
Imports System.Configuration.ConfigurationManager
Imports Contenido.presentacion
Partial Public Class MapLocationInformationCCT
    Inherits PaginaBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(pages.Home)
        If Not IsPostBack Then
            'Dim ds As DataTable
            'With New PresentacionOpciones
            'ds = .GetIdModulebyName("InfoPropiedad")
            'If Not ds Is Nothing AndAlso ds.Rows.Count = 1 Then
            MapLocationCCT1.IdModulo = 17 'ds.Rows(0)("IdModulo")
            MapLocationCCT1.IdEmpresa = Me.cInfoActual.Empresa
            MapLocationCCT1.ModeView = Opciones.ViewMode.Edit
            MapLocationCCT1.IdIdioma = PortalCulture.GetIDCulture
            MapLocationCCT1.idDelHotel = Me.cInfoActual.Hotel
            'End If
            'End With
        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lbltitle.Text = PortalCulture.GetString("A00720")
    End Sub

End Class