using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chronograph {
    public class ChronographForm : Form {
        private List<Frame> frames;
        private Button captureButton;
        private Button measureButton;
        private Button markButton;
        private Button computeButton;
        private ListBox framesList;
        private Label distanceBetweenLengthMarksLabel;
        private TextBox distanceBetweenLengthMarksBox;
        private FrameControl frame;
        private Label helpLabel;

        public ChronographForm() {
            frames = new List<Frame>();
            ClientSize = new Size(1024, 768);

            captureButton = new Button() {
                Text = "Capture",
                Location = new Point(10, 10)
            };
            captureButton.Click += captureButton_Click;

            measureButton = new Button() {
                Text = "Measure",
                Location = new Point(210, 520)
            };
            measureButton.Click += measureButton_Click;

            markButton = new Button() {
                Text = "Mark",
                Location = new Point(290, 520)
            };
            markButton.Click += markButton_Click;

            computeButton = new Button() {
                Text = "Compute",
                Location = new Point(370, 520)
            };
            computeButton.Click += computeButton_Click;

            distanceBetweenLengthMarksLabel = new Label {
                Location = new Point(450, 500),
                Text = "Distance between marks",
                Size = new Size(150, 15)
            };

            distanceBetweenLengthMarksBox = new TextBox() {
                Location = new Point(450, 520),
                Text = "1.00"
            };

            framesList = new ListBox() {
                Location = new Point(10, 40),
                Size = new Size(150, 520)
            };
            framesList.SelectedValueChanged += framesList_SelectedValueChanged;

            frame = new FrameControl() {
                Location = new Point(200, 10),
                Size = new Size(640, 480)
            };

            helpLabel = new Label() {
                Location = new Point(10, 570),
                Size = new Size(800, 200),
                Text = @"
1) Press the capture button, wait for the beep, and then send the item under test in front of the camera
2) Select a frame, press the measure button, and then click on two points of reference
3) Enter the distance (in your preferred unit) between the points of reference
4) Select each frame of interest, click the mark button, and click on the leading edge of the item under test in that frame
5) Press the compute button to determine the velocity in between each marked frame
".Trim()
            };

            this.Controls.Add(captureButton);
            this.Controls.Add(measureButton);
            this.Controls.Add(markButton);
            this.Controls.Add(computeButton);
            this.Controls.Add(distanceBetweenLengthMarksLabel);
            this.Controls.Add(distanceBetweenLengthMarksBox);
            this.Controls.Add(framesList);
            this.Controls.Add(frame);
            this.Controls.Add(helpLabel);
        }

        private void captureButton_Click(object sender, EventArgs e) {
            Thread.Sleep(3000);
            Console.Beep();
            var video = new VideoManager();
            frames = video.CaptureFrames();
            UpdateFramesList();
        }

        private void measureButton_Click(object sender, EventArgs e) {
            frame.State = FrameControlState.AwaitingLengthMark1;
        }

        private void markButton_Click(object sender, EventArgs e) {
            frame.State = FrameControlState.AwaitingItemMark;
        }

        private void computeButton_Click(object sender, EventArgs e) {
            var computer = new VelocityComputer();
            double distanceBetweenLengthMarks;
            if (double.TryParse(distanceBetweenLengthMarksBox.Text, out distanceBetweenLengthMarks)) {
                computer.ComputeVelocity(frames, distanceBetweenLengthMarks);
            } else {
                MessageBox.Show("Please type a number into the distance between marks box");
            }
        }

        private void framesList_SelectedValueChanged(object sender, EventArgs e) {
            frame.Frame = ((Frame)framesList.SelectedItem);
            frame.State = FrameControlState.Idle;
            frame.Invalidate();
        }

        private void UpdateFramesList() {
            framesList.Items.Clear();
            for (int ii = 0; ii < frames.Count; ii++) {
                framesList.Items.Add(frames[ii]);
            }
        }
    }
}
