using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using System.Linq;

namespace Constructor_API.Application.Services
{
    public class GraphPointService
    {
        IGraphPointRepository _graphPointRepository;
        IFloorsTransitionRepository _floorConnectionRepository;
        IFloorRepository _floorRepository;
        IBuildingRepository _buildingRepository;
        IProjectRepository _projectRepository;
        IPredefinedGraphPointTypeRepository _predefinedGraphPointTypeRepository;
        IMapper _mapper;

        public GraphPointService(IGraphPointRepository graphPointRepository, IFloorsTransitionRepository floorConnectionRepository,
            IFloorRepository floorRepository, IMapper mapper, IBuildingRepository buildingRepository, 
            IProjectRepository projectRepository, IPredefinedGraphPointTypeRepository predefinedGraphPointTypeRepository)
        {
            _graphPointRepository = graphPointRepository;
            _floorConnectionRepository = floorConnectionRepository;
            _floorRepository = floorRepository;
            _mapper = mapper;
            _buildingRepository = buildingRepository;
            _projectRepository = projectRepository;
            _predefinedGraphPointTypeRepository = predefinedGraphPointTypeRepository;
        }

        public async Task InsertGraphPoint(CreateGraphPointDto graphPointDto, CancellationToken cancellationToken)
        {
            //Проверка на повтор точки по id
            if (await _graphPointRepository.CountAsync(g => g.Id == graphPointDto.Id, cancellationToken) != 0)
                throw new AlreadyExistsException($"Graph point {graphPointDto.Id} already exists");

            //Этаж, полученный из dto точки
            Floor floor = await _floorRepository.FirstOrDefaultAsync(f => f.Id == graphPointDto.FloorId, cancellationToken) 
                ?? throw new NotFoundException($"Floor {graphPointDto.FloorId} is not found");

            //В массив id точек в этаже добавляется новая точка
            floor.GraphPoints ??= [];
            floor.GraphPoints = [..floor.GraphPoints.Append(graphPointDto.Id)];
            floor.UpdatedAt = DateTime.UtcNow;
            await _floorRepository.UpdateAsync(f => f.Id == graphPointDto.FloorId, floor, cancellationToken);

            //Проверка на наличие id соединения между этажами у точки
            if (graphPointDto.TransitionId != null)
            {
                //Поиск соединения по id
                FloorsTransition? connection = await _floorConnectionRepository.FirstOrDefaultAsync(s => s.Id == 
                    graphPointDto.TransitionId, cancellationToken);
                if (connection == null)
                {
                    //Если не найдено, то создается новое
                    var newConnection = new FloorsTransition
                    {
                        Id = graphPointDto.TransitionId,
                        BuildingId = floor.BuildingId,
                        LinkIds = [graphPointDto.Id],
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    await _floorConnectionRepository.AddAsync(newConnection, cancellationToken);
                }
                else
                {
                    //Проверка на наличие ссылки 2-х точек на одном этаже на одно соединение
                    if (floor.GraphPoints.Any(g => g == graphPointDto.Id))
                        throw new AlreadyExistsException(
                            $"There is more than 1 graph point with ConnectionId {graphPointDto.TransitionId} " +
                            $"on floor {floor.FloorNumber}");

                    //Если соединение найдено и ссылка на него одна, то id точки добавляется в его массив Links
                    connection.LinkIds ??= [];
                    if (!connection.LinkIds.Contains(graphPointDto.Id))
                        connection.LinkIds = [..connection.LinkIds.Append(graphPointDto.Id)];
                    connection.UpdatedAt = DateTime.UtcNow;
                    await _floorConnectionRepository.UpdateAsync(s => s.Id == connection.Id, connection, cancellationToken);
                }
            }
            //Возможно стоит типы перенести из проекта в пользователя
            var building = await _buildingRepository.FirstOrDefaultAsync(b => b.Id == floor.BuildingId, cancellationToken);
            var project = await _projectRepository.FirstOrDefaultAsync(p => p.Id == building.ProjectId, cancellationToken);

            //Маппинг из dto в точку
            GraphPoint graphPoint = _mapper.Map<GraphPoint>(graphPointDto);

            //Предопределенные типы
            var predefinedTypes = await _predefinedGraphPointTypeRepository.ListAsync(cancellationToken);

            //Проверка каждого типа у точки
            foreach (string type in graphPoint.Types)
            {
                //Если нет мреди предопределенных
                if (!predefinedTypes.Any(t => t.Name == type))
                {
                    //Проверяются пользовательские
                    if (project.CustomGraphPointTypes == null || !project.CustomGraphPointTypes.Any(t => t.Name == type))
                    {
                        throw new NotFoundException($"Graph point type \"{type}\" is not found");
                    }
                }
            }

            await _graphPointRepository.AddAsync(graphPoint, cancellationToken);
            await _graphPointRepository.SaveChanges();

            //return Result.Result.Success();
        }

        public async Task<GraphPoint> GetGraphPointById(string id, CancellationToken cancellationToken)
        {
            return await _graphPointRepository.FirstOrDefaultAsync(g => g.Id == id, cancellationToken)
                ?? throw new NotFoundException($"Graph point is not found");
        }

        public async Task<IReadOnlyList<GraphPoint>> GetAllGraphPoints(CancellationToken cancellationToken)
        {
            return await _graphPointRepository.ListAsync(cancellationToken);
        }

        public async Task<FloorsTransition> GetFloorConnectionByGraphPoint(string id, CancellationToken cancellationToken)
        {
            var graphPoint = await _graphPointRepository.FirstOrDefaultAsync(g => g.Id == id, cancellationToken)
                ?? throw new NotFoundException($"Graph point is not found");
            return await _floorConnectionRepository.FirstOrDefaultAsync(c => c.Id == graphPoint.TransitionId,
                cancellationToken) ?? throw new NotFoundException("Floor connection is not found");
        }

        public async Task DeleteGraphPoint(string id, CancellationToken cancellationToken)
        {
            var graphPoint = await _graphPointRepository.FirstOrDefaultAsync(g => g.Id == id, cancellationToken)
                ?? throw new NotFoundException("Graph point is not found");
            var floor = await _floorRepository.FirstOrDefaultAsync(f => f.Id == graphPoint.FloorId, cancellationToken)
                ?? throw new NotFoundException("Floor is not found");
            floor.GraphPoints = [..floor.GraphPoints.Where(g => g != id)];

            var room = floor.Rooms.FirstOrDefault(r => r.Id == id);
            if (room != null) floor.Rooms = [..floor.Rooms.Where(r => r.Id != id)];
            floor.UpdatedAt = DateTime.UtcNow;

            var connection = await _floorConnectionRepository.FirstOrDefaultAsync(c => c.Id == graphPoint.TransitionId,
                cancellationToken);
            if (connection != null && connection.LinkIds != null)
            {
                connection.LinkIds = [.. connection.LinkIds.Where(g => g != id)];
                connection.UpdatedAt = DateTime.UtcNow;
            }

            if (connection != null)
                await _floorConnectionRepository.UpdateAsync(c => c.Id == connection.Id, connection, cancellationToken);
            await _floorRepository.UpdateAsync(f => f.Id == floor.Id, floor, cancellationToken);
            await _graphPointRepository.RemoveAsync(g => g.Id == id, cancellationToken);
            await _graphPointRepository.SaveChanges();
        }
    }
}
