using Gameplay.Constructs;
using Gameplay.Enums;
using System.Diagnostics;
using PopulationGame = Gameplay.Games.Population.Game;
using TournamentGame = Gameplay.Games.Tournament.Game;

var options = Options.Init();

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
Console.WriteLine($"Time: {elapsedMs * 0.001:0.00}s");