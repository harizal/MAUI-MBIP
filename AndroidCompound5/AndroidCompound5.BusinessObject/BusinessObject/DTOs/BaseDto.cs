using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AndroidCompound5.BusinessObject.BusinessObject.DTOs
{
	public class BaseDto
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // auto-increment
		public int Id { get; set; }
	}
}
