using UnityEngine;

public class SkillController : MonoBehaviour
{
    bool _moveTip;
    Vector2[] _tipPotisions;
    RectTransform _rt;
    [SerializeField] int _size = 50;

    void Start()
    {
        SetUp();
        SetTipPosition();
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        MouseMove();
    }

    void SetUp()
    {
        _rt = GetComponent<RectTransform>();
        _tipPotisions = new Vector2[transform.childCount];
    }

    private void MouseMove()
    {
        if (Input.GetMouseButtonDown(0) && !_moveTip)
        {
            Vector3 mousePosition = Input.mousePosition;
            if (MouseCheck(mousePosition))
            {
                _moveTip = true;
            }
        }
        else if (Input.GetMouseButtonDown(0) && _moveTip)
        {
            SetTipPosition();
            _moveTip = false;
        }

        if (_moveTip)
        {
            Vector3 mousePosition = Input.mousePosition;
            _rt.anchoredPosition = mousePosition;
        }
    }

    void SetTipPosition()
    {
        for (int i = 0; i < _tipPotisions.Length; i++)
        {
            _tipPotisions[i] = transform.GetChild(i).GetComponent<RectTransform>().localPosition;
            _tipPotisions[i] = new Vector2(_rt.anchoredPosition.x + _tipPotisions[i].x, _rt.anchoredPosition.y + _tipPotisions[i].y);
        }
    }

    bool MouseCheck(Vector2 vec)
    {
        for (int i = 0; i < _tipPotisions.Length; i++)
        {
            if (_tipPotisions[i].x + (_size / 2) > vec.x && vec.x > _tipPotisions[i].x - (_size / 2))
            {
                if (_tipPotisions[i].y + (_size / 2) > vec.y && vec.y > _tipPotisions[i].y - (_size / 2))
                { 
                    return true;
                }
            }
        }

        return false;
    }
}