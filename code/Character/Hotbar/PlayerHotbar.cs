namespace Spire;

public partial class PlayerHotbar : BaseNetworkable
{
	public PlayerHotbar() { }

	public PlayerHotbar( BaseCharacter character )
	{
		Character = character;
	}

	[Net]
	public BaseCharacter Character { get; set; }

	public void SetCurrent( BaseCarriable carriable )
	{
		Character.ActiveChild = carriable;
	}
}
