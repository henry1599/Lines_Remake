using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverEffect;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable == false)
        {
            return;
        }
        if (hoverEffect != null)
        {
            hoverEffect.SetActive(true);
        }
        FindObjectOfType<AudioManager>().Play("Click", false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable == false)
        {
            return;
        }
        if (hoverEffect != null)
        {
            hoverEffect.SetActive(false);
        }
    }
}
