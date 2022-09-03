using UnityEngine;

/// <summary>
/// Skillピース一つ一つ
/// </summary>
public class SkillController : MonoBehaviour
{
    [Tooltip("チップを動かしているかどうか")] bool _moveTip;
    [Tooltip("それぞれのチップの場所")] Vector2[] _tipPotisions;
    [Tooltip("チップのトランスフォーム")]RectTransform _rt;
    [Tooltip("チップの大きさ"), SerializeField] int _size = 50;
    [Tooltip("スキルテーブルマネージャー"), SerializeField] SkillTableManager _tableManager;

    [Tooltip("スキルチップの回転")] Vector3 _rotationPoint;
    bool _isSet;

    void Start()
    {
        SetUp();
        SetTipPosition();
    }

    void LateUpdate()
    {
        MouseMove();
    }

    /// <summary>
    /// チップの設定
    /// </summary>
    void SetUp()
    {
        //トランスフォームの取得
        _rt = GetComponent<RectTransform>();
        //それぞれのチップの数
        _tipPotisions = new Vector2[transform.childCount];
    }

    /// <summary>
    /// マウスの動き
    /// </summary>
    void MouseMove()
    {
        //マウス押したときの処理　もしチップをつかんでいなかったら
        if (Input.GetMouseButtonDown(0) && !_moveTip && !_tableManager.IsSkillTipMove)
        {
            Vector3 mousePosition = Input.mousePosition;
            //もしカーソル下に自分があったら
            if (MouseCheck(mousePosition))
            {
                if (_isSet)
                {
                    _tableManager.ClearTable(_tipPotisions);
                    _isSet = false;
                }
                _moveTip = true;
                _tableManager.ChangeMoveState(true);
            }
        }
        //マウス押したときの処理　もしチップをつかんでいたら
        else if (Input.GetMouseButtonDown(0) && _moveTip)
        {
            //Tipの位置を保存
            SetTipPosition();
            //置けるかどうかを判定
            int[] nums = _tableManager.SerchSet(_tipPotisions);
            if(nums != null)
            {
                _isSet = true;
                //Tipの位置を固定
                _rt.anchoredPosition = _tableManager.TableTip(nums);

                //Tip内での動かしているという情報を変更
                _moveTip = false;
                //動かしているという状態を変更
                _tableManager.ChangeMoveState(false);
            }
        }

        //マウス右クリックで回転
        if (Input.GetMouseButtonDown(1) && _moveTip)
        {
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);
        }

        //もし選択中ならマウスの先にチップを持ってくる
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
            //チップの子オブジェクトの場所を格納
            _tipPotisions[i] = transform.GetChild(i).GetComponent<RectTransform>().localPosition;
            //チップの親オブジェクトとの誤差を埋める
            _tipPotisions[i] = new Vector2(_rt.anchoredPosition.x + _tipPotisions[i].x, _rt.anchoredPosition.y + _tipPotisions[i].y);
        }
    }

    /// <summary>
    /// クリックした際にマウスがピースの上にあるか
    /// </summary>
    /// <param name="vec">マウスのポジション</param>
    /// <returns>ピースの上か否か</returns>
    /// 

    //チップをつかむ
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
