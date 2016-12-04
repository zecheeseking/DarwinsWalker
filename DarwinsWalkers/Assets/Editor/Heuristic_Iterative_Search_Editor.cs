using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HeuristicIterativeSearch_IK))]
public class Heuristic_Iterative_Search_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add bone", GUILayout.Width(100)))
        {
            var hisIK = (HeuristicIterativeSearch_IK)target;
            hisIK.AddBone(0.0f, 1.5f);

            EditorUtility.SetDirty(hisIK);
        }
        if (GUILayout.Button("Remove bone", GUILayout.Width(100)))
        {
            var hisIK = (HeuristicIterativeSearch_IK)target;
            hisIK.RemoveBone();

            EditorUtility.SetDirty(hisIK);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}