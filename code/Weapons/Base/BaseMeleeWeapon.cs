namespace Spire;

public partial class BaseMeleeWeapon : BaseWeapon
{
	public override float PrimaryRate => 1f;
	public override void AttackPrimary()
	{
		base.AttackPrimary();

		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
	}
}
