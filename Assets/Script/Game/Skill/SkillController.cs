using UnityEngine;

/// <summary>
/// Skill�s�[�X����
/// </summary>
public class SkillController : MonoBehaviour
{
    [Tooltip("�`�b�v�𓮂����Ă��邩�ǂ���")] bool _moveTip;
    [Tooltip("���ꂼ��̃`�b�v�̏ꏊ")] Vector2[] _tipPotisions;
    [Tooltip("�`�b�v�̃g�����X�t�H�[��")]RectTransform _rt;
    [Tooltip("�`�b�v�̑傫��"), SerializeField] int _size = 50;
    [Tooltip("�X�L���e�[�u���}�l�[�W���["), SerializeField] SkillTableManager _tableManager;

    [Tooltip("�X�L���`�b�v�̉�]")] Vector3 _rotationPoint;
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
    /// �`�b�v�̐ݒ�
    /// </summary>
    void SetUp()
    {
        //�g�����X�t�H�[���̎擾
        _rt = GetComponent<RectTransform>();
        //���ꂼ��̃`�b�v�̐�
        _tipPotisions = new Vector2[transform.childCount];
    }

    /// <summary>
    /// �}�E�X�̓���
    /// </summary>
    void MouseMove()
    {
        //�}�E�X�������Ƃ��̏����@�����`�b�v������ł��Ȃ�������
        if (Input.GetMouseButtonDown(0) && !_moveTip && !_tableManager.IsSkillTipMove)
        {
            Vector3 mousePosition = Input.mousePosition;
            //�����J�[�\�����Ɏ�������������
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
        //�}�E�X�������Ƃ��̏����@�����`�b�v������ł�����
        else if (Input.GetMouseButtonDown(0) && _moveTip)
        {
            //Tip�̈ʒu��ۑ�
            SetTipPosition();
            //�u���邩�ǂ����𔻒�
            int[] nums = _tableManager.SerchSet(_tipPotisions);
            if(nums != null)
            {
                _isSet = true;
                //Tip�̈ʒu���Œ�
                _rt.anchoredPosition = _tableManager.TableTip(nums);

                //Tip���ł̓������Ă���Ƃ�������ύX
                _moveTip = false;
                //�������Ă���Ƃ�����Ԃ�ύX
                _tableManager.ChangeMoveState(false);
            }
        }

        //�}�E�X�E�N���b�N�ŉ�]
        if (Input.GetMouseButtonDown(1) && _moveTip)
        {
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);
        }

        //�����I�𒆂Ȃ�}�E�X�̐�Ƀ`�b�v�������Ă���
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
            //�`�b�v�̎q�I�u�W�F�N�g�̏ꏊ���i�[
            _tipPotisions[i] = transform.GetChild(i).GetComponent<RectTransform>().localPosition;
            //�`�b�v�̐e�I�u�W�F�N�g�Ƃ̌덷�𖄂߂�
            _tipPotisions[i] = new Vector2(_rt.anchoredPosition.x + _tipPotisions[i].x, _rt.anchoredPosition.y + _tipPotisions[i].y);
        }
    }

    /// <summary>
    /// �N���b�N�����ۂɃ}�E�X���s�[�X�̏�ɂ��邩
    /// </summary>
    /// <param name="vec">�}�E�X�̃|�W�V����</param>
    /// <returns>�s�[�X�̏ォ�ۂ�</returns>
    /// 

    //�`�b�v������
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
