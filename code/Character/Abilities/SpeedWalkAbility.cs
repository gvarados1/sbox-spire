namespace Spire.Abilities;

public partial class SpeedWalkAbility : PlayerAbility
{
	public override float Cooldown => 10f;
	public override string AbilityName => "Speed Walk";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/speed_walk.png";
	public override PlayerAbilityType Type => PlayerAbilityType.Movement;
	public override float AbilityDuration => 1f;

	protected override void PreAbilityExecute()
	{
		base.PreAbilityExecute();

		var controller = Character.ActiveController as CharacterController;
		if ( controller is null )
			return;

		controller.SpeedMultiplier = 2f;
	}

	protected override void PostAbilityExecute()
	{
		base.PostAbilityExecute();

		var controller = Character.ActiveController as CharacterController;
		if ( controller is null )
			return;

		controller.SpeedMultiplier = 1f;
	}
}
