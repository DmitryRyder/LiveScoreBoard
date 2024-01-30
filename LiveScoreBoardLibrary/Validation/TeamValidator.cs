using FluentValidation;
using LiveScoreBoardLibrary.Models;

namespace LiveScoreBoardLibrary.Validation;

public class TeamValidator : AbstractValidator<Team>
{
    public TeamValidator()
    {
        RuleFor(team => team.Name).NotEmpty();
    }
}
