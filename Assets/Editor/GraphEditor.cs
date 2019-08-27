using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Graph))]
public class GraphEditor : Editor
{
    bool isCreated = false;
    static int maxGridSize = 100;
    int graphWidth = 0;
    int graphHeight = 0;
    bool showDebug = false;
    Graph myGraph;
    // Texture2D tex;

    void OnEnable()
    {
        myGraph = (Graph)target;
        graphWidth = myGraph.width;
        graphHeight = myGraph.height;
        showDebug = myGraph.showDebugEdges;
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = myGraph.isEmpty;
        graphWidth = EditorGUILayout.IntField("Width: ", graphWidth);
        graphWidth = Mathf.Clamp(graphWidth, 0, maxGridSize);

        graphHeight = EditorGUILayout.IntField("Height: ", graphHeight);
        graphHeight = Mathf.Clamp(graphHeight, 0, maxGridSize);

        if (GUILayout.Button("Create New Graph"))
        {
            isCreated = true;
            myGraph.CreateNewGraph(graphWidth, graphHeight);
            EditorUtility.SetDirty(myGraph);
        }

        // tex = EditorGUILayout.ObjectField("texture", tex, typeof(Texture2D), false) as Texture2D;

        // GUI.enabled = myGraph.isEmpty && tex != null;
        // if (GUILayout.Button("Create New Graph From Texture"))
        // {
        //     isCreated = true;
        //     myGraph.CreateNewGraphFromTexture(tex);
        //     EditorUtility.SetDirty(myGraph);
        // }

        GUI.enabled = !myGraph.isEmpty;
        if (GUILayout.Button("Rebake Graph Button"))
        {
            myGraph.Rebake();
            EditorUtility.SetDirty(myGraph);
            Debug.Log("Rebaking Graph");
        }
        if (GUILayout.Button("Show Debug Edges"))
        {
            Color temp = Handles.color;
            Handles.color = Color.magenta;
            Graph.DrawDebugGraphInEditor(myGraph);
            Handles.color = temp;
        }
        if (GUILayout.Button("Delete Graph Button"))
        {
            Undo.RecordObject(myGraph, "Delete Graph");
            myGraph.DeleteGraph();
            EditorUtility.SetDirty(myGraph);
            isCreated = false;
            Debug.Log("Deleting Graph");
        }

        myGraph.showDebugEdges = GUILayout.Toggle(myGraph.showDebugEdges, "Debug: Show Edges");

    }

    private void OnSceneGUI() {
        if (myGraph.showDebugEdges)
        {
            ShowDebugEdges();
        }
    }

    void ShowDebugEdges()
    {
        Color temp = Handles.color;
        Handles.color = Color.magenta;
        Graph.DrawDebugGraphInEditor(myGraph);
        Handles.color = temp;
    }
}