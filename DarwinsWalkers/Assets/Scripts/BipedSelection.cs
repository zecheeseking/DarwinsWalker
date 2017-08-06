using UnityEngine;

public class BipedSelection : MonoBehaviour
{
    public void IsSelected(bool selected)
    {
        var mrs = gameObject.GetComponentsInChildren<MeshRenderer>();

        if (selected)
        {
            foreach (MeshRenderer mr in mrs)
            {
                var mat = mr.material;
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f);
            }
        }
        else
        {
            foreach (MeshRenderer mr in mrs)
            {
                var mat = mr.material;
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.5f);
            }
        }
    }
}
