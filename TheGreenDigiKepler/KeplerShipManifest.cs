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
                    hull = 7,
                    hullMax = 7,
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
                new ExternalCard[] { BasicMineCard ?? throw new Exception(), BasicDroneCard ?? throw new Exception()},
                new ExternalArtifact[] { SalvagerSystem ?? throw new Exception() },
                new Type[] { typeof(DodgeColorless), typeof(DroneshiftColorless)} ,
                new Type[] { typeof(ShieldPrep) },
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
                ExternalSprite.GetRaw((int)Spr.parts_missiles_ancient),
                ExternalSprite.GetRaw((int)Spr.parts_missiles_gemini_off)
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
        private ExternalArtifact? SalvagerSystemTwo { get; set; }
        private ExternalArtifact? SalvageNet { get; set; }

        public void LoadManifest(IArtifactRegistry registry)
        {
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
                    "Salvager Systems",
                    "If a missile were to hit your inactive missile bay, it is destroyed instead. " +
                    "At the start of your turn, if you don’t have a <c=card>Salvager System</c> in your hand, gain one."
                );

                registry.RegisterArtifact(SalvagerSystem);
            }
            {
                var spr = ExternalSprite.GetRaw((int)Spr.artifacts_AresCannonV2);
                SalvagerSystemTwo = new ExternalArtifact
                (
                    "TheGreenDigi.Kepler.Artifact.SalvagerSystemV2",
                    typeof(SalvagerSystemsTwo),
                    spr,
                    new ExternalGlossary[0],
                    null,
                    null
                );

                SalvagerSystemTwo.AddLocalisation
                (
                    "Salvager Systems V2",
                    "Replaces <c=artifact>Salvager Systems</c>.\nIf a missile were to hit your inactive missile bay, it is destroyed instead. " +
                    "At the start of your turn, if you don’t have a <c=card>Swarm Mode</c> in your hand, gain one."
                );

                registry.RegisterArtifact(SalvagerSystemTwo);
            }
            {
                var spr = ExternalSprite.GetRaw((int)Spr.artifacts_MultiThreading_off);
                SalvageNet = new ExternalArtifact
                (
                    "TheGreenDigi.Kepler.Artifact.SalvageNet",
                    typeof(SalvageNet),
                    spr,
                    new ExternalGlossary[0],
                    null,
                    null
                );

                SalvageNet.AddLocalisation
                (
                    "Salvage Net",
                    "If a missile were to hit your inactive missile bay, it is instead removed without destroying it, turned around, and added to your hand in a <c=card>Relaunch</c>."
                );

                registry.RegisterArtifact(SalvageNet);
            }
        }
    }

    public partial class KeplerShipManifest : ICardManifest
    {
        private ExternalCard? BasicMineCard { get; set; }
        private ExternalCard? BasicDroneCard { get; set; }

        public void LoadManifest(ICardRegistry registry)
        {
            {
                var spr = ExternalSprite.GetRaw((int)Spr.cards_Deflection);
                ExternalCard salvagerSystemCard = new ExternalCard("TheGreenDigi.Kepler.Card.SalvagerSystem", typeof(SalvagerSystemCard), spr, null);
                salvagerSystemCard.AddLocalisation("Salvager System");
                registry.RegisterCard(salvagerSystemCard);
            }
            {
                var swarmmode_spr = ExternalSprite.GetRaw((int)Spr.cards_Deflection);
                ExternalCard swarmmodeCard = new ExternalCard("TheGreenDigi.Kepler.Card.SwarmMode", typeof(SwarmModeCard), swarmmode_spr, null);
                swarmmodeCard.AddLocalisation("Swarm Mode");
                registry.RegisterCard(swarmmodeCard);
            }
            {
                var relaunch_spr = ExternalSprite.GetRaw((int)Spr.cards_SeekerMissileCard);
                ExternalCard relaunchCard = new ExternalCard("TheGreenDigi.Kepler.Card.Relaunch", typeof(RelaunchCard), relaunch_spr, null);
                relaunchCard.AddLocalisation("Relaunch");
                registry.RegisterCard(relaunchCard);
            }
            {
                var basicmine_spr = ExternalSprite.GetRaw((int)Spr.cards_colorless);
                ExternalCard basicMineCard = new ExternalCard("TheGreenDigi.Kepler.Card.BasicMine", typeof(BasicMineCard), basicmine_spr, null);
                basicMineCard.AddLocalisation("Basic Mine");
                registry.RegisterCard(basicMineCard);
                BasicMineCard = basicMineCard;
            }
            {
                var basicdrone_spr = ExternalSprite.GetRaw((int)Spr.cards_colorless);
                ExternalCard basicDroneCard = new ExternalCard("TheGreenDigi.Kepler.Card.BasicDrone", typeof(BasicDroneCard), basicdrone_spr, null);
                basicDroneCard.AddLocalisation("Basic Drone");
                registry.RegisterCard(basicDroneCard);
                BasicDroneCard = basicDroneCard;
            }
        }
    }
}
