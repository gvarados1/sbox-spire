namespace Spire.UI;

[UseTemplate]
public partial class PlayerNameplate : BaseNameplate
{
	public override string NameplateName => Character.Client?.Name;

	protected bool IsLocalPlayer => Character == Local.Pawn;

	public PlayerNameplate( BaseCharacter character ) : base( character )
	{
		BindClass( "local", () => IsLocalPlayer );
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
