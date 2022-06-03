using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Helpers
{
    public class TowardMover : MonoBehaviour
    {

        public enum MoveObject { sprite, volume, image, uiText, lightRange, lrColor, lightIntensity ,scale};
        public MoveObject moveObject;
        public float moveToFloat;
        public float moveFromFloat;
        SpriteRenderer sr;
        AudioSource audio;
        Image image;
        Text uiText;
        Light light;
        LineRenderer lr;
        public float speed = 0.5f;
        public float alpha;
        bool destroyOnEnd = true;
        Transform myTransform;

        private void Awake()
        {
           

        }
        void Start()
        {
           // delConcurents();
            alpha = moveFromFloat;
            if (moveObject == MoveObject.sprite) { sr = GetComponent<SpriteRenderer>(); }
            if (moveObject == MoveObject.volume) { audio = GetComponent<AudioSource>(); }
            if (moveObject == MoveObject.image) { image = GetComponent<Image>(); }
            if (moveObject == MoveObject.uiText) { uiText = GetComponent<Text>(); }
            if (moveObject == MoveObject.lightRange) { light = GetComponent<Light>(); }
            if (moveObject == MoveObject.lightIntensity) { light = GetComponent<Light>(); }
            if (moveObject == MoveObject.lrColor) { lr = GetComponent<LineRenderer>(); }
            if (moveObject == MoveObject.scale) { myTransform = GetComponent<Transform>(); }
           
        }

      
        void Update()
        {
            if (moveObject == MoveObject.sprite)
            {
                alpha = Mathf.MoveTowards(alpha, moveToFloat, speed * Time.unscaledDeltaTime);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
                if (alpha == moveToFloat)
                {

                    Destroy(this);
                }
            }
            if (moveObject == MoveObject.image)
            {
                alpha = Mathf.MoveTowards(alpha, moveToFloat, speed * Time.unscaledDeltaTime);
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                if (alpha == moveToFloat)
                {

                    if (destroyOnEnd) Destroy(gameObject);
                    else Destroy(this);
                }
            }
            if (moveObject == MoveObject.uiText)
            {
                alpha = Mathf.MoveTowards(alpha, moveToFloat, speed * Time.unscaledDeltaTime);
                uiText.color = new Color(uiText.color.r, uiText.color.g, uiText.color.b, alpha);
                if (alpha == moveToFloat)
                {

                    if (destroyOnEnd) Destroy(gameObject);
                    else Destroy(this);
                }
            }
            if (moveObject == MoveObject.volume)
            {
                alpha = Mathf.MoveTowards(alpha, moveToFloat, speed * Time.unscaledDeltaTime);
                audio.volume = alpha;
                if (alpha == moveToFloat)
                {

                    Destroy(this);
                }
            }
            if (moveObject == MoveObject.lightRange)
            {
                alpha = Mathf.MoveTowards(alpha, moveToFloat, speed * Time.unscaledDeltaTime);
                light.range = alpha;
                if (alpha == moveToFloat)
                {

                    if (destroyOnEnd) Destroy(gameObject);
                    else Destroy(this);
                }
            }
            if (moveObject == MoveObject.lightIntensity)
            {
                alpha = Mathf.MoveTowards(alpha, moveToFloat, speed * Time.unscaledDeltaTime);
                light.intensity = alpha;
                if (alpha == moveToFloat)
                {

                    if (destroyOnEnd) Destroy(gameObject);
                    else Destroy(this);
                }
            }
            if (moveObject == MoveObject.lrColor)
            {
                alpha = Mathf.MoveTowards(alpha, moveToFloat, speed * Time.unscaledDeltaTime);
                lr.SetColors(new Color(1, 1, 1, alpha), new Color(1, 1, 1, alpha));
                if (alpha == moveToFloat)
                {

                    if (destroyOnEnd) Destroy(gameObject);
                    else Destroy(this);

                }
            }
            if (moveObject == MoveObject.scale)
            {
                alpha = Mathf.MoveTowards(alpha, moveToFloat, speed * Time.unscaledDeltaTime);
                myTransform.localScale = new Vector3(alpha, alpha, alpha);
                if (alpha == moveToFloat)
                {

                    if (destroyOnEnd) Destroy(gameObject);
                    else Destroy(this);

                }
            }
        }
        void delConcurents()
        {
            TowardMover[] tw = gameObject.GetComponents<TowardMover>();
            foreach (var t in tw)
            {
                if (t != this) Destroy(t);
            }
        }
        /// <summary>
        /// УСТАРЕЛА
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="speed"></param>
        /// <param name="destroyEnd"></param>
        public void setParams(MoveObject mo, float from, float to, float speed, bool destroyEnd = true)
        {
            moveToFloat = to;
            moveFromFloat = from;
            this.speed = speed;
            moveObject = mo;
            destroyOnEnd = destroyEnd;
        }
        public static void Add(GameObject target, MoveObject mo, float from, float to, float speed, bool destroyEnd = true)
        {
           var tm = target.GetComponent<TowardMover>();
            if (tm != null) if (tm.moveObject == mo) Destroy(tm);
            target.AddComponent<TowardMover>().setParams(mo, from, to, speed, destroyEnd);
        }

    }
}