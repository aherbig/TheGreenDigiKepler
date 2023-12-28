using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler.Kepler.Actions
{
    public class KeplerSalvagerSystemDelayedCardAction : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            bool flag = false;
            foreach (Card card in c.hand)
            {
                if (card is ToggleBayCard)
                    flag = true;
            }

            if (flag)
                return;
            c.QueueImmediate(new AAddCard()
            {
                card = new ToggleBayCard(),
                destination = CardDestination.Hand
            });
        }
    }
}