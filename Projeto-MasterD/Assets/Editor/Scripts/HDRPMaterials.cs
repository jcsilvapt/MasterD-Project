using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Presets;

public class HDRPMaterials : EditorWindow
{
    Material mat;
    Color col;


    Texture baseMap;
    Texture roughnessMap;
    Texture normalMap;
    Texture occlusionMap;

    private string folderPath = "";

    [MenuItem("HDRPEditor/HDRP/Single Material")]
    static void UpdateHDRPMaterials()
    {
        HDRPMaterials window = GetWindow<HDRPMaterials>();
        window.Show();
    }

    private void OnGUI()
    {
        #region Find Path
        // escolher a pasta onde estao as texturas
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.TextField(folderPath.Replace(Application.dataPath, ""));
        if (GUILayout.Button("Browse"))
        {
            FindPath();
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        #region Mapas de Texturas
        //mapas de texturas dos materiais
        mat = (Material)EditorGUILayout.ObjectField("Material", mat, typeof(Material), false);
        baseMap = (Texture)EditorGUILayout.ObjectField("Color Map", baseMap, typeof(Texture), false);
        roughnessMap = (Texture)EditorGUILayout.ObjectField("Roughness Map", roughnessMap, typeof(Texture), false);
        normalMap = (Texture)EditorGUILayout.ObjectField("Normal Map", normalMap, typeof(Texture), false);
        occlusionMap = (Texture)EditorGUILayout.ObjectField("Ambient Occlusion", occlusionMap, typeof(Texture), false);
        col = Color.white;
        #endregion

        #region Alterações de Materiais
        //Transforma o Material para HDRP sem ter que colocar texturas
        if (GUILayout.Button("Single Material to HDRP"))
        {
            Undo.RecordObject(mat, "Update RP");
            mat.shader = Shader.Find("HDRP/Lit");
        }

        //transforma o material para HDRP com as texturas que se colocar nos mapas 
        if (GUILayout.Button("Edit Textures to HDRP"))
        {
            SwitchToRP(mat);
        }

        //transforma todos os materiais automaticamente
        if (GUILayout.Button("Convert All to HDRP"))
        {
            ConvertAllToHDRP();
        }
        #endregion
    }

    #region Funções
    void SwitchToRP(Material mat)
    {
        Undo.RecordObject(mat, "Update RP");
        mat.shader = Shader.Find("HDRP/Lit");
        mat.SetColor("_BaseColor", col);
        mat.SetFloat("_BumpMap", 0);
        mat.SetTexture("_BaseColorMap", baseMap);
        mat.SetTexture("_DetailMap", normalMap);
        mat.SetTexture("_NormalMap", normalMap);
        mat.SetTexture("_MaskMap", roughnessMap);
    }

    void FindPath()
    {
        folderPath = EditorUtility.OpenFolderPanel("Select Material Folder", "Assets/", "");
    }

    void ConvertAllToHDRP()
    {
        string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

        foreach (string filePath in files)
        {
            if (filePath.EndsWith(".mat"))
            {
                string unityMaterialPath = filePath.Substring(filePath.IndexOf("Assets")).Replace("\\", "/");
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(unityMaterialPath);
                Undo.RecordObject(mat, "Update RP");
                mat.shader = Shader.Find("HDRP/Lit");
            }
        }
    }
    #endregion
}



