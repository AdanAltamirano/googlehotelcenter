Imports System.Net.Http
Imports System.Runtime.CompilerServices

Namespace API.Helpers
    Public Module HttpRequestMessage_Extensions
        ''' <summary>
        ''' Optiene el valor del header AcceptLanguage y lo transforma a los valores de UV
        ''' </summary>
        ''' <param name="source"></param>
        ''' <returns>Si el valor es 'es' entonces devolverá 1 caso contrario se devolverá un 2</returns>
        <Extension()>
        Public Function GetLanguageUV(ByVal source As HttpRequestMessage) As Integer
            Return If(source.Headers.AcceptLanguage.FirstOrDefault()?.Value.ToLower() = "es", 1, 2)
        End Function
    End Module
End Namespace

