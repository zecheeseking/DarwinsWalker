using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HermiteSpline))]
public class HermiteSplineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("Add Control Point", GUILayout.Width(150)))
        {
            var hSpline = (HermiteSpline)target;
            hSpline.AddControlPoint();

            EditorUtility.SetDirty(hSpline);

        }
        if (GUILayout.Button("Refresh Spline", GUILayout.Width(100)))
        {
            var hSpline = (HermiteSpline)target;
            hSpline.RefreshSplinePoints();

            EditorUtility.SetDirty(hSpline);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void OnSceneGUI()
    {
        var hSpline = (HermiteSpline)target;

        for(int i = 0; i < hSpline.splineControlPoints.Count; ++i)
        {
            HermiteSplineControlPoint hcp = hSpline.splineControlPoints[i];
            Vector3 startMove = hcp.position;
            hcp.position = Handles.FreeMoveHandle(hcp.position, Quaternion.identity, 1f, new Vector3(1f, 1f, 1f), Handles.RectangleCap);
            hcp.tangent = Handles.FreeMoveHandle(hcp.tangent, Quaternion.identity, .5f, new Vector3(1f, 1f, 1f), Handles.RectangleCap);
            hcp.tangent += (hcp.position - startMove);
            hSpline.splineControlPoints[i] = hcp;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(hSpline);
            hSpline.RefreshSplinePoints();
        }
    }
}