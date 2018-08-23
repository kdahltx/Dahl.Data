namespace Dahl.Data.Common
{
    public class LastError
    {
        public string Message { get; set; }
        public int Code { get; set; }

        public void Reset()
        {
            Message = string.Empty;
            Code = 0;
        }

        public void Set(int code, string message)
        {
            Message = message;
            Code = code;
        }
    }
}
