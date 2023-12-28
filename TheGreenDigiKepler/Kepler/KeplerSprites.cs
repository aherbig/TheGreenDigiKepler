using CobaltCoreModding.Definitions.ExternalItems;

namespace TheGreenDigiKepler.Kepler
{
    public static class KeplerSprites
    {
        public static DirectoryInfo? ModRootFolder { get; set; }
        private static string SpriteFolder => Path.Combine(ModRootFolder.FullName, "Sprites");

        private static ExternalSprite LoadFromFile(string filePath)
        {
            var path = Path.Combine(SpriteFolder, filePath);
            var sprite = new ExternalSprite($"{typeof(KeplerSprites).Namespace}.Sprites.{filePath}", new FileInfo(path));
            return sprite;
        }

        public static class Artifacts
        {
            public static ExternalSprite KeplerCannon { get; } = LoadFromFile(Path.Combine("artifacts", Path.GetFileName("KeplerCannon.png")));
            public static ExternalSprite KeplerCannon2 { get; } = LoadFromFile(Path.Combine("artifacts", Path.GetFileName("KeplerCannon2.png")));
            public static ExternalSprite SalvageNet { get; } = LoadFromFile(Path.Combine("artifacts", Path.GetFileName("SalvageNet.png")));
        }
    }
}