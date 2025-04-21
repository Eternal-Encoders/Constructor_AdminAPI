using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Helpers.Attributes
{
    public class PointTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is CreateGraphPointFromFloorDto pointDto1)
                //if (value is GraphPoint pointDto)
                {
                bool containsTransition = pointDto1.Types.Contains("stair") ||
                    pointDto1.Types.Contains("elevator") || pointDto1.Types.Contains("escalator");
                if (pointDto1.TransitionId == null && containsTransition)
                {
                    //throw new ValidationException("Graph point has type \"stair\" but stair id is not specified");
                    ErrorMessage = "Graph point has type of floor connection category but connection id is not specified";
                    return false;
                }

                if (pointDto1.TransitionId != null && !containsTransition)
                {
                    //throw new ValidationException("Graph point has stair id but type \"stair\" is not specified");
                    ErrorMessage = "Graph point has connection id but type of floor connection is not specified";
                    return false;
                }

                //if (pointDto.Room == null && !pointDto.Types.Contains("corridor"))
                //{
                //    throw new ValidationException("Graph point has no room info but type \"corridor\" is not specified");
                //}

                //if (pointDto.Room != null && pointDto.Types.Contains("corridor"))
                //{
                //    throw new ValidationException("Graph point has room info but type \"corridor\" is specified");
                //}
            }

            if (value is CreateGraphPointDto pointDto2)
            //if (value is GraphPoint pointDto)
            {
                bool containsTransition = pointDto2.Types.Contains("stair") ||
                    pointDto2.Types.Contains("elevator") || pointDto2.Types.Contains("escalator");
                if (pointDto2.TransitionId == null && containsTransition)
                {
                    //throw new ValidationException("Graph point has type \"stair\" but stair id is not specified");
                    ErrorMessage = "Graph point has type of floor connection category but connection id is not specified";
                    return false;
                }

                if (pointDto2.TransitionId != null && !containsTransition)
                {
                    //throw new ValidationException("Graph point has stair id but type \"stair\" is not specified");
                    ErrorMessage = "Graph point has connection id but type of floor connection is not specified";
                    return false;
                }
            }

            return true;
        }
    }
}
