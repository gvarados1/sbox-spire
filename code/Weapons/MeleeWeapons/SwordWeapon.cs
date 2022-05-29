using Spire.Abilities;

namespace Spire;

public partial class SwordWeapon : BaseMeleeWeapon
{
	public override HoldType HoldType => HoldType.Item;
	public override HoldHandedness HoldHandedness => HoldHandedness.RightHand;
	public override string ModelPath => "assets/weapons/basic_sword.vmdl";

	public override List<Type> Abilities => new()
	{
		typeof( SwordSlash ),
		typeof( SwordHeavyAttack ),
		typeof( SwordHeal )
	};

	public override void SimulateAnimator( PawnAnimator anim )
	{
		base.SimulateAnimator( anim );

		anim.SetAnimParameter( "holdtype_pose_hand", 0.07f );
	}
}
