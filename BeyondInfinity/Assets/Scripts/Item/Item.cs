using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void Interact();
}
public class Item : MonoBehaviour, IInteractable
{
    public ItemData data;
    
    public string GetInteractPrompt()
    {
        string str = $"{data.name}\n{data.description}";
        return str;
    }

    public void Interact()
    {
        PlayerManager.Instance.Player.interaction.itemData = data;
        PlayerManager.Instance.Player.interaction.OnAddItem?.Invoke();
        Destroy(gameObject);
    }
}
