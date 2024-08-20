namespace Futbol_Insight_Jobs.Models
{
    public class ResultBase
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
    public class ResultModel<T> : ResultBase
    {
        public T Data { get; set; }

        public ResultModel(int code, string message, T data = default)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public ResultModel()
        {
        }
    }
}
