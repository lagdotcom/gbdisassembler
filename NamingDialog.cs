using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GBDisassembler
{
    public partial class NamingDialog : Form
    {
        public NamingDialog()
        {
            InitializeComponent();
        }

        public string NameString
        {
            get => NameBox.Text;
            set
            {
                NameBox.Text = value;
            }
        }

        private void NamingDialog_Shown(object sender, EventArgs e)
        {
            NameBox.Focus();
        }
    }
}
