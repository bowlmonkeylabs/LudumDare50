using TMPro;
using UnityEngine;
using BML.ScriptableObjectCore.Scripts.Events;

public class HoverTextDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text hoverTextDisplay;
    [SerializeField] private GameEvent onUnHover;
    [SerializeField] private DynamicGameEvent onShowHoverText;

    private void Awake()
    {
        onUnHover.Subscribe(OnUnHover);
        onShowHoverText.Subscribe(OnShowHoverText);
    }

    public void OnShowHoverText(object text, object prevText)
    {
        hoverTextDisplay.text = (string)text;
        hoverTextDisplay.gameObject.SetActive(true);
    }

    public void OnUnHover()
    {
        hoverTextDisplay.gameObject.SetActive(false);
    }
}
