using LAB_06.Models;

namespace LAB_06.DTOs;

public class CreateReservationDto
{
    public DateTime Date { get; set; }
    public int RoomId { get; set; }
    public string OrganizerName { get; set; }
    public string Topic { get; set; }
    public Status Status { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}