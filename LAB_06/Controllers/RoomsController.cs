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
        new Room { Id = 1, Name = "Room 1" },
        new Room { Id = 2, Name = "Room 2" },
        new Room { Id = 3, Name = "Room 3" }
    };


    [HttpGet]
    public IActionResult Get(int id)
    {
        return Ok(Rooms.Where(r => r.Id > id));
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        var room = Rooms.FirstOrDefault(x => x.Id == id);

        if (room == null) return NotFound();

        return Ok(room);
    }

    [HttpGet]
    [Route("{id}/building")]
    public IActionResult GetByBuilding(string? buildingCode)
    {
        if (buildingCode == null)
        {
            var rooms = Rooms;
            return Ok(rooms);
        }
        else
        {
            var rooms = Rooms.Where(r => r.BuildingCode == buildingCode).ToList();
            return Ok(rooms);
        }
    }


    [HttpPost]
    public IActionResult Post([FromBody] CreateRoomDto room)
    {
        var roomModel = new Room
        {
            Name = room.Name,
            Capacity = room.Capacity,
            Floor = room.Floor,
            HasProjector = room.HasProjector,
            IsActive = room.IsActive
        };

        Rooms.Add(roomModel);

        return Created();
    }
}