using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CRD.WinUI.Editors
{
    public class WrDataGridView : System.Windows.Forms.DataGridView
    {
        public WrDataGridView()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnCreateControl()
        {
            this.EnableHeadersVisualStyles = false;
            this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247, 246, 239);
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            this.ColumnHeadersHeight = 26;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.RowHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.RowHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            this.RowHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DefaultCellStyle.SelectionBackColor = Color.Wheat;
            this.DefaultCellStyle.SelectionForeColor = Color.DarkSlateBlue;
            this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BackgroundColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.AllowUserToOrderColumns = true;
            this.AutoGenerateColumns = true;

            base.OnCreateControl();
        }

        Color defaultcolor;

        //移到单元格时的颜色
        protected override void OnCellMouseMove(DataGridViewCellMouseEventArgs e)
        {

            base.OnCellMouseMove(e);
            try
            {
                Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.YellowGreen;

            }
            catch (Exception)
            {

            }
        }

        //进入单元格时保存当前的颜色
        protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
        {
            base.OnCellMouseEnter(e);
            try
            {
                defaultcolor = Rows[e.RowIndex].DefaultCellStyle.BackColor;
            }
            catch (Exception)
            {

            }
        }

        //离开时还原颜色
        protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
        {
            base.OnCellMouseLeave(e);
            try
            {
                Rows[e.RowIndex].DefaultCellStyle.BackColor = defaultcolor;
            }
            catch (Exception)
            {

            }
        }

        ///// <summary>
        ///// Row重绘前处理
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnRowPrePaint(DataGridViewRowPrePaintEventArgs e)
        //{
        //    base.OnRowPrePaint(e);

        //    //是否是选中状态
        //    if ((e.State & DataGridViewElementStates.Selected) ==
        //                DataGridViewElementStates.Selected)
        //    {
        //        // 计算选中区域Size
        //        int width = this.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + _RowHeadWidth;

        //        Rectangle rowBounds = new Rectangle(
        //          0, e.RowBounds.Top, width,
        //            e.RowBounds.Height);

        //        // 绘制选中背景色
        //        using (LinearGradientBrush backbrush =
        //            new LinearGradientBrush(rowBounds,
        //                Color.GreenYellow,
        //                e.InheritedRowStyle.ForeColor, 90.0f))
        //        {
        //            e.Graphics.FillRectangle(backbrush, rowBounds);
        //            e.PaintCellsContent(rowBounds);
        //            e.Handled = true;
        //        }

        //    }
        //}

        /// <summary>
        /// Column和RowHeader绘制
        /// </summary>
        /// <param name="e"></param>
        void drawColumnAndRow(DataGridViewCellPaintingEventArgs e)
        {
            // 绘制背景色
            using (LinearGradientBrush backbrush =
                new LinearGradientBrush(e.CellBounds,
                    ProfessionalColors.MenuItemPressedGradientBegin,
                    ProfessionalColors.MenuItemPressedGradientMiddle
                    , LinearGradientMode.Vertical))
            {

                Rectangle border = e.CellBounds;
                border.Width -= 1;
                //填充绘制效果
                e.Graphics.FillRectangle(backbrush, border);
                //绘制Column、Row的Text信息
                e.PaintContent(e.CellBounds);
                //绘制边框
                ControlPaint.DrawBorder3D(e.Graphics, e.CellBounds, Border3DStyle.Etched);
            }
        }

        ///// <summary>
        ///// 重绘Column、Row
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        //{
        //    //如果是Column
        //    if (e.RowIndex == -1)
        //    {
        //        drawColumnAndRow(e);
        //        e.Handled = true;
        //        //如果是Rowheader
        //    }
        //    //else if (e.ColumnIndex < 0 && e.RowIndex >= 0)
        //    //{
        //    //    drawColumnAndRow(e);
        //    //    e.Handled = true;
        //    //}
        //}
    }
}