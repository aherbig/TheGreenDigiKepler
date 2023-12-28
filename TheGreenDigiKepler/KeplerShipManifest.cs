using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Microsoft.Extensions.Logging;
using TheGreenDigiKepler.Kepler;
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
            Kepler = new ExternalShip($"{typeof(KeplerShipManifest).Namespace}.Ship",
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
                new [] { MissileBay, Cockpit, Cannon, MissileBay },
                KeplerSprites.Parts.Chassis,
                null
            );
            shipRegistry.RegisterShip(Kepler);
        }

        public void LoadManifest(IStartershipRegistry registry)
        {
            if (Kepler == null)
                return;
            var starter = new ExternalStarterShip
            ($"{typeof(KeplerShipManifest).Namespace}.Ship.Starter",
                Kepler.GlobalName,
                new [] { BasicMineCard ?? throw new Exception(), BasicDroneCard ?? throw new Exception()},
                new []
                {
                    KeplerBayArtifact ?? throw new Exception(),
                    MissilePDS ?? throw new Exception(),
                },
                new [] { typeof(DodgeColorless), typeof(DroneshiftColorless)} ,
                new [] { typeof(ShieldPrep) },
                exclusiveArtifacts: new []
                {
                    KeplerBayArtifact ?? throw new Exception(),
                    KeplerBayV2Artifact ?? throw new Exception(),
                    MissileTractorBeam ?? throw new Exception(),
                    MissilePDS ?? throw new Exception(),
                }
            );

            starter.AddLocalisation("Kepler", "An old war ship with two missile bays and a missile defense system.");

            registry.RegisterStartership(starter);
        }

        private ExternalPart CreateMissileBay()
        {
            return new ExternalPart
            (
                $"{typeof(KeplerShipManifest).Namespace}.Ship.Parts.MissileBay",
                new Part()
                {
                    active = false,
                    damageModifier = PDamMod.none,
                    type = PType.missiles,
                },
                KeplerSprites.Parts.BayActive,
                KeplerSprites.Parts.BayInactive
            );
        }

        private ExternalPart CreateCannon()
        {
            return new ExternalPart
            (
                $"{typeof(KeplerShipManifest).Namespace}.Ship.Parts.Cannon",
                new Part()
                {
                    active = true,
                    damageModifier = PDamMod.none,
                    type = PType.cannon,
                },
                KeplerSprites.Parts.Cannon
            );
        }

        private ExternalPart CreateCockpit()
        {
            return new ExternalPart
            (
                $"{typeof(KeplerShipManifest).Namespace}.Ship.Parts.Cockpit",
                new Part()
                {
                    active = true,
                    damageModifier = PDamMod.none,
                    type = PType.cockpit,
                },
                KeplerSprites.Parts.Cockpit
            );
        }
    }

    public partial class KeplerShipManifest : ISpriteManifest
    {
        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            KeplerSprites.ModRootFolder = ModRootFolder;
            ExternalSprite[] spritesToRegister = {
                KeplerSprites.Artifacts.Bay,
                KeplerSprites.Artifacts.BayV2,
                KeplerSprites.Artifacts.MissileTractorBeam,
                KeplerSprites.Artifacts.MissilePDS,
                KeplerSprites.Parts.Chassis,
                KeplerSprites.Parts.Cannon,
                KeplerSprites.Parts.Cockpit,
                KeplerSprites.Parts.BayActive,
                KeplerSprites.Parts.BayInactive,
                KeplerSprites.Cards.BasicDrone,
                KeplerSprites.Cards.KeplerCardArt,
                KeplerSprites.Cards.KeplerCardFrame,
            };

            if (spritesToRegister.Any(sprite => !artRegistry.RegisterArt(sprite)))
            {
                throw new Exception("Cannot register sprite.");
            }
        }
    }

    public partial class KeplerShipManifest : IArtifactManifest
    {
        private ExternalArtifact? KeplerBayArtifact { get; set; }
        private ExternalArtifact? KeplerBayV2Artifact { get; set; }
        private ExternalArtifact? MissileTractorBeam { get; set; }
        private ExternalArtifact? MissilePDS { get; set; }

        public void LoadManifest(IArtifactRegistry registry)
        {
            {
                Type artifactType = typeof(KeplerBay);
                KeplerBayArtifact = new ExternalArtifact
                (
                    artifactType.FullName ?? throw new Exception(),
                    artifactType,
                    KeplerSprites.Artifacts.Bay,
                    new ExternalGlossary[0],
                    null,
                    null
                );

                KeplerBayArtifact.AddLocalisation
                (
                    "Kepler Bay",
                    "At the start of your turn, if you don’t have a <c=card>Toggle Bay</c> in your hand, gain one."
                );

                registry.RegisterArtifact(KeplerBayArtifact);
            }
            {
                Type artifactType = typeof(KeplerBayV2);
                KeplerBayV2Artifact = new ExternalArtifact
                (
                    artifactType.FullName ?? throw new Exception(),
                    artifactType,
                    KeplerSprites.Artifacts.BayV2,
                    new ExternalGlossary[0],
                    null,
                    null
                );

                KeplerBayV2Artifact.AddLocalisation
                (
                    "Kepler Bay V2",
                    "Replaces <c=artifact>Kepler Bay</c>.\nIf a missile were to hit your inactive missile bay, it is destroyed instead. " +
                    "At the start of your turn, if you don’t have a <c=card>Swarm Mode</c> in your hand, gain one."
                );

                registry.RegisterArtifact(KeplerBayV2Artifact);
            }
            {
                Type artifactType = typeof(MissileTractorBeam);
                MissileTractorBeam = new ExternalArtifact
                (
                    artifactType.FullName ?? throw new Exception(),
                    artifactType,
                    KeplerSprites.Artifacts.MissileTractorBeam,
                    new ExternalGlossary[0],
                    null,
                    null
                );

                MissileTractorBeam.AddLocalisation
                (
                    "Missile Tractor Beam",
                    "Replaces <c=artifact>Missile P.D.S.</c>.\nIf a missile were to hit your inactive missile bay, it is instead removed without destroying it, turned around, and added to your hand in a <c=card>Relaunch</c>."
                );

                registry.RegisterArtifact(MissileTractorBeam);
            }
            {
                Type artifactType = typeof(MissilePDS);
                MissilePDS = new ExternalArtifact
                (
                    artifactType.FullName ?? throw new Exception(),
                    artifactType,
                    KeplerSprites.Artifacts.MissilePDS,
                    new ExternalGlossary[0],
                    null,
                    null
                );

                MissilePDS.AddLocalisation
                (
                    "Missile P.D.S.",
                    "If a missile were to hit your inactive missile bay, it is destroyed instead."
                );

                registry.RegisterArtifact(MissilePDS);
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
                ExternalCard salvagerSystemCard = new ExternalCard($"{typeof(KeplerShipManifest).Namespace}.Card.ToggleBay", typeof(ToggleBayCard), spr, null);
                salvagerSystemCard.AddLocalisation("Toggle Bay");
                registry.RegisterCard(salvagerSystemCard);
            }
            {
                var swarmmode_spr = ExternalSprite.GetRaw((int)Spr.cards_Deflection);
                ExternalCard swarmmodeCard = new ExternalCard($"{typeof(KeplerShipManifest).Namespace}.Card.SwarmMode", typeof(SwarmModeCard), swarmmode_spr, null);
                swarmmodeCard.AddLocalisation("Swarm Mode");
                registry.RegisterCard(swarmmodeCard);
            }
            {
                var relaunch_spr = ExternalSprite.GetRaw((int)Spr.cards_SeekerMissileCard);
                ExternalCard relaunchCard = new ExternalCard($"{typeof(KeplerShipManifest).Namespace}.Card.Relaunch", typeof(RelaunchCard), relaunch_spr, null);
                relaunchCard.AddLocalisation("Relaunch");
                registry.RegisterCard(relaunchCard);
            }
            {
                var basicmine_spr = ExternalSprite.GetRaw((int)Spr.cards_colorless);
                ExternalCard basicMineCard = new ExternalCard($"{typeof(KeplerShipManifest).Namespace}.Card.BasicMine", typeof(BasicMineCard), basicmine_spr, null);
                basicMineCard.AddLocalisation("Basic Mine");
                registry.RegisterCard(basicMineCard);
                BasicMineCard = basicMineCard;
            }
            {
                var basicdrone_spr = ExternalSprite.GetRaw((int)Spr.cards_colorless);
                ExternalCard basicDroneCard = new ExternalCard($"{typeof(KeplerShipManifest).Namespace}.Card.BasicDrone", typeof(BasicDroneCard), basicdrone_spr, null);
                basicDroneCard.AddLocalisation("Basic Drone");
                registry.RegisterCard(basicDroneCard);
                BasicDroneCard = basicDroneCard;
            }
        }
    }
}
