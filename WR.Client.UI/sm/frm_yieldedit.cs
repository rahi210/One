using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;

namespace WR.Client.UI
{
    public partial class frm_yieldedit : FormBase
    {
        public bool IsAdd = false;
        public WMYIELDSETTING wmYield;

        public frm_yieldedit()
        {
            InitializeComponent();
        }

        private void frm_yieldedit_Load(object sender, EventArgs e)
        {
            if (IsAdd)
            {
                this.Text = "Create New Yield";
            }
            else
            {
                this.Text = "Update Yield";
                txtRecipeId.Text = wmYield.RECIPE_ID;
                nudLotYield.Value = wmYield.LOT_YIELD;
                nudWaferYield.Value = wmYield.WAFER_YIELD;

                nudAYield.Value = wmYield.MASKA_YIELD;
                nudBYield.Value = wmYield.MASKB_YIELD;
                nudCYield.Value = wmYield.MASKC_YIELD;
                nudDYield.Value = wmYield.MASKD_YIELD;
                nudEYield.Value = wmYield.MASKE_YIELD;

                txtImg.Text = wmYield.IMAGE_NAME;

                txtRecipeId.Enabled = false;

                SetYieldType(wmYield.YIELD_TYPE);
            }
        }

        private void SetYieldType(string type)
        {
            switch (type)
            {
                case "0":
                    rbRepice.Checked = true;
                    rbLayer.Checked = false;
                    rbDevice.Checked = false;
                    break;
                case "1":
                    rbRepice.Checked = false;
                    rbLayer.Checked = true;
                    rbDevice.Checked = false;
                    break;
                case "2":
                    rbRepice.Checked = false;
                    rbLayer.Checked = false;
                    rbDevice.Checked = true;
                    break;
                default:
                    rbRepice.Checked = true;
                    rbLayer.Checked = false;
                    rbDevice.Checked = false;
                    break;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtRecipeId.Text.Trim().Length < 1)
            {
                txtRecipeId.Focus();
                MsgBoxEx.Info("Please input recipe id");
                return;
            }

            IwrService service = wrService.GetService();
            var res = 0;

            var type = rbRepice.Checked ? "0" : rbLayer.Checked ? "1" : rbDevice.Checked ? "2" : "0";
            
            if (IsAdd)
            {
                res = service.AddYield(txtRecipeId.Text, type, nudLotYield.Value, nudWaferYield.Value, nudAYield.Value, nudBYield.Value, nudCYield.Value, nudDYield.Value, nudEYield.Value,txtImg.Text);
            }
            else
            {
                res = service.EditYield(txtRecipeId.Text, type, nudLotYield.Value, nudWaferYield.Value, nudAYield.Value, nudBYield.Value, nudCYield.Value, nudDYield.Value, nudEYield.Value,txtImg.Text);
            }

            if (res == -2)
            {
                MsgBoxEx.Info("The recipe id already exists");
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void btnImg_Click(object sender, EventArgs e)
        {
            OpenFileDialog sd = new OpenFileDialog();
            sd.Filter = "(*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jpg;*.png;*.jpeg;*.bmp;*.gif|All files(*.*)|*.*";

            IwrService service = wrService.GetService();

            if (DialogResult.OK == sd.ShowDialog())
            {
                var file = new UpFile();

                txtImg.Text = sd.SafeFileName;
                
                var stream = new System.IO.FileStream(sd.FileName, System.IO.FileMode.Open);
                file.FileName = sd.SafeFileName;
                file.FileStream = stream;
                service.UploadFile(file);

                stream.Flush();
                stream.Close();
            }
        }
    }
}
