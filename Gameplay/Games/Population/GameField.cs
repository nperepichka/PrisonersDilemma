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
                Score = s.Actions.Sum(_ => _.GetScore(s.Id, Options.MinSteps)),
            });

            var d1 = score.Min(s => s.Score) / 2;
            var d2 = score.Max(s => s.Score) + score.Min(s => s.Score);

            var score2 = score.Select(s => new
            {
                s.Id,
                s.Score,
                ReverseScore = d2 - s.Score,
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

            var birthIndex = 0;
            var deathIndex = Randomizer.Next(Strategies.Count);

            for (var i = 0; i < cSums.Length; i++)
            {
                if (cSums[i].Cumulative >= r1)
                {
                    birthIndex = i;
                    break;
                }
            }

            for (var i = 0; i < cSums.Length; i++)
            {
                if (cSums[i].ReverseCumulative >= r2)
                {
                    deathIndex = i;
                    break;
                }
            }

            var birth = Strategies.First(_ => _.Id == cSums[birthIndex].Id);
            var death = Strategies.First(_ => _.Id == cSums[deathIndex].Id);
            deathIndex = Strategies.IndexOf(death);
            Strategies[deathIndex] = birth.Clone();
        }

        /*private static double[] CumulativeSums(double[] values)
        {
            if (values == null || values.Length == 0)
            {
                return [];
            }

            var results = new double[values.Length];
            results[0] = values[0];

            for (var i = 1; i < values.Length; i++)
            {
                results[i] = results[i - 1] + values[i];
            }

            return results;
        }*/
    }
}
