using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NetCore.zh_hans
{
    internal class Translate
    {
        // 计算MD5值
        public static string EncryptString(string str)
        {
            var md5 = MD5.Create();
            // 将字符串转换成字节数组
            var byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            var byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            var sb = new StringBuilder();
            foreach (var b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }

        private static readonly HttpClient _httpClient = new HttpClient(new SocketsHttpHandler());
        /// <summary>
        /// 调用百度翻译API并返回结果
        /// </summary>
        /// <param name="str">要翻译的英文字符</param>
        /// <param name="inputAppId">百度翻译接口|AppId </param>
        /// <param name="inputSecretKey">百度翻译接口|SecretKey </param>
        /// <returns>返回的翻译结果</returns>
        public static async Task<string> TranslateText(string str, string inputAppId, string inputSecretKey)
        {
            // 源语言
            var from = "en";
            // 目标语言
            var to = "zh";
            // 改成您的APP ID
            var appId = inputAppId;
            var rd = new Random();
            var salt = rd.Next(100000).ToString();
            // 改成您的密钥
            var secretKey = inputSecretKey;
            var sign = EncryptString(appId + str + salt + secretKey);
            var url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(str);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;

            var retString = "";
            try
            {
                retString = await _httpClient.GetStringAsync(url);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"translate [{str}] error,{e.Message}");
            }

            if (string.IsNullOrWhiteSpace(retString))
            {
                Debug.WriteLine($"translate [{str}] error, content is null");
                return null;
            }

            var result = JsonConvert.DeserializeObject<TranslateResult>(retString);

            if (result.error_code != 0)
            {
                Debug.WriteLine($"translate [{str}] error, {retString}");
                return null;
            }

            return result.trans_result.FirstOrDefault()?.dst;

            //返回值做一下处理 
            //var regex = new Regex("(?<=(" + "dst\":\"" + "))[.\\s\\S]*?(?=(" + "\"}]" + "))");//筛选标准
            //var matchVar = regex.Matches(retString);//匹配返回的Unicode结果
            //if (matchVar.Count <= 0)
            //{
            //    return null;
            //}

            //var unicodeStr = matchVar[0].Value;
            //var resultsOkStr = UnicodeToString(unicodeStr);
            //return resultsOkStr;

        }

        //https://api.fanyi.baidu.com/api/trans/product/apidoc
        public class TranslateResult
        {
            public int error_code { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public Trans_Result[] trans_result { get; set; }
        }

        public class Trans_Result
        {
            /// <summary>
            /// 原文
            /// </summary>
            public string src { get; set; }
            /// <summary>
            /// 译文
            /// </summary>
            public string dst { get; set; }
        }


        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string UnicodeToString(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public static string String2Unicode(string source)
        {
            var bytes = Encoding.Unicode.GetBytes(source);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }


    }
}
