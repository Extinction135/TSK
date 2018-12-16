using System.Collections.Generic;

namespace DungeonRun
{
	public static class RoomData_ThievesHideout_ExitBossHub
	{
		//data is added to this list
		public static List<RoomXmlData> Data = new List<RoomXmlData>();
		//data uses this reference
		public static RoomXmlData dataRef;
		//data is added in the constructor below
		static RoomData_ThievesHideout_ExitBossHub()
		{

			#region ThievesHideout_BossRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.ThievesHideout_BossRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Tree_Med; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Tree_Med_Stump; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 120; dataRef.inds.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 296; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 296; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Tall; obj.direction = Direction.Down; obj.posX = 296; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 296; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Flowers; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Flowers; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush_Stump; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush_Stump; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 88; dataRef.ints.Add(obj);}
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion


			#region ThievesHideout_ExitRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.ThievesHideout_ExitRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_ExitPillarLeft; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_ExitPillarRight; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_ExitLight; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 160; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_Exit; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 152; dataRef.inds.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Signpost; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Enemy_Rat; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Enemy_Rat; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 8; dataRef.ints.Add(obj);}
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion


			#region ThievesHideout_HubRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.ThievesHideout_HubRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Tree_Med_Stump; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 232; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Tree_Med_Stump; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 40; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Tree_Med_Stump; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 232; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Tree_Med_Stump; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 40; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_SkullPillar; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 56; dataRef.inds.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 6; obj.posY = 25; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 36; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 9; obj.posY = 294; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 20; obj.posY = 7; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 26; obj.posY = 118; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 27; obj.posY = 155; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 26; obj.posY = 199; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 22; obj.posY = 244; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 37; obj.posY = 84; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 53; obj.posY = 6; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 69; obj.posY = 105; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 73; obj.posY = 155; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 71; obj.posY = 202; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 69; obj.posY = 231; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 86; obj.posY = 116; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 87; obj.posY = 170; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 90; obj.posY = 183; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 102; obj.posY = 106; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 106; obj.posY = 169; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 106; obj.posY = 187; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 102; obj.posY = 277; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 116; obj.posY = 21; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 122; obj.posY = 85; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 169; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 118; obj.posY = 203; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 117; obj.posY = 219; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 135; obj.posY = 26; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 137; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 153; obj.posY = 5; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 22; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 148; obj.posY = 117; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 171; obj.posY = 100; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 183; obj.posY = 20; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 182; obj.posY = 75; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 164; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 186; obj.posY = 297; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 196; obj.posY = 36; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 203; obj.posY = 89; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 197; obj.posY = 186; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 196; obj.posY = 263; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 199; obj.posY = 299; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 219; obj.posY = 20; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 212; obj.posY = 101; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 215; obj.posY = 186; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 229; obj.posY = 68; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 234; obj.posY = 84; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 228; obj.posY = 101; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 164; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 233; obj.posY = 181; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 234; obj.posY = 282; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 247; obj.posY = 68; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 246; obj.posY = 213; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 262; obj.posY = 165; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 266; obj.posY = 298; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 276; obj.posY = 9; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 283; obj.posY = 85; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 282; obj.posY = 169; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 298; obj.posY = 116; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerRight; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalRight; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalRight; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerLeft; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerRight; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalLeft; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalLeft; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalLeft; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerLeft; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerLeft; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerRight; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalRight; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalRight; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerRight; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Statue; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Statue; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_HammerPost_Up; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_HammerPost_Up; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_HammerPost_Up; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_HammerPost_Up; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_HammerPost_Up; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_HammerPost_Up; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_HammerPost_Up; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_HammerPost_Up; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Grass_Cut; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 5; obj.posY = 20; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 5; obj.posY = 90; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 20; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 20; obj.posY = 90; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 23; obj.posY = 123; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 199; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 43; obj.posY = 294; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 57; obj.posY = 23; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 181; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 53; obj.posY = 213; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 70; obj.posY = 43; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 69; obj.posY = 186; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 74; obj.posY = 235; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 84; obj.posY = 106; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 87; obj.posY = 276; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 69; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 103; obj.posY = 91; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 105; obj.posY = 117; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 105; obj.posY = 299; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 116; obj.posY = 59; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 116; obj.posY = 149; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 133; obj.posY = 89; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 132; obj.posY = 230; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 137; obj.posY = 295; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 9; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 155; obj.posY = 139; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 151; obj.posY = 167; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 153; obj.posY = 187; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 164; obj.posY = 90; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 230; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 186; obj.posY = 91; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 185; obj.posY = 203; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 201; obj.posY = 100; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 197; obj.posY = 134; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 149; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 202; obj.posY = 167; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 196; obj.posY = 278; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 215; obj.posY = 87; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 215; obj.posY = 134; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 218; obj.posY = 148; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 233; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 235; obj.posY = 198; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 244; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 261; obj.posY = 295; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 281; obj.posY = 90; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 283; obj.posY = 103; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 282; obj.posY = 122; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 278; obj.posY = 138; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 295; obj.posY = 103; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 298; obj.posY = 133; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 299; obj.posY = 218; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 298; obj.posY = 232; dataRef.ints.Add(obj);}
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion

		}
	}
}