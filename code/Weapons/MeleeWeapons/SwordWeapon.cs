using Spire.Abilities;

namespace Spire;

public partial class SwordWeapon : BaseWeapon
{
	public override HoldType HoldType => HoldType.Item;
	public override HoldHandedness HoldHandedness => HoldHandedness.RightHand;

	public override string ModelPath => "assets/weapons/basic_sword.vmdl";

	// Abilities
	public override Type AttackAbilityType => typeof( SwordSlash );
	public override Type SpecialAbilityType => typeof( SwordHeavyAttack );
	// End of abilities

	public override void SimulateAnimator( PawnAnimator anim )
	{
		base.SimulateAnimator( anim );

		anim.SetAnimParameter( "holdtype_pose_hand", 0.07f );
	}
}
