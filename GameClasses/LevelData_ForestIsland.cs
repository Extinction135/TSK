using System.Collections.Generic;

namespace DungeonRun
{
	public static class LevelData_ForestIsland
	{
		//levelData specific to this island
		public static RoomXmlData ForestIsland_MainEntrance = new RoomXmlData();

		//data is added here
		static LevelData_ForestIsland()
		{

			#region ForestIsland_MainEntrance

			ForestIsland_MainEntrance.type = RoomID.ForestIsland_MainEntrance;
			ForestIsland_MainEntrance.windDirection = Direction.None;
			ForestIsland_MainEntrance.windFrequency = 2;
			ForestIsland_MainEntrance.windIntensity = 0;
			#endregion

		}
	}
}