namespace wm.Web2.Controllers
{
    public enum ReturnStatus
    {
        ok,
        errror
    }
    public class ReturnJsonObject<T>
    {
        public string status { get; set; }
        public T data { get; set; }
        public string[] errors { get; set; }
    }
}