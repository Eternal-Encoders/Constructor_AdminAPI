using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Helpers.Attributes
{
    public class ObjectIdAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string id)
            {
                if (!ObjectId.TryParse(id, out _))
                {
                    //throw new ValidationException("Wrong input: specified ID is not a valid 24 digit hex string");
                    ErrorMessage = "Wrong input: specified ID is not a valid 24 digit hex string";
                    return false;
                }
            }
            else if (value is string[] ids)
            {
                foreach (var i in ids)
                {
                    if (!ObjectId.TryParse(i, out _))
                    {
                        ErrorMessage = "Wrong input: specified ID is not a valid 24 digit hex string";
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
