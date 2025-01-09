using UnityEngine;

public class LockUnlockMouse : MonoBehaviour
{
    public void Lock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnLock()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
}
