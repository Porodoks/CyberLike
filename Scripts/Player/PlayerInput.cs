using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float leftMouseBufferTimeMS = 50;
    private float leftMouseBufferTimer;

    private float spaceBufferTimeMS = 150;
    private float spaceBufferTimer;
    void Update()
    {
        leftMouseBufferTimer -= Time.deltaTime;
        spaceBufferTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0))
            leftMouseBufferTimer = leftMouseBufferTimeMS / 1000;

        if (Input.GetKeyDown(KeyCode.Space))
            spaceBufferTimer = spaceBufferTimeMS / 1000;
    }

    public bool LeftMouseButtonPressed()
    {
        return leftMouseBufferTimer > 0;
    }
    public bool SpacePressed()
    {
        return spaceBufferTimer > 0;
    }
    public bool CtrlDown()
    {
        return Input.GetKeyDown(KeyCode.LeftControl);
    }
    public bool CtrlPressed()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }
    public bool CtrlUp()
    {
        return Input.GetKeyUp(KeyCode.LeftControl);
    }
    public bool ShiftDown()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }
    public bool ShiftPressed()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
    public bool ShiftUp()
    {
        return Input.GetKeyUp(KeyCode.LeftShift);
    }
    public Vector2 GetInputRaw()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
