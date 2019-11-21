Imports FluentValidation
Imports FluentValidation.Attributes
Imports RateManager.API.Helpers

Namespace API.Models

    <Validator(GetType(RatesByPlanRQValidator))>
    Public Class RatesByPlanRQ
        Public Property StartDate As Date
        Public Property EndDate As Date
        Public Property RoomId As Integer?
    End Class

    Public Class RatesByPlanRQValidator
        Inherits AbstractValidator(Of RatesByPlanRQ)

        Public Sub New()
            RuleFor(Function(x) x.StartDate) _
            .Must(Function(Root, StartDate, Context) StartDate.IsValidDate()) _
            .WithMessage("StartDate must be a valid Date")

            RuleFor(Function(x) x.EndDate) _
            .Must(Function(Root, EndDate, Context) EndDate.IsValidDate()) _
            .WithMessage("EndDate must be a valid Date")

            RuleFor(Function(x) x.RoomId) _
            .Must(Function(Root, RoomId, Context) RoomId Is Nothing OrElse RoomId > 0) _
            .WithMessage("RoomId must be greater than 0")
        End Sub

    End Class
End Namespace


