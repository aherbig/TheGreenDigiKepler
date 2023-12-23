using TheGreenDigiKepler.Kepler.Actions;

namespace TheGreenDigiKepler.Kepler.Cards
{
    [CardMeta(deck = Deck.ares, dontOffer = true, rarity = Rarity.common)]
    public class SalvagerSystemCard : Card
    {
        public override string Name() => "Salvager System Card";

        private static Spr card_sprite = Spr.cards_Deflection;

        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new KeplerSalvagarSystemBaySwitchAction());
                    break;
            }

            return list;
        }

        public override CardData GetData(State state) => new CardData
        {
            description = "Toggle which missile bay is active.",
            cost = 0,
            art = card_sprite,
            retain = true,
            temporary = true,
            singleUse = true,
        };
    }
    
}