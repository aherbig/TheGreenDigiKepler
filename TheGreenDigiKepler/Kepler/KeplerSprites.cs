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
            private static string ArtifactPathForFileName(string fileName) => Path.Combine("artifacts", Path.GetFileName(fileName));

            public static ExternalSprite KeplerCannon { get; } = LoadFromFile(ArtifactPathForFileName("KeplerCannon.png"));
            public static ExternalSprite KeplerCannon2 { get; } = LoadFromFile(ArtifactPathForFileName("KeplerCannon2.png"));
            public static ExternalSprite SalvageNet { get; } = LoadFromFile(ArtifactPathForFileName("SalvageNet.png"));
        }
        
        public static class Parts
        {
            private static string PartPathForFileName(string fileName) => Path.Combine("parts", Path.GetFileName(fileName));

            public static ExternalSprite Cannon { get; } = LoadFromFile(PartPathForFileName("KeplerParts_Cannon.png"));
            public static ExternalSprite Cockpit { get; } = LoadFromFile(PartPathForFileName("KeplerParts_Cockpit.png"));
            public static ExternalSprite BayActive { get; } = LoadFromFile(PartPathForFileName("KeplerParts_BayActive.png"));
            public static ExternalSprite BayInactive { get; } = LoadFromFile(PartPathForFileName("KeplerParts_BayInactive.png"));
        }
    }
}