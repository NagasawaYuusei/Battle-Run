using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class ShoterBase : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("撃つインプットシステム")]protected bool m_isFire; //Fire1

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
    protected bool m_isFireSecond;

    public bool IsFireSecond
    {
        get
        {
            return m_isFireSecond;
        }
    }

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
