using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] int m_enemyMaxHP = 150;
    [SerializeField] float m_changeTime = 0.3f;
    int m_enemyHP;
    [SerializeField] Slider m_slider;
    [SerializeField] Canvas m_canvas;
    [SerializeField] GameObject m_damageText;
    Vector3 m_damageVec; 

    public int EnemyHP
    {
        get
        {
            return m_enemyHP;
        }
        set
        {
            m_enemyHP = value;
        }
    }

    void Start()
    {
        Setup();
    }
    void Update()
    {
        Death();
        CameraRotate();
    }

    void Setup()
    {
        m_slider.maxValue = m_enemyMaxHP;
        m_enemyHP = m_enemyMaxHP;
        m_slider.value = m_enemyHP;
        m_damageVec = m_damageText.GetComponent<RectTransform>().position;
        m_damageText.SetActive(false);
    }

    public void HP(int value)
    {
        DOTween.To(() => m_slider.value, x => m_slider.value = x, m_enemyHP-value, m_changeTime);
        m_enemyHP -= value;

        m_damageText.SetActive(true);
        m_damageText.GetComponent<RectTransform>().position = m_damageVec;
        Text dTexT = m_damageText.GetComponent<Text>();
        dTexT.text = value.ToString();
        Rigidbody dRb = m_damageText.GetComponent<Rigidbody>();
        dRb.AddForce(Vector3.up, ForceMode.Impulse);
        float a = 0;
        //DOTween.To(() => dTexT.color.a, x => dTexT.color.a = x, a, 1f);
        //HPàÍãCÇ…è≠Ç»Ç≠Ç»ÇÈver
        //m_slider.value = m_enemyHP;
    }

    void Death()
    {
        if(m_enemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    void CameraRotate()
    {
        m_canvas.transform.rotation = Camera.main.transform.rotation;
    }
}
