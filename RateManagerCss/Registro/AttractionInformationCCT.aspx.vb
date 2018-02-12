Imports Portal.Facade
Imports Portal.Common
Imports System.Configuration.ConfigurationManager
Imports Contenido.presentacion
Partial Public Class AttractionInformationCCT
    Inherits PaginaBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(pages.Home)
        If Not IsPostBack Then
            'Dim ds As DataTable
            'With New PresentacionOpciones
            'ds = .GetIdModulebyName("InfoPropiedad")
            'If Not ds Is Nothing AndAlso ds.Rows.Count = 1 Then
            AreaAttraction1.IdModulo = 17 'ds.Rows(0)("IdModulo")
            AreaAttraction1.IdEmpresa = Me.cInfoActual.Empresa
            AreaAttraction1.ModeView = Opciones.ViewMode.Edit
            AreaAttraction1.IdIdioma = PortalCulture.GetIDCulture
            AreaAttraction1.idDelHotel = Me.cInfoActual.Hotel
            'End If
            'End With
        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lbltitle.Text = PortalCulture.GetString("A00717")
    End Sub

End Class