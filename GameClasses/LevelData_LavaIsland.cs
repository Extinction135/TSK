using System.Collections.Generic;

namespace DungeonRun
{
	public static class LevelData_LavaIsland
	{
		//levels specific to this island
		public static RoomXmlData LavaIsland_MainEntrance = new RoomXmlData();

		//level data
		static LevelData_LavaIsland()
		{

			#region LavaIsland_MainEntrance

			LavaIsland_MainEntrance.type = RoomID.LavaIsland_MainEntrance;
			#endregion

		}
	}
}