namespace RIMA.RoomDesigner.Core
{
    public readonly struct GenerationInput
    {
        public GenerationInput(int seed, string biomeId, string archetypeId, int width, int height, int generatorVersion)
        {
            this.seed = seed;
            this.biomeId = biomeId ?? string.Empty;
            this.archetypeId = archetypeId ?? string.Empty;
            this.width = width;
            this.height = height;
            this.generatorVersion = generatorVersion;
        }

        public readonly int seed;
        public readonly string biomeId;
        public readonly string archetypeId;
        public readonly int width;
        public readonly int height;
        public readonly int generatorVersion;
    }
}
