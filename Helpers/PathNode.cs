using Constructor_API.Models.Entities;
using Constructor_API.Models.InnerObjects;

namespace Constructor_API.Helpers
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
