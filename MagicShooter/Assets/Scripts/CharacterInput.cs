using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    [Header("Keys")]
    public KeyCode SprintKey;
    public KeyCode SpellMenuKey;
    public KeyCode PauseKey;

    public static float MouseInputX;
    public static float MouseInputY;

    public static float MoveInputX;
    public static float MoveInputY;
    public static bool IsMoving;


    public static bool Sprint;

    public static bool LeftMouseButton;
    public static bool RightMouseButton;
    public static float MouseScroll;


    public static bool SpellMenu;
    public static bool PauseInput;

    public static bool Jump;

    public bool InputOn = true;
    public static bool MouseInputAllowed = true;
    public static bool MoveInputAllowed = true;
    public static bool ShootInputAllowed = true;
    public static bool AllInputAllowed = true;

    public void Start()
    {
        MouseInputAllowed = true;
        MoveInputAllowed = true;
        ShootInputAllowed = true;
        AllInputAllowed = true;
    }

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
        GetMouseInput();
        GetMoveInput();
        GetShootInput();

        SpellMenu = Input.GetKey(SpellMenuKey);

        if (Input.GetKeyDown(PauseKey) && !LevelController.Instance.GameEnd) PauseManager.TogglePause();
    }

    private void GetMouseInput()
    {
        if (MouseInputAllowed && AllInputAllowed)
        {
            MouseInputX = Input.GetAxis(GlobalStrings.MouseXInput);
            MouseInputY = Input.GetAxis(GlobalStrings.MouseYInput);
        }
        else
        {
            MouseInputX = 0;
            MouseInputY = 0;
        }
    }

    private void GetMoveInput()
    {
        if (MoveInputAllowed && AllInputAllowed)
        {
            if (Input.GetButtonDown(GlobalStrings.JumpInput)) Jump = true;

            MoveInputX = Input.GetAxisRaw(GlobalStrings.HorizontalInput);
            MoveInputY = Input.GetAxisRaw(GlobalStrings.VerticalInput);

            Sprint = Input.GetKey(SprintKey);
        }
        else
        {
            Jump = false;
            MoveInputX = 0;
            MoveInputY = 0;
            Sprint = false;
        }

        if (MoveInputX != 0 || MoveInputY != 0) IsMoving = true;
        else IsMoving = false;
    }

    private void GetShootInput()
    {
        if (ShootInputAllowed && AllInputAllowed)
        {
            LeftMouseButton = Input.GetMouseButton(0);
            RightMouseButton = Input.GetMouseButton(1);
            MouseScroll = Input.GetAxis(GlobalStrings.MouseScrollWheel);
        }
        else
        {
            LeftMouseButton = false;
            RightMouseButton = false;
            MouseScroll = 0;
        }
    }
}
