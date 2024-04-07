using UnityEditor;
using UnityEngine;

namespace Source.Scripts.Utils.Editor
{
    public class ForceSaveSceneAndProject : MonoBehaviour
    {
        [MenuItem("File/Save Scene And Project %#&s")]
        private static void FunctionForceSaveSceneAndProject()
        {
            EditorApplication.ExecuteMenuItem("File/Save");
            EditorApplication.ExecuteMenuItem("File/Save Project");
            Debug.Log("Saved scene and project");
        }
    }
}