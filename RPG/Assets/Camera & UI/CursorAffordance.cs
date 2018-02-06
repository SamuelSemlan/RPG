using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D attackCursor = null;
    [SerializeField] Texture2D debugCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

	CameraRaycaster cameraRaycaster;

	void Start () {
		cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += OnLayerChanged; // Registering
	}
	
	void OnLayerChanged (Layer newLayer) {
        switch (newLayer)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.ForceSoftware);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.ForceSoftware);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(debugCursor, cursorHotspot, CursorMode.ForceSoftware);
                break;
            default:
                print("Something is wrong");
                return;
        }
	}

    // TODO consider de-registering OnLayerChange on leaving all game scenes
}
