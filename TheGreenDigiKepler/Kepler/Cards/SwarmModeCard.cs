namespace TheGreenDigiKepler.Kepler.Cards
{
    [CardMeta(deck = Deck.ares, dontOffer = true, rarity = Rarity.common)]
    public class SwarmModeCard : Card
    {
        public override string Name() => "Swarm Mode";

        public override CardData GetData(State state) => new CardData()
        {
            cost = 1,
            retain = true,
            singleUse = true,
            temporary = true,
            flippable = true,
            description = "Both missile bays are active until next turn. <c=keyword>Flip to toggle.</c>",
            art = new Spr?(Spr.cards_Terminal)
        };

        public override List<CardAction> GetActions(State s, Combat c) => new List<CardAction>()
        {
            new AActivateAllParts()
            {
                partType = PType.missiles
            }
        };

        public override void OnFlip(G g)
        {
            if (!(g.state.route is Combat route))
                return;
            route.QueueImmediate(new ATogglePart()
            {
                partType = PType.missiles
            });
        }
    }
}