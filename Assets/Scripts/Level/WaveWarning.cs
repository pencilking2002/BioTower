using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

namespace BioTower
{
    public class WaveWarning : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Polyline[] lines;

        public void Animate()
        {
            animator.Play("Move");
        }

        public void FadeOut()
        {
            animator.Play("FadeOut");
            Debug.Log("Play fade out");
        }
    }
}
