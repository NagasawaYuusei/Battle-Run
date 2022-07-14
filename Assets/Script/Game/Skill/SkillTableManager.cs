using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTableManager : MonoBehaviour
{
    [SerializeField] GameObject _tableObject;
    Vector2[] _tableTransforms;
    [SerializeField] int _size = 50;
    bool _isSkillTipMove;
    public bool IsSkillTipMove => _isSkillTipMove;

    void Start()
    {
        _tableTransforms = new Vector2[_tableObject.transform.childCount];
        for(int i = 0; i < _tableTransforms.Length; i++)
        {
            _tableTransforms[i] = _tableObject.transform.GetChild(i).GetComponent<RectTransform>().localPosition;
            _tableTransforms[i] = new Vector2(_tableTransforms[i].x + _tableObject.GetComponent<RectTransform>().anchoredPosition.x,
                _tableTransforms[i].y + _tableObject.GetComponent<RectTransform>().anchoredPosition.y);

        }
    }

    public int SerchSet(Vector2 mousePos)
    {
        for (int i = 0; i < _tableTransforms.Length; i++)
        {
            if (_tableTransforms[i].x + (_size / 2) > mousePos.x && mousePos.x > _tableTransforms[i].x - (_size / 2))
            {
                if (_tableTransforms[i].y + (_size / 2) > mousePos.y && mousePos.y > _tableTransforms[i].y - (_size / 2))
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public Vector2 SetTableTip(int tipNum)
    {
        return _tableTransforms[tipNum];
    }

    public void ChangeMoveState(bool state)
    {
        _isSkillTipMove = state;
    }
}
