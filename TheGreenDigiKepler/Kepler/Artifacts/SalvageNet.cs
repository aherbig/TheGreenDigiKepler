using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler.Kepler.Artifacts
{
    public class SalvageNet : Artifact
    {
        public override Spr GetSprite()
        {
            return Spr.artifacts_MultiThreading_off;
        }
        
        public override int ModifyBaseMissileDamage(State state, Combat? combat, bool targetPlayer)
        {
            if (!targetPlayer || combat?.currentCardAction is not AMissileHit missileHit)
            {
                return 0;
            }

            Part? shipPart = state.ship.GetPartAtWorldX(missileHit.worldX);
            if (shipPart is not { type: PType.missiles, active: false })
            {
                return 0;
            }

            combat.QueueImmediate(new AAddCard()
            {
                card = new RelaunchCard { outgoingDamage = missileHit.outgoingDamage },
                destination = CardDestination.Hand
            });
            return 0;
        }
    }
}
