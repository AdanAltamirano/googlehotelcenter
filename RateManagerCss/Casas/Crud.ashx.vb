Imports System.Web
Imports System.Web.Services
Imports System.Data.SqlClient

Public Class Crud
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim method As String = context.Request.Params("MethodName")
        context.Response.ContentType = "text/json"
        Select Case method
            Case "GetCategories"
                context.Response.Write(GetCategories())
            Case "GetCategoriesCasas"
                context.Response.Write(GetCategoriesCasas())
            Case "deleteCategory"
                Dim id = context.Request.Params("id")
                context.Response.Write(deleteCategory(id))
            Case "deleteCategoryCasas"
                Dim id = context.Request.Params("id")
                context.Response.Write(deleteCategoryCasas(id))
            Case "GetAmenitiesByCategory"
                Dim id = context.Request.Params("id")
                context.Response.Write(getAmenitiesByCategoryId(id))
            Case "deleteAmenity"
                Dim id = context.Request.Params("id")
                context.Response.Write(deleteAmenity(id))
            Case "updateCategory"
                Dim id_cat = context.Request.Params("id_cat")
                context.Response.Write(updateCategory(id_cat))
                Exit Select
        End Select

    End Sub
    Public Function GetCategoriesCasas() As String

        Dim connString As String = ConfigurationManager.AppSettings("HotelConnectionString")
        Dim spGetCategories As String = "spGetCategoriasCasas"

        Dim jsonResponse As String

        Using Conn As New SqlConnection(connString)
            Conn.Open()
            Dim command As New SqlCommand(spGetCategories, Conn)

            Try
                command.CommandType = CommandType.StoredProcedure
                Dim adap As New SqlDataAdapter(command)
                Dim data = New DataSet

                adap.Fill(data)

                Dim cont = 0

                jsonResponse = ""

                jsonResponse += "{ ""response"": """", "
                jsonResponse += """results"": ["
                For Each item As DataRow In data.Tables(0).Rows

                    cont += 1
                    jsonResponse += "{  ""id"": """ & item("idCategoriaCasas").ToString() & ""","
                    jsonResponse += """name"": """ & item("Descripcion").ToString() & ""","

                    If cont = data.Tables(0).Rows.Count Then
                        jsonResponse += """id_dictionary"": """ & item("idDiccionario").ToString() & """}"
                    Else
                        jsonResponse += """id_dictionary"": """ & item("idDiccionario").ToString() & """},"
                    End If
                Next

                jsonResponse += "]}"
                Conn.Close()

            Catch ex As Exception

                jsonResponse = "{" _
                               + """Error"":""True""" _
                               + ",""Description"":""" + ex.Message + """}"
            End Try
        End Using

        Return jsonResponse

    End Function
    Public Function GetCategories() As String

        Dim connString As String = ConfigurationManager.AppSettings("HotelConnectionString")
        Dim spGetCategories As String = "spGetCategoryAmenity"

        Dim jsonResponse As String

        Using Conn As New SqlConnection(connString)
            Conn.Open()
            Dim command As New SqlCommand(spGetCategories, Conn)

            Try
                command.CommandType = CommandType.StoredProcedure
                Dim adap As New SqlDataAdapter(command)
                Dim data = New DataSet

                adap.Fill(data)

                Dim cont = 0

                jsonResponse = ""

                jsonResponse += "{ ""response"": """", "
                jsonResponse += """results"": ["
                For Each item As DataRow In data.Tables(0).Rows

                    cont += 1
                    jsonResponse += "{  ""id"": """ & item("idCategoriaAmenidad").ToString() & ""","
                    jsonResponse += """name"": """ & item("Descripcion").ToString() & ""","

                    If cont = data.Tables(0).Rows.Count Then
                        jsonResponse += """id_dictionary"": """ & item("idDiccionario").ToString() & """}"
                    Else
                        jsonResponse += """id_dictionary"": """ & item("idDiccionario").ToString() & """},"
                    End If
                Next

                jsonResponse += "]}"

                
            Catch ex As Exception

                jsonResponse = "{" _
                               + """Error"":""True""" _
                               + ",""Description"":""" + ex.Message + """}"
            End Try
        End Using

        Return jsonResponse

    End Function

    Public Function deleteCategoryCasas(ByVal id As Integer) As String
        Dim message = "Se ha eliminado con exito"
        Dim jsonResponse = ""

        Dim connString As String = ConfigurationManager.AppSettings("HotelConnectionString")
        Dim spDeleteCategory As String = "spDeleteCategoriasCasas"
        Dim _error As String

        Using Conn As New SqlConnection(connString)
            Conn.Open()
            Dim command As New SqlCommand(spDeleteCategory, Conn)

            Try
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id

                command.ExecuteReader()

                jsonResponse += "{" _
                               + """Error"":""False""" _
                               + ",""Description"":""" + message + """}"
            Catch ex As Exception
                _error = ex.Message

                jsonResponse += "{" _
                               + """Error"":""True""" _
                               + ",""Description"":""" + _error + """}"
            End Try
        End Using

        Return jsonResponse
    End Function
    Public Function deleteCategory(ByVal id As Integer) As String
        Dim message = "Se ha eliminado con exito"
        Dim jsonResponse = ""

        Dim connString As String = ConfigurationManager.AppSettings("HotelConnectionString")
        Dim spDeleteCategory As String = "spDeleteCategory"
        Dim _error As String

        Using Conn As New SqlConnection(connString)
            Conn.Open()
            Dim command As New SqlCommand(spDeleteCategory, Conn)

            Try
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id

                command.ExecuteReader()

                jsonResponse += "{" _
                               + """Error"":""False""" _
                               + ",""Description"":""" + message + """}"
            Catch ex As Exception
                _error = ex.Message

                jsonResponse += "{" _
                               + """Error"":""True""" _
                               + ",""Description"":""" + _error + """}"
            End Try
        End Using

        Return jsonResponse
    End Function

    Public Function deleteAmenity(ByVal id As Integer) As String
        Dim message = "Se ha eliminado con exito."
        Dim jsonResponse = ""

        Dim connString As String = ConfigurationManager.AppSettings("PortalConnectionString")
        Dim spDeleteCategory As String = "spDeleteAmenityCubaHouse"
        Dim _error As String

        Using Conn As New SqlConnection(connString)
            Conn.Open()
            Dim command As New SqlCommand(spDeleteCategory, Conn)

            Try
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id

                command.ExecuteReader()

                jsonResponse += "{" _
                               + """Error"":""False""" _
                               + ",""Description"":""" + message + """}"
            Catch ex As Exception
                _error = ex.Message

                jsonResponse += "{" _
                               + """Error"":""True""" _
                               + ",""Description"":""" + _error + """}"
            End Try
        End Using

        Return jsonResponse
    End Function

    Public Function getAmenitiesByCategoryId(ByVal id_Cat As Integer) As String

        Dim connString As String = ConfigurationManager.AppSettings("PortalConnectionString")
        Dim spGetAmenities As String = "spGetAllAmenitiesCubaHousesByCategory"

        Dim jsonResponse As String

        Using Conn As New SqlConnection(connString)
            Conn.Open()
            Dim command As New SqlCommand(spGetAmenities, Conn)

            Try
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@idCategoriaAmenidad", SqlDbType.Int).Value = id_Cat

                Dim adap As New SqlDataAdapter(command)
                Dim data = New DataSet

                adap.Fill(data)
                Dim cont = 0

                jsonResponse = ""

                jsonResponse += "{ ""response"": """", "
                jsonResponse += """results"": ["
                For Each item As DataRow In data.Tables(0).Rows

                    cont += 1
                    jsonResponse += "{  ""id"": """ & item("idAmenidad").ToString() & ""","
                    jsonResponse += """name"": """ & item("Descripcion").ToString() & ""","
                    jsonResponse += """idCategoriaAmenidad"": """ & item("idCategoriaAmenidad").ToString() & ""","

                    If cont = data.Tables(0).Rows.Count Then
                        jsonResponse += """id_dictionary"": """ & item("idDiccionario").ToString() & """}"
                    Else
                        jsonResponse += """id_dictionary"": """ & item("idDiccionario").ToString() & """},"
                    End If
                Next

                jsonResponse += "]}"

            Catch ex As Exception

                jsonResponse = "{" _
                               + """Error"":""True""" _
                               + ",""Description"":""" + ex.Message + """}"
            End Try
        End Using

        Return jsonResponse
    End Function

    Public Function updateCategory(ByVal id_cat As Integer) As String
        Dim jsonResponse, message As String
        jsonResponse = ""
        message = "goiyo"

        Dim es = Gobal.ctrl_idioma_cat.GetES
        Dim en = Gobal.ctrl_idioma_cat.GetEN
        Dim id = id_cat

        jsonResponse = "{" _
                               + """Error"":""FALSE""" _
                               + ",""Description"":""" + message + """}"

        Return jsonResponse
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class