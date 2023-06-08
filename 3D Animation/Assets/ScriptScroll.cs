using SG;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScriptScroll : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject Content;
    private int index = -1;
    private float medianaPosition;
    private float halfLengthPanel;
    private List<ItemInfoDisplay> contents;
    private Dictionary<Interactable, ItemInfoDisplay> items;
    private Dictionary<ItemInfoDisplay, Interactable> interactions;
    private PlayerManager playerManager;
    private InputManager inputManager;

    private void Start()
    {
        ItemInfoDisplay prefab = Instantiate(Prefab).GetComponent<ItemInfoDisplay>();
        prefab.transform.SetParent(Content.transform, false);
        Destroy(prefab.gameObject);
        inputManager = FindObjectOfType<InputManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        halfLengthPanel = GetComponent<RectTransform>().rect.size.y/2f;
        medianaPosition = transform.position.y;
        contents = new List<ItemInfoDisplay>();
        items = new Dictionary<Interactable, ItemInfoDisplay>();
        interactions = new Dictionary<ItemInfoDisplay, Interactable>();
    }

    public void Add(Interactable item)
    {
        if (item == null || (items != null &&items.ContainsKey(item)))
            return;
        Debug.Log(item.ItemName);
        if (index == -1)
            index = 0;
        ItemInfoDisplay prefab = Instantiate(Prefab).GetComponent<ItemInfoDisplay>();
        if (prefab == null)
            return;
        prefab.transform.SetParent(Content.transform,false);
        prefab.Icon.sprite = item.ItemIcon;
        prefab.Text.text = item.ItemName;
        if (Content.transform.childCount == 1)
            prefab.GetComponent<Image>().color= Color.yellow;
        if (prefab == null || item == null)
            return;
        interactions.Add(prefab, item);
        items.Add(item, prefab);
        contents.Add(prefab);
        CalculateDistance(20f);
    }

    private void PickUp()
    {
        interactions[contents[index]].Interact(playerManager);
        Interactable interactable = interactions[contents[index]];
        interactions.Remove(contents[index]);
        Destroy(contents[index].gameObject);
        contents.RemoveAt(index);
        index = Mathf.Clamp(index, 0, contents.Count - 1);
        if (index != -1)
            contents[index].GetComponent<Image>().color= Color.yellow;
    }

    public void Remove(IEnumerable<Interactable> colliders, int length)
    {
        if (contents.Count == 0 && length == 0)
            return;
        contents = contents.Where(x =>
        {
            if (!interactions.ContainsKey(x))
                return false;
            else if (colliders.Contains(interactions[x]))
                return true;
            else
            {
                ItemInfoDisplay item = items[interactions[x]];
                items.Remove(interactions[x]);
                interactions.Remove(item);
                Destroy(x.gameObject);
                return false;
            }
        })
            .ToList();
        if (contents.Count == 0)
            index = -1;
        else
            index = Mathf.Clamp(index, 0, contents.Count - 1);
        if (index != -1)
        {
            contents[index].GetComponent<Image>().color = Color.yellow;
        }

    }

    void Update()
    {
        if (contents.Count < 1)
            return;
        if (inputManager.FIsPressed)
            PickUp();
        if (inputManager.ScrollDown)
        {
            contents[index].GetComponent<Image>().color = Color.red;
            index++;
            index = Mathf.Clamp(index, 0, contents.Count - 1);
            contents[index].GetComponent<Image>().color = Color.yellow;
            CalculateDistance();
        }
        else if (inputManager.ScrollUp)
        {
            contents[index].GetComponent<Image>().color = Color.red;
            index--;
            index = Mathf.Clamp(index, 0, contents.Count - 1);
            contents[index].GetComponent<Image>().color = Color.yellow;
            CalculateDistance();
        }
    }

    private void CalculateDistance(float d=0)
    {
        if (contents.Count < 3)
            return;
        RectTransform rectCurrent = contents[index].GetComponent<RectTransform>();
        float selectedPosition = rectCurrent.position.y;
        float halfSize = rectCurrent.rect.size.y / 2f;
        if ((selectedPosition + halfSize+ d) - medianaPosition > halfLengthPanel)
        {
            RectTransform content = Content.GetComponent<RectTransform>();
            float difference = (selectedPosition + halfSize) - (medianaPosition + halfLengthPanel) + d;
            Vector3 direction = content.position - new Vector3(0, difference, 0);
            content.position = direction;
        }
        else if (medianaPosition - (selectedPosition - halfSize) > halfLengthPanel)
        {
            RectTransform content = Content.GetComponent<RectTransform>();
            float difference = (medianaPosition - halfLengthPanel) - (selectedPosition - halfSize) ;
            Vector3 direction = content.position + new Vector3(0, difference, 0);
            content.position = direction;
        }
    }
}
