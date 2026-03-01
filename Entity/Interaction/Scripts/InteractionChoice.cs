using System;
using UnityEngine;

namespace bitLoner
{
    public struct InteractionChoice
    {
        public string Choice;
        public Sprite Icon;
        public Func<bool> OnClick;

        public InteractionChoice(string choice, Sprite icon, Func<bool> onClick)
        {
            Choice = choice;
            Icon = icon;
            OnClick = onClick;
        }
    }
}