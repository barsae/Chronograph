using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;

namespace Chronograph {
    public class VideoManager {
        private List<Frame> frames;
        private VideoCaptureDevice video;
        private bool running = true;

        public List<Frame> CaptureFrames() {
            frames = new List<Frame>();
            var filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            var filter = filters[0];

            Console.WriteLine("Opening {0}", filter.MonikerString);
            video = new VideoCaptureDevice(filter.MonikerString);

            video.VideoSourceError += video_VideoSourceError;
            video.NewFrame += video_NewFrame;
            video.Start();

            while (running) {
            }

            video.Stop();

            return frames;
        }

        public void video_NewFrame(object sender, NewFrameEventArgs eventArgs) {
            frames.Add(new Frame() {
                Bitmap = (Bitmap)eventArgs.Frame.Clone(),
                TimeStamp = DateTime.Now,
                FrameNumber = frames.Count
            });

            if (frames.Count == 30) {
                running = false;
            }
        }

        public void video_VideoSourceError(object sender, VideoSourceErrorEventArgs eventArgs) {
            Console.WriteLine(eventArgs.Description);
        }
    }
}
