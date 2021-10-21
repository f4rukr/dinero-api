using AutoMapper;
using Klika.Dinero.Database.DbContexts;
using Klika.Dinero.Model.DTO.Bank.Response;
using Klika.Dinero.Model.Entities;
using Klika.Dinero.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Klika.Dinero.Service.Banks
{
    public class BankService : IBankService
    {
        private readonly DineroDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<BankService> _logger;

        public BankService(
            DineroDbContext dbContext,
            IMapper mapper,
            ILogger<BankService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BankListResponse> GetBanksAsync()
        {
            try
            {
                BankListResponse response = new BankListResponse();

                List<Bank> banks = await _dbContext.Banks.AsNoTracking()
                                                         .ToListAsync()
                                                         .ConfigureAwait(false);

                response.Banks = _mapper.Map<List<Bank>, List<BankResponseDTO>>(banks);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetBanksAsync));
                throw;
            }
        }
    }
}
