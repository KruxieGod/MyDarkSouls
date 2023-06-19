using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearedWall : MonoBehaviour
{
    private bool isDisappeared;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.CompareTag("Weapon") && !isDisappeared)
        {
            isDisappeared = true;
            GetComponent<Animator>().SetBool("IsDisappearing", true);
            GetComponent<Collider>().isTrigger= true;
            GetComponent<AudioSource>().Play();
            Destroy(gameObject,2f);
        }
    }
}
