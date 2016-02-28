using System.Collections.Generic;

namespace wm.Service.Common
{
    public class ServiceReturn
    {
        public bool IsSucceed { get; set; }
        public IEnumerable<string> Messages { get; set; }

        public static ServiceReturn Ok { get { return new ServiceReturn { IsSucceed = true }; } }
        public static ServiceReturn Error(string message)
        {
            return new ServiceReturn { IsSucceed = false, Messages = new List<string>() { message } };
        }
        public static ServiceReturn Error(IEnumerable<string> messages)
        {
            return new ServiceReturn { IsSucceed = false, Messages = messages };
        }
    }
}
