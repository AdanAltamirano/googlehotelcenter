Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Partial Class CtrlPoliticas
    Inherits UserControl

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblAmen As System.Web.UI.WebControls.Label

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Public Property IdIdioma() As Long
        Get
            Return viewstate("IdIdioma")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdIdioma") = Value
            CargoContenido()
        End Set
    End Property
    Public Property Idrubro() As Long
        Get
            Return viewstate("Idrubro")
        End Get
        Set(ByVal Value As Long)
            viewstate("Idrubro") = Value
            CargoContenido()
        End Set
    End Property
    Public Property IdEmpresa() As Long
        Get
            Return viewstate("IdEmpresa")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdEmpresa") = Value
            CargoContenido()
        End Set
    End Property
    Public Property ModeV() As Opciones.ViewMode
        Get
            Return viewstate("ModeV")
        End Get
        Set(ByVal Value As Opciones.ViewMode)
            viewstate("ModeV") = Value
            CargoContenido()
        End Set
    End Property
    Public Property Nota() As String
        Get
            Return viewstate("_nota")
        End Get
        Set(ByVal Value As String)
            viewstate("_nota") = Value
        End Set
    End Property
    Public Property Pagina() As String
        Get
            Return viewstate("_pagina")
        End Get
        Set(ByVal Value As String)
            viewstate("_pagina") = Value
        End Set
    End Property

    Private Sub CargoContenido()
        If Me.Idrubro > 0 AndAlso Me.IdIdioma > 0 AndAlso Me.IdEmpresa > 0 Then
            CargaPoliticasEmp()
            dlstPoliticas.Visible = True
            If Me.ModeV = Opciones.ViewMode.Edit Then
                CargaPoliticas()
                chkPoliticas.Visible = False

                lnkEditar.Visible = True
                If dlstPoliticas.Items.Count = 0 Then
                    lnkEditar.Visible = False
                    lnkGuardar.Visible = True
                    chkPoliticas.Visible = True
                    dlstPoliticas.Visible = False
                End If
            End If
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
    End Sub

    Private Sub CargaPoliticasEmp()
        Dim dt As DataTable
        With New DataCtrlPoliticas
            dt = .LoadEmpresasAmenEmp(Me.IdEmpresa, Me.IdIdioma, Me.Idrubro)
        End With
        dlstPoliticas.DataSource = dt
        dlstPoliticas.DataBind()
    End Sub

    Private Sub CargaPoliticas()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Long
        With New DataCtrlPoliticas
            dt = .LoadEmpresasPol(Me.IdEmpresa, Me.IdIdioma, Me.Idrubro)
        End With
        chkPoliticas.DataSource = dt
        chkPoliticas.DataTextField = DataCtrlPoliticas.Descripcion_Field
        chkPoliticas.DataValueField = DataCtrlPoliticas.Idpolitica_Field
        chkPoliticas.DataBind()
        For i = 0 To chkPoliticas.Items.Count - 1
            dr = dt.Rows(i)
            chkPoliticas.Items(i).Selected = False
            If Not dr.IsNull(DataCtrlPoliticas.IdEmpresa_Field) Then
                chkPoliticas.Items(i).Selected = True
            End If
        Next
    End Sub

    Public Sub GuardaPoliticas()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Long
        'Dim sw As Boolean
        With New DataCtrlPoliticas
            dt = .CreaTabla
            For i = 0 To chkPoliticas.Items.Count - 1
                If chkPoliticas.Items(i).Selected Then
                    dr = dt.NewRow
                    dr.Item(DataCtrlPoliticas.Idpolitica_Field) = chkPoliticas.Items(i).Value
                    dr.Item(DataCtrlPoliticas.IdEmpresa_Field) = Me.IdEmpresa
                    dt.Rows.Add(dr)
                End If
            Next
            If Not .InsertPoliticas(dt, Me.IdEmpresa) Then
                lblMensaje.Text = "no se pudo guardar politicas de la empresa"
            End If
            CargoContenido()
        End With
    End Sub

    Private Sub lnkGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkGuardar.Click
        chkPoliticas.Visible = False
        dlstPoliticas.Visible = True
        lnkEditar.Visible = True
        lnkGuardar.Visible = False
        GuardaPoliticas()
        CType(Me.Page, PaginaBase).guardalog(Pagina, PaginaBase.acciones.Modificar, Nota)
    End Sub

    Private Sub lnkEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkEditar.Click
        chkPoliticas.Visible = True
        dlstPoliticas.Visible = False
        lnkEditar.Visible = False
        lnkGuardar.Visible = True
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lnkEditar.Text = PortalCulture.GetString("A00697")
        lnkGuardar.Text = PortalCulture.GetString("A00698")
    End Sub
End Class

Public Class DataCtrlPoliticas
    Const IdIdioma_Parm As String = "@IdIdioma"
    Const IdEmpresa_Parm As String = "@IdEmpresa"
    Const IdRubro_Parm As String = "@IdRubro"
    Const Idpolitica_Parm As String = "@Idpolitica"

    Public Const IdRubro_Field As String = "IdRubro"
    Public Const IdEmpresa_Field As String = "IdEmpresa"
    Public Const Idpolitica_Field As String = "Idpolitica"
    Public Const Descripcion_Field As String = "Descripcion"
    Public Const IdDiccionario_Field As String = "IdRubro"
    Public Const Amidades_Tabla As String = "Politicas"
    Public Const EmpresaPoliticas_Tabla As String = "Empresas_Politicas"
    Private Enum loadcommad
        AmenRubro = 0
        AmenByEmp = 1
        AmenByEmpAmen = 2
    End Enum

    Public Sub New()
    End Sub

    Public Function CreaTabla() As DataTable
        Dim table As DataTable = New DataTable(EmpresaPoliticas_Tabla)
        With table.Columns
            .Add(Idpolitica_Field, GetType(System.Int32))
            .Add(IdEmpresa_Field, GetType(System.Int32))
        End With
        Return table
    End Function

    Private Function GetLoadCommand(ByVal p As loadcommad) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        Select Case p
            Case loadcommad.AmenRubro
                procedure = "spPoliticasGetByRubro"
                loadcommand = New SqlCommand(procedure, New SqlConnection(AppSettings("PortalConectionString")))
                loadcommand.CommandType = CommandType.StoredProcedure
                loadcommand.Parameters.Add(New SqlParameter(IdIdioma_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdRubro_Parm, SqlDbType.Int))
            Case loadcommad.AmenByEmp
                procedure = "spPoliticasGetByEmp"
                loadcommand = New SqlCommand(procedure, New SqlConnection(AppSettings("PortalConectionString")))
                loadcommand.CommandType = CommandType.StoredProcedure
                loadcommand.Parameters.Add(New SqlParameter(IdIdioma_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdRubro_Parm, SqlDbType.Int))
            Case loadcommad.AmenByEmpAmen
                procedure = "spPoliticasGetByEmpAmen"
                loadcommand = New SqlCommand(procedure, New SqlConnection(AppSettings("PortalConectionString")))
                loadcommand.CommandType = CommandType.StoredProcedure
                loadcommand.Parameters.Add(New SqlParameter(IdIdioma_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdRubro_Parm, SqlDbType.Int))
        End Select
        GetLoadCommand = loadcommand
    End Function

    Private Function GetInsertCommand(ByVal sqlconn As SqlConnection) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        procedure = "spPoliticasEmpresaCreate"
        loadcommand = New SqlCommand(procedure, sqlconn)
        loadcommand.CommandType = CommandType.StoredProcedure
        loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
        loadcommand.Parameters.Add(New SqlParameter(Idpolitica_Parm, SqlDbType.Int))
        loadcommand.Parameters(IdEmpresa_Parm).SourceColumn = IdEmpresa_Field
        loadcommand.Parameters(Idpolitica_Parm).SourceColumn = Idpolitica_Field
        GetInsertCommand = loadcommand
    End Function

    Private Function GetDeleteCommand(ByVal sqlconn As SqlConnection) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        procedure = "spPoliticasEmpresaDelete"
        loadcommand = New SqlCommand(procedure, sqlconn)
        loadcommand.CommandType = CommandType.StoredProcedure
        loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
        loadcommand.Parameters(IdEmpresa_Parm).SourceColumn = IdEmpresa_Field
        GetDeleteCommand = loadcommand
    End Function

    Public Function InsertPoliticas(ByVal dt As DataTable, ByVal idempresa As Long) As Boolean
        Dim dsCommand As SqlDataAdapter = New SqlDataAdapter
        Dim tr As SqlTransaction
        Dim sqlconn As SqlConnection = New SqlConnection(AppSettings("PortalConectionString"))
        Dim ds As DataSet = New DataSet
        With dsCommand
            Try
                sqlconn.Open()
                tr = sqlconn.BeginTransaction
                .DeleteCommand = GetDeleteCommand(sqlconn)
                .DeleteCommand.Transaction = tr
                .DeleteCommand.Parameters(IdEmpresa_Parm).Value = idempresa
                .DeleteCommand.ExecuteNonQuery()
                .InsertCommand = GetInsertCommand(sqlconn)
                .InsertCommand.Transaction = tr
                ds.Tables.Add(dt)
                .Update(ds, EmpresaPoliticas_Tabla)
                tr.Commit()
            Catch e As Exception
                tr.Rollback()
                Return False
            Finally
                sqlconn.Close()
            End Try
        End With
        InsertPoliticas = True
    End Function

    Public Function LoadPoliticas(ByVal idioma As Long, ByVal idrubro As Long) As DataTable
        Dim data As New DataTable
        Dim dsCommand As SqlDataAdapter = New SqlDataAdapter
        With dsCommand
            Try
                .SelectCommand = GetLoadCommand(loadcommad.AmenRubro)
                .SelectCommand.Parameters(IdIdioma_Parm).Value = idioma
                .SelectCommand.Parameters(IdRubro_Parm).Value = idrubro
                .Fill(data)
            Catch e As Exception
            Finally

            End Try
        End With
        LoadPoliticas = data
    End Function

    Public Function LoadEmpresasPol(ByVal idempresa As Long, ByVal ididioma As Long, ByVal idrubro As Long) As DataTable
        Dim data As New DataTable
        Dim dsCommand As SqlDataAdapter = New SqlDataAdapter
        With dsCommand
            Try
                .SelectCommand = GetLoadCommand(loadcommad.AmenByEmp)
                .SelectCommand.Parameters(IdIdioma_Parm).Value = ididioma
                .SelectCommand.Parameters(IdEmpresa_Parm).Value = idempresa
                .SelectCommand.Parameters(IdRubro_Parm).Value = idrubro
                .Fill(data)
            Catch e As Exception
                Return data
            Finally
            End Try
        End With
        Return data
    End Function

    Public Function LoadEmpresasAmenEmp(ByVal idempresa As Long, ByVal ididioma As Long, ByVal idrubro As Long) As DataTable
        Dim data As New DataTable
        Dim dsCommand As SqlDataAdapter = New SqlDataAdapter
        With dsCommand
            Try
                .SelectCommand = GetLoadCommand(loadcommad.AmenByEmpAmen)
                .SelectCommand.Parameters(IdIdioma_Parm).Value = ididioma
                .SelectCommand.Parameters(IdEmpresa_Parm).Value = idempresa
                .SelectCommand.Parameters(IdRubro_Parm).Value = idrubro
                .Fill(data)
            Catch e As Exception
                Return data
            Finally
            End Try
        End With
        LoadEmpresasAmenEmp = data
    End Function
End Class
