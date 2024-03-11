namespace Gameplay.Games.Population
{
    internal class Generation()
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public double Score { get; set; }

        public double Total { get; set; }

        public int Children { get; set; }
    }
}
