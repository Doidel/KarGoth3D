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
    private int currentWidth;
    private int currentHeight;

    private string _globalTextureName = "_GlobalEdgeTex";

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

        _camera.targetTexture = new RenderTexture(_camera.pixelWidth * _downResFactor, _camera.pixelHeight * _downResFactor, 16);
        _camera.targetTexture.filterMode = FilterMode.Bilinear;

        Shader.SetGlobalTexture(_globalTextureName, _camera.targetTexture);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHeight != Screen.currentResolution.height || currentWidth != Screen.currentResolution.width)
        {
            currentHeight = Screen.height;
            currentWidth = Screen.width;
            GenerateRT();
        }

    }
}
