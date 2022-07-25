using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;

namespace YesUtils
{

    public class ShaderSwap : EditorWindow
    {

        public Texture2D logo;
        public string plug = "Tool made by Yes/Y e s#6969";
        public string profile = "https://vrchat.com/home/user/usr_6018bfa9-da5a-45af-a2e0-741dfdd9a9ee";
        private Vector2 scroll;
        private AdvancedDropdownState dropdownState = null;
        private List<string> shaderList = new List<string>();

        private List<Material> material = new List<Material>();
 
        private bool materialFoldout = false;

        private string targetShader = "Standard";
        private string selectedShader = "Standard";

        [MenuItem("YesUtils/Shader Swap Utility")]
        public static void ShowWindow()
        {
            var window = GetWindow<ShaderSwap>("Shader Swap Utility");
            window.minSize = new Vector2(408f, 570f);
        }

        private void OnGUI()
        {
            
            GUILayout.Label("Use this to swap shaders of materials in your scene or project.", EditorStyles.boldLabel);
            GUILayout.Space(20f);
            GUILayout.Label("Shader you want to replace", EditorStyles.boldLabel);
            if (EditorGUILayout.DropdownButton(new GUIContent(selectedShader), FocusType.Passive))
            {
                var dropdown = new CustomAdvancedDropdown(dropdownState, Dropdown_ItemSelected);
                dropdown.Show(GUILayoutUtility.GetLastRect());
            }
            GUILayout.Space(20f);
            GUILayout.Label("New Shader", EditorStyles.boldLabel);
            if (EditorGUILayout.DropdownButton(new GUIContent(targetShader), FocusType.Passive))
            {
                var dropdown = new CustomAdvancedDropdown(dropdownState, Dropdown_ItemSelected_Target);
                dropdown.Show(GUILayoutUtility.GetLastRect());
            }
            GUILayout.Space(20f);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Scene Materials", GUILayout.Width((position.width / 3) - 3), GUILayout.Height(50f)))
            {
                material.Clear();
                foreach(var target in FindObjectsOfType<Renderer>())
                {
                    foreach(Material mat in target.sharedMaterials)
                    {
                        if (mat.shader.name == selectedShader && !material.Contains(mat) && mat.hideFlags != HideFlags.NotEditable)
                        {
                            material.Add(mat);
                        }
                    }
                }
            }
            if (GUILayout.Button("Project Materials", GUILayout.Width((position.width / 3) - 3), GUILayout.Height(50f)))
            {
                material.Clear();
                string[] guids = (string[])AssetDatabase.FindAssets("t: Material", new[] { "Assets/" });
                List<string> shaders = new List<string>();
                foreach (string id in guids)
                {
                    shaders.Add(AssetDatabase.GUIDToAssetPath(id));
                }
                foreach (string obj in shaders)
                {
                    Material target = (Material)AssetDatabase.LoadAssetAtPath(obj, typeof(Material));
                    if(target.shader.name == selectedShader)
                    {
                        material.Add(target);
                    }
                    
                }
            }
            if(GUILayout.Button("Clear Materials", GUILayout.Width((position.width / 3) - 3), GUILayout.Height(50f)))
            {
                material.Clear();
            }
            GUILayout.EndHorizontal();

            materialFoldout = EditorGUILayout.Foldout(materialFoldout, "Material List");
            if (materialFoldout)
            {
                GUILayout.BeginVertical();
                scroll =  EditorGUILayout.BeginScrollView(scroll, GUILayout.MinHeight(50f), GUILayout.MaxHeight(200F), GUILayout.ExpandHeight(true));
                foreach(Object mat in material)
                {
                    EditorGUILayout.ObjectField(mat, typeof(Material), false);
                }
                EditorGUILayout.EndScrollView();
                if(GUILayout.Button("Add Material"))
                {
                    material.Add(null);
                }
                if (GUILayout.Button("Remove Material"))
                {
                    if(material.Count != 0)
                    {
                        material.Remove(material[material.Count - 1]);
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.Space(20f);
            if(GUILayout.Button("Process Materials!", GUILayout.Height(40f)))
            {
                foreach(Material mat in material)
                {
                    mat.shader = Shader.Find(targetShader);
                }
            }
            GUILayout.Space(40f);
            GUILayout.BeginVertical();
            GUILayout.Label(plug, EditorStyles.boldLabel);
            GUILayout.Label(logo, GUILayout.MinWidth(400f), GUILayout.MinHeight(200f));
            GUILayout.EndVertical();

        }

        private void Dropdown_ItemSelected(string item)
        {
            selectedShader = item;
        }
        private void Dropdown_ItemSelected_Target(string item)
        {
            targetShader = item;
        }
    }
}