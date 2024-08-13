namespace Futbol_Insight_Jobs.Models
{

    public class ResultModel<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResultModel(int code, string message, T data = default)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}
