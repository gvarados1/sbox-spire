namespace Spire.Abilities;

public partial class ConeArrowAttack : BasicArrowAttack
{
	// Configuration
	public override float Cooldown => 10f;
	public override string AbilityName => "Cone Attack";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/arrow_cone_attack.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Special;
	public override float PlayerSpeedMultiplier => 0.1f;
	public override float AbilityDuration => 0.5f;

	protected override void PostAbilityExecute()
	{
		base.PostAbilityExecute();

		if ( Host.IsClient )
			return;

		CreateProjectile( -15f );
		CreateProjectile( 0f );
		CreateProjectile( 15f );
	}
}
