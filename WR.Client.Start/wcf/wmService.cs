using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using WR.Utils.Start;

namespace WR.Client.Start
{
    //public abstract class ClientBase<TChannel> : ICommunicationObject, IDisposable where TChannel : class
    public class wmService : ClientBase<WR.WCF.Client.Contract.IwmService>
    {
        public static Binding GetBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            binding.Security.Mode = BasicHttpSecurityMode.None;

            binding.OpenTimeout = new TimeSpan(0, 0, 1);

            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            return binding;
        }

        public static EndpointAddress GetAddress()
        {
            string http = Config.GetXmlValue(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Start.exe.config"), "RemoteURL");
            Uri url = new Uri(string.Format("{0}/{1}.svc", http.TrimEnd('/'), "wmService"));
            return new EndpointAddress(url);
        }

        public wmService()
            : base(GetBinding(), GetAddress())
        {
        }

        /// <summary>
        /// 检查服务器连接是否正常
        /// </summary>
        /// <returns></returns>
        public bool CheckConnect()
        {
            try
            {
                base.Open();
                base.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取连接通道
        /// </summary>
        /// <returns></returns>
        public WR.WCF.Client.Contract.IwmService GetChannel()
        {
            return base.Channel;
        }
    }
}
