using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCore.zh_hans
{
    public partial class FormTranslate : Form
    {
        public FormTranslate()
        {
            InitializeComponent();
        }

        private async void button_Import_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "zh-hans"); //指定翻译结果文件夹位置
            if (!Directory.Exists(path))//如果不存在就创建
            {
                var Gdir = new DirectoryInfo(Environment.CurrentDirectory);
                Gdir.CreateSubdirectory("zh-hans");
            }

            var openXml = new OpenFileDialog
            {
                Multiselect = true,
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet", "packs", "Microsoft.AspNetCore.App.Ref", "3.1.10", "ref", "netcoreapp3.1"),
                // InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet", "packs"),
                Filter = "要翻译的文件,可多选|*.xml"
            };
            if (openXml.ShowDialog() == DialogResult.OK)
            {
                var fileNames = openXml.FileNames; //获取文件列表

                if (fileNames.Length <= 0)
                {
                    MessageBox.Show("没有选择xml文件");
                }

                progressBar_Translate.Invoke((MethodInvoker)(() =>
                {
                    progressBar_Translate.Value = 0;//设置当前值
                    progressBar_Translate.Step = 1;//设置没次增长多少
                }));

                var translateDict = await GetTranslateDict(fileNames, textBox_appId.Text.Trim(), textBox_secretKey.Text.Trim());

                Invoke((MethodInvoker)(() =>
               {
                   button_Import.Text = "开始生成翻译后xml,请耐心等待...";
                   progressBar_Translate.Value = 0;//设置当前值
                   progressBar_Translate.Step = 1;//设置没次增长多少
               }));

                var list = new List<Task>();
                foreach (var fileName in fileNames)
                {
                    var task = ReplaceTask(fileName, translateDict, path);
                    list.Add(task);
                }

                await Task.WhenAll(list.ToArray());

                button_Import.Invoke((MethodInvoker)(() =>
                {
                    button_Import.Text = "翻译完成。";
                    button_Import.Enabled = true;
                }));

                //翻译全部完成后，自动打开zh_hans文件夹，然后把zh_hans复制到对应的资源文件夹下面即可
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
        }

        private async Task ReplaceTask(string fileName, Dictionary<string, string> translateDict, string basePath)
        {
            var readXmlList = await File.ReadAllTextAsync(fileName, Encoding.UTF8); //读取选定的Xml之一

            foreach (var translate in translateDict)
            {
                //原文+译文替换原文,后面换行加些空白字符保持格式一致性 
                readXmlList = readXmlList.Replace(translate.Key, $"{translate.Key}<para>{translate.Value}</para>");
            }

            //翻译结果文件夹位置
            var path = Path.Combine(basePath, Path.GetFileName(fileName));
            await using var swStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            await swStream.WriteAsync(Encoding.UTF8.GetBytes(readXmlList));
            await swStream.FlushAsync();
            swStream.Close();
        }

        private async Task<Dictionary<string, string>> GetTranslateDict(string[] fileNames, string appid, string secret)
        {
            var allText = (await GetAllText(fileNames)).ToList();
            var dict = new Dictionary<string, string>(allText.Count);

            progressBar_Translate.Invoke((MethodInvoker)(() =>
            {
                //设置进度条最大长度值
                progressBar_Translate.Maximum = allText.Count;

            }));

            const int bathCount = 200;

            var list = new List<Task>();

            var size = (allText.Count / bathCount) + 1;
            for (var i = 0; i < size; i++)
            {
                var data = allText.Skip(i * bathCount).Take(bathCount).ToList();
                if (!data.Any())
                {
                    break;
                }
                list.Add(Task.Run(async () =>
                {
                    foreach (var text in data)
                    {
                        Invoke((MethodInvoker)(() =>
                        {
                            progressBar_Translate.Value += progressBar_Translate.Step;//更新进度条

                            button_Import.Text = $"翻译文本：{text}";
                        }));

                        var translationText = await TranslateText(text, appid, secret);
                        dict[text] = translationText;
                    }
                }));
            }

            await Task.WhenAll(list.ToArray());

            return dict;
        }

        /// <summary>
        /// 获取所有待翻译的文本
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        private async Task<HashSet<string>> GetAllText(string[] fileNames)
        {
            var englishTextSet = new HashSet<string>(16 * 100);
            foreach (var fileName in fileNames)
            {
                button_Import.Invoke((MethodInvoker)(() =>
                {
                    button_Import.Text = $"正在导入{fileName}文本信息";
                }));

                var readXmlList = await File.ReadAllTextAsync(fileName, Encoding.UTF8); //读取选定的Xml之一
                var regexPatterns = new[]
                {
                    //匹配<summary>中间的英文说明</summary>
                    "(?<=(<summary>))[.\\s\\S]*?(?=(</summary>))",
                    //匹配<param name=""></param>
                    "(?<=(<param name=\"(.+?)\">))[.\\s\\S]*?(?=(</param>))",
                    //匹配<typeparam name=""></typeparam>
                    "(?<=(<typeparam name=\"(.+?)\">))[.\\s\\S]*?(?=(</typeparam>))",
                    //匹配<returns></returns>
                    "(?<=(<returns>))[.\\s\\S]*?(?=(</returns>))",
                };


                var matches = new List<Match>();
                foreach (var pattern in regexPatterns)
                {
                    var regex = new Regex(pattern);
                    var result = regex.Matches(readXmlList);
                    if (result.Count > 0)
                    {
                        for (var i = 0; i < result.Count; i++)
                        {
                            matches.Add(result[i]);
                        }
                    }
                }

                foreach (var match in matches)
                {
                    var text = match.Value?.Trim()
                        .Replace("\r\n", "")
                        .Replace("  ", " ")
                        .Trim('\n', ' ')
                        ;
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        englishTextSet.Add(text);
                    }
                }
            }

            return englishTextSet;
        }
        private static async Task<string> TranslateText(string text, string appid, string secret)
        {
            var okStr = await Translate.TranslateText(text, appid, secret); //执行翻译
            if (string.IsNullOrWhiteSpace(okStr))
            {
                return "";
            }

            okStr = okStr
                //移除多余的转义符，否则VS不会正确显示摘要; 这里被%#！坑惨了，翻译了全部AspNetCore文档总共130万字符才发现，阿西吧。。。
                .Replace("\\", "")
                .Replace("环境名称。开发", "EnvironmentName.Development")
                .Replace("环境名称。暂存", "EnvironmentName.Staging") //同上
                .Replace("环境名称。生产", "EnvironmentName.Production") //同上
                .Replace("系统。异常", "System.Exception") //同上
                .Replace("“", "\"") //同上
                .Replace("”", "\""); //同上

            //至此接近完美了，硬伤是百度翻译词义的准确性，GoogleTranslate很不错，可惜我申请不了接口，要绑定支付方式，尝试过用协议调用https://translate.google.cn/，单位时间内对IP有翻译次数限制，挂代理速度及稳定性又很无奈，先将就下吧。。。

            return okStr;
        }

        private void FormTranslate_Load(object sender, EventArgs e)
        {
            var baidu = AppConfig.GetBaiduAccount();
            textBox_appId.Text = baidu.Appid;
            textBox_secretKey.Text = baidu.Secret;
        }

        private void FormTranslate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_appId.Text.Trim())
                || string.IsNullOrWhiteSpace(textBox_secretKey.Text.Trim()))
            {
                return;
            }

            AppConfig.SetBaiduAccount(textBox_appId.Text.Trim(), textBox_secretKey.Text.Trim());
        }

    }
}
