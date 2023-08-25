using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEventTriggerCollider : MonoBehaviour
{
    [SerializeField] private GameObject animatorObject;
    [SerializeField] private Transform directionBoss;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        GlobalEventManager.SendBossFightEvent(true);
        if (animatorObject != null)
            animatorObject.GetComponent<Animator>().CrossFade("OpenDoor",0.2f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        var position = directionBoss.position;
        var directionPlayer = position - other.transform.position;
        var directionObject = position - transform.position;
        if (!(Vector3.Dot(directionPlayer, directionObject) > 0)) return;
        GlobalEventManager.SendBossFightEvent(false);
        if (animatorObject != null)
            animatorObject.GetComponent<Animator>().CrossFade("CloseDoor",0.2f);
    }
}
