using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WR.Client.Utils;
using System.IO;

namespace WR.Client.UI
{
    public partial class frm_merge : FormBase
    {
        public frm_merge()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                clbSinfs.Items.AddRange(openFileDialog1.FileNames);

                for (int i = 0; i < clbSinfs.Items.Count; i++)
                {
                    clbSinfs.SetItemChecked(i, true);
                }
            }
        }

        //合并SINF文件
        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (clbSinfs.CheckedItems.Count < 2)
            {
                MsgBoxEx.Info("Please select the merged file.");
                return;
            }

            //权限判断
            if (clbSinfs.CheckedItems.Count > 2)
            {
                var hasMerge = DataCache.Tbmenus.Count(s => s.MENUCODE == "40002") > 0;

                if (!hasMerge)
                {
                    MsgBoxEx.Info("Up to two files can be merged.");
                    return;
                }
            }

            List<SINFModel> sinfList = new List<SINFModel>();
            int count = 0;

            for (int i = 0; i < clbSinfs.CheckedItems.Count; i++)
            {
                var model = new SINFModel();

                model.DieArray = File.ReadAllLines(clbSinfs.CheckedItems[i].ToString());
                model.Path = clbSinfs.CheckedItems[i].ToString();
                model.Name = model.Path.Substring(model.Path.LastIndexOf("\\") + 1);

                if (model.DieArray.Length > 0)
                {
                    if (!string.IsNullOrEmpty(model.DieArray[2]))
                        model.Wafer = model.DieArray[2].Split(':')[1];

                    if (!string.IsNullOrEmpty(model.DieArray[4]))
                        model.Rows = int.Parse(model.DieArray[4].Split(':')[1]);

                    if (!string.IsNullOrEmpty(model.DieArray[5]))
                        model.Cols = int.Parse(model.DieArray[5].Split(':')[1]);

                    //7,8
                    model.RefDie = string.Format("{0},{1}", model.DieArray[7].Split(':')[1], model.DieArray[8].Split(':')[1]);
                }

                sinfList.Add(model);
            }


            //判断wafer是否相同
            count = sinfList.Select(s => s.Wafer).Distinct().Count();

            if (count != 1)
            {
                MsgBoxEx.Info("Wafer name is not equal.");
                return;
            }

            //判断layout是否一致
            count = sinfList.Select(s => new { s.Wafer, s.Rows, s.Cols }).Distinct().Count();

            if (count != 1)
            {
                MsgBoxEx.Info("The number of rows or columns is not equal.");
                return;
            }

            //ref die
            count = sinfList.Select(s => new { s.Wafer, s.Rows, s.Cols, s.RefDie }).Distinct().Count();

            if (count != 1)
            {
                MsgBoxEx.Info("The reference die is not equal.");
                return;
            }

            var newSINF = new List<string>();

            //合并
            foreach (var sinf in sinfList)
            {
                if (newSINF.Count == 0)
                    newSINF.AddRange(sinf.DieArray);
                else
                {
                    for (int i = 12; i < sinf.DieArray.Length; i++)
                    {
                        if (sinf.DieArray[i].Length == 0)
                            continue;

                        var newValue = MergeRow(newSINF[i], sinf.DieArray[i]);

                        if (newValue.Contains("error"))
                        {
                            MsgBoxEx.Info(string.Format("Numbers cannot be merged with non-numbers(--,@@),In line:{0},File Name:{1}.", i, sinf.Name));
                            return;
                        }

                        newSINF[i] = newValue;
                    }
                }
            }

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllLines(saveFileDialog1.FileName, newSINF.ToArray());

                MsgBoxEx.Info("Merge SINF file is complete.");
            }
        }

        private string MergeRow(string newRow, string oldRow)
        {
            var newRowArray = newRow.Split(':')[1].Split(' ');
            var oldRowArray = oldRow.Split(':')[1].Split(' ');

            if (newRowArray.Length != oldRowArray.Length)
                return string.Empty;

            for (int i = 0; i < newRowArray.Length; i++)
            {
                var newClassId = newRowArray[i];
                var oldClassId = oldRowArray[i];

                if (newClassId != oldClassId && newClassId != "error")
                {
                    if (newClassId == "__" || oldClassId == "__" || newClassId == "@@" || oldClassId == "@@")
                    {
                        newRowArray[i] = "error";
                    }
                    else
                    {
                        if (int.Parse(newClassId) < int.Parse(oldClassId))
                            newRowArray[i] = oldClassId;
                    }
                }
            }

            return string.Format("RowData:{0}", string.Join(" ", newRowArray));
        }
    }

    public class SINFModel
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public string Wafer { get; set; }

        public int Rows { get; set; }
        public int Cols { get; set; }

        public string RefDie { get; set; }

        public string[] DieArray { get; set; }
    }
}
