using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace I0plus.XduiUnity
{
    public class ScrollRectEx : ScrollRect
    {
        private bool _routeToParent;

        /// <summary>
        ///     Always route initialize potential drag event to parents
        /// </summary>
        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData,
                ExecuteEvents.initializePotentialDrag);
            base.OnInitializePotentialDrag(eventData);
        }

        /// <summary>
        ///     Drag event
        /// </summary>
        public override void OnDrag(PointerEventData eventData)
        {
            if (_routeToParent)
                ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
            else
                base.OnDrag(eventData);
        }

        /// <summary>
        ///     Begin drag event
        /// </summary>
        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (!horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y))
                _routeToParent = true;
            else if (!vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
                _routeToParent = true;
            else
                _routeToParent = false;

            if (_routeToParent)
                ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
            else
                base.OnBeginDrag(eventData);
        }

        /// <summary>
        ///     End drag event
        /// </summary>
        public override void OnEndDrag(PointerEventData eventData)
        {
            if (_routeToParent)
                ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
            else
                base.OnEndDrag(eventData);
            _routeToParent = false;
        }
    }
}