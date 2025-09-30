using Unity.Entities;

namespace Sentience
{
    public static class EntityLibrary
    {
        public static bool Has<T>(Entity entity)
        {
            return World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<T>(entity);
        }

        public static T Get<T>(Entity entity)
        {
            return World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentObject<T>(entity);
        }

        // public static void Set<T>(Entity entity, IComponentData value) where T : unmanaged, IComponentData
        // {
        // World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData<T>(entity, value);
        // }

        public static bool IsHostile(Entity source, Entity target)
        {
            if (!Has<Identity>(source)) return false;
            Identity identity = Get<Identity>(source);
            if (identity.Faction == null) return false;

            if (!Has<Identity>(target)) return false;
            Identity targetIdentity = Get<Identity>(target);
            if (targetIdentity.Faction == null) return false;

            return targetIdentity.Faction.IsHostile(identity.Faction);
        }
    }
}