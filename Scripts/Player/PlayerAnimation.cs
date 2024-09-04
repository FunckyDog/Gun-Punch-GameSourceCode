using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : Singleton<PlayerAnimation>
{
    public Animator playerAnim;

    public Animator powerAnim;
    public Animator powerAnim_pre;

    private void Update()
    {
        AnimationSwitch();
    }

    private void AnimationSwitch()
    {
        playerAnim.SetFloat("speed", PlayerController.instance.rb.velocity.magnitude);
        playerAnim.SetBool("isRun", PlayerController.instance.isRun);
        playerAnim.SetBool("isSampleAttack", PlayerController.instance.isSampleAttack);
        playerAnim.SetBool("isCharge", PlayerController.instance.isCharge);
        playerAnim.SetBool("isBlock", PlayerController.instance.isBlock);
        playerAnim.SetBool("isBreakLevel", PlayerController.instance.isBreakLevel);

        powerAnim.SetInteger("level", PlayerController.instance.currentLevel);
        powerAnim_pre.SetInteger("level", PlayerController.instance.currentLevel);
    }
}
