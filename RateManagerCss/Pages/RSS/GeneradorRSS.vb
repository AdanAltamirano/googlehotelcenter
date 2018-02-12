Imports System
Imports System.Xml
Imports System.IO
Public Class GeneradorRSS
#Region " Miembros Privados "
    Private RSS As XmlTextWriter
    Private strTituloArticulo As String
    Private strLinkArticulo As String
    Private strDescripcionArticulo As String
    Private strAutor As String
    Private datFechaPublicacion As Date
    Private strContenido As String
    Private strTituloCanal As String
    Private strLinkCanal As String
    Private strDescripcionCanal As String
    Private strCopyRight As String
    Private strGenerador As String
    Private strWebMaster As String
    Private datLastBuild As Date    
#End Region
#Region " Propiedades Publicas "
    Public Property TituloArticulo() As String
        Get
            Return strTituloArticulo
        End Get
        Set(ByVal value As String)
            strTituloArticulo = value
        End Set
    End Property
    Public Property LinkArticulo() As String
        Get
            Return strLinkArticulo
        End Get
        Set(ByVal value As String)
            strLinkArticulo = value
        End Set
    End Property
    Public Property DescripcionArticulo() As String
        Get
            Return strDescripcionArticulo
        End Get
        Set(ByVal value As String)
            strDescripcionArticulo = value
        End Set
    End Property
    Public Property Autor() As String
        Get
            Return strAutor
        End Get
        Set(ByVal value As String)
            strAutor = value
        End Set
    End Property
    Public Property FechaPublicacion() As Date
        Get
            Return datFechaPublicacion
        End Get
        Set(ByVal value As Date)
            datFechaPublicacion = value
        End Set
    End Property
    Public Property Contenido() As String
        Get
            Return strContenido
        End Get
        Set(ByVal value As String)
            strContenido = value
        End Set
    End Property
    Public Property TituloCanal() As String
        Get
            Return strTituloCanal
        End Get
        Set(ByVal value As String)
            strTituloCanal = value
        End Set
    End Property
    Public Property LinkCanal() As String
        Get
            Return strLinkCanal
        End Get
        Set(ByVal value As String)
            strLinkCanal = value
        End Set
    End Property
    Public Property DescripcionCanal() As String
        Get
            Return strDescripcionCanal
        End Get
        Set(ByVal value As String)
            strDescripcionCanal = value
        End Set
    End Property
    Public Property CopyRight() As String
        Get
            Return strCopyRight
        End Get
        Set(ByVal value As String)
            strCopyRight = value
        End Set
    End Property
    Public Property Generador() As String
        Get
            Return strGenerador
        End Get
        Set(ByVal value As String)
            strGenerador = value
        End Set
    End Property
    Public Property WebMaster() As String
        Get
            Return strWebMaster
        End Get
        Set(ByVal value As String)
            strWebMaster = value
        End Set
    End Property
    Public Property UltimaActualizacion() As Date
        Get
            Return datLastBuild
        End Get
        Set(ByVal value As Date)
            datLastBuild = value
        End Set
    End Property
#End Region
#Region " Metodos Publicos "
    Public Sub New(ByVal stream As System.IO.Stream, ByVal encoding As System.Text.Encoding)
        RSS = New XmlTextWriter(stream, encoding)
        RSS.Formatting = Formatting.Indented
    End Sub
    Public Sub New(ByVal w As System.IO.TextWriter)
        RSS = New XmlTextWriter(w)
        RSS.Formatting = Formatting.Indented
    End Sub
    Public Sub EscribirArticulo()
        With RSS
            .WriteStartElement("item")
            .WriteElementString("title", strTituloArticulo)
            .WriteElementString("link", strLinkArticulo)
            .WriteElementString("description", strDescripcionArticulo)
            .WriteElementString("author", strAutor)
            .WriteElementString("pubDate", datFechaPublicacion)
            .WriteElementString("subject", strContenido)
            .WriteEndElement()
        End With
    End Sub
    Public Sub IniciarCanal()
        With RSS
            .WriteStartDocument()
            .WriteStartElement("rss")
            .WriteAttributeString("version", "2.0")
            .WriteStartAttribute("version", "'2.0'")
            .WriteStartElement("channel")
            .WriteElementString("title", strTituloCanal)
            .WriteElementString("link", strLinkCanal)
            .WriteElementString("description", strDescripcionCanal)
            .WriteElementString("language", "es-ES")
            .WriteElementString("copyright", strCopyRight)
            .WriteElementString("generator", strGenerador)
            .WriteElementString("webmaster", strWebMaster)
            .WriteElementString("lastBuildDate", datLastBuild)
        End With
    End Sub
    Public Sub CerrarCanal()
        With RSS
            .WriteEndElement()
            .WriteEndElement()
            .WriteEndDocument()
            .Flush()
            .Close()
        End With
    End Sub
    Public Function CerrarCanal2() As XmlTextWriter
        Return RSS
    End Function
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class

Public Class GeneradorRSS2
    '/ <summary>
    '/ Escribe el principio de un documento RSS a un XmlTextWriter
    '/ </summary>
    '/ <param name="Escritor">El XmlTextWriter donde se escribirá</param>
    '/ <returns>El XmlTextWriter con la información de cabecera ya escrita</returns>
    Public Function EscribirInicioRSS(ByVal Escritor As XmlTextWriter, ByVal tituloCanal As String, ByVal descripcionCanal As String, ByVal linkCanal As String) As XmlTextWriter
        'Abrimos el documento
        Escritor.WriteStartDocument()
        'Si se desea añadir un comentario al archivo RSS:
        Escritor.WriteComment("Comentario de prueba")
        'El primer elemento a añadir será la declaración del RSS
        Escritor.WriteStartElement("rss")
        Escritor.WriteAttributeString("version", "2.0")
        Escritor.WriteAttributeString("xmlns:MisionChannel", "http://www.hotelesmision.com")
        'Definimos el elemento 'padre'
        Escritor.WriteStartElement("channel")
        'Si queremos añadir algún elemento inicial independiente
        'de los contenidos, como que web somos, copyright, etc:
        Escritor.WriteElementString("title", tituloCanal)
        Escritor.WriteElementString("link", linkCanal)
        Escritor.WriteElementString("description", descripcionCanal)
        'Devolvemos el objeto XML (RSS) con la cabecera ya escrita
        Return Escritor        
    End Function
    '/ <summary>
    '/ Añade un elemento al XmlTextWriter pasado
    '/ </summary>
    '/ <param name="Escritor">El XmlTextWriter donde se escribirá</param>
    '/ <param name="CadenaTitulo">El título del elemento RSS</param>
    '/ <param name="CadenaLink">La URL del elemento actual</param>
    '/ <param name="CadenaDescrip">Descripción del elemento</param>
    '/ <returns>El XmlTextWriter con el nuevo elemento escrito en él</returns>
    Public Function AddElementosRSS(ByVal Escritor As XmlTextWriter, ByVal CadenaTitulo As String, ByVal CadenaLink As String, ByVal CadenaDescrip As String) As XmlTextWriter
        'Abrímos un nuevo elemento
        Escritor.WriteStartElement("item")
        '*************************

        'Añadimos los elementos hijos del actual objeto
        Escritor.WriteElementString("title", CadenaTitulo)
        Escritor.WriteElementString("link", CadenaLink)
        Escritor.WriteElementString("description", CadenaDescrip)
        Escritor.WriteElementString("pubdate", DateTime.Now.ToString("r"))
        '**********************************************

        'Cerramos el elemento
        Escritor.WriteEndElement()
        '********************

        'Devolvemos el objeto XML (RSS) con el elemento añadido
        Return Escritor
        '******************************************************
    End Function

    '/ <summary>
    '/ Añade un elemento al XmlTextWriter pasado
    '/ </summary>
    '/ <param name="Escritor">El XmlTextWriter donde se escribirá</param>
    '/ <param name="CadenaTitulo">El título del elemento RSS</param>
    '/ <param name="CadenaLink">La URL del elemento actual</param>
    '/ <param name="CadenaDescrip">Descripción del elemento</param>
    '/ <param name="BoolDescriptCDATA">Escribe la descripción como CDATA</param>
    '/ <returns>El XmlTextWriter con el nuevo elemento escrito en él</returns>
    Public Function AddElementosRSS(ByVal Escritor As XmlTextWriter, ByVal CadenaTitulo As String, ByVal CadenaLink As String, ByVal CadenaDescrip As String, ByVal BoolDescriptCDATA As Boolean, ByVal pubdate As DateTime) As XmlTextWriter

        'Abrímos un nuevo elemento
        Escritor.WriteStartElement("item")
        '*************************

        'Añadimos los elementos hijos del actual objeto

        'Escritor.WriteElementString("title", CadenaTitulo)
        If (BoolDescriptCDATA = True) Then
            Escritor.WriteStartElement("title")
            Escritor.WriteCData(CadenaTitulo)
            Escritor.WriteEndElement()
        Else
            Escritor.WriteElementString("title", CadenaTitulo)
        End If
        Escritor.WriteElementString("link", CadenaLink)
        'Si así lo hemos especificado, la
        'descripción de añadirá como CDATA
        If (BoolDescriptCDATA = True) Then
            Escritor.WriteStartElement("description")
            Escritor.WriteCData(CadenaDescrip)
            Escritor.WriteEndElement()
        Else
            Escritor.WriteElementString("description", CadenaDescrip)
        End If
        '*********************************
        Escritor.WriteElementString("pubdate", pubdate)
        '**********************************************

        'Cerramos el elemento
        Escritor.WriteEndElement()
        '********************

        'Devolvemos el objeto XML (RSS) con el elemento añadido
        Return Escritor
        '******************************************************
    End Function

    '/ <summary>
    '/ Finalmente, cerramos los elementos del RSS y el documento en sí
    '/ </summary>
    '/ <param name="Escritor">El XmlTextWriter donde se escribirá</param>
    '/ <returns>El XmlTextWriter con todos los elementos cerrados</returns>
    Public Function EscribirFinRSS(ByVal Escritor As XmlTextWriter) As XmlTextWriter

        'Cerramos los elementos abiertos y el documento
        Escritor.WriteEndElement()
        Escritor.WriteEndElement()
        Escritor.WriteEndDocument()
        '**********************************************

        'Devolvemos el objeto con todo el documento ya escrito
        Return Escritor
        '*****************************************************
    End Function
End Class


