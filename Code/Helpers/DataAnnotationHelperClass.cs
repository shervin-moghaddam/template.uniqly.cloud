using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using template.Services;
using static template.Code.DataTypeConversionHelperClass;

namespace template.Code.Helpers;

public class DataAnnotationHelperClass
{
    public class StringValidation : ValidationAttribute
    {
        public StringValidation(bool canBeEmpty = false, int MinLength = 0, int maxLength = 0)
        {
            this.MinLength = MinLength;
            this.CanBeEmpty = canBeEmpty;
            this.MaxLength = maxLength;
        }

        public int MaxLength { get; private set; }
        public int MinLength { get; private set; }
        public bool CanBeEmpty { get; private set; }

        protected override ValidationResult IsValid(object ObjectValue, ValidationContext validationContext)
        {
            // Get localization setup
            GlobalTranslationService Translation =
                validationContext.GetService(typeof(GlobalTranslationService)) as GlobalTranslationService;

            // Maxlength
            if (MaxLength > 0 && cStr(ObjectValue).Length > MaxLength)
                return new ValidationResult(Translation?
                    .GetText("ModelValidation_String_TooManyChars")
                    .Replace("{{maxlength}}", cStr(MaxLength)));

            string x = Translation.GetText("ModelValidation_String_CannotBeEmpty");
            
            // If empty
            if (!CanBeEmpty && string.IsNullOrEmpty(cStr(ObjectValue)))
                return new ValidationResult(
                    Translation?.GetText("ModelValidation_String_CannotBeEmpty"));
            return ValidationResult.Success;
        }
    }

    public class EmailValidation : ValidationAttribute
    {
        public EmailValidation(bool canBeEmpty = false)
        {
            this.CanBeEmpty = canBeEmpty;
        }

        public bool CanBeEmpty { get; private set; }

        protected override ValidationResult IsValid(object ObjectValue, ValidationContext validationContext)
        {
            // Get localization setup
            GlobalTranslationService Translation =
                validationContext.GetService(typeof(GlobalTranslationService)) as GlobalTranslationService;

            // If empty
            if (!CanBeEmpty && string.IsNullOrEmpty(cStr(ObjectValue)))
                return new ValidationResult(
                    Translation?.GetText("ModelValidation_String_CannotBeEmpty"));
            
            const string CheckPattern =
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            // Using regular expressions to see if input Email Address maches pattern
            if (Regex.IsMatch(cStr(ObjectValue), CheckPattern)) // If correct
            {
                return ValidationResult.Success;
            }
            else
            {
                // Incorrect email
                return new ValidationResult(
                    Translation?.GetText("ModelValidation_Email_InvalidEmail"));
            }

            return ValidationResult.Success;
        }
    }

    public class DecimalValidation : ValidationAttribute
    {
        public DecimalValidation(float MinValue = 0, float MaxValue = 0)
        {
            this.MinValue = MinValue;
            this.MaxValue = MaxValue;
        }

        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        protected override ValidationResult IsValid(object ObjectValue, ValidationContext validationContext)
        {
            // Get localization setup
            GlobalTranslationService Translation =
                validationContext.GetService(typeof(GlobalTranslationService)) as GlobalTranslationService;

            if (Decimal.TryParse(cStr(ObjectValue), out decimal d))
            {
                if (MaxValue > 0 && d > cDec(MaxValue))
                    return new ValidationResult(
                        Translation?.GetText("ModelValidation_Decimal_AmountTooHigh")
                            .Replace("{{maxvalue}}", cStr(MaxValue)));

                if (MinValue != 0 && d < cDec(MinValue))
                    return new ValidationResult(
                        Translation?.GetText("ModelValidation_Decimal_AmountTooLow")
                            .Replace("{{minvalue}}", cStr(MinValue)));
            }
            else
                return new ValidationResult(
                    Translation?.GetText("ModelValidation_Decimal_WrongInput")
                        .Replace("{{value}}", cStr(ObjectValue)));

            return ValidationResult.Success;
        }
    }
}