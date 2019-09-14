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

    public Equipment[] defaultItems;
    public SkinnedMeshRenderer targetMesh;
    Equipment[] currentEquipment;
    SkinnedMeshRenderer[] currentMeshes;

    public delegate void OnEquipmentChanged (Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;

    void Start () {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames (typeof (EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        EquipDefaultItems ();
    }

    public void PlaceInSlot (int slotIndex, Equipment newItem) {
        if (currentEquipment[slotIndex] == newItem) {
            return;
        }

        // Inventory and equipment array updates:
        Equipment oldItem = currentEquipment[slotIndex];
        // If something is in slot, put into inventory
        if (oldItem != null)
            inventory.Add (oldItem);
        currentEquipment[slotIndex] = newItem;

        // Meshes and Blend shapes:
         if (oldItem != null) {
            // first delete mesh of old item
            if (currentMeshes[slotIndex] != null) {
                Destroy (currentMeshes[slotIndex].gameObject);
            }
        }
        if (newItem != null) {
            // set blend shape
            SetEquipmentBlendShapes (newItem, 100);

            // update mesh with newItem
            SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer> (newItem.mesh);
            newMesh.transform.parent = targetMesh.transform;

            newMesh.bones = targetMesh.bones;
            newMesh.rootBone = targetMesh.rootBone;
            currentMeshes[slotIndex] = newMesh;
        } else {
            // reset mesh blend shape
            SetEquipmentBlendShapes (oldItem, 0);
        }

        // Event dispatch:
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
        EquipDefaultItems ();
    }

    public void EquipDefaultItems () {
        foreach (Equipment item in defaultItems) {
            Equip (item);
        }
    }

    public void Update () {
        if (Input.GetKeyDown ("u")) {
            UnequipAll ();
        }
    }

    void SetEquipmentBlendShapes (Equipment item, int weight) {
        foreach (EquipmentMeshRegion blendShape in item.coveredMeshRegions) {
            targetMesh.SetBlendShapeWeight ((int) blendShape, weight);
        }
    }

}