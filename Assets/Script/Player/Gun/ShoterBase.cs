using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class ShoterBase : MonoBehaviour
{
    [Header("Input")]
    protected bool m_isFire; //Fire1

    [Header("Ray")]
    [SerializeField] Transform m_cameraTransform; //カメラのトランスフォーム
    [SerializeField] float m_distance = 200f; // 銃の範囲
    [SerializeField] LayerMask m_enemyLayer; //敵のレイヤー
    protected RaycastHit m_enemy; //当たった敵

    [Header("Damage")]
    [SerializeField] protected int m_damage; //ダメージの値
    [SerializeField] protected float m_damagePerSecond; //DPS
    
    [Header("Attack")]
    [SerializeField] protected float m_coolDownTime; //アタックのクールタイム
    protected int m_atkCount; //クールタイム外に撃った回数
    protected float m_atkNextTime;
    [SerializeField] protected float m_recoilValue; //リコイルの値
    [SerializeField] protected float m_recoilUpValue; //リコイルの上昇値

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
