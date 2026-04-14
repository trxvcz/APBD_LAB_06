using System.ComponentModel.DataAnnotations;
using LAB_06.Models;

namespace LAB_06.DTOs;

public class CreateReservationDto:IValidatableObject
{
    public DateTime Date { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
    public int RoomId { get; set; }
    
    [Required(ErrorMessage = "Organizer name is required")]
    [StringLength(100,ErrorMessage = "Organizer name cannot exceed 100 characters")]
    public string OrganizerName { get; set; }
    [StringLength(100)]
    [Required(ErrorMessage = "Topic is required")]
    public string Topic { get; set; }
    
    public Status Status { get; set; }
    
    public TimeOnly StartTime { get; set; }
    
    public TimeOnly EndTime { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "Czas zakończenia (EndTime) musi być późniejszy niż czas rozpoczęcia (StartTime).", 
                new[] { nameof(EndTime) }
            );
        }

        if (Date.Date < DateTime.Now.Date)
        {
            yield return new ValidationResult(
                "Data rezerwacji nie może być z przeszlości.",
                new[] { nameof(Date) }
            );
        }
        
        var duration = EndTime - StartTime;
        if (duration.TotalMinutes < 15 && EndTime > StartTime)
        {
            yield return new ValidationResult(
                "Rezerwacja musi trwać minimum 15 minut.",
                new[] { nameof(EndTime) }
            );
        }
    }
}