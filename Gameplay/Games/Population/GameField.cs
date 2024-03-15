using Gameplay.Enums;
using Gameplay.Games.Population.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Population
{
    internal class GameField(Options options, IList<IStrategy> strategies) :
        Abstracts.GameField<Options>(options, strategies)
    {
        protected override void AddStrategies(IList<IStrategy> strategies)
        {
            Strategies = strategies.ToList();
            OriginalStrategies = strategies.ToList();
        }

        private IList<IStrategy> OriginalStrategies { get; set; }

        private readonly Tournament.Options TournamentOptions = new(options.HumaneFlexible, options.SelfishFlexible, options.f, 0.05);

        public void DoStep()
        {
            var tournamentGameField = new Tournament.GameField(TournamentOptions, Strategies);
            tournamentGameField.DoSteps();

            var score = Strategies.Select(s => new
            {
                s.Id,
                s.Name,
                Actions = tournamentGameField.Actions.Where(_ => _.ContainsStrategy(s.Id)).ToArray(),
            }).Select(s => new
            {
                s.Id,
                s.Name,
                Score = s.Actions.Average(_ => _.GetScore(s.Id, Options.MinSteps)),
            })
            .OrderByDescending(_ => _.Score)
            .ToArray();

            var birthId = Guid.Empty;
            var deathId = Guid.Empty;

            switch (Options.PopulationBuildType)
            {
                case PopulationBuildType.MoranProcess:
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
                    break;
                case PopulationBuildType.MinMax:
                    birthId = score.First().Id;
                    deathId = score.Last().Id;
                    break;
                default:
                    throw new NotImplementedException($"Not implemented: {Options.PopulationBuildType}");
            }

            var birth = GetBirthStategy(() => {
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
