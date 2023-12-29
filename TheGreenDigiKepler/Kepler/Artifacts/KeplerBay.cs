using TheGreenDigiKepler.Kepler.Actions;
using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler.Kepler.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] {ArtifactPool.EventOnly}, unremovable = true)]
    public class KeplerBay : Artifact
    {
        public static string DisplayName => "Kepler Bay".ToUpper();
        public override void OnTurnStart(State state, Combat combat)
        {
            bool containsSalvagerCard = combat.hand.OfType<ToggleBayCard>().Any();
            if (containsSalvagerCard)
            {
                return;
            }

            combat.Queue(new KeplerSalvagerSystemDelayedCardAction());
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>()
        {
            new TTCard()
            {
                card = (Card) new ToggleBayCard()
            }
        };
    }
}
