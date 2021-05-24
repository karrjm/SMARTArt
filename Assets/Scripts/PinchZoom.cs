using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinchZoom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isDragging;
    private float _currentScale;
    public float minScale, maxScale;
    private float _temp = 0;
    private float _scalingRate = 2;

    private void Start()
    {
        _currentScale = transform.localScale.x;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.touchCount == 1)
        {
            _isDragging = true;
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }


    private void Update()
    {
        if (_isDragging)
            if (Input.touchCount == 2)
            {
                transform.localScale = new Vector2(_currentScale, _currentScale);
                float distance = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                if (_temp > distance)
                {
                    if (_currentScale < minScale)
                        return;
                    _currentScale -= (Time.deltaTime) * _scalingRate;
                }

                else if (_temp < distance)
                {
                    if (_currentScale > maxScale)
                        return;
                    _currentScale += (Time.deltaTime) * _scalingRate;
                }

                _temp = distance;
            }
    }
}
