using WR.Client.Utils;
using WR.WCF.Contract;
using WR.Utils;

using System.ServiceModel.Description;

namespace WR.Client.WCF
{
    public class wrService : WCFService<IwrService>
    {
        private static LoggerEx log = null;

        public wrService()
        { }

        public static IwrService GetService()
        {

            try
            {
                wrService ws = new wrService();
                foreach (var item in ws.Endpoint.Contract.Operations)
                {
                    DataContractSerializerOperationBehavior dc = item.Behaviors.Find<DataContractSerializerOperationBehavior>() as DataContractSerializerOperationBehavior;
                    if (dc != null)
                        dc.MaxItemsInObjectGraph = int.MaxValue;
                }

                return ws.GetChannel();
                //return new wrService().GetChannel();
            }
            catch (System.Exception ex)
            {
                if (log == null)
                    log = LogService.Getlog(typeof(wrService));

                log.Error(ex);

                throw;
            }
        }
    }
}
