using System.Collections.Generic;

namespace DungeonRun
{
	public static class LevelData_LavaIsland
	{
		//levelData specific to this island
		public static RoomXmlData LavaIsland_MainEntrance = new RoomXmlData();

		//data is added here
		static LevelData_LavaIsland()
		{

			#region LavaIsland_MainEntrance

			LavaIsland_MainEntrance.type = RoomID.LavaIsland_MainEntrance;
			LavaIsland_MainEntrance.windDirection = Direction.None;
			LavaIsland_MainEntrance.windFrequency = 2;
			LavaIsland_MainEntrance.windIntensity = 0;
			#endregion

		}
	}
}