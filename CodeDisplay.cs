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

    public partial class CodeDisplay : UserControl
    {
        const int Row = 12;
        const int Main = 100;
        const int Operands = Main + 40;
        private uint offset = 0;
        private uint currentLine = 0;
        private IOperand currentOp = null;
        private Font hoverFont;
        private Dictionary<Rectangle, uint> lineHotspots;
        private Dictionary<Rectangle, IOperand> opHovers;
        private Disassembler project;

        public CodeDisplay()
        {
            DoubleBuffered = true;
            InitializeComponent();

            lineHotspots = new Dictionary<Rectangle, uint>();
            opHovers = new Dictionary<Rectangle, IOperand>();
            hoverFont = new Font(Font, FontStyle.Underline);

            MouseWheel += CodeDisplay_MouseWheel;
        }

        public uint CurrentLine
        {
            get => currentLine;
            set
            {
                currentLine = value;
                Invalidate();
            }
        }

        public uint Offset
        {
            get => offset;
            set
            {
                offset = value;
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
                Scrolly.Value = 0;

                if (project == null)
                    Scrolly.Visible = false;
                else
                {
                    Scrolly.Maximum = project.ROM.Length;
                    Scrolly.Visible = true;
                }

                Invalidate();
            }
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

            e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);
            int x = Padding.Left;
            int y = e.ClipRectangle.Top + Padding.Top;
            uint o = offset;

            while (y < e.ClipRectangle.Bottom && o < project.ROM.Length)
            {
                int sy = y;

                if (project.Labeller.Labels.ContainsKey(o))
                {
                    y += Row;

                    string label = project.Labeller.Identify(o);
                    e.Graphics.DrawString($"{label}:", Font, Brushes.DarkGreen, x + Main, y);

                    y += Row;
                }

                if (o == currentLine)
                    e.Graphics.FillRectangle(Brushes.Yellow, x, y, e.ClipRectangle.Width, Font.Height);

                e.Graphics.DrawString($"{o / 0x4000:X2}:{o % 0x4000:X4}", Font, Brushes.Gray, x, y);

                uint move = 1;
                if (project.Instructions.ContainsKey(o))
                {
                    Instruction inst = project.Instructions[o];
                    move = inst.TotalSize;

                    PaintInstruction(e.Graphics, inst.OpType, y);
                    if (inst.Operands != null) PaintOperands(e.Graphics, inst.Operands, y);
                }
                else
                {
                    PaintInstruction(e.Graphics, "db", y);
                    e.Graphics.DrawString($"${project.ROM[o]:X2}", Font, Brushes.DarkRed, x + Operands, y);
                }

                y += Row;
                lineHotspots.Add(new Rectangle(e.ClipRectangle.Left, sy, e.ClipRectangle.Width, y - sy), o);

                o += move;
            }
        }

        private void PaintInstruction(Graphics g, string op, int y)
        {
            g.DrawString(op, Font, Brushes.Black, Padding.Left + Main, y);
        }

        private void PaintOperands(Graphics g, IOperand[] ops, int y)
        {
            int x = Padding.Left + Operands;
            int o = 0;

            foreach (IOperand op in ops)
            {
                Brush br;
                Font f;
                string s = op.ToString();

                if (op == currentOp)
                {
                    br = Brushes.Blue;
                    f = hoverFont;
                }
                else
                {
                    br = Brushes.Black;
                    f = Font;
                }

                Size size = g.MeasureString(s, f).ToSize();

                Rectangle r = new Rectangle(x, y, size.Width, size.Height);

                if (op.AbsoluteAddress.HasValue)
                    opHovers[r] = op;

                g.DrawString(s, f, br, x, y);
                x += size.Width;

                o++;
                if (o < ops.Length)
                {
                    s = ", ";
                    size = g.MeasureString(s, Font).ToSize();

                    g.DrawString(s, Font, Brushes.Black, x, y);
                    x += size.Width;
                }
            }
        }

        private void CodeDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            int change = e.Delta > 0 ? Scrolly.LargeChange : -Scrolly.LargeChange;
            Scrolly.Value = (Scrolly.Value - change).Clamp(Scrolly.Minimum, Scrolly.Maximum);
        }

        private void Scrolly_ValueChanged(object sender, EventArgs e)
        {
            Offset = (uint)Scrolly.Value;
        }
        
        private void CodeDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            var rect = lineHotspots.FirstOrDefault(pair => pair.Key.Contains(e.X, e.Y));

            if (rect.Key.Width > 0) CurrentLine = rect.Value;
        }

        private void CodeDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            var rect = opHovers.FirstOrDefault(pair => pair.Key.Contains(e.X, e.Y));

            if (rect.Key.Width > 0)
            {
                currentOp = rect.Value;
                Cursor = Cursors.Hand;
            }
            else
            {
                currentOp = null;
                Cursor = Cursors.Default;
            }

            Invalidate();
        }
    }
}
