using UnityEngine;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{
    [SerializeField] Material _ClipMat;
    [SerializeField] GameObject _go;
    Material _firstMaterial;
    float _value = 0;
    bool _currentScan;
    [SerializeField] float _scanTime = 4.0f;
    [SerializeField] ShaderControllerTest _sc;
    float _timer;
    nav _nav;

    void Start()
    {
        _firstMaterial = _go.GetComponent<MeshRenderer>().material;
        _nav = FindObjectOfType<nav>();
    }

    void Update()
    {
        UpdateShaderParam();
    }
    void UpdateShaderParam()
    {
        if (_currentScan && _timer < _scanTime)
        {
            _timer += Time.deltaTime;
            _value += Time.deltaTime;
            _ClipMat.SetFloat("_Flag", _value);
        }


        if (_currentScan && _timer >= _scanTime)
        {
            _timer = 0;
            ResetValue();
            _currentScan = false;
        }
    }

    void ResetValue()
    {
        _go.GetComponent<MeshRenderer>().material = _firstMaterial;
        _value = 0;
        _nav.NavLine(false);
    }

    void Scan()
    {
        _sc.UpdateShaderParam();
        _go.GetComponent<MeshRenderer>().material = _ClipMat;
        _currentScan = true;
        _nav.NavLine(true);
    }

    public void PlayerScan(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Scan();
        }
    }
}
