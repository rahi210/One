using System.Xml;
using System.Configuration;

namespace WR.Utils.Start
{
    public class Config
    {
        /// <summary>
        /// 配置参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <returns></returns>
        public static Configuration GetConfig()
        {
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public static string GetXmlValue(string path,string key)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList nodes = doc.GetElementsByTagName("appSettings")[0].ChildNodes;
            foreach (XmlNode node in nodes)
            {
                if (node.NodeType == XmlNodeType.Element && node.Attributes["key"].Value == key)
                    return node.Attributes["value"].Value;
            }

            return "";
        }
    }
}
