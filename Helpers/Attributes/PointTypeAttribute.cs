using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Helpers.Attributes
{
    public class PointTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is GraphPointFromFloorDto pointDto)
                //if (value is GraphPoint pointDto)
                {
                if (pointDto.StairId == null && pointDto.Types.Contains("stair"))
                {
                    //throw new ValidationException("Graph point has type \"stair\" but stair id is not specified");
                    ErrorMessage = "Graph point has type \"stair\" but stair id is not specified";
                    return false;
                }

                if (pointDto.StairId != null && !pointDto.Types.Contains("stair"))
                {
                    //throw new ValidationException("Graph point has stair id but type \"stair\" is not specified");
                    ErrorMessage = "Graph point has stair id but type \"stair\" is not specified";
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

            return true;
        }
    }
}
