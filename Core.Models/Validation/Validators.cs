using System.Globalization;
using System.Text.RegularExpressions;

namespace ClientManagement.Models.Validation
{
    public static class Validators
    {
        public static ValidatorFn Required()
        {
            var result = new ValidatorFn()
            {
                Message = CultureInfo.InvariantCulture.TextInfo.ToTitleCase("This Field Is Required"),
                Name = nameof(Validators.Required)
            };
            result.Validator = (val) =>
            {
                if (val is IEnumerable<string> collection)
                {
                    return collection.Any() ? null : new KeyValuePair<string, string>(result.Name, result.Message);
                }
                if (!string.IsNullOrWhiteSpace(val?.ToString()))
                {
                    return null;
                }
                else
                {
                    return new KeyValuePair<string, string>(result.Name, result.Message);
                }
            };
            return result;
        }

        public static ValidatorFn Email() {

            var result = new ValidatorFn()
            {
                Name = nameof(Validators.Email),
                Message = CultureInfo.InvariantCulture.TextInfo.ToTitleCase("Invalid Email")
            };
            result.Validator = (val) =>
            {
                if (Regex.IsMatch(val?.ToString()?.Trim() ?? string.Empty, @".+@.+") || string.IsNullOrWhiteSpace(val?.ToString()))
                {
                    return null;
                }
                else
                {
                    return new KeyValuePair<string, string>(result.Name, result.Message);
                }
            };
            return result;
        }

        public static ValidatorFn Pattern(Regex regex, string patternName, string validationMessage = "") {

            var result = new ValidatorFn()
            {
                Name = $"{patternName}Pattern",
                Message = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(validationMessage)
            };
            result.Validator = (val) =>
            {
                if (string.IsNullOrWhiteSpace(val?.ToString()))
                {
                    return null;
                }
                return regex.IsMatch(val?.ToString() ?? string.Empty) ? null : new KeyValuePair<string, string>(result.Name , result.Message);
            };
            return result;
        }

        public static ValidatorFn Date()
        {
            var result = new ValidatorFn()
            {
                Name = nameof(Date),
                Message = CultureInfo.InvariantCulture.TextInfo.ToTitleCase("Invalid date format")
            };
            result.Validator = (val) =>
            {
                if (string.IsNullOrWhiteSpace(val?.ToString()) || val is DateTime)
                {
                    return null;
                }
                return Regex.IsMatch(val?.ToString() ?? string.Empty, @"^(\d{2}/\d{2}/\d{4})$") ? null : new KeyValuePair<string, string>(result.Name, result.Message);
            };
            return result;
        }

        public static ValidatorFn Number()
        {
            var result = new ValidatorFn()
            {
                Name = nameof(Number),
                Message = CultureInfo.InvariantCulture.TextInfo.ToTitleCase("Invalid numeric format")
            };
            result.Validator = (val) =>
            {
                if (string.IsNullOrWhiteSpace(val?.ToString()) || val is int || val is double)
                {
                    return null;
                }
                return Regex.IsMatch(val?.ToString() ?? string.Empty, @$"^(\d*{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}?\d+)$") ? null : new KeyValuePair<string, string>(result.Name, result.Message);
            };
            return result;
        }

    }
}
