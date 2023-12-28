namespace TheGreenDigiKepler.Kepler.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new [] { Upgrade.A, Upgrade.B }, dontOffer = true)]
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
                    list.Add(new ASpawn() { thing = new AttackDrone() });
                    break;

                case Upgrade.B:
                    list.Add(new ASpawn() { thing = new AttackDrone { upgraded = true } });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state) => new CardData
        {
            cost = 1,
            exhaust = upgrade switch
            {
                Upgrade.A => false,
                _ => true
            },
        };
    }
}