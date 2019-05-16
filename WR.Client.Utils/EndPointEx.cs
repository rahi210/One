using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;

using WR.Utils;

namespace WR.Client.Utils
{
    public static class EndPointEx<T>
    {
        public static Binding GetBinding()
        {
            //WSHttpBinding binding = new WSHttpBinding();
            //binding.MaxReceivedMessageSize = 67108864;
            //binding.Security.Mode = SecurityMode.None;
            //NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            //binding.MaxReceivedMessageSize = 67108864;
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            binding.Security.Mode = BasicHttpSecurityMode.None;
            //binding.TransferMode = TransferMode.Streamed;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            //binding.UseDefaultWebProxy = false;
            binding.SendTimeout = new TimeSpan(0, 30, 0);
            binding.OpenTimeout = new TimeSpan(0, 30, 0);

            return binding;
        }

        public static EndpointAddress GetAddress()
        {
            string http = Config.GetXmlValue(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Start.exe.config"), "RemoteURL");
            Uri url = new Uri(string.Format("{0}/{1}.svc", http.TrimEnd('/'), typeof(T).Name.TrimStart('I').TrimStart('i')));
            return new EndpointAddress(url);
        }
    }
}
