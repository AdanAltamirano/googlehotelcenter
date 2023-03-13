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
        Public Function RateAdd(<FromBody> RQ As RateUpdateRQ, HotelId As Integer) As Net.Http.HttpResponseMessage
            RQ.HotelId = HotelId
            Dim serviceRQ As DTO.RateUpdateRQ = MappingRateUpdateRQ(RQ)
            Dim result As KeyValuePair(Of String, String) = Service.AddRate(serviceRQ)

            If result.Key = 1 Then
                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        'POST api/hotels/1/rates/
        <Route("{RateId:int}/daily/{day:datetime}"), HttpPost>
        Public Function RateUpdate(<FromBody> RQ As RateUpdateRQ, HotelId As Integer, RateId As Integer, day As Date) As Net.Http.HttpResponseMessage

            RQ.HotelId = HotelId
            RQ.RateId = RateId
            RQ.StartDate = day
            RQ.EndDate = day
            Dim serviceRQ As DTO.RateUpdateRQ = MappingRateUpdateRQ(RQ)
            Dim result As KeyValuePair(Of String, String) = Service.AddRate(serviceRQ)

            If result.Key = 1 Then
                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        Private Function MappingRateUpdateRQ(RQ As RateUpdateRQ) As DTO.RateUpdateRQ
            Dim ServiceRQ As New DTO.RateUpdateRQ With {
                .HotelId = RQ.HotelId,
                .RoomId = RQ.RoomId,
                .RatePlanCode = RQ.RatePlanCode,
                .RateId = RQ.RateId,
                .StartDate = RQ.StartDate,
                .EndDate = RQ.EndDate,
                .IsOccupancyRate = RQ.IsOccupancyRate
            }

            Dim ServiceRQRatePlans As New List(Of DTO.RatePlanHeader)
            For Each itemRatePlan As RatePlanHeader In RQ.RatePlans
                Dim tempRatePlan As New DTO.RatePlanHeader With {
                    .Code = itemRatePlan.Code,
                    .Name = itemRatePlan.Name
                }

                ServiceRQRatePlans.Add(tempRatePlan)
            Next

            ServiceRQ.RatePlans = ServiceRQRatePlans


            Dim ServiceRQDates As New List(Of DTO.RateUpdateRQDate)
            For Each itemDate As RateUpdateRQDate In RQ.Dates
                Dim tempDate As New DTO.RateUpdateRQDate With {
                    .StartDate = itemDate.StartDate,
                    .EndDate = itemDate.EndDate
                    }
                ServiceRQDates.Add(tempDate)
            Next

            ServiceRQ.Dates = ServiceRQDates


            Dim ServiceRQPricesBase As New List(Of DTO.DailyRateDetailPrice)
            For Each PriceBase As DailyRateDetailPrice In RQ.Prices.Base
                Dim ServiceRQPriceBase As New DTO.DailyRateDetailPrice With {
                    .Type = PriceBase.Type,
                    .Price = PriceBase.Price,
                    .Occupation = PriceBase.Occupation
                    }
                ServiceRQPricesBase.Add(ServiceRQPriceBase)
            Next

            Dim ServiceRQPricesException As New List(Of DTO.DailyRateDetailPrice)
            If Not RQ.Prices.Exceptions Is Nothing Then
                For Each PriceException As DailyRateDetailPrice In RQ.Prices.Exceptions
                    Dim ServiceRQPriceException As New DTO.DailyRateDetailPrice With {
                        .Type = PriceException.Type,
                        .Price = PriceException.Price,
                        .Occupation = PriceException.Occupation
                        }
                    ServiceRQPricesException.Add(ServiceRQPriceException)
                Next
            End If

            Dim ServiceRQPricesExtra As New List(Of DTO.DailyRateDetailPrice)
            If Not RQ.Prices.Extra Is Nothing Then
                For Each PriceExtra As DailyRateDetailPrice In RQ.Prices.Extra
                    Dim ServiceRQPriceExtra As New DTO.DailyRateDetailPrice With {
                        .Type = PriceExtra.Type,
                        .Price = PriceExtra.Price,
                        .Occupation = PriceExtra.Occupation
                        }
                    ServiceRQPricesExtra.Add(ServiceRQPriceExtra)
                Next
            End If

            Dim ServiceRQPrices As New DTO.RateUpdatePrices With {
                .Base = ServiceRQPricesBase,
                .Exceptions = ServiceRQPricesException,
                .ExceptionDays = If(RQ.Prices?.ExceptionDays Is Nothing, "NNNNNNN", GetDaysOfWeekString(RQ.Prices.ExceptionDays)),
                .Extra = ServiceRQPricesExtra
                }
            ServiceRQ.Prices = ServiceRQPrices

            If Not RQ.Prices.Promotion Is Nothing Then
                Dim serviceRQPricesPromotion As New DTO.RateUpdateRQPromotion
                With serviceRQPricesPromotion
                    .Discount = RQ.Prices.Promotion.Discount
                    .EnglishDescription = RQ.Prices.Promotion.EnglishDescription
                    .SpanishDescription = RQ.Prices.Promotion.SpanishDescription
                End With
                ServiceRQ.Prices.Promotion = serviceRQPricesPromotion
            End If

            If Not RQ.Rules Is Nothing Then
                Dim ServiceRQGuestsRestrictions As New DTO.RateUpdateRQGuestsRestriction
                If Not RQ.Rules.GuestsRestrictions Is Nothing Then
                    With ServiceRQGuestsRestrictions
                        .Children = RQ.Rules.GuestsRestrictions.Children
                        .MaxAdults = RQ.Rules.GuestsRestrictions.MaxAdults
                        .MinAdults = RQ.Rules.GuestsRestrictions.MinAdults
                        .ExtraGuests = RQ.Rules.GuestsRestrictions.ExtraGuests
                        .MaxGuests = RQ.Rules.GuestsRestrictions.ExtraGuests
                    End With
                End If

                Dim ServiceRQBookingWindow As New DTO.RateUpdateRQBookingWindow
                If Not RQ.Rules.BookingWindow Is Nothing Then
                    With ServiceRQBookingWindow
                        .EndDate = RQ.Rules.BookingWindow.EndDate
                        .StartDate = RQ.Rules.BookingWindow.StartDate
                    End With
                End If
                Dim ServiceRQRules As New DTO.RateUpdateRQRules With {
                .UseDefaultRules = RQ.Rules.UseDefaultRules,
                .MinLOS = RQ.Rules.MinLOS,
                .MaxLOS = RQ.Rules.MaxLOS,
                .MaxAdvanceBooking = RQ.Rules.MaxAdvanceBooking,
                .MinAdvanceBooking = RQ.Rules.MinAdvanceBooking,
                .NoArrival = If(RQ.Rules?.NoArrival Is Nothing, "NNNNNNN", GetDaysOfWeekString(RQ.Rules.NoArrival)),
                .GuestsRestrictions = ServiceRQGuestsRestrictions,
                .BookingWindow = ServiceRQBookingWindow
                }
                ServiceRQ.Rules = ServiceRQRules
            End If

            Return ServiceRQ
        End Function

        Private Function GetDaysOfWeekString(Days As DaysOfWeek) As String
            Dim Week As String = ""

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
            If Days.Sun Then
                Week += "Y"
            Else
                Week += "N"
            End If

            Return Week
        End Function
    End Class
End Namespace