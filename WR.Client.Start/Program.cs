using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using WR.Utils.Start;

namespace WR.Client.Start
{
    static class Program
    {
        private static LoggerEx log = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {


                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomain_UnhandledException);

                LogService.InitializeService(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Start.exe.config"));
                log = LogService.Getlog(typeof(Program));
                log.Fatal("======>=Start up=>=====");

                #region 检查是否有服务器地址
                string url = Config.GetXmlValue(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Start.exe.config"), "RemoteURL");
                if (string.IsNullOrEmpty(url))
                {
                    frm_Set frmSet = new frm_Set();
                    frmSet.ShowDialog();
                }

                url = Config.GetXmlValue(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Start.exe.config"), "RemoteURL");
                if (string.IsNullOrEmpty(url))
                {
                    MessageBox.Show("没有指定服务器地址，不能启动程序！", "信息提示", MessageBoxButtons.OK);
                    return;
                }
                #endregion

                #region 检查是否有更新文件
                using (frm_update set = new frm_update())
                {
                    if (set.ShowDialog() != DialogResult.OK)
                        return;
                }
                #endregion

                //System.Diagnostics.Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WR.Pos.exe"));
                Form frm = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "\\WR.Client.UI.dll").CreateInstance("WR.Client.UI.frm_main") as Form;
                if (frm != null)
                {
                    Application.Run(frm);
                    log.Fatal("=========<=Shut down=<============");
                }
                else
                    log.Fatal("not find Assembly");

                Application.DoEvents();
                Application.Exit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show("操作中出现错误！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            log.Error(e.Exception);
            MessageBox.Show("操作中出现错误！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //可以记录日志并转向错误bug窗口友好提示用户
            Exception ex = e.ExceptionObject as Exception;

            log.Error(ex.Message);
            MessageBox.Show("操作中出现错误！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
