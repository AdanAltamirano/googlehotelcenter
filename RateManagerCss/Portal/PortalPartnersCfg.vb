Imports System.Web
Imports System.xml
Imports System.Configuration.ConfigurationManager

Public Class PortalPartnersCfg
    Inherits PortalPartners
    'Implementar el cache de datos con dependencia al archivo.
    'La configuraion es leida de un archivo xml y mantenida en el cache con dependencia a archivo
    Private Const _FilePath As String = "~/Portal/PortalPartners.xml"
    Private Const _CacheName As String = "PortalPartners"

    Private Const _ReWriteFile As String = "PortalReWriteRules.xml"
    Private Const _ReWriteCacheName As String = "PortalReWriteRules"

    Private _Partner As PortalPartners.PortalPartnerRow
    Private _PartnerId As Long = 0

    Public ReadOnly Property SharedFiles() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                Return _Partner.SharedFiles
            End If
        End Get
    End Property
    Public ReadOnly Property AllowAgency() As String
        Get
            Try
                If _Partner Is Nothing Then
                    Return False
                Else
                    Return CType(_Partner.AllowAgency, Boolean)
                End If
            Catch ex As Exception
            End Try
            Return False
        End Get
    End Property

    Public ReadOnly Property CurrencyCode() As String
        Get
            If _Partner Is Nothing Then
                Return "USD"
            Else
                Return _Partner.CurrencyCode
            End If
        End Get
    End Property

    Public ReadOnly Property StyleSheets() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                Return _Partner.StyleSheets
            End If
        End Get
    End Property

    Public ReadOnly Property Key() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                Return _Partner.Key
            End If
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                Return _Partner.Name
            End If
        End Get
    End Property
    Public ReadOnly Property DomainName() As String
        Get
            Try
                If Not _Partner Is Nothing Then
                    Return _Partner.DomainName
                End If
            Catch ex As Exception
            End Try
            Return ""
        End Get
    End Property

    Public ReadOnly Property SecureSite() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                Return _Partner.SecureSite
            End If
        End Get
    End Property

    Public ReadOnly Property NonSecureSite() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                Return _Partner.Site
            End If
        End Get
    End Property
    Public ReadOnly Property Rss() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                Return _Partner.RSS
            End If
        End Get
    End Property
    Public ReadOnly Property ImagesSystem() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                Return _Partner.ImagesSystem
            End If
        End Get
    End Property
    Public ReadOnly Property UrlSite() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                If HttpContext.Current.Request.IsSecureConnection Then
                    Return _Partner.SecureSite
                Else
                    Return _Partner.Site
                End If

            End If
        End Get
    End Property
    Public ReadOnly Property EmailHeader() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                Return _Partner.EmailHeader
            End If
        End Get
    End Property
    Public ReadOnly Property EmailFooter() As String
        Get
            If _Partner Is Nothing Then
                Return ""
            Else
                Return _Partner.EmailFooter
            End If
        End Get
    End Property
    Public ReadOnly Property EmailReservas() As String
        Get
            If Not _Partner Is Nothing Then
                Try
                    Return _Partner.EmailReservas
                Catch ex As Exception
                End Try
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property RedirectionRules() As String
        Get
            If Not _Partner Is Nothing Then
                Try
                    Return _Partner.RedirectionFile
                Catch ex As Exception
                End Try
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property PortalTrackingCodeNonSecure() As String
        Get
            If Not _Partner Is Nothing Then
                Try
                    Return _Partner.PortalTrackingCodeNonSecure
                Catch ex As Exception
                End Try
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property PortalTrackingCodeSecure() As String
        Get
            If Not _Partner Is Nothing Then
                Try
                    Return _Partner.PortalTrackingCodeSecure
                Catch ex As Exception
                End Try
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property SecureCertificationScript() As String
        Get
            If Not _Partner Is Nothing Then
                Try
                    Return _Partner.SecureCertificationScript
                Catch ex As Exception
                End Try
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property PartnerId() As Integer
        Get
            Try
                Return _Partner.Id
            Catch ex As Exception
                'PortalServiceTracer.ServiceTracer("Error al leeer el partner id, tomando partner 0", PortalServiceTacerErrorTypes.Severity)
                Return 0
            End Try

        End Get
        'Set(ByVal Value As Integer)
        '    _PartnerId = Value
        '    Dim dr() As DataRow
        '    dr = Me.PortalPartner.Select(Me.PortalPartner.IdColumn.ColumnName & "='" & Value & "'")
        '    If dr.Length > 0 Then
        '        _Partner = dr(0)
        '    Else
        '        If _Partner Is Nothing Then
        '            PortalServiceTracer.ServiceTracer("Falta Partner default en PortalPartners.xml ", PortalServiceTacerErrorTypes.Warning)
        '        End If
        '    End If
        'End Set
    End Property
    Public ReadOnly Property BlockActivites() As Boolean
        Get
            Try
                Return CType(_Partner.BlockActivies, Boolean)
            Catch ex As Exception
            End Try
            Return False
        End Get
    End Property

    Public ReadOnly Property DefaultActivitiesCityIATA() As String
        Get
            If Not _Partner Is Nothing Then
                Try
                    Return _Partner.DefaultActivitiesCityIATA
                Catch ex As Exception
                End Try
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property DefaultActivitiesCityRefPoint() As String
        Get
            If Not _Partner Is Nothing Then
                Try
                    Return _Partner.DefaultActivitiesCityRefPoint
                Catch ex As Exception
                End Try
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property DefaultActivitiesCityName() As String
        Get
            If Not _Partner Is Nothing Then
                Try
                    Return _Partner.DefaultActivitiesCityName
                Catch ex As Exception
                End Try
            End If
            Return ""
        End Get
    End Property

    Private Sub LookForPartnerId()
        Dim dr As PortalPartnerRow
        Dim Id As Long = -1
        Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri.ToString
        Try
            For Each dr In Me.PortalPartner.Rows
                _Partner = dr
                If HttpContext.Current.Request.IsSecureConnection Then
                    If path.ToLower.IndexOf(dr.SecureKey.ToLower) = 0 Then
                        Id = dr.Id
                        _Partner = dr
                        Exit For
                    End If
                Else
                    If path.ToLower.IndexOf(dr.Key.ToLower) = 0 Then
                        Id = dr.Id
                        _Partner = dr
                        Exit For
                    End If
                End If

            Next
            ' If Id = -1 Then Return 0
            'Return Id
        Catch ex As Exception
            'Ocurrio un error al parsear al partner.
            'PortalServiceTracer.ServiceTracer("Error al parsear un Partner Id", PortalServiceTacerErrorTypes.Severity)
        End Try
    End Sub

    Public Sub LoadPartnerById(ByVal id As Long)
        Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri.ToString
        Try
            Dim dr() As DataRow
            dr = Me.PortalPartner.Select(Me.PortalPartner.IdColumn.ColumnName & "='" & id & "'")
            If dr.Length > 0 Then
                _Partner = dr(0)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub New()
        Call LoadMe()
        'Carga los valores por default
        Call LookForPartnerId()
        'Dim drd() As DataRow
        'drd = Me.PortalPartner.Select(Me.PortalPartner.IdColumn.ColumnName & "='" & Me.PartnerId & "'")
        'If drd.Length = 0 Then
        '    PortalServiceTracer.ServiceTracer("Falta Partner " & PartnerId & " tomando default en PortalPartners.xml ", PortalServiceTacerErrorTypes.Warning)
        '    _Partner = Me.PortalPartner.Rows(0)
        'Else
        '    _Partner = drd(0)
        'End If
    End Sub

    Public Function isValid() As Boolean
        Return (Not _Partner Is Nothing)
    End Function

    Private Sub LoadMe()
        Dim cfgPath As String = HttpContext.Current.Server.MapPath(ParseRootPath(_FilePath))
        If HttpContext.Current.Cache(_CacheName) Is Nothing OrElse HttpContext.Current.Cache(_CacheName).ToString = "" Then
            Me.ReadXml(cfgPath)
            HttpContext.Current.Cache.Insert(_CacheName, Me.GetXml.ToString, New System.Web.Caching.CacheDependency(cfgPath))
        Else
            Dim cm As XmlTextReader = New XmlTextReader(HttpContext.Current.Cache(_CacheName).ToString, XmlNodeType.Element, Nothing)
            Me.ReadXml(cm)
        End If
    End Sub

    Private Function ParseRootPath(ByVal path As String) As String
        If (path.IndexOf("~") = 0) Then
            'si es en raiz.
            If System.Web.HttpContext.Current.Request.ApplicationPath = "/" Then
                path = path.Replace("~", "")
            Else 'esta en un directorio virtual
                path = path.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath)
            End If
            path = path.Replace("//", "/") 'por si trae doble //
        Else
            'no tiene la tilde lo dejamos intacto
        End If
        Return path
    End Function



End Class