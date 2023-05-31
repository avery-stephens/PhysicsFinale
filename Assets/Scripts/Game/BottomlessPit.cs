using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BottomlessPit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Transform camFollow = collision.transform;
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("waAaAaAAahHHhhhh");
            //FindObjectOfType<CinemachineVirtualCamera>().m_Follow = camFollow;
            FindObjectOfType<CinemachineVirtualCamera>().enabled = false;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Enemy ded");
            var enemy = collision.gameObject.GetComponent<AICharacter2D>();
            enemy.Damage((int)enemy.health);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Transform camFollow = collision.transform;
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<ControllerCharacter2D>();
            player.Damage((int)player.health);
        }
        
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Transform camFollow = collision.transform;
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("waAaAaAAahHHhhhh");
    //        collision.gameObject.GetComponent<CinemachineVirtualCamera>().m_Follow = camFollow;
    //    }
    //}
}
