using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private bool _isPaused;

    [SerializeField] private GameObject _pauseMenu;

    [Header("UI Panels")]
    [SerializeField] private GameObject _buildPanel;

    [Header("UI Focus Assignments")]
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _defaultBuildButton;

    public void Pause()
    {
        _isPaused = true;
        _pauseMenu.SetActive(true);

        if (_buildPanel != null)
        {
            _buildPanel.SetActive(false);
        }

        Time.timeScale = 0;

        EventSystem.current.SetSelectedGameObject(null);
        if (_resumeButton != null)
        {
            _resumeButton.Select();
            EventSystem.current.SetSelectedGameObject(_resumeButton.gameObject);
        }
    }

    public void Resume()
    {
        _isPaused = false;
        _pauseMenu.SetActive(false);

        if (_buildPanel != null)
        {
            _buildPanel.SetActive(true);
        }

        Time.timeScale = 1;

        StartCoroutine(ForceGamepadSelectNextFrame());
    }

    // Brute-force coroutine to reactivate the Gamepad Input loop without mouse intervention
    private IEnumerator ForceGamepadSelectNextFrame()
    {
        // 1. Completely strip out any residual mouse hover states
        EventSystem.current.SetSelectedGameObject(null);

        // 2. Wait two frames for the canvas geometry and layout system to compile
        yield return null;
        yield return null;

        if (_defaultBuildButton != null)
        {
            // 3. Forcibly assign the target button object to the active EventSystem frame slot
            _defaultBuildButton.Select();
            EventSystem.current.SetSelectedGameObject(_defaultBuildButton.gameObject);

            // 4. FIX: Simulate a tiny dummy navigation signal to trick the New Input System UI module 
            // into believing a controller button was physically pressed, instantly waking up the gamepad loop.
            AxisEventData fakeInputData = new AxisEventData(EventSystem.current);
            fakeInputData.moveDir = MoveDirection.None; // Keeps selection right on the button

            ExecuteEvents.Execute(_defaultBuildButton.gameObject, fakeInputData, ExecuteEvents.moveHandler);
        }
    }

    public void Toggle(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _isPaused = !_isPaused;
            _pauseMenu.SetActive(_isPaused);

            if (_buildPanel != null)
            {
                _buildPanel.SetActive(!_isPaused);
            }

            Time.timeScale = _isPaused ? 0 : 1;

            if (_isPaused)
            {
                EventSystem.current.SetSelectedGameObject(null);
                if (_resumeButton != null) _resumeButton.Select();
            }
            else
            {
                StartCoroutine(ForceGamepadSelectNextFrame());
            }
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("TitleScreenScene");
        Time.timeScale = 1;
    }
}
