using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スキルが置かれてある初期位置に付けるマネージャー
/// </summary>
public class SkillTableManager : MonoBehaviour
{
    [Tooltip("スキルテーブル"), SerializeField] GameObject _tableObject;
    [Tooltip("スキルテーブルの大きさ")] Vector2[,] _tableTransforms;
    [Tooltip("スキルテーブルの横幅"), SerializeField] static int _horizontalTipNum = 4;
    [Tooltip("スキルテーブルの立幅"), SerializeField] static int _verticalTipNum = 4;
    [Tooltip("スキルテーブル一つのサイズ")] int _size = 50;
    [Tooltip("スキルテーブル一つに割り当てる絵"), SerializeField] Sprite _sprite;
    [Tooltip("スキルチップを動かしているかどうか")] bool _isSkillTipMove;

    bool[,] _isTips = new bool[_verticalTipNum, _horizontalTipNum];

    //カプセル化
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
            for (int i = 0; i < _horizontalTipNum; i++)
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

        if (_verticalTipNum % 2 == 0)
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

    public int[] SerchSet(Vector2[] tipPositions)
    {
        int[,] tablePoss = new int[2, tipPositions.Length];
        int isTableCount = 0;
        for (int i = 0; i < tipPositions.Length; i++)
        {
            for (int v = 0; v < _verticalTipNum; v++)
            {
                for (int h = 0; h < _horizontalTipNum; h++)
                {
                    if (!_isTips[v, h])
                    {
                        if (_tableTransforms[v, h].x + (_size / 2) > tipPositions[i].x && tipPositions[i].x > _tableTransforms[v, h].x - (_size / 2))
                        {
                            if (_tableTransforms[v, h].y + (_size / 2) > tipPositions[i].y && tipPositions[i].y > _tableTransforms[v, h].y - (_size / 2))
                            {
                                tablePoss[0, i] = v;
                                tablePoss[1, i] = h;
                                isTableCount++;
                            }
                        }
                    }
                }
            }
        }

        if (isTableCount == tipPositions.Length)
        {
            for (int i = 0; i < tipPositions.Length; i++)
            {
                _isTips[tablePoss[0, i], tablePoss[1, i]] = true;
            }
            Debug.Log(_isTips[0, 0] + "," + _isTips[0, 1] + "," + _isTips[0, 2] + "," + _isTips[0, 3]);
            Debug.Log(_isTips[1, 0] + "," + _isTips[1, 1] + "," + _isTips[1, 2] + "," + _isTips[1, 3]);
            Debug.Log(_isTips[2, 0] + "," + _isTips[2, 1] + "," + _isTips[2, 2] + "," + _isTips[2, 3]);
            Debug.Log(_isTips[3, 0] + "," + _isTips[3, 1] + "," + _isTips[3, 2] + "," + _isTips[3, 3]);
            return new int[] { tablePoss[0, 0], tablePoss[1, 0] };
        }

        return null;
    }

    public void ClearTable(Vector2[] tipPositions)
    {
        int[,] tablePoss = new int[2, tipPositions.Length];
        for (int i = 0; i < tipPositions.Length; i++)
        {
            for (int v = 0; v < _verticalTipNum; v++)
            {
                for (int h = 0; h < _horizontalTipNum; h++)
                {
                    if (_tableTransforms[v, h].x + (_size / 2) > tipPositions[i].x && tipPositions[i].x > _tableTransforms[v, h].x - (_size / 2))
                    {
                        if (_tableTransforms[v, h].y + (_size / 2) > tipPositions[i].y && tipPositions[i].y > _tableTransforms[v, h].y - (_size / 2))
                        {
                            tablePoss[0, i] = v;
                            tablePoss[1, i] = h;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < tipPositions.Length; i++)
        {
            _isTips[tablePoss[0, i], tablePoss[1, i]] = false;
        }
        Debug.Log(_isTips[0, 0] + "," + _isTips[0, 1] + "," + _isTips[0, 2] + "," + _isTips[0, 3]);
        Debug.Log(_isTips[1, 0] + "," + _isTips[1, 1] + "," + _isTips[1, 2] + "," + _isTips[1, 3]);
        Debug.Log(_isTips[2, 0] + "," + _isTips[2, 1] + "," + _isTips[2, 2] + "," + _isTips[2, 3]);
        Debug.Log(_isTips[3, 0] + "," + _isTips[3, 1] + "," + _isTips[3, 2] + "," + _isTips[3, 3]);
    }

    public Vector2 TableTip(int[] tipNums)
    {
        return _tableTransforms[tipNums[0], tipNums[1]];
    }

    public void ChangeMoveState(bool state)
    {
        _isSkillTipMove = state;
    }
}