﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Common;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSessions]
    public class TaskStatusController : ApiController
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IHttpTaskFetcher _taskFetcher;

        public TaskStatusController(ICommonRepository commonRepository, IHttpTaskFetcher taskFetcher)
        {
            _commonRepository = commonRepository;
            _taskFetcher = taskFetcher;
        }

        public Status Get(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);

            return new Status
                       {
                           Name = task.Status.Name,
                           Ordinal = task.Status.Ordinal,
                           StatusId = task.Status.StatusId
                       };
        }

        public void Put(long taskId, long statusId)
        {
            var task = _taskFetcher.GetTask(taskId);

            var status = _commonRepository.Get<Data.Model.Status>(statusId);
            if (status == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            ReasonPhrase = string.Format("Status {0} not found", statusId)
                        });
            }

            task.Status = status;

            _commonRepository.Save(task);
        }
    }
}