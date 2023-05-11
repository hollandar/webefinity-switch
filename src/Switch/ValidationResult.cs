using System.Collections.Generic;

namespace Webefinity.Switch
{
    /// <summary>
    /// Provides information about the validity of an individual command option, and the entire command line.
    /// Instances of the ValidationResult are combined by the ArgumentHandler using the Combine method.
    /// </summary>
    public class ValidationResult
    {
        List<string> errors = new();
        bool valid;

        /// <summary>
        /// Construct a new validation result.
        /// </summary>
        /// <param name="isValid">Was the validation successful (true), or did it fail (false).</param>
        /// <param name="errors">Any errors that describe the validation failure.</param>
        public ValidationResult(bool isValid = true, params string[] errors)
        {
            this.valid = isValid;
            this.errors.AddRange(errors);
        }

        /// <summary>
        /// True if the validation was successful.
        /// False if the validation was unsuccessful.  Will usually contain Errors.
        /// </summary>
        public bool IsValid => this.valid;

        /// <summary>
        /// A list of errors that describe a validation failure.
        /// </summary>
        public ICollection<string> Errors => this.errors.AsReadOnly();

        /// <summary>
        /// Stack another validation on top of this one.
        /// The result is valid only if the passed results, and the current result, were all valid.
        /// Errors are combined into a single list.
        /// </summary>
        /// <param name="validationResults">The </param>
        public void Combine(params ValidationResult[] validationResults)
        {
            foreach (var validationResult in validationResults)
            {
                this.valid &= validationResult.IsValid;
                this.errors.AddRange(validationResult.Errors);
            }
        }
    }
}
