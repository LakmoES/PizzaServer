using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class ExceptionDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ExceptionMessage { get; set; }    // сообщение об исключении

        [Required]
        public string ControllerName { get; set; }  // контроллер, где возникло исключение

        [Required]
        public string ActionName { get; set; }  // действие, где возникло исключение

        [Required]
        public string StackTrace { get; set; }  // стек исключения

        [Required]
        public DateTime Date { get; set; }  // дата и время исключения
    }
}