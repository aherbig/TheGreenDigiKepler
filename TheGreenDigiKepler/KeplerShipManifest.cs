using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Microsoft.Extensions.Logging;
using TheGreenDigiKepler.Kepler.Artifacts;
using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler
{
    public partial class KeplerShipManifest : IShipManifest, IShipPartManifest, IStartershipManifest
    {
        public IEnumerable<DependencyEntry> Dependencies => new DependencyEntry[0];
        public DirectoryInfo? GameRootFolder { get; set; }
        public ILogger? Logger { get; set; }
        public DirectoryInfo? ModRootFolder { get; set; }
        public string Name => "KeplerShipManifest";

        private ExternalShip? Kepler { get; set; }

        private ExternalPart? _missileBay;

        private ExternalPart MissileBay
        {
            get { return _missileBay ??= CreateMissileBay(); }
        }

        private ExternalPart? _cockpit;

        private ExternalPart Cockpit
        {
            get { return _cockpit ??= CreateCockpit(); }
        }

        private ExternalPart? _cannon;

        private ExternalPart Cannon
        {
            get { return _cannon ??= CreateCannon(); }
        }

        public void LoadManifest(IShipPartRegistry registry)
        {
            registry.RegisterPart(MissileBay);
            registry.RegisterPart(Cockpit);
            registry.RegisterPart(Cannon);
        }

        public void LoadManifest(IShipRegistry shipRegistry)
        {
            Kepler = new ExternalShip("TheGreenDigi.Kepler.Ship",
                new Ship()
                {
                    baseDraw = 5,
                    baseEnergy = 3,
                    heatTrigger = 3,
                    heatMin = 0,
                    hull = 10,
                    hullMax = 10,
                    shieldMaxBase = 3
                },
                new ExternalPart[] { MissileBay, Cockpit, Cannon, MissileBay },
                ExternalSprite.GetRaw((int)Spr.parts_chassis_boxy),
                null
            );
            shipRegistry.RegisterShip(Kepler);
        }

        public void LoadManifest(IStartershipRegistry registry)
        {
            if (Kepler == null)
                return;
            var starter = new ExternalStarterShip
            ("TheGreenDigi.Kepler.Ship.Starter",
                Kepler.GlobalName,
                new ExternalCard[0],
                new ExternalArtifact[] { SalvagerSystem ?? throw new Exception() },
                new Type[0],
                new Type[0],
                exclusiveArtifacts: new ExternalArtifact[] { SalvagerSystem ?? throw new Exception() }
            );

            starter.AddLocalisation("Kepler", "Eat missiles and have fun.");

            registry.RegisterStartership(starter);
        }

        private ExternalPart CreateMissileBay()
        {
            return new ExternalPart
            (
                "TheGreenDigi.Kepler.Ship.MissileBay",
                new Part()
                {
                    active = false,
                    damageModifier = PDamMod.none,
                    type = PType.missiles,
                },
                ExternalSprite.GetRaw((int)Spr.parts_missiles_conveyor)
            );
        }

        private ExternalPart CreateCannon()
        {
            return new ExternalPart
            (
                "TheGreenDigi.Kepler.Ship.Cannon",
                new Part()
                {
                    active = true,
                    damageModifier = PDamMod.none,
                    type = PType.cannon,
                },
                ExternalSprite.GetRaw((int)Spr.parts_cannon_conveyor)
            );
        }

        private ExternalPart CreateCockpit()
        {
            return new ExternalPart
            (
                "TheGreenDigi.Kepler.Ship.Cockpit",
                new Part()
                {
                    active = true,
                    damageModifier = PDamMod.none,
                    type = PType.cockpit,
                },
                ExternalSprite.GetRaw((int)Spr.parts_cockpit_conveyor)
            );
        }

    }

    public partial class KeplerShipManifest : IArtifactManifest
    {
        private ExternalArtifact? SalvagerSystem { get; set; }

        public void LoadManifest(IArtifactRegistry registry)
        {
            var spr = ExternalSprite.GetRaw((int)Spr.artifacts_AresCannon);
            SalvagerSystem = new ExternalArtifact
            (
                "TheGreenDigi.Kepler.Artifact.SalvagerSystem",
                typeof(SalvagerSystem),
                spr,
                new ExternalGlossary[0],
                null,
                null
            );

            SalvagerSystem.AddLocalisation
            (
                "Kepler Salvager Systems",
                "If a missile were to hit your inactive missile bay, it is destroyed instead. " +
                "At the start of your turn, if you don’t have a Salvager System in your hand, gain one."
            );

            registry.RegisterArtifact(SalvagerSystem);
        }
    }

    public partial class KeplerShipManifest : ICardManifest
    {
        public void LoadManifest(ICardRegistry registry)
        {
            var spr = ExternalSprite.GetRaw((int)Spr.cards_OxygenLeak);
            if (spr == null)
                return;
            ExternalCard card = new ExternalCard("TheGreenDigi.Kepler.Card.SalvagerSystem", typeof(SalvagerSystemCard), spr, null);
            card.AddLocalisation("Salvager System");
            registry.RegisterCard(card);
        }
    }
}
