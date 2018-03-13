using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CreateApplicant.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CreateApplicant.Controllers
{
    [Route("api/CreateApplicant")]
    public class CreateApplicantController : Controller
    {
        private DatabaseContext _context;

        public CreateApplicantController(DatabaseContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            string strCreateApplicant = "CreateApplicant";

            return strCreateApplicant;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public string CreateNewApplicant([FromBody]Applicant applicant)
        {
            string strReturn = string.Empty;
            PostToDB(applicant);

            // Uncomment the next line to save a copy of the image to the azure filesystem
            //SaveImage(strBase64, strFilename);

            strReturn = applicant.appFilename;
            return strReturn;
        }

        private void PostToDB(Applicant applicant)
        {
            using (_context)
            {

                Applicant newApplicant = new Applicant
                {
                    appFirstName = applicant.appFirstName,
                    appLastName = applicant.appLastName,
                    appFilename = applicant.appFilename,
                    appUniqueCode = applicant.appUniqueCode,
                    appBirthdate = applicant.appBirthdate,
                    appEmail = applicant.appEmail,
                    appImage = applicant.appImage
                };
                _context.Applicants.Add(newApplicant);
                _context.SaveChanges();
            }
        }


        private bool SaveImage(string strImage, string strFileName)
        {
            String path = @"C:\ImageStorage\"; //Path

            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            //  Strip off image type
            int intComma = strImage.IndexOf(',') + 1;
            int intImageLength = strImage.Length - intComma;
            strImage = strImage.Substring(intComma, intImageLength);


            string strImagePath = Path.Combine(path, strFileName);
            byte[] bytes = Convert.FromBase64String(strImage);

            System.IO.File.WriteAllBytes(strImagePath, bytes);

            return true;
        }
    }

}
