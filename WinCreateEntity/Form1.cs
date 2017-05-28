using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using HSMS.DAL;

namespace WinCreateEntity
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = ConfigurationManager.ConnectionStrings["SqlConnString_L"].ConnectionString;
            textBox2.Text = ConfigurationManager.AppSettings["AppFilePath"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "select 1 cbx, id,name from sysobjects where xtype='U'";
            DataTable dt = SqlHelper.SqlQueryData(sql);
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt == null || dt.Rows.Count < 1)
            {
                MessageBox.Show("   没有可以生成的表   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            button2.Enabled = false;
            string sql = "select a.name colname,b.name typename from syscolumns a inner join SysTypes b on a.xtype=b.xtype where b.status=0 and a.id =";

            StringBuilder sbt = new StringBuilder();

            foreach (DataRow item in dt.Rows)
            {
                if (item["cbx"].ToString() != "1")
                    continue;

                string className = GetEntName(item["name"].ToString());
                sbt.Length = 0;
                sbt.AppendLine("using System;");
                sbt.AppendLine("using System.Runtime.Serialization;");
                sbt.AppendLine("using System.ComponentModel.DataAnnotations;");
                sbt.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
                sbt.AppendLine("/********************************************");
                sbt.AppendLine(" * " + item["name"] + "实体类");
                sbt.AppendLine(" * ");
                sbt.AppendLine(" * *****************************************/");
                sbt.AppendLine("");
                sbt.AppendLine("namespace WR.WCF.DataContract");
                sbt.AppendLine("{");
                sbt.AppendLine("    [DataContract]");
                sbt.AppendLine("    [Table(\"" + item["name"].ToString() + "\")]");
                sbt.AppendLine("    public class " + className);
                sbt.AppendLine("    {");

                DataTable dtCols = SqlHelper.SqlQueryData(sql + item["id"] + " order by colid");
                foreach (DataRow row in dtCols.Rows)
                {
                    sbt.AppendLine("        [DataMember]");
                    sbt.AppendLine("        public " + GetType(row["typename"].ToString()) + " " + GetColName(row["colname"].ToString()));
                    sbt.AppendLine("        { get;set; }");
                    sbt.AppendLine("");
                }
                sbt.AppendLine("    }");
                sbt.AppendLine("}");

                string filename = className + ".cs";
                string path = AppDomain.CurrentDomain.BaseDirectory.ToLower().Replace("\\bin\\debug\\", "") + textBox2.Text.Trim();
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (!File.Exists(Path.Combine(path, filename)))
                {
                    FileStream fs = File.Create(Path.Combine(path, filename));
                    fs.Close(); fs.Dispose();
                }
                //记入日志
                using (TextWriter writer = File.CreateText(Path.Combine(path, filename)))
                {
                    writer.Write(sbt.ToString());
                    writer.Close();
                }
            }

            button2.Enabled = true;
            MessageBox.Show("   成功生成   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string GetEntName(string name)
        {
            //string[] p = name.Split(new char[] { '_' });
            //return p[1] + "Entity";

            return name.Replace("_", "");
        }

        public string GetColName(string name)
        {
            string p = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name);
            return p;
        }

        public string GetType(string sqltype)
        {
            string type_ = "string";
            switch (sqltype.ToLower())
            {
                case "int":
                case "smallint":
                    type_ = "int";
                    break;
                case "decimal":
                case "numeric":
                case "float":
                case "money":
                    type_ = "decimal";
                    break;
                case "bit":
                    type_ = "bool";
                    break;
                case "nvarchar":
                case "varchar":
                case "nchar":
                case "char":
                case "ntext":
                case "text":
                    type_ = "string";
                    break;
                case "datetime":
                case "smalldatetime":
                    type_ = "DateTime";
                    break;
                case "uniqueidentifier":
                    type_ = "Guid";
                    break;
                default:
                    type_ = "string";
                    break;
            }            

            return type_;
        }
    }
}
