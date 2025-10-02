using System.Collections.Generic;
using MindTheatre;
using Unity.Rendering;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Sentience
{
    [System.Serializable]
    public class Body : EntityComponent
    {
        public Mesh Mesh;
        public Material Material;
        public EntitySpawn Prefab;
        public EntitySpawn Spawn;

        public void SpawnBody(System.Random random, Transform parent, Vector3 worldPosition)
        {
            ID id = Data.Get<ID>();

            Material = new Material(Material);
            if (Data.Has<Avatar>())
            {
                Avatar avatar = Data.Get<Avatar>();
                Material.mainTexture = avatar.Sprite.texture;
                Material.mainTextureOffset = new(avatar.Sprite.rect.x / avatar.Sprite.texture.width, avatar.Sprite.rect.y / avatar.Sprite.texture.height);
                Material.mainTextureScale = new(avatar.Sprite.rect.size.x / avatar.Sprite.texture.width, avatar.Sprite.rect.size.y / avatar.Sprite.texture.height);
                // Material.mainTextureScale = avatar.Sprite.textureRect.size * avatar.Sprite.spriteAtlasTextureScale;
                // avatar.Sprite.pivot;
            }

            if (Prefab != null)
            {
                EntitySpawn spawn = Object.Instantiate(Prefab.gameObject, parent).GetComponent<EntitySpawn>();
                Spawn = spawn;

                MeshRenderer renderer = spawn.GetComponent<MeshRenderer>();
                if (renderer == null) renderer = spawn.AddComponent<MeshRenderer>();
                MeshFilter meshFilter = spawn.GetComponent<MeshFilter>();
                if (meshFilter == null) meshFilter.AddComponent<MeshFilter>();
                meshFilter.mesh = Mesh;
                renderer.material = Material;

                Spawn.OnSpawn(Data, id.Type, worldPosition);
            }
            // else
            // {
            // Spawn = null;
            // var desc = new RenderMeshDescription(shadowCastingMode: ShadowCastingMode.Off, receiveShadows: false);
            // var renderMeshArray = new RenderMeshArray(new Material[] {Material}, new Mesh[] {Mesh});
            // RenderMeshUtility.AddComponents(Data.Entity, World.DefaultGameObjectInjectionWorld.EntityManager, desc, renderMeshArray, MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
            // }

            id.Position = worldPosition;
        }
    }

    [System.Serializable]
    public class BodyAuthoring : EntityAuthoring
    {
        public Mesh Mesh;
        public Material Material;
        public List<EntitySpawn> Prefabs = new();

        public override IEntityComponent Spawn(System.Random random)
        {
            Body body = new();
            body.Mesh = Mesh;
            body.Material = Material;
            if (Prefabs.Count > 0) body.Prefab = Prefabs[random.Next(0, Prefabs.Count)];
            return body;
        }
    }
}