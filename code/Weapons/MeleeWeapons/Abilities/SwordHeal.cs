namespace Spire.Abilities;

public partial class SwordHeal : WeaponAbility
{
	// Configuration
	public override float Cooldown => 30f;
	public override string AbilityName => "Healing Salve";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/heal.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Ultimate;

	public override void Execute()
	{
		base.Execute();

		var player = Weapon.Owner as BaseCharacter;
		player.Health = player.MaxHealth;

		Particles.Create( "particles/example/pull_towards_example/pull_towards_example.vpcf", player, "eyes", true );
	}
}
