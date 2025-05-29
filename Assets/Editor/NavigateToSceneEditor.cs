using UnityEditor;

[CustomEditor(typeof(NavigateToScene), true)]
public class NavigateToSceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = target as NavigateToScene;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(script.Scene);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("Scene");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
