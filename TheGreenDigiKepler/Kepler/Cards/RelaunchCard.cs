namespace TheGreenDigiKepler.Kepler.Cards
{
    [CardMeta(deck = Deck.ares, rarity = Rarity.common, dontOffer = true)]
    public class RelaunchCard : Card
    {
        private static Spr card_sprite = Spr.cards_SeekerMissileCard;

        public int outgoingDamage;
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