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
		var current = Character.ActiveChild as BaseCarriable;
		current?.ActiveEnd( Character, false );

		Character.ActiveChild = carriable;
		carriable.OnCarryStart( Character );
		carriable.ActiveStart( Character );
	}
}
