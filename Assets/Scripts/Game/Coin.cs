using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
//[RequireComponent(typeof(AudioSource))]
public class Coin : MonoBehaviour
{
    //[SerializeField] public AudioSource m_Clip;
    [SerializeField] private GameObject m_CoinEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddCoin();
            GameManager.Instance.AddScore(100);
            Instantiate(m_CoinEffect);
            gameObject.SetActive(false);
        }
    }
}
