using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chronograph {
    public class FrameControl : Control {
        public Frame Frame { get; set; }
        public FrameControlState State { get; set; }

        private Point? hoverPoint; 

        public FrameControl() {
            State = FrameControlState.Idle;
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            hoverPoint = e.Location;
            DoubleBuffered = true;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            if (Frame != null) {
                switch (State) {
                    case FrameControlState.AwaitingLengthMark1:
                        Frame.LengthMark1 = e.Location;
                        State = FrameControlState.AwaitingLengthMark2;
                        break;
                    case FrameControlState.AwaitingLengthMark2:
                        Frame.LengthMark2 = e.Location;
                        State = FrameControlState.Idle;
                        break;
                    case FrameControlState.AwaitingItemMark:
                        Frame.ItemMark = e.Location;
                        State = FrameControlState.Idle;
                        break;
                }
                Invalidate();
            } else {
                State = FrameControlState.Idle;
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (Frame != null) {
                e.Graphics.DrawImage(Frame.Bitmap, new Point(0, 0));

                if (hoverPoint.HasValue) {
                    var point = hoverPoint.Value;
                    e.Graphics.DrawLine(Pens.Blue, point.X, 0, point.X, Height);
                }

                if (Frame.LengthMark1.HasValue) {
                    var point = Frame.LengthMark1.Value;
                    e.Graphics.DrawLine(Pens.White, point.X, 0, point.X, Height);
                }

                if (Frame.LengthMark2.HasValue) {
                    var point = Frame.LengthMark2.Value;
                    e.Graphics.DrawLine(Pens.White, point.X, 0, point.X, Height);
                }

                if (Frame.ItemMark.HasValue) {
                    var point = Frame.ItemMark.Value;
                    e.Graphics.DrawLine(Pens.Red, point.X, 0, point.X, Height);
                }
            }
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, Width - 1, Height - 1));
            base.OnPaint(e);
        }
    }

    public enum FrameControlState {
        Idle,
        AwaitingLengthMark1,
        AwaitingLengthMark2,
        AwaitingItemMark
    }
}
