using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace NetCore.zh_hans
{
    public static class AppConfig
    {
        private static readonly string configPath;

        static AppConfig()
        {
            //获得配置文件的全路径
            configPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "app.json";
        }
        public static BaiduAccount GetBaiduAccount()
        {
            if (!File.Exists(configPath))
            {
                return new BaiduAccount();
            }

            string json = File.ReadAllText(configPath, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new BaiduAccount();
            }

            return JsonConvert.DeserializeObject<BaiduAccount>(json);
        }
        public static void SetBaiduAccount(string appid, string secret)
        {
            using FileStream fs = System.IO.File.Open(configPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);

            string json = JsonConvert.SerializeObject(new BaiduAccount
            {
                Appid = appid,
                Secret = secret
            });

            byte[] bytes = Encoding.UTF8.GetBytes(json);
            fs.Write(bytes, 0, bytes.Length);
            //刷新缓冲区
            fs.Flush();

            //  File.WriteAllText(configPath, json);
        }

    }
    public class BaiduAccount
    {
        public string Appid { get; set; }
        public string Secret { get; set; }
    }

}