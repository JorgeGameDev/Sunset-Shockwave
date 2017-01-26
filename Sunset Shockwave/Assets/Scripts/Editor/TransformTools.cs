// Created by Jorge Carvalho
// Allows for easy rounding of values in selected objects transforms.

using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
[ExecuteInEditMode]
public class TransformTools : EditorWindow
{

    [MenuItem("Window/Custom Tools/Transform Help")]
    // Called when the window is shown in the editor.
    private static void ShowWindow()
    {
        TransformTools windowHandle = (TransformTools)EditorWindow.GetWindow(typeof(TransformTools));
        GUIContent titleContent = new GUIContent("Transform Help");
        windowHandle.titleContent = titleContent;
        windowHandle.autoRepaintOnSceneChange = true;
    }

    // Called when the inspector is updated.
    void OnInspectorUpdate()
    {
        Repaint();
    }

    // Called everytime the GUI is open.
    void OnGUI()
    {
        // Gets the current selected transforms.
        Transform[] transformSelected = Selection.transforms;

        // Checks if there are transforms selected.
        if(transformSelected.Length > 0)
        {
            // Starts the UI Layout.
            GUILayout.BeginHorizontal();

            // Centers content to the middle.
            GUIStyle centeredStyle = new GUIStyle("label");
            centeredStyle.alignment = TextAnchor.MiddleCenter;

            // Draw each editor piece.
            PositionGroup(transformSelected, centeredStyle);
            ScaleGroup(transformSelected, centeredStyle);

            GUILayout.EndHorizontal();
        }
    }

    // Draws the Position Group on UI.
    static void PositionGroup(Transform[] transformSelected, GUIStyle centeredStyle)
    {
        // Starts Vertical Layout
        GUILayout.BeginVertical();
        GUILayout.Label("Position", centeredStyle);

        // Used to Reset Position.
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Reset"))
        {
            foreach (Transform transform in transformSelected)
            {
                transform.position = Vector3.zero;
            }
        }

        GUILayout.Label("Round", centeredStyle);
        GUI.backgroundColor = Color.white;
        // Used to Round to Units.
        if (GUILayout.Button("Units"))
        {
            foreach (Transform transform in transformSelected)
            {
                Vector3 roundedTransform = transform.position;
                roundedTransform.x = Mathf.Round(roundedTransform.x);
                roundedTransform.y = Mathf.Round(roundedTransform.y);
                roundedTransform.z = Mathf.Round(roundedTransform.z);
                transform.position = roundedTransform;
            }
        }

        // Used to round decimals.
        if (GUILayout.Button("Decimals"))
        {
            foreach (Transform transform in transformSelected)
            {
                Vector3 roundedTransform = transform.position;
                roundedTransform.x = Mathf.Round(roundedTransform.x * 10) / 10;
                roundedTransform.y = Mathf.Round(roundedTransform.y * 10) / 10;
                roundedTransform.z = Mathf.Round(roundedTransform.z * 10) / 10;
                transform.position = roundedTransform;
            }
        }

        // Used to round hundreths.
        if (GUILayout.Button("Hundreths"))
        {
            foreach (Transform transform in transformSelected)
            {
                Vector3 roundedTransform = transform.position;
                roundedTransform.x = Mathf.Round(roundedTransform.x * 100) / 100;
                roundedTransform.y = Mathf.Round(roundedTransform.y * 100) / 100;
                roundedTransform.z = Mathf.Round(roundedTransform.z * 100) / 100;
                transform.position = roundedTransform;
            }
        }

        // Ends Vertical Layout
        GUILayout.EndVertical();
    }

    // Draws the Position Group on UI.
    /*static void RotationGroup(Transform[] transformSelected, GUIStyle centeredStyle)
    {
        // Starts Vertical Layout
        GUILayout.BeginVertical();
        GUILayout.Label("Rotation", centeredStyle);

        // Used to Reset Position.
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Reset"))
        {
            foreach (Transform transform in transformSelected)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }

        GUILayout.Label("Round", centeredStyle);
        GUI.backgroundColor = Color.white;
        // Used to Round to Units.
        if (GUILayout.Button("Units"))
        {
            foreach (Transform transform in transformSelected)
            {
                Vector3 roundedTransform = transform.eulerAngles;
                roundedTransform.x = Mathf.Round(transform.eulerAngles.x);
                roundedTransform.y = Mathf.Round(transform.eulerAngles.y);
                roundedTransform.z = Mathf.Round(transform.eulerAngles.z);
                transform.rotation = Quaternion.Euler(roundedTransform);
            }
        }

        // Used to round decimals.
        if (GUILayout.Button("Decimals"))
        {
            foreach (Transform transform in transformSelected)
            {

            }
        }

        // Used to round hundreths.
        if (GUILayout.Button("Hundreths"))
        {
            foreach (Transform transform in transformSelected)
            {

            }
        }

        // Ends Vertical Layout
        GUILayout.EndVertical();
    }*/

    // Draws the Scale Group on UI.
    static void ScaleGroup(Transform[] transformSelected, GUIStyle centeredStyle)
    {
        // Starts Vertical Layout
        GUILayout.BeginVertical();
        GUILayout.Label("Scale", centeredStyle);

        // Used to Reset Position.
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Reset"))
        {
            foreach (Transform transform in transformSelected)
            {
                transform.localScale = Vector3.one;
            }
        }

        GUILayout.Label("Round", centeredStyle);
        GUI.backgroundColor = Color.white;
        // Used to Round to Units.
        if (GUILayout.Button("Units"))
        {
            foreach (Transform transform in transformSelected)
            {
                Vector3 roundedTransform = transform.localScale;
                roundedTransform.x = Mathf.Round(roundedTransform.x);
                roundedTransform.y = Mathf.Round(roundedTransform.y);
                roundedTransform.z = Mathf.Round(roundedTransform.z);
                transform.localScale = roundedTransform;
            }
        }

        // Used to round decimals.
        if (GUILayout.Button("Decimals"))
        {
            foreach (Transform transform in transformSelected)
            {
                Vector3 roundedTransform = transform.localScale;
                roundedTransform.x = Mathf.Round(roundedTransform.x * 10) / 10;
                roundedTransform.y = Mathf.Round(roundedTransform.y * 10) / 10;
                roundedTransform.z = Mathf.Round(roundedTransform.z * 10) / 10;
                transform.localScale = roundedTransform;
            }
        }

        // Used to round hundreths.
        if (GUILayout.Button("Hundreths"))
        {
            foreach (Transform transform in transformSelected)
            {
                Vector3 roundedTransform = transform.localScale;
                roundedTransform.x = Mathf.Round(roundedTransform.x * 100) / 100;
                roundedTransform.y = Mathf.Round(roundedTransform.y * 100) / 100;
                roundedTransform.z = Mathf.Round(roundedTransform.z * 100) / 100;
                transform.localScale = roundedTransform;transform.localScale = roundedTransform;
            }
        }

        // Ends Vertical Layout
        GUILayout.EndVertical();
    }
}
