Imports System.IO
Imports System.Xml

Namespace Utitlities.XML

    Public Module XmlUtilitie
        Function ToXmlString(ByVal item As Object) As String

            Dim xmlSerializer As System.Xml.Serialization.XmlSerializer = New System.Xml.Serialization.XmlSerializer(item.GetType())

            Dim xml As String = ""

            Using stringWriter As StringWriter = New StringWriter()

                Using writer As XmlWriter = XmlWriter.Create(stringWriter)

                    xmlSerializer.Serialize(writer, item)
                    xml = stringWriter.ToString()
                End Using

            End Using

            Return xml

        End Function

    End Module

End Namespace
