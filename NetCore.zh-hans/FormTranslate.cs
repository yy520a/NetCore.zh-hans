using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

        private void button_Import_Click(object sender, EventArgs e)
        {

            button_Import.Text = "正在翻译中...";

            string path = Environment.CurrentDirectory + "\\" + "zh-hans"; //指定翻译结果文件夹位置
            if (!Directory.Exists(path))//如果不存在就创建
            {
                DirectoryInfo Gdir = new DirectoryInfo(Environment.CurrentDirectory);
                Gdir.CreateSubdirectory("zh-hans");
            }

            OpenFileDialog openXml = new OpenFileDialog
            {
                Multiselect = true,
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet", "packs"),
                Filter = "要翻译的文件,可多选|*.xml"
            };
            if (openXml.ShowDialog() == DialogResult.OK)
            {
                string[] fileNames = openXml.FileNames; //获取文件列表

                progressBar_Translate.Invoke((MethodInvoker)(() =>
                {
                    progressBar_Translate.Maximum = fileNames.Length;//设置进度条最大长度值
                    progressBar_Translate.Value = 0;//设置当前值
                    progressBar_Translate.Step = 1;//设置没次增长多少
                }));

                Thread thread_Translate = new Thread(delegate ()
                {
                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        progressBar_Translate.Invoke((MethodInvoker)(() =>
                        {
                            progressBar_Translate.Value += progressBar_Translate.Step;//更新进度条
                        }));

                        string ReadXmlList = File.ReadAllText(fileNames[i], Encoding.UTF8).Trim(); //读取选定的Xml之一

                        MatchCollection MatchVar;
                        Regex MatchPic = new Regex("(?<=(" + "<summary>" + "))[.\\s\\S]*?(?=(" + "</summary>" + "))");//筛选标准
                        MatchVar = MatchPic.Matches(ReadXmlList);//匹配<summary>中间的英文说明</summary>
                        if (MatchVar.Count >= 1)
                        {
                            List<string> repeatList = new List<string>();//防重复翻译列表
                            for (int k = 0; k < MatchVar.Count; k++)
                            {
                                if (!repeatList.Contains(MatchVar[k].Value.Trim())) //判断是否已翻译过的字符串
                                {
                                    repeatList.Add(MatchVar[k].Value.Trim());//添加到防重复翻译列表
                                    string HWC = MatchVar[k].Value.Trim().Replace("\r\n", "");//处理掉字符中间的换行符
                                    string okStr = Translate.TranslateText(HWC, textBox_appId.Text.Trim(), textBox_secretKey.Text.Trim()); //执行翻译
                                    if (okStr != null)
                                    {
                                        okStr = okStr
                                            .Replace("\\", "") //移除多余的转义符，否则VS不会正确显示摘要; 这里被%#！坑惨了，翻译了全部AspNetCore文档总共130万字符才发现，阿西吧。。。
                                            .Replace("环境名称。开发", "EnvironmentName.Development") //同上
                                            .Replace("环境名称。暂存", "EnvironmentName.Staging") //同上
                                            .Replace("环境名称。生产", "EnvironmentName.Production") //同上
                                            .Replace("系统。异常", "System.Exception") //同上
                                            .Replace("“", "\"") //同上
                                            .Replace("”", "\""); //同上
                                        //至此接近完美了，硬伤是百度翻译词义的准确性，GoogleTranslate很不错，可惜我申请不了接口，要绑定支付方式，尝试过用协议调用https://translate.google.cn/，单位时间内对IP有翻译次数限制，挂代理速度及稳定性又很无奈，先将就下吧。。。

                                        //原文+译文替换原文,后面换行加些空白字符保持格式一致性 
                                        ReadXmlList = ReadXmlList.Replace(MatchVar[k].Value, $"{MatchVar[k].Value}<para>{okStr}</para>{Environment.NewLine}");
                                    }
                                }
                            }

                            string saveFilePath = path + "\\" + Path.GetFileName(fileNames[i]);//保存翻译后的文档
                            StreamWriter swStream;
                            if (File.Exists(saveFilePath))
                            {
                                swStream = new StreamWriter(saveFilePath, false);
                            }
                            else
                            {
                                swStream = File.CreateText(saveFilePath);
                            }

                            swStream.Write(ReadXmlList);
                            swStream.Flush();
                            swStream.Close();
                        }
                    }
                    button_Import.Invoke((MethodInvoker)(() =>
                    {
                        button_Import.Text = "翻译完成。";
                    }));

                    System.Diagnostics.Process.Start("explorer.exe", path);   //翻译全部完成后，自动打开zh_hans文件夹，然后把zh_hans复制到对应的资源文件夹下面即可
                });
                thread_Translate.IsBackground = true;
                thread_Translate.Name = "Translate";
                thread_Translate.Start();
            }
        }


        private void FormTranslate_Load(object sender, EventArgs e)
        {
            BaiduAccount baidu = AppConfig.GetBaiduAccount();
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
