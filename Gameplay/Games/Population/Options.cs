namespace Gameplay.Games.Population
{
    internal class Options(bool flexible) :
        Abstracts.Options(flexible, flexible, flexible ? DefaultFlexibility : 0)
    {
        public readonly int SamePopulationStepsToStop = 5;
    }
}
