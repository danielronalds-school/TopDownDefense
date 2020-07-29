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
            centre = new Point(spriteCentre("y"), spriteCentre("x"));

            matrix = new Matrix();

            matrix.RotateAt((int)rotationAngle, centre);
            g.Transform = matrix;
            playerRec.Location = new Point(x, y);
            g.DrawImage(playerImage, playerRec);
        }

        public int spriteCentre(string Axis)
        {
            if(Axis == "y")
            {
                return y + (height / 2);
            }
            return x + (width / 2);
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
