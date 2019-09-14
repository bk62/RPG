using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {
    #region Singleton
    public static EquipmentManager instance;

    void Awake () {
        instance = this;
    }

    #endregion

    Equipment[] currentEquipment;

    public delegate void OnEquipmentChanged (Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;

    void Start () {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames (typeof (EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }

    public void PlaceInSlot (int slotIndex, Equipment newItem) {
        if (currentEquipment[slotIndex] == newItem) {
            return;
        }

        Equipment oldItem = currentEquipment[slotIndex];

        // if something is in slot, put into inventory
        if (oldItem != null)
            inventory.Add (oldItem);

        currentEquipment[slotIndex] = newItem;

        if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke (newItem, oldItem);
    }

    public void Equip (Equipment newItem) {
        int slotIndex = (int) newItem.equipSlot;

        PlaceInSlot (slotIndex, newItem);
    }

    public void Unequip (int slotIndex) {
        PlaceInSlot (slotIndex, null);
    }

    public void UnequipAll () {
        for (int i = 0; i < currentEquipment.Length; i++) {
            Unequip (i);
        }
    }
    public void Update () {
        if (Input.GetKeyDown ("u")) {
            UnequipAll ();
        }
    }

}