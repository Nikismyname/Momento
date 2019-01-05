namespace Momento.Services.Models.CustomAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ShouldNotBeValidationAttribute : ValidationAttribute
    {
        private string target;

        public ShouldNotBeValidationAttribute(string target)
        {
            this.target = target;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var paramValue = (string)value;

            if (paramValue.ToUpper() == target.ToUpper())
            {
                return new ValidationResult("Directory Name can not be \"root\"!");
            }

            return ValidationResult.Success;
        }
    }
}
