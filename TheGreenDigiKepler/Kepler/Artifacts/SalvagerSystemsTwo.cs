using TheGreenDigiKepler.Kepler.Actions;
using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler.Kepler.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] {ArtifactPool.Boss}, unremovable = true)]
    public class SalvagerSystemsTwo : Artifact
    {
        public override string Name() => "Salvager Systems V2";
        public override string Description() => "Replaces <c=artifact>Salvager Systems</c>.\nIf a missile were to hit your inactive missile bay, it is destroyed instead. " +
                                                "At the start of your turn, if you don’t have a <c=card>Swarm Mode</c> in your hand, gain one.";

        private bool activeBayShouldBeTheFirstOne;

        public override void OnReceiveArtifact(State s)
        {
            string artifactType = "SalvagerSystem";
            foreach (Artifact artifact in s.artifacts)
            {
                if (artifact.Key() == artifactType)
                    artifact.OnRemoveArtifact(s);
            }
            s.artifacts.RemoveAll((Predicate<Artifact>) (r => r.Key() == artifactType));

            foreach (Part shipPart in s.ship.parts)
            {
                if (shipPart is { type: PType.missiles })
                {
                    shipPart.active = true;
                }
            }
        }

        public override void OnTurnStart(State state, Combat combat)
        {
            if (state.ship.GetPartTypeCount(PType.missiles) > 1)
            {
                bool isFirstBay = true;
                List<Part> parts = state.ship.parts;
                for (int index = 0; index < parts.Count; ++index)
                {
                    if (parts[index].type == PType.missiles)
                    {
                        if (isFirstBay)
                        {
                            parts[index].active = activeBayShouldBeTheFirstOne;
                        }
                        else
                        {
                            parts[index].active = !activeBayShouldBeTheFirstOne;
                        }
                        isFirstBay = false;
                    }
                }
                activeBayShouldBeTheFirstOne = !activeBayShouldBeTheFirstOne;
            }
            bool flag = false;
            foreach (Card card in combat.hand)
            {
                if (card is SwarmModeCard)
                    flag = true;
            }
            if (flag)
                return;
            combat.Queue(new KeplerSwarmModeDelayedCardAction());
        }

        public override int ModifyBaseMissileDamage(State state, Combat? combat, bool targetPlayer)
        {
            if (!targetPlayer || combat?.currentCardAction is not AMissileHit missileHit)
            {
                return 0;
            }

            combat.stuff.TryGetValue(missileHit.worldX, out var stuffBase);
            if (stuffBase is not Missile missile)
            {
                return 0;
            }

            int attackLocation = missile.missileType switch
            {
                MissileType.seeker => missile.GetSeekerImpact(state, combat),
                _ => missileHit.worldX
            };

            Part? shipPart = state.ship.GetPartAtWorldX(attackLocation);
            if (shipPart is not { type: PType.missiles, active: false })
            {
                return 0;
            }

            missileHit.weaken = false;
            missileHit.status = null;
            return -int.MaxValue;
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>()
        {
            new TTCard()
            {
                card = new SwarmModeCard()
            }
        };
    }
}