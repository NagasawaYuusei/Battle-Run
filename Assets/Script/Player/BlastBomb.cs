using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastBomb : MonoBehaviour
{
    [SerializeField] float m_bombPower = 10;
    [SerializeField] float m_bombRadius = 5;
    [SerializeField] float m_upwardsModifier;
    [SerializeField] LayerMask m_human;
    [SerializeField] string m_playerName = "Player";
    [SerializeField] float m_firstPower;
    [SerializeField] string m_bombMazzleName = "BombMazzle";

    // Update is called once per frame

    void Start()
    {
        First();
    }

    void First()
    {
        Transform playerTF = GameObject.FindWithTag(m_bombMazzleName).transform;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce((playerTF.forward + (-playerTF.up))* m_firstPower);
    }
    void Update()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        if (Input.GetButtonDown("Bomb"))
        {
            Bomb();
        }
    }

    void Bomb()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, m_bombRadius,m_human);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(m_bombPower, explosionPos, m_bombRadius,m_upwardsModifier);
        }
        //PlayerController pc = GameObject.Find(m_playerName).GetComponent<PlayerController>();
        //pc.IsBomb = true;
        Destroy(gameObject);
    }
}
