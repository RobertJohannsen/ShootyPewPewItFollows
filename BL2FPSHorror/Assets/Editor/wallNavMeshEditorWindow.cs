using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class wallNavMeshEditorWindow : EditorWindow
{
    GameObject wallNav;
    // Start is called before the first frame update
    
    [MenuItem("Tools/Level Tools")]
    public static void ShowWindow()
    {
        GetWindow(typeof(wallNavMeshEditorWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Tools", EditorStyles.boldLabel);

        if(GUILayout.Button("Create wall NavMesh"))
        {
            createWallNavMesh();
        }
    }

    private void createWallNavMesh()
    {
        Debug.Log("created thing");
    }
}
