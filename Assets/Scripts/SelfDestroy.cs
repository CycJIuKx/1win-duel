using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{


    public class SelfDestroy : MonoBehaviour
    {
        public float timeDelay = 1;
        public bool isUnscale = false;
        public System.Action onDestroy;
        void Start()
        {

        }
        private void OnDisable()
        {
           //  DestroyMe();
        }
        // Update is called once per frame
        void Update()
        {
            if (!isUnscale)
            {
                timeDelay -= Time.deltaTime;
                if (timeDelay < 0) DestroyMe();
            }
            else
            {
                timeDelay -= Time.unscaledDeltaTime;
            }

            if (timeDelay < 0)
            {
                 DestroyMe();
            }
        }
        public void DestroyMe()
        {
            onDestroy?.Invoke();
            Destroy(gameObject);
        }
    }
}