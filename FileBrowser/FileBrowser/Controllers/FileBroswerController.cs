using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Web.Http;
using System.Web.Mvc;
using FileBrowser.Models;
using Newtonsoft.Json;

namespace FileBrowser.Controllers
{
    public class FileBroswerController : ApiController
    {
        // GET api/FileBroswer
        public CurrentDirectory Get()
        {
            FileBrowserManager manager = new FileBrowserManager(string.Empty);
            return manager.GetInformationAboutDirectory(false);
        }

        // GET api/FileBroswer/5
        public CurrentDirectory Get(string id,bool isRecursion)
        {
            FileBrowserManager manager = new FileBrowserManager(id);
            return manager.GetInformationAboutDirectory(isRecursion);
        }

        // POST api/FileBroswer
        public void Post([FromBody]string value)
        {
        }

        // PUT api/FileBroswer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/FileBroswer/5
        public void Delete(int id)
        {
        }
    }
}
