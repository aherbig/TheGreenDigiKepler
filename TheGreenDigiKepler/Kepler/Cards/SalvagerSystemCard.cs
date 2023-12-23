using TheGreenDigiKepler.Kepler.Actions;

namespace TheGreenDigiKepler.Kepler.Cards
{
    [CardMeta(deck = Deck.ares, rarity = Rarity.common)]
    public class SalvagerSystemCard : Card
    {
        private static Spr card_sprite = Spr.cards_GoatDrone;

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
            cost = 0,
            art = card_sprite,
            retain = true,
            temporary = true,
            singleUse = true,
        };

        public override string Name() => "KeplerDemoCard";
    }
    
}