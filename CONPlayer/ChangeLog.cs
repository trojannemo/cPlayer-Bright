using System;
using System.Reflection;
using System.Windows.Forms;

namespace cPlayer
{
    public partial class ChangeLog : Form
    {

        private readonly string filePath;

        public ChangeLog()
        {
            InitializeComponent();
            filePath = Application.StartupPath + "\\bin\\help\\";
            lblCopyright.Text = "© 2014 - " + DateTime.Now.Year;
            rtbChangeLog.LoadFile(filePath + "changelog.txt", RichTextBoxStreamType.PlainText);
        }
    }
}
