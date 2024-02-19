using Gameplay.Constructs;
using Gameplay.Constructs.Values;
using Gameplay.Strategies;
using Gameplay.Strategies.Interfaces;

namespace Gameplay
{
    internal static class Game
    {
        private const string TableFormat = "{0,8}{1,12}{2,15}{3,25}{4,10}";

        private static void WriteScores(List<IStrategy> strategies, List<ActionsHistory> actions)
        {
            var score = strategies.Select(s => new
            {
                s.Name,
                s.Egotistical,
                Actions = actions.Where(_ => _.Strategy1Name == s.Name || _.Strategy2Name == s.Name),
            }).Select(s => new
            {
                s.Name,
                s.Egotistical,
                Score = s.Actions.Average(_ => _.GetScore(s.Name)),
                AggresiveNumber = s.Actions.Sum(_ => _.GetDefectsCount(s.Name)) * 10 / s.Actions.Sum(_ => _.GetStepsCount()),
            }).Select(s => new
            {
                s.Name,
                s.Egotistical,
                s.Score,
                AggresiveNumber = Math.Max(s.AggresiveNumber - 1, 0),
                Absolute = s.Score / Options.D,
                Cooperation = s.Score / Options.C,
            }).OrderByDescending(_ => _.Score);

            Console.WriteLine(string.Format(TableFormat, "Score", "Absolute", "Cooperation", "Name", "Flags"));
            Console.WriteLine("-----------------------------------------------------------------------");
            foreach (var s in score)
            {
                var succeedFlag = s.Absolute >= 63 && s.Cooperation >= 89 ? "*" : "";
                var egotisticalFlag = s.Egotistical ? "E" : "";
                var flagsStr = string.Format("{0,2}{1,2}{2,2}", succeedFlag, egotisticalFlag, s.AggresiveNumber);
                Console.WriteLine(string.Format(TableFormat, $"{s.Score:0.00}", $"{s.Absolute:0.00}%", $"{s.Cooperation:0.00}%", s.Name, flagsStr));
            }

            var egotisticalTotalScore = score.Where(_ => _.Egotistical).Sum(s => s.Score);
            var humaneTotalScore = score.Where(_ => !_.Egotistical).Sum(s => s.Score);
            Console.WriteLine($"Total score: egotistical {egotisticalTotalScore:0.00} / humane {humaneTotalScore:0.00}");
            Console.WriteLine();
        }

        public static void RunGame(double f, bool humaneFlexible, bool egotisticalFlexible)
        {
            var naff = humaneFlexible ? "HF " : "";
            var aff = egotisticalFlexible ? "EF" : "";
            Console.WriteLine($"Flexible: {f:0.00} {naff}{aff}");

            List<IStrategy> strategies = null;
            List<ActionsHistory> actions = [];
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
                    new CooperateTillDefect(),
                    new TitForTwoTats(),
                    new TwoTitsForTat(),
                    new TitForTatButCanDefect(),
                    new TitForTatButCanCooperate(),
                    new KindTitForTat(),
                    new AfraidTitForTat(),
                    new Tricky(),
                    new Smart()
                    );

                gameField.DoSteps();
                strategies ??= gameField.Strategies;
                actions.AddRange(gameField.Actions);
            }

            WriteScores(strategies, actions);
        }
    }
}
