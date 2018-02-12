Imports System.Data
Imports System.Data.SqlClient

Public Class ClsAssingCountryRatePlan
    Implements IDisposable

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: eliminar estado administrado (objetos administrados).
            End If

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: invalidar Finalize() sólo si la instrucción Dispose(ByVal disposing As Boolean) anterior tiene código para liberar recursos no administrados.
    'Protected Overrides Sub Finalize()
    '    ' No cambie este código. Ponga el código de limpieza en la instrucción Dispose(ByVal disposing As Boolean) anterior.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Private DsCommand As SqlDataAdapter
    Private InsertCommand As SqlCommand
    Private UpdateCommand As SqlCommand

    Const PARAM_IDPAIS As String = "@idPais"
    Const PARAM_IDRATEPLAN As String = "@idRatePlan"
    Const PARAM_IDHOTEL As String = "@idHotel"

    Public Sub New()
        MyBase.New()

        DsCommand = New SqlDataAdapter

        DsCommand.TableMappings.Add("Table", "PaisRestricciones")
    End Sub

    Public Function InsertAssingnment(ByVal IdHotel As String, ByVal IdRatePlan As String, ByVal IdPais As String, ByRef strError As String) As Boolean
        Try
                InsertCommand = New SqlCommand

                With InsertCommand
                    .Connection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("portalconnectionstring"))
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "spInsertAssignamentCountry"

                    With .Parameters
                        .Add(New SqlParameter(PARAM_IDHOTEL, SqlDbType.Int))
                        .Add(New SqlParameter(PARAM_IDPAIS, SqlDbType.NVarChar, 3))
                        .Add(New SqlParameter(PARAM_IDRATEPLAN, SqlDbType.NVarChar, 4))

                        .Item(PARAM_IDHOTEL).Value = CInt(IdHotel)
                        .Item(PARAM_IDPAIS).Value = IdPais
                        .Item(PARAM_IDRATEPLAN).Value = IdRatePlan
                    End With
                End With

            With DsCommand
                .InsertCommand = InsertCommand
                .InsertCommand.Connection.Open()
                .InsertCommand.ExecuteNonQuery()
                .InsertCommand.Connection.Close()
            End With
        Catch ex As Exception
            strError = ex.Message
            Return False
        Finally
            With DsCommand.InsertCommand.Connection
                If .State = ConnectionState.Open Or .State = ConnectionState.Broken Then
                    .Close()
                End If
            End With
        End Try
        Return True
    End Function

    Public Function DeleteAsignmentCountries(ByVal IdHotel As Integer, ByVal IdRatePlan As String, ByRef strError As String)
        Dim ds As DataSet = New DataSet

        DsCommand.DeleteCommand = New SqlCommand
        DsCommand.DeleteCommand.Connection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("portalconnectionstring"))

        With DsCommand
            Try
                With .DeleteCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "spDeleteAssignamentCountry"
                    .Parameters.AddWithValue("@IdHotel", IdHotel)
                    .Parameters.AddWithValue("@IdRatePlan", IdRatePlan)

                    .Connection.Open()
                    .ExecuteNonQuery()
                    .Connection.Close()
                End With
            Catch ex As Exception
                strError = ex.Message
            Finally
                With .DeleteCommand
                    If .Connection.State = ConnectionState.Closed Or .Connection.State = ConnectionState.Broken Then
                        .Connection.Close()
                    End If
                End With
            End Try
        End With
    End Function

    Public Function GetAssignamentCountries(ByVal IdHotel As Integer, ByVal IdRatePlan As String, ByRef strError As String) As DataSet
        Dim ds As DataSet = New DataSet

        DsCommand.SelectCommand = New SqlCommand
        DsCommand.SelectCommand.Connection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("portalconnectionstring"))

        With DsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "spGetAssignamentCountryByHotelId"
                    .Parameters.AddWithValue("@IdHotel", IdHotel)
                    .Parameters.AddWithValue("@IdRAtePlan", IdRatePlan)
                End With
                .Fill(ds)
            Catch ex As Exception
                strError = ex.Message
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()
            End Try

        End With

        Return ds
    End Function
End Class
