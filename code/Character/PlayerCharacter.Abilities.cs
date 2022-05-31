using Spire.Abilities;

namespace Spire;

public partial class PlayerCharacter
{
	[Net]
	public PlayerAbility MovementAbility { get; set; }
	[Net]
	public PlayerAbility FirstAbility { get; set; }
	[Net]
	public PlayerAbility SecondAbility { get; set; }
	[Net]
	public PlayerAbility UltimateAbility { get; set; }

	public IEnumerable<PlayerAbility> GetPlayerAbilities()
	{
		yield return MovementAbility;
		yield return FirstAbility;
		yield return SecondAbility;
		yield return UltimateAbility;
	}

	public PlayerAbility GetAbilityFromSlot( int slot )
	{
		var abilities = GetPlayerAbilities().ToList();
		return abilities[slot];
	}

	public void SimulateAbilities( Client cl )
	{
		using ( LagCompensation() )
		{
			int i = 0;
			foreach ( var ability in GetPlayerAbilities() )
			{
				if ( Input.Down( PlayerAbility.GetInputButtonFromSlot( i ) ) )
				{
					if ( ability is not null && ability.CanExecute() )
					{
						ability.TryExecute();
					}
				}

				i++;
			}
		}
	}
}
