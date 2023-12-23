namespace TheGreenDigiKepler.Kepler.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] {ArtifactPool.Boss}, unremovable = true)]
    public class SalvagerSystemsTwo : Artifact
    {
        public override string Name() => "Salvager Systems V2";
        public override string Description() => "Replaces <c=artifact>Salvager Systems</c>.\nIf a missile were to hit your inactive missile bay, it is destroyed instead. " +
                                                "At the start of your turn, if you don’t have a <c=card>Swarm Mode</c> in your hand, gain one.";

        public override void OnReceiveArtifact(State s)
        {
            string artifactType = "SalvagerSystem";
            foreach (Artifact artifact in s.artifacts)
            {
                if (artifact.Key() == artifactType)
                    artifact.OnRemoveArtifact(s);
            }
            s.artifacts.RemoveAll((Predicate<Artifact>) (r => r.Key() == artifactType));

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
           /* if (state.ship.GetPartTypeCount(PType.missiles) > 1)
            {
                List<Part> parts = state.ship.parts;
                for (int index = 0; index < parts.Count; ++index)
                {
                    if (parts[index].type == PType.missiles && parts[index].flip == this.nextCannonShouldBeTheFlippedOne)
                        parts[index].active = false;
                }
                this.nextCannonShouldBeTheFlippedOne = !this.nextCannonShouldBeTheFlippedOne;
            }
            bool flag = false;
            foreach (Card card in combat.hand)
            {
                if (card is AresSuperCannon)
                    flag = true;
            }
            if (flag)
                return;
            combat.Queue((CardAction) new AAresCannon2CardDelay());*/
        }

      /*  public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>()
        {
            new TTCard()
            {
                card = (Card) new AresSuperCannon()
            }
        };*/
    }
}