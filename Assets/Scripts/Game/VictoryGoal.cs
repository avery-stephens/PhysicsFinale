using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.Instance.SetGameWon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) GameManager.Instance.SetGameWon();
    }
}