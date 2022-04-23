using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    public class TencentCloudServer
    { 
        public string Email { get; set; }
        public KeyValuePair<string,string> Region { get; set; }
        public string InstanceId { get; set; }
        public string InstanceName { get; set; }
        public int   BaiFenBi { get; set; }
        public string IP { get; set; }
        public int TrafficUsed { get; set; }
        public int TrafficPackageTotal { get; set; }
        public int TrafficPackageRemaining { get; set; }
        public string InstanceState { get; set; }
        public DateTime ExpireTime { get; set; }
            

    }
}
