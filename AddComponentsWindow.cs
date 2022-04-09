using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.IO;

public class AddComponentsWindow : EditorWindow
{
    private static Dictionary<string, bool> PhysicsComponents = new Dictionary<string, bool>();
    bool physicsGroup = true;
    private static Dictionary<string, bool> Physics2DComponents = new Dictionary<string, bool>();
    bool physics2DGroup = true;
    private static Dictionary<string, bool> AllScripts = new Dictionary<string, bool>();
    bool scriptsGroup = true;

    private static Dictionary<string, Type> ComponentsTypes = new Dictionary<string, Type>();

    [MenuItem("Window/Add Multiple Components")]
    static void OpenWindow()
    {
        GetWindow<AddComponentsWindow>().Show();
    }

    void OnEnable()
    {
        ComponentsTypes.Clear();
        // Physics
        PhysicsComponents.Clear();
        PhysicsComponents.Add("BoxCollider", false);
        PhysicsComponents.Add("SphereCollider", false);
        PhysicsComponents.Add("MeshCollider", false);
        PhysicsComponents.Add("Rigidbody", false);
        ComponentsTypes.Add("Rigidbody", typeof(Rigidbody));
        ComponentsTypes.Add("BoxCollider", typeof(BoxCollider));
        ComponentsTypes.Add("SphereCollider", typeof(SphereCollider));
        ComponentsTypes.Add("MeshCollider", typeof(MeshCollider));


        // Physics 2D
        Physics2DComponents.Clear();
        Physics2DComponents.Add("BoxCollider 2D", false);
        Physics2DComponents.Add("CircleCollider 2D", false);
        Physics2DComponents.Add("CapsuleCollider 2D", false);
        Physics2DComponents.Add("CompositeCollider 2D", false);
        Physics2DComponents.Add("Rigidbody 2D", false);
        ComponentsTypes.Add("Rigidbody 2D", typeof(Rigidbody2D));
        ComponentsTypes.Add("BoxCollider 2D", typeof(BoxCollider2D));
        ComponentsTypes.Add("CircleCollider2D", typeof(CircleCollider2D));
        ComponentsTypes.Add("CapsuleCollider2D", typeof(CapsuleCollider2D));
        ComponentsTypes.Add("CompositeCollider2D", typeof(CompositeCollider2D));

        // Custom Scripts
        AllScripts.Clear();
        UnityEngine.Object[] scripts = Resources.LoadAll("Scripts");
        foreach (UnityEngine.Object script in scripts)
        {
            if (script.GetType().Equals(typeof(MonoScript)))
            {
                AllScripts.Add(script.name, true);
               // ComponentsTypes.Add(script.name, typeof(script.name));
               // TODO find a way to trully store the type of the script, and not as MonoScript.
            }
        }
    }


    private void OnGUI()
    {
        var styleCenter = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        GUILayout.Label("Choose the components to insert. Works with multiselected objects", styleCenter, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        physicsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(physicsGroup, "Physics Components");
        if (physicsGroup)
            foreach (string key in PhysicsComponents.Keys.ToList())
            {
                PhysicsComponents[key] = EditorGUILayout.ToggleLeft(key, PhysicsComponents[key]);
            }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space();

        physics2DGroup = EditorGUILayout.BeginFoldoutHeaderGroup(physics2DGroup, "Physics2D Components");
        if (physics2DGroup)
            foreach (string key in Physics2DComponents.Keys.ToList())
            {
                Physics2DComponents[key] = EditorGUILayout.ToggleLeft(key, Physics2DComponents[key]);
            }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space();


        // Custom Scripts
        /*
        GUILayout.Label("Custom Scripts (inside Resources/Scripts)", EditorStyles.boldLabel);
        scriptsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(scriptsGroup, "Resources Scripts");
        if (scriptsGroup)
        {
            foreach (string key in AllScripts.Keys.ToList())
            {
                AllScripts[key] = EditorGUILayout.ToggleLeft(key, AllScripts[key]);
            }
        }
        */


        EditorGUILayout.Space();
        if (GUILayout.Button("Add Components", GUILayout.MinWidth(100), GUILayout.MaxWidth(300)))
        {
            addComponents(PhysicsComponents);
            addComponents(Physics2DComponents);
           // addComponents(AllScripts);
        }

    }

    void addComponents(Dictionary<string, bool> dict)
    {
        foreach (var item in dict)
        {
            if (item.Value == true)
            {
                AddComponent(item.Key);
            }
        }
    }

    void AddComponent(string componentName)
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            var component = obj.GetComponent(componentName);
            if (component == null)
            {
                
                if (ComponentsTypes.ContainsKey(componentName))
                {
                   Type T = ComponentsTypes[componentName];
                   obj.AddComponent(T);
                }
            }
        }
    }


}
