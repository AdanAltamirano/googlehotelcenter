Imports FluentValidation
Imports FluentValidation.Attributes
Imports RateManager.API.Helpers

Namespace API.Models

    <Validator(GetType(RateUpdateRQValidator))>
    Public Class RateUpdateRQ
        Public Property StartDate As Date
        Public Property EndDate As Date
        Public Property RoomId As Integer?
        Public Property RateId As Integer
        Public Property HotelId As Integer?
        Public Property RatePlanId As String
        Public Property ExtraAdultPrice As Decimal?
        Public Property ExtraChildPrice As Decimal?
        Public Property ExtraJuniorPrice As Decimal?
        Public Property RateCode As String
        Public Property IsOccupancyRate As Boolean?
        Public Property Rules As RateUpdateRQRules
        Public Property Promotion As RateUpdateRQPromotion
        Public Property BookingWindow As RateUpdateRQBookingWindow
        Public Property Prices As List(Of DailyRateDetailPrice)
        Public Property ExceptionPrices As List(Of DailyRateDetailPrice)
        Public Property GuestsRestrictions As RateUpdateRQGuestsRestriction
    End Class

    Public Class DailyRateDetailPrice
        Public Property Id As Integer?
        Public Property RateId As Integer
        Public Property Occupation As Integer
        Public Property Type As PaxType
        Public Property Price As Decimal
    End Class

    <Validator(GetType(RateUpdateRQRulesValidator))>
    Public Class RateUpdateRQRules
        Public Property ExceptionDays As DaysOfWeek
        Public Property NoArrival As DaysOfWeek
        Public Property UseDefaultRules As Boolean
        Public Property MinLOS As Integer?
        Public Property MaxLOS As Integer?
        Public Property MaxAdvnaceBooking As Integer?
        Public Property MinAdvnaceBooking As Integer?
    End Class

    Public Class DaysOfWeek
        Public Property Sun As Boolean
        Public Property Mon As Boolean
        Public Property Tue As Boolean
        Public Property Wed As Boolean
        Public Property Thu As Boolean
        Public Property Fri As Boolean
        Public Property Sat As Boolean
    End Class

    Public Enum PaxType
        Adult = 1
        Child
        Junior
    End Enum

    <Validator(GetType(RateUpdateRQPromotionValidator))>
    Public Class RateUpdateRQPromotion
        Public Property Discount As Integer
        Public Property EnglishDescription As String
        Public Property SpanishDescription As String
    End Class

    <Validator(GetType(RateUpdateRQBookingWindowValidator))>
    Public Class RateUpdateRQBookingWindow
        Public Property StartDate As Date
        Public Property EndDate As Date
    End Class

    <Validator(GetType(RateUpdateRQGuestsRestrictionValidator))>
    Public Class RateUpdateRQGuestsRestriction
        Public Property MaxGuest As Byte?
        Public Property MaxAdults As Byte?
        Public Property MinAdults As Byte?
        Public Property Children As Byte?
        Public Property ExtraGuests As Byte?
    End Class

    Public Class RateUpdateRQValidator
        Inherits AbstractValidator(Of RateUpdateRQ)

        Public Sub New()
            RuleFor(Function(x) x.RateId) _
            .Must(Function(Root, RateId, Context) RateId >= 0) _
            .WithMessage("StartDate must be a valid Date")

            RuleFor(Function(x) x.StartDate) _
            .Must(Function(Root, StartDate, Context) StartDate.IsValidDate()) _
            .WithMessage("StartDate must be a valid Date")

            RuleFor(Function(x) x.EndDate) _
            .Must(Function(Root, EndDate, Context) EndDate.IsValidDate()) _
            .WithMessage("EndDate must be a valid Date")

            RuleFor(Function(x) x.RoomId) _
            .Must(Function(Root, RoomId, Context) Not RoomId Is Nothing AndAlso RoomId > 0) _
            .WithMessage("RoomId must be greater than 0")

            RuleFor(Function(x) x.RatePlanId) _
            .Must(Function(Root, RatePlanId, Context) Not String.IsNullOrEmpty(RatePlanId)) _
            .WithMessage("RoomId must not be empty")

            RuleFor(Function(x) x.RateCode) _
            .Must(Function(Root, RateCode, Context) Not String.IsNullOrEmpty(RateCode)) _
            .WithMessage("RateCode must not be empty")

            RuleFor(Function(x) x.HotelId) _
            .Must(Function(Root, HotelId, Context) Not HotelId Is Nothing AndAlso HotelId > 0) _
            .WithMessage("HotelId must be greater than 0")

            RuleFor(Function(x) x.ExtraAdultPrice) _
            .Must(Function(Root, ExtraAdultPrice, Context) ExtraAdultPrice Is Nothing OrElse ExtraAdultPrice >= 0) _
            .WithMessage("ExtraAdultPrice must be greater than 0")

            RuleFor(Function(x) x.ExtraChildPrice) _
            .Must(Function(Root, ExtraChildPrice, Context) ExtraChildPrice Is Nothing OrElse ExtraChildPrice >= 0) _
            .WithMessage("ExtraChildPrice must be greater than 0")

            RuleFor(Function(x) x.ExtraJuniorPrice) _
            .Must(Function(Root, ExtraJuniorPrice, Context) ExtraJuniorPrice Is Nothing OrElse ExtraJuniorPrice >= 0) _
            .WithMessage("ExtraJuniorPrice must be greater than 0")

            RuleFor(Function(x) x.IsOccupancyRate) _
            .Must(Function(Root, IsOccupancyRate, Context) Not IsOccupancyRate Is Nothing) _
            .WithMessage("IsOccupancyRate must not be empty")
        End Sub

    End Class

    Public Class RateUpdateRQRulesValidator
        Inherits AbstractValidator(Of RateUpdateRQRules)

        Public Sub New()
            RuleFor(Function(x) x.MaxAdvnaceBooking) _
            .Must(Function(Root, MaxAdvnaceBooking, Context) MaxAdvnaceBooking Is Nothing OrElse MaxAdvnaceBooking > 0) _
            .WithMessage("AdvnaceBooking must be greater than 0")

            RuleFor(Function(x) x.MinAdvnaceBooking) _
            .Must(Function(Root, MinAdvnaceBooking, Context) MinAdvnaceBooking Is Nothing OrElse MinAdvnaceBooking > 0) _
            .WithMessage("MinAdvnaceBooking must be greater than 0")

            RuleFor(Function(x) x.MaxLOS) _
            .Must(Function(Root, MaxLOS, Context) MaxLOS Is Nothing OrElse MaxLOS > 0) _
            .WithMessage("MaxLOS must be greater than 0")

            RuleFor(Function(x) x.MinLOS) _
            .Must(Function(Root, MinLOS, Context) MinLOS Is Nothing OrElse MinLOS > 0) _
            .WithMessage("MinLOS must be greater than 0")
        End Sub
    End Class

    Public Class RateUpdateRQPromotionValidator
        Inherits AbstractValidator(Of RateUpdateRQPromotion)

        Public Sub New()
            RuleFor(Function(x) x.Discount) _
            .Must(Function(Root, Discount, Context) Discount > 0) _
            .WithMessage("Discount must be greater than 0")

            RuleFor(Function(x) x.SpanishDescription) _
            .Must(Function(Root, SpanishDescription, Context) Not String.IsNullOrEmpty(SpanishDescription)) _
            .WithMessage("SpanishDescription must not be empty")

            RuleFor(Function(x) x.EnglishDescription) _
            .Must(Function(Root, EnglishDescription, Context) Not String.IsNullOrEmpty(EnglishDescription)) _
            .WithMessage("EnglishDescription must not be empty")
        End Sub
    End Class

    Public Class RateUpdateRQBookingWindowValidator
        Inherits AbstractValidator(Of RateUpdateRQBookingWindow)
        Public Sub New()
            RuleFor(Function(x) x.StartDate) _
            .Must(Function(Root, StartDate, Context) StartDate.IsValidDate()) _
            .WithMessage("StartDate must be a valid Date")

            RuleFor(Function(x) x.EndDate) _
            .Must(Function(Root, EndDate, Context) EndDate.IsValidDate()) _
            .WithMessage("EndDate must be a valid Date")
        End Sub
    End Class

    Public Class RateUpdateRQGuestsRestrictionValidator
        Inherits AbstractValidator(Of RateUpdateRQGuestsRestriction)
        Public Sub New()
            RuleFor(Function(x) x.MaxGuest) _
            .Must(Function(Root, MaxGuest, Context) MaxGuest Is Nothing OrElse MaxGuest > 0) _
            .WithMessage("MaxGuest must be greater than 0")

            RuleFor(Function(x) x.MaxAdults) _
            .Must(Function(Root, MaxAdults, Context) MaxAdults Is Nothing OrElse MaxAdults > 0) _
            .WithMessage("MaxAdults must be greater than 0")

            RuleFor(Function(x) x.MinAdults) _
            .Must(Function(Root, MinAdults, Context) MinAdults Is Nothing OrElse MinAdults > 0) _
            .WithMessage("MinAdults must be greater than 0")

            RuleFor(Function(x) x.Children) _
            .Must(Function(Root, Children, Context) Children Is Nothing OrElse Children > 0) _
            .WithMessage("Childs must be greater than 0")

            RuleFor(Function(x) x.ExtraGuests) _
            .Must(Function(Root, ExtraGuests, Context) ExtraGuests Is Nothing OrElse ExtraGuests > 0) _
            .WithMessage("ExtraGuests must be greater than 0")
        End Sub
    End Class
End Namespace