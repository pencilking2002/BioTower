using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleExplosion : MonoBehaviour
    {
        private ParticleSystem _ps;
        private ParticleSystem ps
        {
            get
            {
                if (!_ps)
                    _ps = GetComponent<ParticleSystem>();
                return _ps;
            }
        }

        public void Play()
        {
            transform.SetParent(null);
            var length = ps.main.duration;
            ps.Play();
            Destroy(gameObject, length);
        }
    }
}
