using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] Material _ClipMat;
    [SerializeField] GameObject _go;
    Material _firstMaterial;
    float _value = 0;
    bool _currentScan;
    [SerializeField] float _scanTime = 4.0f;
    [SerializeField] ShaderControllerTest _sc;

    void Start()
    {
        _firstMaterial = _go.GetComponent<Material>();
    }

    void Update()
    {
        UpdateShaderParam();
    }
    void UpdateShaderParam()
    {
        if (_currentScan && _value < _scanTime)
        {
            _value += Time.deltaTime;
            _ClipMat.SetFloat("_Flag", _value);
        }


        if (_value > _scanTime)
        {
            ResetValue();
            _currentScan = false;
        }
    }

    void ResetValue()
    {
        _go.GetComponent<MeshRenderer>().material = _firstMaterial;
        _value = 0;
    }

    public void Scan()
    {
        _go.GetComponent<MeshRenderer>().material = _ClipMat;
    }
}
