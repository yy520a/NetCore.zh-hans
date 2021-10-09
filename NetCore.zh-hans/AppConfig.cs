using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetCore.zh_hans
{
    public static class AppConfig
    {
        private static readonly string configPath;
        private static readonly string dataPath;

        static AppConfig()
        {
            //获得配置文件的全路径
            configPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "app.json";
            dataPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data.json";
        }
        public static BaiduAccount GetBaiduAccount()
        {
            if (!File.Exists(configPath))
            {
                return new BaiduAccount();
            }

            var json = File.ReadAllText(configPath, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new BaiduAccount();
            }

            return JsonConvert.DeserializeObject<BaiduAccount>(json);
        }
        public static void SetBaiduAccount(string appid, string secret)
        {
            using var fs = System.IO.File.Open(configPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);

            var json = JsonConvert.SerializeObject(new BaiduAccount
            {
                Appid = appid,
                Secret = secret
            });

            var bytes = Encoding.UTF8.GetBytes(json);
            fs.Write(bytes, 0, bytes.Length);
            //刷新缓冲区
            fs.Flush();

            //  File.WriteAllText(configPath, json);
        }


        public static void SetTranslateData(IDictionary<string, string> data)
        {
            using var fs = System.IO.File.Open(dataPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);

            var json = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(json);
            fs.Write(bytes, 0, bytes.Length);
            //刷新缓冲区
            fs.Flush();
        }

        public static IDictionary<string, string> GetTranslateData()
        {
            if (!File.Exists(dataPath))
            {
                return new Dictionary<string, string>();
            }

            var json = File.ReadAllText(dataPath, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new Dictionary<string, string>();
            }

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

    }
    public class BaiduAccount
    {
        public string Appid { get; set; }
        public string Secret { get; set; }
    }

}