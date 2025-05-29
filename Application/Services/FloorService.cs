using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.DTOs.Update;
using Constructor_API.Models.Entities;
using Constructor_API.Models.Objects;
using MongoDB.Bson;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

namespace Constructor_API.Application.Services
{
    public class FloorService
    {
        IFloorRepository _floorRepository;
        IGraphPointRepository _graphPointRepository;
        IFloorsTransitionRepository _floorsTransitionRepository;
        IBuildingRepository _buildingRepository;
        IProjectRepository _projectRepository;
        IPredefinedGraphPointTypeRepository _predefinedGraphPointTypeRepository;
        IMapper _mapper;
        ImageService _imageService;

        public FloorService(IFloorRepository floorRepository,
                            IGraphPointRepository graphPointRepository,
                            IFloorsTransitionRepository floorConnectionRepository,
                            IBuildingRepository buildingRepository,
                            IMapper mapper,
                            IProjectRepository projectRepository,
                            IPredefinedGraphPointTypeRepository predefinedGraphPointTypeRepository,
                            ImageService imageService)
        {
            _floorRepository = floorRepository;
            _graphPointRepository = graphPointRepository;
            _floorsTransitionRepository = floorConnectionRepository;
            _buildingRepository = buildingRepository;
            _mapper = mapper;
            _projectRepository = projectRepository;
            _predefinedGraphPointTypeRepository = predefinedGraphPointTypeRepository;
            _imageService = imageService;
        }

        public async Task<Tuple<Dictionary<string, string[]>, Room[]>> ValidateRooms(
            Room[] rooms, 
            GraphPointType[]? customGraphPointTypes,
            CreateGraphPointFromFloorDto[]? graphPointDtos,
            Dictionary<string, string[]> graphIdsDict, 
            CancellationToken cancellationToken)
        {
            if (graphPointDtos == null) throw new ValidationException("Graph points were not transferred");

            DateTime now = DateTime.UtcNow;
            var corridorPreTypes = (await _predefinedGraphPointTypeRepository.ListAsync(t =>
                t.Category == "Corridor", cancellationToken)).Select(x => x.Name);
            var roomPreTypes = (await _predefinedGraphPointTypeRepository.ListAsync(t =>
                t.Category == "Room", cancellationToken)).Select(x => x.Name);
            var passagePreTypes = (await _predefinedGraphPointTypeRepository.ListAsync(t =>
                t.Category == "Passage", cancellationToken)).Select(x => x.Name);
            CreateGraphPointFromFloorDto? roomPoint;

            if (customGraphPointTypes != null)
            {
                if (corridorPreTypes == null)
                    corridorPreTypes = customGraphPointTypes.Where(x => x.Category == "Corridor")
                        .Select(x => x.Name);
                else
                    corridorPreTypes = corridorPreTypes.Concat(customGraphPointTypes
                        .Where(x => x.Category == "Corridor").Select(x => x.Name));

                if (roomPreTypes == null)
                    roomPreTypes = customGraphPointTypes.Where(x => x.Category == "Room")
                        .Select(x => x.Name);
                else
                    roomPreTypes = roomPreTypes.Concat(customGraphPointTypes
                        .Where(x => x.Category == "Room").Select(x => x.Name));

                if (passagePreTypes == null)
                    passagePreTypes = customGraphPointTypes.Where(x => x.Category == "Passage")
                        .Select(x => x.Name);
                else
                    passagePreTypes = passagePreTypes.Concat(customGraphPointTypes
                        .Where(x => x.Category == "Passage").Select(x => x.Name));
            }

            foreach (var room in rooms)
            {
                roomPoint = graphPointDtos.FirstOrDefault(g => g.Id == room.Id);
                if (roomPoint is null)
                    throw new NotFoundException($"There is a room {room.Id} without graph point");
                else if (roomPoint.Types != null ? corridorPreTypes.Intersect(roomPoint.Types).Count() != 0 : false)
                    throw new ValidationException($"Graph point {room.Id} has type of category \"Corridor\" but it is room");
                else if (roomPoint.Types != null ? passagePreTypes.Intersect(roomPoint.Types).Count() != 0 : false)
                    throw new ValidationException($"Graph point {room.Id} has type of category \"Passage\" but it is room");
                else if (roomPoint.Types != null ? roomPreTypes.Intersect(roomPoint.Types).Count() == 0 : true)
                    throw new ValidationException($"Graph point {room.Id} has not type of category \"Room\"");
                else
                {
                    if (!graphIdsDict.ContainsKey(room.Id))
                        graphIdsDict.Add(room.Id, roomPoint.Links ?? []);

                    if (room.Passages != null && roomPoint.Links != null)
                    {
                        foreach (var passage in room.Passages)
                        {
                            var gpPassage = graphPointDtos.FirstOrDefault(g => g.Id == passage.Id);
                            if (gpPassage == null)
                                throw new NotFoundException($"There is a passage {passage.Id} without graph point");
                            else if (gpPassage.Types != null ? passagePreTypes.Intersect(gpPassage.Types).Count() == 0 : true)
                                throw new ValidationException($"Graph point {passage.Id} has not type of category \"Passage\"");
                            else if (gpPassage.Types != null ? corridorPreTypes.Intersect(roomPoint.Types).Count() != 0 : false)
                                throw new ValidationException($"Graph point {passage.Id} has type of category \"Corridor\" but it is passage");
                            else if (gpPassage.Types != null ? roomPreTypes.Intersect(roomPoint.Types).Count() != 0 : false)
                                throw new ValidationException($"Graph point {passage.Id} has type of category \"Room\" but it is passage");
                            else if (gpPassage.Links == null || !roomPoint.Links.Contains(gpPassage.Id))
                                throw new ValidationException($"There is no mutual connection between graph points {room.Id} and {gpPassage.Id}");

                            if (!graphIdsDict.ContainsKey(gpPassage.Id))
                                graphIdsDict.Add(gpPassage.Id, gpPassage.Links ?? []);
                        }

                        //if (roomPoint.Links != null ? room.Passages.Length != roomPoint.Links.Length : true)
                        //    throw new ValidationException("Room point has too many or too few links");
                    }
                }
                //room.CreatedAt = now;
                //room.UpdatedAt = now;
            }

            return new Tuple<Dictionary<string, string[]>, Room[]>(graphIdsDict, rooms);
        }

        public void ValidateTypes(GraphPoint graphPoint, IReadOnlyList<PredefinedGraphPointType> predefinedTypes,
            GraphPointType[]? customGraphPointTypes)
        {
            foreach (string type in graphPoint.Types)
            {
                //Предопределенные
                if (!predefinedTypes.Any(t => t.Name == type))
                {
                    //Заданные пользователем
                    if (customGraphPointTypes == null || !customGraphPointTypes.Any(t => t.Name == type))
                    {
                        throw new NotFoundException($"Graph point type \"{type}\" is not found");
                    }
                }
            }
        }

        public Dictionary<string, string[]> ValidateLinks(GraphPoint graphPoint,
            Dictionary<string, string[]> graphIdsDict,
            CreateGraphPointFromFloorDto[] graphPointDtos)
        {
            if (!graphIdsDict.ContainsKey(graphPoint.Id))
                graphIdsDict.Add(graphPoint.Id, graphPoint.Links);

            foreach (var link in graphIdsDict[graphPoint.Id])
            {
                var g = graphPointDtos.FirstOrDefault(x => x.Id == link)
                    ?? throw new NotFoundException($"Graph point {link} is not found");

                if (g.Id != null && !graphIdsDict.ContainsKey(g.Id))
                    graphIdsDict.Add(g.Id, g.Links ?? []);

                if (!graphIdsDict[g.Id].Any(x => x == graphPoint.Id))
                    throw new ValidationException($"There is no mutual connection between graph points {graphPoint.Id} and {link}");
            }
            return graphIdsDict;
        }

        public async Task<Floor> InsertFloor(CreateFloorDto floorDto, CancellationToken cancellationToken)
        {
            //Проверка на повтор этажа
                if (await _floorRepository.CountAsync(f =>
                    f.BuildingId == floorDto.BuildingId && f.Index == floorDto.Index, cancellationToken) != 0)
                throw new AlreadyExistsException("Floor already exists");
            //Дата и время
            DateTime now = DateTime.UtcNow;
            //Проверка на наличие здания
            var building = await _buildingRepository.FirstOrDefaultAsync(b =>
               b.Id == floorDto.BuildingId, cancellationToken) ?? throw new NotFoundException("Building is not found");
            //Маппинг этажа, заполнение полей
            Floor floor = _mapper.Map<Floor>(floorDto);
            floor.Id = ObjectId.GenerateNewId().ToString();
            floor.Rooms = [];
            floor.GraphPoints = [];
            floor.Decorations = [];
            floor.Width = 0;
            floor.Height = 0;
            //floor.ImageIds = [];
            //Обновление здания, добавление в него этажа
            if (building.FloorIds == null)
                building.FloorIds = [floor.Id];
            else
                building.FloorIds = [.. building.FloorIds.Append(floor.Id)];
            building.LastFloorId = floor.Id;
            await _buildingRepository.UpdateAsync(b => b.Id == floor.BuildingId, building, cancellationToken);
            floor.CreatedAt = now;
            floor.UpdatedAt = now;

            await _floorRepository.AddAsync(floor, cancellationToken);
            await _floorsTransitionRepository.SaveChanges();

            return floor;
        }


        //public async Task InsertFloor(CreateFloorDto floorDto, CancellationToken cancellationToken)
        //{
        //    //Проверка на повтор этажа
        //    if (await _floorRepository.CountAsync(f =>
        //        f.BuildingId == floorDto.BuildingId && f.FloorNumber == floorDto.FloorNumber, cancellationToken) != 0)
        //        throw new AlreadyExistsException("Floor already exists");
        //    //Дата и время
        //    DateTime now = DateTime.UtcNow;

        //    //Проверка на наличие здания
        //    var building = await _buildingRepository.FirstOrDefaultAsync(b =>
        //       b.Id == floorDto.BuildingId, cancellationToken) ?? throw new NotFoundException("Building is not found");
        //    //Проверка на наличие проекта
        //    var project = await _projectRepository.FirstOrDefaultAsync(p =>
        //        p.Id == building.ProjectId, cancellationToken) ?? throw new NotFoundException("Project is not found");
        //    //Проверка на наличие dto точек графа, если нет, то ставится пустой массив
        //    CreateGraphPointFromFloorDto[] graphPointDtos = floorDto.GraphPoints ?? [];

        //    //Словарь для id точек и их связей
        //    Dictionary<string, string[]> graphIdsDict = [];
        //    //Лист для обновленных и новых переходов
        //    List<FloorsTransition> updatedTransitions = [];
        //    List<FloorsTransition> createdTransitions = [];
        //    //Массив для точек
        //    GraphPoint[] graphPoints = new GraphPoint[graphPointDtos.Length];

        //    //Маппинг этажа, заполнение полей
        //    Floor floor = _mapper.Map<Floor>(floorDto);
        //    floor.Id = ObjectId.GenerateNewId().ToString();
        //    floor.Rooms ??= [];
        //    //Обновление здания, добавление в него этажа
        //    if (building.FloorIds == null)
        //        building.FloorIds = [floor.Id];
        //    else
        //        building.FloorIds = [.. building.FloorIds.Append(floor.Id)];
        //    await _buildingRepository.UpdateAsync(b => b.Id == floor.BuildingId, building, cancellationToken);

        //    //Поиск предопределенных типов
        //    var predefinedTypes = await _predefinedGraphPointTypeRepository.ListAsync(cancellationToken);

        //    //Валидация комнат и проходов
        //    var valRoomRes = await ValidateRooms(floor.Rooms, project.CustomGraphPointTypes, graphPointDtos, graphIdsDict, cancellationToken);
        //    graphIdsDict = valRoomRes.Item1;
        //    floor.Rooms = valRoomRes.Item2;

        //    foreach (var room in floor.Rooms)
        //    {
        //        room.CreatedAt = now;
        //        room.UpdatedAt = now;
        //        if (room.Passages != null)
        //            foreach (var pass in room.Passages)
        //            {
        //                pass.CreatedAt = now;
        //                pass.UpdatedAt = now;
        //            }
        //    }

        //    //Работа с каждой точкой
        //    for (int i = 0; i < graphPointDtos.Length; i++)
        //    {
        //        //Проверка на наличие точки в БД
        //        if (await _graphPointRepository.CountAsync(g => g.Id == graphPointDtos[i].Id, cancellationToken) != 0)
        //            throw new AlreadyExistsException($"Graph point with ID {graphPointDtos[i].Id} already exists");

        //        //Проверка на повторное добавление по id
        //        if (graphPoints.Count(g => g.Id == graphPointDtos[i].Id) != 0)
        //            throw new AlreadyExistsException($"Graph point ID {graphPointDtos[i].Id} repeats");

        //        //Маппинг точки, заполнение полей
        //        GraphPoint gp = _mapper.Map<GraphPoint>(graphPointDtos[i]);
        //        gp.FloorId = floor.Id;

        //        //Проверка типов
        //        ValidateTypes(gp, predefinedTypes, project.CustomGraphPointTypes);

        //        //Проверка связей
        //        graphIdsDict = ValidateLinks(gp, graphIdsDict, graphPointDtos);

        //        //Заполнение массивов
        //        graphPoints[i] = gp;

        //        //Проверка на наличие id перехода
        //        if (gp.TransitionId == null) continue;

        //        //Проверка на наличие перехода в БД
        //        FloorsTransition? transition = await _floorsTransitionRepository.FirstOrDefaultAsync(z => 
        //            z.Id == gp.TransitionId, cancellationToken);

        //        //Если его нет
        //        if (transition == null)
        //        {
        //            //Если нет перехода в массиве для созданных переходов, то создается новый
        //            if (!createdTransitions.Any(c => c.Id == gp.TransitionId))
        //            {
        //                var newTransition = new FloorsTransition
        //                {
        //                    Id = gp.TransitionId,
        //                    BuildingId = building.Id,
        //                    LinkIds = [gp.Id],
        //                    CreatedAt = now,
        //                    UpdatedAt = now,
        //                };
        //                createdTransitions.Add(newTransition);
        //            }
        //            //Если есть, то выбрасывается исключение, так как переход только между разными этажами
        //            else
        //            {
        //                throw new AlreadyExistsException(
        //                    $"There is more than 1 graph point with transition_id {gp.TransitionId} on floor {floorDto.FloorNumber}");
        //            }
        //        }
        //        //Если переход есть в БД
        //        else
        //        {
        //            //Поиск перехода в массиве для созданных связей
        //            //Если его нет
        //            if (!updatedTransitions.Any(c => c.Id == transition.Id))
        //            {
        //                if (transition.LinkIds == null) 
        //                    transition.LinkIds = [gp.Id];
        //                else
        //                    transition.LinkIds = [.. transition.LinkIds.Append(gp.Id)];
        //                transition.UpdatedAt = DateTime.UtcNow;
        //                updatedTransitions.Add(transition);
        //            }
        //            //Если он есть, то выбрасывается исключение
        //            else
        //            {
        //                throw new AlreadyExistsException(
        //                    $"There is more than 1 graph point with transition_id {gp.TransitionId} on floor {floorDto.FloorNumber}");
        //            }
        //        }
        //    }

        //    //Добавление и обновление переходов
        //    if (createdTransitions.Count > 0)
        //        await _floorsTransitionRepository.AddRangeAsync(createdTransitions, cancellationToken);

        //    foreach (var update in updatedTransitions)
        //    {
        //        await _floorsTransitionRepository.UpdateAsync(s => s.Id == update.Id,
        //            update, cancellationToken);
        //    }

        //    floor.GraphPoints = [..graphIdsDict.Keys];
        //    //floor.Rooms = floorDto.Rooms == null ? [] : floorDto.Rooms.Values.ToArray();

        //    await _floorRepository.AddAsync(floor, cancellationToken);
        //    if (graphPoints.Length > 0)
        //        await _graphPointRepository.AddRangeAsync([..graphPoints], cancellationToken);

        //    await _floorRepository.SaveChanges();
        //}

        public async Task<IReadOnlyList<Floor>> GetAllFloors(CancellationToken cancellationToken)
        {
            return await _floorRepository.ListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<GraphPoint>> GetGraphPointsByFloor(string id, CancellationToken cancellationToken)
        {
            if (await _floorRepository.CountAsync(b => b.Id == id, cancellationToken) == 0)
                throw new NotFoundException("Floor is not found");
            return await _graphPointRepository.ListAsync(g => g.FloorId == id, cancellationToken);
        }

        public async Task<IReadOnlyList<FloorsTransition>> GetStairsByFloor(string id, CancellationToken cancellationToken)
        {
            if (await _floorRepository.CountAsync(b => b.Id == id, cancellationToken) == 0)
                throw new NotFoundException("Floor is not found");

            var graphPoints = await _graphPointRepository.ListAsync(g => g.FloorId == id, cancellationToken)
                ?? throw new NotFoundException("Graph points are not found");
            var stairIds = new List<string>();
            var res = new List<FloorsTransition>();

            foreach (var graphPoint in graphPoints)
            {
                if (graphPoint.TransitionId != null) stairIds.Add(graphPoint.TransitionId);
            }

            foreach (var stairId in stairIds)
            {
                var stair = await _floorsTransitionRepository.FirstAsync(s => s.Id == stairId, cancellationToken)
                    ?? throw new NotFoundException($"Stair with id {stairId} is not found");
                res.Add(stair);
            }

            return res;
        }

        public async Task<Floor> GetFloorById(string id, CancellationToken cancellationToken)
        {
            var floor = await _floorRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new NotFoundException("Floor is not found");

            var building = await _buildingRepository.FirstOrDefaultAsync(b =>
                 b.Id == floor.BuildingId, cancellationToken) ?? throw new NotFoundException("Building is not found");

            building.LastFloorId = floor.Id;
            await _buildingRepository.UpdateAsync(b => b.Id == floor.BuildingId, building, cancellationToken);

            await _floorRepository.SaveChanges();
            return floor;
        }

        public async Task<GetFloorDto> GetFloorByIdWithGraphPoints(string id,
            CancellationToken cancellationToken)
        {
            var floor = await _floorRepository.FirstOrDefaultAsync(f => f.Id == id, cancellationToken)
                ?? throw new NotFoundException("Floor is not found");
            GetFloorDto res = _mapper.Map<GetFloorDto>(floor);

            var building = await _buildingRepository.FirstOrDefaultAsync(b =>
                b.Id == floor.BuildingId, cancellationToken) ?? throw new NotFoundException("Building is not found");

            building.LastFloorId = floor.Id;
            await _buildingRepository.UpdateAsync(b => b.Id == floor.BuildingId, building, cancellationToken);

            res.GraphPoints = [..await _graphPointRepository.ListAsync(g => g.FloorId == floor.Id, cancellationToken)];

            await _floorRepository.SaveChanges();
            return res;
        }

        public async Task<Tuple<Models.Entities.Image?, MultipartContent>> GetFloorByIdWithGraphPointsMultipart(string id,
            CancellationToken cancellationToken)
        {
            var floor = await _floorRepository.FirstOrDefaultAsync(f => f.Id == id, cancellationToken)
                ?? throw new NotFoundException("Floor is not found");
            GetFloorDto res = _mapper.Map<GetFloorDto>(floor);

            var building = await _buildingRepository.FirstOrDefaultAsync(b =>
                b.Id == floor.BuildingId, cancellationToken) ?? throw new NotFoundException("Building is not found");

            building.LastFloorId = floor.Id;
            await _buildingRepository.UpdateAsync(b => b.Id == floor.BuildingId, building, cancellationToken);

            res.GraphPoints = [.. await _graphPointRepository.ListAsync(g => g.FloorId == floor.Id, cancellationToken)];

            await _floorRepository.SaveChanges();

            MultipartContent multipartContent = new MultipartContent("mixed");

            var jsonContent = new StringContent(JsonSerializer.Serialize(res), Encoding.UTF8, "application/json");
            multipartContent.Add(jsonContent);

            if (floor.Background != null)
            {
                var tuple = await _imageService.GetImageById(floor.Background.ImageId, CancellationToken.None);
                var fileContent = new ByteArrayContent(tuple.Item2.ToArray());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(tuple.Item1.MimeType);
                multipartContent.Add(fileContent);
                return new Tuple<Models.Entities.Image?, MultipartContent>(tuple.Item1, multipartContent);
            }

            return new Tuple<Models.Entities.Image?, MultipartContent>(null, multipartContent);
        }

        public async Task DeleteFloor(string id, CancellationToken cancellationToken)
        {
            var floor = await _floorRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new NotFoundException("Floor is not found");

            var building = await _buildingRepository.FirstOrDefaultAsync(b =>
                b.Id == floor.BuildingId, cancellationToken) ?? throw new NotFoundException("Building is not found");
            if (building.LastFloorId == floor.Id)
            {
                building.LastFloorId = "";
                await _buildingRepository.UpdateAsync(b => b.Id == floor.BuildingId, building, cancellationToken);
            }

            if (floor.Background != null)
                await _imageService.DeleteImageById(floor.Background.ImageId, cancellationToken);

            foreach (var point in floor.GraphPoints)
            {
                var connection = await _floorsTransitionRepository.FirstOrDefaultAsync(c => c.LinkIds != null &&
                    c.LinkIds.Contains(point), cancellationToken);
                if (connection is not null)
                {
                    if (connection.LinkIds is not null)
                        connection.LinkIds = [..connection.LinkIds.Where(l => l != point)];
                    connection.UpdatedAt = DateTime.UtcNow;
                    await _floorsTransitionRepository.UpdateAsync(c => c.Id == connection.Id,
                        connection, cancellationToken);
                }
            }
            await _graphPointRepository.RemoveRangeAsync(g => floor.GraphPoints.Contains(g.Id), cancellationToken);
            await _floorRepository.RemoveAsync(f => f.Id == id, cancellationToken);
            await _floorRepository.SaveChanges();
        }

        public async Task UpdateFloor(string id, UpdateFloorDto floorDto, IFormFile file, CancellationToken cancellationToken)
        {
            var prevFloor = await _floorRepository.FirstOrDefaultAsync(f => f.Id == id, cancellationToken)
                ?? throw new NotFoundException("Floor is not found");
            //Проверка на наличие здания
            var building = await _buildingRepository.FirstOrDefaultAsync(b =>
               b.Id == prevFloor.BuildingId, cancellationToken) ?? throw new NotFoundException("Building is not found");
            //Проверка на наличие проекта
            var project = await _projectRepository.FirstOrDefaultAsync(p =>
                p.Id == building.ProjectId, cancellationToken) ?? throw new NotFoundException("Project is not found");
            DateTime now = DateTime.UtcNow;
            Dictionary<string, string[]> graphIdsDict = [];
            building.LastFloorId = prevFloor.Id;

            if (file != null && floorDto.Background != null)
            {
                if (prevFloor.Background != null)
                    await _imageService.DeleteImageById(prevFloor.Background.ImageId, cancellationToken);
                var image = await _imageService.InsertImage(file, cancellationToken);
                prevFloor.Background = new BackgroundImage
                {
                    ImageId = image.Id, 
                    X = floorDto.Background.X,
                    Y = floorDto.Background.Y,
                    //Width = 10,
                    //Height = 10,
                    Multiplier = floorDto.Background.Multiplier,
                };
            }
            else if (floorDto.Background != null && floorDto.Background.ImageId == "")
                await _imageService.DeleteImageById(prevFloor.Background.ImageId, cancellationToken);
            else if (file == null && floorDto.Background.ImageId == prevFloor.Background.ImageId)
            {
                prevFloor.Background.X = floorDto.Background.X;
                prevFloor.Background.Y = floorDto.Background.Y;
                prevFloor.Background.Multiplier = floorDto.Background.Multiplier;
            }

            if (floorDto.BuildingId != null && floorDto.BuildingId != prevFloor.BuildingId)
            {
                if (building.LastFloorId == prevFloor.Id)
                    building.LastFloorId = "";
                await _buildingRepository.UpdateAsync(b => b.Id == prevFloor.BuildingId, building, cancellationToken);

                building = await _buildingRepository.FirstOrDefaultAsync(b =>
                    b.Id == floorDto.BuildingId, cancellationToken) ?? throw new NotFoundException("Building is not found");
                project = await _projectRepository.FirstOrDefaultAsync(p =>
                    p.Id == building.ProjectId, cancellationToken) ?? throw new NotFoundException("Project is not found");

                if (await _floorRepository.CountAsync(f => f.Index == floorDto.Index &&
                    f.BuildingId == floorDto.BuildingId, cancellationToken) != 0)
                    throw new AlreadyExistsException(
                        $"Floor in building {floorDto.BuildingId} with number {floorDto.Index} already exists");
                else
                {
                    if (floorDto.Index != null && floorDto.Index != prevFloor.Index)
                    {
                        prevFloor.Index = (int)floorDto.Index;
                        prevFloor.BuildingId = floorDto.BuildingId;
                    }

                    building.LastFloorId = prevFloor.Id;
                    await _buildingRepository.UpdateAsync(b => b.Id == prevFloor.BuildingId, building, cancellationToken);
                }
            }
            else if (floorDto.Index != null && floorDto.Index != prevFloor.Index)
            {
                var floorForExchange = await _floorRepository.FirstOrDefaultAsync(f => f.Index == floorDto.Index &&
                    f.BuildingId == floorDto.BuildingId, cancellationToken);
                if (floorForExchange != null)
                {
                    floorForExchange.Index = prevFloor.Index;
                    await _floorRepository.UpdateAsync(f => f.Id == floorForExchange.Id, floorForExchange, cancellationToken);
                    prevFloor.Index = (int)floorDto.Index;
                }
                await _buildingRepository.UpdateAsync(b => b.Id == prevFloor.BuildingId, building, cancellationToken);
            }
            else
                await _buildingRepository.UpdateAsync(b => b.Id == prevFloor.BuildingId, building, cancellationToken);

            prevFloor.Name = floorDto.Name ?? prevFloor.Name;
            //prevFloor.ImageIds = floorDto.ImageIds;
            prevFloor.Width = floorDto.Width ?? prevFloor.Width;
            prevFloor.Height = floorDto.Height ?? prevFloor.Height;
            prevFloor.Decorations = floorDto.Decorations ?? prevFloor.Decorations;
            prevFloor.UpdatedAt = now;

            if (floorDto.Rooms != null)
            {
                Tuple<Dictionary<string, string[]>, Room[]> roomTuple;
                if (floorDto.GraphPoints != null)
                    roomTuple = await ValidateRooms(floorDto.Rooms, project.CustomGraphPointTypes,
                        floorDto.GraphPoints, graphIdsDict, cancellationToken);
                else
                    roomTuple = await ValidateRooms(floorDto.Rooms, project.CustomGraphPointTypes,
                        await _graphPointRepository.CreateGraphPointsFromFloorListAsync(id), graphIdsDict, cancellationToken);


                foreach (var room in floorDto.Rooms)
                {
                    room.UpdatedAt = now;
                    var prevRoom = prevFloor.Rooms.FirstOrDefault(r => r.Id == room.Id);
                    try
                    {
                        room.CreatedAt = prevRoom.CreatedAt;
                    }
                    catch (Exception ex)
                    {
                        room.CreatedAt = now;
                    }

                    if (room.Passages != null)
                        foreach (var pass in room.Passages)
                        {
                            pass.UpdatedAt = now;

                            try
                            {
                                pass.CreatedAt = prevRoom
                                    .Passages
                                    .FirstOrDefault(p => p.Id == pass.Id)
                                    .CreatedAt;
                            }
                            catch (Exception ex)
                            {
                                pass.CreatedAt = now;
                            }
                        }
                }
            }

            if (floorDto.GraphPoints != null)
            {
                //Поиск предопределенных типов
                var predefinedTypes = await _predefinedGraphPointTypeRepository.ListAsync(cancellationToken);
                List<CreateGraphPointFromFloorDto> gpDtoForUpdate = [];
                List<CreateGraphPointFromFloorDto> gpDtoForInsert = [];
                List<GraphPoint> gpForInsert = [];
                List<string> gpIdForDelete = [];
                List<FloorsTransition> updatedTransitions = [];
                List<FloorsTransition> createdTransitions = [];

                foreach (var gp in floorDto.GraphPoints)
                {
                    //Проверка на повторное добавление по id
                    //if (floorDto.GraphPoints.Count(g => g.Id == gp.Id) > 1)
                    //    throw new AlreadyExistsException($"Graph point ID {gp.Id} repeats");
                    if (gpDtoForUpdate.Any(g => g.Id == gp.Id) || gpDtoForInsert.Any(g => g.Id == gp.Id))
                        throw new AlreadyExistsException($"Graph point ID {gp.Id} repeats");

                    if (prevFloor.GraphPoints.Contains(gp.Id))
                        gpDtoForUpdate.Add(gp);
                    else gpDtoForInsert.Add(gp);
                }
                foreach (var gpid in prevFloor.GraphPoints)
                {
                    if (floorDto.GraphPoints.Any(g => g.Id == gpid))
                        gpIdForDelete.Add(gpid);
                }

                foreach (var gpDto in gpDtoForUpdate)
                {
                    //Маппинг точки, заполнение полей
                    GraphPoint gp = _mapper.Map<GraphPoint>(gpDto);
                    gp.FloorId = prevFloor.Id;
                    gp.CreatedAt = prevFloor.CreatedAt;
                    gp.UpdatedAt = now;

                    //Проверка типов
                    ValidateTypes(gp, predefinedTypes, project.CustomGraphPointTypes);

                    //Проверка связей
                    graphIdsDict = ValidateLinks(gp, graphIdsDict, [..gpDtoForUpdate]);

                    //Проверка на наличие id перехода
                    if (gp.TransitionId == null) continue;

                    //Проверка на наличие перехода в БД
                    FloorsTransition? transition = await _floorsTransitionRepository.FirstOrDefaultAsync(z =>
                        z.Id == gp.TransitionId, cancellationToken);

                    //Если его нет
                    if (transition == null)
                    {
                        //Поиск перехода в массиве для созданных переходов
                        //Если его нет, то создается новый
                        if (!createdTransitions.Any(c => c.Id == gp.TransitionId))
                        {
                            var newTransition = new FloorsTransition
                            {
                                Id = gp.TransitionId,
                                BuildingId = building.Id,
                                LinkIds = [gp.Id],
                                CreatedAt = now,
                                UpdatedAt = now,
                            };
                            createdTransitions.Add(newTransition);
                        }
                        //Если есть, то выбрасывается исключение, так как переход только между разными этажами
                        else
                        {
                            throw new AlreadyExistsException(
                                $"There is more than 1 graph point with transition_id {gp.TransitionId} on floor {floorDto.Index}");
                        }
                    }
                    //Если переход есть в БД
                    else
                    {
                        //Поиск перехода в массиве для созданных связей
                        var transitionForUpdate = updatedTransitions.FirstOrDefault(c => c.Id == transition.Id);
                        //Добавляется новый
                        if (transitionForUpdate == null)
                        {
                            if (transition.LinkIds == null)
                                transition.LinkIds = [gp.Id];
                            else
                                transition.LinkIds = [.. transition.LinkIds.Append(gp.Id)];
                            transition.UpdatedAt = DateTime.UtcNow;
                            updatedTransitions.Add(transition);
                        }
                        //Обновляется старый
                        else
                        {
                            if (transitionForUpdate.LinkIds == null)
                                transitionForUpdate.LinkIds = [gp.Id];
                            else
                                transitionForUpdate.LinkIds = [.. transitionForUpdate.LinkIds.Append(gp.Id)];
                        }
                    }
                }

                foreach (var gpDto in gpDtoForInsert)
                {
                    if (await _graphPointRepository.CountAsync(g => g.Id == gpDto.Id, cancellationToken) != 0)
                        throw new AlreadyExistsException($"Graph point with ID {gpDto.Id} already exists");
                    GraphPoint gp = _mapper.Map<GraphPoint>(gpDto);
                    gp.FloorId = prevFloor.Id;
                    gp.CreatedAt = now;
                    gp.UpdatedAt = now;

                    //Проверка типов
                    ValidateTypes(gp, predefinedTypes, project.CustomGraphPointTypes);

                    //Проверка связей
                    graphIdsDict = ValidateLinks(gp, graphIdsDict, [.. gpDtoForInsert]);

                    //Проверка на наличие id перехода
                    if (gp.TransitionId == null) continue;

                    //Проверка на наличие перехода в БД
                    FloorsTransition? transition = await _floorsTransitionRepository.FirstOrDefaultAsync(z =>
                        z.Id == gp.TransitionId, cancellationToken);

                    //Если его нет
                    if (transition == null)
                    {
                        //Поиск перехода в массиве для созданных переходов
                        //Если его нет, то создается новый
                        if (!createdTransitions.Any(c => c.Id == gp.TransitionId))
                        {
                            var newTransition = new FloorsTransition
                            {
                                Id = gp.TransitionId,
                                BuildingId = building.Id,
                                LinkIds = [gp.Id],
                                CreatedAt = now,
                                UpdatedAt = now,
                            };
                            createdTransitions.Add(newTransition);
                        }
                        //Если есть, то выбрасывается исключение, так как переход только между разными этажами
                        else
                        {
                            throw new AlreadyExistsException(
                                $"There is more than 1 graph point with transition_id {gp.TransitionId} on floor {floorDto.Index}");
                        }
                    }
                    //Если переход есть в БД
                    else
                    {
                        //Поиск перехода в массиве для созданных связей
                        var transitionForUpdate = updatedTransitions.FirstOrDefault(c => c.Id == transition.Id);
                        //Добавляется новый
                        if (transitionForUpdate == null)
                        {
                            if (transition.LinkIds == null)
                                transition.LinkIds = [gp.Id];
                            else
                                transition.LinkIds = [.. transition.LinkIds.Append(gp.Id)];
                            transition.UpdatedAt = DateTime.UtcNow;
                            updatedTransitions.Add(transition);
                        }
                        //Обновляется старый
                        else
                        {
                            if (transitionForUpdate.LinkIds == null)
                                transitionForUpdate.LinkIds = [gp.Id];
                            else
                                transitionForUpdate.LinkIds = [.. transitionForUpdate.LinkIds.Append(gp.Id)];
                        }
                    }
                    gpForInsert.Add(gp);
                }

                var graphPointsForDelete = await _graphPointRepository.ListAsync(g => 
                    gpIdForDelete.Contains(g.Id), cancellationToken);

                foreach (var gp in graphPointsForDelete)
                {
                    if (gp.TransitionId != null)
                    {
                        var transitionForUpdate = updatedTransitions.FirstOrDefault(c => c.Id == gp.TransitionId);
                        if (transitionForUpdate == null)
                        {
                            FloorsTransition? transition = await _floorsTransitionRepository.FirstOrDefaultAsync(z =>
                                z.Id == gp.TransitionId, cancellationToken);

                            if (transition == null) throw new NotFoundException(
                                $"Transition {gp.TransitionId} is not found");

                            if (transition.LinkIds == null) 
                                transition.LinkIds = [];
                            else
                                transition.LinkIds = [.. transition.LinkIds.Where(i => i != gp.Id)];
                            transition.UpdatedAt = DateTime.UtcNow;
                            updatedTransitions.Add(transition);
                        }
                        else
                        {
                            if (transitionForUpdate.LinkIds == null)
                                transitionForUpdate.LinkIds = [];
                            else
                                transitionForUpdate.LinkIds = [.. transitionForUpdate.LinkIds.Where(i => i != gp.Id)];
                        }
                    }
                }

                //Добавление и обновление переходов
                if (createdTransitions.Count > 0)
                    await _floorsTransitionRepository.AddRangeAsync(createdTransitions, cancellationToken);

                foreach (var transition in updatedTransitions)
                {
                    //Если у перехода есть ровно одна ссылка на точку на этаже
                    if (transition.LinkIds.Intersect(prevFloor.GraphPoints).Count() <= 1)
                        await _floorsTransitionRepository.UpdateAsync(s => 
                            s.Id == transition.Id, transition, cancellationToken);
                    //Иначе исключение
                    else throw new AlreadyExistsException(
                        $"There is more than 1 graph point with transition_id {transition.Id} on floor {floorDto.Index}");
                }

                //Добавление и обновление точек
                if (gpForInsert.Count > 0)
                    await _graphPointRepository.AddRangeAsync(gpForInsert, cancellationToken);
                if (gpIdForDelete.Count > 0)
                    await _graphPointRepository.RemoveRangeAsync(g => gpIdForDelete.Contains(g.Id), cancellationToken);

                prevFloor.GraphPoints = graphIdsDict.Keys.ToArray();
            }

            await _floorRepository.UpdateAsync(f => f.Id == prevFloor.Id, prevFloor, cancellationToken);
            await _floorRepository.SaveChanges();
        }
    }
}
