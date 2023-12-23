namespace TheGreenDigiKepler.Kepler.Actions
{
    public class KeplerSalvagarSystemBaySwitchAction : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            foreach (Part shipPart in s.ship.parts)
            {
                if (shipPart is { type: PType.missiles })
                {
                    shipPart.active = !shipPart.active;
                }
            }
        }
    }
}