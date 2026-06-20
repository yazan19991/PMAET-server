namespace PMAET_server.Models.Responses
{
    /// <summary>
    /// Represents a message containing status and details for a response.
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage"/> class with the specified status and message.
        /// </summary>
        /// <param name="status">The status code indicating the result of an operation.</param>
        /// <param name="message">A message providing additional details about the status.</param>
        /// <param name="code">A Code providing additional details about the error.</param> 
        public ResponseMessage(int status, string message)
        {
            Status = status;
            Message = message;
        
        }
        public ResponseMessage(int status, string message, string? code)
        {
            Status = status;
            Message = message;
            Code = code;
        }
        /// <summary>
        /// Gets or sets the status code indicating the result of an operation.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets a message providing additional details about the status.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets a Code providing additional details about the error.
        /// </summary>
        public string? Code { get; set; }
    }
}
