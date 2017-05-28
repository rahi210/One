using System.Resources;
using System.Windows.Forms;
using WR.Client.Resoures;

namespace WR.Client.Utils
{
    public class ResourceHepler
    {
        /// <summary>
        /// 语言简称
        /// </summary>
        private static string _lang = "en";

        /// <summary>
        /// 资源变量
        /// </summary>
        private static ResourceManager resManager;


        public static void SetLang(Control ctrl)
        {
            if (resManager == null)
                resManager = new ResourceManager("WR.Client.Resoures.WrResource",
                typeof(WrResource).Assembly);



            string txt = resManager.GetString("");

        }

        /// <summary>
        /// 获取资源配置字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            if (resManager == null)
                resManager = new ResourceManager("WR.Client.Resoures.WrResource",
                typeof(WrResource).Assembly);

            return resManager.GetString(string.Format("{0}_{1}", key, _lang));
        }
    }
}
