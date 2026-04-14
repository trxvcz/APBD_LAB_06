using LAB_06.DTOs;
using LAB_06.Models;
using Microsoft.AspNetCore.Mvc;

namespace LAB_06.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    public static List<Reservation> Reservations = new()
    {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Jan Kowalski",
            Topic = "Spotkanie Zarządu",
            Date = new DateTime(2024, 5, 20),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(10, 30),
            Status = Status.CANCELLED
        },
        new Reservation
        {
            Id = 2,
            RoomId = 1,
            OrganizerName = "Anna Nowak",
            Topic = "Rekrutacja Junior .NET",
            Date = new DateTime(2024, 5, 20),
            StartTime = new TimeOnly(11, 0),
            EndTime = new TimeOnly(12, 0),
            Status = Status.PLANNED
        },
        new Reservation
        {
            Id = 3,
            RoomId = 2,
            OrganizerName = "Marek Zegar",
            Topic = "Warsztaty Agile",
            Date = new DateTime(2024, 5, 21),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(15, 0),
            Status = Status.CONFIRMED
        },
        new Reservation
        {
            Id = 4,
            RoomId = 3,
            OrganizerName = "Katarzyna Kwiatkowska",
            Topic = "Daily Scrum",
            Date = new DateTime(2024, 5, 21),
            StartTime = new TimeOnly(8, 30),
            EndTime = new TimeOnly(9, 0),
            Status = Status.CONFIRMED
        },
        new Reservation
        {
            Id = 5, RoomId = 2, OrganizerName = "Piotr Wiśniewski", Topic = "Prezentacja Produktu",
            Date = new DateTime(2024, 5, 22), StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(14, 30),
            Status = Status.CANCELLED
        },
        new Reservation
        {
            Id = 6, RoomId = 1, OrganizerName = "Zofia Polska", Topic = "Burza Mózgów - Marketing",
            Date = new DateTime(2024, 5, 22), StartTime = new TimeOnly(15, 0), EndTime = new TimeOnly(16, 30),
            Status = Status.ARCHIVED
        }
    };

    [HttpGet]
    public IActionResult Get([FromQuery] DateTime? date, [FromQuery] Status? status, [FromQuery] int? roomId)
    {
        var query = Reservations.AsEnumerable();

        if (date.HasValue)
            query = query.Where(r => r.Date.Date == date.Value.Date);

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        if (roomId.HasValue)
            query = query.Where(r => r.RoomId == roomId.Value);

        return Ok(query.ToList());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var reservation = Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult Post([FromBody] CreateReservationDto reservationDto)
    {
        var newID = Reservations.Any() ? Reservations.Max(r => r.Id) + 1 : 1;
        var Rooms = RoomsController.Rooms;
        var existingRoom =  Rooms.FirstOrDefault(r => r.Id == reservationDto.RoomId);
        
        if (existingRoom == null) return NotFound();
        
        if (!existingRoom.IsActive) return BadRequest("Nie można zarezerwować nieaktywnego pokoju");

        bool hasConflict = Reservations.Any(r =>
            r.RoomId == reservationDto.RoomId &&
            r.Date.Date == reservationDto.Date.Date &&
            r.StartTime < reservationDto.EndTime &&
            r.EndTime > reservationDto.StartTime &&
            r.Status != Status.CANCELLED
        );

        if (hasConflict) return Conflict("Sala jest już zajęta w tym terminie.");

        var reservationModel = new Reservation
        {
            Id = newID,
            RoomId = reservationDto.RoomId,
            Date = reservationDto.Date,
            OrganizerName = reservationDto.OrganizerName,
            Topic = reservationDto.Topic,
            StartTime = reservationDto.StartTime,
            EndTime = reservationDto.EndTime,
            Status = Status.PLANNED
        };
        
        Reservations.Add(reservationModel);
        return CreatedAtAction(nameof(GetById), new { id = reservationModel.Id }, reservationModel);


    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] CreateReservationDto reservationDto)
    {
        var existingReservation = Reservations.FirstOrDefault(r => r.Id == id);

        if (existingReservation == null) return NotFound();
        
        existingReservation.RoomId = reservationDto.RoomId;
        existingReservation.Date = reservationDto.Date;
        existingReservation.Topic = reservationDto.Topic;
        existingReservation.OrganizerName = reservationDto.OrganizerName;
        existingReservation.StartTime = reservationDto.StartTime;
        existingReservation.EndTime = reservationDto.EndTime;
        existingReservation.Status = reservationDto.Status;
        
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var reservation = Reservations.FirstOrDefault(r => r.Id == id);
        
        if (reservation == null) return NotFound();
        
        var hasFutureReservations = ReservationsController.Reservations.Any(r => r.RoomId == id && r.Date >= DateTime.Now.Date);
        if (hasFutureReservations)
        {
            return Conflict("Nie można usunąć sali, ponieważ ma przypisane przyszłe rezerwacje.");
        }
        Reservations.Remove(reservation);
        return NoContent();
    }
    
}