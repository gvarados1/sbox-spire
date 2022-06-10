namespace Spire.Abilities;

public partial class SpeedWalkAbility : PlayerAbility
{
	public override float Cooldown => 10f;
	public override string Name => "Speed Walk";
	public override string Description => "";
	public override string Icon => "ui/ability_icons/speed_walk.png";
	public override PlayerAbilityType Type => PlayerAbilityType.Movement;
	public override float Duration => 1f;

	protected override void PreRun()
	{
		base.PreRun();

		var controller = Character.ActiveController as CharacterController;
		if ( controller is null )
			return;

		controller.SpeedMultiplier = 2f;
	}

	protected override void PostRun()
	{
		base.PostRun();

		var controller = Character.ActiveController as CharacterController;
		if ( controller is null )
			return;

		controller.SpeedMultiplier = 1f;
	}
}
