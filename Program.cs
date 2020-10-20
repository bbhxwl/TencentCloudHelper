using API;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TencentCloudHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            //自己简单的把自己项目关于良心云的api封装的拿出来简单的做了一下。 qq470138890 ，欢迎交流.net core技术
            if (args==null&&args.Length!=0)
            {
                Console.WriteLine("您没有输入SecretId和SecretKey");
                return;
            }
           
            for (int i = 0; i < args.Length; i=i+2)
            {
                TencentCloudApiCommon.l.Add(new API.Model.TencentCloudApiUser()
                {
                    SecretId = args[i],
                    SecretKey = args[i+1]
                });
            }
            Task.Factory.StartNew(()=> {
                Auto();
            });
            while (true)
            {

                Thread.Sleep(10000);
            }
            
        }

        public static   void Auto() {

            try
            {
                var list = TencentCloudApiCommon.GetAll();
                foreach (var item in list)
                {
                    try
                    {
                        if (item.BaiFenBi > 95)
                        {
                            if (item.InstanceState == "运行中")
                            {
                              TencentCloudApiCommon.Stop(item.Email, item.InstanceId, item.Region.Key);
                                
                             }
                        }
                        else
                        {
                            if (item.InstanceState == "关机")
                            {
                                TencentCloudApiCommon.Start(item.Email, item.InstanceId, item.Region.Key);
                             }
                        }
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
}
