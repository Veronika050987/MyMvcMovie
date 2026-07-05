using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace MyMvcMovie.Models
{
	public class Movie
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Название фильма обязательно!")]
		[StringLength(70)]
		[Display(Name = "Movie title")]
		//[RegularExpression(@"^[A-ZА-Я]+[a-zа-я]*$", ErrorMessage = "Строка содержит недопустимые символы")]
		public string Title { get; set; }
		[DataType(DataType.Date)]
		[Display(Name = "Release date")]
		public DateTime ReleaseDate { get; set; }
		[RegularExpression(@"^[A-ZА-Я]+[a-zа-я]*$", ErrorMessage = "Строка содержит недопустимые символы")]
		[Required]
		[StringLength(30)]
		public string? Genre { get; set; }
		public string? URL { get; set; }
		public string? Brief { get; set; }
		public string? Poster { get; set; }

		public byte[]? photo { get; set; }
		[Range(1, 100)]
		[DataType(DataType.Currency)]
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; }
		[Range(1, 100)]
		public string? Rating { get; set; }
	}
}
