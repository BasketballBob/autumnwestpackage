using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
using AWP;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MADCUP.STM
{
    public class SpriteMesh : MonoBehaviour
    {
        public Sprite sprite;
        public Color color = Color.white;
        
        #region Autumn additions
            public Material baseMaterial;
            // public SpriteMeshSettings settings;
        #endregion

        [SerializeField]
        private Mesh mesh;
        [SerializeField]
        private Material material;

        private const string defaultSpritePath = "Assets/STM (Sprite To Mesh)/Icon_Pack/Square.png";
        private Sprite previousSprite;

        #if UNITY_EDITOR
            [MenuItem("GameObject/2D Object/SpriteMesh", false, 10)]
            static void CreateSpriteMeshObject(MenuCommand menuCommand)
            {
                GameObject go = new GameObject("SpriteMesh");
                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                go.AddComponent<SpriteMesh>();

                // Set position in front of the camera
                Camera sceneCamera = SceneView.lastActiveSceneView.camera;
                if (sceneCamera != null)
                {
                    go.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * 1.5f;
                    go.transform.rotation = Quaternion.identity;
                }

                GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
                Selection.activeObject = go;
            }
        #endif

        // void Start()
        // {
        //     //Initialize();
        // }

        // #if UNITY_EDITOR
        //     void OnValidate()
        //     {
        //         // Use delayCall to defer Initialize call
        //         EditorApplication.delayCall += DelayedInitialize;
        //     }
        // #endif

        // void DelayedInitialize()
        // {
        //     if (this == null) return; // Ensure the object still exists
        //     Initialize();
        // }

        // void Update()
        // {
        //     if (!Application.isPlaying || sprite != previousSprite)
        //     {
        //         Initialize();
        //         previousSprite = sprite;
        //     }
        // }

        void OnDestroy()
        {
            Cleanup();
        }

        void Cleanup()
        {
            if (mesh != null)
            {
                DestroyImmediate(mesh);
            }

            if (material != null)
            {
                DestroyImmediate(material);
            }
        }

        public void Initialize()
        {
            #if UNITY_EDITOR
                if (sprite == null)
                {
                    LoadDefaultSprite();
                }
            #endif

            UpdateMesh();
        }

        #if UNITY_EDITOR
            void LoadDefaultSprite()
            {
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(defaultSpritePath);
            }
        #endif

        void UpdateMesh(bool forceUpdate = false)
        {
            #if UNITY_EDITOR
            Undo.RecordObject(gameObject, "UpdateMesh");
            #endif

            // Early exit if sprite or its required properties are null
            if (sprite == null || sprite.vertices == null || sprite.uv == null || sprite.triangles == null)
            {
                return;
            }

            // Create or update material
            if (forceUpdate || material == null)
            {
                material = CreateMaterialBasedOnRenderPipeline();

                #if UNITY_EDITOR
                AWAssetGeneration.CreateMaterial(material, gameObject.name);
                #endif
            }

            // Create or update mesh
            if (mesh == null)
            {
                mesh = new Mesh { name = "GM_" + gameObject.name};

                #if UNITY_EDITOR
                AWAssetGeneration.CreateMesh(mesh, gameObject.name);
                #endif
            }
            else
            {
                mesh.Clear();
            }

            // Initialize arrays
            Vector3[] vertices = new Vector3[sprite.vertices.Length];
            Vector2[] uvs = new Vector2[sprite.uv.Length];
            int[] triangles = new int[sprite.triangles.Length];

            // Convert sprite data to mesh data
            for (int i = 0; i < sprite.vertices.Length; i++)
            {
                vertices[i] = sprite.vertices[i];  // Assuming sprite.vertices are Vector3
            }

            for (int i = 0; i < sprite.uv.Length; i++)
            {
                uvs[i] = sprite.uv[i];  // Copy UVs
            }

            for (int i = 0; i < sprite.triangles.Length; i++)
            {
                triangles[i] = sprite.triangles[i];  // Copy triangles
            }

            // Assign vertices, UVs, and triangles to the Mesh
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            // Recalculate normals for proper lighting
            mesh.RecalculateNormals();

            // Ensure MeshFilter and MeshRenderer components exist
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = mesh;

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            meshRenderer.sharedMaterial = material; // Assign the material to MeshRenderer

            // Assign texture and color to material, only if sprite.texture is not null
            if (sprite.texture != null)
            {
                material.mainTexture = sprite.texture;
            }
            else
            {
                material.mainTexture = null;
            }
            material.color = color;
            material.SetFloat("_Smoothness", 0f);
        }

        void SetMaterialToTransparent(Material mat)
        {
            // Set material to transparent
            mat.SetFloat("_Surface", 1); // SurfaceType.Transparent
            mat.SetFloat("_Blend", 0); // BlendMode.Alpha
            mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetFloat("_ZWrite", 0);
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            mat.EnableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.DisableKeyword("_SURFACE_TYPE_OPAQUE");
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        }

        Material CreateMaterialBasedOnRenderPipeline()
        {
            Material mat;
            var renderPipelineAsset = GraphicsSettings.currentRenderPipeline;

            if (baseMaterial != null)
            {
                mat = new Material(baseMaterial)
                {
                    name = "Autumn Base Generated Material"
                };
                SetMaterialToTransparent(mat);
            }
            else if (renderPipelineAsset == null)
            {
                mat = new Material(Shader.Find("Standard"))
                {
                    name = "Generated Material"
                };
                SetMaterialToTransparent(mat);
            }
            else if (renderPipelineAsset.GetType().Name.Contains("HDRenderPipelineAsset"))
            {
                mat = new Material(Shader.Find("HDRP/Lit"))
                {
                    name = "Generated Material"
                };
                SetMaterialToTransparent(mat);
            }
            else if (renderPipelineAsset.GetType().Name.Contains("UniversalRenderPipelineAsset"))
            {
                Debug.Log("URP");
                mat = new Material(Shader.Find("Universal Render Pipeline/Lit"))
                {
                    name = "Generated Material"
                };
                SetMaterialToTransparent(mat);
            }
            else
            {
                Debug.Log("Current render pipeline: Unknown");
                mat = new Material(Shader.Find("Standard"))
                {
                    name = "Generated Material"
                };
                SetMaterialToTransparent(mat);
            }

            return mat;
        }

        #region Autumn additions
            public void ForceUpdate()
            {
                Initialize();
                UpdateMesh(forceUpdate: true);
            }
        #endregion
    }

    // public class SpriteMeshSettings
    // {
    //     public float _Smoothness;
    // }
}
