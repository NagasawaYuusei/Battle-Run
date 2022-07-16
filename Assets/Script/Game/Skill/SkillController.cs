using UnityEngine;

/// <summary>
/// Skillピース
/// </summary>
public class SkillController : MonoBehaviour
{
    bool _moveTip;
    Vector2[] _tipPotisions;
    RectTransform _rt;
    [SerializeField] int _size = 50;
    [SerializeField] SkillTableManager _tableManager;
    void Start()
    {
        SetUp();
        SetTipPosition();
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

    /// <summary>
    /// マウスの動き
    /// </summary>
    void MouseMove()
    {
        if (Input.GetMouseButtonDown(0) && !_moveTip && !_tableManager.IsSkillTipMove)
        {
            Vector3 mousePosition = Input.mousePosition;
            if (MouseCheck(mousePosition))
            {
                _moveTip = true;
                _tableManager.ChangeMoveState(true);
            }
        }
        else if (Input.GetMouseButtonDown(0) && _moveTip)
        {
            int[] nums = _tableManager.SerchSet(Input.mousePosition);
            if(nums != null)
            {
                _rt.anchoredPosition = _tableManager.TableTip(nums);
                SetTipPosition();
                _moveTip = false;
                _tableManager.ChangeMoveState(false);
            }
        }

        if (_moveTip)
        {
            Vector3 mousePosition = Input.mousePosition;
            _rt.anchoredPosition = mousePosition;
        }
    }

    /// <summary>
    /// 新しいピースの場所を保存
    /// </summary>
    void SetTipPosition()
    {
        for (int i = 0; i < _tipPotisions.Length; i++)
        {
            _tipPotisions[i] = transform.GetChild(i).GetComponent<RectTransform>().localPosition;
            _tipPotisions[i] = new Vector2(_rt.anchoredPosition.x + _tipPotisions[i].x, _rt.anchoredPosition.y + _tipPotisions[i].y);
        }
    }

    /// <summary>
    /// クリックした際にマウスがピースの上にあるか
    /// </summary>
    /// <param name="vec">マウスのポジション</param>
    /// <returns>ピースの上か否か</returns>
    bool MouseCheck(Vector2 vec)
    {
        //チップひとつひとつを探索
        for (int i = 0; i < _tipPotisions.Length; i++)
        {
            if (_tipPotisions[i].x + (_size / 2) > vec.x && vec.x > _tipPotisions[i].x - (_size / 2))
            {
                if (_tipPotisions[i].y + (_size / 2) > vec.y && vec.y > _tipPotisions[i].y - (_size / 2))
                { 
                    //カーソルの上にそれがあった時点でTrueとする。
                    return true;
                }
            }
        }

        return false;
    }
}
