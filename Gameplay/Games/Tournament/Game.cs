using Gameplay.Strategies;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Tournament
{
    internal static class Game
    {
        private const string TableFormat = "{0,8}{1,12}{2,15}{3,25}{4,10}";

        private static void WriteScores(IEnumerable<IStrategy> strategies, List<History> actions)
        {
            var score = strategies.Select(s => new
            {
                s.Name,
                s.Egotistical,
                s.Nice,
                Actions = actions.Where(_ => _.Strategy1Name == s.Name || _.Strategy2Name == s.Name),
            }).Select(s => new
            {
                s.Name,
                s.Egotistical,
                s.Nice,
                Score = s.Actions.Average(_ => _.GetScore(s.Name)),
                AggressiveNumber = s.Actions.Sum(_ => _.GetDefectsCount(s.Name)) * 10 / s.Actions.Sum(_ => _.GetStepsCount()),
            }).Select(s => new
            {
                s.Name,
                s.Egotistical,
                s.Nice,
                s.Score,
                AggressiveNumber = Math.Max(s.AggressiveNumber - 1, 0),
                Absolute = s.Score / Options.D,
                Cooperation = s.Score / Options.C,
            }).OrderByDescending(_ => _.Score);

            Console.WriteLine(string.Format(TableFormat, "Score", "Absolute", "Cooperation", "Name", "Flags"));
            Console.WriteLine("-----------------------------------------------------------------------");
            foreach (var s in score)
            {
                var succeedFlag = s.Absolute >= 60 && s.Cooperation >= 85 ? "*" : "";
                var egotisticalFlag = s.Egotistical ? "E" : "";
                var niceFlag = s.Nice ? "N" : "";
                var flagsStr = string.Format("{0,2}{1,2}{2,2}{3,2}", succeedFlag, niceFlag, egotisticalFlag, s.AggressiveNumber);
                Console.WriteLine(string.Format(TableFormat, $"{s.Score:0.00}", $"{s.Absolute:0.00}%", $"{s.Cooperation:0.00}%", s.Name, flagsStr));
            }

            var egotisticalTotalScore = score.Where(_ => _.Egotistical).Sum(s => s.Score);
            var humaneTotalScore = score.Where(_ => !_.Egotistical).Sum(s => s.Score);
            var maxActions = Math.Max(actions.Max(_ => _.GetStrategy1Actions().Count), actions.Max(_ => _.GetStrategy2Actions().Count));
            Console.WriteLine($"Total score: egotistical {egotisticalTotalScore:0.00} / humane {humaneTotalScore:0.00}   Max steps: {maxActions}");
            Console.WriteLine();
        }

        public static void RunGame(double f, bool humaneFlexible, bool egotisticalFlexible)
        {
            var naff = humaneFlexible ? "HF " : "";
            var aff = egotisticalFlexible ? "EF" : "";
            Console.WriteLine($"Flexible: {f:0.00} {naff}{aff}   Seed: {Options.Seed:0.00}");

            IEnumerable<IStrategy> strategies = null;
            List<History> actions = [];
            Options.HumaneFlexible = humaneFlexible;
            Options.EgotisticalFlexible = egotisticalFlexible;
            Options.f = f;

            for (var r = 0; r < Options.Repeats; r++)
            {
                using var gameField = new GameField(
                    new AlwaysCooperate(),
                    new AlwaysDefect(),
                    new TitForTat(),
                    new AllRandom(),
                    new Friedman(),
                    new TitForTwoTats(),
                    new TwoTitsForTat(),
                    new TitForTatButCanDefect(),
                    new TitForTatButCanCooperate(),
                    new Grofman(),
                    new Tullock(),
                    new Graaskamp(),
                    new Downing(),
                    new Smart()
                    );

                gameField.DoSteps();
                strategies ??= gameField.Strategies.DistinctBy(_ => _.Name);
                actions.AddRange(gameField.Actions);
            }

            WriteScores(strategies, actions);
        }
    }
}
