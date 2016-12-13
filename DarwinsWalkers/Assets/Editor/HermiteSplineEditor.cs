//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;

//struct ControlPointHandle
//{
//    public ControlPointHandle(Vector3 pos, Vector3 tan)
//    {
//        this.pos = pos;
//        this.tan = tan;
//    }

//    public Vector3 pos;
//    public Vector3 tan;
//}

//[CustomEditor(typeof(HermiteSpline))]
//public class HermiteSplineEditor : Editor
//{
//    private List<ControlPointHandle> handles = new List<ControlPointHandle>();

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        GUILayout.BeginHorizontal();
//        GUILayout.FlexibleSpace();
//        if(GUILayout.Button("Add CP", GUILayout.Width(100)))
//        {
//            var hSpline = (HermiteSpline)target;
//            AddHandle();
//            hSpline.AddControlPoint(handles[handles.Count - 1].pos, handles[handles.Count - 1].tan);

//            EditorUtility.SetDirty(hSpline);

//        }
//        if (GUILayout.Button("Remove CP", GUILayout.Width(100)))
//        {
//            var hSpline = (HermiteSpline)target;
//            handles.RemoveAt(handles.Count - 1);
//            hSpline.RemoveControlPoint();

//            EditorUtility.SetDirty(hSpline);
//        }
//        if (GUILayout.Button("Refresh Spline", GUILayout.Width(100)))
//        {
//            var hSpline = (HermiteSpline)target;
//            hSpline.RefreshSplinePoints();

//            EditorUtility.SetDirty(hSpline);
//        }
//        GUILayout.FlexibleSpace();
//        GUILayout.EndHorizontal();
//    }

//    private void AddHandle()
//    {
//        if (handles.Count == 0)
//            handles.Add(new ControlPointHandle(new Vector3(0, 0, 0), new Vector3(1, 0, 0)));
//        else
//        {
//            var newPos = handles[handles.Count - 1].pos + new Vector3(3,0,0);
//            handles.Add(new ControlPointHandle(newPos, newPos + new Vector3(1,0,0)));
//        }
//    }

//    private void OnSceneGUI()
//    {
//        var hSpline = (HermiteSpline)target;
        
//        for(int i = 0; i < handles.Count; ++i)
//        {
//            HermiteSplineControlPoint hcp = hSpline.GetSplinePointAt(i);
//            Vector3 startMove = hcp.Position;
//            var pos = Handles.FreeMoveHandle(handles[i].pos, Quaternion.identity, 1f, new Vector3(1f, 1f, 1f), Handles.RectangleCap);
//            var tan = Handles.FreeMoveHandle(handles[i].tan, Quaternion.identity, .5f, new Vector3(1f, 1f, 1f), Handles.RectangleCap);
//            handles[i] = new ControlPointHandle(pos, tan);
//            hSpline.SetControlPointAt(i, handles[i].pos, handles[i].tan);
//        }

//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(hSpline);
//            hSpline.RefreshSplinePoints();
//        }
//    }
//}