using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class NetworkButtonPressExtensions
{
    public static NetworkButtonPress ToNetworkButtonPress(this ButtonPress button)
    {
        switch (button)
        {
            case ButtonPress.Up: return NetworkButtonPress.Up;
            case ButtonPress.Down: return NetworkButtonPress.Down;
            case ButtonPress.Back: return NetworkButtonPress.Back;
            case ButtonPress.Forward: return NetworkButtonPress.Forward;
            case ButtonPress.Button1: return NetworkButtonPress.Button1;
            case ButtonPress.Button2: return NetworkButtonPress.Button2;
            case ButtonPress.Button3: return NetworkButtonPress.Button3;
            case ButtonPress.Button4: return NetworkButtonPress.Button4;
            case ButtonPress.Button5: return NetworkButtonPress.Button5;
            case ButtonPress.Button6: return NetworkButtonPress.Button6;
            case ButtonPress.Button7: return NetworkButtonPress.Button7;
            case ButtonPress.Button8: return NetworkButtonPress.Button8;
            case ButtonPress.Button9: return NetworkButtonPress.Button9;
            case ButtonPress.Button10: return NetworkButtonPress.Button10;
            case ButtonPress.Button11: return NetworkButtonPress.Button11;
            case ButtonPress.Button12: return NetworkButtonPress.Button12;
            case ButtonPress.Start: return NetworkButtonPress.Start;
            default: return NetworkButtonPress.None;
        }
    }

    public static NetworkButtonPress ToNetworkButtonPress(this IEnumerable<ButtonPress> buttons)
    {
        NetworkButtonPress n = NetworkButtonPress.None;

        if (buttons != null)
        {
            foreach (ButtonPress button in buttons)
            {
                n |= button.ToNetworkButtonPress();
            }
        }

        return n;
    }

    public static ReadOnlyCollection<ButtonPress> ToButtonPresses(this NetworkButtonPress buttonPresses)
    {
        List<ButtonPress> list = new List<ButtonPress>();

        if (buttonPresses != NetworkButtonPress.None)
        {
            if ((buttonPresses & NetworkButtonPress.Up) != 0) list.Add(ButtonPress.Up);
            if ((buttonPresses & NetworkButtonPress.Down) != 0) list.Add(ButtonPress.Down);
            if ((buttonPresses & NetworkButtonPress.Back) != 0) list.Add(ButtonPress.Back);
            if ((buttonPresses & NetworkButtonPress.Forward) != 0) list.Add(ButtonPress.Forward);
            if ((buttonPresses & NetworkButtonPress.Button1) != 0) list.Add(ButtonPress.Button1);
            if ((buttonPresses & NetworkButtonPress.Button2) != 0) list.Add(ButtonPress.Button2);
            if ((buttonPresses & NetworkButtonPress.Button3) != 0) list.Add(ButtonPress.Button3);
            if ((buttonPresses & NetworkButtonPress.Button4) != 0) list.Add(ButtonPress.Button4);
            if ((buttonPresses & NetworkButtonPress.Button5) != 0) list.Add(ButtonPress.Button5);
            if ((buttonPresses & NetworkButtonPress.Button6) != 0) list.Add(ButtonPress.Button6);
            if ((buttonPresses & NetworkButtonPress.Button7) != 0) list.Add(ButtonPress.Button7);
            if ((buttonPresses & NetworkButtonPress.Button8) != 0) list.Add(ButtonPress.Button8);
            if ((buttonPresses & NetworkButtonPress.Button9) != 0) list.Add(ButtonPress.Button9);
            if ((buttonPresses & NetworkButtonPress.Button10) != 0) list.Add(ButtonPress.Button10);
            if ((buttonPresses & NetworkButtonPress.Button11) != 0) list.Add(ButtonPress.Button11);
            if ((buttonPresses & NetworkButtonPress.Button12) != 0) list.Add(ButtonPress.Button12);
            if ((buttonPresses & NetworkButtonPress.Start) != 0) list.Add(ButtonPress.Start);
        }

        return new ReadOnlyCollection<ButtonPress>(list);
    }
}