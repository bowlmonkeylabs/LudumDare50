using TMPro;
using UnityEngine;
using BML.ScriptableObjectCore.Scripts.Variables;
using BML.ScriptableObjectCore.Scripts.Events;

public class HoverTextDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text hoverTextDisplay;
    [SerializeField] private BoolReference IsHoverTextOpen;
    [SerializeField] private GameEvent onUnHover;
    [SerializeField] private DynamicGameEvent onShowHoverText;

    private void Awake()
    {
        onUnHover.Subscribe(OnUnHover);
        onShowHoverText.Subscribe(OnShowHoverText);
    }

    private void OnDisable()
    {
        onUnHover.Unsubscribe(OnUnHover);
        onShowHoverText.Unsubscribe(OnShowHoverText);
    }

    public void OnShowHoverText(object text, object prevText)
    {
        hoverTextDisplay.text = (string)text;
        hoverTextDisplay.gameObject.SetActive(true);
        IsHoverTextOpen.Value = true;
    }

    public void OnUnHover()
    {
        hoverTextDisplay.gameObject.SetActive(false);
        IsHoverTextOpen.Value = false;
    }
}
