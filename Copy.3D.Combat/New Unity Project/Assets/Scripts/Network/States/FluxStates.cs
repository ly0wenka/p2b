using System.Collections.Generic;
using FPLibrary;

public struct FluxStates{
	#region public instance properties
	public long networkFrame {get; set;}

    public Dictionary<System.Reflection.MemberInfo, System.Object> tracker;

    // Deprecated Below
    public GlobalState global;
    public GUIState battleGUI;
    public CameraState camera;
    public CharacterState player1;
    public CharacterState player2;
	#endregion

	#region public instance methods
	public void Override(FluxSimpleState state){
		this.player1.life							= state.p1.life;
		this.player1.gauge							= state.p1.gauge;
		this.player1.shellTransform.position		= state.p1.position;
		this.player1.shellTransform.fpPosition		= FPVector.ToFPVector(state.p1.position);

		this.player2.life							= state.p2.life;
		this.player2.gauge							= state.p2.gauge;
		this.player2.shellTransform.position		= state.p2.position;
        this.player2.shellTransform.fpPosition      = FPVector.ToFPVector(state.p2.position);

        this.networkFrame = state.frame;
	}
	#endregion
}
