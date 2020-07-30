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

        private int x, y, width, height;

        private Image playerImage;

        public Rectangle playerRec;

        private int PlayerSpeed = 3;

        private int fireDelay;

        private int maxFireDelay = 5;

        private Matrix matrix;

        public List<Projectile> projectiles = new List<Projectile>();

        public Player(int position_x, int position_y, int scale, int angle)
        {
            playerImage = Properties.Resources.TopDownCharacter;
            x = position_x;
            y = position_y;
            width = 135 * scale;
            height = 80 * scale;
            playerRec = new Rectangle(x, y, width, height);
        }

        public void DrawPlayer(Graphics g, Point Mouse, bool playerFire)
        {
            playerRec.Location = new Point(x, y);

            matrix = new Matrix();

            matrix.RotateAt((int)CalculeAngle(rifleBarrel(),Mouse), spriteCentre());
            g.Transform = matrix;
            /*g.TranslateTransform(playerRec.X, playerRec.Y);
            g.RotateTransform((int)rotationAngle);*/
            g.DrawImage(playerImage, playerRec);
            g.DrawEllipse(Pens.Red, new Rectangle(spriteCentre(), new Size(9, 9))); // Sprite Centre Visulisation
            g.DrawEllipse(Pens.Green, new Rectangle(rifleBarrel(), new Size(9, 9)));// Rifle Visulisation

            if(playerFire && fireDelay >= maxFireDelay)
            {
                fireDelay = 0;
                projectiles.Add(new Projectile(rifleBarrel(), (int)CalculeAngle(rifleBarrel(), Mouse)));
            }
            else if(fireDelay < maxFireDelay)
            {
                fireDelay++;
            }
        }

        public Point spriteCentre()
        {
            Point SpriteCentre;
            int SpriteCentreX = playerRec.Location.X + (width / 3);
            int SpriteCentreY = playerRec.Location.Y + (height / 2);
            SpriteCentre = new Point(SpriteCentreX, SpriteCentreY);
            return SpriteCentre;
        }

        public Point rifleBarrel()
        {
            Point rifleBarrel;
            int rifleBarrelX = playerRec.Location.X + ((width/4)*3) + 10;
            int rifleBarrelY = playerRec.Location.Y + (height / 2) + 11;
            rifleBarrel = new Point(rifleBarrelX,rifleBarrelY);
            return rifleBarrel;
        }

        public double CalculeAngle(Point start, Point arrival)
        {
            var radian = Math.Atan2((arrival.Y - start.Y), (arrival.X - start.X));
            var angle = (radian * (180 / Math.PI) + 360) % 360;

            return angle;
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
