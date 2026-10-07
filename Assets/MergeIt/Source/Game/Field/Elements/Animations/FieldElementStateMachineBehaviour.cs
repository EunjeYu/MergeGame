// Copyright (c) 2024, Awessets

using MergeIt.Core.FieldElements;
using UnityEngine;

namespace MergeIt.Game.Field.Elements.Animations
{
    [SharedBetweenAnimators]
    public class FieldElementStateMachineBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            var view = animator.gameObject.GetComponent<IFieldElementView>();
            int stateHash = stateInfo.shortNameHash;
            
            if (view != null)
            {
                if (stateHash == FieldElementAnimationController.GetFieldElementState(FieldElementState.Hint))
                {
                    view.ResetState();
                }
            }
        }
    }
}