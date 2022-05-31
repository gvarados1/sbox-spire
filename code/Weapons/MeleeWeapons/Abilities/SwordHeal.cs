using Spire.Buffs;

namespace Spire.Abilities;

public partial class SwordHeal : WeaponAbility
{
	// Configuration
	public override float Cooldown => 5f;
	public override string AbilityName => "Healing Salve";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/heal.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Ultimate;

	protected void CreateParticles( BaseCharacter character )
	{
		character.CreateParticle( "particles/abilities/basic_heal.vpcf", true, "eyes" );
	}

	public override void Execute()
	{
		base.Execute();

		if ( Host.IsClient ) return;

		var player = Weapon.Owner as BaseCharacter;
		player.AddBuff<BaseHealingBuff>()
			.WithTickAction( CreateParticles )
			.WithDestroyAction( ( BaseCharacter character ) => Log.State( "Destroyed buff" ) );
	}
}
