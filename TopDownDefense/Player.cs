using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TopDownDefense
{
    class Player
    {

        public int x, y, width, height;

        public Image playerImage;

        public Rectangle playerRec;

        public int PlayerSpeed = 3;

        public Matrix matrix;

        Point centre;

        public Player(int position_x, int position_y, int scale, int angle)
        {
            playerImage = Properties.Resources.TopDownCharacter;
            x = position_x;
            y = position_y;
            width = 135 * scale;
            height = 80 * scale;
            playerRec = new Rectangle(x, y, width, height);
        }

        public void DrawPlayer(Graphics g, Double rotationAngle)
        {
            playerRec.Location = new Point(x, y);

            centre = new Point((playerRec.X+(playerRec.Width/2)),(playerRec.Y+(playerRec.Height/2)));

            matrix = new Matrix();


            matrix.RotateAt((int)rotationAngle, playerRec.Location);
            g.DrawEllipse(Pens.Red, new Rectangle(playerRec.Location, new Size(7, 7)));
            g.Transform = matrix;
            /*g.TranslateTransform(playerRec.X, playerRec.Y);
            g.RotateTransform((int)rotationAngle);*/
            g.DrawImage(playerImage, playerRec);
        }

        public int spriteCentre(string Axis)
        {
            if(Axis == "y")
            {
                return playerRec.Y;
            }
            return playerRec.X;
        }

        public void MovePlayer(bool playerLeft, bool playerRight, bool playerUp, bool playerDown)
        {
            if(playerLeft)
            {
                x -= PlayerSpeed;
            }

            if (playerRight)
            {
                x += PlayerSpeed;
            }

            if (playerUp)
            {
                y -= PlayerSpeed;
            }

            if (playerDown)
            {
                y += PlayerSpeed;
            }
        }

    }
}
