using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class ShoterBase : MonoBehaviour
{
    [Header("Input")]
    protected bool m_isFire; //Fire1

    [Header("Ray")]
    [SerializeField, Tooltip("カメラのトランスフォーム")] Transform m_cameraTransform;
    [SerializeField, Tooltip("銃の範囲")] float m_distance = 200f;
    [SerializeField, Tooltip("敵のレイヤー")] LayerMask m_enemyLayer;
    [Tooltip("当たった敵")] protected RaycastHit m_enemy;

    [Header("Damage")]
    [SerializeField, Tooltip("ダメージの値")] protected int m_damage;
    [SerializeField, Tooltip("DPS")] protected float m_damagePerSecond;
    
    [Header("Attack")]
    [SerializeField, Tooltip("アタックのクールタイム")] protected float m_coolDownTime;
    [SerializeField, Tooltip("")] protected float m_recoilCoolDownTime;
    [Tooltip("クールタイム外に撃った回数")] protected int m_atkCount;
    [Tooltip("当たった敵")] protected float m_atkNextTime;
    [SerializeField, Tooltip("リコイルの値")] protected float m_recoilValue;
    [SerializeField, Tooltip("リコイルの上昇値")] protected float m_recoilUpValue;

    /// <summary>マウスレフトインプットシステム</summary>
    public void PlayerFire(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            m_isFire = true;
        }

        if(context.canceled)
        {
            m_isFire = false;
        }
    }

    /// <summary>マウスライトインプットシステム</summary>
    public void PlayerFireSecond(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            FireSecondMethod(true);
        }
        
        if(context.canceled)
        {
            FireSecondMethod(false);
        }
    }

    /// <summary>攻撃の関数</summary>
    protected void Fire()
    {
        m_coolDownTime += Time.deltaTime;
        if (m_isFire)
        {
            FireMethod();
        }
    }

    /// <summary>レフト攻撃メソッド</summary>
    protected virtual void FireMethod(){}

    /// <summary>ライト攻撃メソッド</summary>
    protected virtual void FireSecondMethod(bool on){}

    /// <summary>リコイルメソッド</summary>
    protected virtual void Recoil(CinemachinePOV pov) {}

    /// <summary>攻撃判定</summary>
    protected bool IsCollision()
    {
        bool isCollision = Physics.Raycast(m_cameraTransform.position, m_cameraTransform.forward, out m_enemy, m_distance, m_enemyLayer);
        return isCollision;
    }
}
