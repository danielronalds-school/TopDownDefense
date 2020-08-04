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

        Random random = new Random();

        private int x, y, width, height;
        private Image enemyImage;
        
        public Rectangle enemyRec;

        private Matrix enemyMatrix;

        private string prefferedTarget;

        private int enemySpeed = 2;
        private double xSpeed, ySpeed;

        string currentObjective = "Player";

        int AgroRange = 200;

        public int Damage = 1;

        public int health = 15;

        public bool enemyHit = false;
        int hitShake = 15;

        int objectiveAngle;

        public Enemy(Point position, string objective)
        {
            width = 43;
            height = width;
            x = position.X;
            y = position.Y;
            enemyImage = Properties.Resources.Drone_2;

            enemyRec = new Rectangle(x,y,width,height);

            prefferedTarget = objective;

            if(prefferedTarget == "Player")
            {
                enemySpeed = 3;
            }
        }

        public void DrawEnemy(Graphics g)
        {
            enemyMatrix = new Matrix();

            if(enemyHit)
            {
                if (random.Next(1, 10) < 6)
                {
                    objectiveAngle -= hitShake;
                }
                else
                {
                    objectiveAngle += hitShake;
                }

                enemyHit = false;
            }

            enemyMatrix.RotateAt(objectiveAngle, enemyCentre());
            g.Transform = enemyMatrix;

            g.DrawImage(enemyImage, enemyRec);
        }

        public void moveEnemy(Graphics g, Rectangle Crystal, Rectangle Player)
        {
            Point objectivePoint;
            currentObjective = prefferedTarget;


            if (prefferedTarget == "Player")
            {
                objectivePoint = new Point((Player.X + (Player.Width / 2)), (Player.Y + (Player.Height / 2)));
            }
            else if (prefferedTarget == "Crystal")
            {
                objectivePoint = new Point((Crystal.X + (Crystal.Width / 2)), (Crystal.Y + (Crystal.Height / 2)));
            }
            else
            {
                if (InXAgroRange(Player) && InYAgroRange(Player) && prefferedTarget == "any")
                {
                    objectivePoint = new Point((Player.X + (Player.Width / 2)), (Player.Y + (Player.Height / 2)));
                }
                else
                {
                    objectivePoint = new Point((Crystal.X + (Crystal.Width / 2)), (Crystal.Y + (Crystal.Height / 2)));
                }
            }

            objectiveAngle = (int)angle.CalculateAngle(enemyCentre(), objectivePoint);

            xSpeed = enemySpeed * (Math.Cos((objectiveAngle) * Math.PI / 180));
            ySpeed = enemySpeed * (Math.Sin((objectiveAngle) * Math.PI / 180));

            x += (int)xSpeed;
            y += (int)ySpeed;
            enemyRec.Location = new Point(x, y);
        }

        private bool InXAgroRange(Rectangle Player)
        {
            int Range = Player.X - enemyRec.X;

            if (Range <= AgroRange && Range >= (AgroRange * -1))
            {
                return true;
            }
            return false;
        }

        private bool InYAgroRange(Rectangle Player)
        {
            int Range = Player.Y - enemyRec.Y;

            if (Range <= AgroRange && Range >= (AgroRange * -1))
            {
                return true;
            }
            return false;
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
