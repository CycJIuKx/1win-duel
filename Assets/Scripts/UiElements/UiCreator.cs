using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Helpers.UI
{


    public class UiCreator : MonoBehaviour
    {
        public static UiCreator instance;
        [SerializeField] Transform content;
        [SerializeField] GameObject MiddleTextPref;
        [Space(5)]

        public GameObject commentPref;
        public Transform commentCOntent;
        

        private void Awake()
        {
            instance = this;
        }
        void Start()
        {

            
        }
        private void Update()
        {
            if (Input.GetKeyDown( KeyCode.Q))
            {
                CreateMiddleText("test testtesttesttest", 1, 3);
            }
        }
        //  public void CreateComment(string comment, Transform target)
        //  {
        //      GameObject go = Instantiate(commentPref, commentCOntent);
        //      go.GetComponent<UIFollower>().Init(target);
        //      go.GetComponentInChildren<Text>().text = comment;
        //  }
        public GameObject CreateMiddleText(string text, float animSpeedFactor = 1, float lifeTime = 3)
        {
            GameObject go = Instantiate(MiddleTextPref, content);
            go.GetComponent<Animator>().SetFloat("Speed", animSpeedFactor);
            go.GetComponentInChildren<Text>().text = text;
            StartCoroutine(addTowardFade(go, lifeTime));
            return go;

        }
        IEnumerator addTowardFade(GameObject go, float delay)
        {

            yield return new WaitForSeconds(delay);
            go.GetComponentInChildren<Text>().gameObject.
                AddComponent<Helpers.TowardMover>().setParams(Helpers.TowardMover.MoveObject.uiText, 1, 0, 1, true);
            yield return new WaitForSeconds(1);
            Destroy(go);
        }
    }
}