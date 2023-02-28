using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventroy
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemShadow : MonoBehaviour
    {
        public SpriteRenderer itemSprite;

        private SpriteRenderer ShadowSprite;

        private void Awake()
        {
            ShadowSprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            ShadowSprite.sprite = itemSprite.sprite;
            ShadowSprite.color = new Color(0, 0, 0, 0.3f);
        }
    }
}

