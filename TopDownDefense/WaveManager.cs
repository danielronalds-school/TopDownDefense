﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TopDownDefense
{
    class WaveManager
    {
        public int Wave = 1;
        public int onScreenEnemies;
        public int enemiesInWave;
        private int currentEnemiesInWave;
        private int maxOnScreenEnemies = 15;
        private int minOnScreenEnemies = 4;

        public void nextWave()
        {
            onScreenEnemies = numberOfEnemiesOnScreen(Wave);
            enemiesInWave = numberOfEnemiesInWave(Wave);

            currentEnemiesInWave = enemiesInWave;

            Console.WriteLine("Wave " + Wave + " Starting");
        }

        public void drawProgressionBar(Graphics g, Size canvas)
        {
            int barWidth = 1000;
            int barHeight = 10;

            decimal percentage = (decimal)/*If this isnt there, it automaticly rounds and messes it all up*/enemiesInWave / currentEnemiesInWave;

            int rectWidth = barWidth - (int)(percentage * 1000);
            int rectHeight = barHeight;

            Console.WriteLine("Enemies left in wave: " + enemiesInWave);
            Console.WriteLine("Percentage of enemies left: " + percentage);

            Brush waveBarBrush = new SolidBrush(Color.Blue);
            Brush backgroundBrush = new SolidBrush(Color.LightGray);

            Size rectSize = new Size(rectWidth, rectHeight);

            int rectX, rectY;

            rectX = 0 + (canvas.Width/2 - barWidth/2);
            rectY = 20;

            Point rectPoint = new Point(rectX, rectY);

            Rectangle waveBarRect;
            Rectangle barBacking;

            waveBarRect = new Rectangle(rectPoint, rectSize);
            barBacking = new Rectangle(rectPoint.X, rectPoint.Y, barWidth, barHeight);

            g.FillRectangle(backgroundBrush, barBacking);
            g.FillRectangle(waveBarBrush, waveBarRect);
        }

        public bool waveOver()
        {
            if(enemiesInWave <= 0)
            {
                return true;
            }
            return false;
        }

        private int numberOfEnemiesOnScreen(int wave)
        {
            double result;

            result = (wave ^ 2 / 20) + wave;

            if (result > maxOnScreenEnemies)
            {
                result = maxOnScreenEnemies;
            } else if(result < minOnScreenEnemies)
            {
                result = minOnScreenEnemies;
            }


            return (int)result;
        }

        private int numberOfEnemiesInWave(int wave)
        {
            double result;

            result = (5 * wave) + 10;

            return (int)result;
        }
    }
}
