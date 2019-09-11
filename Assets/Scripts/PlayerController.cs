using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerMotor))]
public class PlayerController : MonoBehaviour {
    public LayerMask movementMask;
    public Interactable focus;

    Camera cam;
    PlayerMotor motor;

    void Start () {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor> ();
    }

    void Update () {
        if (Input.GetMouseButtonDown (0)) {
            Ray ray = cam.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast (ray, out hit, 100, movementMask)) {
                Debug.Log ("Left click on " + hit.collider.name + " " + hit.point);

                // Move player to what we hit
                motor.MoveToPoint (hit.point);

                // Stop focusing any objects
                RemoveFocus ();
            }
        }

        if (Input.GetMouseButtonDown (1)) {
            Ray ray = cam.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast (ray, out hit, 100)) {
                Debug.Log ("Right click on " + hit.collider.name + " " + hit.point);

                // Check if we hit an interactable object
                Interactable interactable = hit.collider.GetComponent<Interactable> ();
                if (interactable != null) {
                    // Set it as our focus
                    SetFocus (interactable);
                }
            }
        }
    }

    void SetFocus (Interactable newFocus) {
        if (newFocus != focus) {
            // if focus has changed
            if (focus != null)
                focus.onDefocused ();
            focus = newFocus;
            motor.FollowTarget (newFocus);
        }

        // notify interactable each time its clicked on
        // (even if it was already focused)
        newFocus.OnFocused (transform);
    }

    void RemoveFocus () {
        if (focus != null)
            focus.onDefocused ();
        focus = null;
        motor.StopFollowingTarget ();
    }

}