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
    public partial class BankDialog : Form
    {
        public BankDialog()
        {
            InitializeComponent();
        }

        public uint Bank
        {
            get => (uint)BankBox.Value;
            set => BankBox.Value = value;
        }

        public int Max
        {
            get => (int)BankBox.Maximum;
            set => BankBox.Maximum = value;
        }

        private void BankDialog_Shown(object sender, EventArgs e)
        {
            BankBox.Focus();
            BankBox.Select(0, BankBox.Text.Length);
        }
    }
}
