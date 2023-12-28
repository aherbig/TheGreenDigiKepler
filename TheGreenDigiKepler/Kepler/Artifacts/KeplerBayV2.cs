using TheGreenDigiKepler.Kepler.Actions;
using TheGreenDigiKepler.Kepler.Cards;

namespace TheGreenDigiKepler.Kepler.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] {ArtifactPool.Boss}, unremovable = true)]
    public class KeplerBayV2 : Artifact
    {
        private bool activeBayShouldBeTheFirstOne;

        public override void OnReceiveArtifact(State s)
        {
            foreach (Artifact artifact in s.artifacts)
            {
                if (artifact is KeplerBay)
                    artifact.OnRemoveArtifact(s);
            }
            s.artifacts.RemoveAll((Predicate<Artifact>) (r => r is KeplerBay));

            foreach (Part shipPart in s.ship.parts)
            {
                if (shipPart is { type: PType.missiles })
                {
                    shipPart.active = true;
                }
            }
        }

        public override void OnTurnStart(State state, Combat combat)
        {
            if (state.ship.GetPartTypeCount(PType.missiles) > 1)
            {
                bool isFirstBay = true;
                List<Part> parts = state.ship.parts;
                for (int index = 0; index < parts.Count; ++index)
                {
                    if (parts[index].type == PType.missiles)
                    {
                        if (isFirstBay)
                        {
                            parts[index].active = activeBayShouldBeTheFirstOne;
                        }
                        else
                        {
                            parts[index].active = !activeBayShouldBeTheFirstOne;
                        }
                        isFirstBay = false;
                    }
                }
                activeBayShouldBeTheFirstOne = !activeBayShouldBeTheFirstOne;
            }
            bool flag = false;
            foreach (Card card in combat.hand)
            {
                if (card is SwarmModeCard)
                    flag = true;
            }
            if (flag)
                return;
            combat.Queue(new KeplerSwarmModeDelayedCardAction());
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>()
        {
            new TTCard()
            {
                card = new SwarmModeCard()
            }
        };
    }
}