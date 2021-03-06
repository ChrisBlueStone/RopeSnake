﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RopeSnake.Mother3.IO;

namespace RopeSnake.Mother3.Data
{
    // TODO: maybe some sub-classing based on item type? needs more investigation
    public sealed class Item
    {
        // Knowns
        public int Index { get; set; }
        public ItemType Type { get; set; }
        public bool Key { get; set; }
        public int SellPrice { get; set; }
        public EquipFlags EquipFlags { get; set; }
        public int Hp { get; set; }
        public int Pp { get; set; }
        public int Offense { get; set; }
        public int Defense { get; set; }
        public int Iq { get; set; }
        public int Speed { get; set; }
        public FixedKeyDictionary<AilmentType, int> AilmentProtection { get; private set; }
        public FixedKeyDictionary<ElementalType, int> ElementalProtection { get; private set; }
        public int LowerHp { get; set; }
        public int UpperHp { get; set; }
        public int BattleTextIndex { get; set; }

        // Unknowns
        [JsonProperty(PropertyName = "UnknownData")]
        private byte[] unknownData = new byte[0x34];

        public byte GetUnknown(int index)
        {
            return unknownData[index];
        }

        public void SetUnknown(int index, byte value)
        {
            unknownData[index] = value;
        }

        public Item()
        {
            AilmentProtection = new FixedKeyDictionary<AilmentType, int>(
                (AilmentType[])Enum.GetValues(typeof(AilmentType)));

            ElementalProtection = new FixedKeyDictionary<ElementalType, int>(
                (ElementalType[])Enum.GetValues(typeof(ElementalType)));
        }
    }

    public enum ItemType
    {
        Weapon = 0,
        Body = 1,
        Head = 2,
        Arms = 3,
        Food = 4,
        StatusHealer = 5,
        BattleA = 6,
        BattleB = 7,
        ImportantA = 8,
        ImportantB = 9
    }

    [Flags]
    public enum EquipFlags
    {
        None = 0x0000,
        EmptyA = 0x0001,
        Flint = 0x0002,
        Lucas = 0x0004,
        Duster = 0x0008,
        Kumatora = 0x0010,
        Boney = 0x0020,
        Salsa = 0x0040,
        Wess = 0x0080,
        Thomas = 0x0100,
        Ionia = 0x0200,
        Fuel = 0x0400,
        Alec = 0x0800,
        Fassad = 0x1000,
        Claus = 0x2000,
        EmptyB = 0x4000,
        EmptyC = 0x8000
    }

    public enum AilmentType
    {
        Poison = 0,
        Paralysis = 1,
        Sleep = 2,
        Strange = 3,
        Cry = 4,
        Forgetful = 5,
        Nausea = 6,
        Fleas = 7,
        Burned = 8,
        Solidified = 9,
        Numb = 10
    }

    public enum ElementalType
    {
        Neutral = 0,
        Fire = 1,
        Freeze = 2,
        Thunder = 3,
        Bomb = 4
    }
}
