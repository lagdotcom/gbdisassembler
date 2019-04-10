using GBLib.Operand;
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
    public partial class GotoDialog : Form
    {
        public GotoDialog()
        {
            InitializeComponent();
        }

        public BankedAddress Address
        {
            get
            {
                try
                {
                    return BankedAddress.Parse(AddressBox.Text);
                }
                catch (Exception)
                {
                    return new BankedAddress();
                }
            }

            set { AddressBox.Text = value.ToString(); }
        }

        private void GotoDialog_Shown(object sender, EventArgs e)
        {
            AddressBox.Focus();
        }
    }
}
