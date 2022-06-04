using Spire.Abilities;

namespace Spire;

public partial class CrossbowWeapon : BaseWeapon
{
	public override HoldType HoldType => HoldType.Shotgun;
	public override HoldType RelaxedHoldType => HoldType;
	public override HoldHandedness HoldHandedness => HoldHandedness.RightHand;

	public override string ModelPath => "weapons/rust_crossbow/rust_crossbow.vmdl";

	public override List<Type> Abilities => new()
	{
		typeof( BasicArrowAttack ),
		typeof( ExplosiveArrowAttack ),
		typeof( ConeArrowAttack ),
	};

	public override void SimulateAnimator( PawnAnimator anim )
	{
		base.SimulateAnimator( anim );
	}
}
