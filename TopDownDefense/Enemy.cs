using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TopDownDefense
{
    class Enemy
    {
        private int x, y, width, height;
        private Image enemyImage;
        
        public Rectangle enemyRec;

        private Matrix enemyMatrix;

        private int enemySpeed = 3;
        private double xSpeed, ySpeed;

        string currentObjective = "Player";

        int objectiveAngle;

        public Enemy(int position_x, int position_y)
        {
            width = 42;
            height = width;
            x = position_x;
            y = position_y;
            enemyImage = Properties.Resources.Drone;

            enemyRec = new Rectangle(x,y,width,height);
        }

        public void DrawEnemy(Graphics g)
        {
            enemyMatrix = new Matrix();

            enemyMatrix.RotateAt(objectiveAngle, enemyCentre());
            g.Transform = enemyMatrix;

            g.DrawImage(enemyImage, enemyRec);
        }

        public void moveEnemy(Graphics g, Rectangle Crystal, Rectangle Player)
        {
            Point objectivePoint;

            if (currentObjective == "Crystal")
            {
                objectivePoint = new Point((Crystal.X + (Crystal.Width/2)), (Crystal.Y + (Crystal.Height / 2)));
            }
            else
            {
                objectivePoint = new Point((Player.X + (Player.Width / 2)), (Player.Y + (Player.Height / 2)));
            }

            objectiveAngle = (int)CalculateAngle(enemyCentre(), objectivePoint);

            xSpeed = enemySpeed * (Math.Cos((objectiveAngle) * Math.PI / 180));
            ySpeed = enemySpeed * (Math.Sin((objectiveAngle) * Math.PI / 180));

            x += (int)xSpeed;
            y += (int)ySpeed;
            enemyRec.Location = new Point(x, y);
        }

        private Point enemyCentre()
        {
            Point enemyCentre;

            int x = enemyRec.X + (enemyRec.Width/2);
            int y = enemyRec.Y + (enemyRec.Height / 2);

            enemyCentre = new Point(x, y);

            return enemyCentre;
        }

        public double CalculateAngle(Point start, Point arrival)
        {
            var radian = Math.Atan2((arrival.Y - start.Y), (arrival.X - start.X));
            var angle = (radian * (180 / Math.PI) + 360) % 360;

            return angle;
        }
    }
}
