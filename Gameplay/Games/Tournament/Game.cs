using Gameplay.Constructs;
using Gameplay.Games.Tournament.Constructs;
using Gameplay.Strategies.Helpers;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Tournament
{
    internal class Game(Options options)
    {
        private const string TableFormat = "{0,8}{1,12}{2,15}{3,25}{4,10}";

        private IEnumerable<IStrategy> Strategies { get; set; }

        private void WriteScores(List<History> actions)
        {
            var score = Strategies.Select(s => new
            {
                s.Name,
                s.Selfish,
                s.Nice,
                s.Id,
                Actions = actions.Where(_ => _.ContainsStrategy(s.Id)).ToArray(),
            }).Select(s => new
            {
                s.Name,
                s.Selfish,
                s.Nice,
                Score = s.Actions.Average(_ => _.GetScore(s.Id, options.MinSteps)),
                AggressiveNumber = s.Actions.Sum(_ => _.GetDefectsCount(s.Id)) * 10 / s.Actions.Sum(_ => _.GetStepsCount()),
            }).Select(s => new
            {
                s.Name,
                s.Selfish,
                s.Nice,
                s.Score,
                AggressiveNumber = Math.Max(s.AggressiveNumber - 1, 0),
                Absolute = s.Score / options.D,
                Cooperation = s.Score / options.C,
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
            var maxActions = Math.Max(actions.Max(_ => _.Strategy1Actions.Count), actions.Max(_ => _.Strategy2Actions.Count));
            Console.WriteLine($"Total score: selfish {selfishTotalScore:0.00} / humane {humaneTotalScore:0.00}   Max steps: {maxActions}");
            Console.WriteLine();
        }

        public void RunGame()
        {
            var gameStrategies = StrategiesBuilder.GetAllStrategies();
            Strategies = gameStrategies.DistinctBy(_ => _.Name);

            List<History> actions = [];

            var hff = options.HumaneFlexible ? "HF " : "";
            var sff = options.SelfishFlexible ? "SF" : "";
            Console.WriteLine($"Flexible: {options.FlexibilityValue:0.00} {hff}{sff}   Seed: {options.Seed:0.00}");

            for (var r = 0; r < options.Repeats; r++)
            {
                var gameField = new GameField(options, gameStrategies);
                gameField.DoSteps();
                actions.AddRange(gameField.Actions);
            }

            WriteScores(actions);
        }
    }
}
