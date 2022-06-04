using Microsoft.AspNetCore.Http;

namespace MYRAY.Business.Exceptions;

public class MException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MException"/> class.
        /// </summary>
        /// <param name="statusCode">http status code.</param>
        /// <param name="errorMessage">error custom Msg.</param>
        /// <param name="target">things meet error.</param>
        public MException(int statusCode, string errorMessage, string target = "")
        {
            this.StatusCode = statusCode;
            this.ErrorMessageObject = new ErrorCustomMessage
            {
                Message = errorMessage,
                Target = System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(target),
            };
        }
        
        /// <summary>
        /// Gets or sets exception Message.
        /// </summary>
        public ErrorCustomMessage ErrorMessageObject { get; set; }

        /// <summary>
        /// Gets or sets list of error message.
        /// </summary>
        public IList<ErrorCustomMessage> Details { get; set; } = new List<ErrorCustomMessage>();
        
        /// <summary>
        /// Gets or sets Http Status Code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets custom message.
        /// </summary>
        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(this.ErrorMessageObject.Message))
                {
                    return !string.IsNullOrEmpty(base.Message) ? base.Message : "Undefined Error!";
                }

                return this.ErrorMessageObject.Message;
            }
        }
    }
