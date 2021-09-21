using System;

public interface IVisitor
{
    void VisitLight(ILightUnit unit);
    void VisitArmored(IArmoredUnit unit);
}