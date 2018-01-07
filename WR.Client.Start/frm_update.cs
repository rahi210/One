using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

using WR.Client.Start;
using WR.WCF.Client.DataContract;
using WR.Utils.Start;

namespace WR.Client.Start
{
    public partial class frm_update : Form
    {
        private LoggerEx log = null;

        public frm_update()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint, true);

            log = LogService.Getlog(this.GetType());
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            Rectangle rect = new Rectangle(Point.Empty, this.Size);
            int radius = 12;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(
                rect.Right - radius,
                rect.Y,
                radius,
                radius,
                270,
                90);
            path.AddArc(
                rect.Right - radius,
                rect.Bottom - radius,
                radius,
                radius, 0, 90);
            path.AddArc(
                rect.X,
                rect.Bottom - radius,
                radius,
                radius,
                90,
                90);

            if (base.Region != null)
            {
                base.Region.Dispose();
            }
            base.Region = new Region(path);
        }

        private void frm_update_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private bool AppUpdate()
        {
            try
            {
                string pathTmp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "update");

                //上次更新日期
                string updateDate = Config.GetXmlValue(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Start.exe.config"), "AppUpdate");
                DateTime date_ = DateTime.MinValue;
                //如果更新日期格式错误，初始最早日期
                if (!DateTime.TryParseExact(updateDate, "yyyy-MM-dd HH:mm:ss",
                    System.Globalization.CultureInfo.GetCultureInfo("zh-CN"),
                    System.Globalization.DateTimeStyles.None,
                    out date_))
                    date_ = DateTime.ParseExact("20130101", "yyyyMMdd",
                        System.Globalization.CultureInfo.GetCultureInfo("zh-CN"));

                updateDate = date_.ToString("yyyyMMddHHmmss");

                wmService service = new wmService();
                List<FileEntity> files = service.GetChannel().GetAppFilesList(updateDate);

                if (files.Count > 0)
                {
                    progressBarX1.Text = "Upgrade file found......";
                    progressBarX1.Maximum = files.Count;

                    this.Text = "System is upgrading...";
                    this.Update();

                    int buffersize = 1024;
                    byte[] buffer = new byte[buffersize];
                    int count = 0;

                    //如果有更新文件，建立保存文件的临时目录
                    if (Directory.Exists(pathTmp))
                        Directory.Delete(pathTmp, true);

                    Directory.CreateDirectory(pathTmp);
                    //记录本次更新日期
                    DateTime dtMax = DateTime.MinValue;

                    int currfile = 1;
                    foreach (FileEntity file in files)
                    {
                        if (dtMax < file.LastTime)
                            dtMax = file.LastTime;

                        progressBarX1.Value = currfile;
                        progressBarX1.Text = string.Format("upgrading[{0}/({1}/{2})]...", file.FileName, currfile, files.Count);
                        log.Fatal(string.Format("upgrade file[{0}]/Ver.Time[{1:yyyy-MM-dd HH:mm:ss}]", file.FileName, file.LastTime));

                        //下载更新文件
                        Stream stream = service.GetChannel().DownloadFileStream(file.FileName, file.MapPath);

                        FileStream fstream = new FileStream(Path.Combine(pathTmp, file.FileName), FileMode.CreateNew, FileAccess.Write, FileShare.Write);
                        while ((count = stream.Read(buffer, 0, buffersize)) > 0)
                        {
                            fstream.Write(buffer, 0, count);
                        }
                        fstream.Flush();
                        fstream.Close();

                        currfile++;
                    }

                    System.Configuration.Configuration config = Config.GetConfig();
                    config.AppSettings.Settings.Remove("AppUpdate");
                    config.AppSettings.Settings.Add("AppUpdate", dtMax.ToString("yyyy-MM-dd HH:mm:ss"));
                    config.Save();

                    progressBarX1.Text = "Upgrade is complete, Starting......";
                    System.Threading.Thread.Sleep(100);

                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Error in upgrade system:");
                log.Fatal(ex);
                //MessageBox.Show("Error in upgrade system！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (ex.Message.Contains("SOAP"))
                {
                    if (DialogResult.Yes == MessageBox.Show("Network configuration may be wrong, whether to re configure?", "Information", MessageBoxButtons.YesNo))
                    {
                        frm_Set frmSet = new frm_Set();
                        if (frmSet.ShowDialog() == DialogResult.OK)
                        {
                            MessageBox.Show("Please run the program again.", "Information");
                        }
                    }
                }
            }

            return false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            string auto = Config.GetXmlValue(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Start.exe.config"), "AutoUpdate");
            if (auto != "1")
            {
                DialogResult = DialogResult.OK;
                return;
            }

            if (AppUpdate())
            {
                string pathTmp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "update");
                if (Directory.Exists(pathTmp))
                {
                    string[] files = Directory.GetFiles(pathTmp);
                    foreach (string file in files)
                    {
                        FileInfo fileinfo = new FileInfo(file);
                        string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileinfo.Name);
                        if (File.Exists(filename))
                            File.Delete(filename);

                        fileinfo.MoveTo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileinfo.Name));
                    }

                    Directory.Delete(pathTmp, true);
                }

                DialogResult = DialogResult.OK;
            }
            else
                DialogResult = DialogResult.No;
        }
    }
}
