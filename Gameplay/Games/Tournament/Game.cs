using Gameplay.Constructs;
using Gameplay.Games.Helpers;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Tournament
{
    internal class Game(double f, bool humaneFlexible, bool selfishFlexible)
    {
        private const string TableFormat = "{0,8}{1,12}{2,15}{3,25}{4,10}";

        private Options Options { get; set; } = new Options(f, humaneFlexible, selfishFlexible);

        private IEnumerable<IStrategy> Strategies { get; set; }

        private void WriteScores(List<History> actions)
        {
            var score = Strategies.Select(s => new
            {
                s.Name,
                s.Selfish,
                s.Nice,
                Actions = actions.Where(_ => _.ContainsStrategy(s.Name)).ToArray(),
            }).Select(s => new
            {
                s.Name,
                s.Selfish,
                s.Nice,
                Score = s.Actions.Average(_ => _.GetScore(s.Name, Options.MinSteps)),
                AggressiveNumber = s.Actions.Sum(_ => _.GetDefectsCount(s.Name)) * 10 / s.Actions.Sum(_ => _.GetStepsCount()),
            }).Select(s => new
            {
                s.Name,
                s.Selfish,
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
                var selfishFlag = s.Selfish ? "S" : "";
                var niceFlag = s.Nice ? "N" : "";
                var flagsStr = $"{succeedFlag,2}{niceFlag,2}{selfishFlag,2}{s.AggressiveNumber,2}";
                Console.WriteLine(string.Format(TableFormat, $"{s.Score:0.00}", $"{s.Absolute:0.00}%", $"{s.Cooperation:0.00}%", s.Name, flagsStr));
            }

            var selfishTotalScore = score.Where(_ => _.Selfish).Sum(s => s.Score);
            var humaneTotalScore = score.Where(_ => !_.Selfish).Sum(s => s.Score);
            var maxActions = Math.Max(actions.Max(_ => _.GetStrategy1Actions().Count), actions.Max(_ => _.GetStrategy2Actions().Count));
            Console.WriteLine($"Total score: selfish {selfishTotalScore:0.00} / humane {humaneTotalScore:0.00}   Max steps: {maxActions}");
            Console.WriteLine();
        }

        public void RunGame()
        {
            var gameStrategies = StrategiesBuilder.GetAllStrategies();
            Strategies = gameStrategies.DistinctBy(_ => _.Name);

            List<History> actions = [];

            var hff = Options.HumaneFlexible ? "HF " : "";
            var sff = Options.SelfishFlexible ? "SF" : "";
            Console.WriteLine($"Flexible: {f:0.00} {hff}{sff}   Seed: {Options.Seed:0.00}");

            for (var r = 0; r < Options.Repeats; r++)
            {
                var gameField = new GameField(Options, gameStrategies);
                gameField.DoSteps();
                actions.AddRange(gameField.Actions);
            }

            WriteScores(actions);
        }
    }
}
