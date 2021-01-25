using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<ActivityDto>> { }


        public class Handler : IRequestHandler<Query, List<ActivityDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;

            }

            public async Task<List<ActivityDto>> Handle(Query request,
                CancellationToken cancellationToken)
            {
                 var activities = await _context.Activities
               .ToListAsync();
                 //.Include(x => x.UserActivities)
                 //.ThenInclude(x => x.AppUser)
                 //.SingleOrDefaultAsync(x => x.Id == request.Id);
                    //.ToListAsync();
              //  var activities = await _context.Activities.FromSqlRaw<Activity>("uspLoadActivities").ToListAsync();
                // var activities = await _context.Activities.FromSqlRaw<Activity>("uspLoadActivitybyID {0}", "06d4ff44-5c4a-4ac1-abac-73db68185d31").ToListAsync();

                return _mapper.Map<List<Activity>, List<ActivityDto>>(activities);
            }
        }
    }
}