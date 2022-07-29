using UnityEngine;

/// <summary>
/// Skill�s�[�X
/// </summary>
public class SkillController : MonoBehaviour
{
    bool _moveTip;
    Vector2[] _tipPotisions;
    RectTransform _rt;
    [SerializeField] int _size = 50;
    [SerializeField] SkillTableManager _tableManager;

    // mino��]
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
    /// �}�E�X�̓���
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
    /// �V�����s�[�X�̏ꏊ��ۑ�
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
    /// �N���b�N�����ۂɃ}�E�X���s�[�X�̏�ɂ��邩
    /// </summary>
    /// <param name="vec">�}�E�X�̃|�W�V����</param>
    /// <returns>�s�[�X�̏ォ�ۂ�</returns>
    /// 


    //�ύX
    bool MouseCheck(Vector2 vec)
    {
        //�`�b�v�ЂƂЂƂ�T��
        for (int i = 0; i < _tipPotisions.Length; i++)
        {
            if (_tipPotisions[i].x + (_size / 2) > vec.x && vec.x > _tipPotisions[i].x - (_size / 2))
            {
                if (_tipPotisions[i].y + (_size / 2) > vec.y && vec.y > _tipPotisions[i].y - (_size / 2))
                { 
                    //�J�[�\���̏�ɂ��ꂪ���������_��True�Ƃ���B
                    return true;
                }
            }
        }

        return false;
    }
}
