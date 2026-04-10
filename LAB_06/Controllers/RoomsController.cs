using LAB_06.DTOs;
using LAB_06.Models;
using Microsoft.AspNetCore.Mvc;

namespace LAB_06.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    public static List<Room> Rooms = new()
    {
        new Room { Id = 1, Name = "Room 1", Capacity = 10, BuildingCode = "A1", HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Room 2", Capacity = 25, BuildingCode = "B2", HasProjector = false, IsActive = true },
        new Room { Id = 3, Name = "Room 3", Capacity = 50, BuildingCode = "A1", HasProjector = true, IsActive = false }
    };


    [HttpGet]
    public IActionResult Get([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
        var query = Rooms.AsEnumerable();

        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            query = query.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly.HasValue && activeOnly.Value)
            query = query.Where(r => r.IsActive);

        return Ok(query.ToList());
    }
    
    
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var room = Rooms.FirstOrDefault(x => x.Id == id);
        if (room == null) return NotFound();
        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode)
    {
        var rooms = Rooms.Where(r => r.BuildingCode == buildingCode).ToList();
        return Ok(rooms);
    }


    [HttpPost]
    public IActionResult Post([FromBody] CreateRoomDto roomDto)
    {
        var newId = Rooms.Any() ? Rooms.Max(r => r.Id) + 1 : 1;
        
        var roomModel = new Room
        {
            Id = newId,
            Name = roomDto.Name,
            Capacity = roomDto.Capacity,
            Floor = roomDto.Floor,
            HasProjector = roomDto.HasProjector,
            IsActive = roomDto.IsActive,
            BuildingCode = roomDto.BuildingCode
        };

        Rooms.Add(roomModel);
        return CreatedAtAction(nameof(GetById), new { id = roomModel.Id }, roomModel);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] CreateRoomDto roomDto)
    {
        var existingRoom = Rooms.FirstOrDefault(r => r.Id == id);
        if (existingRoom == null) return NotFound();
        
        existingRoom.Name = roomDto.Name;
        existingRoom.Capacity = roomDto.Capacity;
        existingRoom.Floor = roomDto.Floor;
        existingRoom.HasProjector = roomDto.HasProjector;
        existingRoom.IsActive = roomDto.IsActive;
        existingRoom.BuildingCode = roomDto.BuildingCode;

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var room = Rooms.FirstOrDefault( room => room.Id == id );
        if (room == null) return NotFound();
        
        Rooms.Remove(room);
        return NoContent();
    }
}