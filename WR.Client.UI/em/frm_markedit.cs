using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WR.WCF.DataContract;
using WR.WCF.Contract;
using WR.Client.WCF;

namespace WR.Client.UI
{
    public partial class frm_markedit : Form
    {
        public bool IsAdd = false;
        public EMCLASSIFICATIONMARK emMark;

        public frm_markedit()
        {
            InitializeComponent();
        }

        private void frm_examedit_Load(object sender, EventArgs e)
        {
            this.Text = "Update";
            txtID.Enabled = false;
            txtName.Enabled = false;

            if (emMark != null)
            {
                txtID.Text = emMark.CID;
                txtName.Text = emMark.NAME;
                nudMark.Value = emMark.MARK;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            IwrService service = wrService.GetService();

            service.EditCLASSIFICATIONMARK(txtID.Text, txtName.Text, (int)nudMark.Value);

            DialogResult = DialogResult.OK;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}
