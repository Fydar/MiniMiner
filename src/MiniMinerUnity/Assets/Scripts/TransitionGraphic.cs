using System;
using UnityEngine;
using UnityEngine.UI;

namespace MiniMinerUnity
{
    [RequireComponent(typeof(Graphic))]
    public class TransitionGraphic : MonoBehaviour
    {
        private Graphic graphic;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void SetTime(float time)
        {
            if (graphic == null)
            {
                graphic = GetComponent<Graphic>();
                graphic.material = Instantiate(graphic.material);
            }
            if (time < 0.01f)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                graphic.material.SetFloat("_animateTime", Mathf.Clamp01(time));
            }
        }
    }
}
