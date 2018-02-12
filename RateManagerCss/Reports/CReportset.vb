Imports System.IO
Imports System.Xml
Imports System.Xml.Schema
Imports System.Collections.Specialized

Imports System.Configuration.ConfigurationSettings
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.shared
Imports System.Data.SqlClient

Public Class CReportset
    Protected _mCrRptDocument As New ReportDocument
    Protected _mcParameters As NameValueCollection

    Protected _msReportName As String
    Protected _msCulture As String
    Protected _msReportDescription As String
    Protected _msServerName As String = ""
    Protected _msDataBase As String = ""
    Protected _msUser As String = ""
    Protected _msPassword As String = ""


    Sub New()
        Dim sStringConection As String
        _mcParameters = New NameValueCollection
        sStringConection = System.Configuration.ConfigurationManager.AppSettings("HotelConnection")
        GetConection(_msServerName, _msDataBase, _msUser, _msPassword, sStringConection)
    End Sub

    '//        Name: Function ExistParam(...) As Boolean
    '// Description: return type interface.
    '//        (c) Dr 2005
    Function ExistParam(ByVal spParam As String, _
                        ByVal dfCrystal As DataDefinition) As Boolean
        Dim i As Short

        For i = 0 To dfCrystal.ParameterFields.Count - 1
            If dfCrystal.ParameterFields.Item(i).Name.ToUpper = spParam.ToUpper Then
                Return (True)
            End If
        Next
        Return (False)

    End Function

    Function GetConection(ByRef nServer As String, _
                             ByRef nDb As String, _
                             ByRef nUser As String, _
                             ByRef nPassw As String, _
                             ByVal StringConection As String) As Byte
        Dim sOffSet As String()
        Dim val As String
        Dim sParam As String

        Try
            'StringConection = _msStringConection.ToUpper
            sOffSet = Split(StringConection, ";")
            For Each val In sOffSet
                sParam = val.Substring(val.IndexOf("=") + 1, val.Length - val.IndexOf("=") - 1)
                Select Case Trim(val.Substring(0, val.IndexOf("=")).ToUpper)
                    Case "DATA SOURCE", "SERVER"
                        nServer = sParam
                    Case "INITIAL CATALOG", "DATABASE"
                        nDb = sParam
                    Case "USER ID"
                        nUser = sParam
                    Case "PASSWORD"
                        nPassw = sParam
                End Select
            Next
        Catch ex As Exception
            Return 0
        End Try
        Return 1
    End Function

    Function GetParameters(ByVal ds As Crystal_1_0) As Short
        Dim i As Integer
        Dim drp As Crystal_1_0.ReportRow
        Dim dr As DataRow
        Dim rowArray(1) As Object

        drp = ds.Report.Rows(0)
        _msReportName = drp.Name
        _msReportDescription = drp.Description
        _msCulture = drp.Culture

        If Not IsDBNull(drp.Name) Then _mcParameters.Add("ReportName", drp.Name.ToString & ".Rpt")

        '// Set the discreet value to optional parameters for Header.
        dr = ds.Header.Rows(0)
        rowArray = dr.ItemArray
        For i = 0 To rowArray.Length - 1
            If Not IsDBNull(rowArray(i)) Then _
                _mcParameters.Add(ds.Header.Columns(i).ToString, rowArray(i))
        Next

        '// Set the discreet value to optional parameters for Header.
        For Each drq As DataRow In ds.QueryParameters.Rows
            _mcParameters.Add(drq("Name"), drq("Value"))
        Next
        For Each drr As DataRow In ds.Resources.Rows
            _mcParameters.Add(drr("id"), drr("value"))
        Next
        Return 0
    End Function

    Private Sub SetConnectionInfoReport(ByVal myReportDocument As ReportDocument)
        Dim crTables As Tables

        crTables = myReportDocument.Database.Tables
        For Each crTable As CrystalDecisions.CrystalReports.Engine.Table In crTables
            Dim tblLogonInfo As TableLogOnInfo = crTable.LogOnInfo
            tblLogonInfo.ConnectionInfo.ServerName = _msServerName
            tblLogonInfo.ConnectionInfo.DatabaseName = _msDataBase
            tblLogonInfo.ConnectionInfo.UserID = _msUser
            tblLogonInfo.ConnectionInfo.Password = _msPassword
            crTable.ApplyLogOnInfo(tblLogonInfo)
            crTable.Location = _msDataBase & ".dbo." & crTable.Location.Substring(crTable.Location.LastIndexOf(".") + 1)
        Next

    End Sub

    Private Sub SetSectionReport(ByVal crReportDocument As ReportDocument)
        Dim crSections As Sections
        Dim crSection As Section
        Dim crReportObjects As ReportObjects
        Dim crReportObject As ReportObject
        Dim crSubreportObject As SubreportObject
        Dim crSubReport As New ReportDocument
        Dim crConnInfo As New ConnectionInfo
        Dim crTables As Tables
        Dim crTable As Table

        'Set the sections collection with report sections
        crSections = crReportDocument.ReportDefinition.Sections

        For Each crSection In crSections
            crReportObjects = crSection.ReportObjects
            For Each crReportObject In crReportObjects
                If crReportObject.Kind = _
                   ReportObjectKind.SubreportObject Then
                    'If you find a subreport, typecast the reportobject
                    crSubreportObject = CType(crReportObject, SubreportObject)
                    'Open the subreport
                    crSubReport = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName)
                    crTables = crSubReport.Database.Tables
                    '// Loop through each table and set the connection info
                    For Each crTable In crTables
                        Dim tblLogonInfo As TableLogOnInfo = crTable.LogOnInfo
                        tblLogonInfo.ConnectionInfo.ServerName = _msServerName
                        tblLogonInfo.ConnectionInfo.UserID = _msUser
                        tblLogonInfo.ConnectionInfo.Password = _msPassword
                        tblLogonInfo.ConnectionInfo.DatabaseName = _msDataBase
                        crTable.ApplyLogOnInfo(tblLogonInfo)
                        crTable.Location = _msDataBase & ".dbo." & crTable.Location.Substring(crTable.Location.LastIndexOf(".") + 1)
                    Next

                End If
            Next
        Next

    End Sub

    Function AddParamRpt(ByVal parametersCollection As NameValueCollection, _
                         ByVal crReportDocument As ReportDocument) As DataDefinition

        Dim DiscreteParam As New ParameterDiscreteValue
        Dim RangeParam As New ParameterRangeValue
        Dim ParamValues As New ParameterValues
        Dim paramField As New ParameterField()
        Dim dfCrystal As DataDefinition
        Dim objKey
        Dim valueParam() As String
        Dim sname As String

        dfCrystal = crReportDocument.DataDefinition
        For Each objKey In parametersCollection.Keys
            valueParam = parametersCollection.GetValues(objKey)
            sname = String.Format("@{0}", objKey.ToString)
            If ExistParam(sname, dfCrystal) Then

                paramField.ParameterFieldName = String.Format("@{0}", objKey.ToString)

                ParamValues = New ParameterValues
                DiscreteParam.Value = valueParam(0)
                'If DiscreteParam.Value = "" Then DiscreteParam.Value = DBNull.Value
                ParamValues.Add(DiscreteParam)
                dfCrystal.ParameterFields(sname).ApplyCurrentValues(ParamValues)
            End If
        Next
        Return dfCrystal

    End Function

    Public Function Report(ByVal ds As Crystal_1_0, ByVal dsDataSource As DataSet, ByVal pathFile As String) As ReportDocument

        _mCrRptDocument = New ReportDocument
        GetParameters(ds)
        If System.IO.File.Exists(pathFile) Then
            _mCrRptDocument.Load(pathFile)
            If dsDataSource Is Nothing Then
                SetConnectionInfoReport(_mCrRptDocument)

                SetSectionReport(_mCrRptDocument)
            Else
                _mCrRptDocument.SetDataSource(dsDataSource)
            End If
            AddParamRpt(_mcParameters, _mCrRptDocument)
        End If

        Return _mCrRptDocument

    End Function

    Public Function Report(ByVal ds As Crystal_1_0, Optional ByVal dsDataSource As DataSet = Nothing) As ReportDocument
        Dim spathReport As String
        Dim fileCrystalReport As String

        _mCrRptDocument = New ReportDocument
        spathReport = System.Configuration.ConfigurationManager.AppSettings("RutaReport")

        GetParameters(ds)

        fileCrystalReport = String.Format("{0}\{1}.Rpt", spathReport, _
                                          _msReportName).Replace("\\", "\")

        Return Me.Report(ds, dsDataSource, fileCrystalReport)

    End Function

End Class
