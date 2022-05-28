namespace Spire.UI;

[UseTemplate]
public partial class PlayerNameplate : BaseNameplate
{
	public override string NameplateName => Character.Client?.Name;
	protected bool IsLocalPlayer => Character == Local.Pawn;

	// @ref
	public Panel HealthBarFill { get; set; }

	public PlayerNameplate( BaseCharacter character ) : base( character )
	{
		BindClass( "local", () => IsLocalPlayer );
	}

	public override void Update()
	{
		base.Update();

		if ( Character is PlayerCharacter player )
		{
			HealthBarFill.Style.Width = Length.Fraction( HealthFraction );
		}
	}
}
