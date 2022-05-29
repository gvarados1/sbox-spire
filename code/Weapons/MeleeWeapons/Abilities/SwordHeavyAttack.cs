namespace Spire.Abilities;

public partial class SwordHeavyAttack : BaseMeleeAttackAbility
{
	// Configuration
	public override float Cooldown => 10f;
	public override string AbilityName => "Heavy Strike";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "";
	public override WeaponAbilityType Type => WeaponAbilityType.Special;

	public override float BaseDamage => 60f;
	public override string HitFleshSoundPath => "heavy_sword_stab";

	public override void Execute()
	{
		base.Execute();

		Log.Info( "Sword Heavy Attack!" );
	}
}
