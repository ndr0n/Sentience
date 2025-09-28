using System;
using Sentience;
using UnityEngine;

namespace MindTheatre
{
    public abstract class EntityInteraction : Interaction
    {
        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            if (!HasInteraction(self, interactor, target)) return false;
            Body body = self.Get<Body>();
            return TryExecute(body, interactor, target);
        }

        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            Body data = self.Get<Body>();
            if (data == null) return false;
            return IsPossible(data, interactor, target);
        }

        protected abstract bool IsPossible(Body self, EntityData interactor, EntityData target);

        protected abstract bool TryExecute(Body self, EntityData interactor, EntityData target);
    }
}