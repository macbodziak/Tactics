using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Node))]
[CanEditMultipleObjects]
public class NodeEditor : Editor
{
    SerializedProperty x;
    SerializedProperty y;
    SerializedProperty walkable;
    SerializedProperty character;

    void OnEnable()
    {
        x = serializedObject.FindProperty("_x");
        y = serializedObject.FindProperty("_y");
        walkable = serializedObject.FindProperty("myWalkable");
        character = serializedObject.FindProperty("myCharacter");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.LabelField("Position", x.intValue + "," + y.intValue);
        //in the future - rebake on change
        walkable.boolValue = EditorGUILayout.Toggle("Walkable", walkable.boolValue);

        HandleCharacterProperty();

        if (Selection.objects.Length == 1)
        {
            printEdges();
        }
        serializedObject.ApplyModifiedProperties();
    }

    void printEdges()
    {

        int length = 0;
        SerializedProperty nodes = serializedObject.FindProperty("_nodes");
        SerializedProperty costs = serializedObject.FindProperty("_costs");

        if (!nodes.isArray || !costs.isArray || nodes.arraySize == 0)
        {
            return;
        }
        
        nodes.Next(true);
        nodes.Next(true);
        costs.Next(true);
        costs.Next(true);
        length = nodes.intValue;
        nodes.Next(true);
        costs.Next(true);
        EditorGUILayout.LabelField("", GUILayout.MinHeight(15));
        EditorGUILayout.LabelField("Node:", "Cost:");
        int lastIndex = length - 1;
        for (int i = 0; i < length; i++)
        {
            EditorGUILayout.LabelField(nodes.objectReferenceValue.name, costs.intValue.ToString());
            if (i < lastIndex)
            {
                nodes.Next(false);
                costs.Next(false);
            }
        }
    }

    void HandleCharacterProperty()
    {
        character.objectReferenceValue = EditorGUILayout.ObjectField("Character", character.objectReferenceValue, typeof(Character), true);
        if (character.objectReferenceValue != null)
        {
            GUIContent content = new GUIContent("Update Character Reference", "Update The Node Refernece in this Character so it points to this Node");

            if (GUILayout.Button(content))
            {
                SerializedObject charSo = new SerializedObject(character.objectReferenceValue);
                
                charSo.FindProperty("_node").objectReferenceValue = serializedObject.targetObject;
                Character charObj = (Character)charSo.targetObject;
                charObj.transform.position = ((Node)target).transform.position;
                charSo.ApplyModifiedProperties();
            }
            // EditorGUILayout.LabelField("Character", character.objectReferenceValue.name);
        }
    }
}
