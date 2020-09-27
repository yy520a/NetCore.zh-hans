using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NetCore.zh_hans
{
    class AppXml
    {

        public static void XmlWrite(string tagName, string Valuex)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                //获得配置文件的全路径　　  
                string strFileName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "App.xml";
                //string strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "NotesAccess.exe.config";
                doc.Load(strFileName);
                //找出名称为“add”的所有元素　　  
                XmlNodeList nodes = doc.GetElementsByTagName("add");
                for (int i = 0; i < nodes.Count; i++)
                {
                    //获得将当前元素的key属性　　　　  
                    XmlAttribute att = nodes[i].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素　　　　  
                    if (att.Value == tagName)
                    {
                        //对目标元素中的第二个属性赋值　　　　　  
                        att = nodes[i].Attributes["value"];
                        att.Value = Valuex;
                        break;
                    }
                }
                //保存上面的修改　　  
                doc.Save(strFileName);
            }
            catch { }
        }

        public static string XmlRead(string tagName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                //获得配置文件的全路径　　  
                string strFileName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "App.xml";
                //string strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "NotesAccess.exe.config";
                doc.Load(strFileName);
                //找出名称为“add”的所有元素　　  
                XmlNodeList nodes = doc.GetElementsByTagName("add");
                for (int i = 0; i < nodes.Count; i++)
                {
                    //获得将当前元素的key属性　　　　  
                    XmlAttribute att = nodes[i].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素　　　　  
                    if (att.Value == tagName)
                    {
                        //对目标元素中的第二个属性赋值　　　　　  
                        att = nodes[i].Attributes["value"];
                        return att.Value;
                    }
                }
                return string.Empty;
            }
            catch { return string.Empty; }

        }


    }
}
