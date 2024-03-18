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
        var optionsFiles = ProcessArgs(args);
        foreach (var optionsFile in optionsFiles)
        {
            RunGameWithOptions(optionsFile);
        }
    }

    private static void RunGameWithOptions(string optionsFile)
    {
        var options = Options.Init(optionsFile);
        OutputHelper.Write($"Running game with options: {optionsFile}");

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

    private static string[] ProcessArgs(string[] args)
    {
        if (args.Length > 0)
        {
            foreach (string arg in args)
            {
                if (!File.Exists(arg))
                {
                    throw new ArgumentException($"File not found: {arg}");
                }
            }

            return args;
        }

        return ["Options.json"];
    }
}