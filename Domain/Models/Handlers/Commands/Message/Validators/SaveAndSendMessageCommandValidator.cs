using FluentValidation;

namespace Domain.Models.Handlers.Commands.Message.Validators
{
    public class SaveAndSendMessageCommandValidator : AbstractValidator<SaveAndSendMessageCommand>
    {
        public SaveAndSendMessageCommandValidator()
        {
            RuleFor(command => command.Content)
                .NotNull()
                .NotEmpty()
                .MaximumLength(128)
                .WithMessage("The message was entered incorrectly. Maximum length is 128 characters.");
        }
    }
}
