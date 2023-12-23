using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler.Kepler.Actions
{
    public class KeplerSwarmModeDelayedCardAction : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            this.timer = 0.0;
            bool flag = false;
            foreach (Card card in c.hand)
            {
                if (card is SwarmModeCard)
                    flag = true;
            }

            if (flag)
                return;
            c.QueueImmediate(new AAddCard()
            {
                card = new SwarmModeCard(),
                destination = CardDestination.Hand
            });
        }
    }
}