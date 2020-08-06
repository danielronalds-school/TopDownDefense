using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownDefense
{
    class WaveManager
    {
        public int Wave = 1;
        public int onScreenEnemies;
        public int enemiesInWave;
        private int maxOnScreenEnemies = 15;
        private int minOnScreenEnemies = 4;

        public void nextWave()
        {
            onScreenEnemies = numberOfEnemiesOnScreen(Wave);
            enemiesInWave = numberOfEnemiesInWave(Wave);

            Console.WriteLine("Wave " + Wave + " Starting");
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
