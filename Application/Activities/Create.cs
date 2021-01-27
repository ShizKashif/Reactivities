using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistence;
using FluentValidation;
using Application.Interfaces;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }
            public string Category { get; set; }

            public DateTime Date { get; set; }
            public string City { get; set; }

            public string Venue { get; set; }

            public int ActivityID { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Category).NotEmpty();
                RuleFor(x => x.Date).NotEmpty();
                RuleFor(x => x.City).NotEmpty();
                RuleFor(x => x.Venue).NotEmpty();

            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // Linq code 
                // _context.Activities.Add(activity);
                // var success = await _context.SaveChangesAsync( ) > 0;


                // declaring sql parameter to give sql data types to our attributes 
                SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@pID", request.Id),
                    new SqlParameter("@pTitle", request.Title),
                    new SqlParameter("@pDescription", request.Description),
                    new SqlParameter("@pCategory", request.Category),
                    new SqlParameter("@pDate", request.Date),
                    new SqlParameter("@pCity", request.City),
                    new SqlParameter("@pVenue", request.Venue),
                    new SqlParameter("@pActivityID",System.Data.SqlDbType.Int){Direction = System.Data.ParameterDirection.Output},
                    new SqlParameter("@puserName", _userAccessor.GetCurrentUsername()),
                    new SqlParameter("@pIsHost", true),
                    new SqlParameter("@pDateJoined", DateTime.Now)                   
                    
                };

                var rtnStatus = await _context.Database.ExecuteSqlRawAsync("uspCreateActivity  {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7} OUT, {8}, {9}, {10}", param);
                // Console.WriteLine(rtnStatus);
                // Console.WriteLine(param[7].Value);

                if (rtnStatus != 0) return Unit.Value;

                throw new Exception("Problem saving changes");

            }
        }
    }
}