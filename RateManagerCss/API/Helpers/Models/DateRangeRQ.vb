Imports FluentValidation
Imports FluentValidation.Attributes
Imports RateManager.API.Helpers

Namespace API.Models
    <Validator(GetType(DateRangeRQValidator))>
    Public Class DateRangeRQ
        Public Property StartDate As Date
        Public Property EndDate As Date
    End Class

    Public Class DateRangeRQValidator
        Inherits AbstractValidator(Of DateRangeRQ)

        Public Sub New()
            RuleFor(Function(x) x.StartDate) _
            .Must(Function(Root, StartDate, Context) StartDate.IsValidDate()) _
            .WithMessage("StartDate must be a valid Date")

            RuleFor(Function(x) x.EndDate) _
            .Must(Function(Root, EndDate, Context) EndDate.IsValidDate()) _
            .WithMessage("EndDate must be a valid Date")
        End Sub

    End Class
End Namespace
