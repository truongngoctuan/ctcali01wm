namespace wm.Web2.Controllers
{
    public enum ReturnStatus
    {
        Ok,
        Errror
    }
    public class ReturnJsonObject<T>
    {
        public string Status { get; set; }
        public T Data { get; set; }
        public string[] Errors { get; set; }
    }
}