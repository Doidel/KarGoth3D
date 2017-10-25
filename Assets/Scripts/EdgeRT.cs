using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class EdgeRT : MonoBehaviour {

    [HideInInspector]
    [SerializeField]
    private Camera _camera;
    private int _downResFactor = 1;

    private string _globalTextureName = "_GlobalEdgeTex";

    public Material buildingMaterial = null;

    private void Awake()
    {
        GenerateRT();
    }

    void OnPreRender()
    {
        //buildingMaterial.SetFloat("_Draw", 1);
        //Shader.SetGlobalFloat("_Draw", 1);
    }

    void GenerateRT()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.DepthNormals;

        if (_camera.targetTexture != null)
        {
            RenderTexture temp = _camera.targetTexture;
            _camera.targetTexture = null;
            DestroyImmediate(temp);
        }

        _camera.targetTexture = new RenderTexture(_camera.pixelWidth >> _downResFactor, _camera.pixelHeight >> _downResFactor, 16);
        _camera.targetTexture.filterMode = FilterMode.Bilinear;

        Shader.SetGlobalTexture(_globalTextureName, _camera.targetTexture);
    }
}
