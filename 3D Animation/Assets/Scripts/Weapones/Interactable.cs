using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;

public class Interactable : MonoBehaviour
{
    public float Radius = 0.6f;
    public virtual string ItemName { get; set; }
    public virtual Sprite ItemIcon { get; set; }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color= Color.blue;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("You interacted with an object!");
        //Called when Player Interacts
    }
}
