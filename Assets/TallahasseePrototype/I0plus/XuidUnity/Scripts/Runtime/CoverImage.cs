using UnityEngine;
using UnityEngine.UI;

namespace I0plus.XduiUnity
{
    [ExecuteAlways]
    public class CoverImage : MonoBehaviour
    {
        private Image _cachedImage;
        private float _parentHeight;
        private float _parentWidth;
        private float _preferredHeight;
        private float _preferredWidth;

        protected Image CachedImage
        {
            get
            {
                if (_cachedImage == null) _cachedImage = GetComponent<Image>();
                return _cachedImage;
            }
        }

        private void Update()
        {
            var image = CachedImage;
            if (image == null) return;

            var parentTransform = transform.parent as RectTransform;
            var parentWidth = parentTransform.rect.width;
            var parentHeight = parentTransform.rect.height;
            var preferredWidth = image.preferredWidth;
            var preferredHeight = image.preferredHeight;

            if (parentWidth == _parentWidth && parentHeight == _parentHeight && preferredWidth == _preferredWidth &&
                preferredHeight == _preferredHeight)
            {
                return;
            }

            var narrow = parentHeight / parentWidth <= preferredHeight / preferredWidth;
            var rect = transform as RectTransform;
            rect.sizeDelta = narrow
                ? new Vector2(parentWidth, preferredHeight * parentWidth / preferredWidth)
                : new Vector2(preferredWidth * parentHeight / preferredHeight, parentHeight);

            _parentWidth = parentWidth;
            _parentHeight = parentHeight;
            _preferredWidth = preferredWidth;
            _preferredHeight = preferredHeight;
        }

        private void OnEnable()
        {
            var rect = transform as RectTransform;
            var center = new Vector2(0.5f, 0.5f);
            rect.anchorMin = center;
            rect.anchorMax = center;
        }
    }
}