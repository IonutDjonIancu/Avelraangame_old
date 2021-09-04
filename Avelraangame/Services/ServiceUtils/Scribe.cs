namespace Avelraangame.Services.ServiceUtils
{
    public static class Scribe
    {
        // should keep the typed constant short!
        // Valuables are always last in any list

        #region Characters
        public const string Characters_GenericName = "Marqus";
        #endregion

        #region Stats
        public const string Stats_Strength = "Strength";
        public const string Stats_Toughness = "Toughness";
        public const string Stats_Awareness = "Awareness";
        public const string Stats_Abstract = "Strength";
        #endregion


        #region Assets
        public const string Assets_Health = "Health";
        public const string Assets_Mana = "Mana";
        public const string Assets_Harm = "Harm";
        public const string Assets_DRM = "DRM";
        public const string Assets_Experience = "Experience";
        public const string Assets_Wealth = "Wealth";
        #endregion


        #region Skills
        public const string Skills_Apothecary = "Apothecary";
        public const string Skills_Arcane = "Arcane";
        public const string Skills_Dodge = "Dodge";
        public const string Skills_Hide = "Hide";
        public const string Skills_Melee = "Melee";
        public const string Skills_Navigation = "Navigation";
        public const string Skills_Psionics = "Psionics";
        public const string Skills_Ranged = "Ranged";
        public const string Skills_Resistance = "Resistance";
        public const string Skills_Scouting = "Scouting";
        public const string Skills_Social = "Social";
        public const string Skills_Spot = "Spot";
        public const string Skills_Survival = "Survival";
        public const string Skills_Tactics = "Tactics";
        public const string Skills_Traps = "Traps";
        public const string Skills_Unarmed = "Unarmed";
        #endregion


        #region SkillsExplained
        public const string SkillsExplained_Apothecary = "... to fill";
        public const string SkillsExplained_Arcane = "... to fill";
        public const string SkillsExplained_Dodge = "... to fill";
        public const string SkillsExplained_Hide = "... to fill";
        public const string SkillsExplained_Melee = "... to fill";
        public const string SkillsExplained_Navigation = "... to fill";
        public const string SkillsExplained_Psionics = "... to fill";
        public const string SkillsExplained_Ranged = "... to fill";
        public const string SkillsExplained_Resistance = "... to fill";
        public const string SkillsExplained_Scouting = "... to fill";
        public const string SkillsExplained_Social = "... to fill";
        public const string SkillsExplained_Spot = "... to fill";
        public const string SkillsExplained_Survival = "... to fill";
        public const string SkillsExplained_Tactics = "... to fill";
        public const string SkillsExplained_Traps = "... to fill";
        public const string SkillsExplained_Unarmed = "... to fill";
        #endregion


        #region InventorySlots
        public const string InventorySlots_Mainhand = "Mainhand";
        public const string InventorySlots_Offhand = "Offhand";
        public const string InventorySlots_Ranged = "Ranged";
        public const string InventorySlots_Armour = "Armour";
        public const string InventorySlots_Trinkets = "Trinkets";
        public const string InventorySlots_Supplies = "Supplies";
        #endregion


        #region ItemTypes
        public const string ItemTypes_Apparatus = "Apparatus";
        public const string ItemTypes_Armour = "Armour";
        public const string ItemTypes_Axe = "Axe";
        public const string ItemTypes_Bow = "Bow";
        public const string ItemTypes_Club = "Club";
        public const string ItemTypes_Crossbow = "Crossbow";
        public const string ItemTypes_Mace = "Mace";
        public const string ItemTypes_Polearm = "Polearm";
        public const string ItemTypes_Shield = "Shield";
        public const string ItemTypes_Spear = "Spear";
        public const string ItemTypes_Sword = "Sword";
        public const string ItemTypes_Warhammer = "Warhammer";
        public const string ItemTypes_Valuables = "Valuables";
        public const string ItemTypes_ObjectFromAfar = "Object from afar";
        #endregion


        #region ItemNamePrefixes
        public const string ItemNamePrefixes_1_Basic = "Basic";
        public const string ItemNamePrefixes_1_LowQuality = "Low quality";
        public const string ItemNamePrefixes_1_Improvised = "Improvised";
        public const string ItemNamePrefixes_1_StolenImprovised = "Stolen-improvised";
        public const string ItemNamePrefixes_1_Cracked = "Cracked";

        public const string ItemNamePrefixes_2_Embossed = "Embossed";
        public const string ItemNamePrefixes_2_Curated = "Curated";
        public const string ItemNamePrefixes_2_Treated = "Treated";
        public const string ItemNamePrefixes_2_Tempered = "Tempered";
        public const string ItemNamePrefixes_2_Durable = "Durable";

        public const string ItemNamePrefixes_3_Mastercraft = "Mastercraft";
        public const string ItemNamePrefixes_3_Superior = "Superior";
        public const string ItemNamePrefixes_3_Elegant = "Elegant";
        public const string ItemNamePrefixes_3_Balanced = "Well-balanced";
        public const string ItemNamePrefixes_3_Purified = "Purified";

        public const string ItemNamePrefixes_4_Heirloom = "Heirloom";
        #endregion


        #region ItemLevels
        public const string ItemLevels_Common = "Common";
        public const string ItemLevels_Refined = "Refined";
        public const string ItemLevels_Masterwork = "Masterwork";
        public const string ItemLevels_Heirloom = "Heirloom";
        public const string ItemLevels_Artifact = "Artifact";
        public const string ItemLevels_Relic = "Relic";
        #endregion


        #region ItemArtifacts
        public const string ItemArtifacts_MarshardDaggers = "Marshard Daggers";
        #endregion


        #region ItemRelics
        public const string ItemRelics_SwordOfLuvoran = "Sword of Luvoran";
        #endregion


        #region Errors
        public const string Error_Switch_notCoveredCase = "The current switch statement returned the default case.";
        public const string Error_IfElseIf_notCoveredCase = "The current IfElseIf block returned the final else case.";
        public const string Error_AttributeIsMissing = "is missing."; // to check for null or empty/whitespace strings
        public const string Error_Player_wardCheckNotEqualsWard = "Wards do not match.";
        public const string Error_Player_wardTooLong = "The ward is too long, 4 chars max.";
        public const string Error_Player_nameTooLong = "The name is too long, 50 chars max.";
        public const string Error_Player_alreadyExists = "The player name already exists.";
        public const string Error_Player_playersLimitReached = "The limit for total number of players has be reached.";
        #endregion


        #region ApplicationMessages
        public const string Application_operation_success = "Success";
        public const string Application_requestIsInvalid = "Request is invalid";
        
        public enum ShortMessages
        {
            Ok,
            BadRequest,
            Success,
            Failure,
            ResourceNotFound,
            NpcDmg
        }
        #endregion

        #region TemporaryDataTable
        public enum ServiceTempData
        {
            PlayerData
        }


        #endregion

        #region CRUD
        public static class CrudActions
        {
            public static string Create = "Create";
            public static string Read = "Read";
            public static string Update = "Update";
            public static string Delete = "Delete";
        }
        #endregion

        #region GameSettings
        public static class Difficulty
        {
            /// <summary>
            /// 1
            /// </summary>
            public static string D1_Easy = "Easy";
            /// <summary>
            /// 2
            /// </summary>
            public static string D2_Normal = "Normal";
            /// <summary>
            /// 3
            /// </summary>
            public static string D3_Hard = "Hard";
            /// <summary>
            /// 4
            /// </summary>
            public static string D4_Heroic = "Heroic";
            /// <summary>
            /// 5
            /// </summary>
            public static string D5_Legendary = "Legendary";
            /// <summary>
            /// 6
            /// </summary>
            public static string D6_Astral = "Astral";
        }
        #endregion
    }
}
