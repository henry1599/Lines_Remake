using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickUI : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable == false)
        {
            return;
        }
        FindObjectOfType<AudioManager>().Play("Click", false);
    }
}
