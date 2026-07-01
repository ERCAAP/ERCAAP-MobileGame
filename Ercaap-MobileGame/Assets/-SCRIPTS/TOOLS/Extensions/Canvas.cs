using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LunaGames.Extentions
{
    public static class Canvas
    {
        private static PointerEventData _eventDataCurrentPosition;
        private static List<RaycastResult> _results;


        public static bool IsOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }

        public static Vector3 GetWorldPosition(this RectTransform canvasElement)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasElement, canvasElement.position, Camera.main, out var result);
            return result;
        }
        /// <summary>
        /// Overlay Canvas üzerinde bulunan bir objeden fýrlatýlan bir ýþýnýn sanal bir düzlemde çartýðý konumu verir.
        /// </summary>
        /// <param name="UIElement">3D uzayda denk konumu bulunmasý istenilen UI Elementi</param>
        /// <param name="planePos">UI Elementin konmuna denk olmasý istenilen düzlem konumu</param>
        /// <param name="planeUp">UI Elementin konmuna denk olmasý istenilen düzlem eðimi</param>
        /// <returns></returns>
        public static Vector3 GetOverlayToWorldPosition(this RectTransform UIElement, Vector3 planePos, Vector3 planeUp)
        {
            Plane plane = new Plane();
            plane.SetNormalAndPosition(planeUp, planePos);

            Ray ray = Camera.main.ScreenPointToRay(UIElement.position);
            float distanceFromUIElement;
            plane.Raycast(ray, out distanceFromUIElement);

            return ray.GetPoint(distanceFromUIElement);
        }
        public static Vector2 GetScreenPosition(this Transform worldElement)
        {
            return (Vector2)(Camera.main.WorldToScreenPoint(worldElement.position));
        }
    }
}
