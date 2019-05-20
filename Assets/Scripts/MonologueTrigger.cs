using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{
    [Multiline] public string monologue;
    public bool playOnce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) MonologueManager.main.OpenMonologue(monologue);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MonologueManager.main.CloseMonologue();
            if (playOnce) Destroy(gameObject);
        }
    }
}