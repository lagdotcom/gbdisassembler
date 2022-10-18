using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Lag.DisassemblerLib;

namespace Lag.Disassembler
{

    public partial class CodeDisplay : UserControl
    {
        public const int BigJump = 0x40;

        private const int PerRawByte = 20;

        private int RawBytes => 90;
        private int Main => project == null ? RawBytes : RawBytes + project.MaxOpSize * PerRawByte + 5;
        private int Operands => Main + 50;
        private int Comment => Operands + 200;

        private uint offset = 0;
        private uint currentLine = 0;
        private Dictionary<Rectangle, uint> lineHotspots;
        private Dictionary<Rectangle, Word> wordHovers;
        private List<uint> lineNumbers;
        private IProject project;

        private Font hoverFont;
        private Brush rawBytesBrush = Brushes.LightCoral;
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
        private Brush romOpBrush = Brushes.Red;
        private Brush identifiedOpBrush = Brushes.DarkOrange;
        private Brush commentBrush = Brushes.Green;

        public CodeDisplay()
        {
            DoubleBuffered = true;
            InitializeComponent();

            lineHotspots = new Dictionary<Rectangle, uint>();
            wordHovers = new Dictionary<Rectangle, Word>();
            lineNumbers = new List<uint>();
            hoverFont = new Font(Font, FontStyle.Underline);

            Scrolly.LargeChange = BigJump;

            Disposed += CodeDisplay_Disposed;
            MouseWheel += CodeDisplay_MouseWheel;
        }

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

        public Word CurrentOp { get; private set; } = null;

        public uint ContextLine { get; private set; }
        public Word ContextOp { get; private set; } = null;
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

        public IProject Project
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
            wordHovers.Clear();
            lineNumbers.Clear();

            e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);
            int x = Padding.Left;
            int y = e.ClipRectangle.Top + Padding.Top;
            uint o = offset;

            while (y < e.ClipRectangle.Bottom && o < project.ROM.Length)
            {
                Word addr = Project.FromAbsolute(o);

                int sy = y;
                lineNumbers.Add(o);

                if (project.Labeller.Handles(addr))
                {
                    y += Font.Height;

                    string label = project.Labeller.Identify(addr);
                    e.Graphics.DrawString($"{label}:", Font, labelBrush, x + Main, y);

                    y += Font.Height;
                }

                if (o == currentLine)
                    e.Graphics.FillRectangle(Brushes.Yellow, x, y, e.ClipRectangle.Width, Font.Height);

                Font f = Font;
                Brush br = offsetBrush;
                if (CurrentOp?.Absolute == o)
                {
                    f = hoverFont;
                    br = hoverOffsetBrush;
                }

                e.Graphics.DrawString($"{addr}", f, br, x, y);

                uint move;
                bool end = false;
                int yadd = Font.Height;
                if (project.Instructions.ContainsKey(o))
                {
                    IInstruction inst = project.Instructions[o];
                    move = inst.TotalSize;

                    PaintBytes(e.Graphics, project.ROM.Slice(o, o + move), y);
                    PaintInstruction(e.Graphics, inst.OpType, inst.IsReal, y);
                    if (inst.Operands != null) PaintOperands(e.Graphics, inst.Operands, y);

                    end = inst.IsEnd;
                }
                else
                {
                    IOperand[] fakeops = new IOperand[] { new DisassemblerLib.Byte(project.ROM[o]) { IsHex = true } };
                    PaintBytes(e.Graphics, project.ROM.Slice(o, o+1), y);
                    PaintInstruction(e.Graphics, "db", false, y);
                    PaintOperands(e.Graphics, fakeops, y);

                    move = 1;
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

        private void PaintBytes(Graphics g, byte[] bytes, int y)
        {
            int x = RawBytes;
            foreach (byte b in bytes)
            {
                g.DrawString(b.ToString("X2"), Font, rawBytesBrush, x, y);
                x += PerRawByte;
            }
        }

        private void PaintInstruction(Graphics g, string op, bool real, int y)
        {
            g.DrawString(op, Font, real ? realOpBrush : fakeOpBrush, Padding.Left + Main, y);
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
                Word word = op as Word;

                if (word != null)
                {
                    IPortHandler handler = project.FindHandler(word);
                    if (handler != null)
                    {
                        s = handler.Identify(word);
                        handled = true;

                        if (word.Indirect)
                            s = $"({s})";
                    }
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
                else if (word != null && word.ROM)
                {
                    br = romOpBrush;
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
                if (word != null)
                    wordHovers[r] = word;

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

        private void ReplaceOp(IOperand old, Word rep)
        {
            IInstruction inst = project.Instructions[ContextOpLine];
            for (var i = 0; i < inst.Operands.Length; i++)
            {
                IOperand op = inst.Operands[i];
                if (op == old)
                {
                    Replace?.Invoke(this, new ReplaceEventArgs(ContextOpLine, rep));
                    Invalidate();
                    return;
                }
            }

            MessageBox.Show($"Could not find operand ${old} to replace!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ReplaceData(uint location, Word rep)
        {
            Replace?.Invoke(this, new ReplaceEventArgs(ContextLine, rep));
            Invalidate();
        }

        private void ShowOperandTypeContext(Point location)
        {
            ContextOp = CurrentOp;
            ContextOpLine = lineHotspots.First(pair => pair.Key.Contains(location)).Value;

            ForceOperandBank.Enabled = ContextOp.ROM;

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
            if (e.Button == MouseButtons.Left && CurrentOp != null && CurrentOp.ROM)
            {
                Goto?.Invoke(this, new GotoEventArgs(CurrentOp.Absolute));
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
            var rect = wordHovers.FirstOrDefault(pair => pair.Key.Contains(e.X, e.Y));

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
            ReplaceOp(ContextOp, new Word(ContextOp.Offset) { RAM = false, IsHex = false });
        }

        private void SetOperandHex_Click(object sender, EventArgs e)
        {
            ReplaceOp(ContextOp, new Word(ContextOp.Offset) { RAM = false, IsHex = true });
        }

        private void SetOperandRAM_Click(object sender, EventArgs e)
        {
            ReplaceOp(ContextOp, new Word(ContextOp.Offset));
        }

        private void SetOperandROM_Click(object sender, EventArgs e)
        {
            ReplaceOp(ContextOp, Project.GuessFromContext(ContextOpLine, ContextOp.Offset));
        }

        private void ForceOperandBank_Click(object sender, EventArgs e)
        {
            using (BankDialog dialog = new BankDialog
            {
                Segments = project.Segments,
                Seg = ContextOp.Seg,
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    ReplaceOp(ContextOp, new Word(dialog.Seg, ContextOp.Offset));
            }
        }

        private void SetDataByte_Click(object sender, EventArgs e)
        {
            ReplaceData(ContextLine, null);
        }

        private void SetDataWord_Click(object sender, EventArgs e)
        {
            ReplaceData(ContextLine, new Word(GetContextWord()));
        }

        private void SetDataROM_Click(object sender, EventArgs e)
        {
            ReplaceData(ContextLine, Project.GuessFromContext(ContextLine, GetContextWord()));
        }

        private void SetDataRAM_Click(object sender, EventArgs e)
        {
            ReplaceData(ContextLine, new Word(GetContextWord()));
        }

        private uint GetContextWord()
        {
            return (uint)(project.ROM[ContextLine] + (project.ROM[ContextLine + 1] << 8));
        }
    }
}
