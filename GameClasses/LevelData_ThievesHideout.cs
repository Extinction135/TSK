using System.Collections.Generic;

namespace DungeonRun
{
	public static class LevelData_ThievesHideout
	{
		//levelData specific to this island
		public static RoomXmlData ThievesDen_GateEntrance = new RoomXmlData();

		//data is added here
		static LevelData_ThievesHideout()
		{

			#region ThievesDen_GateEntrance

			ThievesDen_GateEntrance.type = RoomID.ThievesDen_GateEntrance;
			ThievesDen_GateEntrance.windDirection = Direction.None;
			ThievesDen_GateEntrance.windFrequency = 2;
			ThievesDen_GateEntrance.windIntensity = 0;
			#endregion

		}
	}
}