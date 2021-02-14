using FPLibrary;

public class ButtonSequenceRecord
{
    #region trackable definitions

    public ButtonPress buttonPress;
    public Fix64 chargeTime;

    #endregion

    public ButtonSequenceRecord(ButtonPress buttonPress, Fix64 chargeTime)
    {
        this.buttonPress = buttonPress;
        this.chargeTime = chargeTime;
    }
}