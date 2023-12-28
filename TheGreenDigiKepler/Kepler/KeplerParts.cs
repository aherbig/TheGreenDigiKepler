using CobaltCoreModding.Definitions.ExternalItems;

namespace TheGreenDigiKepler.Kepler
{
    public static class KeplerParts
    {
        public static ExternalPart LeftMissileBay { get; } = CreateLeftMissileBay();
        public static ExternalPart Cockpit { get; } = CreateCockpit();
        public static ExternalPart Cannon { get; } = CreateCannon();
        public static ExternalPart RightMissileBay { get; } = CreateRightMissileBay();

        private static ExternalPart CreateLeftMissileBay()
        {
            return new ExternalPart
            (
                $"{typeof(KeplerParts).FullName}.LeftMissileBay",
                new Part()
                {
                    active = true,
                    damageModifier = PDamMod.none,
                    type = PType.missiles,
                },
                KeplerSprites.Parts.LeftBayActive,
                KeplerSprites.Parts.LeftBayInactive
            );
        }

        private static ExternalPart CreateRightMissileBay()
        {
            return new ExternalPart
            (
                $"{typeof(KeplerParts).FullName}.RightMissileBay",
                new Part()
                {
                    active = false,
                    damageModifier = PDamMod.none,
                    type = PType.missiles,
                },
                KeplerSprites.Parts.RightBayActive,
                KeplerSprites.Parts.RightBayInactive
            );
        }

        private static ExternalPart CreateCockpit()
        {
            return new ExternalPart
            (
                $"{typeof(KeplerParts).FullName}.Cockpit",
                new Part()
                {
                    active = true,
                    damageModifier = PDamMod.none,
                    type = PType.cockpit,
                },
                KeplerSprites.Parts.Cockpit
            );
        }

        private static ExternalPart CreateCannon()
        {
            return new ExternalPart
            (
                $"{typeof(KeplerParts).FullName}.Cannon",
                new Part()
                {
                    active = true,
                    damageModifier = PDamMod.none,
                    type = PType.cannon,
                },
                KeplerSprites.Parts.Cannon
            );
        }
    }
}