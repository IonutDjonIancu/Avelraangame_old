using System.Collections.Generic;

namespace Avelraangame.Services.ServiceUtils
{
    public static class ItemUtils
    {
        public static readonly ItemTypes itemTypes = new ItemTypes();
        public static readonly ItemNames itemNames = new ItemNames();
        public static readonly InventorySlots inventorySlots = new InventorySlots();

        #region TypedClasses
        public class ItemTypes
        {
            public readonly string Apparatus = Scribe.ItemTypes_Apparatus;
            public readonly string Armour = Scribe.ItemTypes_Armour;
            public readonly string Axe = Scribe.ItemTypes_Axe;
            public readonly string Bow = Scribe.ItemTypes_Bow;
            public readonly string Club = Scribe.ItemTypes_Club;
            public readonly string Crossbow = Scribe.ItemTypes_Crossbow;
            public readonly string Mace = Scribe.ItemTypes_Mace;
            public readonly string ObjectFromAfar = Scribe.ItemTypes_ObjectFromAfar;
            public readonly string Polearm = Scribe.ItemTypes_Polearm;
            public readonly string Shield = Scribe.ItemTypes_Shield;
            public readonly string Spear = Scribe.ItemTypes_Spear;
            public readonly string Sword = Scribe.ItemTypes_Sword;
            public readonly string Valuables = Scribe.ItemTypes_Valuables;
            public readonly string Warhammer = Scribe.ItemTypes_Warhammer;
        }

        public class InventorySlots
        {
            public readonly string Mainhand = Scribe.InventorySlots_Mainhand;
            public readonly string Offhand = Scribe.InventorySlots_Offhand;
            public readonly string Ranged = Scribe.InventorySlots_Ranged;
            public readonly string Armour = Scribe.InventorySlots_Armour;
            public readonly string Trinkets = Scribe.InventorySlots_Trinkets;
            public readonly string Supplies = Scribe.InventorySlots_Supplies;
        }

        public class ItemNames
        {
            public readonly Common common = new Common();
            public readonly Refined refined = new Refined();
            public readonly Masterwork masterwork = new Masterwork();
            public readonly string Heirloom = Scribe.ItemNamePrefixes_4_Heirloom;
            public readonly string ObjectFromAfar = Scribe.ItemTypes_ObjectFromAfar;

            public class Common
            {
                public readonly string Basic = Scribe.ItemNamePrefixes_1_Basic;
                public readonly string LowQuality = Scribe.ItemNamePrefixes_1_LowQuality;
                public readonly string Improvised = Scribe.ItemNamePrefixes_1_Improvised;
                public readonly string StolenImprovised = Scribe.ItemNamePrefixes_1_StolenImprovised;
                public readonly string Cracked = Scribe.ItemNamePrefixes_1_Cracked;
            }

            public class Refined
            {
                public readonly string Embossed = Scribe.ItemNamePrefixes_2_Embossed;
                public readonly string Curated = Scribe.ItemNamePrefixes_2_Curated;
                public readonly string Treated = Scribe.ItemNamePrefixes_2_Treated;
                public readonly string Tempered = Scribe.ItemNamePrefixes_2_Tempered;
                public readonly string Durable = Scribe.ItemNamePrefixes_2_Durable;
            }

            public class Masterwork
            {
                public readonly string Mastercraft = Scribe.ItemNamePrefixes_3_Mastercraft;
                public readonly string Superior = Scribe.ItemNamePrefixes_3_Superior;
                public readonly string Elegant = Scribe.ItemNamePrefixes_3_Elegant;
                public readonly string Balanced = Scribe.ItemNamePrefixes_3_Balanced;
                public readonly string Purified = Scribe.ItemNamePrefixes_3_Purified;
            }
        }
        #endregion

        #region Lists
        // Valuables are always set at the end of any list
        public static List<string> List_of_ItemTypes = new List<string>()
        {
            itemTypes.Apparatus, // 0    
            itemTypes.Armour, // 1
            itemTypes.Axe, // 2
            itemTypes.Bow, // 3
            itemTypes.Club, // 4
            itemTypes.Crossbow, // 5
            itemTypes.Mace, // 6
            itemTypes.Polearm, // 7
            itemTypes.Shield, // 8
            itemTypes.Spear, // 9
            itemTypes.Sword, // 10
            itemTypes.Warhammer, // 11
            itemTypes.Valuables // 12
        };

        public static List<string> List_of_CommonNamePrefixes = new List<string>()
        {
            itemNames.common.Basic, // 0
            itemNames.common.LowQuality, // 1
            itemNames.common.Improvised, // 2
            itemNames.common.StolenImprovised, // 3
            itemNames.common.Cracked // 4 
        };

        public static List<string> List_of_RefinedNamePrefixes = new List<string>()
        {
            itemNames.refined.Embossed, // 0
            itemNames.refined.Curated, // 1
            itemNames.refined.Treated, // 2
            itemNames.refined.Tempered, // 3
            itemNames.refined.Durable // 4
        };

        public static List<string> List_of_MasterworkNamePrefixes = new List<string>()
        {
            itemNames.masterwork.Mastercraft, // 0
            itemNames.masterwork.Superior, // 1
            itemNames.masterwork.Elegant, // 2
            itemNames.masterwork.Balanced, // 3
            itemNames.masterwork.Purified // 4
        };
        #endregion

        #region Enums
        public enum Slots
        {
            Mainhand = 1,
            Offhand,
            Ranged,
            Armour,
            Trinkets,
            Supplies // 6
        }

        public enum Types
        {
            Apparatus = 1,
            Armour,
            Axe,
            Bow,
            Club,
            Crossbow,
            Mace,
            Polearm,
            Shield,
            Spear,
            Sword,
            Valuables,
            Warhammer // 13
        }
        #endregion
    }
}
