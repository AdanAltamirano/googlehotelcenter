Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Partial Class ctrlActividades

    Inherits System.Web.UI.UserControl

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

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
    Public Property IdIdioma() As Long
        Get
            Return viewstate("IdIdioma")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdIdioma") = Value
        End Set
    End Property

    Public Property IdEmpresa() As Long
        Get
            Return viewstate("IdEmpresa")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdEmpresa") = Value
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

    Private Sub CargoContenido()
        If Me.IdIdioma > 0 AndAlso Me.IdEmpresa > 0 Then
            CargaServicios()
            CargaServiciosEmp()
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        lnkEditar.Attributes.Add("onClick", "javascript:document.getElementById('serviciosedit').style.display='block';document.getElementById('servicios').style.display='none';onResizeIframe();return false;")
    End Sub

    Private Sub CargaServiciosEmp()
        Try
            Dim dt As DataTable
            With New DataCtrlActividades
                dt = .LoadServicios(Me.IdEmpresa, Me.IdIdioma)
            End With
            If Not IsNothing(dt) Then
                dt.DefaultView.RowFilter = "IsNull(idEmpresa,-1)<>-1"
                dlstServicios.DataSource = dt.DefaultView
            Else
                dlstServicios.DataSource = Nothing
            End If
            dlstServicios.DataBind()
        Catch e As Exception
            Throw New Exception("datalista databind:" & e.ToString)
        Finally
        End Try
    End Sub

    Private Sub CargaServicios()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Long
        With New DataCtrlActividades
            dt = .LoadServicios(Me.IdEmpresa, Me.IdIdioma)
        End With
        chkServicios.DataSource = dt
        chkServicios.DataTextField = "texto"
        chkServicios.DataValueField = DataCtrlActividades.CodigoActividad_Field
        chkServicios.DataBind()
        For i = 0 To chkServicios.Items.Count - 1
            dr = dt.Rows(i)
            chkServicios.Items(i).Selected = False
            If Not dr.IsNull(DataCtrlActividades.IdEmpresa_Field) Then
                chkServicios.Items(i).Selected = True
            End If
        Next
    End Sub

    Public Sub GuardaServicios()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Long
        Dim sw As Boolean
        With New DataCtrlActividades
            dt = .CreaTabla
            For i = 0 To chkServicios.Items.Count - 1
                If chkServicios.Items(i).Selected Then
                    dr = dt.NewRow
                    dr.Item(.CodigoActividad_Field) = chkServicios.Items(i).Value
                    dr.Item(.IdEmpresa_Field) = Me.IdEmpresa
                    dt.Rows.Add(dr)
                End If
            Next
            If Not .InsertActividades(dt, Me.IdEmpresa) Then
                lblMensaje.Text = "no se pudo guardar politicas de la empresa"
            End If
            CargoContenido()
        End With
    End Sub

    Private Sub lnkGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkGuardar.Click
        GuardaServicios()
        CType(Me.Page, PaginaBase).guardalog(Me.Pagina, PaginaBase.acciones.Modificar, Nota)
    End Sub



    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lnkEditar.Text = PortalCulture.GetString("A00694")
        lnkGuardar.Text = PortalCulture.GetString("A00695")
    End Sub
End Class

Public Class DataCtrlActividades
    Const IdIdioma_Parm = "@IdIdioma"
    Const IdEmpresa_Parm = "@IdEmpresa"
    Const CodigoActividad_Parm = "@CodigoActividad"


    Public Const IdEmpresa_Field = "IdEmpresa"
    Public Const CodigoActividad_Field = "CodigoActividad"
    Public Const Descripcion_Field = "Descripcion"
    Public Const IdDiccionario_Field = "IdDiccionario"

    Public Const Servicios_Tabla = "CodigosActividad"
    Public Const EmpresaServicios_Tabla = "Empresas_Servicios"

    Private Enum TypeCommand
        AllCategory = 0
    End Enum

    Public Sub New()
    End Sub

    Public Function CreaTabla() As DataTable
        Dim table As DataTable = New DataTable(EmpresaServicios_Tabla)
        With table.Columns
            .Add(CodigoActividad_Field, GetType(System.String))
            .Add(IdEmpresa_Field, GetType(System.Int32))
        End With
        Return table
    End Function

    Private Function GetLoadCommand(ByVal p As TypeCommand) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        Select Case p
            Case TypeCommand.AllCategory
                procedure = "spEmpresasServiciosGetAll"
                loadcommand = New SqlCommand(procedure, New SqlConnection(AppSettings("PortalConectionString")))
                loadcommand.CommandType = CommandType.StoredProcedure
                loadcommand.Parameters.Add(New SqlParameter(IdIdioma_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
        End Select
        GetLoadCommand = loadcommand
    End Function

    Private Function GetInsertCommand(ByVal sqlconn As SqlConnection) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        procedure = "spEmpresasServiciosCreate"
        loadcommand = New SqlCommand(procedure, sqlconn)
        loadcommand.CommandType = CommandType.StoredProcedure
        loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
        loadcommand.Parameters.Add(New SqlParameter(CodigoActividad_Parm, SqlDbType.NVarChar, 12))
        loadcommand.Parameters(IdEmpresa_Parm).SourceColumn = IdEmpresa_Field
        loadcommand.Parameters(CodigoActividad_Parm).SourceColumn = CodigoActividad_Field
        GetInsertCommand = loadcommand
    End Function

    Private Function GetDeleteCommand(ByVal sqlconn As SqlConnection) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        procedure = "spEmpresasServiciosDelete"
        loadcommand = New SqlCommand(procedure, sqlconn)
        loadcommand.CommandType = CommandType.StoredProcedure
        loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
        loadcommand.Parameters(IdEmpresa_Parm).SourceColumn = IdEmpresa_Field
        GetDeleteCommand = loadcommand
    End Function

    Public Function InsertActividades(ByVal dt As DataTable, ByVal idempresa As Long) As Boolean
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
                .Update(ds, Me.EmpresaServicios_Tabla)
                tr.Commit()
            Catch e As Exception
                tr.Rollback()
                Return False
            Finally
                sqlconn.Close()
            End Try
        End With
        InsertActividades = True
    End Function

    Public Function LoadServicios(ByVal idempresa As Long, ByVal idioma As Long) As DataTable
        Dim data As New DataTable
        Dim dsCommand As SqlDataAdapter = New SqlDataAdapter
        With dsCommand
            Try
                .SelectCommand = GetLoadCommand(TypeCommand.AllCategory)
                .SelectCommand.Parameters(IdIdioma_Parm).Value = idioma
                .SelectCommand.Parameters(IdEmpresa_Parm).Value = idempresa
                .Fill(data)
            Catch e As Exception
            Finally
                If .SelectCommand.Connection.State = ConnectionState.Open Or .SelectCommand.Connection.State = ConnectionState.Broken Then
                    .SelectCommand.Connection.Close()
                End If
            End Try
        End With
        LoadServicios = data
    End Function
End Class
