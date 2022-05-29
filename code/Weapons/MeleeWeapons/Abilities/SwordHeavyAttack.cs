namespace Spire.Abilities;

public partial class SwordHeavyAttack : WeaponAbility
{
	// Configuration
	public override float Cooldown => 10f;
	public override string AbilityName => "Heavy Strike";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "";
	public override WeaponAbilityType Type => WeaponAbilityType.Special;

	public override void Execute( BaseWeapon weapon )
	{
		base.Execute( weapon );

		Log.Info( "Sword Heavy Attack!" );
	}
}
