using TheGreenDigiKepler.Kepler.Actions;

namespace TheGreenDigiKepler.Kepler.Cards
{
    [CardMeta(dontOffer = true, rarity = Rarity.common)]
    public class ToggleBayCard : Card
    {
        public override string Name() => "Toggle Bay Card";

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
            retain = true,
            temporary = true,
            singleUse = true,
        };
    }
    
}