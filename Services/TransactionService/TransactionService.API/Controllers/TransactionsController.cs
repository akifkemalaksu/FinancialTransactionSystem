using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Dtos.TransactionDtos;
using TransactionService.Application.Services.DataAccessors;
using TransactionService.Domain.Entities;
using ServiceDefaults.Controllers;

namespace TransactionService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ApiControllerBase
    {
    }
}
