using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BluedeskUpload.Models
{
    public class Download
    {
        [Key]
        public int DownloadId { get; set; }
        public ApplicationUser Gebruiker { get; set; }
        public int UploadId { get; set; }
        public Upload Upload { get; set; }
    }
}