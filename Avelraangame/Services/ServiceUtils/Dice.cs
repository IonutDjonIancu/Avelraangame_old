using System;

namespace Avelraangame.Services.ServiceUtils
{
    public static class Dice
    {
        private static readonly Random Random;

        static Dice()
        {
            Random = new Random();
        }


        #region WITH_REROLL
        /// <summary>
        /// Has reroll
        /// </summary>
        /// <returns></returns>
        public static int Roll_d_20()
        {
            int result = 0;

            while (true)
            {
                int roll = Random.Next(1, 21);
                result += roll;


                if (roll != 20)
                {
                    break;
                }

            }

            return result;
        }

        #endregion



        #region NO_REROLL
        /// <summary>
        /// No reroll method
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Roll_x_to_20(int x) => Random.Next(1, 21) * x;

        /// <summary>
        /// No reroll method
        /// </summary>
        /// <returns></returns>
        public static int Roll_0_to_100() => Random.Next(0, 101);

        /// <summary>
        /// No reroll method
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Roll_0_to_max(int max) => Random.Next(0, ++max);

        /// <summary>
        /// No reroll method
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Roll_min_to_max(int min, int max) => Random.Next(min, ++max);
        #endregion
    }
}
