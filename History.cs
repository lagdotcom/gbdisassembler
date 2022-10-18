using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lag.Disassembler
{
    class History
    {
        private readonly List<uint> locations;

        public History(int max, Button back, Button fwd)
        {
            Maximum = max;
            BackButton = back;
            ForwardButton = fwd;

            BackButton.Click += BackButton_Click;
            ForwardButton.Click += ForwardButton_Click;

            locations = new List<uint>();
            Clear();
        }

        public event EventHandler<GotoEventArgs> Goto;

        public Button BackButton { get; private set; }
        public Button ForwardButton { get; private set; }

        public int Count => locations.Count;
        public uint Current => locations[Position];
        public int Maximum { get; private set; }
        public int Position { get; private set; }

        public void Clear()
        {
            Position = 0;
            locations.Clear();
            locations.Add(0);

            UpdateButtons();
        }

        public void Remember(uint loc)
        {
            if (Current != loc)
            {
                locations.Add(loc);
                Position++;

                UpdateButtons();
            }
        }

        public void Move(uint loc)
        {
            locations.RemoveRange(Position + 1, Count - Position - 1);

            locations.Add(loc);
            if (Count > Maximum) locations.RemoveAt(0);

            Position = locations.Count - 1;
            UpdateButtons();
            FireEvent();
        }

        private void UpdateButtons()
        {
            BackButton.Enabled = Position > 0;
            ForwardButton.Enabled = Position < (Count - 1);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Position--;
            UpdateButtons();
            FireEvent();
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            Position++;
            UpdateButtons();
            FireEvent();
        }

        private void FireEvent()
        {
            Goto?.Invoke(this, new GotoEventArgs(Current));
        }
    }
}
