using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MindTheatre;
using Unity.Collections;
using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public abstract class Interaction : ScriptableObject
    {
        public string Name = "Interaction";
        public string Description = "";
        public string Tags = "";

        public abstract bool HasInteraction(EntityData self, EntityData interactor, EntityData target);

        public bool TryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(self.Entity, new InteractionComponent(this, self.Entity, interactor.Entity, target.Entity));
            return true;
        }

        public abstract bool Interact(ref SystemState state, RefRW<InteractionComponent> comp);

        public bool IsWithinRange(Body self, Body interactor, Vector2 range)
        {
            float distance = Vector3.Distance(self.Spawn.transform.position, interactor.Spawn.transform.position);
            if (distance >= range.x && distance <= (range.y + 0.5f)) return true;
            return false;
        }
    }
}