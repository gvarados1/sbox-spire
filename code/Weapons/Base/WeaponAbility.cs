namespace Spire.Abilities;

public partial class WeaponAbility : BaseNetworkable
{
	// Configuration
	public virtual float Cooldown => 5f;
	public virtual string AbilityName => "Weapon Ability";
	public virtual string AbilityDescription => "This ability does nothing.";
	public virtual string AbilityIcon => "";

	public virtual WeaponAbilityType Type => WeaponAbilityType.Attack;

	[Net]
	public TimeSince LastUsed { get; protected set; }

	[Net]
	public TimeUntil NextUse { get; protected set; }

	[Net]
	public bool InProgress { get; protected set; } = false;

	/// <summary>
	/// The duration of an ability. If set to 0f, will run <see cref="Execute"/>, otherwise <see cref="AsyncExecute"/>
	/// </summary>
	public virtual float AbilityDuration => 0f;

	/// <summary>
	/// Allows you to handle an ability asynchronously.
	/// </summary>
	/// <param name="weapon"></param>
	/// <returns></returns>
	protected async virtual Task AsyncExecute( BaseWeapon weapon )
	{
		InProgress = true;
		await GameTask.DelaySeconds( AbilityDuration );
		InProgress = false;
		NextUse = Cooldown;
	}

	/// <summary>
	/// Called just before an ability is executed.
	/// </summary>
	/// <param name="weapon"></param>
	protected virtual void PreAbilityExecute( BaseWeapon weapon )
	{

	}

	/// <summary>
	/// Called after the ability has finished. If specified, this will be executed in <paramref name="AbilityDuration"/> seconds.
	/// </summary>
	/// <param name="weapon"></param>
	protected virtual void PostAbilityExecute( BaseWeapon weapon )
	{

	}

	/// <summary>
	/// Returns whether or not an ability can be executed.
	/// </summary>
	/// <param name="weapon"></param>
	/// <returns></returns>
	public virtual bool CanExecute( BaseWeapon weapon )
	{
		return NextUse <= 0 && !InProgress;
	}

	/// <summary>
	/// Synchronous version of an ability.
	/// </summary>
	/// <param name="weapon"></param>
	public virtual void Execute( BaseWeapon weapon )
	{
		NextUse = Cooldown;
	}

	/// <summary>
	/// Will try to execute an ability
	/// </summary>
	/// <param name="weapon"></param>
	internal void TryExecute( BaseWeapon weapon )
	{
		if ( AbilityDuration > 0f )
		{
			PreAbilityExecute( weapon );
			_ = AsyncExecute( weapon );
			PostAbilityExecute( weapon );
		}
		else
		{
			PreAbilityExecute( weapon );
			Execute( weapon );
			PostAbilityExecute( weapon );
		}
	}
}
