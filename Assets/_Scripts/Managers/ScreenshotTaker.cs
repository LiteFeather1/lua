using UnityEngine;

#if UNITY_EDITOR
public class ScreenshotTaker : MonoBehaviour
{
    private int _session;
    private int _run;
    private int _screenShotsTaken;

    private void Awake()
    {
        _session = PlayerPrefsHelper.GetSessions();
        _run = PlayerPrefsHelper.GetRuns();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
            ScreenCapture.CaptureScreenshot($"Assets/Screenshots/Session_{_session}_Run_{_run}_{_screenShotsTaken++}.png");
    }
}
#endif