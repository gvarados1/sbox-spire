using Spire.Buffs;

namespace Spire.Abilities;

public partial class SelfHealAbility : PlayerAbility
{
	// Configuration
	public override float Cooldown => 30f;
	public override string Name => "Healing Salve";
	public override string Description => "";
	public override string Icon => "ui/ability_icons/heal.png";

	protected override void PreRun()
	{
		base.PreRun();

		Character.AddBuff<BaseHealingBuff>();
	}
}
