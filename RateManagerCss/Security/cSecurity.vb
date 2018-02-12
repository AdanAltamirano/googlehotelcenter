Option Strict On
Option Explicit On 

Imports Portal.General.Facade
Imports Portal.General.Common.Data


Imports System.Web

Imports System.Web.Security

Public Class cSecurity

    Public Shared Function createUserCookie(ByVal identity As Integer, ByVal expiresMinutes As Integer, ByVal persist As Boolean, ByVal UserInfo As String()) As HttpCookie
        Dim tkt As FormsAuthenticationTicket
        Dim cookiestr As String
        Dim ck As HttpCookie

        tkt = New FormsAuthenticationTicket(1, _
                  identity.ToString, _
                  DateTime.Now(), _
                  DateTime.Now.AddMinutes(expiresMinutes), _
                  persist, _
                  Join(UserInfo, ","))

        'Cookie encriptada
        cookiestr = FormsAuthentication.Encrypt(tkt)
        Dim CookieName As String = FormsAuthentication.FormsCookieName() & "UI"

        ck = New HttpCookie(CookieName, cookiestr)

        If (persist) Then
            ck.Expires = tkt.Expiration
        End If

        ck.Path = FormsAuthentication.FormsCookiePath()

        Return ck
    End Function

    Public Shared Function createCookieUsuarioHotel(ByVal identity As Integer, ByVal expiresMinutes As Integer, ByVal persist As Boolean, ByVal UserInfo As String()) As HttpCookie
        Dim tkt As FormsAuthenticationTicket
        Dim cookiestr As String
        Dim ck As HttpCookie
        Dim roleListArray As String()


        tkt = New FormsAuthenticationTicket(1, _
           identity.ToString, _
           DateTime.Now(), _
           DateTime.Now.AddMinutes(expiresMinutes), _
           persist, _
           Join(UserInfo, ","))


        'Cookie encriptada
        cookiestr = FormsAuthentication.Encrypt(tkt)
        Dim CookieName As String = FormsAuthentication.FormsCookieName() & "UI"

        ck = New HttpCookie(CookieName, cookiestr)

        If (persist) Then
            ck.Expires = tkt.Expiration
        End If

        ck.Path = FormsAuthentication.FormsCookiePath()

        Return ck
    End Function
    Public Shared Function createCookie(ByVal identity As Integer, ByVal expiresMinutes As Integer, ByVal persist As Boolean) As HttpCookie
        Dim tkt As FormsAuthenticationTicket
        Dim cookiestr As String
        Dim ck As HttpCookie
        Dim roleListArray As String()
        Dim isUserChange As Boolean = False

        With New cUserSystem
            roleListArray = .GetRolesById(identity)
        End With

        'Buscar si el usuario tiene el rol de UserChange
        For Each rol As String In roleListArray
            If rol = "UserChain" Then
                isUserChange = True
                Exit For
            End If
        Next

        If Not isUserChange Then ' si no tiene rol userChange no se hace nada a los roles
            tkt = New FormsAuthenticationTicket(1, _
                      identity.ToString, _
                      DateTime.Now(), _
                      DateTime.Now.AddMinutes(expiresMinutes), _
                      persist, _
                      Join(roleListArray, ","))
        Else ' si tiene el rol userChange quitar los demas roles en cado de tenerlos
            tkt = New FormsAuthenticationTicket(1, _
                      identity.ToString, _
                      DateTime.Now(), _
                      DateTime.Now.AddMinutes(expiresMinutes), _
                      persist, _
                      "UserChain")
        End If





        'Cookie encriptada
        cookiestr = FormsAuthentication.Encrypt(tkt)

        ck = New HttpCookie(FormsAuthentication.FormsCookieName(), cookiestr)

        If (persist) Then
            ck.Expires = tkt.Expiration
        End If

        ck.Path = FormsAuthentication.FormsCookiePath()

        Return ck
    End Function
    Public Shared Function createUserHCookie(ByVal identity As Integer, ByVal expiresMinutes As Integer, ByVal persist As Boolean) As HttpCookie
        Dim tkt As FormsAuthenticationTicket
        Dim cookiestr As String
        Dim ck As HttpCookie

        tkt = New FormsAuthenticationTicket(1, _
                  identity.ToString, _
                  DateTime.Now(), _
                  DateTime.Now.AddMinutes(expiresMinutes), _
                  persist, _
                  "UsuarioHotel")

        'Cookie encriptada
        cookiestr = FormsAuthentication.Encrypt(tkt)

        ck = New HttpCookie(FormsAuthentication.FormsCookieName(), cookiestr)

        If (persist) Then
            ck.Expires = tkt.Expiration
        End If

        ck.Path = FormsAuthentication.FormsCookiePath()

        Return ck
    End Function

    Public Shared Function ValidateUser(ByVal userName As String, ByVal passWord As String) As UserData
        Dim user As UserData
        With New cUserSystem
            Dim pass As Byte() = crypto.hashedPassword(passWord)
            Dim passRAS As String = crypto.EncryptString128Bit(passWord, crypto.PublicKey)
            user = .GetUserByEmail(userName, pass, passRAS)
        End With
        Return user
    End Function
    Public Shared Function GetRolesUser(ByVal Id As Integer) As String()
        Dim user() As String
        With New cUserSystem
            user = .GetRolesById(Id)
        End With
        Return user
    End Function
    Public Shared Function ValidateUsuario(ByVal userName As String, ByVal passWord As String) As UsuarioHotelData
        Dim user As UsuarioHotelData
        With New UsuarioHotelFacade
            Dim passRAS As String = crypto.EncryptString128Bit(passWord, crypto.PublicKey)
            user = .GetUserByName(userName)
        End With
        Return user
    End Function



End Class


