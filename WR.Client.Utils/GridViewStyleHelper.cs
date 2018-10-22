using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace WR.Client.Utils
{
    public class GridViewStyleHelper
    {
        /// <summary>
        /// 获取表格样式
        /// </summary>
        /// <param name="dgvMain"></param>
        public static void LoadDataGridViewStyle(DataGridView dgvData)
        {
            string strFromName = dgvData.FindForm().Name;
            try
            {
                string XMLPath;
                XMLPath = Application.StartupPath + @"\\GridViewStyle" + "\\";
                string FileName = XMLPath + strFromName + "_" + dgvData.Name + ".xml";

                if (!Directory.Exists(XMLPath))
                    Directory.CreateDirectory(XMLPath);

                //如果不存在配置文件，则保存。
                if (!File.Exists(FileName))
                {
                    SaveDataGridViewStyle(dgvData);
                }

                DataTable dtStyle = new DataTable();
                dtStyle.TableName = dgvData.Name;
                dtStyle.Columns.Add("Name");  //列名
                dtStyle.Columns.Add("HeaderText");  //标题
                dtStyle.Columns.Add("Width");  //宽度 
                dtStyle.Columns.Add("Visble");  //是否显示               
                dtStyle.Columns.Add("DisplayIndex");  //显示顺序

                dtStyle.ReadXml(FileName);

                foreach (DataRow row in dtStyle.Rows)
                {
                    dgvData.Columns[row["Name"].ToString().Trim()].HeaderText = row["HeaderText"].ToString().Trim();
                    dgvData.Columns[row["Name"].ToString().Trim()].Width = int.Parse(row["Width"].ToString().Trim());
                    dgvData.Columns[row["Name"].ToString().Trim()].Visible = Boolean.Parse(row["Visble"].ToString().Trim());
                    dgvData.Columns[row["Name"].ToString().Trim()].DisplayIndex = int.Parse(row["DisplayIndex"].ToString().Trim());
                }

                //允许用户手动排序列。
                dgvData.AllowUserToOrderColumns = true;
                //列的宽度改变时候触发自动保存事件。
                dgvData.ColumnWidthChanged += new DataGridViewColumnEventHandler(dgvMain_ColumnWidthChanged);
                //列的显示位置改变时候触发自动保存事件。
                dgvData.ColumnDisplayIndexChanged += new DataGridViewColumnEventHandler(dgvMain_ColumnDisplayIndexChanged);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void dgvMain_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            SaveDataGridViewStyle(e.Column.DataGridView);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void dgvMain_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            SaveDataGridViewStyle(e.Column.DataGridView);
        }

        /// <summary>
        /// 保存用户自定义列表顺序
        /// </summary>
        /// <param name="DgvMain">DataGridView名称</param>
        private static void SaveDataGridViewStyle(DataGridView dgvData)
        {
            try
            {
                string strFormName = dgvData.FindForm().Name;
                string XMLPath;
                XMLPath = Application.StartupPath + @"\\GridViewStyle" + "\\";
                string FileName = XMLPath + strFormName + "_" + dgvData.Name + ".xml";  //生成文件到指定目录
                DataTable DTname = new DataTable();
                DTname.TableName = dgvData.Name;
                DTname.Columns.Add("Name");  //列名
                DTname.Columns.Add("HeaderText");  //标题
                DTname.Columns.Add("Width");  //宽度 
                DTname.Columns.Add("Visble");  //是否显示             
                DTname.Columns.Add("DisplayIndex");  //显示顺序              


                string[] array = new string[dgvData.Columns.Count];
                //获取Visble =true 的列  
                foreach (DataGridViewColumn column in dgvData.Columns)
                {
                    if (column.Visible == true)
                    {
                        //  拖动列顺序 
                        array[column.DisplayIndex] = column.Name + '|' + column.HeaderText + '|' + column.Width + '|' + column.Visible + '|' + column.DisplayIndex;
                    }
                }
                int ColumnsCount = array.Length;
                //取列属性
                for (int i = 0; i < ColumnsCount; i++)
                {
                    string[] str = new string[5];
                    try
                    {
                        DataRow row = DTname.NewRow();
                        str = array.GetValue(i).ToString().Split('|'); //分隔
                        row["Name"] = str[0];
                        row["HeaderText"] = str[1];
                        row["Width"] = str[2];
                        row["Visble"] = str[3];
                        row["DisplayIndex"] = str[4];
                        DTname.Rows.Add(row);
                        DTname.AcceptChanges();
                    }
                    catch
                    {
                        continue;
                    }
                }
                DTname.WriteXml(FileName);
            }
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        ///删除指定的XML文件
        /// </summary>
        /// <param name="dgvMain"></param>
        /// <returns></returns>
        public static bool DeleteDataGridViewStyle(DataGridView dgvMain)
        {
            string strFormName = dgvMain.FindForm().Name;
            string XMLPath = Application.StartupPath + @"\\GridViewStyle" + "\\";
            string FileName = XMLPath + strFormName + "_" + dgvMain.Name + ".xml";
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
