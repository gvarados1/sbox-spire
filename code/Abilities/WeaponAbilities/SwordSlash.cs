namespace Spire.Abilities;

public partial class SwordSlash : BaseMeleeAttackAbility
{
	// Configuration
	public override float Cooldown => 1f;
	public override string Name => "Slash";
	public override string Description => "";
	public override string Icon => "ui/ability_icons/sword_slash.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Attack;
	public override string PreAbilityParticle => "particles/weapons/basic_sword/basic_swipe/basic_swipe.vpcf";
	public override bool OriginateParticlesFromCharacter => true;

}
