namespace TheGreenDigiKepler.Kepler.Actions
{
    public class KeplerSalvagarSystemBaySwitchAction : CardAction
    {
        internal static string glossary_item = "";

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

        public override Icon? GetIcon(State s) => new Icon(Spr.icons_ace, 42, Colors.attackFail);

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> tooltips = new List<Tooltip>();

            tooltips.Add(new TTGlossary(glossary_item, Array.Empty<object>()));

            return tooltips;
        }
    }
}