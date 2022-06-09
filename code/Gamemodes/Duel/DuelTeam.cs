namespace Spire.Gamemodes.Duel;

public enum DuelTeam
{
	Red,
	Blue
}

public static partial class DuelTeamExtensions
{
	public static IEnumerable<Client> GetMembers( this DuelTeam team )
	{
		return Client.All.Where( x => x.GetTeam() == team );
	}

	public static string GetName( this DuelTeam team )
	{
		return team switch
		{
			DuelTeam.Red => "Red",
			DuelTeam.Blue => "Blue",
			_ => "N/A"
		};
	}
}
