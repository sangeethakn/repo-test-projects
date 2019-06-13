using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestXMLPost.Models;

namespace TestXMLPost.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {

        /// <summary>
        /// StudentDetails
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Produces("application/xml")]
        [ProducesResponseType(typeof(Student), (int)HttpStatusCode.OK)]
        [HttpPost("StudentDetails", Name = "StudentDetails")]
        public IActionResult StudentDetails([FromBody] StudentRegister request)
        {
            var resp = new Student
            {
                StudentId = 1,
                StudentName = "Abcd"
            };
            return Ok(resp);
        }
        /// <summary>
        /// Get StudentDetails
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Produces("application/xml")]
        [HttpGet("GetStudentDetails", Name = "GetStudentDetails")]
        public async Task<IActionResult> GetStudentDetails()
        {
            var StudentRegister = new StudentRegister
            {
                Age=123,
                Place="qwertyuiop",
                StudentName="abc"
            };
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://localhost:44352/api/Test/StudentDetails";
                    StringContent stringContent = new StringContent(GetXMLFromObject(StudentRegister), Encoding.UTF8, "application/xml");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    var response = await client.PostAsync(url, stringContent);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    StudentRegister = JsonConvert.DeserializeObject<StudentRegister>(responseBody);
                }
            }
            catch (Exception ex)
            {
            }
            
            return Ok(StudentRegister);
        }
        public static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }

    }
}