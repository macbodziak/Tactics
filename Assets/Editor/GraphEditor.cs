using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Graph))]
public class GraphEditor : Editor
{
    bool isCreated;
    static int maxGridSize = 100;
    int graphWidth = 0;
    int graphHeight = 0;

    void OnEnable()
    {
        Graph myGraph = (Graph)target;
        graphWidth = myGraph.width;
        graphHeight = myGraph.height;
    }

    public override void OnInspectorGUI()
    {
        Graph myGraph = (Graph)target;
        GUI.enabled = myGraph.isEmpty;
        graphWidth = EditorGUILayout.IntField("Width: ", graphWidth);
        graphWidth = Mathf.Clamp(graphWidth, 0, maxGridSize);

        graphHeight = EditorGUILayout.IntField("Height: ", graphHeight);
        graphHeight = Mathf.Clamp(graphHeight, 0, maxGridSize);
        // EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
        // EditorGUILayout.HelpBox("PLACEHOLDER!!! - Map needs rebaking", MessageType.Warning);


        if (GUILayout.Button("Create New Graph"))
        {
            isCreated = true;
            myGraph.CreateNewGraph(graphWidth, graphHeight);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        GUI.enabled = !myGraph.isEmpty;
        if (GUILayout.Button("Rebake Graph Button"))
        {
            myGraph.Rebake();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log("Rebaking Graph");
        }
        if (GUILayout.Button("Placeholder Button"))
        {
            Debug.Log("Placeholder Button Pressed - not yet implemented");
        }
        if (GUILayout.Button("Delete Graph Button"))
        {
            myGraph.DeleteGraph();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            isCreated = false;
            Debug.Log("Deleting Graph");
        }
    }
}