Imports System.Runtime.Serialization
Imports System.Data.SqlClient

Namespace ConfluxMapping
    <System.ComponentModel.DesignerCategory("Code"), SerializableAttribute()> _
    Public Class clsData
        Inherits DataSet

        Public Enum CodeType
            Rooms
            Rates
        End Enum

#Region "Tables and column names"
#Region "BD Conflux"
        Public Const TBL_IDS_Booking As String = "IDS_Booking"
        Public Const FLD_ID As String = "Id"
        Public Const FLD_BookingId As String = "BookingId"
        Public Const FLD_Checked_IDS As String = "Checked_IDS"
        Public Const FLD_Mapped_IDS As String = "Mapped_IDS"
        Public Const FLD_Checked_Ws As String = "Checked_Ws"
#End Region

#Region "BD OzRateCrs"

#End Region
#End Region


#Region "Functions"
        'Constructoe to support serialization
        Private Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        Private Sub BuidDataTables()
            Dim IDS_Booking As New DataTable(TBL_IDS_Booking)

            With IDS_Booking.Columns
                .Add(FLD_ID, GetType(System.Int32))
                .Add(FLD_BookingId, GetType(System.Int32))
                .Add(FLD_Checked_IDS, GetType(System.Boolean))
                .Add(FLD_Mapped_IDS, GetType(System.Boolean))
                .Add(FLD_Checked_Ws, GetType(System.Boolean))
            End With

            Me.Tables.Add(IDS_Booking)
        End Sub
#End Region

    End Class

#Region "DataAccess"
    Public Class clsDataAccess
        Implements IDisposable

        Private disposedValue As Boolean ' Para detectar llamadas redundantes
        Private dsCommand As SqlDataAdapter
        Private insertCommand As SqlCommand
        Private updateCommand As SqlCommand
        Private Connection As SqlConnection

        Private Class Procedures
            Public Shared ReadOnly GET_IDSBookings As String = "spGetIDSBookings_All"
            Public Shared ReadOnly GET_UnmappedRoomsRateplansByHotel_IDS As String = "spGetUnmappedReservationsByHotel_IDS"
            Public Shared ReadOnly UPDATE_ConfluxMappedIDS As String = "spUpdateMapped_IDS"

            Public Shared ReadOnly GET_RateCRSHotels As String = "spHotelesGetAll"
            Public Shared ReadOnly GET_IDS_Hotels As String = "Get_IDS_Hotels"
            Public Shared ReadOnly GET_UVRoomCode As String = "getUVRoomCode"
            Public Shared ReadOnly GET_UVRateCode As String = "Get_IDS_Rate_Code"
        End Class

        Private Class Params

        End Class

        Public Sub New(ByVal stringConnection As String)
            MyBase.New()

            dsCommand = New SqlDataAdapter
            dsCommand.SelectCommand = New SqlCommand
            Connection = New SqlConnection(stringConnection)
            'dsCommand.TableMappings.Add("Table", clsData.TBL_IDS_Booking)

        End Sub

        Public Function GetRateCrsHotels(ByRef strError As String) As DataSet
            Dim RateCRSHotels As New DataSet
            With dsCommand
                Try
                    With .SelectCommand
                        .Connection = Connection
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = Procedures.GET_RateCRSHotels
                    End With
                    .Fill(RateCRSHotels)
                Catch ex As Exception
                    strError = ex.Message & " : Method GetRateCrsHotels"
                Finally
                    ' dispose commands
                    If Not .SelectCommand Is Nothing Then
                        If Not .SelectCommand.Connection Is Nothing Then
                            .SelectCommand.Connection.Dispose()
                        End If
                        .SelectCommand.Dispose()
                    End If
                    .Dispose()
                End Try
            End With
            Return RateCRSHotels
        End Function

        Public Function GetIDSCrsHotels(ByVal CrsHotelID As Integer, ByVal IDS As Integer, ByRef strError As String) As DataSet
            Dim CRSIDSHotels As New DataSet
            With dsCommand
                Try
                    With .SelectCommand
                        .Connection = Connection
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = Procedures.GET_IDS_Hotels

                        .Parameters.AddWithValue("@idhotel", CrsHotelID)
                        .Parameters.AddWithValue("@ids", IDS)
                        .Parameters.AddWithValue("@codigo", "0")
                    End With
                    .Fill(CRSIDSHotels)
                Catch ex As Exception
                    strError = ex.Message & " : Method GetIDSCrsHotels"
                Finally
                    ' dispose commands
                    If Not .SelectCommand Is Nothing Then
                        If Not .SelectCommand.Connection Is Nothing Then
                            .SelectCommand.Connection.Dispose()
                        End If
                        .SelectCommand.Dispose()
                    End If
                    .Dispose()
                End Try
            End With
            Return CRSIDSHotels
        End Function

        Public Function GetUvRoomOrRateCodes(ByVal IDS_ID As Integer, ByVal IDS_RoomOrRateCode As String, ByVal RateCrsHotelId As Integer, ByVal Type As clsData.CodeType, ByRef strError As String) As DataSet
            Dim RateCRSRoom As New DataSet
            With dsCommand
                Try
                    With .SelectCommand
                        .Connection = Connection
                        .CommandType = CommandType.StoredProcedure

                        Select Case Type
                            Case clsData.CodeType.Rates
                                .CommandText = Procedures.GET_UVRateCode
                                .Parameters.AddWithValue("@idhotel", RateCrsHotelId)
                            Case clsData.CodeType.Rooms
                                .CommandText = Procedures.GET_UVRoomCode
                                .Parameters.AddWithValue("@hotelId", RateCrsHotelId)
                        End Select

                        .Parameters.AddWithValue("@ids", IDS_ID)
                        .Parameters.AddWithValue("@codigo", IDS_RoomOrRateCode)

                    End With
                    .Fill(RateCRSRoom)
                Catch ex As Exception
                    strError = ex.Message & " : Method GetUvRoomOrRateCodes"
                Finally
                    ' dispose commands
                    If Not .SelectCommand Is Nothing Then
                        If Not .SelectCommand.Connection Is Nothing Then
                            .SelectCommand.Connection.Dispose()
                        End If
                        .SelectCommand.Dispose()
                    End If
                    .Dispose()
                End Try
            End With
            Return RateCRSRoom
        End Function

        Public Function GetUnmappedRoomsRateplansByHotel_IDS(ByVal Hotel_IDS As Integer, ByRef strError As String) As DataSet
            Dim UnmappedReservations As New DataSet
            With dsCommand
                Try
                    With .SelectCommand
                        .Connection = Connection
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = Procedures.GET_UnmappedRoomsRateplansByHotel_IDS

                        .Parameters.AddWithValue("@Hotel_IDS", Hotel_IDS)
                    End With
                    .Fill(UnmappedReservations)
                Catch ex As Exception
                    strError = ex.Message & " : Method GetUnmappedRoomsRateplansByHotel_IDS"
                Finally
                    ' dispose commands
                    If Not .SelectCommand Is Nothing Then
                        If Not .SelectCommand.Connection Is Nothing Then
                            .SelectCommand.Connection.Dispose()
                        End If
                        .SelectCommand.Dispose()
                    End If
                    .Dispose()
                End Try
            End With
            Return UnmappedReservations
        End Function

        Public Function UpdateMappedIDS(ByVal Hotel_IDS As Integer, ByVal ChannelId As Integer, Optional ByVal RoomSourceId As String = "", Optional ByVal RateSourceId As String = "", Optional ByRef strError As String = "") As Boolean
            Dim updateCommand As New SqlCommand

            With updateCommand
                Try
                    .Connection = Connection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = Procedures.UPDATE_ConfluxMappedIDS

                    .Parameters.AddWithValue("@Hotel_IDS", Hotel_IDS)
                    .Parameters.AddWithValue("@ChannelId", ChannelId)
                    If Not RoomSourceId = String.Empty Then
                        .Parameters.AddWithValue("@RoomSourceId", RoomSourceId)
                    End If
                    If Not RateSourceId = String.Empty Then
                        .Parameters.AddWithValue("@RateSourceId", RateSourceId)
                    End If

                    .Connection.Open()
                    .ExecuteNonQuery()
                    .Connection.Close()
                Catch ex As Exception
                    strError = ex.Message & " : Method UpdateMappedIDS"
                    If .Connection.State = ConnectionState.Broken Or Connection.State = ConnectionState.Open Then
                        .Connection.Close()
                    End If
                    Return False
                Finally
                    If .Connection.State = ConnectionState.Broken Or Connection.State = ConnectionState.Open Then
                        .Connection.Close()
                    End If
                End Try
                Return True
            End With
            Return False
        End Function

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If (Not disposing) Then
                Exit Sub  ' we're being collected, so let the GC take care of this object
            End If
            If Not dsCommand Is Nothing Then
                If Not dsCommand.SelectCommand Is Nothing Then
                    If Not dsCommand.SelectCommand.Connection Is Nothing Then
                        dsCommand.SelectCommand.Connection.Dispose()
                    End If
                    dsCommand.SelectCommand.Dispose()
                End If
                dsCommand.Dispose()
                dsCommand = Nothing
            End If
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

    End Class
#End Region
End Namespace


