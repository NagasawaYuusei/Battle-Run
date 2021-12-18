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
    Tweener m_tweener;
    int m_damage;
    float m_time;

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
        State();
        Death();
        CameraRotate();
    }

    void Setup()
    {
        m_slider.maxValue = m_enemyMaxHP;
        m_enemyHP = m_enemyMaxHP;
        m_slider.value = m_enemyHP;
    }

    public void HP(int value)
    {
        m_tweener?.Kill();
        DOTween.To(() => m_slider.value, x => m_slider.value = x, m_enemyHP - value, m_changeTime).SetLink(gameObject);
        //HPˆê‹C‚É­‚È‚­‚È‚éver
        //m_slider.value = m_enemyHP;

        if(m_time > 0.5f)
        {
            m_damage = value;
        }
        else
        {
            m_damage += value;
        }
        m_time = 0;

        Text dTexT = m_damageText.GetComponent<Text>();
        Color clear = new Color(255, 255, 255, 0);
        dTexT.color = Color.white;
        dTexT.text = m_damage.ToString();
        m_tweener = DOTween.To(() => dTexT.color, x => dTexT.color = x, clear, 1f).SetLink(gameObject);

        m_enemyHP -= value;
    }

    void State()
    {
        m_time += Time.deltaTime;
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
