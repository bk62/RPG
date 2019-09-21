using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator {
    public WeaponAnimations[] weaponAnimations;
    Dictionary<Equipment, AnimationClip[]> weaponAnimationsDict;

    protected override void Start () {
        base.Start();
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;

        weaponAnimationsDict = new Dictionary<Equipment, AnimationClip[]>();
        foreach(WeaponAnimations a in weaponAnimations) {
            weaponAnimationsDict.Add(a.weapon, a.clips);
        }
    }

    void OnEquipmentChanged (Equipment newItem, Equipment oldItem) {
        // TODO temporarily disabled setting layer weights
        // b/c could not set avatar masks due to a display bug
        // on layer settings set mask

        // Right hand
        // new weapon
        if (newItem != null && newItem.equipSlot == EquipmentSlot.Weapon) {
            // animator.SetLayerWeight (1, 1);
            if (weaponAnimationsDict.ContainsKey(newItem)) {
                currentAttackAnimSet = weaponAnimationsDict[newItem];
            }
        }
        // unequip weapon
        else if (newItem == null && oldItem != null && oldItem.equipSlot == EquipmentSlot.Weapon) {
            // animator.SetLayerWeight (1, 0);
            currentAttackAnimSet = defaultAttackAnimSet;
        }

        // Left hand
        // new shield
        if (newItem != null && newItem.equipSlot == EquipmentSlot.Shield) {
            // animator.SetLayerWeight (2, 1);
        }
        // unequip
        else if (newItem == null && oldItem != null && oldItem.equipSlot == EquipmentSlot.Shield) {
            // animator.SetLayerWeight (2, 0);
        }
    }

    [System.Serializable]
    public struct WeaponAnimations {
        public Equipment weapon;
        public AnimationClip[] clips;
    }
}