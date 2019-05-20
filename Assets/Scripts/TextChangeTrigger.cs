using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class TextChangeTrigger : MonoBehaviour
{
    [Multiline] public string text;
    public TMP_Text textComponent;
    public bool playOnce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textComponent.text = text;
            if (playOnce) Destroy(gameObject);
        }
    }
}