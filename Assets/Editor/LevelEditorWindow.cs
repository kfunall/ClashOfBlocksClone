using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditorWindow : Editor
{
    const string drawButtonInfo =
    "\nUsing this custom editor you can design grid based level.\n\nThe scriptable object keep datas of the level units positions.\n\nSpecify the level units positions and add the scriptable object the list on the level data.\n\nGround -> The place where cubes are.\n\nBorder -> Walls around the grounds.\n";
    Level level;
    Vector3 mousePosition;
    SerializedProperty width, height;
    bool needsRepaint;
    string whichItem = "";
    private void OnEnable()
    {
        level = target as Level;
        SceneView.duringSceneGui += OnScene;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnScene;
    }
    private void OnScene(SceneView sceneView)
    {
        Event guiEvent = Event.current;
        if (guiEvent.type == EventType.Repaint)
            DrawLevel();
        else if (guiEvent.type == EventType.Layout)
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        else
        {
            HandleInput(guiEvent);
            if (needsRepaint)
                HandleUtility.Repaint();
        }
        EditorUtility.SetDirty(level);
    }
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();
        GUILayout.Label("Create Your Level", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GetReferances();
        DrawPropertFields(width, height);
        EditorGUILayout.Space();
        Buttons();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(drawButtonInfo, MessageType.None);
        EditorUtility.SetDirty(level);
    }
    private void GetReferances()
    {
        width = serializedObject.FindProperty("width");
        height = serializedObject.FindProperty("height");
    }
    private void DrawPropertFields(params SerializedProperty[] props)
    {
        for (int i = 0; i < props.Length; i++)
        {
            EditorGUILayout.PropertyField(props[i]);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space(1);
        }
    }
    private void Buttons()
    {
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Draw Width x Height Ground"))
            DrawGround();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Draw A Ground"))
            whichItem = "Ground";
        if (GUILayout.Button("Draw A Border"))
            whichItem = "Border";
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Draw Red Cube"))
            whichItem = "RedCube";
        if (GUILayout.Button("Draw Yellow Cube"))
            whichItem = "YellowCube";
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear Grounds"))
        {
            level.GroundPositions = new List<Vector3>();
            needsRepaint = true;
        }
        if (GUILayout.Button("Clear Borders"))
        {
            level.BorderPositions = new List<Vector3>();
            needsRepaint = true;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear Red Cubes"))
        {
            level.RedCubesPositions = new List<Vector3>();
            needsRepaint = true;
        }
        if (GUILayout.Button("Clear Yellow Cubes"))
        {
            level.YellowCubesPositions = new List<Vector3>();
            needsRepaint = true;
        }
        EditorGUILayout.EndHorizontal();
    }
    private void HandleInput(Event guiEvent)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float drawPlaneHeight = 0;
        float distanceToDrawPlane = (drawPlaneHeight - ray.origin.y) / ray.direction.y;
        mousePosition = ray.GetPoint(distanceToDrawPlane);
        mousePosition.x = Mathf.Round(mousePosition.x);
        mousePosition.z = Mathf.Round(mousePosition.z);
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
        {
            Undo.RecordObject(level, "Last Point");
            CheckWhichItem(mousePosition);
            needsRepaint = true;
        }
    }
    private void CheckWhichItem(Vector3 mousePos)
    {
        switch (whichItem)
        {
            case "Ground":
                if (level.BorderPositions.Contains(mousePos + Vector3.up * 0.5f))
                    level.BorderPositions.Remove(mousePos);
                if (!level.GroundPositions.Contains(mousePos))
                    level.GroundPositions.Add(mousePos);
                break;
            case "Border":
                if (level.GroundPositions.Contains(mousePos))
                    level.GroundPositions.Remove(mousePos);
                if (!level.BorderPositions.Contains(mousePos + Vector3.up * 0.5f))
                    level.BorderPositions.Add(mousePos + Vector3.up * 0.5f);
                break;
            case "RedCube":
                if (level.YellowCubesPositions.Contains(mousePos + Vector3.up * 0.125f))
                    level.YellowCubesPositions.Remove(mousePos + Vector3.up * 0.125f);
                if (!level.RedCubesPositions.Contains(mousePos + Vector3.up * 0.125f))
                    level.RedCubesPositions.Add(mousePos + Vector3.up * 0.125f);
                break;
            case "YellowCube":
                if (level.RedCubesPositions.Contains(mousePos + Vector3.up * 0.125f))
                    level.RedCubesPositions.Remove(mousePos + Vector3.up * 0.125f);
                if (!level.YellowCubesPositions.Contains(mousePos + Vector3.up * 0.125f))
                    level.YellowCubesPositions.Add(mousePos + Vector3.up * 0.125f);
                break;
        }
    }
    private void DrawGround()
    {
        for (int i = 0; i < level.Width; i++)
        {
            for (int j = 0; j < level.Height; j++)
            {
                Vector3 pos = new Vector3(i, 0f, j);
                if (!level.GroundPositions.Contains(pos))
                    level.GroundPositions.Add(pos);
            }
        }
        needsRepaint = true;
    }
    private void DrawLevel()
    {
        Handles.color = Color.magenta;
        for (int i = 0; i < level.GroundPositions.Count; i++)
        {
            Handles.RectangleHandleCap(i, level.GroundPositions[i], Quaternion.Euler(90f, 0f, 0f), 0.5f, EventType.Repaint);
        }
        Handles.color = Color.cyan;
        for (int j = 0; j < level.BorderPositions.Count; j++)
        {
            Handles.DrawWireCube(level.BorderPositions[j], Vector3.one);
        }
        Handles.color = Color.red;
        for (int j = 0; j < level.RedCubesPositions.Count; j++)
        {
            Handles.DrawWireCube(level.RedCubesPositions[j], Vector3.one);
        }
        Handles.color = Color.yellow;
        for (int j = 0; j < level.YellowCubesPositions.Count; j++)
        {
            Handles.DrawWireCube(level.YellowCubesPositions[j], Vector3.one);
        }
        needsRepaint = false;
    }
}