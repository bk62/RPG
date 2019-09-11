using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public float radius = 3f;
    // interaction point
    public Transform interactionTransform;

    // is currently being focused
    bool isFocus = false;
    Transform player;

    // whether the usr has interacted with this interactable
    // after last focus
    bool hasInteracted = false;

    public virtual void Interact () {
        // Override!
        Debug.Log("Interacting with " + transform.name);
    }

    void Update () {
        if (isFocus && !hasInteracted) {
            float distance = Vector3.Distance (player.position, interactionTransform.position);
            if (distance <= radius) {
                Interact ();
                hasInteracted = true;
            }
        }
    }
    public void OnFocused (Transform playerTransform) {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void onDefocused () {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected () {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere (interactionTransform.position, radius);
    }
}