using FluentValidation;
using LiveScoreBoardLibrary.Models;

namespace LiveScoreBoardLibrary.Validation;

public class TeamValidator : AbstractValidator<Team>
{
    public TeamValidator(IEnumerable<Match> existingMatches)
    {
        RuleFor(team => team.Name).NotEmpty();
        RuleFor(team => team.Name).Must(name => !existingMatches.Any(match => match.HomeTeam.Name == name || match.AwayTeam.Name == name))
            .WithMessage("Team with the same name already exists in the scoreboard");
    }
}
