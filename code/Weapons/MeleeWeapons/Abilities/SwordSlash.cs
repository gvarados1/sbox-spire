namespace Spire.Abilities;

public partial class SwordSlash : BaseMeleeAttackAbility
{
	// Configuration
	public override float Cooldown => 1f;
	public override string AbilityName => "Slash";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "";
	public override WeaponAbilityType Type => WeaponAbilityType.Attack;

	public override void Execute()
	{
		base.Execute();

		Log.Info( "Sword Slash!" );
	}
}
