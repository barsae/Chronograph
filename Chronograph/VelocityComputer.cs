using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chronograph {
    public class VelocityComputer {
        public void ComputeVelocity(List<Frame> frames, double distanceBetweenLengthMarks) {
            var frameWithLengthMarks = frames.FirstOrDefault(frame => frame.LengthMark1.HasValue && frame.LengthMark2.HasValue);

            if (frameWithLengthMarks == null) {
                MessageBox.Show("Please use the 'Measure' tool on a frame");
                return;
            }

            var distancePerPixel = distanceBetweenLengthMarks / Math.Abs(frameWithLengthMarks.LengthMark1.Value.X - frameWithLengthMarks.LengthMark2.Value.X);
            Console.WriteLine("Distance per pixel: {0}", distancePerPixel);

            var velocities = new List<string>();
            Frame previousFrame = null;
            foreach (var frame in frames) {
                if (frame.ItemMark != null) {
                    if (previousFrame != null) {
                        var deltaTime = frame.TimeStamp - previousFrame.TimeStamp;
                        var deltaPixels = Math.Abs(frame.ItemMark.Value.X - previousFrame.ItemMark.Value.X);
                        var distance = distancePerPixel * deltaPixels;
                        var velocity = distance / deltaTime.TotalSeconds;
                        velocities.Add(string.Format("Velocity between {0} and {1} was {2:0.00} units per second", previousFrame, frame, velocity));
                    }

                    previousFrame = frame;
                }
            }

            if (velocities.Count > 0) {
                MessageBox.Show(string.Join("\n", velocities));
            } else {
                MessageBox.Show("Please use the 'Mark' tool on at least 2 frames");
            }
        }
    }
}
