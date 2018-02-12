Imports System.Web
Imports System.Web.Services
Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Portal.Catalogos.Facade
Imports Portal.Catalogos.Common.Data
Imports System.Collections.Generic

Public Class LocationItems
    Implements System.Web.IHttpHandler, IRequiresSessionState

    Private Const TYPEPARAM As String = "type"
    Private Const PARENTPARAM As String = "parent"
    Private Const LANGPARAM As String = "lang"
    Private Const TARGETPARAM As String = "target"

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        With context
            Dim result As New StringBuilder()
            Dim target As String = If(.Request.Params(TARGETPARAM) IsNot Nothing, .Request.Params(TARGETPARAM), String.Empty)
            Try
                If .Request.QueryString(TYPEPARAM) IsNot Nothing Then
                    Select Case .Request.QueryString(TYPEPARAM).ToLower()
                        Case "country"
                            Dim lang As Integer = 1
                            If .Request.QueryString(LANGPARAM) IsNot Nothing Then Integer.TryParse(.Request.QueryString(LANGPARAM), lang)
                            result.Append(Me.GetDictionaryString(Me.GetCountries(lang), target))
                        Case "state"
                            If .Request.QueryString(PARENTPARAM) IsNot Nothing Then
                                result.Append(Me.GetDictionaryString(Me.GetStates(.Request.QueryString(PARENTPARAM)), target))
                            End If
                        Case "district"
                            If .Request.QueryString(PARENTPARAM) IsNot Nothing Then
                                result.Append(Me.GetDictionaryString(Me.GetDistricts(.Request.QueryString(PARENTPARAM)), target))
                            End If
                        Case "city"
                            If .Request.QueryString(PARENTPARAM) IsNot Nothing Then
                                result.Append(Me.GetDictionaryString(Me.GetCities(.Request.QueryString(PARENTPARAM)), target))
                            End If
                        Case "area"
                            If .Request.QueryString(PARENTPARAM) IsNot Nothing Then
                                result.Append(Me.GetDictionaryString(Me.GetAreas(.Request.QueryString(PARENTPARAM)), target))
                            End If
                    End Select
                End If
            Catch ex As Exception
                result.Append("[{""id"":""0"",""name"":""" + PortalCulture.GetString("00821", False) + """, ""target"":""" + target + """}]")
            End Try

            .Response.ContentType = "application/json"
            .Response.ContentEncoding = Encoding.UTF8
            .Response.Write(If(result.ToString().Trim().Length = 0, "Empty result", result.ToString()))

        End With

    End Sub

    Private Function GetDictionaryString(ByVal dictionary As Dictionary(Of String, String), Optional ByVal target As String = "") As String
        Dim result As String = String.Empty
        If dictionary.Count > 0 Then
            result += "["
            Dim sep As String = ""
            For Each item As KeyValuePair(Of String, String) In dictionary
                result += sep + "{"
                result += """id"":""" + item.Key + """,""name"":""" + item.Value + """, ""target"":""" + target + """"
                result += "}"
                sep = ","
            Next
            result += "]"
        End If
        Return result
    End Function

    Private Function GetCountries(ByVal lang As Integer) As Dictionary(Of String, String)

        GetCountries = New Dictionary(Of String, String)
        Dim strErr As String = String.Empty
        Dim data As clsCommonPaises = (New clsFacadePaises).GetPaises(lang)

        If data.Tables.Contains(clsCommonPaises.TABLA_PAISES) Then
            For Each row As DataRow In data.Tables(clsCommonPaises.TABLA_PAISES).Rows
                GetCountries.Add(row(clsCommonPaises.FLD_IDPAIS), row(clsCommonPaises.FLD_NOMBRE))
            Next
        End If

    End Function


    Private Function GetStates(ByVal parent As String) As Dictionary(Of String, String)
        GetStates = New Dictionary(Of String, String)
        If parent.Trim().Length > 0 Then
            Dim strErr As String = String.Empty
            Dim data As clsCommonEstados = (New clsFacadeEstados).GetByPais(parent, strErr)

            If data.Tables.Contains(clsCommonEstados.TABLA_ESTADOS) Then
                For Each row As DataRow In data.Tables(clsCommonEstados.TABLA_ESTADOS).Rows
                    GetStates.Add(row(clsCommonEstados.FLD_IDESTADO), row(clsCommonEstados.FLD_NOMBRE))
                Next
            End If
        End If
    End Function


    Private Function GetDistricts(ByVal parent As String) As Dictionary(Of String, String)
        GetDistricts = New Dictionary(Of String, String)
        If parent.Trim().Length > 0 Then
            Dim strErr As String = String.Empty
            Dim data As clsCommonMunicipios = (New clsFacadeMunicipios).GetByIdEstado(parent, strErr)


            If data.Tables.Contains(clsCommonMunicipios.TABLA_MUNICIPIOS) Then
                For Each row As DataRow In data.Tables(clsCommonMunicipios.TABLA_MUNICIPIOS).Rows
                    GetDistricts.Add(row(clsCommonMunicipios.FLD_IDMUNICIPIO), row(clsCommonMunicipios.FLD_NOMBRE))
                Next
            End If
        End If
    End Function

    Private Function GetCities(ByVal parent As String) As Dictionary(Of String, String)
        GetCities = New Dictionary(Of String, String)
        If parent.Trim().Length > 0 Then
            Dim strErr As String = String.Empty
            Dim data As clsCommonCiudades = (New clsFacadeCiudades).GetByIdMunicipio(parent, strErr)

            If data.Tables.Contains(clsCommonCiudades.TABLA_CIUDADES) Then
                For Each row As DataRow In data.Tables(clsCommonCiudades.TABLA_CIUDADES).Rows
                    GetCities.Add(row(clsCommonCiudades.FLD_IDCIUDAD), row(clsCommonCiudades.FLD_NOMBRE))
                Next

                If GetCities.Count = 0 Then
                    GetCities.Add(0, PortalCulture.GetString("00821"))
                End If
            End If
        End If
        'GetCities.Add("0", PortalCulture.GetString("00821", False))
    End Function

    Private Function GetAreas(ByVal parent As String) As Dictionary(Of String, String)
        GetAreas = New Dictionary(Of String, String)

        If parent.Trim().Length > 0 Then
            Dim strErr As String = String.Empty
            Dim data As clsCommonAreas = (New clsFacadeAreas).GetByIdCiudad(parent, strErr)

            If data IsNot Nothing AndAlso data.Tables.Contains(clsCommonAreas.TABLA_AREAS) Then
                For Each row As DataRow In data.Tables(clsCommonAreas.TABLA_AREAS).Rows
                    GetAreas.Add(row(clsCommonAreas.FLD_IDAREA), row(clsCommonAreas.FLD_NOMBRE))
                Next
            End If
        End If

    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class