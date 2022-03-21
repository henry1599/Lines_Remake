using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int gameLevel;
    [SerializeField] private bool isIngame;
    [SerializeField] private InfoLevelDisplay infoLevelDisplay;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isIngame)
        {
            infoLevelDisplay.SetupIngame(SettingManager.Instance.ChosenLevel);
        }
        else
        {
            infoLevelDisplay.Setup(SettingManager.Instance.GetLevels()[gameLevel]);
        }
        infoLevelDisplay.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoLevelDisplay.gameObject.SetActive(false);
    }
}
