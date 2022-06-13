namespace Spire.Abilities;

public abstract partial class AbilityInteraction : BaseNetworkable
{
	public AbilityInteraction()
	{
	}

	public AbilityInteraction( Ability ability )
	{
		Ability = ability;
	}

	[Net]
	public Ability Ability { get; set; }

	public static AbilityInteraction From( Ability ability )
	{
		var type = ability.Data.InteractionType;

		AbilityInteraction interaction = type switch
		{
			AbilityInteractionType.Generic => new GenericAbilityInteraction(),
			AbilityInteractionType.WorldPoint => new WorldPointAbilityInteraction(),
			_ => new GenericAbilityInteraction()
		};

		interaction.Ability = ability;

		return interaction;
	}

	[Net, Predicted]
	public Vector3 WorldCursorPosition { get; set; }

	public void GetWorldCursor()
	{
		var trace = Trace.Ray( Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 100000f )
			.WithoutTags( "player" )
			.Radius( 5f )
			.Run();

		DebugOverlay.Sphere( trace.HitPosition, 8f, Color.Green );

		WorldCursorPosition = trace.HitPosition;
	}

	/// <summary>
	/// Called every tick on the client
	/// </summary>
	public virtual void OnTick()
	{
	}

	public void Start()
	{
		Ability.GetCharacter().InteractingAbility = Ability;

		OnStart();
	}

	protected virtual void OnStart()
	{

	}

	public void Cancel()
	{

	}

	public void End()
	{
		Ability.GetCharacter().InteractingAbility = null;

		OnEnd();
	}

	protected virtual void OnEnd()
	{

	}
}
