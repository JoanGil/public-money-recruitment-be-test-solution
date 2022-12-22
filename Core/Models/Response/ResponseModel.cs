using System.Net;

namespace VacationRental.Core.Models.Response
{
    public class ResponseModel
    {
        public object Data { get; set; }
        public ValidationModel Validation { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }

        public ResponseModel()
        {
            Validation = new ValidationModel() { IsValid = true };
            HttpStatusCode = HttpStatusCode.OK;
        }

        public ResponseModel(object data) : this()
        {
            Data = data;
        }

        public ResponseModel(bool isValid = true) : this() { }

        public ResponseModel(string message, bool isValid = false)
        {
            Validation = new ValidationModel()
            {
                IsValid = isValid,
                Message = message
            };
        }

        public ResponseModel(HttpStatusCode httpStatusCode, string message, bool isValid = false) : this(message, isValid)
        {
            HttpStatusCode = httpStatusCode;
        }
    }

    public class ResponseModel<T>
    {
        public T Data { get; set; }
        public ValidationModel Validation { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }

        public ResponseModel()
        {
            Validation = new ValidationModel() { IsValid = true };
            HttpStatusCode = HttpStatusCode.OK;
        }

        public ResponseModel(T data) : this()
        {
            Data = data;
        }

        public ResponseModel(bool isValid = true) : this() { }

        public ResponseModel(string message, bool isValid = false)
        {
            Validation = new ValidationModel()
            {
                IsValid = isValid,
                Message = message
            };
        }

        public ResponseModel(HttpStatusCode httpStatusCode, string message, bool isValid = false) : this(message, isValid)
        {
            HttpStatusCode = httpStatusCode;
        }
    }

    public class ValidationModel
    {
        public string Message { get; set; }
        public bool IsValid { get; set; }

        public ValidationModel()
        {

        }

        public ValidationModel(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }
}
