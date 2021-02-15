using UnityEngine;

[System.Serializable]
public class StanceInfo : ScriptableObject
{
    public CombatStance combatStance = CombatStance.Stance1;
    public MoveInfo cinematicIntro;
    public MoveInfo cinematicOutro;

    public BasicMoves basicMoves = new BasicMoves();
    public MoveInfo[] attackMoves = new MoveInfo[0];

    public MoveSetData ConvertData()
    {
        MoveSetData moveSet = new MoveSetData();
        moveSet.combatStance = this.combatStance;
        moveSet.cinematicIntro = this.cinematicIntro;
        moveSet.cinematicOutro = this.cinematicOutro;
        moveSet.basicMoves = this.basicMoves;
        moveSet.attackMoves = this.attackMoves;

        return moveSet;
    }
}