namespace TheGreenDigiKepler.Kepler.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new [] { Upgrade.A, Upgrade.B })]
    public class BasicDroneCard : Card
    {
        public override string Name() => "Basic Drone";

        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new ASpawn() { thing = new AttackDrone() });
                    break;

                case Upgrade.A:
                    list.Add(new ASpawn() { thing = new AttackDrone { upgraded = true } });
                    break;

                case Upgrade.B:
                    list.Add(new ASpawn { thing = new AttackDrone(), disabled = flipped });
                    list.Add(new ADummyAction());
                    list.Add(new ASpawn { thing = new ShieldDrone(), disabled = !flipped });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state) => new CardData
        {
            cost = 1,
            art = new Spr?(upgrade switch
            {
                Upgrade.B => flipped ? Spr.cards_Adaptability_Bottom : Spr.cards_Adaptability_Top,
                _ => Spr.cards_colorless
            }),
            artTint = "ffffff",
            floppable = upgrade == Upgrade.B,
        };
    }
}