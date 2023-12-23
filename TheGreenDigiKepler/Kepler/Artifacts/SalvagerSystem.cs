using TheGreenDigiKepler.Kepler.Actions;
using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler.Kepler.Artifacts
{
    public class SalvagerSystem : Artifact
    {
        public bool salvaging = true;

        public override void OnReceiveArtifact(State state)
        {
            bool toggle = false;
            foreach (Part shipPart in state.ship.parts)
            {
                if (shipPart is { type: PType.missiles })
                {
                    shipPart.active = toggle;
                    toggle = !toggle;
                }
            }
        }

        public override void OnTurnStart(State state, Combat combat)
        {
            bool containsSalvagerCard = combat.hand.OfType<SalvagerSystemCard>().Any();
            if (containsSalvagerCard)
            {
                return;
            }

            combat.Queue(new KeplerSalvagerSystemDelayedCardAction());
        }

        public override Spr GetSprite()
        {
            return Spr.artifacts_AresCannon;
        }

        public override int ModifyBaseMissileDamage(State state, Combat? combat, bool targetPlayer)
        {
            if (!targetPlayer || combat?.currentCardAction is not AMissileHit missileHit)
            {
                return base.ModifyBaseMissileDamage(state, combat, targetPlayer);
            }

            Part? shipPart = state.ship.GetPartAtWorldX(missileHit.worldX);
            if (shipPart is { type: PType.missiles, active: false })
            {
                return -int.MaxValue;
            }
            
            return base.ModifyBaseMissileDamage(state, combat, targetPlayer);
        }
    }
}
