using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BluedeskUpload.Models
{
    public class Upload
    {
        [Key]
        public int UploadId { get; set; }
        public ApplicationUser Gebruiker { get; set; }
        public IEnumerable<SelectListItem> Gebruikers;
        [DisplayName("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
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
        [DisplayName("Phone number")]
        public string Telefoon { get; set; }
    }
}

