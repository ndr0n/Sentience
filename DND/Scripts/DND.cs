using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    public static class DND
    {
        public enum Alignment
        {
            ChaoticEvil = 0,
            NeutralEvil = 1,
            LawfulEvil = 2,
            ChaoticNeutral = 3,
            Neutral = 4,
            LawfulNeutral = 5,
            ChaoticGood = 6,
            NeutralGood = 7,
            LawfulGood = 8,
        }

        public enum AbilityScore
        {
            Strength = 0,
            Dexterity = 1,
            Constitution = 2,
            Intelligence = 3,
            Wisdom = 4,
            Charisma = 5,
        }

        public static Alignment ParseAlignment(string alignment)
        {
            alignment = alignment.Replace(" ", "").ToLower();
            Alignment align = Alignment.Neutral;
            if (Enum.TryParse(alignment, true, out Alignment parsed))
            {
                align = parsed;
            }
            return align;
        }

        public static int GetModifier(int attribute)
        {
            return (attribute / 2) - 5;
        }

        public static int GetModifierClamped(int attribute, int minimum = int.MinValue, int maximum = int.MaxValue)
        {
            int modifier = GetModifier(attribute);
            if (modifier < minimum) modifier = minimum;
            else if (modifier > maximum) modifier = maximum;
            return modifier;
        }

        public static List<int> GetModifiers(List<int> attributes)
        {
            List<int> modifiers = new();
            for (int i = 0; i < attributes.Count; i++) modifiers.Add(GetModifier(attributes[i]));
            return modifiers;
        }

        public static int GetRoll(string info, List<int> modifiers = null)
        {
            int currentIndex = 0;
            string[] data = info.Split("+");
            int extra = 0;
            int roll = 0;
            if (info.Substring(1, 1) == "d")
            {
                string[] dice = data[0].Split("d");
                int diceAmount = int.Parse(dice[0]);
                int diceType = int.Parse(dice[1]);
                roll = GetRoll(diceAmount, diceType);
                currentIndex = 2;
            }
            else
            {
                roll = int.Parse(data[0]);
                currentIndex = 1;
            }
            if (data.Length >= currentIndex)
            {
                for (int i = 1; i < data.Length; i++)
                {
                    if (int.TryParse(data[i], out int result))
                    {
                        extra += result;
                    }
                    if (modifiers != null)
                    {
                        extra += modifiers[GetAttributeFromString(data[i])];
                    }
                }
            }
            roll += extra;
            if (roll < 1) roll = 1;
            Debug.Log($"{info} rolled {roll}");
            return roll;
        }

        public static int GetRoll(int amount, int dice, int modifiers = 0)
        {
            int roll = 0;
            for (int i = 0; i < amount; i++) roll += UnityEngine.Random.Range(1, dice + 1);
            roll += modifiers;
            return roll;
        }

        public static int GetAttributeFromString(string attribute)
        {
            string[] names = Enum.GetNames(typeof(AbilityScore));
            for (int i = 0; i < names.Length; i++)
            {
                if (attribute.ToLower().Substring(0, 3) == names[i].ToLower().Substring(0, 3)) return i;
            }
            return -1;
        }

        public static int GetPowerFromPrice(int price)
        {
            return (int) Math.Floor(Math.Cbrt(price) + 5f);
        }

        public static int GetSuccessProbability(int dc)
        {
            return 5 * (21 - dc);
        }

        public static int GetProbabilityFromDifficulty(int dc, int modifier = 0)
        {
            return 100;
        }
    }
}