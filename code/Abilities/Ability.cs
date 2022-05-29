namespace Spire.Abilities;

public partial class Ability : BaseNetworkable
{
	// Configuration
	public virtual float Cooldown => 5f;
	public virtual string AbilityName => "Ability";
	public virtual string AbilityDescription => "This ability does nothing.";
	public virtual string AbilityIcon => "";
	public virtual string AbilityExecuteSound => "";

	/// <summary>
	/// The duration of an ability. If set to 0f, will run <see cref="Execute"/>, otherwise <see cref="AsyncExecute"/>
	/// </summary>
	public virtual float AbilityDuration => 0f;

	[Net]
	public TimeSince LastUsed { get; set; }

	[Net]
	public TimeUntil NextUse { get; set; }

	[Net]
	public bool InProgress { get; set; } = false;

	[Net]
	public Entity Entity { get; set; }

	/// <summary>
	/// Allows you to handle an ability asynchronously.
	/// </summary>
	/// <returns></returns>
	protected async virtual Task AsyncExecute()
	{
		InProgress = true;
		await GameTask.DelaySeconds( AbilityDuration );
		InProgress = false;
		NextUse = Cooldown;
		LastUsed = 0;
	}

	/// <summary>
	/// Called just before an ability is executed.
	/// </summary>
	protected virtual void PreAbilityExecute()
	{
		if ( !string.IsNullOrEmpty( AbilityExecuteSound ) )
			Entity.PlaySound( AbilityExecuteSound );
	}

	/// <summary>
	/// Called after the ability has finished. If specified, this will be executed in <paramref name="AbilityDuration"/> seconds.
	/// </summary>
	protected virtual void PostAbilityExecute()
	{
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
		LastUsed = 0;
		NextUse = Cooldown;
	}

	/// <summary>
	/// Will try to execute an ability
	/// </summary>
	internal void TryExecute()
	{
		if ( AbilityDuration > 0f )
		{
			PreAbilityExecute();
			_ = AsyncExecute();
			PostAbilityExecute();
		}
		else
		{
			PreAbilityExecute();
			Execute();
			PostAbilityExecute();
		}
	}
}
