using Gameplay.Enums;
using System.Diagnostics;
using TournamentGame = Gameplay.Games.Tournament.Game;
using PopulationGame = Gameplay.Games.Population.Game;

var gameType = GameType.Tournament;

var watch = Stopwatch.StartNew();

switch (gameType)
{
    case GameType.Tournament:
        TournamentGame.RunGame(0, false, false);
        //TournamentGame.RunGame(0.25, false, true);
        //TournamentGame.RunGame(0.25, true, false);
        TournamentGame.RunGame(0.25, true, true);
        //TournamentGame.RunGame(0.5, false, true);
        //TournamentGame.RunGame(0.5, true, false);
        TournamentGame.RunGame(0.5, true, true);
        break;
    case GameType.Population:
        PopulationGame.RunGame(false, false);
        //PopulationGame.RunGame(true, true);
        break;
    default:
        throw new NotImplementedException($"Game type not implemented: {gameType}");
}

watch.Stop();
var elapsedMs = watch.ElapsedMilliseconds;
Console.WriteLine($"Time: {elapsedMs * 0.001:0.00}s");