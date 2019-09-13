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

    public void Equip (Equipment newItem) {
        int slotIndex = (int) newItem.equipSlot;

        Equipment oldItem = null;

        if (currentEquipment[slotIndex] != null) {
            // something already in slot
            oldItem = currentEquipment[slotIndex];
            inventory.Add (oldItem);
        }

        currentEquipment[slotIndex] = newItem;

        if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke (newItem, oldItem);
    }

    public void Unequip (int slotIndex) {
        if (currentEquipment[slotIndex] == null) {
            return;
        }

        // something in slot
        Equipment oldItem = currentEquipment[slotIndex];
        inventory.Add (oldItem);

        currentEquipment[slotIndex] = null;

         if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke (null, oldItem);
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