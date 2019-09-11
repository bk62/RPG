using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour {
    // takes .1s to transition betn animations
    const float locomotionAnimSmoothTime = 0.1f;
    NavMeshAgent agent;
    Animator animator;

    void Start () {
        agent = GetComponent<NavMeshAgent> ();
        animator = GetComponentInChildren<Animator> ();
    }

    void Update () {
        // current speed / max speed is betn 0..1
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent", speedPercent, locomotionAnimSmoothTime, Time.deltaTime);
    }
}