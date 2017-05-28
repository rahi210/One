using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;

namespace WR.Client.UI
{
    public partial class frm_options : FormBase
    {
        public ppG tbm = new ppG();

        public frm_options()
        {
            InitializeComponent();
        }

        private void frm_options_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = tbm;
        }


    }


    public class ppG
    {
        private bool _Surface = true;
        public bool Surface
        {
            set { _Surface = value; }
            get { return _Surface; }
        }

        private bool _Id = true;
        public bool Id
        {
            set { _Id = value; }
            get { return _Id; }
        }
        private bool _ADC = true;
        public bool ADC
        {
            set { _ADC = value; }
            get { return _ADC; }
        }

        private bool _Updated = true;
        public bool Updated
        {
            set { _Updated = value; }
            get { return _Updated; }
        }

        private bool _Image = true;
        public bool Image
        {
            set { _Image = value; }
            get { return _Image; }
        }

        private bool _ClassId = true;
        public bool ClassId
        {
            set { _ClassId = value; }
            get { return _ClassId; }
        }

        private bool _Description = true;
        public bool Description
        {
            set { _Description = value; }
            get { return _Description; }
        }

        private bool _XRel = true;
        public bool XRel
        {
            set { _XRel = value; }
            get { return _XRel; }
        }

        private bool _YRel = true;
        public bool YRel
        {
            set { _YRel = value; }
            get { return _YRel; }
        }

        private bool _DefectRow = false;
        public bool DefectRow
        {
            set { _DefectRow = value; }
            get { return _DefectRow; }
        }

        private bool _DefectColumn = false;
        public bool DefectColumn
        {
            set { _DefectColumn = value; }
            get { return _DefectColumn; }
        }
    }
}
