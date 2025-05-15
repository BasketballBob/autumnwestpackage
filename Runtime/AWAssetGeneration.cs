using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AWP
{
    public static class AWAssetGeneration
    {
        public const string RootFolder = "Assets/GeneratedAssets/";

        public const string MeshFolder = "Mesh/";
        public const string MaterialFolder = "Material/";

        public static void EnsureDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath)) return;
            Directory.CreateDirectory(directoryPath);
        }

        public static void CreateMesh(Mesh mesh, string name) => 
            CreateAsset(mesh, name + ".asset", MeshFolder);

        public static void CreateMaterial(Material material, string name) =>
            CreateAsset(material, name + ".mat", MaterialFolder);

        public static void CreateAsset(Object asset, string name, string subFolder)
        {
            EnsureDirectory(RootFolder + subFolder);
            UnityEditor.AssetDatabase.CreateAsset(asset, RootFolder + subFolder + "GA_" + name);
        }
    }
}
