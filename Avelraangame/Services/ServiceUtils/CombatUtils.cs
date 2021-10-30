namespace Avelraangame.Services.ServiceUtils
{
    public class CombatUtils
    {
        public enum TacticalSituation
        {
            Major_tactical_advantage = 1,
            Slight_tactical_advantage,
            OnPar,
            Slight_tactical_disadvantage,
            Major_tactical_disadvantage
        }

        public enum FightDifficulty
        {
            Easy = 1,
            Normal,
            Hard,
            Heroic,
            Legendary,
            Astral,
            Random // random should consider the level of the character (or the party) by skills and/or experience
        }

    }
}
