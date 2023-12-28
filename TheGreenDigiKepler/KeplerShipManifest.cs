﻿using CobaltCoreModding.Definitions;
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
                new [] { MissileBay, Cockpit, Cannon, MissileBay },
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
                new [] { BasicMineCard ?? throw new Exception(), BasicDroneCard ?? throw new Exception()},
                new [] { SalvagerSystemArtifact ?? throw new Exception() },
                new [] { typeof(DodgeColorless), typeof(DroneshiftColorless)} ,
                new [] { typeof(ShieldPrep) },
                exclusiveArtifacts: new [] { SalvagerSystemArtifact ?? throw new Exception(), SalvagerSystemTwoArtifact ?? throw new Exception(), SalvageNetArtifact ?? throw new Exception() }
            );

            starter.AddLocalisation("Kepler", "A missile war ship with two bays that can be manually toggled.");

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
                KeplerSprites.Parts.BayActive,
                KeplerSprites.Parts.BayInactive
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
                KeplerSprites.Parts.Cannon
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
                KeplerSprites.Artifacts.KeplerCannon,
                KeplerSprites.Artifacts.KeplerCannon2,
                KeplerSprites.Artifacts.SalvageNet,
                KeplerSprites.Parts.Cannon,
                KeplerSprites.Parts.Cockpit,
                KeplerSprites.Parts.BayActive,
                KeplerSprites.Parts.BayInactive,
            };

            if (spritesToRegister.Any(sprite => !artRegistry.RegisterArt(sprite)))
            {
                throw new Exception("Cannot register sprite.");
            }
        }
    }

    public partial class KeplerShipManifest : IArtifactManifest
    {
        private ExternalArtifact? SalvagerSystemArtifact { get; set; }
        private ExternalArtifact? SalvagerSystemTwoArtifact { get; set; }
        private ExternalArtifact? SalvageNetArtifact { get; set; }

        public void LoadManifest(IArtifactRegistry registry)
        {
            {
                SalvagerSystemArtifact = new ExternalArtifact
                (
                    "TheGreenDigi.Kepler.Artifact.SalvagerSystem",
                    typeof(SalvagerSystem),
                    KeplerSprites.Artifacts.KeplerCannon,
                    new ExternalGlossary[0],
                    null,
                    null
                );

                SalvagerSystemArtifact.AddLocalisation
                (
                    "Salvager Systems",
                    "If a missile were to hit your inactive missile bay, it is destroyed instead. " +
                    "At the start of your turn, if you don’t have a <c=card>Salvager System</c> in your hand, gain one."
                );

                registry.RegisterArtifact(SalvagerSystemArtifact);
            }
            {
                SalvagerSystemTwoArtifact = new ExternalArtifact
                (
                    "TheGreenDigi.Kepler.Artifact.SalvagerSystemV2",
                    typeof(SalvagerSystemsTwo),
                    KeplerSprites.Artifacts.KeplerCannon2,
                    new ExternalGlossary[0],
                    null,
                    null
                );

                SalvagerSystemTwoArtifact.AddLocalisation
                (
                    "Salvager Systems V2",
                    "Replaces <c=artifact>Salvager Systems</c>.\nIf a missile were to hit your inactive missile bay, it is destroyed instead. " +
                    "At the start of your turn, if you don’t have a <c=card>Swarm Mode</c> in your hand, gain one."
                );

                registry.RegisterArtifact(SalvagerSystemTwoArtifact);
            }
            {
                SalvageNetArtifact = new ExternalArtifact
                (
                    "TheGreenDigi.Kepler.Artifact.SalvageNet",
                    typeof(SalvageNet),
                    KeplerSprites.Artifacts.SalvageNet,
                    new ExternalGlossary[0],
                    null,
                    null
                );

                SalvageNetArtifact.AddLocalisation
                (
                    "Salvage Net",
                    "If a missile were to hit your inactive missile bay, it is instead removed without destroying it, turned around, and added to your hand in a <c=card>Relaunch</c>."
                );

                registry.RegisterArtifact(SalvageNetArtifact);
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
