namespace TheGreenDigiKepler.Kepler.Cards
{
    [CardMeta(rarity = Rarity.common, dontOffer = true)]
    public class RelaunchCard : Card
    {
        public StuffBase missileToLaunch = new FakeDrone();

        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> cardActionList = new List<CardAction>();
            ASpawn aspawn = new ASpawn
            {
                thing = missileToLaunch
            };
            
            cardActionList.Add(aspawn);

            return cardActionList;
        }

        public override CardData GetData(State state) => new CardData
        {
            cost = 1,
            temporary = true,
            singleUse = true,
        };
    }
}