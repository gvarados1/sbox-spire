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
			AbilityInteractionType.Direction => new DirectionAbilityInteraction(),
			_ => new GenericAbilityInteraction()
		};

		interaction.Ability = ability;

		return interaction;
	}

	[Net, Predicted]
	public Vector3 WorldCursorPosition { get; set; }

	public Vector3 GetWorldCursor()
	{
		var trace = Trace.Ray( Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 100000f )
			.WithoutTags( "player" )
			.Radius( 5f )
			.Run();

		return trace.HitPosition;
	}

	/// <summary>
	/// Called every tick on the client
	/// </summary>
	public virtual void OnTick()
	{
	}

	public void Start()
	{
		var character = Ability.GetCharacter();
		
		character.InteractingAbility = Ability;
		character.TimeSinceInteract = 0;

		OnStart();
	}

	protected virtual void OnStart()
	{

	}

	public void Cancel()
	{
		Ability.GetCharacter().InteractingAbility = null;
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
