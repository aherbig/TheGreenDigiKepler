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

            public static ExternalSprite Bay { get; } = LoadFromFile(ArtifactPathForFileName("KeplerArtifacts_Bay.png"));
            public static ExternalSprite BayV2 { get; } = LoadFromFile(ArtifactPathForFileName("KeplerArtifacts_BayV2.png"));
            public static ExternalSprite MissileTractorBeam { get; } = LoadFromFile(ArtifactPathForFileName("KeplerArtifacts_MissileTractorBeam.png"));
            public static ExternalSprite MissilePDS { get; } = LoadFromFile(ArtifactPathForFileName("KeplerArtifacts_MissilePDS.png"));
        }
        
        public static class Parts
        {
            private static string PartPathForFileName(string fileName) => Path.Combine("parts", Path.GetFileName(fileName));

            public static ExternalSprite Chassis { get; } = LoadFromFile(PartPathForFileName("Kepler_Chassis.png"));
            public static ExternalSprite Cannon { get; } = LoadFromFile(PartPathForFileName("KeplerParts_Cannon.png"));
            public static ExternalSprite Cockpit { get; } = LoadFromFile(PartPathForFileName("KeplerParts_Cockpit.png"));
            public static ExternalSprite BayActive { get; } = LoadFromFile(PartPathForFileName("KeplerParts_BayActive.png"));
            public static ExternalSprite BayInactive { get; } = LoadFromFile(PartPathForFileName("KeplerParts_BayInactive.png"));
        }

        public static class Cards
        {
            private static string CardPathForFileName(string fileName) => Path.Combine("cards", Path.GetFileName(fileName));

            public static ExternalSprite BasicDrone { get; } = LoadFromFile(CardPathForFileName("BasicDrone.png"));
            public static ExternalSprite KeplerCardArt { get; } = LoadFromFile(CardPathForFileName("KeplerCardArt.png"));
            public static ExternalSprite KeplerCardFrame { get; } = LoadFromFile(CardPathForFileName("KeplerCardFrame.png"));
            public static ExternalSprite SwarmMode { get; } = LoadFromFile(CardPathForFileName("SwarmMode.png"));
        }
    }
}