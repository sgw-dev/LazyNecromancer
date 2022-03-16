using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using fancy.component.model;

namespace fancy.component
{
    public class EventComponent : MonoBehaviour
    {
        [SerializeField] float duration = .5f;

        [SerializeField] List<IProp> onPointerDownProp;
        [SerializeField] List<IProp> onPointerUpProp;
        [SerializeField] List<IProp> onPointerEnterProp;
        [SerializeField] List<IProp> onPointerExitProp;

        Dictionary<Type, IComponent> components;

        private void Start()
        {
            components = new Dictionary<Type, IComponent>();
            onPointerDownProp.ForEach(prop => AddComponent(prop));
            onPointerUpProp.ForEach(prop => AddComponent(prop));
            onPointerEnterProp.ForEach(prop => AddComponent(prop));
            onPointerExitProp.ForEach(prop => AddComponent(prop));
        }

        void AddComponent(IProp prop)
        {
            Type type = prop.GetType();
            if (!components.ContainsKey(type))
            {
                components.Add(type, prop.Initialize(gameObject));
            }
        }

        public void OnPointerDown()
        {
            StopAllCoroutines();
            onPointerDownProp.ForEach(prop => components[prop.GetType()].StartEvent(prop));
            StartCoroutine(PlayEvent(onPointerDownProp));
        }

        public void OnPointerUp()
        {
            StopAllCoroutines();
            onPointerUpProp.ForEach(prop => components[prop.GetType()].StartEvent(prop));
            StartCoroutine(PlayEvent(onPointerUpProp));
        }

        public void OnPointerEnter()
        {
            StopAllCoroutines();
            onPointerEnterProp.ForEach(prop => components[prop.GetType()].StartEvent(prop));
            StartCoroutine(PlayEvent(onPointerEnterProp));
        }

        public void OnPointerExit()
        {
            StopAllCoroutines();
            onPointerExitProp.ForEach(prop => components[prop.GetType()].StartEvent(prop));
            StartCoroutine(PlayEvent(onPointerExitProp));
        }

        IEnumerator PlayEvent(List<IProp> props)
        {
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float step = elapsedTime / duration;
                props.ForEach(prop => components[prop.GetType()].Evalulate(step));
                yield return null;
            }
            props.ForEach(prop => components[prop.GetType()].EndEvent());
        }
    }
}