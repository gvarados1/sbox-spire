namespace Spire.Abilities;

public partial class ConeArrowAttack : BasicArrowAttack
{
	// Configuration
	public override string Identifier => "cone_arrow_attack";
	public override WeaponAbilityType Type => WeaponAbilityType.Special;

	protected override void PostRun()
	{
		base.PostRun();

		if ( Host.IsClient )
			return;

		CreateProjectile( -15f );
		CreateProjectile( 0f );
		CreateProjectile( 15f );
	}
}
