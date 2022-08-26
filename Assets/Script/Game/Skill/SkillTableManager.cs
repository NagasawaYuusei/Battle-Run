using UnityEngine;
using UnityEngine.UI;

public class SkillTableManager : MonoBehaviour
{
    [SerializeField] GameObject _tableObject;
    Vector2[,] _tableTransforms;
    [SerializeField] static int _horizontalTipNum = 4;
    [SerializeField] static int _verticalTipNum = 4;
    [SerializeField] int _size = 50;
    [SerializeField] Sprite _sprite;
    bool _isSkillTipMove;

    Transform[,] grid = new Transform[_horizontalTipNum, _verticalTipNum];

    public bool IsSkillTipMove => _isSkillTipMove;

    void Start()
    {
        SetTableTip();
    }

    void SetTableTip()
    {
        _tableTransforms = new Vector2[_verticalTipNum, _horizontalTipNum];
        int[] horizontal = new int[_horizontalTipNum];
        int[] vertical = new int[_verticalTipNum];
        if (_horizontalTipNum % 2 == 0)
        {
            for(int i = 0; i < _horizontalTipNum; i++)
            {
                horizontal[i] = ((-1) * (_horizontalTipNum / 2) * _size) + (_size / 2) + (_size * i);
            }
        }
        else
        {
            for (int i = 0; i < _horizontalTipNum; i++)
            {
                horizontal[i] = ((-1) * (_horizontalTipNum / 2) * _size) + (_size * i);
            }
        }

        if(_verticalTipNum % 2 == 0)
        {
            for (int i = 0; i < _verticalTipNum; i++)
            {
                vertical[i] = ((_verticalTipNum / 2) * _size) - (_size / 2) - (_size * i);
            }
        }
        else
        {
            for (int i = 0; i < _verticalTipNum; i++)
            {
                vertical[i] = ((_verticalTipNum / 2) * _size) - (_size * i);
            }
        }

        for (int v = 0; v < _verticalTipNum; v++)
        {
            for (int h = 0; h < _horizontalTipNum; h++)
            {
                GameObject tableTip = new GameObject($"TableTip({v + 1}, {h + 1})");
                tableTip.transform.parent = _tableObject.transform;
                Image image = tableTip.AddComponent<Image>();
                image.sprite = _sprite;

                RectTransform tableTipTransform = tableTip.GetComponent<RectTransform>();
                tableTipTransform.sizeDelta = new Vector2(_size, _size);
                tableTipTransform.localPosition = new Vector2(horizontal[h], vertical[v]);
                _tableTransforms[v, h] = tableTipTransform.localPosition;
                _tableTransforms[v, h] = new Vector2(_tableTransforms[v, h].x + _tableObject.GetComponent<RectTransform>().anchoredPosition.x,
                    _tableTransforms[v, h].y + _tableObject.GetComponent<RectTransform>().anchoredPosition.y);
            }
        }
    }

    public int[] SerchSet(Vector2 mousePos)
    {
        for (int v = 0; v < _verticalTipNum; v++)
        {
            for (int h = 0; h < _horizontalTipNum; h++)
            {
                if (_tableTransforms[v,h].x + (_size / 2) > mousePos.x && mousePos.x > _tableTransforms[v,h].x - (_size / 2))
                {
                    if (_tableTransforms[v,h].y + (_size / 2) > mousePos.y && mousePos.y > _tableTransforms[v,h].y - (_size / 2))
                    {
                        return new int[] {v,h};
                    }
                }
            }
        }

        return null;
    }

    public Vector2 TableTip(int[] tipNums)
    {
        return _tableTransforms[tipNums[0],tipNums[1]];
    }

    public void ChangeMoveState(bool state)
    {
        _isSkillTipMove = state;
    }
}

public class SkillState
{
    TipState[] m_tipStates;
    public enum Skill
    {
        None,
        Skill1,
        Skill2,
        Skill3,
        Skill4,
    };

    Skill m_skill = Skill.None;

    void TipStatesSet(int tipNums)
    {
        m_tipStates = new TipState[tipNums];
    }
}

public class TipState
{
    int[] m_tipPos = new int[2];
}