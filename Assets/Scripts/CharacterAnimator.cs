using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour {
    public AnimationClip replaceableAttackAnim;
    public AnimationClip[] defaultAttackAnimSet;
    protected AnimationClip[] currentAttackAnimSet;

    // takes .1s to transition betn animations
    const float locomotionAnimSmoothTime = 0.1f;
    NavMeshAgent agent;
    protected Animator animator;
    protected CharacterCombat combat;
    public AnimatorOverrideController overrideController;

    protected virtual void Start () {
        agent = GetComponent<NavMeshAgent> ();
        animator = GetComponentInChildren<Animator> ();
        combat = GetComponent<CharacterCombat> ();

        if (overrideController == null) {
            // allows swapping animator clips
            overrideController = new AnimatorOverrideController (animator.runtimeAnimatorController);
        }
        animator.runtimeAnimatorController = overrideController;

        currentAttackAnimSet = defaultAttackAnimSet;
        combat.OnAttack += OnAttack;
    }

    protected virtual void Update () {
        // current speed / max speed is betn 0..1
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat ("speedPercent", speedPercent, locomotionAnimSmoothTime, Time.deltaTime);

        animator.SetBool ("inCombat", combat.InCombat);
    }

    protected virtual void OnAttack () {
        animator.SetTrigger ("attack");
        int attackIx = Random.Range (0, currentAttackAnimSet.Length);
        overrideController[replaceableAttackAnim.name] = currentAttackAnimSet[attackIx];
    }
}