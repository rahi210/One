using System.ServiceModel;

namespace WR.Client.Utils
{
    //public abstract class ClientBase<TChannel> : ICommunicationObject, IDisposable where TChannel : class
    public abstract class WCFService<Iservice> : ClientBase<Iservice> where Iservice : class
    {
        //public WCFService():base(EndPointEx.wsHttpBinding,EndPointEx.endpointAddress)
        //{
        //}

        public WCFService()
            : base(EndPointEx<Iservice>.GetBinding(), EndPointEx<Iservice>.GetAddress())
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
        public Iservice GetChannel()
        {
            return base.Channel;
        }
    }
}
