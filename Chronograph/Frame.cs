using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronograph {
    public class Frame {
        public Bitmap Bitmap { get; set; }
        public DateTime TimeStamp { get; set; }
        public int FrameNumber { get; set; }

        public Point? LengthMark1 { get; set; }
        public Point? LengthMark2 { get; set; }
        public Point? ItemMark { get; set; }

        public override string ToString() {
            return string.Format("Frame {0:D3}", FrameNumber);
        }
    }
}
