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
            // Moran process with some improvements

            var tournamentGameField = new Tournament.GameField(Options, Strategies);
            tournamentGameField.DoSteps();

            var score = Strategies.Select(s => new
            {
                s.Id,
                s.Name,
                Actions = tournamentGameField
                    .Actions
                    .Where(_ => _.ContainsStrategy(s.Id))
                    .ToArray(),
            }).Select(s => new
            {
                s.Id,
                s.Name,
                Score = s
                    .Actions
                    .Average(_ => _.GetScore(s.Id)),
            }).ToArray();

            var d1 = score.Max(s => s.Score) + score.Min(s => s.Score);

            var score2 = score.Select(s => new
            {
                s.Id,
                s.Score,
                s.Name,
                ReverseScore = d1 - s.Score,
                Random = Randomizer.Next(),
            }).OrderBy(_ => _.Random);

            var cumulative1 = 0.0;
            var cumulative2 = 0.0;

            var cSums = score2.Select(s => new
            {
                s.Id,
                s.Score,
                s.Name,
                Cumulative = (cumulative1 += s.Score),
                ReverseCumulative = (cumulative2 += s.ReverseScore),
            }).ToArray();

            Guid birthId;
            Guid deathId;

            List<string> selected = [];
            var total = cSums.Max(_ => _.Cumulative);
            while (true)
            {
                var r = Randomizer.NextDouble() * total;
                var s = cSums.First(_ => _.Cumulative >= r);
                selected.Add(s.Name);
                if (selected.Count(_ => _ == s.Name) == 3)
                {
                    birthId = s.Id;
                    break;
                }
            }

            selected = [];
            var reverseTotal = cSums.Max(_ => _.ReverseCumulative);
            while (true)
            {
                var r = Randomizer.NextDouble() * reverseTotal;
                var s = cSums.First(_ => _.ReverseCumulative >= r);
                var sId = s.Id.ToString();
                selected.Add(sId);
                if (selected.Count(_ => _ == sId) == 2)
                {
                    deathId = s.Id;
                    break;
                }
            }

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
