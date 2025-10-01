using MindTheatre;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;

namespace Sentience
{
    public struct InteractionComponent : IComponentData
    {
        public FixedString4096Bytes InteractionPath;
        public Entity Self;
        public Entity Interactor;
        public Entity Target;

        public InteractionComponent(Interaction interaction, Entity self, Entity interactor, Entity target)
        {
            InteractionPath = AssetDatabase.GetAssetPath(interaction);
            Self = self;
            Interactor = interactor;
            Target = target;
        }
    }
}