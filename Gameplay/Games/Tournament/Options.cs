﻿namespace Gameplay.Games.Tournament
{
    internal static class Options
    {
        // D > C > d > c
        // 2C > D + c
        public const int D = 7;
        public const int C = 5;
        public const int d = 2;
        public const int c = 0;

        // Flexibility of interaction (f) - the author's idea of strategy research
        public static bool HumaneFlexible = true;
        public static bool EgotisticalFlexible = true;

        public static double f = 0.5;

        public const int MinSteps = 100;
        public const int Repeats = 1;
    }
}