using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lag.DisassemblerLib;

namespace Lag.Disassembler
{
    public partial class RAMDisplay : UserControl
    {
        public const int BigJump = 0x40;
        private IProject project;

        const int Main = 50;
        const int Comment = Main + 100;
        private uint offset = 0;
        private uint currentLine = 0;
        private List<uint> Lines;

        private Brush labelBrush = Brushes.DarkGreen;
        private Brush commentBrush = Brushes.Green;

        public RAMDisplay()
        {
            InitializeComponent();

            Lines = new List<uint>();
            Scrolly.LargeChange = BigJump;

            MouseWheel += RAMDisplay_MouseWheel;
        }

        private void RAMDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            int change = e.Delta > 0 ? Scrolly.LargeChange : -Scrolly.LargeChange;
            Scrolly.Value = (Scrolly.Value - change).Clamp(0, Scrolly.Maximum);
        }

        public uint Minimum => 0;
        public uint Maximum => (uint)Lines.Count;

        public uint CurrentLine
        {
            get => currentLine;
            set
            {
                currentLine = value.Clamp(Minimum, Maximum);
                Invalidate();
            }
        }

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
                    CalculateLines();
                    Scrolly.Maximum = (int)Maximum;
                    Scrolly.Visible = true;
                }

                Invalidate();
            }
        }

        private void CalculateLines()
        {
            Lines.Clear();

            foreach (Segment s in project.RAMSegments)
            {
                uint pos = s.RAMPosition;
                while (pos < s.RAMEnd)
                {
                    Lines.Add(pos);
                    pos++;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (project == null)
            {
                e.Graphics.FillRectangle(Brushes.Gray, e.ClipRectangle);
                return;
            }

            e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);

            int x = Padding.Left;
            int y = e.ClipRectangle.Top + Padding.Top;
            uint o = offset;

            while (y < e.ClipRectangle.Bottom && o < Maximum)
            {
                Word addr = new Word(Lines[(int)o]);

                int sy = y;

                if (project.Labeller.Handles(addr))
                {
                    y += Font.Height;

                    string label = project.Labeller.Identify(addr);
                    e.Graphics.DrawString($"{label}:", Font, labelBrush, x + Main, y);

                    y += Font.Height;
                }

                if (o == currentLine)
                    e.Graphics.FillRectangle(Brushes.Yellow, x, y, e.ClipRectangle.Width, Font.Height);

                e.Graphics.DrawString($"{addr}", Font, Brushes.Black, x, y);

                uint move = 1;
                bool end = false;
                int yadd = Font.Height;
                string name = "?";
                if (project.Namer.Handles(addr)) name = project.Namer.Identify(addr);

                e.Graphics.DrawString(name, Font, Brushes.Black, Padding.Left + Main, y);

                if (project.Comments.ContainsKey(o))
                {
                    e.Graphics.DrawString(project.Comments[o], Font, commentBrush, Padding.Left + Comment, y);
                    y += Font.Height;
                }

                y += yadd;

                if (end) y += Font.Height;

                o += move;
            }
        }

        private void Scrolly_ValueChanged(object sender, EventArgs e)
        {
            Offset = (uint)Scrolly.Value;
        }
    }
}
