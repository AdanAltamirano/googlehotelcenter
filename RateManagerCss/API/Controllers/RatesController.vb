Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Common
Imports RateManager.API.Helpers
Imports RateManager.API.Models

Namespace API.Controllers
    <RoutePrefix("api/hotels/{HotelId:int}/rates")>
    Public Class RatesController
        Inherits ShurikenController

        Public Service As New RatesService

        'GET api/hotels/1/rates
        <Route(""), HttpGet>
        Public Function GetByRatePlan(HotelId As Integer, <FromUri> Req As RatesByPlanRQ) As IEnumerable(Of DTO.RatesByRatePlan)
            Return Service.FindGroupedByRatePlan(HotelId, Req.StartDate, Req.EndDate, Req.RoomId, Request.GetLanguageUV())
        End Function

        'GET api/hotels/1/rates/2345/daily/2019-09-01
        <Route("{RateId:int}/daily/{day:datetime}"), HttpGet>
        Public Function GetDayRate(HotelId As Integer, RateId As Integer, day As Date) As DTO.DailyRateDetail
            Return Service.FindDayRateDetail(RateId, day)
        End Function

        'POST api/hotels/1/rates/
        <Route(""), HttpPost>
        Public Function RateUpdate(<FromBody> RQ As RateUpdateRQ, HotelId As Integer) As Net.Http.HttpResponseMessage
            Dim serviceRQ As DTO.RateUpdateRQ = MappingRateUpdateRQ(RQ)

            If Service.AddRate(serviceRQ) Then
                NoContent()
            End If
            BadRequest(KeyValuePair.Create("Error", "Error"))
        End Function

        Private Function MappingRateUpdateRQ(RQ As RateUpdateRQ) As DTO.RateUpdateRQ
            Dim ServiceRQ As New DTO.RateUpdateRQ With {
                .HotelId = RQ.HotelId,
                .RoomId = RQ.RoomId,
                .RateId = RQ.RateId,
                .RatePlanId = RQ.RatePlanId,
                .RateCode = RQ.RateCode,
                .StartDate = RQ.StartDate,
                .EndDate = RQ.EndDate,
                .IsOccupancyRate = RQ.IsOccupancyRate
                }

            Dim ServiceRQPrices As New List(Of DTO.DailyRateDetailPrice)
            For Each Price As DailyRateDetailPrice In RQ.Prices
                Dim ServiceRQPrice As New DTO.DailyRateDetailPrice With {
                    .Type = Price.Type,
                    .Price = Price.Price,
                    .Occupation = Price.Occupation,
                    .RateId = Price.RateId
                    }
                ServiceRQPrices.Add(ServiceRQPrice)
            Next
            ServiceRQ.Prices = ServiceRQPrices

            Dim ServiceRQRules As New DTO.RateUpdateRQRules With {
                .UseDefaultRules = RQ.Rules.UseDefaultRules,
                .MinLOS = RQ.Rules.MinLOS,
                .MaxLOS = RQ.Rules.MaxLOS,
                .MaxAdvanceBooking = RQ.Rules.MaxAdvnaceBooking,
                .MinAdvanceBooking = RQ.Rules.MinAdvnaceBooking,
                .ExceptionDays = GetDaysOfWeekString(RQ.Rules.ExceptionDays),
                .NoArrival = GetDaysOfWeekString(RQ.Rules.NoArrival)
                }
            ServiceRQ.Rules = ServiceRQRules

            If Not RQ.Promotion Is Nothing Then
                Dim serviceRQPromotion As New DTO.RateUpdateRQPromotion With {
                    .Discount = RQ.Promotion.Discount,
                    .EnglishDescription = RQ.Promotion.EnglishDescription,
                    .SpanishDescription = RQ.Promotion.SpanishDescription
                    }
                ServiceRQ.Promotion = serviceRQPromotion
            End If

            If Not RQ.GuestsRestrictions Is Nothing Then
                Dim ServiceRQGuestsRestrictions As New DTO.RateUpdateRQGuestsRestriction With {
                    .Children = RQ.GuestsRestrictions.Children,
                    .MaxAdults = RQ.GuestsRestrictions.MaxAdults,
                    .MinAdults = RQ.GuestsRestrictions.MinAdults,
                    .ExtraGuests = RQ.GuestsRestrictions.ExtraGuests,
                    .MaxGuests = RQ.GuestsRestrictions.ExtraGuests
                    }
                ServiceRQ.GuestsRestrictions = ServiceRQGuestsRestrictions
            End If

            If Not RQ.BookingWindow Is Nothing Then
                Dim ServiceRQBookingWindow As New DTO.RateUpdateRQBookingWindow With {
                    .EndDate = RQ.BookingWindow.EndDate,
                    .StartDate = RQ.BookingWindow.StartDate
                    }
                ServiceRQ.BookingWindow = ServiceRQBookingWindow
            End If

            Return ServiceRQ
        End Function

        Private Function GetDaysOfWeekString(Days As DaysOfWeek) As String
            Dim Week As String = ""

            If Days.Sun Then
                Week = "Y"
            Else
                Week = "N"
            End If
            If Days.Mon Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Tue Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Wed Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Thu Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Fri Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Sat Then
                Week += "Y"
            Else
                Week += "N"
            End If

            Return Week
        End Function
    End Class
End Namespace