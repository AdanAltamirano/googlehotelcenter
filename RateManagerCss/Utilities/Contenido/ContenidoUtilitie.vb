Imports APIServices.Models

Namespace Utitlities.Contenido
    Public Module ContenidoUtilitie
        Function GetIdElementByModule(ByVal modulo As Integer) As Integer
            Dim idElement As Integer = 0

            Using dbContext As New OzUniEntities()
                idElement = dbContext.Elementos.First(Function(elemento) elemento.IdModulo = modulo).IdElemento
            End Using

            Return idElement
        End Function
    End Module
End Namespace
