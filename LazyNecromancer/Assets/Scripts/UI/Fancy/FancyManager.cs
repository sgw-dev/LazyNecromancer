using fancy.component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace fancy
{
    public class FancyManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] UnityEvent onPointerDown;
        [SerializeField] UnityEvent onPointerUp;
        [SerializeField] UnityEvent onPointerEnter;
        [SerializeField] UnityEvent onPointerExit;

        List<EventComponent> eventComponents;


        private void Awake()
        {
            eventComponents = new List<EventComponent>(GetComponentsInChildren<EventComponent>());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            eventComponents.ForEach(component => component.OnPointerDown());
            onPointerDown.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            eventComponents.ForEach(component => component.OnPointerUp());
            onPointerUp.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            eventComponents.ForEach(component => component.OnPointerEnter());
            onPointerEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            eventComponents.ForEach(component => component.OnPointerExit());
            onPointerExit.Invoke();
        }
    }
}