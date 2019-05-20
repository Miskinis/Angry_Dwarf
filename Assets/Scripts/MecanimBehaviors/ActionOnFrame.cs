using System;
using UnityEngine;

namespace MecanimBehaviors
{
    public class ActionOnFrame : StateMachineBehaviour
    {
        [Tooltip("Total number of frames in this animation")]
        public int totalFrameCount;

        [Tooltip("When Action is fired")] public int actionFrame;

        public Action onFrameAction;

        private float _frameTime;
        private bool _initialized;
        private bool _actionFired;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_initialized) return;

            _initialized = true;
            _frameTime   = stateInfo.length / totalFrameCount;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var currentTime = stateInfo.length * stateInfo.normalizedTime;

            if (!(currentTime >= _frameTime * actionFrame) || _actionFired) return;

            _actionFired = true;
            onFrameAction?.Invoke();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _actionFired = false;
        }
    }
}