Imports System
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes

#Region " Tipos de Tarjeta "

Public Enum tTarjeta
    ccDinerClub = 0
    ccAmericanExpress = 1
    ccJCB = 2
    ccCarteBlanche = 3
    ccVisa = 4
    ccMasterCard = 5
    ccAustralianBankCard = 6
    ccDiscover = 7
End Enum
#End Region
#Region " Clase Tarjeta "

<Guid("33836EAA-9593-4cda-9AFF-F66C6CA4AD38")> _
Public Class Tarjetas
    Public NameTarget() As String = {"Diners Club", "American Express", "JCB", "Carte Blanche", "Visa", "Master Card", "Australian BankCard", "Discover"}
    Public Const Nombres As String = "Diners Club,American Express,JCB,Carte Blanche,Visa,Master Card,Australian BankCard,Discover"
    Private Numero As String
    Private Validacion As Short
    Private EsValida As Boolean

    Public Enum CreditCardType
        Undefined = -1
        DinerClub = 0
        AmericanExpress = 1
        JCB = 2
        CarteBlanche = 3
        Visa = 4
        MasterCard = 5
        AustralianBankCard = 6
        Discover = 7
    End Enum

    Public Enum CreditCardCode
        XX = -1
        DC = 0
        AX = 1
        JC = 2
        CB = 3
        VI = 4
        CA = 5
        AB = 6
        DS = 7
        MC = 50
        IK = 500
    End Enum


    Public Sub New()
    End Sub

    Public Sub New(ByVal number As String, ByVal validation As Short)
        Numero = number
        Validacion = validation
        ValidaTarjeta()
    End Sub

    Public ReadOnly Property IsValid()
        Get
            Return EsValida
        End Get
    End Property

    Private Sub ValidaTarjeta()
        EsValida = False
        If Not IsNumeric(Numero) Then Return

        Dim NumberLength As Integer
        Dim CardType As Byte
        Dim ShouldLength As Short
        Dim Missing As Short

        NumberLength = Len(Numero)
        Select Case Left(Numero, 4)
            Case 3000 To 3059, 3600 To 3699, 3800 To 3889
                CardType = 0 ' "Diners Club"
                ShouldLength = 14
            Case 3400 To 3499, 3700 To 3799
                CardType = 1 '"American Express"
                ShouldLength = 15
            Case 3528 To 3589
                CardType = 2 '"JCB"
                ShouldLength = 16
            Case 3890 To 3899
                CardType = 3 '"Carte Blanche"
                ShouldLength = 14
            Case 4000 To 4999
                CardType = 4 ' "Visa"
                Select Case NumberLength
                    Case Is > 14
                        ShouldLength = 16
                    Case Is < 14
                        ShouldLength = 13
                    Case Else
                        Return
                End Select
            Case 5100 To 5599
                CardType = 5 ' "MasterCard"
                ShouldLength = 16
            Case 5610
                CardType = 6 ' "Australian BankCard"
                ShouldLength = 16
            Case 6011
                CardType = 7 '"Discover"
                ShouldLength = 16
            Case Else
                Return
        End Select
        If CardType <> (Validacion) Then Return '2.1)'es del tipo seleccionado
        If NumberLength <> ShouldLength Then Return '3) tiene la longitud correnta
        Try             '4) Verifica que pase la modulacion 10?
            If Mod10Solution() = False Then Return
            EsValida = True
        Catch e As Exception
        End Try
    End Sub

    Public Function Mod10Solution() As Boolean
        Dim NumberLength As Byte
        Dim Location As Byte
        Dim Checksum As Short
        Dim Digit As Byte

        NumberLength = Len(Numero)

        For Location = 2 - (NumberLength Mod 2) To NumberLength Step 2
            Checksum = Mid(Numero, Location, 1) + Checksum
        Next Location

        For Location = (NumberLength Mod 2) + 1 To NumberLength Step 2
            Digit = Mid(Numero, Location, 1) * 2
            If Digit < 10 Then
                Checksum = Digit + Checksum
            Else
                Checksum = Digit - 9 + Checksum
            End If
        Next Location

        Return (Checksum Mod 10 = 0)
    End Function

    Public Shared Function GetCardType(ByVal code As String) As CreditCardType

        Dim values As Integer() = [Enum].GetValues(GetType(CreditCardCode))
        Dim names As String() = [Enum].GetNames(GetType(CreditCardCode))
        Dim value As Integer = values(Array.IndexOf(names, code.ToUpper()))
        While (value >= 10)
            value = value / 10
        End While

        Return value

    End Function

    Public Shared Function GetCardCode(ByVal type As CreditCardType) As String

        Dim code As CreditCardCode = type

        Return code.ToString()

    End Function

End Class

#End Region


