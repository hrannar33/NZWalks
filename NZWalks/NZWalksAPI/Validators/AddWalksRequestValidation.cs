using FluentValidation;

namespace NZWalksAPI.Validators
{
    public class AddWalksRequestValidation: AbstractValidator<Models.DTO.AddWalkRequest>
    {
        public AddWalksRequestValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
    }
    }
}
