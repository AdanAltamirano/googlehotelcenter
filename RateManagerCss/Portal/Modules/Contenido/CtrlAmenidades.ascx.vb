Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Partial Class CtrlAmenidades
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
            'CargoContenido()
        End Set
    End Property
    Public Property Idrubro() As Long
        Get
            Return viewstate("Idrubro")
        End Get
        Set(ByVal Value As Long)
            viewstate("Idrubro") = Value
            'CargoContenido()
        End Set
    End Property
    Public Property IdEmpresa() As Long
        Get
            Return viewstate("IdEmpresa")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdEmpresa") = Value
            ' CargoContenido()
        End Set
    End Property
    Public Property ModeV() As Opciones.ViewMode
        Get
            Return viewstate("ModeV")
        End Get
        Set(ByVal Value As Opciones.ViewMode)
            viewstate("ModeV") = Value
            '   CargoContenido()
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
    Public Property Nota() As String
        Get
            Return viewstate("_nota")
        End Get
        Set(ByVal Value As String)
            viewstate("_nota") = Value
        End Set
    End Property

    Public Property IdHotelCCT() As Long
        Get
            Return ViewState("IdHotelCCT")
        End Get
        Set(ByVal Value As Long)
            ViewState("IdHotelCCT") = Value
        End Set
    End Property

    Public Property CategoriaCCT() As Long
        Get
            Return ViewState("CategoriaCCT")
        End Get
        Set(ByVal Value As Long)
            ViewState("CategoriaCCT") = Value
        End Set
    End Property

    Public Property ISCCTContent() As Long
        Get
            Return ViewState("ISCCTContent")
        End Get
        Set(ByVal Value As Long)
            ViewState("ISCCTContent") = Value
        End Set
    End Property

    Public Sub CargoContenido()
        If Me.Idrubro > 0 AndAlso Me.IdIdioma > 0 AndAlso Me.IdEmpresa > 0 AndAlso Not ISCCTContent Then
            'Cargamos las amenidades de la empresa
            loadAmenitiesByCompany()
            If Me.ModeV = Opciones.ViewMode.Edit Then
                'Cargamos todas la amenidades del portal
                loadAmenities()
                'If Me.dlAmenities.Items.Count > 0 Then
                'Me.dlEditAmenities.Visible = False

                'lnkEditar.Visible = True
                'lnkGuardar.Visible = False
                'lnkGuardar2.Visible = False
                'Else
                '    Me.dlAmenities.Visible = False
                '    Me.dlEditAmenities.Visible = True
                '    lnkEditar.Visible = False
                '    lnkGuardar.Visible = True
                '    lnkGuardar2.Visible = True
                'End If

            Else
                Me.dlEditAmenities.Visible = False
                lnkEditar.Visible = False
                lnkGuardar.Visible = False
                lnkGuardar2.Visible = False
            End If
        ElseIf ISCCTContent Then
            'Cargamos las amenidades del hotel para call center
            loadAmenitiesByHotelCCT()
            If Me.ModeV = Opciones.ViewMode.Edit Then
                'Cargamos todas la amenidades del portal
                loadAmenitiesCCT()
            Else
                Me.dlEditAmenities.Visible = False
                lnkEditar.Visible = False
                lnkGuardar.Visible = False
                lnkGuardar2.Visible = False
            End If
        End If
    End Sub


    Private Sub loadAmenitiesByCompany()
        Dim dt As DataTable
        With New DataCtrlAmenidades
            dt = .LoadEmpresasAmenEmp(Me.IdEmpresa, Me.IdIdioma, Me.Idrubro)
        End With
        dlAmenities.DataKeyField = DataCtrlAmenidades.IdAmenidad_Field
        dlAmenities.DataSource = dt
        dlAmenities.DataBind()

    End Sub

    Private Function loadAmenities()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Long
        With New DataCtrlAmenidades
            dt = .LoadEmpresasAmen(Me.IdEmpresa, Me.IdIdioma, Me.Idrubro)
        End With
        dlEditAmenities.DataSource = dt
        dlEditAmenities.DataKeyField = DataCtrlAmenidades.IdAmenidad_Field
        dlEditAmenities.DataBind()
        For i = 0 To dlEditAmenities.Items.Count - 1
            dr = dt.Rows(i)
            Dim chk As System.Web.UI.WebControls.CheckBox
            chk = dlEditAmenities.Items(i).FindControl("Check")
            If Not chk Is Nothing Then
                chk.Text = dr.Item(DataCtrlAmenidades.Texto_Field)
                If Not dr.IsNull(DataCtrlAmenidades.IdEmpresa_Field) Then
                    chk.Checked = True
                    chk.CssClass = "clsdarklabel"
                Else
                    chk.Checked = False
                End If
            End If
        Next

    End Function


    'Private Sub CargaAmenidades()
    '    Dim dt As DataTable
    '    Dim dr As DataRow
    '    Dim i As Long
    '    With New DataCtrlAmenidades
    '        dt = .LoadEmpresasAmen(Me.IdEmpresa, Me.IdIdioma, Me.Idrubro)
    '    End With
    '    chkAmenities.DataSource = dt
    '    chkAmenities.DataTextField = DataCtrlAmenidades.Descripcion_Field
    '    chkAmenities.DataValueField = DataCtrlAmenidades.IdAmenidad_Field
    '    chkAmenities.DataBind()
    '    For i = 0 To chkAmenities.Items.Count - 1
    '        dr = dt.Rows(i)
    '        chkAmenities.Items(i).Selected = False
    '        If Not dr.IsNull(DataCtrlAmenidades.IdEmpresa_Field) Then
    '            chkAmenities.Items(i).Selected = True
    '        End If
    '    Next
    'End Sub

    Public Sub GuardaAmenidades()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Long
        Dim sw As Boolean = False

        With New DataCtrlAmenidades
            dt = .CreaTabla
            For i = 0 To dlEditAmenities.Items.Count - 1
                Dim chk As System.Web.UI.WebControls.CheckBox
                chk = dlEditAmenities.Items(i).FindControl("Check")
                If Not chk Is Nothing Then
                    If chk.Checked = True Then
                        dr = dt.NewRow
                        dr.Item(.IdAmenidad_Field) = dlEditAmenities.DataKeys(i)
                        dr.Item(.IdEmpresa_Field) = Me.IdEmpresa
                        dt.Rows.Add(dr)
                        sw = True
                    End If
                End If
            Next
            If Not .InsertAmenidades(dt, Me.IdEmpresa) Then
                lblMensaje.Text = "no se pudo guardar amenidades de la empresa"
            End If
            CargoContenido()
        End With
    End Sub

    Private Sub lnkGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkGuardar.Click, lnkGuardar2.Click
        If Not ISCCTContent Then
            Call GuardaAmenidades()
            Call loadAmenitiesByCompany()
        Else
            Call GuardaAmenidadesCCT()
            Call loadAmenitiesByHotelCCT()
        End If
        CType(Me.Page, PaginaBase).guardalog(Me.Pagina, PaginaBase.acciones.Modificar, Nota)
    End Sub


    Private Sub lnkEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call CargoContenido()
        dlEditAmenities.Visible = True
        dlAmenities.Visible = False
        lnkEditar.Visible = False
        lnkGuardar.Visible = True
        lnkGuardar2.Visible = True
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        lnkEditar.Attributes.Add("onClick", "javascript:document.getElementById('amenidadesedit').style.display='block';document.getElementById('amenidades').style.display='none';onResizeIframe();return false;")
        Hyperlink1.Attributes.Add("onClick", "javascript:document.getElementById('amenidadesedit').style.display='none';document.getElementById('amenidades').style.display='block';return false;")
        Hyperlink2.Attributes.Add("onClick", "javascript:document.getElementById('amenidadesedit').style.display='none';document.getElementById('amenidades').style.display='block';return false;")
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lnkEditar.Text = PortalCulture.GetString("A00686")
        lnkGuardar.Text = PortalCulture.GetString("A00687")
        lnkGuardar2.Text = lnkGuardar.Text

        Hyperlink1.Text = PortalCulture.GetString("M000436")
        Hyperlink2.Text = Hyperlink1.Text
        If lblMensaje.Text.Trim <> "" Then
            lblMensaje.Visible = True
        Else
            lblMensaje.Visible = False
        End If
        Call CargoContenido()
    End Sub

    Private Sub loadAmenitiesByHotelCCT()
        Dim dt As DataTable
        With New DataCtrlAmenidades
            dt = .LoadHotelAmenHtl(Me.IdHotelCCT, Me.IdIdioma, Me.Idrubro)
        End With
        dlAmenities.DataKeyField = DataCtrlAmenidades.IdAmenidad_Field
        dlAmenities.DataSource = dt
        dlAmenities.DataBind()

    End Sub

    Public Sub GuardaAmenidadesCCT()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Long
        Dim sw As Boolean = False

        With New DataCtrlAmenidades
            dt = .CreaTablaCCT
            For i = 0 To dlEditAmenities.Items.Count - 1
                Dim chk As System.Web.UI.WebControls.CheckBox
                chk = dlEditAmenities.Items(i).FindControl("Check")
                If Not chk Is Nothing Then
                    If chk.Checked = True Then
                        dr = dt.NewRow
                        dr.Item(.IdAmenidad_Field) = dlEditAmenities.DataKeys(i)
                        dr.Item(.IdHotel_Field) = Me.IdHotelCCT
                        dt.Rows.Add(dr)
                        sw = True
                    End If
                End If
            Next
            If Not .InsertAmenidadesToCCL(dt, Me.IdHotelCCT) Then
                lblMensaje.Text = "no se pudo guardar amenidades de la empresa"
            End If
            CargoContenido()
        End With
    End Sub

    Private Function loadAmenitiesCCT()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Long
        With New DataCtrlAmenidades
            dt = .LoadHotelesAmenCCT(Me.IdHotelCCT, Me.IdIdioma, Me.Idrubro)
        End With
        dlEditAmenities.DataSource = dt
        dlEditAmenities.DataKeyField = DataCtrlAmenidades.IdAmenidad_Field
        dlEditAmenities.DataBind()
        For i = 0 To dlEditAmenities.Items.Count - 1
            dr = dt.Rows(i)
            Dim chk As System.Web.UI.WebControls.CheckBox
            chk = dlEditAmenities.Items(i).FindControl("Check")
            If Not chk Is Nothing Then
                chk.Text = dr.Item(DataCtrlAmenidades.Texto_Field)
                If Not dr.IsNull(DataCtrlAmenidades.IdHotel_Field) Then
                    chk.Checked = True
                    chk.CssClass = "clsdarklabel"
                Else
                    chk.Checked = False
                End If
            End If
        Next

    End Function

    '  Private Sub dlAmenities_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlAmenities.ItemDataBound
    '      Dim img As System.Web.UI.WebControls.Image
    '      If e.Item.ItemIndex >= -0 Then
    '          img = e.Item.FindControl("Image")
    '          img.Visible = True
    '          If Not img Is Nothing Then
    '              If Not IO.File.Exists(img.ImageUrl) Then
    '                  img.Visible = False
    '              End If
    '          End If
    '      End If
    '  End Sub
End Class

Public Class DataCtrlAmenidades
    Const IdIdioma_Parm = "@IdIdioma"
    Const IdEmpresa_Parm = "@IdEmpresa"
    Const IdRubro_Parm = "@IdRubro"
    Const IdAmenidad_Parm = "@IdAmenidad"
    Const IdHotel_Parm = "@IdHotel"

    Public Const IdRubro_Field = "IdRubro"
    Public Const IdEmpresa_Field = "IdEmpresa"
    Public Const IdAmenidad_Field = "IdAmenidad"
    Public Const IdHotel_Field = "idHotel"
    Public Const Descripcion_Field = "Descripcion"
    Public Const Texto_Field = "Texto"
    Public Const IdDiccionario_Field = "IdDiccionario"
    Public Const Amidades_Tabla = "Amenidades"
    Public Const EmpresaAmenidades_Tabla = "Empresas_Amenidades"
    Public Const ContenidoAmenidadesCCT_Tabla = "Contenido_AmenidadesCCT"
    Private Enum loadcommad
        AmenRubro = 0
        AmenByEmp = 1
        AmenByEmpAmen = 2
        AmenByHotAmen = 3
        AmenByHotel = 4
    End Enum
    Public Sub New()
    End Sub

    Public Function CreaTabla() As DataTable
        Dim table As DataTable = New DataTable(EmpresaAmenidades_Tabla)
        With table.Columns
            .Add(IdAmenidad_Field, GetType(System.Int32))
            .Add(IdEmpresa_Field, GetType(System.Int32))
        End With
        Return table
    End Function

    Public Function CreaTablaCCT() As DataTable
        Dim table As DataTable = New DataTable(ContenidoAmenidadesCCT_Tabla)
        With table.Columns
            .Add(IdAmenidad_Field, GetType(System.Int32))
            .Add(IdHotel_Field, GetType(System.Int32))
        End With
        Return table
    End Function

    Private Function GetLoadCommand(ByVal p As loadcommad) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        Select Case p
            Case loadcommad.AmenRubro
                procedure = "AmenidadesGetByRubro"
                loadcommand = New SqlCommand(procedure, New SqlConnection(AppSettings("PortalConectionString")))
                loadcommand.CommandType = CommandType.StoredProcedure
                loadcommand.Parameters.Add(New SqlParameter(IdIdioma_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdRubro_Parm, SqlDbType.Int))
            Case loadcommad.AmenByEmp
                procedure = "AmenidadesGetByEmp"
                loadcommand = New SqlCommand(procedure, New SqlConnection(AppSettings("PortalConectionString")))
                loadcommand.CommandType = CommandType.StoredProcedure
                loadcommand.Parameters.Add(New SqlParameter(IdIdioma_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdRubro_Parm, SqlDbType.Int))
            Case loadcommad.AmenByEmpAmen
                procedure = "AmenidadesGetByEmpAmen"
                loadcommand = New SqlCommand(procedure, New SqlConnection(AppSettings("PortalConectionString")))
                loadcommand.CommandType = CommandType.StoredProcedure
                loadcommand.Parameters.Add(New SqlParameter(IdIdioma_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdRubro_Parm, SqlDbType.Int))
            Case loadcommad.AmenByHotAmen
                procedure = "spGetAmenidadesToCallCenter"
                loadcommand = New SqlCommand(procedure, New SqlConnection(AppSettings("HotelConnectionString")))
                loadcommand.CommandType = CommandType.StoredProcedure
                loadcommand.Parameters.Add(New SqlParameter(IdIdioma_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdHotel_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdRubro_Parm, SqlDbType.Int))
            Case loadcommad.AmenByHotel
                procedure = "spGetAmenidadesToCallCenterByHotel"
                loadcommand = New SqlCommand(procedure, New SqlConnection(AppSettings("HotelConnectionString")))
                loadcommand.CommandType = CommandType.StoredProcedure
                loadcommand.Parameters.Add(New SqlParameter(IdIdioma_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdHotel_Parm, SqlDbType.Int))
                loadcommand.Parameters.Add(New SqlParameter(IdRubro_Parm, SqlDbType.Int))
        End Select
        GetLoadCommand = loadcommand
    End Function

    Private Function GetInsertCommand(ByVal sqlconn As SqlConnection) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        procedure = "AmenidadesEmpresaCreate"
        loadcommand = New SqlCommand(procedure, sqlconn)
        loadcommand.CommandType = CommandType.StoredProcedure
        loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
        loadcommand.Parameters.Add(New SqlParameter(IdAmenidad_Parm, SqlDbType.Int))
        loadcommand.Parameters(IdEmpresa_Parm).SourceColumn = IdEmpresa_Field
        loadcommand.Parameters(IdAmenidad_Parm).SourceColumn = IdAmenidad_Field
        GetInsertCommand = loadcommand
    End Function

    Private Function GetDeleteCommand(ByVal sqlconn As SqlConnection) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        procedure = "AmenidadesEmpresaDelete"
        loadcommand = New SqlCommand(procedure, sqlconn)
        loadcommand.CommandType = CommandType.StoredProcedure
        loadcommand.Parameters.Add(New SqlParameter(IdEmpresa_Parm, SqlDbType.Int))
        loadcommand.Parameters(IdEmpresa_Parm).SourceColumn = IdEmpresa_Field
        GetDeleteCommand = loadcommand
    End Function

    Public Function InsertAmenidades(ByVal dt As DataTable, ByVal idempresa As Long) As Boolean
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
                .Update(ds, Me.EmpresaAmenidades_Tabla)
                tr.Commit()
            Catch e As Exception
                tr.Rollback()
                sqlconn.Close()
                Return False
            Finally
                If sqlconn.State = ConnectionState.Open Or sqlconn.State = ConnectionState.Open Then
                    sqlconn.Close()
                End If
            End Try
        End With
        InsertAmenidades = True
    End Function

    Public Function LoadAmenidades(ByVal idioma As Long, ByVal idrubro As Long) As DataTable
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
                If .SelectCommand.Connection.State = ConnectionState.Open Or .SelectCommand.Connection.State = ConnectionState.Broken Then
                    .SelectCommand.Connection.Close()
                End If
            End Try
        End With
        LoadAmenidades = data
    End Function

    Public Function LoadEmpresasAmen(ByVal idempresa As Long, ByVal ididioma As Long, ByVal idrubro As Long) As DataTable
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
            Finally
                If .SelectCommand.Connection.State = ConnectionState.Open Or .SelectCommand.Connection.State = ConnectionState.Broken Then
                    .SelectCommand.Connection.Close()
                End If
            End Try
        End With
        LoadEmpresasAmen = data
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

            Finally
                If .SelectCommand.Connection.State = ConnectionState.Open Or .SelectCommand.Connection.State = ConnectionState.Broken Then
                    .SelectCommand.Connection.Close()
                End If
            End Try
        End With
        LoadEmpresasAmenEmp = data
    End Function

    Public Function LoadHotelAmenHtl(ByVal idHotel As Long, ByVal ididioma As Long, ByVal idrubro As Long) As DataTable
        Dim data As New DataTable
        Dim dsCommand As SqlDataAdapter = New SqlDataAdapter
        With dsCommand
            Try
                .SelectCommand = GetLoadCommand(loadcommad.AmenByHotAmen)
                .SelectCommand.Parameters(IdIdioma_Parm).Value = ididioma
                .SelectCommand.Parameters(IdHotel_Parm).Value = idHotel
                .SelectCommand.Parameters(IdRubro_Parm).Value = idrubro
                .Fill(data)
            Catch e As Exception

            Finally
                If .SelectCommand.Connection.State = ConnectionState.Open Or .SelectCommand.Connection.State = ConnectionState.Broken Then
                    .SelectCommand.Connection.Close()
                End If
            End Try
        End With
        LoadHotelAmenHtl = data
    End Function

    Public Function InsertAmenidadesToCCL(ByVal dt As DataTable, ByVal idhotel As Long) As Boolean
        Dim dsCommand As SqlDataAdapter = New SqlDataAdapter
        Dim tr As SqlTransaction
        Dim sqlconn As SqlConnection = New SqlConnection(AppSettings("HotelConnectionString"))
        Dim ds As DataSet = New DataSet
        With dsCommand
            Try
                sqlconn.Open()
                tr = sqlconn.BeginTransaction
                .DeleteCommand = GetDeleteCommandCCT(sqlconn)
                .DeleteCommand.Transaction = tr
                .DeleteCommand.Parameters(IdHotel_Parm).Value = idhotel
                .DeleteCommand.ExecuteNonQuery()
                .InsertCommand = GetInsertCommandCCT(sqlconn)
                .InsertCommand.Transaction = tr
                ds.Tables.Add(dt)
                .Update(ds, Me.ContenidoAmenidadesCCT_Tabla)
                tr.Commit()
            Catch e As Exception
                tr.Rollback()
                sqlconn.Close()
                Return False
            Finally
                If sqlconn.State = ConnectionState.Open Or sqlconn.State = ConnectionState.Open Then
                    sqlconn.Close()
                End If
            End Try
        End With
        InsertAmenidadesToCCL = True
    End Function

    Private Function GetInsertCommandCCT(ByVal sqlconn As SqlConnection) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        procedure = "spInsertAmenidadesToCallCenter"
        loadcommand = New SqlCommand(procedure, sqlconn)
        loadcommand.CommandType = CommandType.StoredProcedure
        loadcommand.Parameters.Add(New SqlParameter(IdHotel_Parm, SqlDbType.Int))
        loadcommand.Parameters.Add(New SqlParameter(IdAmenidad_Parm, SqlDbType.Int))
        loadcommand.Parameters(IdHotel_Parm).SourceColumn = IdHotel_Field
        loadcommand.Parameters(IdAmenidad_Parm).SourceColumn = IdAmenidad_Field
        GetInsertCommandCCT = loadcommand
    End Function

    Private Function GetDeleteCommandCCT(ByVal sqlconn As SqlConnection) As SqlCommand
        Dim procedure As String
        Dim loadcommand As SqlCommand
        procedure = "spDeleteAmenidadesToCallCenter"
        loadcommand = New SqlCommand(procedure, sqlconn)
        loadcommand.CommandType = CommandType.StoredProcedure
        loadcommand.Parameters.Add(New SqlParameter(IdHotel_Parm, SqlDbType.Int))
        loadcommand.Parameters(IdHotel_Parm).SourceColumn = IdHotel_Field
        GetDeleteCommandCCT = loadcommand
    End Function

    Public Function LoadHotelesAmenCCT(ByVal idhotel As Long, ByVal ididioma As Long, ByVal idrubro As Long) As DataTable
        Dim data As New DataTable
        Dim dsCommand As SqlDataAdapter = New SqlDataAdapter
        With dsCommand
            Try
                .SelectCommand = GetLoadCommand(loadcommad.AmenByHotel)
                .SelectCommand.Parameters(IdIdioma_Parm).Value = ididioma
                .SelectCommand.Parameters(IdHotel_Parm).Value = idhotel
                .SelectCommand.Parameters(IdRubro_Parm).Value = idrubro
                .Fill(data)
            Catch e As Exception
            Finally
                If .SelectCommand.Connection.State = ConnectionState.Open Or .SelectCommand.Connection.State = ConnectionState.Broken Then
                    .SelectCommand.Connection.Close()
                End If
            End Try
        End With
        LoadHotelesAmenCCT = data
    End Function
End Class
