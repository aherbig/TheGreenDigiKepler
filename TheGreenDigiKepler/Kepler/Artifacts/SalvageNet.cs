using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler.Kepler.Artifacts
{
    public class SalvageNet : Artifact
    {
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

            combat.QueueImmediate(new AAddCard()
            {
                card = new RelaunchCard { missileType = missile.missileType },
                destination = CardDestination.Hand
            });
            return 0;
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
