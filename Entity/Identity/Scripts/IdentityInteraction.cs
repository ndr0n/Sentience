using System;
using Sentience;
using UnityEngine;

namespace MindTheatre
{
    public abstract class IdentityInteraction : Interaction
    {
        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            if (!IsPossible(self)) return false;
            if (!HasInteraction(self, interactor, target)) return false;
            IdentityData data = self as IdentityData;
            return TryExecute(data, interactor, target);
        }

        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            if (self is not IdentityData data) return false;
            return OnHasInteraction(data, interactor, target);
        }

        protected abstract bool OnHasInteraction(IdentityData self, EntityData interactor, EntityData target);

        protected abstract bool TryExecute(IdentityData self, EntityData interactor, EntityData target);
    }
}