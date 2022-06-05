namespace Spire.Abilities;

// @TODO: These really shouldn't be entities, they were BaseNetworkable
// but they suck on hotload
public partial class Ability : Entity
{
	public Ability()
	{
		Transmit = TransmitType.Always;
	}

	// Configuration
	public virtual float Cooldown => 5f;
	public virtual string AbilityName => "Ability";
	public virtual string AbilityDescription => "This ability does nothing.";
	public virtual string AbilityIcon => "";
	public virtual string AbilityExecuteSound => "";

	public virtual string ParticlePath => "";

	/// <summary>
	/// Apply a speed modifier to the player while the ability is in progress
	/// </summary>
	public virtual float PlayerSpeedMultiplier => 1f;

	/// <summary>
	/// The duration of an ability.
	/// </summary>
	public virtual float AbilityDuration => 0f;

	[Net, Predicted]
	public TimeSince LastUsed { get; set; }

	[Net, Predicted]
	public TimeUntil AbilityFinished { get; set; }

	[Net, Predicted]
	public TimeUntil NextUse { get; set; }

	public bool InProgress { get; set; }

	[Net, Predicted]
	public Entity Entity { get; set; }

	public virtual BaseCharacter GetCharacter()
	{
		if ( Entity is BaseCharacter character )
		{
			return character;
		}
		else
		{
			return Entity.Owner as BaseCharacter;
		}
	}

	public virtual void DoPlayerAnimation()
	{
		GetCharacter()?.SetAnimParameter( "b_attack", true );
	}

	/// <summary>
	/// Called just before an ability is executed.
	/// </summary>
	protected virtual void PreAbilityExecute()
	{
		if ( !string.IsNullOrEmpty( AbilityExecuteSound ) )
			Entity.PlaySound( AbilityExecuteSound );

		if ( !string.IsNullOrEmpty( ParticlePath ) )
			Util.CreateParticle( Entity, ParticlePath, true );

		var character = GetCharacter();
		if ( character.IsValid() )
		{
			character.Controller.SpeedMultiplier = PlayerSpeedMultiplier;
		}
	}

	/// <summary>
	/// Called after the ability has finished. If specified, this will be executed in <paramref name="AbilityDuration"/> seconds.
	/// </summary>
	protected virtual void PostAbilityExecute()
	{
		var character = GetCharacter();
		if ( character.IsValid() )
		{
			character.Controller.SpeedMultiplier = 1f;
		}

		DoPlayerAnimation();
	}

	/// <summary>
	/// Returns whether or not an ability can be executed.
	/// </summary>
	/// <returns></returns>
	public virtual bool CanExecute()
	{
		return NextUse <= 0 && !InProgress;
	}

	/// <summary>
	/// Synchronous version of an ability.
	/// </summary>
	public virtual void Execute()
	{
	}

	/// <summary>
	/// Will try to execute an ability
	/// </summary>
	internal void TryExecute()
	{
		AbilityFinished = AbilityDuration;
		InProgress = true;
		LastUsed = 0f;

		if ( AbilityDuration > 0f )
		{
			PreAbilityExecute();
		}
		else
		{
			PreAbilityExecute();
			Execute();
			PostAbilityExecute();
		}
	}

	public virtual void Tick()
	{
	}

	public override void Simulate( Client cl )
	{
		if ( !InProgress ) return;

		Tick();

		if ( AbilityFinished )
		{
			InProgress = false;
			NextUse = Cooldown;

			PostAbilityExecute();
		}
	}
}
