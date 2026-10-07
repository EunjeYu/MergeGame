// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using UnityEngine;

namespace MergeIt.Core.Animations
{
    [SharedBetweenAnimators]
    public class AnimatorStateMachineBehaviour : StateMachineBehaviour
    {
        private static readonly Dictionary<Animator, bool> ActiveAnimators = new();
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            ActiveAnimators[animator] = true;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (!ActiveAnimators[animator])
            {
                return;
            }
            
            if (stateInfo.normalizedTime >= 1f && 
                !animator.IsInTransition(layerIndex))
            {
                ActiveAnimators[animator] = false;
                CheckForEnd(animator, stateInfo.shortNameHash);
            }
        }

        private void CheckForEnd(Animator animator, int stateHash)
        {
            var listener = animator.gameObject.GetComponent<IWindowAnimationController>();
            if (listener != null)
            {
                if (stateHash == AnimationWindowStates.Open)
                {
                    listener.OnOpenEnd();
                }
                else if (stateHash == AnimationWindowStates.Close)
                {
                    listener.OnCloseEnd();
                }
            }
        }
    }


}