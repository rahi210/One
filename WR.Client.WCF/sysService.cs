using WR.Client.Utils;
using WR.WCF.Contract;

namespace WR.Client.WCF
{
    // [AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Allowed)]
    public class sysService : WCFService<IsysService>
    {
        public sysService()
        { }

        public static IsysService GetService()
        {
            return new sysService().GetChannel();
        }
    }
}
