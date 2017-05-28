using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

/****************************************
 * 日志附着器，显示到文本框中
 * 
 * 
 * 
 * **************************************/

namespace WR.Utils
{
    public class RichEditLogOutPut
    {
        public LogRichEditAppender a = null;
        public RichEditLogOutPut(string libname, RichTextBox richTextBox, int maxLines, int minLines, int kepLines)
        {
            this.a = new LogRichEditAppender(richTextBox, maxLines, minLines, kepLines, @"%d{HH:mm:ss.fff} %-10p %m %n");

            Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            if (hierarchy != null)
            {
                Logger loog = hierarchy.GetLogger(libname) as log4net.Repository.Hierarchy.Logger;
                if (loog != null)
                {
                    loog.AddAppender(this.a);

                }
            }
            a.Threshold = log4net.Core.Level.Info;
        }
    }

    public class LogRichEditAppender : AppenderSkeleton
    {
        private static readonly log4net.ILog log =
      log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int m_maxLines = 250;
        private int m_minLines = 50;
        private int m_kepLines = 2;
        private int m_nowLines = 0;
        private int m_removeLines = 0;
        private int m_removeCharacters = 0;
        private int[] m_LinePos;
        private RichTextBox m_richTextBox;
        private ContextMenu contextMenu;
        private MenuItem levelMenuItem;

        public LogRichEditAppender(RichTextBox richTextBox, int maxLines, int minLines, int kepLines, string PatternLayoutStr)
        {
            m_maxLines = maxLines < 0 ? 0 : maxLines;
            m_minLines = minLines < 0 ? 0 : minLines;
            m_kepLines = kepLines < 0 ? 0 : kepLines;
            if (m_minLines < m_kepLines)
                m_minLines = m_kepLines;
            m_LinePos = new int[m_maxLines];

            Threshold = log4net.Core.Level.All;
            //Layout = new SimpleLayout();
            if (PatternLayoutStr != null)
            {
                Layout = new PatternLayout(PatternLayoutStr);
            }
            else
            {
                Layout = new PatternLayout(@"%d{yyyyMMdd HH:mm:ss.fff} %-10p %m %n");
            }
            m_richTextBox = richTextBox;
            m_richTextBox.Multiline = true;
            m_richTextBox.ReadOnly = true;
            //TODO 右键菜单，日志级别
            if (m_richTextBox.ContextMenu == null)
                contextMenu = new ContextMenu();
            else
                contextMenu = m_richTextBox.ContextMenu;


            contextMenu.MenuItems.Add(new MenuItem("Clear", new EventHandler(delegate(object e, EventArgs a) { m_richTextBox.Clear(); m_nowLines = 0; })));

            contextMenu.MenuItems.Add(new MenuItem("-"));

            foreach (FieldInfo field in (typeof(Level)).GetFields())
            {
                if (field.DeclaringType == typeof(Level))
                {
                    MenuItem item = new MenuItem(field.Name, new EventHandler(LevelSelected));
                    item.RadioCheck = true;
                    item.Checked = false;
                    contextMenu.MenuItems.Add(item);

                    if (String.Compare(field.Name, Threshold.Name, true) == 0)
                    {
                        levelMenuItem = item;
                        item.Checked = true;
                    }

                }

            }

            if (m_richTextBox.ContextMenu == null)
                m_richTextBox.ContextMenu = contextMenu;


            return;

        }

        ~LogRichEditAppender()
        {
            m_richTextBox.ContextMenu = null;
        }

        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }

        public new log4net.Core.Level Threshold //override
        {
            get { return base.Threshold; }
            set
            {
                if (base.Threshold != null && base.Threshold.Equals(value))
                    return;
                base.Threshold = (log4net.Core.Level)value;
                if (contextMenu == null)
                    return;
                foreach (MenuItem item in contextMenu.MenuItems)
                {
                    if (String.Compare(item.Name, Threshold.Name, true) == 0)
                    {
                        levelMenuItem.Checked = false;
                        levelMenuItem = item;
                        levelMenuItem.Checked = true;
                        break;
                    }
                }
            }
        }

        public void LevelSelected(object e, EventArgs a)
        {
            MenuItem item = e as MenuItem;
            if (item == null)
                return;

            foreach (FieldInfo field in (typeof(log4net.Core.Level)).GetFields())
            {
                if (field.DeclaringType == typeof(log4net.Core.Level) && String.Compare(field.Name, item.Text, true) == 0)
                {
                    levelMenuItem.Checked = false;
                    levelMenuItem = item;
                    levelMenuItem.Checked = true;
                    Threshold = (log4net.Core.Level)field.GetValue(Threshold);
                }
            }

            return;
        }

        public delegate void AppendDelegate(LoggingEvent loggingEvent);
        protected override void Append(LoggingEvent loggingEvent)
        {
            object[] parms = { loggingEvent };
            try
            {

                m_richTextBox.BeginInvoke(new AppendDelegate(AppendEvent), parms);
            }
            catch (Exception e)
            {
                //  log.Error(e.Message +e.StackTrace);
            }
        }

        protected void AppendEvent(LoggingEvent loggingEvent)
        {
            m_richTextBox.SelectionStart = m_richTextBox.Text.Length + 1;
            if (m_nowLines >= m_maxLines)
            {
                int removeLines = m_nowLines - m_minLines;
                int removeStart = m_kepLines == 0 ? 0 : m_LinePos[m_kepLines - 1] + 1;
                int removeLength = m_LinePos[m_kepLines + removeLines - 1] - removeStart + 1;
                m_removeLines += removeLines;
                m_removeCharacters += removeLength;
                m_richTextBox.Select(removeStart, removeLength);
                //m_richTextBox.Cut();
                string replace = String.Format("<此处删去{0}个字>\n", m_removeCharacters);
                //string replace = String.Format("<此处删去{0}行,{1}字>\n", m_removeLines, m_removeCharacters);
                m_richTextBox.SelectedText = replace;
                removeLength -= replace.Length;
                m_nowLines -= removeLines;
                for (int i = m_kepLines; i < m_minLines - 1; i++)
                {
                    m_LinePos[i] = m_LinePos[removeLines + i] - removeLength;
                }
            }
            if (loggingEvent.Level < log4net.Core.Level.Warn)
                m_richTextBox.SelectionColor = Color.Black;
            else if (loggingEvent.Level < log4net.Core.Level.Error)
                m_richTextBox.SelectionColor = Color.Orange;
            else if (loggingEvent.Level < log4net.Core.Level.Fatal)
                m_richTextBox.SelectionColor = Color.Red;
            else
                m_richTextBox.SelectionColor = Color.Purple;
            m_richTextBox.AppendText(base.RenderLoggingEvent(loggingEvent));
            m_richTextBox.ScrollToCaret();
            m_LinePos[m_nowLines++] = m_richTextBox.TextLength - 1;
        }
    }
}
