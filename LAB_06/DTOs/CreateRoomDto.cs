using System.ComponentModel.DataAnnotations;

namespace LAB_06.DTOs;

public class CreateRoomDto
{
    [StringLength(100)][Required(ErrorMessage = "Room name is required.")]
    public string Name { get; set; }
    
    [Range(0, int.MaxValue,ErrorMessage = "Please enter valid integer Number" )]
    public int Floor { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
    public int Capacity { get; set; }
    public bool HasProjector { get; set; }
    public bool IsActive { get; set; }
    
    [Required(ErrorMessage = "Building code is required.")]
    [RegularExpression("^(a|b|c|A|B|C)$")]
    public string BuildingCode { get; set; }
}