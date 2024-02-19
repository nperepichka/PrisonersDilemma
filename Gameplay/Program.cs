using Gameplay;
using System.Diagnostics;

var watch = Stopwatch.StartNew();

Game.RunGame(0, false, false);
//Game.RunGame(0.25, false, true);
//Game.RunGame(0.25, true, false);
Game.RunGame(0.25, true, true);
//Game.RunGame(0.5, false, true);
//Game.RunGame(0.5, true, false);
Game.RunGame(0.5, true, true);

watch.Stop();
var elapsedMs = watch.ElapsedMilliseconds;
Console.WriteLine($"Time: {elapsedMs * 0.001:0.00}s");