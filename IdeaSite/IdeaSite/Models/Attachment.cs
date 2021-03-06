﻿using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.IO;

namespace IdeaSite.Models
{
    public class Attachment : IEnumerable
    {
        [Key]
        [Required]
        public int ID { get; set; }

        public int ideaID { get; set; }

        public string name { get; set; }

        public DateTime cre_date { get; set; }

        [Required]
        [StringLength(255)]
        public string storageLocation { get; set; }

        public string deleteObj { get; set; }

        public void DeleteFile()
        {
            if (Directory.Exists(storageLocation))
            {
                var file = String.Format("{0}\\{1}", storageLocation, name);

                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        public void DeleteDirectory()
        {
            if (Directory.Exists(storageLocation))
            {
                if (!Directory.EnumerateFiles(storageLocation).Any())
                {
                    Directory.Delete(storageLocation, true);
                }
            }
        }


        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)storageLocation).GetEnumerator();
        }
    }
}