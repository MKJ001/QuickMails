namespace Common.Validators
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class EmailAddressBulkAttribute : ValidationAttribute
    {
        private string InvalidTypeMessage =>
            $"{nameof(EmailAddressBulkAttribute)} can only validate IEnumerable<string> types.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is IEnumerable<string> emails))
            {
                return new ValidationResult(this.InvalidTypeMessage);
            }
            
            var emailAttribute = new EmailAddressAttribute();
            var invalidEmails = emails.Where(email => !emailAttribute.IsValid(email)).ToList();
            if (invalidEmails.Any())
            {
                var errorMessage = $"Emails {string.Join(", ", invalidEmails)} are invalid.";
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;

        }
    }
}
