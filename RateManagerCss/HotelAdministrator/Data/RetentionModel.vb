Public Class RetentionModel
    Public Class Messages
        Private m_idMensaje As String
        Private m_idConfiguracion As String
        Private m_mensaje As String
        Private m_icono As String
        Private m_fechaInicio As String
        Private m_fechaFin As String
        Private m_orden As String
        Private m_rutas As String
        Private m_tipoMensaje As String
        Private m_duracion As String
        Private m_idioma As String

        Public Property IdMessage() As String
            Get
                Return m_idMensaje
            End Get
            Set(ByVal value As String)
                m_idMensaje = value
            End Set
        End Property

        Public Property IdConfiguration() As String
            Get
                Return m_idConfiguracion
            End Get
            Set(ByVal value As String)
                m_idConfiguracion = value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return m_mensaje
            End Get
            Set(ByVal value As String)
                m_mensaje = value
            End Set
        End Property

        Public Property Icon() As String
            Get
                Return m_icono
            End Get
            Set(ByVal value As String)
                m_icono = value
            End Set
        End Property

        Public Property StartDate() As String
            Get
                Return m_fechaInicio
            End Get
            Set(ByVal value As String)
                m_fechaInicio = value
            End Set
        End Property

        Public Property EndDate() As String
            Get
                Return m_fechaFin
            End Get
            Set(ByVal value As String)
                m_fechaFin = value
            End Set
        End Property

        Public Property Order() As String
            Get
                Return m_orden
            End Get
            Set(ByVal value As String)
                m_orden = value
            End Set
        End Property

        Public Property Rute() As String
            Get
                Return m_rutas
            End Get
            Set(ByVal value As String)
                m_rutas = value
            End Set
        End Property

        Public Property MessageType() As String
            Get
                Return m_tipoMensaje
            End Get
            Set(ByVal value As String)
                m_tipoMensaje = value
            End Set
        End Property

        Public Property Duration() As String
            Get
                Return m_duracion
            End Get
            Set(ByVal value As String)
                m_duracion = value
            End Set
        End Property

        Public Property Language() As String
            Get
                Return m_idioma
            End Get
            Set(ByVal value As String)
                m_idioma = value
            End Set
        End Property
    End Class

    Public Class PredefinedMessages
        Private m_idMensaje As String
        Private m_tipo As String
        Private m_mensaje As String
        Private m_idioma As String

        Public Property IdMessage() As String
            Get
                Return m_idMensaje
            End Get
            Set(ByVal value As String)
                m_idMensaje = value
            End Set
        End Property

        Public Property Type() As String
            Get
                Return m_tipo
            End Get
            Set(ByVal value As String)
                m_tipo = value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return m_mensaje
            End Get
            Set(ByVal value As String)
                m_mensaje = value
            End Set
        End Property

        Public Property Language() As String
            Get
                Return m_idioma
            End Get
            Set(ByVal value As String)
                m_idioma = value
            End Set
        End Property
    End Class
End Class
