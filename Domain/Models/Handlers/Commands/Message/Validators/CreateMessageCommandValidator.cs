using FluentValidation;

namespace Domain.Models.Handlers.Commands.Message.Validators
{
    public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
    {
        public CreateMessageCommandValidator()
        {
            RuleFor(command => command.Content)
                .NotNull()
                .NotEmpty()
                .WithMessage("The message was entered incorrectly");
        }
    }
}
