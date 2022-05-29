namespace Spire.Abilities;

public partial class InstantDamageAbility : PlayerAbility
{
	public override float Cooldown => 5f;
	public override string AbilityName => "Instant Damage";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/sword_slash.png";
	public override PlayerAbilityType Type => PlayerAbilityType.Standard;

	public override void Execute()
	{
		base.Execute();

		Log.Info( "Instant Damage!" );

		Character.TakeDamage( DamageInfo.Generic( 30f ).WithPosition( Character.Position ) );
	}
}
