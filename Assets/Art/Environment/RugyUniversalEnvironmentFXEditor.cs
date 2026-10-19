#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>Editor buttons for testing Rugy's environment randomizer.</summary>
[CustomEditor(typeof(RugyUniversalEnvironmentFX))]
public class RugyUniversalEnvironmentFXEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(12f);
        EditorGUILayout.LabelField("Rugy Environment FX", EditorStyles.boldLabel);

        RugyUniversalEnvironmentFX controller = (RugyUniversalEnvironmentFX)target;

        if (GUILayout.Button("Randomize Environment Mood", GUILayout.Height(32f)))
        {
            Undo.RecordObject(controller, "Randomize Rugy Environment Mood");
            controller.RandomizeNow();
            EditorUtility.SetDirty(controller);
        }

        if (GUILayout.Button("Randomize Calm Courtyard"))
            controller.SetProfile(0);
        if (GUILayout.Button("Randomize Warm Embers"))
            controller.SetProfile(1);
        if (GUILayout.Button("Randomize Moonlit Void"))
            controller.SetProfile(2);
        if (GUILayout.Button("Randomize Thread Storm"))
            controller.SetProfile(3);
        if (GUILayout.Button("Randomize Dream Bloom"))
            controller.SetProfile(4);
    }
}
#endif
