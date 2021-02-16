using System;

[Flags]
public enum NetworkButtonPress
{
    None = 0,
    Forward = 1 << 0,
    Back = 1 << 1,
    Up = 1 << 2,
    Down = 1 << 3,
    Button1 = 1 << 4,
    Button2 = 1 << 5,
    Button3 = 1 << 6,
    Button4 = 1 << 7,

    // 16bits network packages required
    Button5 = 1 << 8,
    Button6 = 1 << 9,
    Button7 = 1 << 10,
    Button8 = 1 << 11,
    Button9 = 1 << 12,
    Button10 = 1 << 13,
    Button11 = 1 << 14,
    Button12 = 1 << 15,

    // 32bits network packages required
    Start = 1 << 16,
}