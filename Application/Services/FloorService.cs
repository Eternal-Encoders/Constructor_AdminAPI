using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Application.Services
{
    public class FloorService
    {
        IFloorRepository _floorRepository;
        IGraphPointRepository _graphPointRepository;
        IStairRepository _stairRepository;
        IBuildingRepository _buildingRepository;
        IMapper _mapper;

        public FloorService(IFloorRepository floorRepository, IGraphPointRepository graphPointRepository,
            IStairRepository stairRepository, IBuildingRepository buildingRepository, IMapper mapper)
        {
            _floorRepository = floorRepository;
            _graphPointRepository = graphPointRepository;
            _stairRepository = stairRepository;
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        public async Task InsertFloor(CreateFloorDto floorDto, CancellationToken cancellationToken)
        {
            if (await _floorRepository.FirstOrDefaultAsync(f =>
                f.BuildingId == floorDto.BuildingId && f.FloorNumber == floorDto.FloorNumber, cancellationToken) != null)
                throw new AlreadyExistsException("Floor already exists");

            Building building = building = await _buildingRepository.FirstOrDefaultAsync(b =>
               b.Id == floorDto.BuildingId, cancellationToken);

            if (building == null)
                throw new NotFoundException("Building not found");

            if (floorDto.FloorNumber > building.MaxFloor || floorDto.FloorNumber < building.MinFloor) 
                throw new ValidationException("Floor number is out of bounds");

            var graphPointsDto = floorDto.GraphPoints == null ? [] : floorDto.GraphPoints;
            List<string> graphIds = [];
            var stairs = await _stairRepository.ListAsync(s => s.BuildingId == building.Id, cancellationToken);
            List<Stair> updatedStairs = [];
            List<GraphPoint> graphPoints = [];
            Floor floor = _mapper.Map<Floor>(floorDto);
            floor.Id = ObjectId.GenerateNewId().ToString();

            building.FloorIds.Append(floor.Id);
            await _buildingRepository.UpdateAsync(f => f.Id == floor.Id, building, cancellationToken);

            foreach (var gpDto in graphPointsDto)
            {
                GraphPoint gp = _mapper.Map<GraphPoint>(gpDto.Value);
                gp.FloorId = floor.Id;

                if (await _graphPointRepository.FirstOrDefaultAsync(g => g.Id == gp.Id, cancellationToken) != null)
                    throw new AlreadyExistsException($"Graph point {gp.Id} already exists");

                graphPoints.Add(gp);
                graphIds.Add(gp.Id);

                if (gp.StairId is null) continue;

                var stairForUpdate = updatedStairs.Where(s => s.Id == gp.StairId).FirstOrDefault();

                if (stairForUpdate is null)
                {
                    Stair? stair = await _stairRepository.FirstOrDefaultAsync(z => z.Id == gp.StairId, cancellationToken);
                    if (stair is null)
                    {
                        var newStair = new Stair
                        {
                            Id = gp.StairId,
                            BuildingId = building.Id,
                            Links = [],
                        };
                        updatedStairs.Add(newStair);
                        await _stairRepository.AddAsync(newStair, cancellationToken);
                    }
                    else
                    {
                        updatedStairs.Add(stair);
                    }
                }
                else
                {
                    stairForUpdate.Links = stairForUpdate.Links == null ?[] : stairForUpdate.Links;
                    if (!stairForUpdate.Links.Any(x => x == gp.Id))
                    {
                        stairForUpdate.Links.Append(gp.Id);
                    }
                }
            }

            for (int i = 0; i < stairs.Count(); i++)
            {
                await _stairRepository.UpdateAsync(s => s.Id == stairs[i].Id, stairs[i], cancellationToken);
            }

            floor.GraphPoints = graphIds.ToArray();
            //floor.Rooms = floorDto.Rooms == null ? [] : floorDto.Rooms.Values.ToArray();

            await _floorRepository.AddAsync(floor, cancellationToken);
            await _graphPointRepository.AddRangeAsync(graphPoints.ToArray(), cancellationToken);

            await _stairRepository.SaveChanges();
        }

        public async Task<IReadOnlyList<Floor>> GetAllFloors(CancellationToken cancellationToken)
        {
            var res = await _floorRepository.ListAsync(cancellationToken);
            return res;
        }

        public async Task<IReadOnlyList<GraphPoint>> GetGraphPointsByFloor(string id, CancellationToken cancellationToken)
        {
            if (await _floorRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken) == null)
                throw new NotFoundException("Floor not found");

            var res = await _graphPointRepository.ListAsync(g => g.FloorId == id, cancellationToken);
            return res;
        }

        public async Task<IReadOnlyList<Stair>> GetStairsByFloor(string id, CancellationToken cancellationToken)
        {
            if (await _floorRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken) == null)
                throw new NotFoundException("Floor not found");

            var graphPoints = await _graphPointRepository.ListAsync(g => g.FloorId == id, cancellationToken);
            if (graphPoints == null) throw new NotFoundException("Graph points not found");

            var stairIds = new List<string>();
            var res = new List<Stair>();

            foreach (var graphPoint in graphPoints)
            {
                if (graphPoint.StairId != null) stairIds.Add(graphPoint.StairId);
            }

            foreach (var stairId in stairIds)
            {
                var stair = await _stairRepository.FirstAsync(s => s.Id == stairId, cancellationToken);
                if (stair == null) throw new NotFoundException($"Stair with id {stairId} is not found");
                res.Add(stair);
            }

            return res;
        }

        public async Task<Floor> GetFloorById(string id, CancellationToken cancellationToken)
        {
            var floor = await _floorRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            if (floor == null) throw new NotFoundException("Floor not found");

            return floor;
        }
    }
}
