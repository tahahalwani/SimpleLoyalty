using FluentValidation;
using UserService.Domain;


namespace UserService.Client
{
    public class EarningValidator : AbstractValidator<Earning>
    {
        public EarningValidator()
        {
            RuleFor(user => user.UserId).NotNull();
            RuleFor(user => user.EarnedPoints).NotNull().ExclusiveBetween(0, 101);
        }
    }
}
