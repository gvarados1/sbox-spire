using Spire.Abilities;

namespace Spire;

public partial class PlayerCharacter
{
	[Net]
	public PlayerAbility FirstAbility { get; set; }
	[Net]
	public PlayerAbility SecondAbility { get; set; }
	[Net]
	public PlayerAbility UltimateAbility { get; set; }

	public IEnumerable<PlayerAbility> GetPlayerAbilities()
	{
		yield return FirstAbility;
		yield return SecondAbility;
		yield return UltimateAbility;
	}

	public InputButton GetInputButtonFromSlot( int slotIndex )
	{
		return slotIndex switch
		{
			0 => InputButton.Slot1,
			1 => InputButton.Slot2,
			2 => InputButton.Slot3,
			_ => InputButton.Slot1
		};
	}

	public void SimulateAbilities( Client cl )
	{
		using ( LagCompensation() )
		{
			int i = 0;
			foreach ( var ability in GetPlayerAbilities() )
			{
				if ( Input.Down( GetInputButtonFromSlot( i ) ) )
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
