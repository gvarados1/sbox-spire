namespace Spire.Abilities;

public partial class WorldPointAbilityInteraction : AbilityInteraction
{
	public override void OnTick()
	{
		base.OnTick();

		WorldCursorPosition = GetWorldCursor();

		ShowWorldCursorWidget();

		if ( Input.Pressed( InputButton.PrimaryAttack ) )
		{
			TryEnd();
		}
		else if ( Input.Pressed( InputButton.SecondaryAttack ) )
		{
			Cancel();
		}
	}

	protected bool IsInRange()
	{
		var range = Ability.Data.AbilityRange;
		var entity = Ability.Entity;
		var dist = entity.Position.Distance( WorldCursorPosition );

		return dist <= range.Max && dist >= range.Min;
	}

	protected void TryEnd()
	{
		if ( !IsInRange() )
		{
			Cancel();
			return;
		}

		End();
	}

	protected void ShowWorldCursorWidget()
	{
		var color = IsInRange() ? Color.Green : Color.Red;

		DebugOverlay.Sphere( WorldCursorPosition, 16f, color );
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
