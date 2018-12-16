using System.Collections.Generic;

namespace DungeonRun
{
	public static class RoomData_SkullIsland_ExitBossHub
	{
		//data is added to this list
		public static List<RoomXmlData> Data = new List<RoomXmlData>();
		//data uses this reference
		public static RoomXmlData dataRef;
		//data is added in the constructor below
		static RoomData_SkullIsland_ExitBossHub()
		{

			#region ForestIsland_BossRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.ForestIsland_BossRoom;
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


			#region ForestIsland_ExitRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.ForestIsland_ExitRoom;
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


			#region ForestIsland_HubRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.ForestIsland_HubRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Tree_Med_Stump; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 232; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Tree_Med_Stump; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 40; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Tree_Med_Stump; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 232; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Tree_Med_Stump; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 40; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_SkullPillar; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 56; dataRef.inds.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Tree_Stump; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 56; dataRef.ints.Add(obj);}
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
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion

		}
	}
}