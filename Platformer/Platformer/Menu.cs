using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Platformer
{
    class Menu
    {
        protected Window owner;

        protected List<Control> Controls { get; }

        public Menu(Window owner)
        {
            this.owner = owner;
            Controls = new List<Control>();
            owner.SizeChanged += OnSizeChanged;
        }

        public void ReceiveControl()
        {
            owner.KeyDown += OnKeyDown;
            owner.KeyUp += OnKeyUp;
            owner.MouseMove += OnMouseMove;
            owner.MouseClick += OnMouseClick;
            foreach (var ctrl in Controls)
            {
                owner.screen.Controls.Add(ctrl);
                ctrl.BringToFront();
            }
        }

        public void ReturnControl()
        {
            owner.KeyDown -= OnKeyDown;
            owner.KeyUp -= OnKeyUp;
            owner.MouseMove -= OnMouseMove;
            owner.MouseClick -= OnMouseClick;
            foreach (var ctrl in Controls)
                owner.screen.Controls.Remove(ctrl);
        }

        protected virtual void OnKeyDown(object o, KeyEventArgs e) { }

        protected virtual void OnKeyUp(object o, KeyEventArgs e) { }

        protected virtual void OnMouseMove(object o, MouseEventArgs e) { }

        protected virtual void OnMouseClick(object o, MouseEventArgs e) { }

        protected virtual void OnSizeChanged(object o, EventArgs e) { }
    }
}
