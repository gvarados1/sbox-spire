namespace Spire;

public partial class BaseCarriable : Sandbox.BaseCarriable
{
	public virtual string ModelPath => "";
	public virtual HoldType HoldType => HoldType.None;
	public virtual HoldHandedness HoldHandedness => HoldHandedness.TwoHands;

	public override void Spawn()
	{
		base.Spawn();

		if ( !string.IsNullOrEmpty( ModelPath ) )
		{
			SetModel( ModelPath );
		}
	}


	public override void SimulateAnimator( PawnAnimator anim )
	{
		base.SimulateAnimator( anim );

		anim.SetAnimParameter( "holdtype", (int)HoldType );
		anim.SetAnimParameter( "holdtype_handedness", (int)HoldHandedness );
	}
}
