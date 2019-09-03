Imports System.Web.Security
Imports System.Configuration.ConfigurationManager
Imports System.Text
Imports System.Xml
Imports System.Security
Imports LoginAuthenticate

Partial Public Class ctrlMenuTap
    Inherits UserControlBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        loadMenu()
    End Sub



    Public Sub loadMenu()
        ctrlMenuUl1.LoadMenu("")
        If TypeOf Me.Page Is PaginaBase Then
            Dim pb As PaginaBase = CType(Me.Page, PaginaBase)
            If pb.IsUsuarioHotel Then
                PersonalizaMenu(pb.creaMenuUsuarioHotel())
            Else
                'CargaMenu()
                CargaMenuXmlData()
            End If
        Else
            CargaMenuXmlData()
        End If
    End Sub


    Private Function CargaMenuXmlData()
        Dim cMenu As New clsMenu
        Dim doc As New XmlDocument
        Dim roleListArray As String()
        Dim menuXml As String

        If (New AuthUser).IsAuthenticated Then
            roleListArray = (New AuthUser).UserInfoArray
            '// Agrega los roles del usuario.
            HttpContext.Current.User = New Principal.GenericPrincipal(HttpContext.Current.User.Identity, roleListArray)

            menuXml = cMenu.GetMenu(AppSettings("idSistema"), (New AuthUser).IdIdiomaMenu, "sys_deals")
            If menuXml <> String.Empty Then

                ctrlMenuUl1.CurrentRoles = ""
                For Each rol As String In roleListArray
                    ctrlMenuUl1.CurrentRoles += rol.ToUpper + ","
                Next
                ctrlMenuUl1.CurrentMenuType = ctrlMenuUl.MenuType.SoloPrimerNivel
                ctrlMenuUl1.DefaultTarget = "frmPrincipal"
                ctrlMenuUl1.LoadMenu(menuXml.Replace("//", "/"))
            End If
        End If
    End Function

    Public Sub PersonalizaMenu(ByVal xml As String)
        Dim doc As New XmlDocument

        Dim roleListArray As String()

        ctrlMenuUl1.CurrentRoles = ""
        roleListArray = (New AuthUser).UserInfoArray
        ctrlMenuUl1.CurrentRoles = ""
        For Each rol As String In roleListArray
            ctrlMenuUl1.CurrentRoles += rol.ToUpper + ","
        Next
        ctrlMenuUl1.CurrentMenuType = ctrlMenuUl.MenuType.SoloPrimerNivel
        ctrlMenuUl1.DefaultTarget = "frmPrincipal"
        ctrlMenuUl1.LoadMenu(xml)
    End Sub

End Class