using UnityEngine;

[CreateAssetMenu (fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    new public string name = "New Item";
    public Sprite icon = null; // inventory icon
    public bool isDefaultItem = false;

    public virtual void Use () {
        // Use the item
        // do something

        Debug.Log ("Using " + name);
    }

}