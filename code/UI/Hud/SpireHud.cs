namespace Spire.UI;

[UseTemplate]
public partial class SpireHud : RootPanel
{
	[Event.BuildInput]
	protected void BuildInput( InputBuilder input )
	{
		// @TODO: handle this logic elsewhere 
		SetClass( "camera-movement", input.Down( InputButton.SecondaryAttack ) );
	}
}
