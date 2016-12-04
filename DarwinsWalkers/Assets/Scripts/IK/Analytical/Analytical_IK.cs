using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Links
{
    public Vector2 StartPos;
    public Vector2 MiddlePos;
    public Vector2 EndPos;
    public float Length1;
    public float Length2;
}

public class Analytical_IK : MonoBehaviour
{
    public Links links;
    private Vector2 _targetPosition = Vector2.zero;

    public Mesh m_LinkMesh;

    public void Awake()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            _targetPosition = new Vector2(hit.point.x, hit.point.y);
        }

        float l0 = links.Length1;
        float l1 = links.Length2;

        float l = (float)Mathf.Sqrt(_targetPosition.x * _targetPosition.x + _targetPosition.y * _targetPosition.y);

        float a = Mathf.Acos(Mathf.Clamp((l0 * l0 + l1 * l1 - l * l) / (2 * l0 * l1), -1.0f, 1.0f));
        float b = Mathf.Acos(Mathf.Clamp((l0 * l0 + l * l - l1 * l1) / (2 * l0 * l), -1.0f, 1.0f));
        float c = Mathf.Atan2(_targetPosition.y, _targetPosition.x);

        Vector2 p0 = new Vector2(l0 * Mathf.Cos(b + c), l0 * Mathf.Sin(b + c));
        Vector2 p1 = new Vector2(p0.x + l1 * Mathf.Cos(b + c + 3.14f + a), p0.y + l1 * Mathf.Sin(b + c + 3.14f + a));
        links.MiddlePos = p0;
        links.EndPos = p1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(links.StartPos, links.MiddlePos);
        Gizmos.DrawLine(links.MiddlePos, links.EndPos);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(links.MiddlePos, 0.5f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(links.EndPos, 0.5f);
    }
}