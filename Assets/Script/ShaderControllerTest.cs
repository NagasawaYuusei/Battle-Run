using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderControllerTest : MonoBehaviour
{
    [SerializeField] Material _ClipMat;

    void Update()
    {
        UpdateShaderParam();
    }
    void UpdateShaderParam()
    {
        Vector4 pos = transform.position * -1;
        _ClipMat.SetVector("_ClipPosition", pos);
    }
}
