using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GBDisassembler
{
    using GBLib;
    using GBLib.Operand;

    public partial class CodeDisplay : UserControl
    {
        public const int BigJump = 0x40;

        const int Main = 100;
        const int Operands = Main + 40;
        const int Comment = Operands + 200;
        private uint offset = 0;
        private uint currentLine = 0;
        private Dictionary<Rectangle, uint> lineHotspots;
        private Dictionary<Rectangle, IOperand> opHovers;
        private List<uint> lineNumbers;
        private Disassembler project;

        private Font hoverFont;
        private Brush realOpBrush = Brushes.Blue;
        private Brush offsetBrush = Brushes.Gray;
        private Brush labelBrush = Brushes.DarkGreen;
        private Brush hoverOffsetBrush = Brushes.DarkBlue;
        private Brush hexOpBrush = Brushes.DarkRed;
        private Brush fakeOpBrush = Brushes.LightGray;
        private Brush defaultOpBrush = Brushes.Black;
        private Brush hoverOpBrush = Brushes.Blue;
        private Brush numericOpBrush = Brushes.Purple;
        private Brush registerOpBrush = Brushes.Blue;
        private Brush addressOpBrush = Brushes.Red;
        private Brush identifiedOpBrush = Brushes.DarkOrange;
        private Brush commentBrush = Brushes.Green;

        public CodeDisplay()
        {
            DoubleBuffered = true;
            InitializeComponent();

            lineHotspots = new Dictionary<Rectangle, uint>();
            opHovers = new Dictionary<Rectangle, IOperand>();
            lineNumbers = new List<uint>();
            hoverFont = new Font(Font, FontStyle.Underline);

            Scrolly.LargeChange = BigJump;

            Disposed += CodeDisplay_Disposed;
            MouseWheel += CodeDisplay_MouseWheel;
        }

        public event EventHandler<DataEventArgs> Data;

        public event EventHandler<GotoEventArgs> Goto;

        public event EventHandler<ReplaceEventArgs> Replace;

        public uint Minimum => 0;
        public uint Maximum => project != null ? (uint)project.ROM.Length : 0;

        public uint CurrentLine
        {
            get => currentLine;
            set
            {
                currentLine = value.Clamp(Minimum, Maximum);
                Invalidate();
            }
        }

        public IOperand CurrentOp { get; private set; } = null;

        public uint ContextLine { get; private set; }
        public IOperand ContextOp { get; private set; } = null;
        public uint ContextOpLine { get; private set; }

        public uint Offset
        {
            get => offset;
            set
            {
                offset = value.Clamp(Minimum, Maximum);
                Scrolly.Value = (int)offset;
                Invalidate();
            }
        }

        public Disassembler Project
        {
            get => project;
            set
            {
                project = value;
                CurrentLine = 0;
                Scrolly.Value = 0;

                if (project == null)
                    Scrolly.Visible = false;
                else
                {
                    Scrolly.Maximum = (int)Maximum;
                    Scrolly.Visible = true;
                }

                Invalidate();
            }
        }

        public void MoveUp()
        {
            if (!lineNumbers.Contains(CurrentLine))
            {
                Offset = CurrentLine - 0x10;
                return;
            }

            int index = lineNumbers.IndexOf(CurrentLine);
            if (index == 0)
            {
                Offset = CurrentLine - 0x10;
                return;
            }

            CurrentLine = lineNumbers[index - 1];
        }

        public void MoveDown()
        {
            if (!lineNumbers.Contains(CurrentLine))
            {
                Offset = CurrentLine - 0x10;
                return;
            }

            int index = lineNumbers.IndexOf(CurrentLine);
            if (index == lineNumbers.Count - 1)
            {
                Offset = CurrentLine - 0x10;
                return;
            }

            CurrentLine = lineNumbers[index + 1];
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (project == null)
            {
                e.Graphics.FillRectangle(Brushes.Gray, e.ClipRectangle);
                return;
            }

            lineHotspots.Clear();
            opHovers.Clear();
            lineNumbers.Clear();

            e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);
            int x = Padding.Left;
            int y = e.ClipRectangle.Top + Padding.Top;
            uint o = offset;

            while (y < e.ClipRectangle.Bottom && o < project.ROM.Length)
            {
                int sy = y;
                lineNumbers.Add(o);

                if (project.Labeller.Labels.ContainsKey(o))
                {
                    y += Font.Height;

                    string label = project.Labeller.Identify(o);
                    e.Graphics.DrawString($"{label}:", Font, labelBrush, x + Main, y);

                    y += Font.Height;
                }

                if (o == currentLine)
                    e.Graphics.FillRectangle(Brushes.Yellow, x, y, e.ClipRectangle.Width, Font.Height);

                Font f = Font;
                Brush br = offsetBrush;
                if (CurrentOp != null && CurrentOp.AbsoluteAddress == o)
                {
                    f = hoverFont;
                    br = hoverOffsetBrush;
                }

                e.Graphics.DrawString($"{o / 0x4000:X2}:{o % 0x4000:X4}", f, br, x, y);

                uint move;
                bool end = false;
                int yadd = Font.Height;
                if (project.Instructions.ContainsKey(o))
                {
                    Instruction inst = project.Instructions[o];
                    move = inst.TotalSize;

                    PaintInstruction(e.Graphics, inst.OpType, true, y);
                    if (inst.Operands != null) PaintOperands(e.Graphics, inst.Operands, y);

                    end = inst.IsEnd;
                }
                else
                {
                    DataType type = project.DataTypes.ContainsKey(o) ? project.DataTypes[o] : DataType.ByteSize;
                    move = PaintData(e.Graphics, o, type, y);
                }

                if (project.Comments.ContainsKey(o))
                {
                    yadd = PaintComment(e.Graphics, project.Comments[o], y);
                }

                y += yadd;
                lineHotspots.Add(new Rectangle(e.ClipRectangle.Left, sy, e.ClipRectangle.Width, y - sy), o);

                if (end) y += Font.Height;

                o += move;
            }
        }

        private void PaintInstruction(Graphics g, string op, bool real, int y)
        {
            g.DrawString(op, Font, real ? realOpBrush : fakeOpBrush, Padding.Left + Main, y);
        }

        private uint PaintData(Graphics g, uint offset, DataType type, int y)
        {
            string inst, data;
            uint move;

            if (type.HasFlag(DataType.WordSize))
            {
                move = 2;
                inst = "dw";
                data = $"${project.ROM[offset+1]:X2}{project.ROM[offset]:X2}";
            }
            else
            {
                move = 1;
                inst = "db";
                data = $"${project.ROM[offset]:X2}";
            }

            PaintInstruction(g, inst, false, y);
            g.DrawString(data, Font, hexOpBrush, Padding.Left + Operands, y);

            return move;
        }

        private int PaintComment(Graphics g, string comment, int y)
        {
            // TODO: word wrap, ; prefix
            g.DrawString(comment, Font, commentBrush, Padding.Left + Comment, y);

            // TODO
            return Font.Height;
        }

        private void PaintOperands(Graphics g, IOperand[] ops, int y)
        {
            int x = Padding.Left + Operands;
            int o = 0;

            foreach (IOperand op in ops)
            {
                Brush br = defaultOpBrush;
                Font f = Font;

                string s = op.ToString();
                bool handled = false;
                IPortHandler handler = project.FindHandler(op);
                if (handler != null && op.AbsoluteAddress.HasValue)
                {
                    s = handler.Identify(op.AbsoluteAddress.Value);
                    handled = true;
                }

                if (op == CurrentOp)
                {
                    br = hoverOpBrush;
                    f = hoverFont;
                }
                else if (handled)
                {
                    br = identifiedOpBrush;
                }
                else if (op.IsRegister)
                {
                    br = registerOpBrush;
                }
                else if (op.AbsoluteAddress.HasValue)
                {
                    br = addressOpBrush;
                }
                else if (op.IsHex)
                {
                    br = hexOpBrush;
                }
                else if (op.IsNumeric)
                {
                    br = numericOpBrush;
                }

                Size size = g.MeasureString(s, f).ToSize();

                Rectangle r = new Rectangle(x, y, size.Width, size.Height);

                if (op.IsNumeric)
                    opHovers[r] = op;

                g.DrawString(s, f, br, x, y);
                x += size.Width;

                o++;
                if (o < ops.Length)
                {
                    s = ", ";
                    size = g.MeasureString(s, Font).ToSize();

                    g.DrawString(s, Font, defaultOpBrush, x, y);
                    x += size.Width;
                }
            }
        }

        private bool IsData(uint location)
        {
            return !project.Instructions.ContainsKey(location);
        }

        private void ReplaceOp(IOperand old, IOperand rep)
        {
            Instruction inst = project.Instructions[ContextOpLine];
            for (var i = 0; i < inst.Operands.Length; i++)
            {
                IOperand op = inst.Operands[i];
                if (op == old)
                {
                    Replace?.Invoke(this, new ReplaceEventArgs(ContextOpLine, i, rep));
                    Invalidate();
                    return;
                }
            }

            MessageBox.Show($"Could not find operand ${old} to replace!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ReplaceData(uint location, DataType type)
        {
            Data?.Invoke(this, new DataEventArgs(location, type));
            Invalidate();
        }

        private void ShowOperandTypeContext(Point location)
        {
            ContextOp = CurrentOp;
            ContextOpLine = lineHotspots.First(pair => pair.Key.Contains(location)).Value;

            ForceOperandBank.Enabled = ContextOp.IsAddress;

            OperandTypeMenu.Show(this, location);
        }

        private void ShowDataTypeContext(Point location)
        {
            ContextLine = CurrentLine;
            DataTypeMenu.Show(this, location);
        }

        private void CodeDisplay_Disposed(object sender, EventArgs e)
        {
            hoverFont.Dispose();
        }

        private void CodeDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            int change = e.Delta > 0 ? Scrolly.LargeChange : -Scrolly.LargeChange;
            Scrolly.Value = (Scrolly.Value - change).Clamp(0, Scrolly.Maximum);
        }

        private void Scrolly_ValueChanged(object sender, EventArgs e)
        {
            Offset = (uint)Scrolly.Value;
        }
        
        private void CodeDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && CurrentOp != null && CurrentOp.AbsoluteAddress.HasValue)
            {
                Goto?.Invoke(this, new GotoEventArgs(CurrentOp.AbsoluteAddress.Value));
                return;
            }

            if (e.Button == MouseButtons.Right && CurrentOp != null)
            {
                ShowOperandTypeContext(e.Location);
                return;
            }

            var rect = lineHotspots.FirstOrDefault(pair => pair.Key.Contains(e.X, e.Y));

            if (rect.Key.Width > 0) CurrentLine = rect.Value;
            if (e.Button == MouseButtons.Right && IsData(CurrentLine))
            {
                ShowDataTypeContext(e.Location);
            }
        }

        private void CodeDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            var rect = opHovers.FirstOrDefault(pair => pair.Key.Contains(e.X, e.Y));

            if (rect.Key.Width > 0)
            {
                CurrentOp = rect.Value;
                Cursor = Cursors.Hand;
            }
            else
            {
                CurrentOp = null;
                Cursor = Cursors.Default;
            }

            Invalidate();
        }

        private void SetOperandDecimal_Click(object sender, EventArgs e)
        {
            ReplaceOp(ContextOp, new Plain(ContextOp.Value));
        }

        private void SetOperandHex_Click(object sender, EventArgs e)
        {
            ReplaceOp(ContextOp, new WordValue(ContextOp.Value));
        }

        private void SetOperandRAM_Click(object sender, EventArgs e)
        {
            ReplaceOp(ContextOp, new Address(ContextOp.Value));
        }

        private void SetOperandROM_Click(object sender, EventArgs e)
        {
            ReplaceOp(ContextOp, BankedAddress.GuessFromContext(ContextOpLine, ContextOp.Value));
        }

        private void ForceOperandBank_Click(object sender, EventArgs e)
        {
            uint? initial = ContextOp.AbsoluteAddress;
            BankDialog dialog = new BankDialog
            {
                Bank = (uint)(initial.HasValue ? initial / BankedAddress.BankSize : 0),
                Max = (int)(project.Header.ROM / BankedAddress.BankSize)
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ReplaceOp(ContextOp, new BankedAddress(dialog.Bank, ContextOp.Value));
            }
        }

        private void SetDataByte_Click(object sender, EventArgs e)
        {
            ReplaceData(ContextLine, DataType.ByteSize);
        }

        private void SetDataWord_Click(object sender, EventArgs e)
        {
            ReplaceData(ContextLine, DataType.WordSize);
        }

        private void SetDataROM_Click(object sender, EventArgs e)
        {
            ReplaceData(ContextLine, DataType.WordSize | DataType.Address | DataType.ROM);
        }

        private void SetDataRAM_Click(object sender, EventArgs e)
        {
            ReplaceData(ContextLine, DataType.WordSize | DataType.Address | DataType.RAM);
        }
    }
}
