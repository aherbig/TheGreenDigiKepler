namespace TheGreenDigiKepler.Kepler.Cards
{
    [CardMeta(deck = Deck.ares, rarity = Rarity.common, dontOffer = true)]
    public class RelaunchCard : Card
    {
        private static Spr card_sprite = Spr.cards_SeekerMissileCard;

        public MissileType missileType;

        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> cardActionList = new List<CardAction>();
            ASpawn aspawn = new ASpawn();
            Missile missile = new Missile();
            missile.yAnimation = 0.0;
            missile.missileType = missileType;
            aspawn.thing = missile;
            cardActionList.Add(aspawn);

            return cardActionList;
        }

        public override CardData GetData(State state) => new CardData
        {
            description = "Launch a missile back.",
            cost = 1,
            art = card_sprite,
            temporary = true,
            singleUse = true,
        };
    }
}