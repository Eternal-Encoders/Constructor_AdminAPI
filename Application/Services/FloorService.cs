using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Application.Services
{
    public class FloorService
    {
        IFloorRepository _floorRepository;
        IGraphPointRepository _graphPointRepository;
        IFloorConnectionRepository _floorConnectionRepository;
        IBuildingRepository _buildingRepository;
        IProjectRepository _projectRepository;
        IPredefinedGraphPointTypeRepository _predefinedGraphPointTypeRepository;
        IMapper _mapper;

        public FloorService(IFloorRepository floorRepository, IGraphPointRepository graphPointRepository,
            IFloorConnectionRepository floorConnectionRepository, IBuildingRepository buildingRepository, IMapper mapper,
            IProjectRepository projectRepository, IPredefinedGraphPointTypeRepository predefinedGraphPointTypeRepository)
        {
            _floorRepository = floorRepository;
            _graphPointRepository = graphPointRepository;
            _floorConnectionRepository = floorConnectionRepository;
            _buildingRepository = buildingRepository;
            _mapper = mapper;
            _projectRepository = projectRepository;
            _predefinedGraphPointTypeRepository = predefinedGraphPointTypeRepository;
        }

        public async Task InsertFloor(CreateFloorDto floorDto, CancellationToken cancellationToken)
        {
            //Проверка на повтор этажа
            if (await _floorRepository.FirstOrDefaultAsync(f =>
                f.BuildingId == floorDto.BuildingId && f.FloorNumber == floorDto.FloorNumber, cancellationToken) != null)
                throw new AlreadyExistsException("Floor already exists");
            DateTime now = DateTime.UtcNow;

            //Проверка на наличие здания
            var building = await _buildingRepository.FirstOrDefaultAsync(b =>
               b.Id == floorDto.BuildingId, cancellationToken) ?? throw new NotFoundException("Building is not found");

            var project = await _projectRepository.FirstOrDefaultAsync(p =>
                p.Id == building.ProjectId, cancellationToken) ?? throw new NotFoundException("Project is not found");

            //Проверка на наличие dto точек графа, если нет, то ставится пустой массив
            CreateGraphPointFromFloorDto[] graphPointDtos = floorDto.GraphPoints ?? [];
            //Массив для id точек
            string[] graphIds = new string[graphPointDtos.Length];
            //Лист для обновленных и новых соединений
            List<FloorConnection> updatedConnections = [];
            List<FloorConnection> createdConnections = [];
            //Массив для точек
            GraphPoint[] graphPoints = new GraphPoint[graphPointDtos.Length];
            //Маппинг этажа, заполнение полей
            Floor floor = _mapper.Map<Floor>(floorDto);
            floor.Id = ObjectId.GenerateNewId().ToString();
            //Обновление здания, добавление в него этажа
            building.FloorIds ??= [];
            building.FloorIds = [..building.FloorIds.Append(floor.Id)];
            await _buildingRepository.UpdateAsync(b => b.Id == floor.BuildingId, building, cancellationToken);

            //Поиск предопределенных типов
            var predefinedTypes = await _predefinedGraphPointTypeRepository.ListAsync(cancellationToken);

            CreateGraphPointFromFloorDto? point;

            //Проверка комнат
            floor.Rooms ??= [];
            foreach (var room in floor.Rooms)
            {
                point = graphPointDtos.FirstOrDefault(g => g.Id == room.Id);
                if (point is null)
                    throw new NotFoundException($"There is a room {room.Id} without graph point");
                else if (point.Types.Contains("corridor"))
                    throw new ValidationException($"Graph point {room.Id} has wrong type \"corridor\"");
                else
                {
                    if (room.Doors != null && point.Links != null)
                    {
                        foreach (var door in room.Doors)
                            if (!graphPointDtos.Any(g => g.Id == door.Id))
                                throw new NotFoundException($"There is a door {door.Id} without graph point");
                            else if (!graphPointDtos.FirstOrDefault(g => g.Id == door.Id).Types.Contains("door"))
                                throw new ValidationException($"Graph point {room.Id} has not type \"door\"");
                            else if (point.Links.Contains(graphPointDtos.FirstOrDefault(g => g.Id == door.Id).Id))
                                throw new ValidationException($"Graph point {room.Id} of room is not linked with graph point of door");
                        if (room.Doors.Length != point.Links.Length)
                            throw new ValidationException("Room pointy has extra links");
                    }
                }
                room.CreatedAt = now;
                room.UpdatedAt = now;  
            }

            //Работа с каждой точкой
            for (int i = 0; i < graphPointDtos.Length; i++)
            {
                //Проверка на наличие точки в БД
                if (await _graphPointRepository.FirstOrDefaultAsync(g => g.Id == graphPointDtos[i].Id, cancellationToken) != null)
                    throw new AlreadyExistsException($"Graph point {graphPointDtos[i].Id} already exists");

                //Маппинг точки, заполнение полей
                GraphPoint gp = _mapper.Map<GraphPoint>(graphPointDtos[i]);
                gp.FloorId = floor.Id;

                //Проверка типов
                foreach (string type in gp.Types)
                {
                    //Предопределенные
                    if (!predefinedTypes.Any(t => t.Id == type))
                    {
                        //Заданные пользователем
                        if (project.CustomGraphPointTypes == null || !project.CustomGraphPointTypes.Any(t => t.Name == type))
                        {
                            throw new NotFoundException($"Graph point type \"{type}\" is not found");
                        }
                    }
                }

                //Заполнение массивов
                graphPoints[i] = gp;
                graphIds[i] = gp.Id;

                //Проверка на наличие id связи этажей
                if (gp.ConnectionId is null) continue;

                //Проверка на наличие связи в БД
                FloorConnection? connection = await _floorConnectionRepository.FirstOrDefaultAsync(z => 
                    z.Id == gp.ConnectionId, cancellationToken);

                //Если ее нет
                if (connection is null)
                {
                    //Поиск связи в массиве для созданных связей
                    var connectionForCreation = createdConnections.FirstOrDefault(c => c.Id == gp.ConnectionId);
                    //Если ее нет, то создается новая
                    if (connectionForCreation is null)
                    {
                        var newConnection = new FloorConnection
                        {
                            Id = gp.ConnectionId,
                            BuildingId = building.Id,
                            Links = [gp.Id],
                            CreatedAt = now,
                            UpdatedAt = now,
                        };
                        createdConnections.Add(newConnection);
                    }
                    //Если есть, то выбрасывается исключение, так как связь между разными этажами
                    else
                    {
                        throw new AlreadyExistsException(
                            $"There is more than 1 graph point with ConnectionId {gp.ConnectionId} on floor {floorDto.FloorNumber}");
                    }
                }
                //Если связь есть в БД
                else
                {
                    //Поиск связи в массиве для созданных связей
                    var connectionForUpdate = updatedConnections.FirstOrDefault(c => c.Id == connection.Id);
                    //Если ее нет
                    if (connectionForUpdate is null)
                    {
                        connection.Links ??= [gp.Id];
                        connection.Links = [..connection.Links.Append(gp.Id)];
                        connection.UpdatedAt = DateTime.UtcNow;
                        updatedConnections.Add(connection);
                    }
                    //Если она есть, то выбрасывается исключение
                    else
                    {
                        throw new AlreadyExistsException(
                            $"There is more than 1 graph point with ConnectionId {gp.ConnectionId} on floor {floorDto.FloorNumber}");
                    }
                }
            }

            //Добавление и обновление соединений
            await _floorConnectionRepository.AddRangeAsync(createdConnections, cancellationToken);

            for (int i = 0; i < updatedConnections.Count(); i++)
            {
                await _floorConnectionRepository.UpdateAsync(s => s.Id == updatedConnections[i].Id,
                    updatedConnections[i], cancellationToken);
            }

            floor.GraphPoints = [..graphIds];
            //floor.Rooms = floorDto.Rooms == null ? [] : floorDto.Rooms.Values.ToArray();

            await _floorRepository.AddAsync(floor, cancellationToken);
            await _graphPointRepository.AddRangeAsync([..graphPoints], cancellationToken);

            await _floorConnectionRepository.SaveChanges();
        }

        public async Task<IReadOnlyList<Floor>> GetAllFloors(CancellationToken cancellationToken)
        {
            var res = await _floorRepository.ListAsync(cancellationToken);
            return res;
        }

        public async Task<IReadOnlyList<GraphPoint>> GetGraphPointsByFloor(string id, CancellationToken cancellationToken)
        {
            if (await _floorRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken) == null)
                throw new NotFoundException("Floor is not found");

            var res = await _graphPointRepository.ListAsync(g => g.FloorId == id, cancellationToken);
            return res;
        }

        public async Task<IReadOnlyList<FloorConnection>> GetStairsByFloor(string id, CancellationToken cancellationToken)
        {
            if (await _floorRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken) == null)
                throw new NotFoundException("Floor is not found");

            var graphPoints = await _graphPointRepository.ListAsync(g => g.FloorId == id, cancellationToken);
            if (graphPoints == null) throw new NotFoundException("Graph points are not found");

            var stairIds = new List<string>();
            var res = new List<FloorConnection>();

            foreach (var graphPoint in graphPoints)
            {
                if (graphPoint.ConnectionId != null) stairIds.Add(graphPoint.ConnectionId);
            }

            foreach (var stairId in stairIds)
            {
                var stair = await _floorConnectionRepository.FirstAsync(s => s.Id == stairId, cancellationToken);
                if (stair == null) throw new NotFoundException($"Stair with id {stairId} is not found");
                res.Add(stair);
            }

            return res;
        }

        public async Task<Floor> GetFloorById(string id, CancellationToken cancellationToken)
        {
            var floor = await _floorRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            if (floor == null) throw new NotFoundException("Floor is not found");

            return floor;
        }

        public async Task DeleteFloor(string id, CancellationToken cancellationToken)
        {
            var floor = await _floorRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)
                ?? throw new NotFoundException("Floor is not found");

            foreach (var point in floor.GraphPoints)
            {
                var connection = await _floorConnectionRepository.FirstOrDefaultAsync(c => c.Links != null &&
                    c.Links.Contains(point), cancellationToken);
                if (connection is not null)
                {
                    if (connection.Links is not null)
                        connection.Links = [..connection.Links.Where(l => l != point)];
                    connection.UpdatedAt = DateTime.UtcNow;
                    await _floorConnectionRepository.UpdateAsync(c => c.Id == connection.Id,
                        connection, cancellationToken);
                }
            }
            await _graphPointRepository.RemoveRangeAsync(g => floor.GraphPoints.Contains(g.Id), cancellationToken);
            await _floorRepository.RemoveAsync(f => f.Id == id, cancellationToken);
            await _floorRepository.SaveChanges();
        }
    }
}
