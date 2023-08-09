namespace Be_My_Voice_Backend.Models
{
    public class APIResponse
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object[] Data { get; set; } = null;
        public List<string> ErrorMessage { get; set; } = null;

        public APIResponse(int Code, bool success, string message, object[] data)
        {
            Code = Code;
            Success = success;
            Message = message;
            Data = data;
        }

        public APIResponse()
        {
            Code = 0;
            Success = false;
            Message = "Something went wrong";
            Data = null;
        }

        public APIResponse(int code, bool success, string message)
        {
            Code = code;
            Success = success;
            Message = message;
            Data = null;
        }

        public APIResponse(int Code, bool success, string message, object data)
        {
            Object[] dataArray = new Object[1];
            dataArray[0] = data;

            this.Code = Code;
            Success = success;
            Message = message;
            Data = dataArray;
        }


    }
}
