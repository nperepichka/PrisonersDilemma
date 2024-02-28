using Gameplay.Enums;
using System.Diagnostics;
using PopulationGame = Gameplay.Games.Population.Game;
using TournamentGame = Gameplay.Games.Tournament.Game;

//var gameType = GameType.Tournament;
var gameType = GameType.Population;

var watch = Stopwatch.StartNew();

switch (gameType)
{
    case GameType.Tournament:
        new TournamentGame(0, false, false).RunGame();
        //new TournamentGame(0.25, false, true).RunGame();
        //new TournamentGame(0.25, true, false).RunGame();
        new TournamentGame(0.25, true, true).RunGame();
        //new TournamentGame(0.5, false, true).RunGame();
        //new TournamentGame(0.5, true, false).RunGame();
        new TournamentGame(0.5, true, true).RunGame();
        break;
    case GameType.Population:
        new PopulationGame(false).RunGame();
        //new PopulationGame(true).RunGame();
        break;
    default:
        throw new NotImplementedException($"Game type not implemented: {gameType}");
}

watch.Stop();
var elapsedMs = watch.ElapsedMilliseconds;
Console.WriteLine($"Time: {elapsedMs * 0.001:0.00}s");