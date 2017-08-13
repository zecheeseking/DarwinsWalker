using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HermiteSplineMonoBehaviour))]
public class HermiteSplineEditor : Editor
{
//    private List<ControlPointHandle> handles = new List<ControlPointHandle>();
    private HermiteSplineMonoBehaviour hSpline;
    private Transform hSplineTransform;
    private Quaternion hSplineRotation;

    private const int lineSteps = 50;

    private void OnSceneGUI()
    {
        hSpline = target as HermiteSplineMonoBehaviour;
        hSplineTransform = hSpline.transform;
        hSplineRotation = hSplineTransform.rotation;

        if (hSpline.Spline == null)
            return;

        Vector3 p0 = ShowPositionPoint(0);
        Vector3 p1 = ShowTangentPoint(0);
        for (int i = 1; i < hSpline.Spline.GetControlPointCount(); i++)
        {
            Vector3 p2 = ShowPositionPoint(i);
            Vector3 p3 = ShowTangentPoint(i);

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            p0 = p2;
            p1 = p3;
        }

        Handles.color = Color.white;
        Vector3 lineStart = hSpline.GetPoint(0f);
        for (int i = 1; i <= lineSteps; i++)
        {
            Vector3 lineEnd = hSpline.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(lineStart, lineEnd);
            lineStart = lineEnd;
        }
    }

    private Vector3 ShowPositionPoint(int index)
    {
        var cp = hSpline.Spline.GetSplinePointAt(index);
        Vector3 p = hSplineTransform.TransformPoint(cp.Position);
        EditorGUI.BeginChangeCheck();
        p = Handles.DoPositionHandle(p, hSplineRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hSpline, "Move Point");
            EditorUtility.SetDirty(hSpline);
            hSpline.Spline.SetControlPointAt(index, hSplineTransform.InverseTransformPoint(p), cp.Tangent);
        }

        return p;
    }

    private Vector3 ShowTangentPoint(int index)
    {
        var cp = hSpline.Spline.GetSplinePointAt(index);
        Vector3 p = hSplineTransform.TransformPoint(cp.Tangent);
        EditorGUI.BeginChangeCheck();
        p = Handles.DoPositionHandle(p, hSplineRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hSpline, "Move Point");
            EditorUtility.SetDirty(hSpline);
            hSpline.Spline.SetControlPointAt(index, cp.Position, hSplineTransform.InverseTransformPoint(p));
        }

        return p;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        hSpline = target as HermiteSplineMonoBehaviour;
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(hSpline, "Add Curve");
            hSpline.AddControlPoint();
            EditorUtility.SetDirty(hSpline);
        }
    }
}