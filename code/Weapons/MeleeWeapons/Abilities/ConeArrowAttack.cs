namespace Spire.Abilities;

public partial class ConeArrowAttack : BasicArrowAttack
{
	// Configuration
	public override float Cooldown => 10f;
	public override string Name => "Cone Attack";
	public override string Description => "";
	public override string Icon => "ui/ability_icons/arrow_cone_attack.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Special;
	public override float PlayerSpeedScale => 0.1f;
	public override float Duration => 0.5f;

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
