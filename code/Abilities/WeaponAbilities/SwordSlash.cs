namespace Spire.Abilities;

public partial class SwordAttack : BaseMeleeAttackAbility
{
	// Configuration
	public override string Identifier => "sword_attack";
	public override WeaponAbilityType Type => WeaponAbilityType.Attack;
	public override string PreAbilityParticle => "particles/weapons/basic_sword/basic_swipe/basic_swipe.vpcf";
	public override bool OriginateParticlesFromCharacter => true;

}
