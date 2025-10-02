using System.Collections.Generic;
using MindTheatre;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using World = Unity.Entities.World;

namespace Sentience
{
    [System.Serializable]
    public class Body : EntityComponent
    {
        // public EntitySpawn Prefab;
        // public Spawn Spawn;

        public Mesh Mesh;
        public Material Material;

        Material renderMaterial;

        public void SpawnBody(System.Random random, Transform parent, Vector3 worldPosition)
        {
            // Spawn spawn = Object.Instantiate(Prefab.gameObject, parent).GetComponent<Spawn>();
            // Spawn = spawn;

            // Material = new Material(Material);
            if (Data.Has<Avatar>())
            {
                Avatar avatar = Data.Get<Avatar>();
                Material.mainTexture = avatar.Sprite.texture;
            }

            var desc = new RenderMeshDescription(shadowCastingMode: ShadowCastingMode.Off, receiveShadows: false);
            var renderMeshArray = new RenderMeshArray(new Material[] {Material}, new Mesh[] {Mesh});
            RenderMeshUtility.AddComponents(Data.Entity, World.DefaultGameObjectInjectionWorld.EntityManager, desc, renderMeshArray, MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

            ID id = Data.Get<ID>();
            id.Position = worldPosition;
            // Spawn.OnSpawn(Data, id.Type, worldPosition);

            // return spawn;
        }
    }

    [System.Serializable]
    public class BodyAuthoring : EntityAuthoring
    {
        // public List<EntitySpawn> Prefabs = new();
        public Mesh Mesh;
        public Material Material;

        public override IComponentData Spawn(System.Random random)
        {
            Body body = new();
            body.Mesh = Mesh;
            body.Material = Material;
            // body.Prefab = Prefabs[random.Next(0, Prefabs.Count)];
            return body;
        }
    }
}