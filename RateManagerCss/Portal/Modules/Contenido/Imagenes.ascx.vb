Imports Albums
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Configuration.ConfigurationManager

Partial Class Imagenes
    Inherits System.Web.UI.UserControl

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents OpenImg As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

#Region "propiedades"
    Public ReadOnly Property IdRubro() As Long
        Get
            Return AppSettings("idRubro")
        End Get
    End Property

    Public Property IdEmpresa() As Long
        Get
            Return ViewState("IdEmpresa")
        End Get
        Set(ByVal Value As Long)
            ViewState("IdEmpresa") = Value
            MyClass.RefrestPage()
        End Set
    End Property
    Public Property Nota() As String
        Get
            Return ViewState("_nota")
        End Get
        Set(ByVal Value As String)
            ViewState("_nota") = Value
        End Set
    End Property
    Public Property Pagina() As String
        Get
            Return ViewState("_pagina")
        End Get
        Set(ByVal Value As String)
            ViewState("_pagina") = Value
        End Set
    End Property

    Public Property IsImgHeader() As Boolean
        Get
            Return ViewState("_IsImgHeader")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("_IsImgHeader") = Value
        End Set
    End Property

#End Region


    'This event return the image selected
    Public Event GetImgPath(ByVal strUrl As String)
    Public Event HideIMGComponent(ByVal Visible As Boolean)

    Private showdlg As Boolean

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        showdlg = False
    End Sub
    'This function Loads all imgs record using the Albums.Facade
    Public Sub RefrestPage()
        With New Albums.Facade.BusinessFacade
            Dim dr As SqlDataReader
            dr = .GetImgsByRequest(Me.IdRubro, Me.IdEmpresa, Me.IsImgHeader)
            With datalistimagenes
                .DataSource = dr
                .DataBind()
            End With
        End With
    End Sub

    Private Sub BtnImgAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnImgAdd.Click
        'Çreando las reglas que validarán
        With New Albums.Facade.BusinessFacade
            If .VerifyValidImg(FileAbreImagen, Request) Then
                Try
                    'Envia el Stream, el request (opcional), IdRubro,IdEmpresa

                    'AddedImage SE MANDA PARA OBTENER LA DIRECCION POR REFERENCIA 
                    Dim AddedImage As String = ""
                    If .CreateImg(FileAbreImagen, AppSettings("Albums_Dir").ToString.ToUpper.Replace("ALBUMS", ""), IdRubro, IdEmpresa, AddedImage, Me.IsImgHeader) Then
                        CType(Me.Page, PaginaBase).guardalog(Me.Pagina, PaginaBase.acciones.Crear, "Se agregó la imagen " & AppSettings("Albums_url").ToString.ToUpper.Replace("ALBUMS", "") & AddedImage & ". ")
                        MyClass.RefrestPage()
                        RaiseEvent GetImgPath(AddedImage)
                        LblError.Text = ""
                    Else
                        Exit Sub
                    End If
                Catch ex As UnauthorizedAccessException
                    Response.Write("<b>No se han especificado los derechos de subdirectorios!!</b>")
                Catch ex As OutOfMemoryException
                    Response.Write("<b>No se puede cargar la imagen.</b>")
                Catch ex As Exception
                    Response.Write("<b>Hay un problema con los directorios!!</b>")
                End Try
            Else
                LblError.Text = PortalCulture.GetString("A00671")   '"La imagen no es valida ó su dimencion no es de 500x400"
                LblError.Visible = True
                showdlg = True
            End If

        End With
    End Sub

    Private Sub DataListImagenes_DeleteCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles datalistimagenes.DeleteCommand
        With New Albums.Facade.BusinessFacade
            Try
                'TODO: cambio imagenes
                .DeleteImg(e.CommandArgument, AppSettings("Albums_Dir").ToString.ToUpper.Replace("ALBUMS", ""), IdRubro, IdEmpresa, Me.IsImgHeader)
                CType(Me.Page, PaginaBase).guardalog(Me.Pagina, PaginaBase.acciones.Modificar, "Se eliminó la imagen " & AppSettings("Albums_url").ToString & "/" & IdRubro & "/" & IdEmpresa & "/" & e.CommandArgument & ". " & Nota)

                MyClass.RefrestPage()

            Catch ex As Exception
                Response.Write("<b>Error,no puedo borrar este elemento.</b>")
            End Try
        End With
    End Sub

    Private Sub DataListImagenes_EditCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles datalistimagenes.EditCommand
        LblError.Text = ""
        With e
            Dim str As String
            If .CommandArgument.ToString.IndexOf("?") > 0 Then
                str = .CommandArgument.ToString.Substring(0, .CommandArgument.ToString.IndexOf("?"))
            Else
                str = .CommandArgument
            End If
            RaiseEvent GetImgPath(str)
        End With
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        Me.LblTit.Text = PortalCulture.GetString("A00672")
        Me.lblAgregar.Text = PortalCulture.GetString("A00673")

        LblTitulo.Text = PortalCulture.GetString("A00668")
        LblDesc.Text = PortalCulture.GetString("A00669")

        Me.BtnImgAdd.Text = PortalCulture.GetString("A00673")
        Me.BtnCancelar.Value = PortalCulture.GetString("A00143")
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        If showdlg Then
            writer.Write("<script> document.getElementById('DivOpenImgFile').style.display='block'; </script> ")
        Else
            writer.Write("<script> document.getElementById('DivOpenImgFile').style.display='none'; </script> ")
        End If
    End Sub

    Private Sub ImgBtnHide_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnHide.Click
        LblError.Text = ""
        RaiseEvent HideIMGComponent(False)
    End Sub

    Public Function CargaImagen(ByVal spath As String, ByVal rubro As String, ByVal empresa As String, ByVal id As String, ByVal tipo As String) As String
        Dim sUrl As String
        If Not IsImgHeader Then
            sUrl = String.Concat(spath, "/", rubro, "/", empresa, "/", id, tipo, "?", DateTime.Now.ToString)
        Else
            sUrl = String.Concat(spath, "/", rubro, "/", empresa, "/", "HDR", "/", id, "?", DateTime.Now.ToString)
        End If
        Return (sUrl)
    End Function

    Public Function Resize(ByVal isResize As Boolean, ByVal arg1 As String) As String
        If IsImgHeader And isResize Then
            Return (String.Format("width: {0};", arg1))
        End If
    End Function


End Class

