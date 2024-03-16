using Gameplay.Enums;
using Gameplay.Games.Population.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gameplay.Constructs
{
    internal class Options
    {
        public GameType GameType { get; set; }

        // TODO: implement
        public string[] Strategies { get; set; }

        #region Socres

        // D > C > d > c
        // 2C > D + c

        public int D { get; set; }

        public int C { get; set; }

        public int d { get; set; }

        public int c { get; set; }

        #endregion

        #region Flexibility

        // Flexibility of interaction - the author's idea of strategy research

        public bool HumaneFlexible { get; set; }

        public bool SelfishFlexible { get; set; }

        public double FlexibilityValue { get; set; }

        #endregion

        #region Possibility

        // Chance of strategy to play randomly (0.00% - 100.00%)
        public double Seed { get; set; }

        // Chance of strategy to mutate (0.00% - 100.00%)
        public double Mutation { get; set; }

        #endregion

        #region Population

        public int SamePopulationStepsToStop { get; set; }

        public PopulationBuildType PopulationBuildType { get; set; }

        public string DominationStrategy { get; set; }

        #endregion

        #region Tournament

        public int MinSteps { get; set; }

        public int SameLastCooperationScores { get; set; }

        public double ValuableCooperationScoreNumber { get; set; }

        public int Repeats { get; set; }

        #endregion

        public static Options Init(string optionsFilePath)
        {
            var jsonLines = File.ReadAllLines(optionsFilePath)
                .Where(_ => !_.Trim().StartsWith("//"));
            var json = string.Join(Environment.NewLine, jsonLines);

            var jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<Options>(json, jsonSerializerOptions)
                ?? throw new ArgumentNullException("Invalid options");
        }
    }
}
