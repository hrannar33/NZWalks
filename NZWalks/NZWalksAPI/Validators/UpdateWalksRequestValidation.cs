using FluentValidation;

namespace NZWalksAPI.Validators
{
    public class UpdateWalksRequestValidation: AbstractValidator<Models.DTO.UpdateWalkRequest>
    {
        public UpdateWalksRequestValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
