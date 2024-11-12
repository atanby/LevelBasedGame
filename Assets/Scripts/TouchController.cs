using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchController : MonoBehaviour
{
    private Vector2 startPosition;
    private int touchedGridIndex;
    private bool callbackSent;
    private const float swipeThreshold = 0.2f;

    void Start() {
        callbackSent = false;
    }

    void Update() {
	    // Handle native touch events
        foreach (Touch touch in Input.touches) {
            HandleTouch(touch.fingerId, Camera.main.ScreenToWorldPoint(touch.position), touch.phase);
        }

	    // Simulate touch events from mouse events
	    if (Input.touchCount == 0) {
            if (Input.GetMouseButtonDown(0) ) {
                HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Began);
            }
            if (Input.GetMouseButton(0) ) {
                HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Moved);
            }
            if (Input.GetMouseButtonUp(0) ) {
                HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Ended);
            }
    }

    void HandleTouch(int touchFingerId, Vector2 touchPosition, TouchPhase touchPhase) {
        switch (touchPhase) {
        case TouchPhase.Began:
            startPosition = touchPosition;
            touchedGridIndex = GameManager.gridLayout.getClickedElementIndex(touchPosition);
            break;
        case TouchPhase.Moved:
            if(callbackSent) {
                return;
            }
            else {
                if(startPosition.x - touchPosition.x >= swipeThreshold) {
                    GameManager.gridLayout.SwapElements(touchedGridIndex, SwipeDirection.Left);
                }
                else if(startPosition.x - touchPosition.x < -swipeThreshold) {
                    GameManager.gridLayout.SwapElements(touchedGridIndex, SwipeDirection.Right);
                }
                else if(startPosition.y - touchPosition.y >= swipeThreshold) {
                    GameManager.gridLayout.SwapElements(touchedGridIndex, SwipeDirection.Down);
                }
                else if(startPosition.y - touchPosition.y < -swipeThreshold) {
                    GameManager.gridLayout.SwapElements(touchedGridIndex, SwipeDirection.Up);
                }
                else {
                    return;
                }
                callbackSent = true;
                GameManager.score += GameManager.gridLayout.checkIfRowsCompleted();
                if(GameManager.gridLayout.checkIfThereAreMoveLeft()) {
                    //There are possible moves
                    Debug.Log("There are possible moves!");
                }
                else {
                    //No possible moves to match a row
                    //Call level is over
                    SceneManager.LoadScene("MainScene");
                    Debug.Log("There are not!");
                }
            }
            break;
        case TouchPhase.Ended:
            callbackSent = false;
            break;
            }
        }
    }
}
