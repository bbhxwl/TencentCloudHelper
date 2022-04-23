using API.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TencentCloud.Lighthouse.V20200324;
using TencentCloud.Lighthouse.V20200324.Models;


namespace API
{
    public class TencentCloudApiCommon
    {
        public static List<TencentCloudApiUser> l = new List<TencentCloudApiUser>() {
        
        };
        public static Dictionary<string, string> Regions = new Dictionary<string, string>() {
        {"ap-beijing","北京" },
        {"ap-chengdu","成都" },
        {"ap-guangzhou","广州" },
        {"ap-hongkong","中国香港" },
        {"ap-nanjing","南京" },
        {"ap-shanghai","上海" },
        {"ap-singapore","新加坡" },
        {"ap-tokyo","东京" },
        {"eu-moscow","莫斯科" },
        {"na-siliconvalley","硅谷" },
        };
       
        public static TencentCloudApiUser GetSecret(string Email)
        {
            return l.Where(p => p.Email.Contains(Email.Trim())).FirstOrDefault();
        }

        public static bool Stop(string Email, string InstanceId, string region)
        {
            try
            {
                var item = l.Where(p => p.Email == Email.Trim()).FirstOrDefault();
                LighthouseClient client = new LighthouseClient(new TencentCloud.Common.Credential() { SecretId = item.SecretId, SecretKey = item.SecretKey }, region);
                var rs = client.StopInstancesSync(new StopInstancesRequest() { InstanceIds = new string[] { InstanceId } });
                return true;
            }
            catch (Exception)
            {


            }
            return false;

        }
        public static bool Start(string Email, string InstanceId, string region)
        {
            try
            {
                var item = l.Where(p => p.Email == Email.Trim()).FirstOrDefault();
                LighthouseClient client = new LighthouseClient(new TencentCloud.Common.Credential() { SecretId = item.SecretId, SecretKey = item.SecretKey }, region);
                var rs = client.StartInstancesSync(new StartInstancesRequest() { InstanceIds = new string[] { InstanceId } });
                return true;
            }
            catch (Exception)
            {


            }
            return false;

        }

        public static List<TencentCloudServer> GetAll()
        {
            List<TencentCloudServer> list = new List<TencentCloudServer>();
            foreach (var item in l)
            {
                foreach (var region in Regions)
                {
                    try
                    {
                        LighthouseClient client = new LighthouseClient(new TencentCloud.Common.Credential() { SecretId = item.SecretId, SecretKey = item.SecretKey }, region.Key);
                        var instances = client.DescribeInstancesSync(new DescribeInstancesRequest());
                        var rs = client.DescribeInstancesTrafficPackagesSync(new DescribeInstancesTrafficPackagesRequest() { });
                        foreach (var r in rs.InstanceTrafficPackageSet)
                        {

                            try
                            {

                                double a = r.TrafficPackageSet[0].TrafficUsed.Value;
                                double b = r.TrafficPackageSet[0].TrafficPackageTotal.Value;
                                int 百分比 = int.Parse(Math.Round(a / b * 100, 0).ToString());
                                var model = instances.InstanceSet.Where(p => p.InstanceId == r.InstanceId).FirstOrDefault();
                                string InstanceState = "";
                                switch (model.InstanceState)
                                {
                                    case "PENDING":
                                        InstanceState = "创建中";
                                        break;
                                    case "LAUNCH_FAILED":
                                        InstanceState = "创建失败";
                                        break;
                                    case "RUNNING":
                                        InstanceState = "运行中";
                                        break;
                                    case "STOPPED":
                                        InstanceState = "关机";
                                        break;
                                    case "STARTING":
                                        InstanceState = "开机中";
                                        break;
                                    case "STOPPING":
                                        InstanceState = "关机中";
                                        break;
                                    case "REBOOTING":
                                        InstanceState = "重启中";
                                        break;
                                    case "SHUTDOWN":
                                        InstanceState = "停止待销毁";
                                        break;
                                    case "TERMINATING":
                                        InstanceState = "销毁中";
                                        break;

                                    default:
                                        break;
                                }


                              
                                list.Add(new TencentCloudServer()
                                {
                                    BaiFenBi = 百分比,
                                    InstanceName = model.InstanceName,
                                    InstanceId = model.InstanceId,
                                    IP = model.PublicAddresses[0],
                                    TrafficPackageRemaining = int.Parse((r.TrafficPackageSet[0].TrafficPackageRemaining.Value / 1024 / 1024 / 1024).ToString()),
                                    TrafficUsed = int.Parse((r.TrafficPackageSet[0].TrafficUsed.Value / 1024 / 1024 / 1024).ToString()),
                                    TrafficPackageTotal = int.Parse((r.TrafficPackageSet[0].TrafficPackageTotal.Value / 1024 / 1024 / 1024).ToString()),
                                    InstanceState = InstanceState,
                                    Email = item.Email,
                                    Region = region,
                                    ExpireTime = DateTime.Parse(model.ExpiredTime)
                                });


                            }
                            catch (Exception)
                            {


                            }

                        }
                    }
                    catch (Exception)
                    {


                    }

                }

            }

            return list;
        }


        
    }
}
