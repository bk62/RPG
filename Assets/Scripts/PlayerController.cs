using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerMotor))]
public class PlayerController : MonoBehaviour {
    public LayerMask movementMask;

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
            }
        }

        if (Input.GetMouseButtonDown (1)) {
            Ray ray = cam.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast (ray, out hit, 100)) {
                Debug.Log ("Right click on " + hit.collider.name + " " + hit.point);

                // Check if we hit an interactable object
                // Set it as our focus
            }
        }
    }
}