namespace Spire.Abilities;

public partial class SpeedWalkAbility : PlayerAbility
{
	public override float Cooldown => 5f;
	public override string AbilityName => "Speed Walk";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/speed_walk.png";
	public override PlayerAbilityType Type => PlayerAbilityType.Movement;
	public override float AbilityDuration => 5f;

	public override void Execute()
	{
		base.Execute();
	}

	protected override void PreAbilityExecute()
	{
		base.PreAbilityExecute();

		var controller = Character.ActiveController as CharacterController;
		if ( controller is null )
			return;

		Log.Info( "Speed multiplier assigned." );
		controller.SpeedMultiplier = 2f;
	}

	protected override void PostAbilityExecute()
	{
		base.PostAbilityExecute();

		var controller = Character.ActiveController as CharacterController;
		if ( controller is null )
			return;

		Log.Info( "Speed multiplier revoked." );
		controller.SpeedMultiplier = 1f;
	}
}
