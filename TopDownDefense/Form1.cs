using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace TopDownDefense
{
    public partial class Form1 : Form
    {
        Graphics g;

        Player player = new Player(300,300,1,0);
        bool playerLeft, playerRight, playerUp, playerDown;

        int MouseX;
        int MouseY;

        public Form1()
        {
            InitializeComponent();
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, Canvas, new object[] { true });
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            //Console.WriteLine(CalculateMouseAngle());
            player.DrawPlayer(g, CalculateMouseAngle());
        }

        private void updateTmr_Tick(object sender, EventArgs e)
        {
            player.MovePlayer(playerLeft, playerRight, playerUp, playerDown);
            Console.WriteLine("Mouse X = " + MouseX + " Mouse Y = " + MouseY + " Angle = " + CalculateMouseAngle());
            Canvas.Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseX = e.X;
            MouseY = e.Y;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.A:
                    playerLeft = true;
                    break;

                case Keys.D:
                    playerRight = true;
                    break;

                case Keys.W:
                    playerUp = true;
                    break;

                case Keys.S:
                    playerDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    playerLeft = false;
                    break;

                case Keys.D:
                    playerRight = false;
                    break;

                case Keys.W:
                    playerUp = false;
                    break;

                case Keys.S:
                    playerDown = false;
                    break;
            }
        }

        public double CalculateMouseAngle()
        {
            double Opposite;
            double Adjacent;

            double AngleRadians;
            double AngleDegrees;

            // Working out the Opposite Size
            int OppositeA = Canvas.Height-MouseY;
            int OppositeB = Canvas.Height - player.spriteCentre("y");

            int OppositeC = Canvas.Height - OppositeA;
            int OppositeD = Canvas.Height - OppositeB;

            Opposite = Canvas.Height - OppositeC - OppositeD;

            // Working out the Adjacent Size
            int AdjacentA = Canvas.Width - MouseX;
            int AdjacentB = Canvas.Width - player.spriteCentre("x");

            int AdjacentC = Canvas.Width - AdjacentA;
            int AdjacentD = Canvas.Width - AdjacentB;

            Adjacent = Canvas.Height - AdjacentC - AdjacentD;

            AngleRadians = Math.Atan2(Opposite, Adjacent);
            AngleDegrees = AngleRadians * (180 / Math.PI);

            if(AngleDegrees < 0)
            {
                AngleDegrees = (AngleDegrees * -1) + 180;
            }

            return AngleDegrees;
        }

    }
}
