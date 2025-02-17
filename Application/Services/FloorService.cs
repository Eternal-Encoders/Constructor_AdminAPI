using AutoMapper;
using ConstructorAdminAPI.Core.Repositories;
using ConstructorAdminAPI.Models.DTOs;
using ConstructorAdminAPI.Models.Entities;
using MongoDB.Bson;

namespace ConstructorAdminAPI.Application.Services
{
    public class FloorService
    {
        IFloorRepository _floorRepository;
        IGraphPointRepository _graphPointRepository;
        IStairRepository _stairRepository;
        IMapper _mapper;

        public FloorService(IFloorRepository floorRepository, IGraphPointRepository graphPointRepository,
            IStairRepository stairRepository, IMapper mapper)
        {
            _floorRepository = floorRepository;
            _graphPointRepository = graphPointRepository;
            _stairRepository = stairRepository;
            _mapper = mapper;
        }

        public async Task<Result.Result> InsertFloor(CreateFloorDto floorDto, CancellationToken cancellationToken)
        {
            //Result.Result result = new();
            if (await _floorRepository.FirstOrDefaultAsync(f =>
                f.Building == floorDto.Building && f.FloorNumber == floorDto.FloorNumber, cancellationToken) != null)
                return Result.Result.Error(new Result.Error("Floor already exists", 400));

            //if (await _buildingRepository.FirstOrDefaultAsync(b =>
            //   b.Name == floorDto.Building, cancellationToken) == null)
            //    return Result.Result.Error(new Result.Error("Building doesn`t exist", 404));

            var graphPoints = floorDto.GraphPoints == null ? [] : floorDto.GraphPoints;
            List<string> graphIds = [];
            var stairs = await _stairRepository.ListAsync(s => s.Building == floorDto.Building, cancellationToken);
            List<Stair> updatedStairs = [];

            foreach (var gp in graphPoints)
            {
                if (await _graphPointRepository.FirstOrDefaultAsync(g => g.Id == gp.Value.Id, cancellationToken) != null)
                    return Result.Result.Error(new Result.Error($"Graph point {gp.Value.Id} already exists", 400));

                graphIds.Add(gp.Value.Id);

                if (gp.Value.StairId is null) continue;

                var stairForUpdate = updatedStairs.Where(s => s.Id == gp.Value.StairId).FirstOrDefault();

                if (stairForUpdate is null)
                {
                    Stair? stair = await _stairRepository.FirstOrDefaultAsync(z => z.Id == gp.Value.StairId, cancellationToken);
                    if (stair is null)
                    {
                        var newStair = new Stair
                        {
                            Id = gp.Value.StairId,
                            Building = floorDto.Building,
                            StairPoint = gp.Value.StairId,
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
                    if (!stairForUpdate.Links.Any(x => x == gp.Value.Id))
                    {
                        stairForUpdate.Links.Append(gp.Value.Id);
                    }
                }
            }

            for (int i = 0; i < stairs.Count; i++)
            {
                await _stairRepository.UpdateAsync(stairs[i].Id, stairs[i], cancellationToken);
            }

            Floor floor = _mapper.Map<Floor>(floorDto);
            floor.Id = ObjectId.GenerateNewId().ToString();
            floor.GraphPoints = graphIds.ToArray();
            floor.Rooms = floorDto.Rooms == null ? [] : floorDto.Rooms.Values.ToArray();

            await _floorRepository.AddAsync(floor, cancellationToken);
            await _graphPointRepository.AddRangeAsync(graphPoints.Values.ToArray(), cancellationToken);

            await _stairRepository.SaveChanges();

            return Result.Result.Success();
        }

        public async Task<Result.Result<IReadOnlyList<Floor>>> GetAllFloors(CancellationToken cancellationToken)
        {
            var res = await _floorRepository.ListAsync(cancellationToken);
            return Result.Result<IReadOnlyList<Floor>>.Success(res);
        }

        public async Task<Result.Result<IReadOnlyList<Floor>>> GetFloorsByBuilding(string building, CancellationToken cancellationToken)
        {
            var res = await _floorRepository.ListAsync(f => f.Building == building, cancellationToken);
            return Result.Result<IReadOnlyList<Floor>>.Success(res);
        }

        public async Task<Result.Result<Floor>> GetFloorByBuildingAndNumber(string building, int number, CancellationToken cancellationToken)
        {
            var res = await _floorRepository.FirstOrDefaultAsync(f => f.Building == building &&
                f.FloorNumber == number, cancellationToken);
            if (res == null) return Result.Result<Floor>.Error(new Result.Error("Floor not found", 404));
            return Result.Result<Floor>.Success(res);
        }

        public async Task<Result.Result<Floor>> GetFloorById(string id, CancellationToken cancellationToken)
        {
            var floor = await _floorRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            if (floor == null) return Result.Result<Floor>.Error(new Result.Error("Floor not found", 404));

            return Result.Result<Floor>.Success(floor);
        }
    }
}
