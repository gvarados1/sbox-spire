using Spire.Abilities;

namespace Spire;

public partial class BaseWeapon : BaseCarriable
{
	[Net, Predicted]
	public TimeSince TimeSinceLastAbility { get; set; } = 0f;

	public virtual List<Type> Abilities => new();

	[Net]
	public IList<WeaponAbility> _Abilities { get; protected set; }

	public virtual float TimeUntilRelaxed { get; set; } = 5f;
	public virtual HoldType RelaxedHoldType { get; set; } = HoldType.None;

	public override void Spawn()
	{
		base.Spawn();

		foreach ( var ability in Abilities )
		{
			var instance = TypeLibrary.Create<WeaponAbility>( ability );
			instance.Weapon = this;

			_Abilities.Add( instance );
		}
	}

	public override void Simulate( Client player )
	{
		if ( !Owner.IsValid() )
			return;

		using ( LagCompensation() )
		{
			foreach ( var ability in _Abilities )
			{
				if ( Input.Down( ability.Type.GetButton() ) )
				{
					if ( ability.CanExecute() )
					{
						ability.TryExecute();
						TimeSinceLastAbility = 0;
					}
				}
			}
		}
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		base.SimulateAnimator( anim );

		if ( TimeSinceLastAbility > TimeUntilRelaxed )
		{
			anim.SetAnimParameter( "holdtype", (int)RelaxedHoldType );
		}
	}
}
