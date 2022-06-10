namespace Spire.Gamemodes.Duel;

[UseTemplate]
public partial class DuelHudPanel : Panel
{
	public DuelGamemode Gamemode => BaseGamemode.Current as DuelGamemode;

	// @ref
	public Panel BlueTeamMembers { get; set; }
	// @ref
	public Panel RedTeamMembers { get; set; }

	public string GameState => Gamemode?.GetGameStateText();

	public string RedScore => $"{Gamemode?.GetTeamScore( DuelTeam.Red )}";
	public string BlueScore => $"{Gamemode?.GetTeamScore( DuelTeam.Blue )}";

	public override void Tick()
	{
		base.Tick();

		UpdateAvatars();
	}

	public Panel GetTeamPanel( DuelTeam team )
	{
		if ( team == DuelTeam.Blue )
			return BlueTeamMembers;
		else
			return RedTeamMembers;
	}

	TimeSince LastUpdatedAvatars = 1f;
	float AvatarUpdateRate = 1f;
	public void UpdateAvatars()
	{
		if ( LastUpdatedAvatars < AvatarUpdateRate )
			return;

		BlueTeamMembers.DeleteChildren( true );
		RedTeamMembers.DeleteChildren( true );

		LastUpdatedAvatars = 0;

		foreach ( var client in Client.All )
		{
			GetTeamPanel( client.GetTeam() )
				.AddChild<Image>( "avatar" )
				.SetTexture( $"avatar:{client.PlayerId}" );

		}
	}
}
