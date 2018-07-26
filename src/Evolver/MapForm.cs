using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolver
{
    public partial class MapForm : Form
    {
        internal EvolutionRunner Runner { get; private set; }

        private bool running = false;
        private Bitmap backgroundImage= null;

        public MapForm()
        {
            InitializeComponent();
            SetDoubleBuffered(panelMap);
        }

        private static void SetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        internal void SetRunner(EvolutionRunner runner)
        {
            Runner = runner;
            DrawBackground();
        }

        private void DrawBackground()
        {
            var clientRect = panelMap.ClientRectangle;
            if (clientRect.Width == 0 || clientRect.Height == 0)
            {
                backgroundImage = null;
                panelMap.BackgroundImage = null;
                return;
            }

            backgroundImage = new Bitmap(clientRect.Width, clientRect.Height);
            Graphics graphics = Graphics.FromImage(backgroundImage);
            var panesPerRow = Runner.Map.Width;
            var panesPerColumn = Runner.Map.Height;
            var paneSize = new Size(clientRect.Size.Width / panesPerRow, clientRect.Size.Height / panesPerColumn);
            var mapSize = new Size(paneSize.Width * panesPerRow, paneSize.Height * panesPerColumn);

            graphics.FillRectangle(Brushes.LightGray, clientRect);
            for (int i = 0; i <= panesPerRow; ++i)
            {
                var x = i * paneSize.Width;
                graphics.DrawLine(Pens.Blue, new Point(x, 0), new Point(x, mapSize.Height));
            }

            for (int i = 0; i <= panesPerColumn; ++i)
            {
                var y = i * paneSize.Height;
                graphics.DrawLine(Pens.Blue, new Point(0, y), new Point(mapSize.Width, y));
            }

            if (Runner is null)
            {
                return;
            }

            foreach (var collision in Runner.Collisions)
            {
                var mapPosition = new Point(collision.X * paneSize.Width, collision.Y * paneSize.Height);
                var paneRect = new Rectangle(mapPosition, paneSize);
                paneRect.Inflate(-1, -1);
                graphics.FillRectangle(Brushes.LightYellow, paneRect);
            }

            foreach (var car in Runner.Cars)
            {
                var mapPosition = new Point(car.Position.X * paneSize.Width, car.Position.Y * paneSize.Height);
                var paneRect = new Rectangle(mapPosition, paneSize);
                paneRect.Inflate(-paneSize.Width / 10, -paneSize.Height / 10);
                graphics.FillRectangle(Brushes.AliceBlue, paneRect);
                var dotPosition = mapPosition;
                switch (car.Direction)
                {
                    case PaneState.MovingUpward:
                        dotPosition.X += paneSize.Width * 4 / 10;
                        dotPosition.Y += paneSize.Height / 10;
                        break;
                    case PaneState.MovingDownward:
                        dotPosition.X += paneSize.Width * 4 / 10;
                        dotPosition.Y += paneSize.Height * 7 / 10;
                        break;
                    case PaneState.MovingLeftward:
                        dotPosition.X += paneSize.Width / 10;
                        dotPosition.Y += paneSize.Height * 4 / 10;
                        break;
                    case PaneState.MovingRightward:
                        dotPosition.X += paneSize.Width * 7 / 10;
                        dotPosition.Y += paneSize.Height * 4 / 10;
                        break;
                }
                graphics.FillEllipse(Brushes.DarkBlue,
                    new Rectangle(dotPosition, new Size(paneSize.Width / 5, paneSize.Height / 5)));
                var goalPosition = new Point(
                    car.Goal.X * paneSize.Width + paneSize.Width / 2,
                    car.Goal.Y * paneSize.Height + paneSize.Height / 2);
                dotPosition.X += paneSize.Width / 10;
                dotPosition.Y += paneSize.Height / 10;
                graphics.DrawLine(Pens.Green, dotPosition, goalPosition);
            }

            panelMap.BackgroundImage = backgroundImage;
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (running)
            {
                buttonStart.Text = "Start";
                running = false;
                Runner.Stop();
                Runner.UpdateEvent -= Runner_UpdateEvent;
            }
            else
            {
                buttonStart.Text = "Stop";
                running = true;
                Runner.UpdateEvent += Runner_UpdateEvent;
                Runner.Run();
            }
        }

        private void Runner_UpdateEvent(object sender, EventArgs e)
        {
            (Owner as WorldForm).UpdateStatistics();
            if (trackBarFreq.Value < 10)
            {
                Thread.Sleep(1000 / trackBarFreq.Value - 50);
            }
            DrawBackground();
        }

        private void MapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (running)
            {
                running = false;
                Runner.Stop();
            }
        }

        private void panelMap_Resize(object sender, EventArgs e)
        {
            DrawBackground();
        }
    }
}
