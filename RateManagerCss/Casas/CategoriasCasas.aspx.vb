Imports System.Data.SqlClient

Public Class CategoriasCasas
    Inherits System.Web.UI.Page


    Dim cat_name_es As String
    Dim cat_name_en As String
    Dim amen_name_es As String
    Dim amen_name_en As String
    Dim id_cat As Integer
    Dim id_dic As Integer
    Dim id_amen_cat As Integer
    Dim id_amen As Integer

    Public idDic As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ctrlIdiomaCategoria.IsMultiline = False
        ctrlIdiomaCategoria.MaxLength = 52
        ctrlIdiomaCategoria.Width = 400

        
        ctrlIdiomaCategoria.RequiredText = False
        'ctrlIdiomaAmen.CargaDatos(201278)

        Gobal.ctrl_idioma_cat = ctrlIdiomaCategoria

    End Sub

    Protected Sub agregar_cat_Click(sender As Object, e As EventArgs) Handles agregar_cat.Click

        cat_name_es = ctrlIdiomaCategoria.GetES
        cat_name_en = ctrlIdiomaCategoria.GetEN



        If cat_name_en = "" Or cat_name_es = "" Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Script", "errorEmptyValues();", True)
        Else
            Dim id_dic As Integer = ctrlIdiomaCategoria.Insert()

            Dim connString As String = ConfigurationManager.AppSettings("HotelConnectionString")
            Dim spSendCategory As String = "spInsertCategoriasCasas"
            Dim _error As String

            Using Conn As New SqlConnection(connString)
                Conn.Open()
                Dim command As New SqlCommand(spSendCategory, Conn)

                Try
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = cat_name_es
                    command.Parameters.Add("@IdDiccionario", SqlDbType.Int).Value = id_dic

                    command.ExecuteReader()
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Script", "successInsertValues();", True)

                Catch ex As Exception

                    _error = ex.Message
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Script", "errorDataBase();", True)

                End Try
            End Using

            ctrlIdiomaCategoria.Limpia()
        End If

    End Sub

   
    'Protected Sub btn_actualizar_Click(sender As Object, e As EventArgs) Handles btn_actualizar.Click

    '    id_amen = id_amen_d.Value
    '    id_dic = id_dic_d.Value
    '    id_amen_cat = Hidden1.Value

    '    Dim connString As String = ConfigurationManager.AppSettings("PortalConnectionString")
    '    Dim spUpdateAmenities As String = "spUpdateAmenityWithCategoryCubaHouses"
    '    Dim _error As String

    '    Using Conn As New SqlConnection(connString)
    '        Conn.Open()
    '        Dim command As New SqlCommand(spUpdateAmenities, Conn)
    '        command.CommandType = CommandType.StoredProcedure

    '        If ctrlIdiomaAmen.GetEN = "" And ctrlIdiomaAmen.GetES = "" Then

    '            Try
    '                command.Parameters.Add("@idAmenidad", SqlDbType.Int).Value = id_amen
    '                command.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = DBNull.Value
    '                command.Parameters.Add("@idCategoriaAmeniad", SqlDbType.Int).Value = id_amen_cat

    '                command.ExecuteReader()
    '                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Script", "updateSuccess();", True)

    '            Catch ex As Exception

    '                _error = ex.Message
    '                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Script", "errorDataBase();", True)

    '            End Try

    '        Else

    '            If ctrlIdiomaAmen.GetEN = "" Or ctrlIdiomaAmen.GetES = "" Then
    '                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Script", "errorEmptyValues();", True)
    '            Else
    '                amen_name_en = ctrlIdiomaAmen.GetEN
    '                amen_name_es = ctrlIdiomaAmen.GetES
    '                Dim echo As Boolean = ctrlIdiomaAmen.Update(id_dic, amen_name_en, amen_name_es)

    '                Try
    '                    command.Parameters.Add("@idAmenidad", SqlDbType.Int).Value = id_amen
    '                    command.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = amen_name_es
    '                    command.Parameters.Add("@idCategoriaAmeniad", SqlDbType.Int).Value = id_amen_cat

    '                    command.ExecuteReader()
    '                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Script", "updateSuccess();", True)

    '                Catch ex As Exception

    '                    _error = ex.Message
    '                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Script", "errorDataBase();", True)

    '                End Try
    '            End If

    '        End If

    '    End Using

    '    ctrlIdiomaAmen.Limpia()
    'End Sub
End Class