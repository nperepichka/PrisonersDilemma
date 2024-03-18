using Gameplay.Constructs;
using Gameplay.Games.Tournament.Constructs;
using Gameplay.Helpers;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Tournament
{
    internal class Game(Options options)
    {
        private const string TableFormat = "{0,7}{1,12}{2,27}{3,7}";

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
                Score = s.Actions.Average(_ => _.GetScore(s.Id, options)),
                Aggressive = Math.Round(s.Actions.Average(_ => _.GetAggressiveValue(s.Id)), 0),
            }).OrderByDescending(_ => _.Score);

            OutputHelper.Write(TableFormat, "Score", "Aggressive", "Name", "Flags");
            OutputHelper.WriteDivider(TableFormat);
            foreach (var s in score)
            {
                var selfishFlag = s.Selfish ? "S" : string.Empty;
                var niceFlag = s.Nice ? "N" : string.Empty;
                OutputHelper.Write(TableFormat, $"{s.Score:0.00}", $"{s.Aggressive}%", s.Name, $"{niceFlag,2}{selfishFlag,2}");
            }

            var selfishTotalScore = score.Where(_ => _.Selfish).Sum(s => s.Score);
            var humaneTotalScore = score.Where(_ => !_.Selfish).Sum(s => s.Score);
            var maxActions = actions.Max(_ => _.Strategy1Actions.Count);
            OutputHelper.Write($"Total score: selfish {selfishTotalScore:0.00} / humane {humaneTotalScore:0.00}   Max steps: {maxActions}", true);
        }

        public void RunGame()
        {
            var gameStrategies = StrategiesBuilder.GetStrategies(options);
            Strategies = gameStrategies.DistinctBy(_ => _.Name);

            List<History> actions = [];

            var hff = options.HumaneFlexible ? "HF " : "";
            var sff = options.SelfishFlexible ? "SF" : "";
            OutputHelper.Write($"Flexible: {options.FlexibilityValue:0.00} {hff}{sff}   Seed: {options.Seed:0.00}");

            for (var r = 0; r < options.TournamentRepeats; r++)
            {
                var gameField = new GameField(options, gameStrategies);
                gameField.DoSteps();
                actions.AddRange(gameField.Actions);
            }

            WriteScores(actions);
        }
    }
}
