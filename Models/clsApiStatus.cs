// ==== clsApiStatus.cs ==== //
using Newtonsoft.Json.Linq;

namespace apiCheckFinal.Models
{
    public class clsApiStatus
    {
        public bool statusExec { get; set; }
        public string msg { get; set; }
        public int ban { get; set; }
        public JObject datos { get; set; }
    }
}
