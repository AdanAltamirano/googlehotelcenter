Imports System.Configuration

Public Class ClsRuWS
    Dim rules As RulesWebService.RulesWebService

    Public Sub CallRUWS(ByVal HotelId As Integer, ByVal RoomTypeId As Integer, ByVal IniDate As Date, ByVal EndDate As Date)
        rules = New RulesWebService.RulesWebService
        rules.Url = ConfigurationManager.AppSettings("RulesWebService")
        rules.CallRules(HotelId, RoomTypeId, IniDate, EndDate)
    End Sub
End Class
