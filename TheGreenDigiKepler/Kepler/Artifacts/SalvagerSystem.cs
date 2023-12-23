using TheGreenDigiKepler.Kepler.Actions;
using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler.Kepler.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] {ArtifactPool.EventOnly}, unremovable = true)]
    public class SalvagerSystem : Artifact
    {
        public override string Name() => "Salvager Systems";
        public override string Description() => "If a missile were to hit your inactive missile bay, it is destroyed instead. " +
                                                "At the start of your turn, if you don’t have a <c=card>Salvager System</c> in your hand, gain one.";

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
                card = (Card) new SalvagerSystemCard()
            }
        };
    }
}
