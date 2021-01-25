using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<ActivityDto>
        {
            public Guid Id { get; set; }

        }
        public class Handler : IRequestHandler<Query, ActivityDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }
            //public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                //var activity = await _context.Activities.FindAsync(request.Id)
                // var activity = await _context.Activities.FromSqlRaw<Activity>("uspLoadActivitybyID {0}", request.Id)
                var activity = await _context.Activities
                    .FindAsync(request.Id);

                // if (activity.Count == 0)
                if (activity == null)
                    throw new RestException(HttpStatusCode.NotFound, new { activity = "Not Found" });

                var activityToReturn = _mapper.Map<Activity, ActivityDto>(activity);

                return activityToReturn;


            }
        }
    }
}