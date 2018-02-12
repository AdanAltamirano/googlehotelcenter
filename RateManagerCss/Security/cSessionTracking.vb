Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Public Class SessionTracking
    Private Shared dbCollector As New SessionDbCollector

    Protected Sub New()
    End Sub

#Region "Serialization"
    Private Shared Function SerializeObject(ByVal obj As Object) As Byte()
        Dim bytes(-1) As Byte
        Dim stream As MemoryStream
        Dim formatter As BinaryFormatter

        Try
            formatter = New BinaryFormatter
            stream = New MemoryStream

            formatter.Serialize(stream, obj)
            stream.Seek(0, SeekOrigin.Begin)
            ReDim bytes(stream.Length - 1)
            stream.Read(bytes, 0, stream.Length)
        Catch ex As Exception
            Throw ex
        Finally
            If Not stream Is Nothing Then
                stream.Close()
            End If
        End Try

        Return bytes
    End Function

    Private Shared Function DeserializeObject(ByVal bytes() As Byte) As Object
        Dim obj As Object
        Dim stream As MemoryStream
        Dim writer As BinaryWriter
        Dim formatter As BinaryFormatter

        Try
            If Not bytes Is Nothing Then
                stream = New MemoryStream(bytes)
                formatter = New BinaryFormatter
                obj = formatter.Deserialize(stream)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not stream Is Nothing Then
                stream.Close()
            End If
        End Try

        Return obj
    End Function
#End Region

    Public Shared Function Load(ByVal SessionID As String, ByVal ElementName As String) As Object
        Dim obj As Object
        Load(SessionID, ElementName, obj)
        Return obj
    End Function

    Public Shared Sub Load(ByVal SessionID As String, ByVal ElementName As String, ByRef Content As Object, Optional ByRef TimeStamp As DateTime = #1/1/2000#)
        Dim bytes() As Byte
        dbCollector.GetSessionElement(SessionID, ElementName, bytes, TimeStamp)
        Content = DeserializeObject(bytes)
    End Sub

    Public Shared Sub Save(ByVal SessionID As String, ByVal ElementName As String, ByVal Content As Object, Optional ByRef TimeStamp As DateTime = #1/1/2000#)
        Dim bytes() As Byte = SerializeObject(Content)
        dbCollector.SetSessionElement(SessionID, ElementName, bytes, TimeStamp)
    End Sub

#Region "Private Class SessionDbCollector"

    Private Class SessionDbCollector
        Private Const COL_SESSIONID As String = "SessionID"
        Private Const COL_ELEMENTNAME As String = "ElementName"
        Private Const COL_CONTENT As String = "Content"
        Private Const COL_TIMESTAMP As String = "TimeStamp"

        Private innerConnection As SqlConnection
        Private mustCloseConnection As Boolean

        Public Sub New()
            innerConnection = New SqlConnection(AppSettings("OzSessionConnectionString"))
        End Sub

        Private Sub OpenConnection()
            If innerConnection.State = ConnectionState.Broken Then
                innerConnection.Close()
            End If

            If innerConnection.State = ConnectionState.Closed Then
                innerConnection.Open()
                mustCloseConnection = True
            Else
                mustCloseConnection = False
            End If
        End Sub

        Private Sub CloseConnection()
            If mustCloseConnection AndAlso (innerConnection.State <> ConnectionState.Closed) Then
                innerConnection.Close()
            End If
        End Sub

        Private Function GetCommandForStoreProcedure(ByVal storeProcedureName As String) As SqlCommand
            Dim cmd As SqlCommand

            Try
                cmd = New SqlCommand(storeProcedureName, innerConnection)
                cmd.CommandType = CommandType.StoredProcedure

                OpenConnection()
                SqlCommandBuilder.DeriveParameters(cmd)
            Catch ex As Exception
                Throw ex
            Finally
                CloseConnection()
            End Try

            Return cmd
        End Function

        Public Sub SetSessionElement(ByVal SessionID As String, ByVal ElementName As String, ByVal Content() As Byte, ByRef TimeStamp As DateTime)
            Try
                OpenConnection()

                Dim cmd As SqlCommand = GetCommandForStoreProcedure("spSetSessionElement")
                cmd.Parameters("@SessionID").Value = SessionID
                cmd.Parameters("@ElementName").Value = ElementName
                cmd.Parameters("@Content").Value = Content
                cmd.Parameters("@TimeStamp").Direction = ParameterDirection.Output
                cmd.ExecuteNonQuery()
                TimeStamp = CDate(cmd.Parameters("@TimeStamp").Value)
            Catch ex As Exception
                Throw ex
            Finally
                CloseConnection()
            End Try
        End Sub

        Public Sub GetSessionElement(ByVal SessionID As String, ByVal ElementName As String, ByRef Content() As Byte, ByRef TimeStamp As DateTime)
            Try
                OpenConnection()

                Dim cmd As SqlCommand = GetCommandForStoreProcedure("spGetSessionElement")
                cmd.Parameters("@SessionID").Value = SessionID
                cmd.Parameters("@ElementName").Value = ElementName

                Dim binary As SqlBinary
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                Dim contentOrd As Integer = reader.GetOrdinal(COL_CONTENT)
                Dim timeStampOrd As Integer = reader.GetOrdinal(COL_TIMESTAMP)

                If reader.Read() Then
                    binary = reader.GetSqlBinary(contentOrd)
                    Content = binary.Value
                    TimeStamp = reader.GetDateTime(timeStampOrd)
                End If
                reader.Close()
            Catch ex As Exception
                Throw ex
            Finally
                CloseConnection()
            End Try
        End Sub
    End Class
#End Region

End Class
