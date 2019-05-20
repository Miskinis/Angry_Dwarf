using TMPro;
using UnityEngine;

public class MonologueManager : MonoBehaviour
{
    public static MonologueManager main;
    public GameObject monologuePanel;
    public TMP_Text monologueTextComponent;

    private void Awake()
    {
        if (main == null)
            main = this;
        else
            Destroy(gameObject);
    }

    public void OpenMonologue(string text)
    {
        monologueTextComponent.text = text;
        monologuePanel.SetActive(true);
    }

    public void CloseMonologue()
    {
        monologuePanel.SetActive(false);
    }
}