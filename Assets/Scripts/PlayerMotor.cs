using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class PlayerMotor : MonoBehaviour {
    NavMeshAgent agent;
    Transform target;

    void Start () {
        agent = GetComponent<NavMeshAgent> ();
    }

    void Update () {
        // TODO use coroutines and only update a few times a second
        if (target != null) {
            agent.SetDestination (target.position);
            FaceTarget ();
        }
    }

    public void MoveToPoint (Vector3 point) {
        agent.SetDestination (point);
    }

    public void FollowTarget (Interactable newTarget) {
        agent.stoppingDistance = newTarget.radius * 0.8f; // move 80% within radius
        agent.updateRotation = false; // manually set rotation

        target = newTarget.interactionTransform;
    }

    public void StopFollowingTarget () {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;

        target = null;
    }

    void FaceTarget () {
        // Look in the target's direction (except y)
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}