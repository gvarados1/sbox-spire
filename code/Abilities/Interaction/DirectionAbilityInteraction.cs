namespace Spire.Abilities;

public partial class DirectionAbilityInteraction : AbilityInteraction
{
	public override void OnTick()
	{
		base.OnTick();

		if ( Host.IsClient )
			TickWidget();

		if ( Input.Pressed( InputButton.PrimaryAttack ) )
		{
			End();
		}
		else if ( Input.Pressed( InputButton.SecondaryAttack ) )
		{
			Cancel();
		}
	}

	protected void TickWidget()
	{
		if ( Ability.TickWidget() )
			return;

		var character = Ability.GetCharacter();
		var pos = character.Position;
		var rot = character.EyeRotation;

		DebugOverlay.Line( pos, pos + rot.Forward * 10000f + Vector3.Up * 10f, Color.Green );
	}

	protected override void OnEnd()
	{
		base.OnEnd();

		Ability.Run();
	}
}
