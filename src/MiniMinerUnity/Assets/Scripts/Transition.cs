using System;
using UnityEngine;
using UnityEngine.UI;

namespace MiniMinerUnity
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Transition : MonoBehaviour
    {
        private Renderer target;
        private MaterialPropertyBlock propertyBlock;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void SetTime(float time)
        {
            if (target == null)
            {
                target = GetComponent<Renderer>();
            }
            if (time < 0.01f)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);

                if (propertyBlock == null)
                {
                    propertyBlock = new MaterialPropertyBlock();
                }

                target.GetPropertyBlock(propertyBlock);
                propertyBlock?.SetFloat("_animateTime", Mathf.Clamp01(time));
                target.SetPropertyBlock(propertyBlock);
            }
        }
    }
}
