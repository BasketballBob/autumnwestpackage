using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    [RequireComponent(typeof(RectTransform))]
    public class TextSizedPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private Vector2 _minSize = new Vector2(640, 480);
        [SerializeField]
        private Vector2 _9SlicingArea = Vector2.zero;

        private RectTransform _rectTrans;

        private void OnEnable()
        {
            _rectTrans = GetComponent<RectTransform>();
            SetNewSize(Vector2.zero);
            SyncPosition();
        }

        private void LateUpdate()
        {
            SetNewSize(_text.GetRenderedValues());
            SyncPosition();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Vector2.zero, _minSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector2.zero, GetComponent<RectTransform>().sizeDelta - _9SlicingArea);
        }

        private void SetNewSize(Vector2 newSize)
        {
            newSize = new Vector2(Mathf.Max(_minSize.x, newSize.x), Mathf.Max(_minSize.y, newSize.y));
            _rectTrans.sizeDelta = newSize + _9SlicingArea;
        }

        private void SyncPosition()
        {
            Vector3[] worldCorners = new Vector3[4];
            _text.rectTransform.GetWorldCorners(worldCorners);
            float centerX = worldCorners[2].x - worldCorners[1].x;

            switch (_text.verticalAlignment)
            {
                case VerticalAlignmentOptions.Top:
                    //_rectTrans.anchorMax = _text.rectTransform.anchorMax;
                    //_rectTrans.position = new Vector2(_text.rectTransform.position.x + _text.rectTransform.rect.center.x, _text.rectTransform.rect.yMax);
                    _rectTrans.position = worldCorners[1];
                    _rectTrans.anchoredPosition = new Vector2(0, _rectTrans.anchoredPosition.y + _9SlicingArea.y / 2);
                    Debug.DrawRay((Vector2)_text.rectTransform.position + _text.rectTransform.rect.max, Vector2.one, Color.magenta);
                    //_rectTrans.anchorMax = new Vector2(0, 1);
                    //_rectTrans.anchorMin = new Vector2(0, 1);
                    _rectTrans.pivot = new Vector2(.5f, 1);
                    
                    break;
                case VerticalAlignmentOptions.Middle:
                    break;
                case VerticalAlignmentOptions.Bottom:
                    break;
            }
        }
    }
}
