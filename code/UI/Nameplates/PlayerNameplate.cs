namespace Spire.UI;

[UseTemplate]
public partial class PlayerNameplate : BaseNameplate
{
	public override string NameplateName => Character.Client?.Name;
	protected bool IsLocalPlayer => Character == Local.Pawn;

	// @ref
	public Panel HealthBarFill { get; set; }

	protected float LerpedHealthFraction { get; set; } = 1f;

	public PlayerNameplate( BaseCharacter character ) : base( character )
	{
		BindClass( "local", () => IsLocalPlayer );
	}

	public override void Update()
	{
		base.Update();

		LerpedHealthFraction = LerpedHealthFraction.LerpTo( HealthFraction, Time.Delta * 10f );
		HealthBarFill.Style.Width = Length.Fraction( LerpedHealthFraction );
	}
}
