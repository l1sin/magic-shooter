using UnityEngine;

public class CharacterInput : MonoBehaviour
{

    public static float MouseInputX;
    public static float MouseInputY;

    public static float MoveInputX;
    public static float MoveInputY;
    public static bool IsMoving;

    public KeyCode SprintKey;
    public static bool Sprint;

    public static bool LeftMouseButton;
    public static bool RightMouseButton;
    public static float MouseScroll;

    public KeyCode SpellMenuKey;
    public static bool SpellMenu;

    public static bool Jump;

    public bool InputOn = true;
    public static bool MouseInputAllowed = true;

    private void Update()
    {
        if (InputOn)
        {
            GetInput();
        }
        else
        {
            MouseInputX = 0;
            MouseInputY = 0;
            MoveInputX = 0;
            MoveInputY = 0;
        }
    }

    private void FixedUpdate()
    {
        Jump = false;
    }

    private void GetInput()
    {
        if (Input.GetButtonDown(GlobalStrings.JumpInput))
        {
            Jump = true;
        }

        if (MouseInputAllowed)
        {
            MouseInputX = Input.GetAxis(GlobalStrings.MouseXInput);
            MouseInputY = Input.GetAxis(GlobalStrings.MouseYInput);
        }
        else
        {
            MouseInputX = 0;
            MouseInputY = 0;
        }

        MoveInputX = Input.GetAxisRaw(GlobalStrings.HorizontalInput);
        MoveInputY = Input.GetAxisRaw(GlobalStrings.VerticalInput);

        LeftMouseButton = Input.GetMouseButton(0);
        RightMouseButton = Input.GetMouseButton(1);
        MouseScroll = Input.GetAxis(GlobalStrings.MouseScrollWheel);

        SpellMenu = Input.GetKey(SpellMenuKey);

        Sprint = Input.GetKey(SprintKey);
        if (MoveInputX != 0 || MoveInputY != 0) IsMoving = true;
        else IsMoving = false;
    }
}
