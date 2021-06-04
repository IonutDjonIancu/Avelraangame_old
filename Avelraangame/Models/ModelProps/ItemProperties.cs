namespace Avelraangame.Models.ModelProps
{
    public class ItemProperties
    {
        // stats
        public int Stats_toStrength { get; set; }
        public int Stats_toToughness { get; set; }
        public int Stats_toAwareness { get; set; }
        public int Stats_toAbstract { get; set; }

        // assets
        public int Assets_toHealth { get; set; }
        public int Assets_toMana { get; set; }
        public int Assets_toHarm { get; set; }
        public int Assets_toExperience { get; set; }
        public int Assets_toDRM { get; set; }
        public int Assets_toWealth { get; set; }

        // skills
        public int Skills_toApothecary { get; set; }
        public int Skills_toArcane { get; set; }
        public int Skills_toDodge { get; set; }
        public int Skills_toHide { get; set; }
        public int Skills_toMelee { get; set; }
        public int Skills_toNavigation { get; set; }
        public int Skills_toPsionics { get; set; }
        public int Skills_toRanged { get; set; }
        public int Skills_toResistance { get; set; }
        public int Skills_toScouting { get; set; }
        public int Skills_toSocial { get; set; }
        public int Skills_toSpot { get; set; }
        public int Skills_toSurvival { get; set; }
        public int Skills_toTactics { get; set; }
        public int Skills_toTraps { get; set; }
        public int Skills_toUnarmed { get; set; }

        // equipped slots
        public bool Slots_MainHand { get; set; }
        public bool Slots_OffHand { get; set; }
        public bool Slots_Ranged { get; set; }
        public bool Slots_Armour { get; set; }
        public bool Slots_Trinkets { get; set; }
        public bool Slots_Supplies { get; set; }
    }
}
