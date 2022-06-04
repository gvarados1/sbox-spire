using Spire.Buffs;

namespace Spire.Abilities;

public partial class SelfHealAbility : PlayerAbility
{
	// Configuration
	public override float Cooldown => 30f;
	public override string AbilityName => "Healing Salve";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/heal.png";

	public override void Execute()
	{
		base.Execute();

		if ( Host.IsClient ) return;

		Character.AddBuff<BaseHealingBuff>();
	}
}
