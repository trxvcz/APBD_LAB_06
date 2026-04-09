using System.ComponentModel.DataAnnotations;

namespace LAB_06.DTOs;

public class CreateRoomDto
{
    [StringLength(100)] public string Name { get; set; }

    public int Floor { get; set; }
    public int Capacity { get; set; }
    public bool HasProjector { get; set; }
    public bool IsActive { get; set; }
}