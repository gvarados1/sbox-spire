namespace Spire.UI;

[UseTemplate( "/UI/Nameplates/PlayerNameplate.html" )]
public partial class PlayerNameplate : BaseNameplate
{
	public override string NameplateName => Character.Client?.Name;

	public PlayerNameplate( BaseCharacter character ) : base( character )
	{
	}

	public override void Update()
	{
		base.Update();

		if ( Character is PlayerCharacter player )
		{
			//
		}
	}
}
