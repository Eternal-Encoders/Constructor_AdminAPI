using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Helpers.Attributes
{
    public class MinMaxFloorValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is Building buildingDto)
            {
                if (buildingDto.MinFloor > buildingDto.MaxFloor)
                {
                    //throw new ValidationException("Wrong specification of max and min floors");
                    ErrorMessage = "Wrong specification of max and min floors";
                    return false;
                }
            }
            else if (value is CreateBuildingDto building)
            {
                if (building.MinFloor > building.MaxFloor)
                {
                    //throw new ValidationException("Wrong specification of max and min floors");
                    ErrorMessage = "Wrong specification of max and min floors";
                    return false;
                }
            }

            return true;
        }
    }
}
