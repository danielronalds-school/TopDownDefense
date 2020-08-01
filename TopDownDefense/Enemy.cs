using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TopDownDefense
{
    public class Enemy
    {
        Angles angle = new Angles();

        private int x, y, width, height;
        private Image enemyImage;
        
        public Rectangle enemyRec;

        private Matrix enemyMatrix;

        private int enemySpeed = 3;
        private double xSpeed, ySpeed;

        string currentObjective = "Player";

        int objectiveAngle;

        public Enemy(int position_x, int position_y, string objective)
        {
            width = 44;
            height = width;
            x = position_x;
            y = position_y;
            enemyImage = Properties.Resources.Drone;

            enemyRec = new Rectangle(x,y,width,height);

            currentObjective = objective;
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

            objectiveAngle = (int)angle.CalculateAngle(enemyCentre(), objectivePoint);

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
    }
}
