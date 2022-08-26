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

    // mino回転
    Vector3 _rotationPoint;

    // grid
    //static Transform[,] grid = new Transform[width, height];
    void Start()
    {
        SetUp();
        SetTipPosition();
    }

    void LateUpdate()
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
            //置けるかどうかを判定
            int[] nums = _tableManager.SerchSet(Input.mousePosition);
            if(nums != null)
            {
                //Tipの位置を固定
                _rt.anchoredPosition = _tableManager.TableTip(nums);
                //Tipの位置を保存
                SetTipPosition();

                //Tip内での動かしているという情報を変更
                _moveTip = false;
                //動かしているという状態を変更
                _tableManager.ChangeMoveState(false);
            }
        }

        if (Input.GetMouseButtonDown(1) && _moveTip)
        {
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);
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
    /// 


    //変更
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
