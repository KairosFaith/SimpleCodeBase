using UnityEngine;
public class OldInputGamepadMap : MonoBehaviour
{
    public const KeyCode
        South = KeyCode.JoystickButton0,
        East = KeyCode.JoystickButton1,
        West = KeyCode.JoystickButton2,
        North = KeyCode.JoystickButton3,
        L1 = KeyCode.JoystickButton4,
        R1 = KeyCode.JoystickButton5,
        Select = KeyCode.JoystickButton6,//left side of center section
        Start = KeyCode.JoystickButton7,
        LeftStickPush = KeyCode.JoystickButton8,
        RightStickPush = KeyCode.JoystickButton9;

    public float
        LeftStickHorizontal,//X axis
        LeftStickVertical,//Y axis
        RightStickHorizontal,//4th axis
        RightStickVertical,//5th axis
        DPadHorizontal,//6th axis
        DPadVertical,//7th axis
        L2Axis,//9th axis
        R2Axis;//10th axis
    void Update()
    {
        LeftStickHorizontal = Input.GetAxis("Horizontal");//X axis
        LeftStickVertical = Input.GetAxis("Vertical");//Y axis
        RightStickHorizontal = Input.GetAxis("Horizontal2");//4th axis
        RightStickVertical = Input.GetAxis("Vertical2");//5th axis
        L2Axis = Input.GetAxis("L2");//9th axis
        R2Axis = Input.GetAxis("R2");//10th axis
        DPadHorizontal = Input.GetAxis("DPadHorizontal");//6th axis
        DPadVertical = Input.GetAxis("DPadVertical");//7th axis
    }
}