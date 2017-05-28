using System;
using System.Drawing;
using System.Windows.Forms;

namespace WR.Client.Controls
{
    public partial class WrMenuItem : UserControl
    {
        /// <summary>
        /// 菜单点击事件
        /// </summary>
        public event EventHandler ItemClick;

        public WrMenuItem()
        {
            InitializeComponent();
        }

        private Image _itemImage;
        /// <summary>
        /// 
        /// </summary>
        public Image ItemImage
        {
            get { return _itemImage; }
            set
            {
                _itemImage = value;
                lblItem.Image = _itemImage;
            }
        }

        private void lblItem_Click(object sender, EventArgs e)
        {
            //lblItem.BackColor = Color.Gray;
            if (ItemClick != null)
                ItemClick(this, e);
        }

        private string _wrText;
        /// <summary>
        /// 文本
        /// </summary>
        public string WrText
        {
            get { return _wrText; }
            set
            {
                _wrText = value;
                lblItem.Text = "        " + _wrText;
            }
        }

        private Color _itemBgColor;
        /// <summary>
        /// 
        /// </summary>
        public Color ItemBgColor
        {
            get { return _itemBgColor; }
            set
            {
                _itemBgColor = value;
                //lblItem.BackColor = _itemBgColor;
                this.BackColor = _itemBgColor;

                if (value == Color.Gray)
                    label5.Visible = false;
                else
                    label5.Visible = true;
            }
        }
    }
}
