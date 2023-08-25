using SG;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPickUp : Interactable
{
    [SerializeField]private Item ItemObject;
    private GameObject weaponModel;
    public override string ItemName
    {
        get { return ItemObject.ItemName; }
        set { ItemName = value; }
    }

    public override Sprite ItemIcon
    {
        get { return ItemObject.ItemIcon; }
        set { ItemIcon = value; }
    }

    private void Awake()
    {
        ItemObject = Instantiate(ItemObject);
        Debug.Log(this);
        var model = ItemObject.modelPrefab.GetComponentInChildren<BoxCollider>().gameObject;
        if (model == null) return;
        weaponModel = Instantiate(ItemObject.modelPrefab.GetComponentInChildren<BoxCollider>().gameObject, transform.position, Quaternion.identity);
        transform.rotation = UnityEngine.Random.rotation;
        UploadModelInScene();
        ItemObject.Id = Guid.NewGuid().ToString(); // создание нового Id
    }

    private void UploadModelInScene()
    {
        Destroy(weaponModel.GetComponent<BoxCollider>());
        var boxCollider = weaponModel.AddComponent<MeshCollider>();
        boxCollider.convex = true;
        var thisBoxCollider = gameObject.AddComponent<MeshCollider>();
        thisBoxCollider.convex = true;
        thisBoxCollider.sharedMesh = boxCollider.sharedMesh;
        Destroy(weaponModel.GetComponent<DamageCollider>());
        Destroy(weaponModel.GetComponent<MeshCollider>());
        gameObject.AddComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        weaponModel.transform.rotation = transform.rotation;
        weaponModel.transform.parent = transform;
        transform.localScale = weaponModel.transform.localScale;
        weaponModel.transform.localScale = Vector3.one;
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

        playerManager.GetComponent<CharacterController>().SimpleMove(Vector3.zero);
        animatorManager.PlayTargetAnimation("Pick Up Item", true,true);
        ItemObject.CharacterStats = playerManager.GetComponent<CharacterStats>();
        playerInventory.Inventory.Add(ItemObject);
        Destroy(gameObject);
    }
}
