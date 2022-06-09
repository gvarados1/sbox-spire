namespace Spire.Gamemodes.Duel;

[UseTemplate]
public partial class DuelHudPanel : Panel
{
	public DuelGamemode Gamemode => BaseGamemode.Current as DuelGamemode;

	public string GameState => Gamemode?.CurrentState.GetName();

	public string RedScore => $"{Gamemode?.GetTeamScore( DuelTeam.Red )}";
	public string BlueScore => $"{Gamemode?.GetTeamScore( DuelTeam.Blue )}";
}
