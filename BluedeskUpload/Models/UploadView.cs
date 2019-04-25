using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace BluedeskUpload.Models
{
    public class UploadView
    {
        [Key]
        public int UploadId { get; set; }
        public ApplicationUser Gebruiker { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }
        [DisplayName("File")]
        [DataType(DataType.Upload)]
        public string Bestand { get; set; }
        [DisplayName("Description")]
        public string Omschrijving { get; set; }
        [DisplayName("Company name")]
        public string Bedrijfsnaam { get; set; }
        [DisplayName("Name")]
        public string Naam { get; set; }
        public string Email { get; set; }
        [DisplayName("Telephone")]
        public string Telefoon { get; set; }
    }
}