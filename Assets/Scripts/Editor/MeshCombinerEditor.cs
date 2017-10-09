using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(hMeshCombiner))]
public class MeshCombinerEditor : Editor
{
    void OnSceneGUI()
    {
        hMeshCombiner mc = hMeshCombiner.Instance;
        if (Handles.Button(mc.transform.position+Vector3.up * 5, Quaternion.LookRotation(Vector3.up), 1, 1, Handles.CylinderCap))
        {
            var t = (target as hMeshCombiner).gameObject;
            mc.Combine(t, t.transform, false);
        }
    }
}
