Imports System.Data.SqlClient

Public Class DataAccess

    Private Const spInsertSeamlessLog As String = "spInsertSeamlessLog"
    Private Const spInsertExportedLog As String = "spInsertExportedLog"
    Private Const spGetExportedLog As String = "spGetExportedLog"

    Public Class Seamless

        Public Class Campos
            Public Const IdLog As String = "IdLog"
            Public Const PN As String = "PN"
            Public Const DS As String = "DS"
            Public Const TS As String = "TS"
            Public Const [IN] As String = "IN"
            Public Const OT As String = "OT"
            Public Const NA As String = "NA"
            Public Const NC As String = "NC"
            Public Const RSP As String = "RSP"
            Public Const TX As String = "TX"
            Public Const MSG As String = "MSG"
            Public Const GDS As String = "GDS"
            Public Const AGY As String = "AGY"
        End Class

        Public Function Insert(ByVal dt As DataTable) As Boolean
            Dim cmd As SqlCommand
            Dim cn As SqlConnection = New SqlConnection(ConexionSQL)
            cmd = New SqlCommand(spInsertSeamlessLog, cn)
            Dim da As New SqlDataAdapter
            Try
                With cmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add("@" & Campos.PN, SqlDbType.NVarChar, 7).SourceColumn = Campos.PN
                    .Parameters.Add("@" & Campos.DS, SqlDbType.NVarChar, 7).SourceColumn = Campos.DS
                    .Parameters.Add("@" & Campos.TS, SqlDbType.NVarChar, 6).SourceColumn = Campos.TS
                    .Parameters.Add("@" & Campos.IN, SqlDbType.NVarChar, 7).SourceColumn = Campos.IN
                    .Parameters.Add("@" & Campos.OT, SqlDbType.NVarChar, 7).SourceColumn = Campos.OT
                    .Parameters.Add("@" & Campos.NA, SqlDbType.Int).SourceColumn = Campos.NA
                    .Parameters.Add("@" & Campos.NC, SqlDbType.Int).SourceColumn = Campos.NC
                    .Parameters.Add("@" & Campos.RSP, SqlDbType.NVarChar, 1).SourceColumn = Campos.RSP
                    .Parameters.Add("@" & Campos.TX, SqlDbType.NVarChar, 2).SourceColumn = Campos.TX
                    .Parameters.Add("@" & Campos.MSG, SqlDbType.NVarChar, 250).SourceColumn = Campos.MSG
                    .Parameters.Add("@" & Campos.GDS, SqlDbType.NVarChar, 2).SourceColumn = Campos.GDS
                    .Parameters.Add("@" & Campos.AGY, SqlDbType.NVarChar, 8).SourceColumn = Campos.AGY
                End With
                cn.Open()
                da.InsertCommand = cmd
                da.Update(dt)
                Insert = True
            Catch ex As Exception
                Insert = False
            Finally
                cn.Close()
            End Try
        End Function

    End Class

    Public Class ExportedLogs

        Public Class Campos
            Public Const IdLog As String = "IdLog"
            Public Const File As String = "File"
            Public Const Year As String = "Year"
        End Class

        Public Function Insert(ByVal sFile As String, ByVal iYear As Integer) As Boolean
            Dim cmd As SqlCommand
            Dim cn As SqlConnection = New SqlConnection(ConexionSQL)
            cmd = New SqlCommand(spInsertExportedLog, cn)
            Try
                With cmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add("@" & Campos.File, SqlDbType.NVarChar, 12).Value = sFile
                    .Parameters.Add("@" & Campos.Year, SqlDbType.Int).Value = iYear
                End With
                cn.Open()
                cmd.ExecuteNonQuery()
                Insert = True
            Catch ex As Exception
                Insert = False
            Finally
                cn.Close()
            End Try
        End Function

        Public Function Validate(ByVal sFile As String, ByVal iYear As Integer) As Boolean
            Dim cmd As SqlCommand
            Dim cn As SqlConnection = New SqlConnection(ConexionSQL)
            cmd = New SqlCommand(spGetExportedLog, cn)
            Dim da As SqlDataAdapter
            Dim ds As New DataSet
            Try
                cn.Open()
                With cmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add("@" & Campos.File, SqlDbType.NVarChar, 12).Value = sFile
                    .Parameters.Add("@" & Campos.Year, SqlDbType.Int).Value = iYear
                End With
                da = New SqlDataAdapter(cmd)
                da.Fill(ds)
                If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count = 0 Then
                    Validate = True
                Else
                    Validate = False
                End If
            Catch ex As Exception
            Finally
                cn.Close()
            End Try
        End Function

    End Class

End Class