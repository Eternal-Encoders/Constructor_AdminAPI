using Constructor_API.Models.Entities;

namespace Constructor_API.Models.Objects
{
    public class PathNode
    {
        public GraphPoint GraphPoint { get; set; }
        public double PathLengthFromStart { get; set; }
        public PathNode? CameFrom { get; set; }
        public double HeuristicPathLength { get; set; }
        public double EstimateFullPathLength
        {
            get
            {
                return PathLengthFromStart + HeuristicPathLength;
            }
        }
    }
}
