using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{



    public class AnimEvent : MonoBehaviour
    {
        public Transform flipTarget;
        public List< UnityEvent> someEvent;
        public void InvokeEvent(int id)
        {
            someEvent[id].Invoke();
        }
      public void FlipTo()
        {
            flipTarget.localScale = new Vector3(-flipTarget.localScale.x, flipTarget.localScale.y, flipTarget.localScale.z);
        }
    }
}