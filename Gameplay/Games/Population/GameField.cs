using Gameplay.Constructs;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Population
{
    internal class GameField
    {
        public GameField(Options options, List<IStrategy> strategies)
        {
            Options = options;
            Randomizer = new();
            Strategies = [];
            Strategies = [.. strategies];
            OriginalStrategies = [.. strategies];
        }

        private Random Randomizer { get; set; }

        private Options Options { get; set; }

        public List<IStrategy> Strategies { get; private set; }

        private List<IStrategy> OriginalStrategies { get; set; }

        public void DoStep()
        {
            // Moran process

            var tournamentGameField = new Tournament.GameField(Options, Strategies);
            tournamentGameField.DoSteps();

            var score = Strategies.Select(s => new
            {
                s.Id,
                Actions = tournamentGameField
                    .Actions
                    .Where(_ => _.ContainsStrategy(s.Id))
                    .ToArray(),
            }).Select(s => new
            {
                s.Id,
                Score = s
                    .Actions
                    .Average(_ => _.GetScore(s.Id, Options.MinSteps)),
            }).ToArray();

            var d1 = score.Max(s => s.Score) + score.Min(s => s.Score);

            var score2 = score.Select(s => new
            {
                s.Id,
                s.Score,
                ReverseScore = d1 - s.Score,
                Random = Randomizer.Next(),
            }).OrderBy(_ => _.Random);

            var cumulative1 = 0.0;
            var cumulative2 = 0.0;

            var cSums = score2.Select(s => new
            {
                s.Id,
                s.Score,
                Cumulative = (cumulative1 += s.Score),
                ReverseCumulative = (cumulative2 += s.ReverseScore),
            }).ToArray();

            var total = cSums.Max(_ => _.Cumulative);
            var r = Randomizer.NextDouble() * total;
            var birthId = cSums.First(_ => _.Cumulative >= r).Id;

            var reverseTotal = cSums.Max(_ => _.ReverseCumulative);
            r = Randomizer.NextDouble() * reverseTotal;
            var deathId = cSums.First(_ => _.ReverseCumulative >= r).Id;

            var birth = GetBirthStategy(() =>
            {
                return Strategies.First(_ => _.Id == birthId);
            });
            var death = Strategies.First(_ => _.Id == deathId);
            var deathIndex = Strategies.IndexOf(death);
            Strategies[deathIndex] = birth.Clone();
        }

        private IStrategy GetBirthStategy(Func<IStrategy> selectStrategy)
        {
            return Options.Mutation > 0 && Randomizer.Next(0, 10001) <= Options.Mutation * 100
                ? OriginalStrategies[Randomizer.Next(OriginalStrategies.Count)]
                : selectStrategy();
        }
    }
}
