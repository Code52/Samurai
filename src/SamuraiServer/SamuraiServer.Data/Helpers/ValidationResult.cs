using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class ValidationResult
    {
        private static ValidationResult _success = new ValidationResult(true);

        /// <summary>
        /// Returns a Success result.
        /// </summary>
        /// <value>Success.</value>
        public static ValidationResult Success
        {
            get { return _success; }
        }

        /// <summary>
        /// Returns a Failed result.
        /// </summary>
        /// <param name="message">A passed message containing the reason the call failed.</param>
        /// <returns></returns>
        public static ValidationResult Failure(string message)
        {
            return new ValidationResult(false, message);
        }

        /// <summary>
        /// Returns a Failed result with further details.
        /// </summary>
        /// <param name="message">A passed message containing the reason the call failed.</param>
        /// <param name="args">Additional details.</param>
        /// <returns></returns>
        public static ValidationResult Failure(string message, params object[] args)
        {
            return Failure(string.Format(CultureInfo.CurrentCulture, message, args));
        }

        /// <summary>
        /// Returns a message back to the calling method, to prompt for user response before being able to assign a response.
        /// </summary>
        /// <param name="message">A message with the further information being prompted for.</param>
        /// <returns></returns>
        public static ValidationResult Ask(string message)
        {
            return new ValidationResult(message);
        }

        /// <summary>
        /// Returns a message back to the calling method, to prompt for user response before being able to assign a response.
        /// </summary>
        /// <param name="message">A message with the further information being prompted for.</param>
        /// <param name="args">Additional details.</param>
        /// <returns></returns>
        public static ValidationResult Ask(string message, params object[] args)
        {
            return Ask(string.Format(CultureInfo.CurrentCulture, message, args));
        }

        /// <summary>
        /// Constructs a new ValidationResult in a failed state. For serialization purposes only.
        /// Use <see cref="Success"/>, <see cref="Failure(string)"/> and <see cref="Ask(string)"/> to get instances
        /// of ValidationResult.
        /// </summary>
        public ValidationResult()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="isValid">A true/false response if the result is valid.</param>
        /// <param name="message">The message to be passed.</param>
        protected ValidationResult(bool? isValid, string message)
        {
            _isValid = isValid;
            _message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        protected ValidationResult(bool isValid)
            : this(isValid, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        protected ValidationResult(string prompt)
            : this(null, prompt)
        {
        }

        private bool? _isValid;
        private string _message;

        /// <summary>
        /// Whether validation was successful. If null, validation depends on the user's
        /// response to the question in <see cref="Message">Message</see>.
        /// </summary>
        public bool? IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }

        /// <summary>
        /// The error message if validation failed, or the question to ask the user if 
        /// validity could not be determined.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }

    /// <summary>
    /// A generic class for defining a Validation Result.
    /// This class will hold the result of a check and pass messages back from the called method.
    /// </summary>
    public class ValidationResult<T>
    {
        /// <summary>
        /// Returns a Success result.
        /// </summary>
        /// <value>Success.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ValidationResult<T> Success
        {
            get { return new ValidationResult<T>(true); }
        }

        /// <summary>
        /// Returns a Failed result.
        /// </summary>
        /// <param name="message">A passed message containing the reason the call failed.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ValidationResult<T> Failure(string message)
        {
            return new ValidationResult<T>(false, message);
        }

        /// <summary>
        /// Returns a Failed result with further details.
        /// </summary>
        /// <param name="message">A passed message containing the reason the call failed.</param>
        /// <param name="args">Additional details.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ValidationResult<T> Failure(string message, params object[] args)
        {
            return Failure(string.Format(CultureInfo.CurrentCulture, message, args));
        }

        /// <summary>
        /// Returns a message back to the calling method, to prompt for user response before being able to assign a response.
        /// </summary>
        /// <param name="message">A message with the further information being prompted for.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ValidationResult<T> Ask(string message)
        {
            return new ValidationResult<T>(message);
        }

        /// <summary>
        /// Returns a message back to the calling method, to prompt for user response before being able to assign a response.
        /// </summary>
        /// <param name="message">A message with the further information being prompted for.</param>
        /// <param name="args">Additional details.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ValidationResult<T> Ask(string message, params object[] args)
        {
            return Ask(string.Format(CultureInfo.CurrentCulture, message, args));
        }

        /// <summary>
        /// Implicit conversion back to ValidationResult
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static implicit operator ValidationResult(ValidationResult<T> me)
        {
            return me.ToValidationResult();
        }

        /// <summary>
        /// Constructs a new ValidationResult in a failed state. For serialization purposes only.
        /// Use <see cref="Success"/>, <see cref="Failure(string)"/> and <see cref="Ask(string)"/> to get instances
        /// of ValidationResult.
        /// </summary>
        public ValidationResult()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="isValid">A true/false response if the result is valid.</param>
        /// <param name="message">The message to be passed.</param>
        protected ValidationResult(bool? isValid, string message)
        {
            _isValid = isValid;
            _message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        protected ValidationResult(bool isValid)
            : this(isValid, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        protected ValidationResult(string prompt)
            : this(null, prompt)
        {
        }

        private bool? _isValid;
        private string _message;

        /// <summary>
        /// Explicit conversion
        /// </summary>
        /// <returns></returns>
        public ValidationResult ToValidationResult()
        {
            var result = new ValidationResult();
            result.IsValid = IsValid;
            result.Message = Message;
            return result;
        }

        /// <summary>
        /// Whether validation was successful. If null, validation depends on the user's
        /// response to the question in <see cref="Message">Message</see>.
        /// </summary>
        public bool? IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }

        /// <summary>
        /// The error message if validation failed, or the question to ask the user if 
        /// validity could not be determined.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// User-defined payload data carried by ValidationResult
        /// </summary>
        public T Data
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the data on a ValidationResult object, returning itself
        /// </summary>
        /// <param name="data">The new data value</param>
        /// <returns></returns>
        public ValidationResult<T> WithData(T data)
        {
            this.Data = data;
            return this;
        }
    }
}
