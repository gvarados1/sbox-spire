namespace Spire.Gamemodes.Duel;

[UseTemplate]
public partial class DuelHudPanel : Panel
{
	public DuelGamemode Gamemode => BaseGamemode.Current as DuelGamemode;

	public string GameState => Gamemode?.CurrentState.GetName();

	// @ref
	public Panel BlueScorePanel { get; set; }
	// @ref
	public Panel RedScorePanel { get; set; }

	public string RedScore => $"{Gamemode?.GetTeamScore( DuelTeam.Red )}";
	public string BlueScore => $"{Gamemode?.GetTeamScore( DuelTeam.Blue )}";

	public override void Tick()
	{
		base.Tick();

		if ( RedScorePanel is null || BlueScorePanel is null )
			return;

		RedScorePanel.Style.BackgroundColor = DuelTeam.Red.GetColor();
		BlueScorePanel.Style.BackgroundColor = DuelTeam.Blue.GetColor();
	}
}
