using UnityEngine;

namespace ECS
{
    public class SoundTrigger : MonoBehaviour
    {
        public AudioSource sound;
        public bool playOnce;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                sound.Play();
                if (playOnce) Destroy(gameObject);
            }
        }
    }
}