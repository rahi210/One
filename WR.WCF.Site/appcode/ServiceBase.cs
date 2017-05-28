using System;
using System.ServiceModel;

using WR.Utils;

namespace WR.WCF.Site
{
    public class ServiceBase
    {
        protected LoggerEx log = null;

        public ServiceBase()
        {
            //初始化日志器
            log = LogService.Getlog(this.GetType());
        }

        protected FaultException GetFault(Exception ex)
        {
            return new FaultException(ex.Message);
        }
    }
}