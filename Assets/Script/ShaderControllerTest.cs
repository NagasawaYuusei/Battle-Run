using UnityEngine;

public class ShaderControllerTest : MonoBehaviour
{
    [SerializeField] Material _ClipMat;
    [SerializeField] GameObject _player;

    public void UpdateShaderParam()
    {
        Vector4 pos = _player.transform.position * -1;
        _ClipMat.SetVector("_ClipPosition", pos);
    }
}
