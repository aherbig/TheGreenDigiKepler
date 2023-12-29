using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler.Kepler.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] {ArtifactPool.Common}, unremovable = true)]
    public class MissileTractorBeam : Artifact
    {
        public override void OnReceiveArtifact(State s)
        {
            foreach (Artifact artifact in s.artifacts)
            {
                if (artifact is MissilePDS)
                    artifact.OnRemoveArtifact(s);
            }
            s.artifacts.RemoveAll((Predicate<Artifact>) (r => r is MissilePDS));
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

            Missile missileToReturn = Mutil.DeepCopy<Missile>(missile);
            missileToReturn.targetPlayer = false;
            missileToReturn.yAnimation = 0.0f;
            missileToReturn.isHitting = false;
            combat.QueueImmediate(new AAddCard()
            {
                card = new RelaunchCard { missileToLaunch = missileToReturn },
                destination = CardDestination.Hand
            });
            missileHit.weaken = false;
            missileHit.status = null;
            return -int.MaxValue;
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>()
        {
            new TTCard()
            {
                card = new RelaunchCard()
            }
        };
    }
}
