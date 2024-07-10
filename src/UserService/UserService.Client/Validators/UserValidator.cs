using FluentValidation;
using UserService.Domain;


namespace UserService.Client
{
    public class UserValidator : AbstractValidator<User>

    {
        public UserValidator()
        {

            RuleFor(user => user.FirstName).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(user => user.LastName).NotNull().NotEmpty().MaximumLength(20);
        }
    }
}
