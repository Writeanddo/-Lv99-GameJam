using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto : MonoBehaviour
{

    public int objectId;
    [SerializeField]
    private Animator animator;

    public AnimationClip animationDefault;
    public AnimationClip animationSelect;

    private PointsOrderPuzzle m_PointsOrderPuzzle;
    private bool isCorret;

    private void Start()
    {
        animator = GetComponent<Animator>();
        var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController[animationDefault] = animationDefault;
        animator.runtimeAnimatorController = overrideController;

  
        m_PointsOrderPuzzle = GetComponentInParent<PointsOrderPuzzle>();
    }

    public void SpriteDefault()
    {
        var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController[animationDefault] = animationDefault;
        animator.runtimeAnimatorController = overrideController;
        isCorret = false;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("puzzle") && !isCorret)
        {

            if (m_PointsOrderPuzzle.currentObjectIndex == objectId)
            {
                isCorret = true;
                var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
                overrideController[animationDefault] = animationSelect;
                animator.runtimeAnimatorController = overrideController;
                m_PointsOrderPuzzle.currentObjectIndex++;
                if(m_PointsOrderPuzzle.currentObjectIndex == m_PointsOrderPuzzle.listObjeto.Count)
                {
                    Puzzle2Controller.Instance.play.SetActive(false);
                    Puzzle2Controller.Instance.win.SetActive(true);
                }
            }
            else
            {
                m_PointsOrderPuzzle.ResetPuzzle();
            }

        }
    }


}

