Imports System.Web
Imports System.Web.Services
Imports APIServices
Imports APIServices.Models.DTO

Public Class RateUpdate
    Inherits APIHandler


    Private RatesServices As New RatesService()
    Private Function ValidadPostRequest(ByRef context As HttpContext, ByRef req As UpdateRateRequest) As Boolean
        Try
            Dim hotelId As Integer
            If Not Integer.TryParse(context.Request.Form("hotelid"), hotelId) Then
                req.Error = "invalid hotelId"
                Return False
            End If

            Dim roomId As Integer
            If Not Integer.TryParse(context.Request.Form("roomId"), roomId) Then
                req.Error = "invalid roomId"
                Return False
            End If

            Dim ratePlanId As String
            ratePlanId = context.Request.Form("ratePlanId")

            Dim startDate As Date
            If Not Date.TryParse(context.Request.Form("startDate"), startDate) Then
                req.Error = "invalid startDate"
                Return False
            End If

            Dim endDate As Date
            If Not Date.TryParse(context.Request.Form("endDate"), endDate) Then
                req.Error = "invalid startDate"
                Return False
            End If

            req.HotelId = hotelId
            req.RoomId = roomId
            req.RatePlanId = ratePlanId
            req.StartDate = startDate
            req.EndDate = endDate
            req.ExtraAdultPrice = CType(context.Request.Form("extraAdultPrice"), Decimal)
            req.ExtraChildPrice = CType(context.Request.Form("extraChildPrice"), Decimal)
            req.ExtraJuniorPrice = CType(context.Request.Form("extraJuniorPrice"), Decimal)
            req.RateCode = context.Request.Form("rateCode")

            Dim promotion As New UpdateRateRequestPromotion With {
                .Discount = CType(context.Request.Form("promotion[discount]"), Decimal),
                .EnglishDescription = context.Request.Form("promotion[englishDescription]"),
                .SpanishDescription = context.Request.Form("promotion[spanishDescription]")
                }

            Dim rules As New UpdateRateRequestRules With {
                .ExceptionDays = context.Request.Form("rules[exceptionDays]"),
                .NoArrival = context.Request.Form("rules[noArrival]"),
                .UseDefaultRules = CType(context.Request.Form("rules[useDefaultRules]"), Boolean),
                .Segment = context.Request.Form("rules[segment]"),
                .MinLOS = CType(context.Request.Form("rules[minLOS]"), Integer),
                .MaxLOS = CType(context.Request.Form("rules[maxLOS]"), Integer),
                .MaxAdvanceBooking = CType(context.Request.Form("rules[maxAdvBooking]"), Integer),
                .MinAdvanceBooking = CType(context.Request.Form("rules[minAdvBooking]"), Integer)
                }

            Dim guestsRestrictions As New UpdateRequestGuestsRestriction With {
            .MaxGuests = CType(context.Request.Form("guestsRestrictions[maxGuests]"), Integer),
            .MaxAdults = CType(context.Request.Form("guestsRestrictions[maxAdults]"), Integer),
            .MinAdults = CType(context.Request.Form("guestsRestrictions[minAdults]"), Integer),
            .Childs = CType(context.Request.Form("guestsRestrictions[childs]"), Integer),
            .ExtraGuests = CType(context.Request.Form("guestsRestrictions[extraGuests]"), Integer)
                }

            Dim bookingWindow As New UpdateRateRequestBookingWindow With {
            .StartDate = CType(context.Request.Form("bookingWindow[startDate]"), Date),
            .EndDate = CType(context.Request.Form("bookingWindow[endDate]"), Date)
                }

            req.Promotion = promotion
            req.Rules = rules
            req.GuestsRestrictions = guestsRestrictions
            req.BookingWindow = bookingWindow

            Dim price As New DailyRateDetailPrice
            price.Id = 0
            price.RateId = 0
            price.Occupation = CType(context.Request.Form("prices[0][occupation]"), Integer)
            price.Type = CType(context.Request.Form("prices[0][type]"), Integer)
            price.Price = CType(context.Request.Form("prices[0][price]"), Decimal)

            req.Prices.Add(price)

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Protected Overrides Sub GetHandler(ByRef context As HttpContext)
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Sub PostHandler(ByRef context As HttpContext)
        Dim req As New UpdateRateRequest()
        If ValidadPostRequest(context, req) Then
            OK(
                context:=context,
                result:=RatesServices.AddRate(req))
        Else
            OkWithError(context, req.Error)
        End If
    End Sub

End Class