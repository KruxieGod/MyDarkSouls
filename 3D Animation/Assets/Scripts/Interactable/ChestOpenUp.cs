using SG;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ChestOpenUp : Interactable
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private string nameAnimation;
    [SerializeField] private List<GameObject> items;
    [SerializeField] private Sprite chestIcon;
    public override Sprite ItemIcon { get => chestIcon;}
    public override string ItemName { get => "Chest";}

    public override void Interact(PlayerManager playerManager)
    {
        ChestPickUp(playerManager);
    }

    private void ChestPickUp(PlayerManager playerManager)
    {
        GetComponent<Animator>().CrossFade(nameAnimation, 0.1f);
    }

    public void DestroyChest()
    {
         SpawnObjectsAsync();
    }

    private void SpawnObjectsAsync()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            for (int i = 0; i < items.Count; i++)
                Instantiate(items[i], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            Destroy(gameObject);
        });
    }

    public void SpawnParticles()
    {
        Destroy(Instantiate(_particleSystem, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity), 10f);
    }
}
