namespace Spire.Abilities;

public partial class WorldPointAbilityInteraction : AbilityInteraction
{
	public override void OnTick()
	{
		base.OnTick();

		GetWorldCursor();

		if ( Input.Pressed( InputButton.PrimaryAttack ) )
		{
			End();
		}
		else if ( Input.Pressed( InputButton.SecondaryAttack ) )
		{
			Cancel();
		}
	}

	protected override void OnEnd()
	{
		base.OnEnd();

		if ( Host.IsClient )
			WorldPointEnd( Ability.NetworkIdent, WorldCursorPosition );

		Ability.Run();
	}

	// @TODO: ServerRpc my beloved
	[ConCmd.Server( "spire_ability_worldpoint" )]
	public static void WorldPointEnd( int abilityNetIdent, Vector3 worldPos )
	{
		var ability = Entity.FindByIndex( abilityNetIdent ) as Ability;
		if ( !ability.IsValid() )
			return;

		ability.Interaction.End();
	}
}
