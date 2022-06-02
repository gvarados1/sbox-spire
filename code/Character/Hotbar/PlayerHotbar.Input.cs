
namespace Spire;

public partial class PlayerHotbar
{
	public void BuildInput( InputBuilder input )
	{
		if ( input.Pressed( InputButton.Slot1 ) ) SetActiveSlot( 0 );
		if ( input.Pressed( InputButton.Slot2 ) ) SetActiveSlot( 1 );
		if ( input.Pressed( InputButton.Slot3 ) ) SetActiveSlot( 2 );
		if ( input.Pressed( InputButton.Slot4 ) ) SetActiveSlot( 3 );
		if ( input.Pressed( InputButton.Slot5 ) ) SetActiveSlot( 4 );
		if ( input.Pressed( InputButton.Slot6 ) ) SetActiveSlot( 5 );
		if ( input.Pressed( InputButton.Slot7 ) ) SetActiveSlot( 6 );
		if ( input.Pressed( InputButton.Slot8 ) ) SetActiveSlot( 7 );
		if ( input.Pressed( InputButton.Slot9 ) ) SetActiveSlot( 8 );
	}
}
