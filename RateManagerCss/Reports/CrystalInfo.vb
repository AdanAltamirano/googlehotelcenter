Imports System
Imports System.Threading
Imports System.Globalization
Imports System.Web.Security
Imports System.Runtime.Serialization
Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports System.Web.HttpContext
Imports System.Xml

Public Class CrystalSession
    Inherits System.Web.UI.Page

    Public CInfo As CrystalInfoISerializable
    Private SESSION_CRYSTAL As String = "SessionCrystal"

    Public Property CCrystalInfo() As CrystalInfoISerializable
        Get
            If Session(SESSION_CRYSTAL) Is Nothing Then
                Session(SESSION_CRYSTAL) = New CrystalInfoISerializable
            End If
            Return Session(SESSION_CRYSTAL)
        End Get
        Set(ByVal Value As CrystalInfoISerializable)
            Session(SESSION_CRYSTAL) = Value
        End Set
    End Property

    Public Function CrystalReportSetReport(ByVal name As String, _
                                           ByVal description As String, _
                                           ByRef ds As Crystal_1_0) As Short
        Dim dr As Crystal_1_0.ReportRow

        dr = ds.Report.NewReportRow
        dr.Name = name
        dr.Description = description
        dr.Culture = PortalCulture.GetCulture.Name
        ds.Report.Rows.Add(dr)
        Return 0
    End Function

    Public Function CrystalReportSetHeader(ByVal title As String, ByVal subTitle As String, _
                                           ByVal subTitle2 As String, _
                                           ByVal description As String, _
                                           ByRef ds As Crystal_1_0) As Short
        Dim drh As Crystal_1_0.HeaderRow

        drh = ds.Header.NewHeaderRow
        drh.Title = title
        drh.subTitle = subTitle
        drh.subTitle2 = subTitle2
        drh.Description = description
        ds.Header.Rows.Add(drh)
        Return 0
    End Function

    Public Function SetQueryParameters(ByVal name As String, ByVal value As String, ByRef ds As Crystal_1_0) As Short
        Dim dr As Crystal_1_0.QueryParametersRow
        dr = ds.QueryParameters.NewQueryParametersRow
        dr("name") = name
        dr("value") = value
        ds.QueryParameters.Rows.Add(dr)

        Return 0
    End Function

    Public Function SetResources(ByVal id As String, ByVal value As String, ByRef ds As Crystal_1_0) As Short
        Dim dr As Crystal_1_0.ResourcesRow
        dr = ds.Resources.NewResourcesRow
        dr("id") = id
        dr("value") = value
        ds.Resources.Rows.Add(dr)
        Return 0
    End Function

End Class

<Serializable()> Public Class CrystalInfoISerializable
    Implements ISerializable

    Public dsCrystal As DataSet

    Public Sub New()
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        With info
            Me.dsCrystal = .GetValue("dsCrystal", GetType(DataSet))
        End With
    End Sub

    Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        With info
            .AddValue("dsCrystal", Me.dsCrystal, GetType(DataSet))
        End With
    End Sub

End Class
