namespace TheGreenDigiKepler.Artifacts
{
    public class SalvagerSystem : Artifact
    {
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
