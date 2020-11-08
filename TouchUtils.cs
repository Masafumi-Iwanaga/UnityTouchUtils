using UnityEditor;
using UnityEngine;

/// <summary>
/// Touch Utils provide common touch information for mobile devices and others
/// </summary>
public class TouchUtils : EditorWindow
{
    private static readonly bool IsAndroid = Application.platform == RuntimePlatform.Android;
    private static readonly bool IsIOS = Application.platform == RuntimePlatform.IPhonePlayer;
    private static bool IsUnityRemote = Menu.GetChecked("Tools/TouchControls/UseUnityRemote");
   
    /// <summary>
    /// Mobile flag
    /// </summary>
    private static bool IsMobile = IsAndroid || IsIOS || IsUnityRemote;

    private static Vector3 startPosition;
    private static Vector3 previousPosition;
    private static int currentFingerId;
    
    /// <summary>
    /// get touch count
    /// </summary>
    public static int TouchCount()
    {
        if (IsMobile)
        {
            return Input.touchCount;
        }else
        {
            if (Input.GetMouseButtonDown(0)) return 1;
            if (Input.GetMouseButton(0)) return 1;
            if (Input.GetMouseButtonUp(0)) return 1;
            return 0;
        }
    }

    /// <summary>
    /// get touch phase
    /// </summary>
    public static TouchUtilsPhase TouchPhase()
    {
        if (IsMobile)
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == UnityEngine.TouchPhase.Began)
                {
                    startPosition = (Vector3)touch.position;
                    currentFingerId = touch.fingerId;
                }
                return (TouchUtilsPhase)(int)touch.phase;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
                previousPosition = Input.mousePosition;
                return TouchUtilsPhase.Began;
            }
            else if (Input.GetMouseButton(0))
            {
                return TouchUtilsPhase.Moved;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                return TouchUtilsPhase.Ended;
            }
        }
        return TouchUtilsPhase.None;
    }
    
    /// <summary>
    /// get current touch position
    /// </summary>
    public static Vector3 TouchPosition()
    {
        if (TouchPhase() != TouchUtilsPhase.None)
        {
            if (IsMobile)
            {
                var touch = Input.GetTouch(0);
                if (touch.fingerId == currentFingerId)
                {
                    return (Vector3)touch.position;
                }
            }
            else
            {
                return Input.mousePosition;
            }
        }
        return  Vector3.zero;
    }

    /// <summary>
    /// get touch delta position
    /// </summary>
    public static Vector3 TouchDeltaPosition()
    {
        if (TouchPhase() != TouchUtilsPhase.None)
        {
            if (IsMobile)
            {
                var touch = Input.GetTouch(0);
                if (touch.fingerId == currentFingerId)
                {
                    return (Vector3)touch.deltaPosition;
                }
            }
            else
            {
                Vector3 currentPosition = Input.mousePosition;
                Vector3 deltaPosition = currentPosition - previousPosition;
                previousPosition = currentPosition;
                return deltaPosition;
            }
        }
        return Vector3.zero;
    }
    
    /// <summary>
    /// get vector from touch start position to current touch position
    /// </summary>
    public static Vector3 TouchMoveVector()
    {
        if (TouchPhase() != TouchUtilsPhase.None)
        {
            if (IsMobile)
            {
                var touch = Input.GetTouch(0);
                if (touch.fingerId == currentFingerId)
                {
                    return (Vector3)touch.position - startPosition;
                }
            }
            else
            {
                return Input.mousePosition - startPosition;
            }
        }
        return Vector3.zero;
    }
    
    /// <summary>
    /// extended touch phase added "None" to "UnityEngine.TouchPhase"
    /// </summary>
    public enum TouchUtilsPhase
    {
        None       = -1,
        Began      = 0,
        Moved      = 1,
        Stationary = 2,
        Ended      = 3,
        Canceled   = 4
    }
    
    /// <summary>
    /// menu to set whether to use Unity Remote
    /// </summary>
    [MenuItem("Tools/TouchControls/UseUnityRemote")]
    private static void SwitchUnityRemote()
    {
        string menuPath = "Tools/TouchControls/UseUnityRemote";
        IsUnityRemote = Menu.GetChecked(menuPath);
        IsUnityRemote = !IsUnityRemote;
        IsMobile = IsAndroid || IsIOS || IsUnityRemote;
        Menu.SetChecked(menuPath, IsUnityRemote);
    }
    
    
}

