using Spire.Abilities;

namespace Spire;

public partial class BaseWeapon : BaseCarriable
{
	[Net, Predicted]
	public TimeSince TimeSincePrimaryAttack { get; set; }

	public virtual List<Type> Abilities => new();

	[Net]
	public IList<WeaponAbility> _Abilities { get; protected set; }

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
						ability.TryExecute();
				}
			}
		}
	}
}
