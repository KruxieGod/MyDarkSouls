using SG;
using System;
using UnityEngine;

public class WeaponPickUp : Interactable
{
    public Item weapon;
    public override string ItemName
    {
        get { return weapon.ItemName; }
        set { ItemName = value; }
    }

    public override Sprite ItemIcon
    {
        get { return weapon.ItemIcon; }
        set { ItemIcon = value; }
    }

    private void Awake()
    {
        weapon = Instantiate(weapon);
        weapon.Id = Guid.NewGuid().ToString(); // создание нового Id
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
        //PICK UP THE ITEM
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
        AnimatorManager animatorManager= playerManager.GetComponent<AnimatorManager>();
        PlayerLocomotion playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();

        playerLocomotion.GetComponent<Rigidbody>().velocity = Vector3.zero;
        animatorManager.PlayTargetAnimation("Pick Up Item", true,true);
        weapon.PlayerId = playerManager.GetComponent<InputManager>().IdPlayer;
        Debug.Log("Collect");
        playerInventory.WeaponsInventory.Add(weapon);
        Destroy(gameObject);
    }
}
