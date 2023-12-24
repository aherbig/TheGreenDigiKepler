namespace TheGreenDigiKepler.Kepler.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new [] { Upgrade.A, Upgrade.B })]
    public class BasicMineCard : Card
    {
        public override string Name() => "Basic Mine";

        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new ASpawn() { thing = new SpaceMine() });
                    break;

                case Upgrade.A:
                    list.Add(new ASpawn() { thing = new SpaceMine() });
                    break;

                case Upgrade.B:
                    list.Add(new ASpawn { thing = new SpaceMine() { bubbleShield = true} });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state) => new CardData
        {
            cost = upgrade switch
            {
                Upgrade.A => 1,
                _ => 2
            },
            art = new Spr?(Spr.cards_colorless),
            artTint = "ffffff",
        };
    }
}