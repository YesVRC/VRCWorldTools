using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace YesUtils
{
    public class CustomAdvancedDropdown : AdvancedDropdown
    {
        private Action<string> onItemSelected;

        public CustomAdvancedDropdown(AdvancedDropdownState state, Action<string> onItemSelected)
            : base(state)
        {
            minimumSize = new Vector2(270, 308);
            this.onItemSelected = onItemSelected; // add item selected callback
        }

        // builds the dropdown menu
        protected override AdvancedDropdownItem BuildRoot()
        {
            // create the root element
            AdvancedDropdownItem root = new AdvancedDropdownItem("Shader List");
            string[] guids = (string[])AssetDatabase.FindAssets("t: shader", new[] {"Assets/"});
            List<string> shaders = new List<string>();
            foreach(string id in guids)
            {
                shaders.Add(AssetDatabase.GUIDToAssetPath(id));
            }
            foreach(string obj in shaders)
            {
                Shader target = (Shader)AssetDatabase.LoadAssetAtPath(obj, typeof(Shader));
                root.AddChild(new AdvancedDropdownItem(target.name));
            }
            // add basic child elements
            root.AddChild(new AdvancedDropdownItem("Standard"));
            return root;
        }

        // called when an item is selected
        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            // alert item selected
            onItemSelected(item.name);
        }
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
    }
}