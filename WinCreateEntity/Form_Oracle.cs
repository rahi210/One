using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Configuration;
using System.Text;
using System.Windows.Forms;

using System.Data.OracleClient;

namespace WinCreateEntity
{
    public partial class Form_Oracle : Form
    {
        public Form_Oracle()
        {
            InitializeComponent();
        }

        private void Form_Oracle_Load(object sender, EventArgs e)
        {
            textBox1.Text = ConfigurationManager.ConnectionStrings["OracleConnString_L"].ConnectionString;
            textBox2.Text = ConfigurationManager.AppSettings["AppFilePath"];
        }

        private DataTable SqlQuery(string sql)
        {
            DataTable dt = new DataTable();
            OracleConnection conn = new OracleConnection(textBox1.Text.Trim());
            OracleDataAdapter adp = new OracleDataAdapter(sql, conn);
            adp.Fill(dt);

            return dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT 1 cbx, table_name FROM User_Tables order by table_name";
            DataTable dt = SqlQuery(sql);
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
            //string sql = "select a.name colname,b.name typename from syscolumns a inner join SysTypes b on a.xtype=b.xtype where b.status=0 and a.id =";
            string sql = @"SELECT A.TABLE_NAME,
                               A.COLUMN_NAME,
                               A.DATA_TYPE,
                               A.DATA_LENGTH,
                               A.NULLABLE,a.DATA_PRECISION,a.DATA_SCALE,
                               CASE WHEN A.COLUMN_NAME = B.COLUMN_NAME THEN 1 ELSE 0  END column_key
                          FROM USER_TAB_COLUMNS A,
                               (SELECT A.CONSTRAINT_NAME, A.COLUMN_NAME, A.TABLE_NAME
                                  FROM USER_CONS_COLUMNS A, USER_CONSTRAINTS B
                                 WHERE A.CONSTRAINT_NAME = B.CONSTRAINT_NAME
                                   AND B.CONSTRAINT_TYPE = 'P') B
                         WHERE A.TABLE_NAME = B.TABLE_NAME(+)
                           AND A.TABLE_NAME = '{0}'
                         ORDER BY A.COLUMN_ID";

            StringBuilder sbt = new StringBuilder();

            foreach (DataRow item in dt.Rows)
            {
                if (item["cbx"].ToString() != "1")
                    continue;

                string className = GetEntName(item["table_name"].ToString());
                sbt.Length = 0;
                sbt.AppendLine("using System;");
                sbt.AppendLine("using System.Runtime.Serialization;");
                sbt.AppendLine("using System.ComponentModel.DataAnnotations;");
                sbt.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
                sbt.AppendLine("/********************************************");
                sbt.AppendLine(" * " + item["table_name"] + "实体类");
                sbt.AppendLine(" * ");
                sbt.AppendLine(" * *****************************************/");
                sbt.AppendLine("");
                sbt.AppendLine("namespace WR.WCF.DataContract");
                sbt.AppendLine("{");
                sbt.AppendLine("    [DataContract]");
                sbt.AppendLine("    [Table(\"" + item["table_name"].ToString() + "\")]");
                sbt.AppendLine("    public class " + className);
                sbt.AppendLine("    {");

                DataTable dtCols = SqlQuery(string.Format(sql,item["table_name"]));
                foreach (DataRow row in dtCols.Rows)
                {
                    sbt.AppendLine("        [DataMember]");
                    sbt.AppendLine("        public " + GetType(row) + " " + GetColName(row["COLUMN_NAME"].ToString()));
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

        public string GetType(DataRow dr)
        {
            string sqltype = dr["DATA_TYPE"].ToString().ToLower();
            if (sqltype == "char" || sqltype == "varchar2" || sqltype == "char" || sqltype == "nvarchar2" || sqltype == "long" || sqltype == "clob")
                return "string";
            else if (sqltype == "number")
            {
                if (int.Parse(dr["DATA_SCALE"].ToString().Trim()) > 0)
                    return "decimal";
                else if (int.Parse(dr["DATA_PRECISION"].ToString().Trim()) > 8)
                    return "Int64";
                else
                    return "int";
            }
            else if (sqltype == "DATE")
                return "DateTime";

            return "string";
        }
    }
}
