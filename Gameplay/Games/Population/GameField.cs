using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Population
{
    internal class GameField(Options options, IList<IStrategy> strategies) :
        Abstracts.GameField<Options>(options, strategies)
    {
        protected override void AddStrategies(IList<IStrategy> strategies)
        {
            Strategies = strategies.ToList();
        }

        private readonly Tournament.Options TournamentOptions = new(options.HumaneFlexible, options.SelfishFlexible, options.f, 0.05);

        public void DoStep()
        {
            // Moran process will be used to get next generation

            var tournamentGameField = new Tournament.GameField(TournamentOptions, Strategies);
            tournamentGameField.DoSteps();

            var score = Strategies.Select(s => new
            {
                s.Id,
                Actions = tournamentGameField.Actions.Where(_ => _.ContainsStrategy(s.Id)).ToArray(),
            }).Select(s => new
            {
                s.Id,
                Score = s.Actions.Average(_ => _.GetScore(s.Id, Options.MinSteps)),
            });

            var d = score.Max(s => s.Score) + score.Min(s => s.Score);

            var score2 = score.Select(s => new
            {
                s.Id,
                s.Score,
                ReverseScore = d - s.Score,
            }).OrderByDescending(_ => _.Score);

            var cumulative1 = 0.0;
            var cumulative2 = 0.0;

            var cSums = score2.Select(s => new
            {
                s.Id,
                s.Score,
                Cumulative = (cumulative1 += s.Score),
                ReverseCumulative = (cumulative2 += s.ReverseScore),
            }).ToArray();

            var total1 = cSums.Max(_ => _.Cumulative);
            var r1 = Randomizer.NextDouble() * total1;

            var total2 = cSums.Max(_ => _.ReverseCumulative);
            var r2 = Randomizer.NextDouble() * total2;

            var birthId = Guid.Empty;
            var deathId = Guid.Empty;

            for (var i = 0; i < cSums.Length; i++)
            {
                if (cSums[i].Cumulative >= r1)
                {
                    birthId = cSums[i].Id;
                    break;
                }
            }

            for (var i = 0; i < cSums.Length; i++)
            {
                if (cSums[i].ReverseCumulative >= r2)
                {
                    deathId = cSums[i].Id;
                    break;
                }
            }

            var birth = Strategies.First(_ => _.Id == birthId);
            var death = Strategies.First(_ => _.Id == deathId);
            var deathIndex = Strategies.IndexOf(death);
            Strategies[deathIndex] = birth.Clone();
        }
    }
}
