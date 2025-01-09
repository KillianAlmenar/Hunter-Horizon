using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDropdownScroller : MonoBehaviour, ISelectHandler
{
    private ScrollRect m_Rect;
    private float scrollPosition = 1;

    // Start is called before the first frame update
    void Start()
    {
        m_Rect = GetComponentInParent<ScrollRect>(true);

        int childCount = m_Rect.content.transform.childCount - 1;
        int childIndex = transform.GetSiblingIndex();

        childIndex = childIndex < ((float)childCount / 2f) ? childIndex - 1 : childIndex;

        scrollPosition = 1 - ((float)childIndex / childCount);

        m_Rect.verticalScrollbar.value = scrollPosition;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (m_Rect)
            m_Rect.verticalScrollbar.value = scrollPosition;
    }
}

