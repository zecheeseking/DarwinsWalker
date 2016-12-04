using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Link
{
    public Vector2 StartPos;
    public float Angle;

    [Range(0.1f, 50.0f)]
    public float Length;
}

public class HeuristicIterativeSearch_IK : MonoBehaviour
{
    private const int MAX_ITERATIONS = 100;
    private const float IK_THRESHOLD = 0.1f;
    public List<Link> _links = new List<Link>();

    public Mesh m_LinkMesh;

    private Vector2 _targetPos = Vector2.zero;

    // Update is called once per frame
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            _targetPos = new Vector2(hit.point.x, hit.point.y);
        }

        UpdateCCDIK();
    }

    public void AddBone(float angle, float length)
    {
        Link l = new Link();

        if (_links.Count != 0)
        {
            Link tmp = _links[_links.Count - 1];
            Vector3 start = Vector3.zero;
            start.x = tmp.StartPos.x + Mathf.Cos(tmp.Angle) * tmp.Length;
            start.y = tmp.StartPos.y + Mathf.Sin(tmp.Angle) * tmp.Length;
            l.StartPos = start;
        }
        else
            l.StartPos = Vector2.zero;

        l.Angle = angle * Mathf.Deg2Rad;
        l.Length = length;

        _links.Add(l);
    }

    public void RemoveBone()
    {
        if (_links.Count != 0)
        {
            _links.RemoveAt(_links.Count - 1);
        }
    }

    private void UpdateCCDIK()
    {
        int numLinks = _links.Count;

        if (numLinks == 0)
            return;

        int itCount = 0;

        //Loop through links
        for (int i = numLinks - 1; i >= 0; --i)
        {
            Link endLink = _links[numLinks - 1];
            Vector2 endEffectorPos = Vector2.zero;
            endEffectorPos.x = endLink.StartPos.x + Mathf.Cos(endLink.Angle) * endLink.Length;
            endEffectorPos.y = endLink.StartPos.y + Mathf.Sin(endLink.Angle) * endLink.Length;

            float error = (_targetPos - endEffectorPos).magnitude;
            if (error < 0.0001f)
                break;

            Link curLink = _links[i];
            Vector2 curStartPoint = curLink.StartPos;
            Vector2 curEndPoint = Vector2.zero;
            curEndPoint.x = curStartPoint.x + Mathf.Cos(curLink.Angle) * curLink.Length;
            curEndPoint.y = curStartPoint.y + Mathf.Sin(curLink.Angle) * curLink.Length;

            Vector2 curToEndEffector = endEffectorPos - curStartPoint;
            Vector2 curToTarget = _targetPos - curStartPoint;

            curToEndEffector.Normalize();
            curToTarget.Normalize();

            float dot = Vector2.Dot(curToTarget, curToEndEffector);
            float ang = Mathf.Acos(dot);

            if (Math.Abs(ang) > IK_THRESHOLD)
            {
                float dir = curToTarget.x * curToEndEffector.y - curToTarget.y * curToEndEffector.x;

                Link l = new Link();
                l.StartPos = _links[i].StartPos;
                l.Length = _links[i].Length;

                if (dir > 0.0f)
                    l.Angle = _links[i].Angle - ang;
                else if (dir < 0.0f)
                    l.Angle = _links[i].Angle + ang;


                _links[i] = l;

                //BOUNCE BACK Applies a bias towards the end links
                i = numLinks - 1;
            }

            itCount++;
            if (itCount > MAX_ITERATIONS)
                break;

            UpdateChain();
        }
    }

    private void UpdateChain()
    {
        for (int i = 1; i < _links.Count; ++i)
        {
            Link tmp = new Link();
            Link prevLink = _links[i - 1];

            Vector2 start = Vector2.zero;
            start.x = prevLink.StartPos.x + Mathf.Cos(prevLink.Angle) * prevLink.Length;
            start.y = prevLink.StartPos.y + Mathf.Sin(prevLink.Angle) * prevLink.Length;
            tmp.StartPos = start;
            tmp.Angle = _links[i].Angle;
            tmp.Length = _links[i].Length;

            _links[i] = tmp;
        }
    }

    private void OnDrawGizmos()
    {
        float curAngle = 0.0f;
        for (int linkId = 0; linkId < _links.Count; ++linkId)
        {
            Vector2 start = _links[linkId].StartPos;
            curAngle = _links[linkId].Angle;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(start, 0.3f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireMesh(m_LinkMesh, start, Quaternion.Euler(0.0f, 0.0f, curAngle * Mathf.Rad2Deg), new Vector3(_links[linkId].Length, 0.3f, 0.3f));
            if (linkId == _links.Count - 1)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(new Vector2(start.x + Mathf.Cos(curAngle) * _links[linkId].Length, start.y + Mathf.Sin(curAngle) * _links[linkId].Length), 0.3f);
            }
        }
    }
}