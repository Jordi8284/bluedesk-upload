using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BluedeskUpload.Models
{
    public class Upload
    {
        [Key]
        public int UploadId { get; set; }
        public ApplicationUser Gebruiker { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }
        public string Bestand { get; set; }
        public string Omschrijving { get; set; }
        public string Bedrijfsnaam { get; set; }
        public string Naam { get; set; }
        public string Email { get; set; }
        public string Telefoon { get; set; }
    }
}