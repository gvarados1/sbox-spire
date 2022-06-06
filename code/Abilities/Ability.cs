namespace Spire.Abilities;

// @TODO: These really shouldn't be entities, they were BaseNetworkable
// but they suck on hotload
public abstract partial class Ability : Entity
{
	public Ability()
	{
		Transmit = TransmitType.Always;
	}

	// Main Configuration

	/// <summary>
	/// The ability's cooldown until you can run it again. This is assigned after Ability.PostRun
	/// </summary>
	public virtual float Cooldown => 5f;

	/// <summary>
	/// A friendly name for the ability
	/// </summary>
	public virtual new string Name => "Ability";

	/// <summary>
	/// A short description of an ability
	/// </summary>
	public virtual string Description => "This ability does nothing.";

	/// <summary>
	/// The ability's icon used in the game's user interface
	/// </summary>
	public virtual string Icon => "";

	/// <summary>
	/// Called on Ability.PreRun
	/// </summary>
	public virtual string PreAbilityParticle => "";

	/// <summary>
	/// Called on Ability.PreRun
	/// </summary>
	public virtual string PreAbilitySound => "";

	/// <summary>
	/// Called on Ability.PostRun
	/// </summary>
	public virtual string PostAbilitySound => "";

	/// <summary>
	/// Apply a speed modifier to the player while the ability is in progress
	/// </summary>
	public virtual float PlayerSpeedScale => 1f;

	/// <summary>
	/// The duration of an ability.
	/// </summary>
	public virtual float Duration => 0f;

	// Network Variables

	[Net, Predicted]
	public TimeSince TimeSinceLastUse { get; set; }

	[Net, Predicted]
	public TimeUntil TimeUntilFinish { get; set; }

	[Net, Predicted]
	public TimeUntil TimeUntilNextUse { get; set; }

	[Net, Predicted]
	public bool InProgress { get; set; }

	[Net, Predicted]
	public Entity Entity { get; set; }

	// Implementation

	public virtual BaseCharacter GetCharacter()
	{
		if ( Entity is BaseCharacter character )
			return character;
		else
			return Entity.Owner as BaseCharacter;
	}

	public virtual void DoPlayerAnimation()
	{
		GetCharacter()?.SetAnimParameter( "b_attack", true );
	}

	/// <summary>
	/// Called just before an ability is ran.
	/// </summary>
	protected virtual void PreRun()
	{
		if ( !string.IsNullOrEmpty( PreAbilitySound ) )
			Entity.PlaySound( PreAbilitySound );

		if ( !string.IsNullOrEmpty( PreAbilityParticle ) )
			Util.CreateParticle( Entity, PreAbilityParticle, true );

		var character = GetCharacter();
		if ( character.IsValid() )
			character.Controller.SpeedMultiplier = PlayerSpeedScale;
	}

	/// <summary>
	/// Called after the ability has finished. If specified, this will be executed in <paramref name="Duration"/> seconds.
	/// </summary>
	protected virtual void PostRun()
	{
		if ( !string.IsNullOrEmpty( PostAbilitySound ) )
			Entity.PlaySound( PostAbilitySound );

		var character = GetCharacter();
		if ( character.IsValid() )
			character.Controller.SpeedMultiplier = 1f;

		DoPlayerAnimation();
	}

	/// <summary>
	/// Returns whether or not an ability can be executed.
	/// </summary>
	/// <returns></returns>
	public virtual bool CanRun()
	{
		return TimeUntilNextUse <= 0 && !InProgress;
	}

	/// <summary>
	/// Start an ability
	/// </summary>
	internal void Run()
	{
		TimeUntilFinish = Duration;
		InProgress = true;
		TimeSinceLastUse = 0f;

		if ( Duration > 0f )
		{
			PreRun();
		}
		else
		{
			PreRun();
			PostRun();
		}
	}

	/// <summary>
	/// Called every tick (shared) while an ability is in progress
	/// </summary>
	public virtual void OnTick()
	{
	}

	public override void Simulate( Client cl )
	{
		if ( !InProgress ) 
			return;

		OnTick();

		if ( TimeUntilFinish )
		{
			InProgress = false;
			TimeUntilNextUse = Cooldown;

			PostRun();
		}
	}
}
