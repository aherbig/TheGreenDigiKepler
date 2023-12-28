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

        public void LoadManifest(IShipPartRegistry registry)
        {
            registry.RegisterPart(KeplerParts.LeftMissileBay);
            registry.RegisterPart(KeplerParts.Cockpit);
            registry.RegisterPart(KeplerParts.Cannon);
            registry.RegisterPart(KeplerParts.RightMissileBay);
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
                new [] { KeplerParts.LeftMissileBay, KeplerParts.Cockpit, KeplerParts.Cannon, KeplerParts.RightMissileBay },
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
                KeplerSprites.Parts.LeftBayActive,
                KeplerSprites.Parts.LeftBayInactive,
                KeplerSprites.Parts.RightBayActive,
                KeplerSprites.Parts.RightBayInactive,
                KeplerSprites.Cards.BasicDrone,
                KeplerSprites.Cards.KeplerCardArt,
                KeplerSprites.Cards.KeplerCardFrame,
                KeplerSprites.Cards.SwarmMode,
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
                ExternalCard salvagerSystemCard = new ExternalCard
                (
                    $"{typeof(KeplerShipManifest).Namespace}.Card.ToggleBay",
                    typeof(ToggleBayCard),
                    KeplerSprites.Cards.KeplerCardArt,
                    KeplerDeck
                );
                salvagerSystemCard.AddLocalisation("Toggle Bay");
                registry.RegisterCard(salvagerSystemCard);
            }
            {
                ExternalCard swarmmodeCard = new ExternalCard
                (
                    $"{typeof(KeplerShipManifest).Namespace}.Card.SwarmMode",
                    typeof(SwarmModeCard),
                    KeplerSprites.Cards.SwarmMode,
                    KeplerDeck
                );
                swarmmodeCard.AddLocalisation("Swarm Mode");
                registry.RegisterCard(swarmmodeCard);
            }
            {
                var relaunch_spr = ExternalSprite.GetRaw((int)Spr.cards_SeekerMissileCard);
                ExternalCard relaunchCard = new ExternalCard($"{typeof(KeplerShipManifest).Namespace}.Card.Relaunch", typeof(RelaunchCard), relaunch_spr, KeplerDeck);
                relaunchCard.AddLocalisation("Relaunch");
                registry.RegisterCard(relaunchCard);
            }
            {
                ExternalCard basicMineCard = new ExternalCard
                (
                    $"{typeof(KeplerShipManifest).Namespace}.Card.BasicMine",
                    typeof(BasicMineCard),
                    KeplerSprites.Cards.BasicDrone,
                    KeplerDeck
                );
                basicMineCard.AddLocalisation("Basic Mine");
                registry.RegisterCard(basicMineCard);
                BasicMineCard = basicMineCard;
            }
            {
                ExternalCard basicDroneCard = new ExternalCard
                (
                    $"{typeof(KeplerShipManifest).Namespace}.Card.BasicDrone",
                    typeof(BasicDroneCard),
                    KeplerSprites.Cards.BasicDrone,
                    KeplerDeck
                );
                basicDroneCard.AddLocalisation("Basic Drone");
                registry.RegisterCard(basicDroneCard);
                BasicDroneCard = basicDroneCard;
            }
        }
    }
    
    public partial class KeplerShipManifest : IDeckManifest
    {
        private ExternalDeck? KeplerDeck { get; set; }

        public void LoadManifest(IDeckRegistry registry)
        {
            KeplerDeck = new ExternalDeck
            (
                $"{typeof(KeplerShipManifest).Namespace}.Decks.Kepler",
                System.Drawing.Color.White,
                System.Drawing.Color.Black,
                KeplerSprites.Cards.KeplerCardArt,
                KeplerSprites.Cards.KeplerCardFrame,
                null
            );
            registry.RegisterDeck(KeplerDeck);
        }
    }
}
