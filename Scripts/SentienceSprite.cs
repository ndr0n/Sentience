using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    struct SentienceTextureParser
    {
        public string texture;
    }

    public class SentienceSprite : MonoBehaviour
    {
        // public SpriteRenderer Renderer;
        public Sprite Example;
        public string Description;
        public Sprite Generated;

        public async Awaitable TryGenerate()
        {
            Debug.Log($"TRY GENERATE SPRITE!");
            Generated = await DungeonMaster.Instance.GenerateSprite(Example, Description);
            Debug.Log($"FINISH GENERATE SPRITE!");
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SentienceSprite))]
    public class SentienceSprite_Editor : Editor
    {
        public SentienceSprite SentienceSprite;

        void OnEnable()
        {
            if (target == null) return;
            SentienceSprite = (SentienceSprite) target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Generate Sprite"))
            {
                _ = SentienceSprite.TryGenerate();
            }
        }
    }
#endif
}