Imports System.Runtime.CompilerServices

Namespace API.Helpers
    Public Module Date_Extensions

        <Extension()>
        Public Function IsValidDate(ByVal source As Date) As Boolean
            Return Not source.Equals(CType(Nothing, Date))
        End Function

    End Module
End Namespace

