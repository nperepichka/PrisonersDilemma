using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Helpers;
using System.Diagnostics;
using PopulationGame = Gameplay.Games.Population.Game;
using TournamentGame = Gameplay.Games.Tournament.Game;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            throw new ArgumentException("Invalid arguments passed");
        }
        var optionsPath = args.Length == 0 ? "Options.json" : args[0];
        RunGameWithOptions(optionsPath);
    }

    private static void RunGameWithOptions(string optionsFile)
    {
        var options = Options.Init(optionsFile);
        var watch = Stopwatch.StartNew();

        switch (options.GameType)
        {
            case GameType.Tournament:
                new TournamentGame(options).RunGame();
                break;
            case GameType.Population:
                new PopulationGame(options).RunGame();
                break;
            default:
                throw new NotImplementedException($"Game type not implemented: {options.GameType}");
        }

        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        OutputHelper.Write($"Time: {elapsedMs * 0.001:0.00}s");
    }
}