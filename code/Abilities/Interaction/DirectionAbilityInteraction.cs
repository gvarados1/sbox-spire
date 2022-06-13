namespace Spire.Abilities;

public partial class DirectionAbilityInteraction : AbilityInteraction
{
	public override void OnTick()
	{
		base.OnTick();

		if ( Input.Pressed( InputButton.PrimaryAttack ) )
		{
			End();
		}
		else if ( Input.Pressed( InputButton.SecondaryAttack ) )
		{
			Cancel();
		}
	}

	protected override void TickGuide( AbilityGuideEntity entity )
	{
		//var character = Ability.GetCharacter();
		//var pos = character.Position;
		//var rot = character.EyeRotation;

		//DebugOverlay.Line( pos, pos + rot.Forward * 10000f + Vector3.Up * 10f, Color.Green );

		entity.SetParticle( "particles/widgets/widget_direction.vpcf" );
		entity.Position = Ability.GetCharacter().Position;
	}

	protected override void OnEnd()
	{
		base.OnEnd();

		Ability.Run();
	}
}
